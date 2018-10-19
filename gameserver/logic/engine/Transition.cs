#region

using LoESoft.GameServer.realm;
using System;
using System.Collections.Generic;

#endregion

namespace LoESoft.GameServer.logic
{
    public abstract class Transition : IStateChildren
    {
        [ThreadStatic]
        private static Random rand;

        private readonly string targetStateName;

        public Transition(string targetState)
        {
            targetStateName = targetState;
        }

        public State TargetState { get; private set; }

        protected static Random Random
        {
            get
            {
                if (rand == null) rand = new Random();
                return rand;
            }
        }

        public bool Tick(Entity host, RealmTime time)
        {
            if (!host.StoredBehaviors.TryGetValue(this, out object state))
                state = null;

            bool ret = TickCore(host, time, ref state);
            if (ret)
                host.SwitchTo(TargetState);

            if (state == null)
                host.StoredBehaviors.Remove(this);
            else
                host.StoredBehaviors[this] = state;
            return ret;
        }

        protected abstract bool TickCore(Entity host, RealmTime time, ref object state);

        internal void Resolve(IDictionary<string, State> states)
        {
            TargetState = states[targetStateName];
        }
    }
}