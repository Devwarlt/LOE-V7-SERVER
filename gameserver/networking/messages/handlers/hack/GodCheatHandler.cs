#region

using LoESoft.GameServer.realm;
using LoESoft.GameServer.realm.entity;
using LoESoft.GameServer.realm.entity.player;
using System;

#endregion

namespace LoESoft.GameServer.networking.messages.handlers.hack
{
    public class GodCheatHandler : ICheatHandler
    {
        protected Projectile Proj { get; set; }

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

            if (Distance(player) <= CollisionRange && Projectile.ProjectileCache.ContainsKey(Id))
            {
                Projectile.Remove(Id);

                Proj.Owner.RemoveProjectileFromId(Owner.Id, Id);

                if (Description.Effects.Length != 0)
                    foreach (ConditionEffect effect in Description.Effects)
                        if (effect.Target == 1)
                            continue;
                        else
                            player.ApplyConditionEffect(effect);

                player.ForceHit(Damage, player, Description.ArmorPiercing);
            }
        }

        public void SetProjectile(Projectile projectile) => this.Proj = projectile;

        private readonly double CollisionRange = 1.0;

        private int Damage => Proj.Damage;

        private Entity Owner
        { get { return Proj.ProjectileOwner; } }

        private Position InitialPosition => Proj.BeginPos;

        private ProjectileDesc Description => Proj.ProjDesc;

        private byte Id => Proj.ProjectileId;

        private float Angle => Proj.Angle;

        private long InitialTime
        { get { return Proj.BeginTime; } }

        private Position CurrentPosition
        { get { return GetProjectilePosition(Program.Manager.Logic.CurrentTime.TotalElapsedMs - InitialTime); } }

        private Player NearestPlayer
        { get { return (Proj as Entity).GetNearestEntity(CollisionRange, true) as Player; } }

        private double Distance(Player player) => Proj.Dist(player);

        private Position GetProjectilePosition(long elapsedTime)
        {
            double x = InitialPosition.X;
            double y = InitialPosition.Y;
            double distance1 = (elapsedTime / 1000.0) * (Description.Speed / 10.0);
            double period = Id % 2 == 0 ? 0 : Math.PI;

            if (Description.Wavy)
            {
                double theta = Angle + (Math.PI * 64) * Math.Sin(period + 6 * Math.PI * (elapsedTime / 1000));

                x += distance1 * Math.Cos(theta);
                y += distance1 * Math.Sin(theta);
            }
            else if (Description.Parametric)
            {
                double theta = (double)elapsedTime / Description.LifetimeMS * 2 * Math.PI;
                double a = Math.Sin(theta) * (Id % 2 != 0 ? 1 : -1);
                double b = Math.Sin(theta * 2) * (Id % 4 < 2 ? 1 : -1);
                double c = Math.Sin(Angle);
                double d = Math.Cos(Angle);

                x += (a * d - b * c) * Description.Magnitude;
                y += (a * c + b * d) * Description.Magnitude;
            }
            else
            {
                if (Description.Boomerang)
                {
                    double distance2 = (Description.LifetimeMS / 1000.0) * (Description.Speed / 10.0) / 2;

                    if (distance1 > distance2)
                        distance1 = distance2 - (distance1 - distance2);
                }

                x += distance1 * Math.Cos(Angle);
                y += distance1 * Math.Sin(Angle);

                if (Description.Amplitude != 0)
                {
                    double distance3 = Description.Amplitude * Math.Sin(period + (double)elapsedTime / Description.LifetimeMS * Description.Frequency * 2 * Math.PI);

                    x += distance3 * Math.Cos(Angle + Math.PI / 2);
                    y += distance3 * Math.Sin(Angle + Math.PI / 2);
                }
            }

            return new Position { X = (float)x, Y = (float)y };
        }
    }
}
