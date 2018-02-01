using gameserver.logic.behaviors;
using gameserver.logic.loot;
using gameserver.logic.transitions;

namespace gameserver.logic
{
    partial class BehaviorDb
    {
        private _ MidlandMidPlains = () => Behav()
            .Init("Orc King",
                new State(
                    new DropPortalOnDeath("Spider Den Portal", 0.1),
                    new Shoot(3),
                    new Spawn("Orc Queen", maxChildren: 2, coolDown: 60000),
                    new Prioritize(
                        new StayAbove(14, 60),
                        new Chase(6, range: 1, duration: 3000, coolDown: 3000),
                        new Wander(6)
                        )
                    ),
                new TierLoot(4, ItemType.Weapon),
                new TierLoot(5, ItemType.Weapon),
                new TierLoot(5, ItemType.Armor),
                new TierLoot(6, ItemType.Armor),
                new ItemLoot("Magic Potion", 0.03),
                new TierLoot(2, ItemType.Ring),
                new TierLoot(2, ItemType.Ability)
            )
            .Init("Orc Queen",
                new State(
                    new Spawn("Orc Mage", maxChildren: 2, coolDown: 8000),
                    new Spawn("Orc Warrior", maxChildren: 3, coolDown: 8000),
                    new Prioritize(
                        new StayAbove(14, 60),
                        new Protect(8, "Orc King", sightRange: 11, protectRange: 7, reprotectRange: 5.4),
                        new Wander(8)
                        ),
                    new HealGroup(10, "OrcKings", 300)
                    ),
                new ItemLoot("Health Potion", 0.03)
            )
            .Init("Orc Warrior",
                new State(
                    new Shoot(3, aim: 1, coolDown: 500),
                    new Prioritize(
                        new StayAbove(14, 60),
                        new Circle(13.5, 2.5, target: "Orc Queen", sightRange: 12, speedVariance: 0.1,
                            radiusVariance: 0.1)
                        )
                    ),
                new ItemLoot("Health Potion", 0.03)
            )
            .Init("Orc Mage",
                new State(
                    new State("circle_player",
                        new Shoot(8, aim: 0.3, coolDown: 1000, coolDownOffset: 500),
                        new Prioritize(
                            new StayAbove(14, 60),
                            new Protect(7, "Orc Queen", sightRange: 11, protectRange: 10, reprotectRange: 3),
                            new Circle(7, 3.5, sightRange: 11)
                            ),
                        new TimedTransition(3500, "circle_queen")
                        ),
                    new State("circle_queen",
                        new Shoot(8, shoots: 3, aim: 0.3, shootAngle: 120, coolDown: 1000, coolDownOffset: 500),
                        new Prioritize(
                            new StayAbove(14, 60),
                            new Circle(12, 2.5, target: "Orc Queen", sightRange: 12, speedVariance: 0.1, radiusVariance: 0.1)
                            ),
                        new TimedTransition(3500, "circle_player")
                        )
                    ),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Wasp Queen",
                new State(
                    new DropPortalOnDeath("Forest Maze Portal", 0.20),
                    new Spawn("Worker Wasp", maxChildren: 5, coolDown: 3400),
                    new Spawn("Warrior Wasp", maxChildren: 2, coolDown: 4400),
                    new State("idle",
                        new StayAbove(4, 60),
                        new Wander(5.5),
                        new PlayerWithinTransition(10, "froth")
                        ),
                    new State("froth",
                        new Shoot(8, aim: 0.1, coolDown: 1600),
                        new Prioritize(
                            new StayAbove(4, 60),
                            new Wander(5.5)
                            )
                        )
                    ),
                new TierLoot(5, ItemType.Weapon),
                new TierLoot(6, ItemType.Weapon),
                new TierLoot(5, ItemType.Armor),
                new TierLoot(6, ItemType.Armor),
                new TierLoot(2, ItemType.Ring),
                new TierLoot(3, ItemType.Ring),
                new TierLoot(2, ItemType.Ability),
                new TierLoot(3, ItemType.Ability),
                new ItemLoot("Health Potion", 0.15),
                new ItemLoot("Magic Potion", 0.07)
            )
            .Init("Worker Wasp",
                new State(
                    new Shoot(8, coolDown: 4000),
                    new Prioritize(
                        new Circle(10, 2, target: "Wasp Queen", radiusVariance: 0.5),
                        new Wander(7.5)
                        )
                    )
            )
            .Init("Warrior Wasp",
                new State(
                    new Shoot(8, aim: 200, coolDown: 1000),
                    new State("protecting",
                        new Prioritize(
                            new Circle(10, 2, target: "Wasp Queen", radiusVariance: 0),
                            new Wander(7.5)
                            ),
                        new TimedTransition(3000, "attacking")
                        ),
                    new State("attacking",
                        new Prioritize(
                            new Chase(8, sightRange: 9, range: 3.4),
                            new Circle(10, 2, target: "Wasp Queen", radiusVariance: 0),
                            new Wander(7.5)
                            ),
                        new TimedTransition(2200, "protecting")
                        )
                    )
            )
            .Init("Earth Golem",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(11, "player_nearby")
                        ),
                    new State("player_nearby",
                        new Shoot(8, shoots: 2, shootAngle: 12, coolDown: 600),
                        new State("first_satellites",
                            new Spawn("Green Satellite", maxChildren: 1, coolDown: 200),
                            new Spawn("Gray Satellite 3", maxChildren: 1, coolDown: 200),
                            new TimedTransition(300, "next_satellite")
                            ),
                        new State("next_satellite",
                            new Spawn("Gray Satellite 3", maxChildren: 1, coolDown: 200),
                            new TimedTransition(200, "follow")
                            ),
                        new State("follow",
                            new Prioritize(
                                new StayAbove(14, 65),
                                new Chase(14, range: 3),
                                new Wander(8)
                                ),
                            new TimedTransition(2000, "wander1")
                            ),
                        new State("wander1",
                            new Prioritize(
                                new StayAbove(15.5, 65),
                                new Wander(5.5)
                                ),
                            new TimedTransition(4000, "circle")
                            ),
                        new State("circle",
                            new Prioritize(
                                new StayAbove(12, 65),
                                new Circle(12, 5.4, sightRange: 11)
                                ),
                            new TimedTransition(4000, "wander2")
                            ),
                        new State("wander2",
                            new Prioritize(
                                new StayAbove(5.5, 65),
                                new Wander(5.5)
                                ),
                            new TimedTransition(3000, "back_and_forth")
                            ),
                        new State("back_and_forth",
                            new Prioritize(
                                new StayAbove(5.5, 65),
                                new BackAndForth(8)
                                ),
                            new TimedTransition(3000, "first_satellites")
                            )
                        ),
                    new Reproduce(max: 1)
                    ),
                new TierLoot(2, ItemType.Ring),
                new ItemLoot("Health Potion", 0.03)
            )
            .Init("Paper Golem",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(11, "player_nearby")
                        ),
                    new State("player_nearby",
                        new Spawn("Blue Satellite", maxChildren: 1, coolDown: 200),
                        new Spawn("Gray Satellite 1", maxChildren: 1, coolDown: 200),
                        new Shoot(10, aim: 0.5, coolDown: 700),
                        new Prioritize(
                            new StayAbove(14, 65),
                            new Chase(10, range: 3, duration: 3000, coolDown: 3000),
                            new Wander()
                            ),
                        new TimedTransition(12000, "idle")
                        ),
                    new Reproduce(max: 1)
                    ),
                new TierLoot(5, ItemType.Weapon),
                new ItemLoot("Health Potion", 0.03)
            )
            .Init("Fire Sprite",
                new State(
                    new Reproduce(max: 2),
                    new Shoot(10, shoots: 2, shootAngle: 7, coolDown: 300),
                    new Prioritize(
                        new StayAbove(14, 55),
                        new Wander(14)
                        )
                    ),
                new TierLoot(5, ItemType.Weapon),
                new ItemLoot("Magic Potion", 0.05)
            )
            .Init("Ice Sprite",
                new State(
                    new Reproduce(max: 2),
                    new Shoot(10, shoots: 3, shootAngle: 7),
                    new Prioritize(
                        new StayAbove(14, 60),
                        new Wander(14)
                        )
                    ),
                new TierLoot(2, ItemType.Ability),
                new ItemLoot("Magic Potion", 0.05)
            )
            .Init("Magic Sprite",
                new State(
                    new Reproduce(max: 2),
                    new Shoot(10, shoots: 4, shootAngle: 7),
                    new Prioritize(
                        new StayAbove(14, 60),
                        new Wander(14)
                        )
                    ),
                new TierLoot(6, ItemType.Armor),
                new ItemLoot("Magic Potion", 0.05)
            )
            .Init("Shambling Sludge",
                new State(
                    new State("idle",
                        new StayAbove(5, 55),
                        new PlayerWithinTransition(10, "toss_sludge")
                        ),
                    new State("toss_sludge",
                        new Prioritize(
                            new StayAbove(5, 55),
                            new Wander(5)
                            ),
                        new Shoot(8, coolDown: 1200),
                        new TossObject("Sludget", range: 3, angle: 20, coolDown: 100000),
                        new TossObject("Sludget", range: 3, angle: 92, coolDown: 100000),
                        new TossObject("Sludget", range: 3, angle: 164, coolDown: 100000),
                        new TossObject("Sludget", range: 3, angle: 236, coolDown: 100000),
                        new TossObject("Sludget", range: 3, angle: 308, coolDown: 100000),
                        new TimedTransition(8000, "pause")
                        ),
                    new State("pause",
                        new Prioritize(
                            new StayAbove(5, 55),
                            new Wander(5)
                            ),
                        new TimedTransition(1000, "idle")
                        )
                    ),
                new TierLoot(4, ItemType.Weapon),
                new TierLoot(5, ItemType.Weapon),
                new TierLoot(5, ItemType.Armor),
                new TierLoot(6, ItemType.Armor),
                new TierLoot(2, ItemType.Ring),
                new TierLoot(2, ItemType.Ability),
                new ItemLoot("Health Potion", 0.15),
                new ItemLoot("Magic Potion", 0.10)
            )
            .Init("Sludget",
                new State(
                    new State("idle",
                        new Shoot(8, aim: 0.5, coolDown: 600),
                        new Prioritize(
                            new Protect(5, "Shambling Sludge", 11, 7.5, 7.4),
                            new Wander(5)
                            ),
                        new TimedTransition(1400, "wander")
                        ),
                    new State("wander",
                        new Prioritize(
                            new Protect(5, "Shambling Sludge", 11, 7.5, 7.4),
                            new Wander(5)
                            ),
                        new TimedTransition(5400, "jump")
                        ),
                    new State("jump",
                        new Prioritize(
                            new Protect(5, "Shambling Sludge", 11, 7.5, 7.4),
                            new Chase(7, sightRange: 6, range: 1),
                            new Wander(5)
                            ),
                        new TimedTransition(200, "attack")
                        ),
                    new State("attack",
                        new Shoot(8, aim: 0.5, coolDown: 600, coolDownOffset: 300),
                        new Prioritize(
                            new Protect(5, "Shambling Sludge", 11, 7.5, 7.4),
                            new Chase(5, sightRange: 6, range: 1),
                            new Wander(5)
                            ),
                        new TimedTransition(4000, "idle")
                        ),
                    new Decay(9000)
                    )
            )
            .Init("Big Green Slime",
                new State(
                    new StayAbove(4, 50),
                    new Shoot(9),
                    new Wander(),
                    new Reproduce(max: 5, radius: 10),
                    new TransformOnDeath("Little Green Slime"),
                    new TransformOnDeath("Little Green Slime"),
                    new TransformOnDeath("Little Green Slime"),
                    new TransformOnDeath("Little Green Slime")
                    )
            )
            .Init("Little Green Slime",
                new State(
                    new StayAbove(4, 50),
                    new Shoot(6),
                    new Wander(),
                    new Protect(4, "Big Green Slime")
                    ),
                new ItemLoot("Health Potion", 0.03),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Pink Blob",
                new State(
                    new StayAbove(4, 50),
                    new Shoot(6, shoots: 3, shootAngle: 7),
                    new Prioritize(
                        new Chase(8, sightRange: 15, range: 5),
                        new Wander()
                        ),
                    new Reproduce(max: 5, radius: 10)
                    ),
                new ItemLoot("Health Potion", 0.03),
                new ItemLoot("Magic Potion", 0.03)
            )
            .Init("Gray Blob",
                new State(
                    new State("searching",
                        new StayAbove(2, 50),
                        new Prioritize(
                            new Charge(20),
                            new Wander()
                            ),
                        new Reproduce(max: 5, radius: 10),
                        new PlayerWithinTransition(2, "creeping")
                        ),
                    new State("creeping",
                        new Shoot(0, shoots: 10, shootAngle: 36, direction: 0),
                        new Decay(0)
                        )
                    ),
                new ItemLoot("Health Potion", 0.03),
                new ItemLoot("Magic Potion", 0.03),
                new ItemLoot("Magic Mushroom", 0.005)
            )
            .Init("Swarm",
                new State(
                    new State("circle",
                        new Prioritize(
                            new StayAbove(4, 60),
                            new Chase(40, sightRange: 11, range: 3.5, duration: 1000, coolDown: 5000),
                            new Circle(19, 3.5, sightRange: 12),
                            new Wander()
                            ),
                        new Shoot(4, aim: 0.1, coolDown: 500),
                        new TimedTransition(3000, "dart_away")
                        ),
                    new State("dart_away",
                        new Prioritize(
                            new StayAbove(4, 60),
                            new Retreat(20, range: 5),
                            new Wander()
                            ),
                        new Shoot(8, shoots: 5, shootAngle: 72, direction: 20, coolDown: 100000, coolDownOffset: 800),
                        new Shoot(8, shoots: 5, shootAngle: 72, direction: 56, coolDown: 100000, coolDownOffset: 1400),
                        new TimedTransition(1600, "circle")
                        ),
                    new Reproduce(max: 1, radius: 100)
                    ),
                new TierLoot(3, ItemType.Weapon),
                new TierLoot(4, ItemType.Weapon),
                new TierLoot(3, ItemType.Armor),
                new TierLoot(4, ItemType.Armor),
                new TierLoot(5, ItemType.Armor),
                new TierLoot(1, ItemType.Ring),
                new TierLoot(1, ItemType.Ability),
                new ItemLoot("Health Potion", 0.24),
                new ItemLoot("Magic Potion", 0.07)
            )
            .Init("Candy Gnome",
                new State(
                    new DropPortalOnDeath("Candyland Portal", 0.25),
                    new Buzz(speed: 15, range: 24),
                    new Retreat(speed: 20, range: 24)
                    ),
                new OnlyOne(
                    new ItemLoot("Red Gumball", 0.5),
                    new ItemLoot("Yellow Gumball", 0.5),
                    new ItemLoot("Green Gumball", 0.5),
                    new ItemLoot("Blue Gumball", 0.5),
                    new ItemLoot("Purple Gumball", 0.5)
                    ),
                new OnlyOne(
                    new ItemLoot("Red Gumball", 0.25),
                    new ItemLoot("Yellow Gumball", 0.25),
                    new ItemLoot("Green Gumball", 0.25),
                    new ItemLoot("Blue Gumball", 0.25),
                    new ItemLoot("Purple Gumball", 0.25)
                    ),
                new OnlyOne(
                    new ItemLoot("Red Gumball", 0.125),
                    new ItemLoot("Yellow Gumball", 0.125),
                    new ItemLoot("Green Gumball", 0.125),
                    new ItemLoot("Blue Gumball", 0.125),
                    new ItemLoot("Purple Gumball", 0.125)
                    ),
                new OnlyOne(
                    new ItemLoot("Red Gumball", 0.0625),
                    new ItemLoot("Yellow Gumball", 0.0625),
                    new ItemLoot("Green Gumball", 0.0625),
                    new ItemLoot("Blue Gumball", 0.0625),
                    new ItemLoot("Purple Gumball", 0.0625)
                    )
            )
        ;
    }
}