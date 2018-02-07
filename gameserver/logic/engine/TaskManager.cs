#region

using gameserver.realm.entity.player;

#endregion

namespace gameserver.logic
{
    public class TaskManager
    {
        protected Player player { get; set; }

        public TaskManager(
            Player player
            )
        {
            this.player = player;
        }
    }
}