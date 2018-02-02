﻿#region

using gameserver.networking.outgoing;
using gameserver.realm;
using gameserver.realm.entity;

#endregion

namespace gameserver.logic.behaviors.Drakes
{
    internal class GreenDrakeAttack : Behavior
    {
        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = 0;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            int cool = (int)state;
            if (cool <= 0)
            {
                var entities = host.GetNearestEntities(6);

                Enemy en = null;
                foreach (Entity e in entities)
                    if (e is Enemy)
                    {
                        en = e as Enemy;
                        break;
                    }

                if (en != null & en.ObjectDesc.Enemy)
                {
                    en.Owner.BroadcastPacket(new SHOWEFFECT
                    {
                        EffectType = EffectType.Nova,
                        Color = new ARGB(0x48D747),
                        TargetId = en.Id,
                        PosA = new Position { X = 1, }
                    }, null);
                    en.Owner.BroadcastPacket(new SHOWEFFECT
                    {
                        EffectType = EffectType.Line,
                        TargetId = host.Id,
                        PosA = new Position { X = en.X, Y = en.Y },
                        Color = new ARGB(0x48D747)
                    }, null);
                    en.ApplyConditionEffect(new ConditionEffect
                    {
                        Effect = ConditionEffectIndex.Slowed,
                        DurationMS = 10000
                    });

                }
                cool = 300;
            }
            else
                cool -= time.ElapsedMsDelta;

            state = cool;
        }
    }
}