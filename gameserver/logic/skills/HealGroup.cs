using gameserver.networking.outgoing;
using System.Linq;
using gameserver.realm;
using gameserver.realm.entity;

namespace gameserver.logic.behaviors
{
    class HealGroup : Behavior
    {
        //State storage: cooldown timer

        double range;
        string group;
        Cooldown coolDown;
        int? amount;

        public HealGroup(double range, string group, Cooldown coolDown = new Cooldown(), int? healAmount = null)
        {
            this.range = (float)range;
            this.group = group;
            this.coolDown = coolDown.Normalize();
            this.amount = healAmount;
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
                if (host.HasConditionEffect(ConditionEffects.Stunned)) return;

                foreach (var entity in host.GetNearestEntitiesByGroup(range, group).OfType<Enemy>())
                {
                    int newHp = (int)entity.ObjectDesc.MaxHP;
                    if (amount != null)
                    {
                        var newHealth = (int)amount + entity.HP;
                        if (newHp > newHealth)
                            newHp = newHealth;
                    }
                    if (newHp != entity.HP)
                    {
                        int n = newHp - entity.HP;
                        entity.HP = newHp;
                        entity.Owner.BroadcastPacket(new SHOWEFFECT()
                        {
                            EffectType = EffectType.Heal,
                            TargetId = entity.Id,
                            Color = new ARGB(0xffffffff)
                        }, null);
                        entity.Owner.BroadcastPacket(new SHOWEFFECT()
                        {
                            EffectType = EffectType.Line,
                            TargetId = host.Id,
                            PosA = new Position() { X = entity.X, Y = entity.Y },
                            Color = new ARGB(0xffffffff)
                        }, null);
                        entity.Owner.BroadcastPacket(new NOTIFICATION()
                        {
                            ObjectId = entity.Id,
                            Text = "+" + n,
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