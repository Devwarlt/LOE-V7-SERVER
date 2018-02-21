﻿#region

using System;
using LoESoft.GameServer.networking.outgoing;
using LoESoft.GameServer.realm;
using LoESoft.GameServer.realm.entity;

#endregion

namespace LoESoft.GameServer.logic.behaviors
{
    public class Aoe : Behavior
    {
        private readonly float radius;
        private readonly bool players;
        private readonly int minDamage;
        private readonly int maxDamage;
        private readonly bool noDef;
        private readonly ARGB color;

        public Aoe(
            double radius,
            bool players,
            int minDamage,
            int maxDamage,
            bool noDef,
            uint color
            )
        {
            this.radius = (float)radius;
            this.players = players;
            this.minDamage = minDamage;
            this.maxDamage = maxDamage;
            this.noDef = noDef;
            this.color = new ARGB(color);
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            var pos = new Position
            {
                X = host.X,
                Y = host.Y
            };

            var damage = Random.Next(minDamage, maxDamage);

            host.Owner.Aoe(pos, radius, players, enemy =>
            {
                if (!players)
                {
                    if (enemy is Enemy)
                        (enemy as Enemy).Damage(null, time, damage, noDef);

                    host.Owner.BroadcastPacket(new SHOWEFFECT
                    {
                        EffectType = EffectType.Nova,
                        TargetId = host.Id,
                        PosA = new Position { X = radius, Y = 0 },
                        Color = color
                    }, null);
                }
                else
                {
                }
            });
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state) { }
    }
}
