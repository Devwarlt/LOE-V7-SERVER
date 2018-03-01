#region

using LoESoft.GameServer.realm;

#endregion

namespace LoESoft.GameServer.logic.behaviors
{
    public class EntityOrder : Behavior
    {
        private readonly ushort name;
        private readonly double range;
        private readonly string targetStateName;
        private State targetState;

        public EntityOrder(
            double range,
            string name,
            string targetState
            )
        {
            this.range = range;
            this.name = BehaviorDb.InitGameData.IdToObjectType[name];
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
                targetState = FindState(Program.Manager.Behaviors.Definitions[name].Item1, targetStateName);
            foreach (Entity i in host.GetNearestEntities(range, name))
                if (!i.CurrentState.Is(targetState))
                    i.SwitchTo(targetState);
        }
    }
}