#region

using LoESoft.Core.models;
using LoESoft.GameServer.realm;
using System.Collections.Generic;

#endregion

namespace LoESoft.GameServer.logic.transitions
{
    public class EntityExistsTransition : Transition
    {
        //State storage: none

        private readonly double dist;
        private readonly ushort target;

        public EntityExistsTransition(string target, double dist, string targetState)
            : base(targetState)
        {
            this.dist = dist;
            try
            {
                this.target = BehaviorDb.InitGameData.IdToObjectType[target];
            }
            catch (KeyNotFoundException)
            { Log.Error($"[State: {targetState}] Entity '{target}' doesn't contains in game assets."); }
        }

        protected override bool TickCore(Entity host, RealmTime time, ref object state)
        {
            return host.GetNearestEntity(dist, target) != null;
        }
    }
}
