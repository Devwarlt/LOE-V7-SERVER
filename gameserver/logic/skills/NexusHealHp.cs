﻿#region

using System;
using LoESoft.GameServer.networking.outgoing;
using LoESoft.GameServer.realm;
using LoESoft.GameServer.realm.entity.player;

#endregion

namespace LoESoft.GameServer.logic.behaviors
{
    public class NexusHealHp : Behavior
    {
        private readonly int amount;
        private readonly double range;
        private Cooldown coolDown;

        public NexusHealHp(
            double range,
            int amount,
            Cooldown coolDown = new Cooldown()
            )
        {
            this.range = (float)range;
            this.amount = amount;
            this.coolDown = coolDown.Normalize();
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = 0;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            int cool = (int)state;

            if (cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffectIndex.Sick)) return;


                if (host.GetNearestEntity(range, null) is Player entity)
                {
                    int maxHp = entity.Stats[0] + entity.Boost[0];
                    int newHp = Math.Min(maxHp, entity.HP + amount);
                    if (newHp != entity.HP)
                    {
                        int n = newHp - entity.HP;
                        entity.HP = newHp;
                        entity.UpdateCount++;
                        entity.Owner.BroadcastMessage(new SHOWEFFECT
                        {
                            EffectType = EffectType.Heal,
                            TargetId = entity.Id,
                            Color = new ARGB(0xffffffff)
                        }, null);
                        entity.Owner.BroadcastMessage(new SHOWEFFECT
                        {
                            EffectType = EffectType.Line,
                            TargetId = host.Id,
                            PosA = new Position { X = entity.X, Y = entity.Y },
                            Color = new ARGB(0xffffffff)
                        }, null);
                        entity.Owner.BroadcastMessage(new NOTIFICATION
                        {
                            ObjectId = entity.Id,
                            Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"+" + n + "\"}}",
                            Color = new ARGB(0xff00ff00)
                        }, null);
                    }
                }
                cool = coolDown.Next(Random);
            }
            else
                cool -= time.ElapsedMsDelta;

            state = cool;
        }
    }
}