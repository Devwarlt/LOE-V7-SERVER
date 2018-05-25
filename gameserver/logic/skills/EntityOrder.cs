#region

using LoESoft.Core.models;
using LoESoft.GameServer.realm;
using System.Collections.Generic;

#endregion

namespace LoESoft.GameServer.logic.behaviors
{
    public class EntityOrder : Behavior
    {
        private readonly int name;
        private readonly double range;
        private readonly string targetStateName;
        private State targetState;

        public EntityOrder(
            double range,
            string name,
            string targetState
            )
        {
            this.range = range; try
            {
                this.name = BehaviorDb.InitGameData.IdToObjectType[name];
            }
            catch (KeyNotFoundException)
            { Log.Error($"[State: {targetState}] Entity '{name}' doesn't contains in game assets."); }
            targetStateName = targetState;
        }

        private static State FindState(State state, string name)
        {
            if (state.Name == name) return state;
            State ret;
            foreach (State i in state.States)
                if ((ret = FindState(i, name)) != null)
                    return ret;
            return null;
        }


        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            if (targetState == null)
                targetState = FindState(GameServer.Manager.Behaviors.Definitions[name].Item1, targetStateName);
            foreach (Entity i in host.GetNearestEntities(range, name))
                if (!i.CurrentState.Is(targetState))
                    i.SwitchTo(targetState);
        }
    }
}