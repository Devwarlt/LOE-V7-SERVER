using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.loot;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ ShorelineShorePlains = () => Behav()
            .Init("Red Gelatinous Cube",
                new State(
                    new Shoot(8, shoots: 2, shootAngle: 10, aim: 0.2, coolDown: 1000),
                    new Wander(),
                    new Reproduce(max: 5),
                    new DropPortalOnDeath("Pirate Cave Portal", 0.01)
                    ),
                new ItemLoot("Health Potion", 0.04),
                new ItemLoot("Magic Potion", 0.04)
            )
            .Init("Purple Gelatinous Cube",
                new State(
                    new Shoot(8, aim: 0.2, coolDown: 600),
                    new Wander(),
                    new Reproduce(max: 5),
                    new DropPortalOnDeath("Pirate Cave Portal", 0.01)
                    ),
                new ItemLoot("Health Potion", 0.04),
                new ItemLoot("Magic Potion", 0.04)
            )
            .Init("Green Gelatinous Cube",
                new State(
                    new Shoot(8, shoots: 5, shootAngle: 72, aim: 0.2, coolDown: 1800),
                    new Wander(),
                    new Reproduce(max: 5),
                    new DropPortalOnDeath("Pirate Cave Portal", 0.01)
                    ),
                new ItemLoot("Health Potion", 0.04),
                new ItemLoot("Magic Potion", 0.04)
            )
        ;
    }
}