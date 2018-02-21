﻿#region

using log4net;
using System;
using LoESoft.GameServer.realm;

#endregion

namespace LoESoft.GameServer.logic
{
    public abstract class Behavior : IStateChildren
    {
        public static ILog log = LogManager.GetLogger(nameof(Behavior));

        /// <summary>
        /// Enable debug log
        /// </summary>
        public static readonly bool debug = false;

        [ThreadStatic]
        private static Random rand;
        protected static Random Random
        {
            get
            {
                if (rand == null) rand = new Random();
                return rand;
            }
        }

        public void Tick(Entity host, RealmTime time)
        {

            if (!host.StateStorage.TryGetValue(this, out object state))
                state = null;

            try
            {
                TickCore(host, time, ref state);

                if (state == null)
                    host.StateStorage.Remove(this);
                else
                    host.StateStorage[this] = state;
            }
            catch (Exception e)
            {
                log.ErrorFormat("BehaviorException:\nHost: {0}\nState: {1}\nInternalExeption:\n{2}",
                    Program.Manager.GameData.ObjectTypeToId[host.ObjectType], state, e);
            }
        }

        protected abstract void TickCore(Entity host, RealmTime time, ref object state);

        public void OnStateEntry(Entity host, RealmTime time)
        {
            if (!host.StateStorage.TryGetValue(this, out object state))
                state = null;

            OnStateEntry(host, time, ref state);

            if (state == null)
                host.StateStorage.Remove(this);
            else
                host.StateStorage[this] = state;
        }

        protected virtual void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
        }

        public void OnStateExit(Entity host, RealmTime time)
        {
            if (!host.StateStorage.TryGetValue(this, out object state))
                state = null;

            OnStateExit(host, time, ref state);

            if (state == null)
                host.StateStorage.Remove(this);
            else
                host.StateStorage[this] = state;
        }

        protected virtual void OnStateExit(Entity host, RealmTime time, ref object state)
        {
        }

        protected internal virtual void Resolve(State parent)
        {
        }
    }
}