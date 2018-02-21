using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.loot;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ EventBossesSkullShrineEvent = () => Behav()
            .Init("Skull Shrine",
                new State(
                    new State("initial",
                        new Shoot(30, 9, 10, coolDown: 750, aim: 1), // add prediction after fixing it...
                        new Reproduce("Red Flaming Skull", 40, 20, coolDown: 500),
                        new Reproduce("Blue Flaming Skull", 40, 20, coolDown: 500),
                        new HpLessTransition(0.35, "flashing")
                        ),
                    new State("flashing",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Flashing(0xFF0000, 0.5, (int)(10 / 0.5)),
                        new TimedTransition(10000, "final")
                        ),
                    new State("final",
                        new RemCond(ConditionEffectIndex.Invulnerable),
                        new Shoot(30, 9, 10, coolDown: 750, aim: 1), // add prediction after fixing it...
                        new Reproduce("Red Flaming Skull", 40, 20, coolDown: 500),
                        new Reproduce("Blue Flaming Skull", 40, 20, coolDown: 500),
                        new Flashing(0xFF0000, 0.5, int.MaxValue / 2)
                        )
                    ),
                new Threshold(0.05,
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
                    new ItemLoot("Orb of Conflict", .001)
                    )
            )

            .Init("Red Flaming Skull",
                new State(
                    new State("Orbit Skull Shrine",
                        new Prioritize(
                            new Protect(3, "Skull Shrine", 30, 15, 15),
                            new Wander(3)
                            ),
                        new EntityNotExistsTransition("Skull Shrine", 40, "Wander")
                        ),
                    new State("Wander",
                        new Wander(3)
                        ),
                        new Shoot(12, 2, 10, coolDown: 750)
                    ),
                    new Threshold(.01,
                        new ItemLoot("Flaming Boomerang", .0005)
                    )
            )

            .Init("Blue Flaming Skull",
                new State(
                    new State("Orbit Skull Shrine",
                        new Circle(15, 15, 40, "Skull Shrine", .6, 10, orbitClockwise: null),
                        new EntityNotExistsTransition("Skull Shrine", 40, "Wander")
                        ),
                    new State("Wander",
                        new Wander(15)
                        ),
                    new Shoot(12, 2, 10, coolDown: 750)
                    )
            )
        ;
    }
}