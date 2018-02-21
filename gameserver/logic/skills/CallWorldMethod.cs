#region

using System;
using System.Reflection;
using LoESoft.GameServer.realm;

#endregion

namespace LoESoft.GameServer.logic.behaviors
{
    public class CallWorldMethod : Behavior
    {
        private readonly string worldClass;
        private readonly string targetMethod;
        private readonly object[] parameters;

        public CallWorldMethod(
            string worldClass,
            string targetMethod,
            params object[] parameters
            )
        {
            this.worldClass = worldClass;
            this.targetMethod = targetMethod;
            this.parameters = parameters;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            if (host.Owner.GetType() == Type.GetType("LoESoft.GameServer.realm.worlds." + worldClass))
            {
                Type type = host.Owner.GetType();
                MethodInfo method = type.GetMethod(targetMethod);
                if (method != null)
                    method.Invoke(host.Owner, parameters);
            }
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state) { }
    }
}
