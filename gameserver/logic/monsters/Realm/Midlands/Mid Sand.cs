using gameserver.logic.behaviors;
using gameserver.logic.loot;
using gameserver.logic.transitions;

namespace gameserver.logic
{
    partial class BehaviorDb
    {
        private _ MidlandMidSand = () => Behav()
            .Init("Tawny Warg",
                new State(
                    new Shoot(3.4),
                    new Prioritize(
                        new Protect(12, "Desert Werewolf", sightRange: 14, protectRange: 8, reprotectRange: 5),
                        new Chase(7, sightRange: 9, range: 2),
                        new Wander(8)
                        )
                    ),
                new ItemLoot("Health Potion", 0.04)
            )

            .Init("Demon Warg",
                new State(
                    new Shoot(4.5),
                    new Prioritize(
                        new Protect(12, "Desert Werewolf", sightRange: 14, protectRange: 8, reprotectRange: 5),
                        new Wander(8)
                        )
                    ),
                new ItemLoot("Health Potion", 0.04)
            )

            .Init("Desert Werewolf",
                new State(
                    new SpawnGroup("Wargs", maxChildren: 8, coolDown: 8000),
                    new State("unharmed",
                        new Shoot(8, index: 0, aim: 0.3, coolDown: 1000, coolDownOffset: 500),
                        new Prioritize(
                            new Chase(sightRange: 10.5, range: 2.5),
                            new Wander(5)
                            ),
                        new HpLessTransition(0.75, "enraged")
                        ),
                    new State("enraged",
                        new Shoot(8, index: 0, aim: 0.3, coolDown: 1000, coolDownOffset: 500),
                        new Taunt(0.7, "GRRRRAAGH!"),
                        new ChangeSize(20, 170),
                        new Flashing(0xffff0000, 0.4, 5000),
                        new Prioritize(
                            new Chase(6.5, sightRange: 9, range: 2),
                            new Wander(6.5)
                            )
                        )
                    ),
                new TierLoot(3, ItemType.Weapon, 0.2),
                new TierLoot(4, ItemType.Weapon, 0.12),
                new TierLoot(3, ItemType.Armor, 0.2),
                new TierLoot(4, ItemType.Armor, 0.15),
                new TierLoot(5, ItemType.Armor, 0.02),
                new TierLoot(1, ItemType.Ring, 0.11),
                new TierLoot(1, ItemType.Ability, 0.38),
                new ItemLoot("Magic Potion", 0.03)
            )

