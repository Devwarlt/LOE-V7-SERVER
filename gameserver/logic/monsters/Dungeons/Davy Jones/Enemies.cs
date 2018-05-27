﻿using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.loot;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ DavyJones = () => Behav()
            .Init("Davy Jones",
                new State(
                    new DropPortalOnDeath("Glowing Realm Portal", 100, PortalDespawnTimeSec: 360),
                    new State("Floating",
                        new ChangeSize(100, 100),
                        new SetAltTexture(1),
                        new SetAltTexture(3),
                        new Wander(2),
                        new Shoot(10, 5, 10, 0, coolDown: 2000),
                        new Shoot(10, 1, 10, 1, coolDown: 4000),
                        new EntityNotExistsTransition("Ghost Lanturn Off", 10, "Vunerable"),
                        new AddCond(ConditionEffectIndex.Invulnerable) // ok
                        ),
                    new State("CheckOffLanterns",
                        new SetAltTexture(2),
                        new StayCloseToSpawn(.1, 3),
                        new Shoot(10, 5, 10, 0, coolDown: 2000),
                        new Shoot(10, 1, 10, 1, coolDown: 4000),
                        new AddCond(ConditionEffectIndex.Invulnerable), // ok
                        new EntityNotExistsTransition("Ghost Lanturn Off", 10, "Vunerable")
                        ),
                    new State("Vunerable",
                        new RemCond(ConditionEffectIndex.Invulnerable), // ok
                        new SetAltTexture(2),
                        new StayCloseToSpawn(.1, 0),
                        new Shoot(10, 5, 10, 0, coolDown: 2000),
                        new Shoot(10, 1, 10, 1, coolDown: 4000),
                        new TimedTransition(5000, "deactivate")
                        ),
                    new State("deactivate",
                        new AddCond(ConditionEffectIndex.Invulnerable), // ok
                        new SetAltTexture(2),
                        new StayCloseToSpawn(.1, 0),
                        new Shoot(10, 5, 10, 0, coolDown: 2000),
                        new Shoot(10, 1, 10, 1, coolDown: 4000),
                        new EntityNotExistsTransition("Ghost Lanturn On", 10, "Floating")
                        )
                    ),
                new Threshold(0.1),
                new TierLoot(3, ItemType.Ring, 0.2),
                new TierLoot(7, ItemType.Armor, 0.2),
                new TierLoot(8, ItemType.Weapon, 0.2),
                new TierLoot(4, ItemType.Ability, 0.1),
                new TierLoot(8, ItemType.Armor, 0.1),
                new TierLoot(4, ItemType.Ring, 0.05),
                new TierLoot(9, ItemType.Armor, 0.03),
                new TierLoot(5, ItemType.Ability, 0.03),
                new TierLoot(9, ItemType.Weapon, 0.03),
                new TierLoot(10, ItemType.Armor, 0.02),
                new TierLoot(10, ItemType.Weapon, 0.02),
                new TierLoot(11, ItemType.Armor, 0.01),
                new TierLoot(11, ItemType.Weapon, 0.01),
                new TierLoot(5, ItemType.Ring, 0.01),
                new ItemLoot("Spirit Dagger", 0.012),
                new ItemLoot("Wine Cellar Incantation", 0.01),
                new ItemLoot("Spectral Cloth Armor", 0.012),
                new ItemLoot("Ghostly Prism", 0.012),
                new ItemLoot("Captain's Ring", 0.012),
                new ItemLoot("Potion of Wisdom", 0.5)
            )
            .Init("Ghost Lanturn Off",
                new State(
                    new TransformOnDeath("Ghost Lanturn On")
                    )
            )
            .Init("Ghost Lanturn On",
                new State(
                    new State("idle",
                        new AddCond(ConditionEffectIndex.Invulnerable), // ok
                        new TimedTransition(5000, "deactivate")
                        ),
                    new State("deactivate",
                        new RemCond(ConditionEffectIndex.Invulnerable), // ok
                        new EntityNotExistsTransition("Ghost Lanturn Off", 10, "shoot"),
                        new TimedTransition(10000, "gone")
                        ),
                    new State("shoot",
                        new Shoot(10, 6, coolDown: 9000001, coolDownOffset: 100),
                        new TimedTransition(1000, "gone")
                        ),
                    new State("gone",
                        new AddCond(ConditionEffectIndex.Invulnerable), // ok
                        new Transform("Ghost Lanturn Off")
                        )
                    )
                    ).Init("GhostShip PurpleDoor Rt",
                new State(
                    new State("Idle",
                        new EntityNotExistsTransition("Purple Key", 500, "Cycle")

                    ),
                    new State("Cycle",
                        new PlayerWithinTransition(2, "Cycle2")

                    ),
                    new State("Cycle2",
                        new Decay(1000)
                    )
               //248, 305
               )
            )
            .Init("GhostShip PurpleDoor Lf",
                new State(
                    new State("Idle",
                        new EntityNotExistsTransition("Purple Key", 500, "Cycle")

                    ),
                    new State("Cycle",
                        new PlayerWithinTransition(2, "Cycle2")

                    ),
                    new State("Cycle2",
                        new Decay(1000)
                    )
               //248, 305
               )
            )
            .Init("Lost Soul",
                new State(
                    new State("Default",
                        new Prioritize(
                            new Circle(0.3, 3, 20, "Ghost of Roger"),
                            new Wander(1)
                            ),
                        new PlayerWithinTransition(4, "Default1")
                        ),
                    new State("Default1",
                       new Charge(0.5, 8, coolDown: 2000),
                       new TimedTransition(2200, "Blammo")
                        ),
                     new State("Blammo",
                       new Shoot(10, shoots: 6, index: 0, coolDown: 2000),
                       new Suicide()
                    )
                )
            ).Init("Ghost of Roger",
                new State(
                    new State("spawn",
                        new Spawn("Lost Soul", 3, 1, 5000),
                        new TimedTransition(100, "Attack")
                    ),
                    new State("Attack",
                        new Shoot(13, 1, 0, 0, coolDown: 10),
                        new TimedTransition(20, "Attack2")
                    ),
                    new State("Attack2",
                        new Shoot(13, 1, 0, 0, coolDown: 10),
                        new TimedTransition(20, "Attack3")
                    ),
                    new State("Attack3",
                        new Shoot(13, 1, 0, 0, coolDown: 10),
                        new TimedTransition(20, "Wait")
                    ),
                    new State("Wait",
                        new TimedTransition(1000, "Attack")
                    )
                )
            )
            .Init("GhostShip GreenDoor Rt",
                new State(
                    new State("Idle",
                        new EntityNotExistsTransition("Green Key", 500, "Cycle")

                    ),
                    new State("Cycle",
                        new PlayerWithinTransition(2, "Cycle2")

                    ),
                    new State("Cycle2",
                        new Decay(1000)
                    )
               //248, 305
               )
            )
            .Init("GhostShip GreenDoor Lf",
                new State(
                    new State("Idle",
                        new EntityNotExistsTransition("Green Key", 500, "Cycle")

                    ),
                    new State("Cycle",
                        new PlayerWithinTransition(2, "Cycle2")

                    ),
                    new State("Cycle2",
                        new Decay(1000)
                    )
               //248, 305
               )
            )
            .Init("GhostShip YellowDoor Rt",
                new State(
                    new State("Idle",
                        new EntityNotExistsTransition("Yellow Key", 500, "Cycle")

                    ),
                    new State("Cycle",
                        new PlayerWithinTransition(2, "Cycle2")

                    ),
                    new State("Cycle2",
                        new Decay(1000)
                    )
               //248, 305
               )
            )
            .Init("GhostShip YellowDoor Lf",
                new State(
                    new State("Idle",
                        new EntityNotExistsTransition("Yellow Key", 500, "Cycle")

                    ),
                    new State("Cycle",
                        new PlayerWithinTransition(2, "Cycle2")

                    ),
                    new State("Cycle2",
                        new Decay(1000)
                    )
               //248, 305
               )
            )
            .Init("GhostShip RedDoor Rt",
                new State(
                    new State("Idle",
                        new EntityNotExistsTransition("Red Key", 500, "Cycle")

                    ),
                    new State("Cycle",
                        new PlayerWithinTransition(2, "Cycle2")

                    ),
                    new State("Cycle2",
                        new Decay(1000)
                    )
               //248, 305
               )
            )
            .Init("GhostShip RedDoor Lf",
                new State(
                    new State("Idle",
                        new EntityNotExistsTransition("Red Key", 500, "Cycle")

                    ),
                    new State("Cycle",
                        new PlayerWithinTransition(2, "Cycle2")

                    ),
                    new State("Cycle2",
                        new Decay(1000)
                    )
               //248, 305
               )
            )
            .Init("Purple Key",
                new State(
                    new State("Idle",
                        new PlayerWithinTransition(1, "Cycle")

                    ),
                    new State("Cycle",
                        new Taunt(true, "Purple Key has been found!"),
                        new Decay(200)



                    )
                )
            )
            .Init("Red Key",
                new State(
                    new State("Idle",
                        new PlayerWithinTransition(1, "Cycle")

                    ),
                    new State("Cycle",
                        new Taunt(true, "Red Key has been found!"),
                        new Decay(200)



                    )
                )
            )
            .Init("Green Key",
                new State(
                    new State("Idle",
                        new PlayerWithinTransition(1, "Cycle")

                    ),
                    new State("Cycle",
                        new Taunt(true, "Green Key has been found!"),
                        new Decay(200)



                    )
                )
            )
            .Init("Yellow Key",
                new State(
                    new State("Idle",
                        new PlayerWithinTransition(1, "Cycle")

                    ),
                    new State("Cycle",
                        new Taunt(true, "Yellow Key has been found!"),
                        new Decay(200)



                    )
                )
            )
  .Init("Lil' Ghost Pirate",
                new State(
                    new ChangeSize(30, 120),
                    new Shoot(10, shoots: 1, index: 0, coolDown: 2000),
                    new State("Default",
                        new Prioritize(
                            new Chase(0.6, 8, 1),
                            new Wander(1)
                            ),
                        new TimedTransition(2850, "Default1")
                        ),
                    new State("Default1",
                       new Retreat(0.2, 3),
                       new TimedTransition(1850, "Default")
                    )
                )
            )
                 .Init("Zombie Pirate Sr",
                new State(
                    new Shoot(10, shoots: 1, index: 0, coolDown: 2000),
                    new State("Default",
                        new Prioritize(
                            new Chase(0.3, 8, 1),
                            new Wander(1)
                            ),
                        new TimedTransition(2850, "Default1")
                        ),
                    new State("Default1",
                       new AddCond(ConditionEffectIndex.Armored),
                       new Prioritize(
                            new Chase(0.3, 8, 1),
                            new Wander(1)
                            ),
                        new TimedTransition(2850, "Default")
                    )
                )
            )
           .Init("Zombie Pirate Jr",
                new State(
                    new Shoot(10, shoots: 1, index: 0, coolDown: 2500),
                    new State("Default",
                        new Prioritize(
                            new Chase(0.4, 8, 1),
                            new Wander(1)
                            ),
                        new TimedTransition(2850, "Default1")
                        ),
                    new State("Default1",
                       new Swirl(0.2, 3),
                       new TimedTransition(1850, "Default")
                    )
                )
            )
        .Init("Captain Summoner",
                new State(
                    new State("Default",
                        new AddCond(ConditionEffectIndex.Invincible)
                        )
                )
            )
           .Init("GhostShip Rat",
                new State(
                    new State("Default",
                        new Shoot(10, shoots: 1, index: 0, coolDown: 1750),
                        new Prioritize(
                            new Chase(0.55, 8, 1),
                            new Wander(1)
                            )
                        )
                )
            )
        .Init("Violent Spirit",
                new State(
                    new State("Default",
                        new ChangeSize(35, 120),
                        new Shoot(10, shoots: 3, index: 0, coolDown: 1750),
                        new Prioritize(
                            new Chase(0.25, 8, 1),
                            new Wander(1)
                            )
                        )
                )
            )
           .Init("School of Ghostfish",
                new State(
                    new State("Default",
                        new Shoot(10, shoots: 3, shootAngle: 18, index: 0, coolDown: 4000),
                        new Wander(3.5)
                        )
                )
            );
    }
}