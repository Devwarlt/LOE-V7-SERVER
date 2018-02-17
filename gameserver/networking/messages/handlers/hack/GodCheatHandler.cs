#region

using gameserver.realm;
using gameserver.realm.entity;
using gameserver.realm.entity.player;
using System;

#endregion

namespace gameserver.networking.messages.handlers.hack
{
    public class GodCheatHandler : ICheatHandler
    {
        protected Projectile projectile { get; set; }

        public GodCheatHandler() { }

        CheatID ICheatHandler.ID
        { get { return CheatID.GOD; } }

        public void Handler()
        {
            Player player = NearestPlayer;

            if (player == null)
                return;

            if (player.HasConditionEffect(ConditionEffectIndex.Invincible)
                || player.HasConditionEffect(ConditionEffectIndex.Invulnerable))
                return;

            if (Distance(player) <= CollisionRange)
            {
                projectile.Owner.RemoveProjectileFromId(owner.Id, id);

                if (description.Effects.Length != 0)
                    foreach (ConditionEffect effect in description.Effects)
                        if (effect.Target == 1)
                            continue;
                        else
                            player.ApplyConditionEffect(effect);

                player.ForceHit(damage, player, description.ArmorPiercing);
            }
        }

        public void SetProjectile(Projectile projectile) => this.projectile = projectile;

        private readonly double CollisionRange = 1.0;

        private int damage
        { get { return projectile.Damage; } }

        private Entity owner
        { get { return projectile.ProjectileOwner; } }

        private Position initialPosition
        { get { return projectile.BeginPos; } }

        private ProjectileDesc description
        { get { return projectile.ProjDesc; } }

        private byte id
        { get { return projectile.ProjectileId; } }

        private float angle
        { get { return projectile.Angle; } }

        private long initialTime
        { get { return projectile.BeginTime; } }

        private Position CurrentPosition
        { get { return GetProjectilePosition(Program.Manager.Logic.CurrentTime.TotalElapsedMs - initialTime); } }

        private Player NearestPlayer
        { get { return (projectile as Entity).GetNearestEntity(CollisionRange, true) as Player; } }

        private double Distance(Player player) => projectile.Dist(player);

        private Position GetProjectilePosition(long elapsedTime)
        {
            double x = initialPosition.X;
            double y = initialPosition.Y;
            double distance1 = (elapsedTime / 1000.0) * (description.Speed / 10.0);
            double period = id % 2 == 0 ? 0 : Math.PI;

            if (description.Wavy)
            {
                double theta = angle + (Math.PI * 64) * Math.Sin(period + 6 * Math.PI * (elapsedTime / 1000));

                x += distance1 * Math.Cos(theta);
                y += distance1 * Math.Sin(theta);
            }
            else if (description.Parametric)
            {
                double theta = (double)elapsedTime / description.LifetimeMS * 2 * Math.PI;
                double a = Math.Sin(theta) * (id % 2 != 0 ? 1 : -1);
                double b = Math.Sin(theta * 2) * (id % 4 < 2 ? 1 : -1);
                double c = Math.Sin(angle);
                double d = Math.Cos(angle);

                x += (a * d - b * c) * description.Magnitude;
                y += (a * c + b * d) * description.Magnitude;
            }
            else
            {
                if (description.Boomerang)
                {
                    double distance2 = (description.LifetimeMS / 1000.0) * (description.Speed / 10.0) / 2;

                    if (distance1 > distance2)
                        distance1 = distance2 - (distance1 - distance2);
                }

                x += distance1 * Math.Cos(angle);
                y += distance1 * Math.Sin(angle);

                if (description.Amplitude != 0)
                {
                    double distance3 = description.Amplitude * Math.Sin(period + (double)elapsedTime / description.LifetimeMS * description.Frequency * 2 * Math.PI);

                    x += distance3 * Math.Cos(angle + Math.PI / 2);
                    y += distance3 * Math.Sin(angle + Math.PI / 2);
                }
            }

            return new Position { X = (float)x, Y = (float)y };
        }
    }
}
