﻿#region

using gameserver.realm;

#endregion

namespace gameserver.logic.behaviors
{
    public class Transform : Behavior
    {
        private readonly ushort target;

        public Transform(
            string target
            )
        {
            this.target = BehaviorDb.InitGameData.IdToObjectType[target];
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            Entity entity = Entity.Resolve(target);

            entity.Move(host.X, host.Y);
            host.Owner.EnterWorld(entity);
            host.Owner.LeaveWorld(host);
        }
    }
}