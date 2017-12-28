#region

using gameserver.realm;

#endregion

namespace gameserver.logic.transitions
{
    public class PlayerWithinTransition : Transition
    {
        private readonly double range;

        public PlayerWithinTransition(double range, string targetState)
            : base(targetState)
        {
            this.range = range;
        }

        protected override bool TickCore(Entity host, RealmTime time, ref object state)
        {
            return host.GetNearestEntity(range, null) != null;
        }
    }
}