﻿#region

using gameserver.logic.behaviors;

#endregion

namespace gameserver.logic
{
    partial class BehaviorDb
    {
        private _ Nexus = () => Behav()
            .Init("White Fountain",
                new State(
                    new NexusHealHp(5, 100, 1000)
                )
            )
            .Init("Winter Fountain Frozen",
                new State(
                    new NexusHealHp(5, 100, 1000)
                )
            )
        ;
    }
}