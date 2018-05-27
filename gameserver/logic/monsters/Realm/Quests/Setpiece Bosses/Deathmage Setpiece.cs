using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.loot;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ SetpieceBossesDeathmageSetpiece = () => Behav()
            .Init("Skeleton",
                new State(
                    new Shoot(3),
                    new State("Default",
                        new Prioritize(
                            new Chase(10, range: 1),
                            new Wander()
                            )
                        ),
                    new State("Protect",
                        new Prioritize(
                            new Protect(10, "Deathmage"),
                            new Wander()
                            ),
                        new EntityNotExistsTransition("Deathmage", 10, "Default")
                        ),
                    new State("Circling",
                        new Prioritize(
                            new Circle(10, 10),
                            new Wander()
                            ),
                        new EntityNotExistsTransition("Deathmage", 10, "Default")
                        ),
                    new State("Engaging",
                        new Prioritize(
                            new Chase(10, sightRange: 15, range: 1),
                            new Wander()
                            ),
                        new EntityNotExistsTransition("Deathmage", 10, "Default")
                        )
                    ),
                new ItemLoot("Long Sword", 0.02),
                new ItemLoot("Dirk", 0.02)
            )

            .Init("Skeleton Swordsman",
                new State(
                    new Shoot(3),
                    new State("Default",
                        new Prioritize(
                            new Chase(10, range: 1),
                            new Wander()
                            )
                        ),
                    new State("Protect",
                        new Prioritize(
                            new Protect(10, "Deathmage"),
                            new Wander()
                            ),
                        new EntityNotExistsTransition("Deathmage", 10, "Default")
                        ),
                    new State("Circling",
                        new Prioritize(
                            new Circle(10, 10),
                            new Wander()
                            ),
                        new EntityNotExistsTransition("Deathmage", 10, "Default")
                        ),
                    new State("Engaging",
                        new Prioritize(
                            new Chase(10, sightRange: 15, range: 1),
                            new Wander()
                            ),
                        new EntityNotExistsTransition("Deathmage", 10, "Default")
                        )
                    ),
                new ItemLoot("Long Sword", 0.03),
                new ItemLoot("Steel Shield", 0.02),
                new ItemLoot("Bronze Helm", 0.02)
            )

            .Init("Skeleton Veteran",
                new State(
                    new Shoot(3),
                    new State("Default",
                        new Prioritize(
                            new Chase(10, range: 1),
                            new Wander()
                            )
                        ),
                    new State("Protect",
                        new Prioritize(
                            new Protect(10, "Deathmage"),
                            new Wander()
                            ),
                        new EntityNotExistsTransition("Deathmage", 10, "Default")
                        ),
                    new State("Circling",
                        new Prioritize(
                            new Circle(10, 10),
                            new Wander()
                            ),
                        new EntityNotExistsTransition("Deathmage", 10, "Default")
                        ),
                    new State("Engaging",
                        new Prioritize(
                            new Chase(10, sightRange: 15, range: 1),
                            new Wander()
                            ),
                        new EntityNotExistsTransition("Deathmage", 10, "Default")
                        )
                    ),
                new ItemLoot("Long Sword", 0.03),
                new ItemLoot("Golden Shield", 0.02),
                new ItemLoot("Cloak of Darkness", 0.01),
                new ItemLoot("Spider Venom", 0.01)
            )

            .Init("Skeleton Mage",
                new State(
                    new Shoot(10),
                    new State("Default",
                        new Prioritize(
                            new Chase(1, range: 7),
                            new Wander(0.4)
                            )
                        ),
                    new State("Protect",
                        new Prioritize(
                            new Protect(10, "Deathmage"),
                            new Wander()
                            ),
                        new EntityNotExistsTransition("Deathmage", 10, "Default")
                        ),
                    new State("Circling",
                        new Prioritize(
                            new Circle(10, 10),
                            new Wander()
                            ),
                        new EntityNotExistsTransition("Deathmage", 10, "Default")
                        ),
                    new State("Engaging",
                        new Prioritize(
                            new Chase(10, sightRange: 15, range: 1),
                            new Wander()
                            ),
                        new EntityNotExistsTransition("Deathmage", 10, "Default")
                        )
                    ),
                new ItemLoot("Missile Wand", 0.02),
                new ItemLoot("Comet Staff", 0.02),
                new ItemLoot("Comet Staff", 0.02),
                new ItemLoot("Fire Nova Spell", 0.02)
            )

            .Init("Deathmage",
                new State(
                    new State("Waiting",
                        new Prioritize(
                            new StayCloseToSpawn(8, 5),
                            new Wander()
                            ),
                        new EntityOrder(10, "Skeleton", "Protect"),
                        new EntityOrder(10, "Skeleton Swordsman", "Protect"),
                        new EntityOrder(10, "Skeleton Veteran", "Protect"),
                        new EntityOrder(10, "Skeleton Mage", "Protect"),
                        new PlayerWithinTransition(15, "Attacking")
                        ),
                    new State("Attacking",
                        new Taunt(0.2, 2000,
                            "{PLAYER}, you will soon be my undead slave!",
                            "My skeletons will make short work of you.",
                            "You will never leave this graveyard alive!"
                            ),
                        new Prioritize(
                            new StayCloseToSpawn(8, 5),
                            new Chase(8, range: 8),
                            new Wander()
                            ),
                        new Shoot(10, shoots: 3, shootAngle: 15, aim: 1),
                        new State("Circling",
                            new Circle(0.8, 5),
                            new EntityOrder(10, "Skeleton", "Circling"),
                            new EntityOrder(10, "Skeleton Swordsman", "Circling"),
                            new EntityOrder(10, "Skeleton Veteran", "Circling"),
                            new EntityOrder(10, "Skeleton Mage", "Circling"),
                            new TimedTransition(2000, "Engaging")
                            ),
                        new State("Engaging",
                            new EntityOrder(10, "Skeleton", "Engaging"),
                            new EntityOrder(10, "Skeleton Swordsman", "Engaging"),
                            new EntityOrder(10, "Skeleton Veteran", "Engaging"),
                            new EntityOrder(10, "Skeleton Mage", "Engaging"),
                            new TimedTransition(2000, "Circling")
                            ),
                        new NoPlayerWithinTransition(30, "Waiting")
                        ),
                    new Spawn("Skeleton", maxChildren: 4, coolDown: 8000),
                    new Spawn("Skeleton Swordsman", maxChildren: 2, coolDown: 8000),
                    new Spawn("Skeleton Veteran", maxChildren: 1, coolDown: 8000),
                    new Spawn("Skeleton Mage", maxChildren: 1, coolDown: 8000)
                    )
            )
        ;
    }
}