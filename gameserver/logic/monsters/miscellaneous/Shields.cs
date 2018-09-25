#region

using LoESoft.GameServer.logic.behaviors;

#endregion

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ Shields = () => Behav()
            .Init("Player's Shield Entity 1",
                new State(
                    new OrbitAndFollow(5, 4, false)
                )
            )
        ;
    }
}