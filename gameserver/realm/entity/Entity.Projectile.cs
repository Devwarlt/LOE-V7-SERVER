#region

using common.models;
using gameserver.networking.messages.handlers.hack;
using System.Collections.Concurrent;

#endregion

namespace gameserver.realm.entity
{
    public class Projectile : Entity
    {
        public Projectile(ProjectileDesc desc)
            : base(Program.Manager.GameData.IdToObjectType[desc.ObjectId])
        {
            ProjDesc = desc;
            CheatHandler = new GodCheatHandler();
            CheatHandler.SetProjectile(this);
        }

        public static ConcurrentDictionary<int, bool> ProjectileCache = new ConcurrentDictionary<int, bool>();

        public static void Add(int id) => ProjectileCache.TryAdd(id, false);

        public static void Remove(int id) => ProjectileCache.TryRemove(id, out bool val);

        private GodCheatHandler CheatHandler { get; set; }

        public Entity ProjectileOwner { get; set; }
        public new byte ProjectileId { get; set; }
        public short Container { get; set; }
        public int Damage { get; set; }
        public long BeginTime { get; set; }
        public Position BeginPos { get; set; }
        public float Angle { get; set; }
        public ProjectileDesc ProjDesc { get; set; }

        public void Destroy() => Owner.LeaveWorld(this);

        public override void Tick(RealmTime time)
        {
            if (ProjectileOwner is Enemy)
                CheatHandler.Handler();

            base.Tick(time);
        }

        public bool IsValidType(Entity entity) =>
            (entity is Enemy
            && ProjDesc.MultiHit)
            || (entity is GameObject
            && (entity as GameObject).Static
            && !(entity is Wall)
            && ProjDesc.PassesCover);

        public void ForceHit(Entity entity, RealmTime time, bool killed)
        {
            Log.Warn("New projectile collision!");

            if (entity == null)
            {
                Log.Warn("Entity is null.");
                return;
            }

            if (!Owner.Entities.ContainsKey(entity.Id))
            {
                Log.Warn($"World '{Owner.Name}' does not contains entity '{entity.Name}'.");
                return;
            }

            if (!ProjectileCache.ContainsKey(ProjectileId))
            {
                Log.Warn($"Adding projectile ID '{ProjectileId}' to cache.");
                Add(ProjectileId);
            }
            else
            {
                Log.Warn($"Projectile ID '{ProjectileId}' already contains in cache.");
                return;
            }

            Move(entity.X, entity.Y);

            if (entity.HitByProjectile(this, time))
                if (IsValidType(entity))
                {
                    Log.Warn($"Removing projectile ID '{ProjectileId}' from cache.");
                    Remove(ProjectileId);
                }
                else
                {
                    Log.Warn($"Destroying projectile and removing projectile ID '{ProjectileId}' from cache.");
                    Remove(ProjectileId);
                    Destroy();
                }

            UpdateCount++;
        }
    }
}