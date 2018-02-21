using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.transitions;
using LoESoft.GameServer.logic.loot;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ Abyss = () => Behav()
            .Init("Archdemon Malphas",
                new State(
                    new OnDeathBehavior(new ApplySetpiece("AbyssDeath")),
                    new State("default",
                        new PlayerWithinTransition(8, "basic")
                        ),
                    new State("basic",
                        new Prioritize(
                            new Chase(0.3),
                            new Wander(2)
                            ),
                        new Reproduce("Malphas Missile", max: 1, radius: 1, coolDown: 1000),
                        new Shoot(10, aim: 1, coolDown: 800),
                        new TimedTransition(10000, "shrink")
                        ),
                    new State("shrink",
                        new Wander(0.4),
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new ChangeSize(-15, 25),
                        new TimedTransition(1000, "smallAttack")
                        ),
                    new State("smallAttack",
                        new Prioritize(
                            new Chase(1, sightRange: 15, range: 8),
                            new Wander(10)
                            ),
                        new Shoot(10, aim: 1, coolDown: 750),
                        new Shoot(10, 6, index: 1, aim: 1, coolDown: 1000),
                        new TimedTransition(10000, "grow")
                        ),
                    new State("grow",
                        new Wander(0.1),
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new ChangeSize(35, 200),
                        new TimedTransition(1050, "bigAttack")
                        ),
                    new State("bigAttack",
                        new Prioritize(
                            new Chase(0.2),
                            new Wander(1)
                            ),
                        new Shoot(10, index: 2, aim: 1, coolDown: 2000),
                        new Shoot(10, index: 2, aim: 1, coolDownOffset: 300, coolDown: 2000),
                        new Shoot(10, 3, index: 3, aim: 1, coolDownOffset: 100, coolDown: 2000),
                        new Shoot(10, 3, index: 3, aim: 1, coolDownOffset: 400, coolDown: 2000),
                        new TimedTransition(10000, "normalize")
                        ),
                    new State("normalize",
                        new Wander(1),
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new ChangeSize(-20, 100),
                        new TimedTransition(1000, "basic")
                        )
                    ),
                //LootTemplates.DefaultEggLoot(EggRarity.Legendary),
                new MostDamagers(3,
                    new ItemLoot("Potion of Vitality", 1.0)
                ),
                new MostDamagers(1,
                    new ItemLoot("Potion of Defense", 1.0)
                ),
                new Threshold(0.025,
                    new TierLoot(9, ItemType.Weapon, 0.1),
                    new TierLoot(4, ItemType.Ability, 0.1),
                    new TierLoot(9, ItemType.Armor, 0.1),
                    new TierLoot(3, ItemType.Ring, 0.05),
                    new TierLoot(10, ItemType.Armor, 0.05),
                    new TierLoot(10, ItemType.Weapon, 0.05),
                    new TierLoot(4, ItemType.Ring, 0.025),
                    new ItemLoot("Demon Blade", 0.01)
                )
            )
            .Init("Malphas Missile",
                new State(
                    new State(
                        new Prioritize(
                            new Chase(0.4, range: 4),
                            new Wander(5)
                        ),
                        new HpLessTransition(0.5, "die"),
                        new TimedTransition(2000, "die")
                    ),
                    new State("die",
                        new Flashing(0xFFFFFF, 0.2, 5),
                        new TimedTransition(1000, "explode")
                        ),
                    new State("explode",
                        new Shoot(10, 8),
                        new Decay(100)
                        )
                    )
            )
            .Init("Imp of the Abyss",
                new State(
                    new Wander(8.75),
                    new Shoot(8, 5, 10, coolDown: 1000)
                    ),
                new ItemLoot("Health Potion", 0.1),
                new ItemLoot("Magic Potion", 0.1),
                new Threshold(0.5,
                    new ItemLoot("Cloak of the Red Agent", 0.01),
                    new ItemLoot("Felwasp Toxin", 0.01)
                    )
            )
            .Init("Demon of the Abyss",
                new State(
                    new Prioritize(
                        new Chase(1, 8, 5),
                        new Wander(2.5)
                        ),
                    new Shoot(8, 3, shootAngle: 10, coolDown: 1000)
                    ),
                new ItemLoot("Fire Bow", 0.05),
                new Threshold(0.5,
                    new ItemLoot("Mithril Armor", 0.01)
                    )
            )
            .Init("Demon Warrior of the Abyss",
                new State(
                    new Prioritize(
                        new Chase(1, 8, 5),
                        new Wander(2.5)
                        ),
                    new Shoot(8, 3, shootAngle: 10, coolDown: 1000)
                    ),
                new ItemLoot("Fire Sword", 0.025),
                new ItemLoot("Steel Shield", 0.025)
            )
            .Init("Demon Mage of the Abyss",
                new State(
                    new Prioritize(
                        new Chase(1, 8, 5),
                        new Wander(2.5)
                        ),
                    new Shoot(8, 3, shootAngle: 10, coolDown: 1000)
                    ),
                new ItemLoot("Fire Nova Spell", 0.02),
                new Threshold(0.1,
                    new ItemLoot("Wand of Dark Magic", 0.01),
                    new ItemLoot("Avenger Staff", 0.01),
                    new ItemLoot("Robe of the Invoker", 0.01),
                    new ItemLoot("Essence Tap Skull", 0.01),
                    new ItemLoot("Demonhunter Trap", 0.01)
                    )
            )
            .Init("Brute of the Abyss",
                new State(
                    new Prioritize(
                        new Chase(1.5, 8, 1),
                        new Wander(2.5)
                        ),
                    new Shoot(8, 3, shootAngle: 10, coolDown: 500)
                    ),
                new ItemLoot("Health Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Obsidian Dagger", 0.02),
                    new ItemLoot("Steel Helm", 0.02)
                    )
            )
            .Init("Brute Warrior of the Abyss",
                new State(
                    new Prioritize(
                        new Chase(1, 8, 1),
                        new Wander(2.5)
                        ),
                    new Shoot(8, 3, shootAngle: 10, coolDown: 500)
                    ),
                new ItemLoot("Spirit Salve Tome", 0.02),
                new Threshold(0.5,
                    new ItemLoot("Glass Sword", 0.01),
                    new ItemLoot("Ring of Greater Dexterity", 0.01),
                    new ItemLoot("Magesteel Quiver", 0.01)
                    )
            )
            ;
    }
}