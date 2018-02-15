#region

using System;
using System.Collections.Generic;

#endregion

namespace gameserver.realm.entity
{
    public interface IProjectileOwner
    {
        Projectile[] Projectiles { get; }
        Entity Self { get; }
    }

    public class Projectile : Entity
    {
        public readonly HashSet<Entity> hitted = new HashSet<Entity>();

        public Projectile(ProjectileDesc desc)
            : base(Program.Manager.GameData.IdToObjectType[desc.ObjectId])
        {
            ProjDesc = desc;
        }

        public IProjectileOwner ProjectileOwner { get; set; }
        public new byte ProjectileId { get; set; }
        public short Container { get; set; }
        public int Damage { get; set; }
        public long BeginTime { get; set; }
        public Position BeginPos { get; set; }
        public float Angle { get; set; }
        public ProjectileDesc ProjDesc { get; set; }

        public void Destroy() => Owner?.LeaveWorld(this);
        
        public override void Tick(RealmTime time) => base.Tick(time);

        public void ForceHit(Entity entity, RealmTime time)
        {
            Move(entity.X, entity.Y);

            if (entity.HitByProjectile(this, time))
            {
                if ((entity is Enemy && ProjDesc.MultiHit) || (entity is GameObject && (entity as GameObject).Static && !(entity is Wall) && ProjDesc.PassesCover))
                    hitted.Add(entity);
                else
                    Destroy();

                ProjectileOwner.Self.ProjectileHit(this, entity);
            }

            UpdateCount++;
        }
    }
}