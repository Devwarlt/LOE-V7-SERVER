#region

using LoESoft.GameServer.realm;

#endregion

namespace LoESoft.GameServer.logic.behaviors
{
    public class RemCond : Behavior
    {
        private readonly ConditionEffectIndex effect;
        private readonly bool perm;

        public RemCond(
            ConditionEffectIndex effect,
            bool perm = false
            )
        {
            this.effect = effect;
            this.perm = perm;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            host.ApplyConditionEffect(new ConditionEffect
            {
                Effect = effect,
                DurationMS = 0
            });
        }

        protected override void OnStateExit(Entity host, RealmTime time, ref object state)
        {
            if (!perm)
            {
                host.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = effect,
                    DurationMS = 0
                });
            }
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
        }
    }
}