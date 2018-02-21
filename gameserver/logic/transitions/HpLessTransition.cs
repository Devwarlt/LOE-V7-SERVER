﻿#region

using LoESoft.GameServer.realm;
using LoESoft.GameServer.realm.entity;

#endregion

namespace LoESoft.GameServer.logic.transitions
{
    public class HpLessTransition : Transition
    {
        //State storage: none

        private readonly double threshold;

        public HpLessTransition(double threshold, string targetState)
            : base(targetState)
        {
            this.threshold = threshold;
        }

        protected override bool TickCore(Entity host, RealmTime time, ref object state)
        {
            if (threshold > 1.0)
                return (host as Enemy).HP < threshold;
            return ((host as Enemy).HP / host.ObjectDesc.MaxHP) < threshold;
        }
    }
}