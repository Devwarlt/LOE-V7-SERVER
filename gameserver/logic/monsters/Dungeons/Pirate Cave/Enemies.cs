using gameserver.logic.behaviors;

namespace gameserver.logic
{
    partial class BehaviorDb
    {
        private _ PirateCaveEnemies = () => Behav()
            .Init("Cave Pirate Brawler",
                new State(
                    new Wander(),
                    new Chase(range: 1, speed: 8)
                )
            )
        ;
    }
}