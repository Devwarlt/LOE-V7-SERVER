using gameserver.networking;
using gameserver.networking.outgoing;
using gameserver.realm;
using gameserver.realm.entity;
using gameserver.realm.entity.player;
using System;
using System.Collections.Generic;

namespace gameserver.logic.skills.Pets
{
    internal class PetShoot : CycleBehavior
    {
        protected readonly double angleOffset;
        protected readonly int count;
        protected readonly double predictive;
        protected readonly int projectileIndex;
        protected readonly double shootAngle;
        protected readonly double? fixedAngle;
        protected readonly double? defaultAngle;
        protected readonly bool special;
        protected readonly int coolDownOffset;
        protected Cooldown coolDown;

        public PetShoot(
            int count = 1,
            double? shootAngle = null,
            int projectileIndex = 0,
            double? fixedAngle = null,
            double angleOffset = 0,
            double? defaultAngle = null,
            double predictive = 0,
            bool special = false,
            int coolDownOffset = 0,
            Cooldown coolDown = new Cooldown()
            )
        {
            this.count = count;
            this.shootAngle = count == 1 ? 0 : (shootAngle ?? 360.0 / count) * Math.PI / 180;
            this.fixedAngle = fixedAngle * Math.PI / 180;
            this.angleOffset = angleOffset * Math.PI / 180;
            this.defaultAngle = defaultAngle * Math.PI / 180;
            this.projectileIndex = projectileIndex;
            this.predictive = predictive;
            this.special = special;
            this.coolDownOffset = coolDownOffset;
            this.coolDown = coolDown.Normalize();
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state) => state = special ? coolDownOffset : 0;

        private static Position EnemyShootHistory(Entity host)
        {
            Position? history = host.TryGetHistory(1);

            if (history == null)
                return new Position { X = host.X, Y = host.Y };

            return new Position { X = history.Value.X, Y = history.Value.Y };
        }

        private static double Predict(Entity host, Entity target, ProjectileDesc desc)
        {
            Position? history = target.TryGetHistory(1);
            if (history == null)
                return 0;

            double originalAngle = Math.Atan2(history.Value.Y - host.Y, history.Value.X - host.X);
            double newAngle = Math.Atan2(target.Y - host.Y, target.X - host.X);


            float bulletSpeed = desc.Speed / 100;
            double angularVelo = (newAngle - originalAngle) / (100 / 1000f);
            return angularVelo * bulletSpeed;
        }

        private void _(string message) => Log.Write(nameof(PetShoot), message, ConsoleColor.DarkYellow);

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            Player player = host.GetPlayerOwner();
            Entity pet = player.Pet;
            bool hatchling = player.HatchlingPet;

            if (hatchling)
                return;

            if (player.Owner == null || pet == null || host == null)
            {
                pet.Owner.LeaveWorld(host);
                return;
            }

            if (host.Owner.SafePlace)
                return;

            int cool = (int?)state ?? -1;
            Status = CycleStatus.NotStarted;

            if (cool <= 0)
            {
                if (player.HasConditionEffect(ConditionEffectIndex.Sick) || player.HasConditionEffect(ConditionEffectIndex.PetDisable))
                    return;

                int stars = player.Stars;

                Entity target = pet.GetNearestEntity(12, false, enemy => enemy is Enemy && pet.Dist(enemy) <= 12) as Enemy;

                if (target != null && target.ObjectDesc.Enemy)
                {
                    ProjectileDesc desc = pet.ObjectDesc.Projectiles[projectileIndex];

                    double a = fixedAngle ??
                               (target == null ?
                               defaultAngle.Value :
                               Math.Atan2(target.Y - pet.Y, target.X - pet.X));
                    a += angleOffset;
                    if (predictive != 0 && target != null)
                        a += Predict(pet, target, desc) * predictive;

                    int variance;

                    if (stars == 70)
                        variance = 7000;
                    else
                        variance = player.Stars * 100;

                    cool = special ? cool = coolDown.Next(Random) : (7750 - variance); // max 750ms cooldown if not special

                    Random rnd = new Random();

                    int min = 0;
                    int max = 100;
                    int success = stars + 30;
                    int rng = rnd.Next(min, max);

                    if (rng > success)
                    {
                        List<Message> _outgoing = new List<Message>();

                        SHOWEFFECT _effect = new SHOWEFFECT();
                        Position _position = new Position();
                        NOTIFICATION _notification = new NOTIFICATION();

                        _position.X = .25f;
                        _position.Y = 2 / _position.X;

                        _effect.Color = new ARGB(0xFF0000);
                        _effect.EffectType = EffectType.Flash;
                        _effect.PosA = _position;
                        _effect.TargetId = pet.Id;

                        _outgoing.Add(_effect);

                        _notification.Color = new ARGB(0xFFFFFF);
                        _notification.ObjectId = pet.Id;
                        _notification.Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"Miss!\"}}";

                        _outgoing.Add(_notification);

                        pet.Owner.BroadcastPackets(_outgoing, null);

                        state = cool;
                        return;
                    }

                    int dmg = rnd.Next(desc.MinDamage, desc.MaxDamage);

                    double startAngle = a - shootAngle * (count - 1) / 2;

                    Position prjPos = EnemyShootHistory(pet);

                    Projectile prj = pet.CreateProjectile(
                        desc, pet.ObjectType, dmg, time.TotalElapsedMs,
                        prjPos, (float)startAngle);

                    // Visual only
                    SERVERPLAYERSHOOT _shoot = new SERVERPLAYERSHOOT();
                    _shoot.BulletId = prj.ProjectileId;
                    _shoot.OwnerId = player.Id;
                    _shoot.ContainerType = pet.ObjectType;
                    _shoot.StartingPos = prj.BeginPos;
                    _shoot.Angle = prj.Angle;
                    _shoot.Damage = 0;

                    pet.Owner.BroadcastPacket(_shoot, null);

                    target.Owner.Timers.Add(new WorldTimer((int)(prj.ProjDesc.Speed * prj.ProjDesc.LifetimeMS) / 100, (world, t) =>
                   {
                       if (target != null)
                           (target as Enemy).Damage(player, time, dmg, prj.ProjDesc.ArmorPiercing, prj.ProjDesc.Effects);
                   }));

                    Status = CycleStatus.Completed;
                }
                else
                    return;
            }
            else
            {
                cool -= time.ElapsedMsDelta;
                Status = CycleStatus.InProgress;
            }
            state = cool;
        }
    }
}