            .Init("Fire Golem",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(11, "player_nearby")
                        ),
                    new State("player_nearby",
                        new Prioritize(
                            new StayAbove(14, 65),
                            new Chase(10, range: 3, duration: 3000, coolDown: 3000),
                            new Wander()
                            ),
                        new Spawn("Red Satellite", maxChildren: 1, coolDown: 200),
                        new Spawn("Gray Satellite 1", maxChildren: 1, coolDown: 200),
                        new State("slowshot",
                            new Shoot(10, index: 0, aim: 0.5, coolDown: 300, coolDownOffset: 600),
                            new TimedTransition(5000, "megashot")
                            ),
                        new State("megashot",
                            new Flashing(0xffffffff, 0.2, 5),
                            new Shoot(10, index: 1, aim: 0.2, coolDown: 90, coolDownOffset: 1000),
                            new TimedTransition(1200, "slowshot")
                            )
                        ),
                    new Reproduce(max: 1)
                    ),
                new TierLoot(6, ItemType.Armor, 0.015),
                new ItemLoot("Health Potion", 0.03)
            )

            .Init("Darkness Golem",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(11, "player_nearby")
                        ),
                    new State("player_nearby",
                        new State("first_satellites",
                            new Spawn("Green Satellite", maxChildren: 1, coolDown: 200),
                            new Spawn("Gray Satellite 2", maxChildren: 1, coolDown: 200),
                            new TimedTransition(200, "next_satellite")
                            ),
                        new State("next_satellite",
                            new Spawn("Gray Satellite 2", maxChildren: 1, coolDown: 200),
                            new TimedTransition(200, "follow")
                            ),
                        new State("follow",
                            new Shoot(6, index: 0, coolDown: 200),
                            new Prioritize(
                                new StayAbove(12, 65),
                                new Chase(12, range: 1),
                                new Wander(5)
                                ),
                            new TimedTransition(3000, "wander1")
                            ),
                        new State("wander1",
                            new Shoot(6, index: 0, coolDown: 200),
                            new Prioritize(
                                new StayAbove(6.5, 65),
                                new Wander(6.5)
                                ),
                            new TimedTransition(3800, "back_up")
                            ),
                        new State("back_up",
                            new Flashing(0xffffffff, 0.2, 25),
                            new Shoot(9, index: 1, coolDown: 1400, coolDownOffset: 1000),
                            new Prioritize(
                                new StayAbove(4, 65),
                                new Retreat(4, 4),
                                new Wander()
                                ),
                            new TimedTransition(5400, "wander2")
                            ),
                        new State("wander2",
                            new Shoot(6, index: 0, coolDown: 200),
                            new Prioritize(
                                new StayAbove(6.5, 65),
                                new Wander(6.5)
                                ),
                            new TimedTransition(3800, "first_satellites")
                            )
                        ),
                    new Reproduce(max: 1)
                    ),
                new TierLoot(2, ItemType.Ring, 0.02),
                new ItemLoot("Magic Potion", 0.03)
            )

            .Init("Sand Phantom",
                new State(
                    new Prioritize(
                        new StayAbove(8.5, 60),
                        new Chase(8.5, sightRange: 10.5, range: 1),
                        new Wander(8.5)
                        ),
                    new Shoot(8, aim: 0.4, coolDown: 400, coolDownOffset: 600),
                    new State("follow_player",
                        new PlayerWithinTransition(4.4, "sneak_away_from_player")
                        ),
                    new State("sneak_away_from_player",
                        new Transform("Sand Phantom Wisp")
                        )
                    )
            )

            .Init("Sand Phantom Wisp",
                new State(
                    new Shoot(8, aim: 0.4, coolDown: 400, coolDownOffset: 600),
                    new State("move_away_from_player",
                        new State("keep_back",
                            new Prioritize(
                                new Retreat(6, range: 5),
                                new Wander(9)
                                ),
                            new TimedTransition(800, "wander")
                            ),
                        new State("wander",
                            new Wander(9),
                            new TimedTransition(800, "keep_back")
                            ),
                        new TimedTransition(6500, "wisp_finished")
                        ),
                    new State("wisp_finished",
                        new Transform("Sand Phantom")
                        )
                    )
            )

            .Init("Great Lizard",
                new State(
                    new State("idle",
                        new StayAbove(6, 60),
                        new Wander(6),
                        new PlayerWithinTransition(10, "charge")
                        ),
                    new State("charge",
                        new Prioritize(
                            new StayAbove(6, 60),
                            new Chase(60, sightRange: 11, range: 1.5)
                            ),
                        new TimedTransition(200, "spit")
                        ),
                    new State("spit",
                        new Shoot(8, index: 0, shoots: 1, coolDown: 100000, coolDownOffset: 1000),
                        new Shoot(8, index: 0, shoots: 2, shootAngle: 16, coolDown: 100000, coolDownOffset: 1200),
                        new Shoot(8, index: 0, shoots: 1, aim: 0.2, coolDown: 100000, coolDownOffset: 1600),
                        new Shoot(8, index: 0, shoots: 2, shootAngle: 24, coolDown: 100000, coolDownOffset: 2200),
                        new Shoot(8, index: 0, shoots: 1, aim: 0.2, coolDown: 100000, coolDownOffset: 2800),
                        new Shoot(8, index: 0, shoots: 2, shootAngle: 16, coolDown: 100000, coolDownOffset: 3200),
                        new Shoot(8, index: 0, shoots: 1, aim: 0.1, coolDown: 100000, coolDownOffset: 3800),
                        new Prioritize(
                            new StayAbove(6, 60),
                            new Wander(6)
                            ),
                        new TimedTransition(5000, "flame_ring")
                        ),
                    new State("flame_ring",
                        new Shoot(7, index: 1, shoots: 30, shootAngle: 12, coolDown: 400, coolDownOffset: 600),
                        new Prioritize(
                            new StayAbove(6, 60),
                            new Chase(6, sightRange: 9, range: 1),
                            new Wander(6)
                            ),
                        new TimedTransition(3500, "pause")
                        ),
                    new State("pause",
                        new Prioritize(
                            new StayAbove(6, 60),
                            new Wander(6)
                            ),
                        new TimedTransition(1000, "idle")
                        )
                    ),
                new TierLoot(4, ItemType.Weapon, 0.14),
                new TierLoot(5, ItemType.Weapon, 0.05),
                new TierLoot(5, ItemType.Armor, 0.19),
                new TierLoot(6, ItemType.Armor, 0.02),
                new TierLoot(2, ItemType.Ring, 0.07),
                new TierLoot(2, ItemType.Ability, 0.27),
                new ItemLoot("Health Potion", 0.12),
                new ItemLoot("Magic Potion", 0.10)
            )

            .Init("Nomadic Shaman",
                new State(
                    new Prioritize(
                        new StayAbove(8, 55),
                        new Wander(7)
                        ),
                    new State("fire1",
                        new Shoot(10, index: 0, shoots: 3, shootAngle: 11, coolDown: 500, coolDownOffset: 500),
                        new TimedTransition(3100, "fire2")
                        ),
                    new State("fire2",
                        new Shoot(10, index: 1, coolDown: 700, coolDownOffset: 700),
                        new TimedTransition(2200, "fire1")
                        )
                    ),
                new ItemLoot("Magic Potion", 0.04)
            )
        ;
    }
}