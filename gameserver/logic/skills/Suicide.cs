#region

using System;
using LoESoft.GameServer.realm;
using LoESoft.GameServer.realm.entity;

#endregion

namespace LoESoft.GameServer.logic.behaviors
{
    public class Suicide : Behavior
    {
        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            if (!(host is Enemy))
                throw new NotSupportedException("Use Decay instead");
            (host as Enemy).Death(time);
        }
    }
}