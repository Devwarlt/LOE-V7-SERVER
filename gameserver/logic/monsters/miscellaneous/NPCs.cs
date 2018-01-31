#region

using gameserver.logic.behaviors;

#endregion

namespace gameserver.logic
{
    partial class BehaviorDb
    {
        private _ NPCs = () => Behav()
            .Init("NPC Gazer", new State(new NPCEngine()))
        ;
    }
}