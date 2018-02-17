#region

using common.models;
using gameserver.networking.messages.handlers.hack;
using System.Collections.Generic;

#endregion

namespace gameserver.realm.entity
{
    public class Projectile : Entity
    {
        public readonly HashSet<Entity> hitted = new HashSet<Entity>();

        public Projectile(ProjectileDesc desc)
            : base(Program.Manager.GameData.IdToObjectType[desc.ObjectId])
        {
            ProjDesc = desc;
            cheatHandler = new GodCheatHandler();
            cheatHandler.SetProjectile(this);
        }

        private GodCheatHandler cheatHandler { get; set; }

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
            /*if (ProjectileOwner is Enemy)
                cheatHandler.Handler();*/
            base.Tick(time);
        }

        public bool isValidType(Entity entity) =>
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
                return;

            Move(entity.X, entity.Y);

            if (entity.HitByProjectile(this, time))
            {
                if (isValidType(entity))
                    hitted.Add(entity);
                else
                    Destroy();

                ProjectileOwner.ProjectileHit(this, entity);
                UpdateCount++;
            }
        }
    }
}