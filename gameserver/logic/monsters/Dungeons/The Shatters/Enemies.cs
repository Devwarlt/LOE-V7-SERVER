using gameserver.logic.behaviors;

namespace gameserver.logic
{
    partial class BehaviorDb
    {
        private _ TheShattersEnemies = () => Behav()
            .Init("shtrs Ice Shield",
                new State(
                    new Charge(range: 8, speed: 20, coolDown: 1000)
                )
            )
        ;
    }
}