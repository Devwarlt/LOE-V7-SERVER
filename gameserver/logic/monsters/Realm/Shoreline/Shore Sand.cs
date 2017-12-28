using gameserver.logic.behaviors;
using gameserver.logic.loot;
using gameserver.logic.transitions;

namespace gameserver.logic
{
    partial class BehaviorDb
    {
        private _ ShorelineShoreSand = () => Behav()
            .Init("Pirate",
                new State(
                    new Prioritize(
                        new Chase(8.5, range: 1, duration: 5000, coolDown: 0),
                        new Wander()
                        ),
                    new Shoot(3, coolDown: 2500)
                    ),
                new TierLoot(1, ItemType.Weapon),
                new ItemLoot("Health Potion", 0.03)
            )
            .Init("Piratess",
                new State(
                    new Prioritize(
                        new Chase(11, range: 1, duration: 3000, coolDown: 1500),
                        new Wander(6)
                        ),
                    new Shoot(3, coolDown: 2500),
                    new Reproduce("Pirate", max: 5),
                    new Reproduce("Piratess", max: 5)
                    ),
                new TierLoot(1, ItemType.Armor),
                new ItemLoot("Health Potion", 0.03)
            )
            .Init("Scorpion Queen",
                new State(
                    new ChangeSize(100, 200),
                    new Wander(2),
                    new Spawn("Poison Scorpion"),
                    new Reproduce("Poison Scorpion", coolDown: 10000, max: 10),
                    new Reproduce(max: 2, radius: 40)
                    ),
                new ItemLoot("Health Potion", 0.03),
                new ItemLoot("Magic Potion", 0.02)
            )
            .Init("Poison Scorpion",
                new State(
                    new Prioritize(
                        new Protect(4, "Scorpion Queen"),
                        new Wander()
                        ),
                    new Shoot(8, coolDown: 2000)
                    )
            )
            .Init("Bandit Leader",
                new State(
                    new Spawn("Bandit Enemy", coolDown: 8000, maxChildren: 4),
                    new State("bold",
                        new State("warn_about_grenades",
                            new Taunt(0.15, "Catch!"),
                            new TimedTransition(400, "wimpy_grenade1")
                            ),
                        new State("wimpy_grenade1",
                            new Grenade(1.4, 12, coolDown: 10000),
                            new Prioritize(
                                new StayAbove(3, 7),
                                new Wander(3)
                                ),
                            new TimedTransition(2000, "wimpy_grenade2")
                            ),
                        new State("wimpy_grenade2",
                            new Grenade(1.4, 12, coolDown: 10000),
                            new Prioritize(
                                new StayAbove(5, 7),
                                new Wander(5)
                                ),
                            new TimedTransition(3000, "slow_follow")
                            ),
                        new State("slow_follow",
                            new Shoot(13, coolDown: 1000),
                            new Prioritize(
                                new StayAbove(4, 7),
                                new Chase(4, sightRange: 9, range: 3.5, duration: 4000),
                                new Wander()
                                ),
                            new TimedTransition(4000, "warn_about_grenades")
                            ),
                        new HpLessTransition(0.45, "meek")
                        ),
                    new State("meek",
                        new Taunt(0.5, "Forget this... run for it!"),
                        new Retreat(5, 6),
                        new EntityOrder(10, "Bandit Enemy", "escape"),
                        new TimedTransition(12000, "bold")
                        )
                    ),
                new TierLoot(1, ItemType.Weapon),
                new TierLoot(1, ItemType.Armor),
                new TierLoot(2, ItemType.Weapon),
                new TierLoot(2, ItemType.Armor),
                new ItemLoot("Health Potion", 0.12),
                new ItemLoot("Magic Potion", 0.14)
            )
            .Init("Bandit Enemy",
                new State(
                    new State("fast_follow",
                        new Shoot(3),
                        new Prioritize(
                            new Protect(6, "Bandit Leader", sightRange: 9, protectRange: 7, reprotectRange: 3),
                            new Chase(10, range: 1),
                            new Wander(6)
                            ),
                        new TimedTransition(3000, "scatter1")
                        ),
                    new State("scatter1",
                        new Prioritize(
                            new Protect(6, "Bandit Leader", sightRange: 9, protectRange: 7, reprotectRange: 3),
                            new Wander(10),
                            new Wander(6)
                            ),
                        new TimedTransition(2000, "slow_follow")
                        ),
                    new State("slow_follow",
                        new Shoot(4.5),
                        new Prioritize(
                            new Protect(6, "Bandit Leader", sightRange: 9, protectRange: 7, reprotectRange: 3),
                            new Chase(sightRange: 9, range: 3.5, duration: 4000),
                            new Wander(5)
                            ),
                        new TimedTransition(3000, "scatter2")
                        ),
                    new State("scatter2",
                        new Prioritize(
                            new Protect(6, "Bandit Leader", sightRange: 9, protectRange: 7, reprotectRange: 3),
                            new Wander(10),
                            new Wander(6)
                            ),
                        new TimedTransition(2000, "fast_follow")
                        ),
                    new State("escape",
                        new Retreat(5, 8),
                        new TimedTransition(15000, "fast_follow")
                        )
                    )
            )
            .Init("Snake",
                new State(
                    new Wander(8),
                    new Shoot(10, coolDown: 2000),
                    new Reproduce(max: 5)
                    ),
                new ItemLoot("Health Potion", 0.03),
                new ItemLoot("Magic Potion", 0.02)
            )
        ;
    }
}