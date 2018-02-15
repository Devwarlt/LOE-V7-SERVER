#region

using gameserver.realm;
using gameserver.realm.entity;
using System.Collections.Generic;

#endregion

namespace gameserver.networking.messages.handlers.hack
{
    public class GodHackModHandler
    {
        protected CollisionMap<Entity> collisionMap { get; set; }
        protected Projectile projectile { get; set; }
        protected ProjectileDesc projectileDescription { get; set; }
        protected World Owner { get; set; }

        public GodHackModHandler(
            Projectile _projectile,
            CollisionMap<Entity> _collisionMap,
            ProjectileDesc _projectileDescription
            )
        {
            projectile = _projectile;
            collisionMap = _collisionMap;
            projectileDescription = _projectileDescription;
            Owner = projectile.Owner;
        }

        public CollisionMap<Entity> CollisionMap
        { get { return collisionMap; } }

        private IProjectileOwner ProjectileOwner
        { get { return projectile.ProjectileOwner; } }

        private HashSet<Entity> Hitted
        { get { return projectile.hitted; } }

        private void Move(Position position) => projectile.Move(position.X, position.Y);

        private void Destroy() => projectile.Destroy();

        public bool TickCore(long elapsedTick, RealmTime time)
        {
            Position _position = projectile.GetPosition(elapsedTick);

            Move(_position);

            if (_position.X < 0 || _position.X > Owner.Map.Width)
            {
                Destroy();
                return false;
            }
            else if (_position.Y < 0 || _position.Y > Owner.Map.Height)
            {
                Destroy();
                return false;
            }
            else if (Owner.Map[(int)_position.X, (int)_position.Y].TileId == 0xff)
            {
                Destroy();
                return false;
            }
            else
            {
                ushort objectType = Owner.Map[(int)_position.X, (int)_position.Y].ObjType;

                if (objectType != 0 && Program.Manager.GameData.ObjectDescs[objectType].OccupySquare && !projectileDescription.PassesCover)
                {
                    Destroy();
                    return false;
                }

                double nearestBy = double.MaxValue;
                Entity entity = null;

                foreach (var i in collisionMap.HitTest(_position.X, _position.Y, 2))
                {
                    if (i == ProjectileOwner.Self)
                        continue;

                    if (i is Container)
                        continue;

                    if (Hitted.Contains(i))
                        continue;

                    double xSide = (i.X - _position.X) * (i.X - _position.X);
                    double ySide = (i.Y - _position.Y) * (i.Y - _position.Y);

                    if (xSide <= 0.5 * 0.5 && ySide <= 0.5 * 0.5 && xSide + ySide <= nearestBy)
                    {
                        nearestBy = xSide + ySide;
                        entity = i;
                    }
                }

                if (entity != null && entity.HitByProjectile(projectile, time))
                {
                    if ((entity is Enemy && projectileDescription.MultiHit) || (entity is GameObject && (entity as GameObject).Static && !(entity is Wall) && projectileDescription.PassesCover))
                        Hitted.Add(entity);
                    else
                    {
                        Destroy();
                        return false;
                    }

                    ProjectileOwner.Self.ProjectileHit(projectile, entity);
                }
            }
            return true;
        }
    }
}
