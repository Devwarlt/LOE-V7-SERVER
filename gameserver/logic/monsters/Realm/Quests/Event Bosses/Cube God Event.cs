using gameserver.logic.behaviors;
using gameserver.logic.loot;
using gameserver.logic.transitions;

namespace gameserver.logic
{
    partial class BehaviorDb
    {
        private _ EventBossesCubeGodEvent = () => Behav()
            .Init("Cube God",
                new State(
                    new State("initial",
                        new Wander(3),
                        new Shoot(30, 9, 10, 0, aim: .5, coolDown: 750),
                        new Shoot(30, 4, 10, 1, aim: .5, coolDown: 1500),
                        new Reproduce("Cube Overseer", 30, 10, coolDown: 1500),
                        new HpLessTransition(0.35, "flashing")
                        ),
                    new State("flashing",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Flashing(0xFF0000, 0.5, (int)(10 / 0.5)),
                        new TimedTransition(10000, "final")
                        ),
                    new State("final",
                        new RemCond(ConditionEffectIndex.Invulnerable),
                        new Wander(3),
                        new Shoot(30, 9, 10, 0, aim: .15, coolDown: 500),
                        new Shoot(30, 4, 10, 1, aim: .15, coolDown: 750),
                        new Reproduce("Cube Overseer", 30, 10, coolDown: 1500),
                        new Flashing(0xFF0000, 0.5, int.MaxValue / 2)
                        )
                    ),
                new Threshold(.05,
                    new TierLoot(8, ItemType.Weapon, .15),
                    new TierLoot(9, ItemType.Weapon, .1),
                    new TierLoot(10, ItemType.Weapon, .07),
                    new TierLoot(11, ItemType.Weapon, .05),
                    new TierLoot(4, ItemType.Ability, .15),
                    new TierLoot(5, ItemType.Ability, .07),
                    new TierLoot(8, ItemType.Armor, .2),
                    new TierLoot(9, ItemType.Armor, .15),
                    new TierLoot(10, ItemType.Armor, .10),
                    new TierLoot(11, ItemType.Armor, .07),
                    new TierLoot(12, ItemType.Armor, .04),
                    new TierLoot(3, ItemType.Ring, .15),
                    new TierLoot(4, ItemType.Ring, .07),
                    new TierLoot(5, ItemType.Ring, .03),
                    new ItemLoot("Potion of Defense", .1),
                    new ItemLoot("Potion of Attack", .1),
                    new ItemLoot("Potion of Vitality", .1),
                    new ItemLoot("Potion of Wisdom", .1),
                    new ItemLoot("Potion of Speed", .1),
                    new ItemLoot("Potion of Dexterity", .1),
                    new ItemLoot("Dirk of Cronus", .001)
                    )
            )

            .Init("Cube Overseer",
                new State(
                    new Prioritize(
                        new Circle(3.75, 10, 30, "Cube God", .075, 5),
                        new Wander(3.75)
                        ),
                    new Reproduce("Cube Defender", 12, 10, coolDown: 1000),
                    new Reproduce("Cube Blaster", 30, 10, coolDown: 1000),
                    new Shoot(10, 4, 10, 0, coolDown: 750),
                    new Shoot(10, index: 1, coolDown: 1500)
                    ),
                new Threshold(.01,
                    new ItemLoot ("Fire Sword", .05)
                    )
            )

            .Init("Cube Defender",
                new State(
                    new Prioritize(
                        new Circle(10.5, 5, 15, "Cube Overseer", .15, 3),
                        new Wander(10.5)
                        ),
                    new Shoot(10, coolDown: 500)
                    )
            )

            .Init("Cube Blaster",
                new State(
                    new State("Orbit",
                        new Prioritize(
                            new Circle(10.5, 7.5, 40, "Cube Overseer", .15, 3),
                            new Wander(10.5)
                            ),
                        new EntityNotExistsTransition("Cube Overseer", 10, "Follow")
                        ),
                    new State("Follow",
                        new Prioritize(
                            new Chase(7.5, 10, 1, 5000),
                            new Wander(10.5)
                            ),
                        new EntityNotExistsTransition("Cube Defender", 10, "Orbit"),
                        new TimedTransition(5000, "Orbit")
                        ),
                    new Shoot(10, 2, 10, 1, aim: 1, coolDown: 500),
                    new Shoot(10, aim: 1, coolDown: 1500)
                    )
            )
        ;
    }
}