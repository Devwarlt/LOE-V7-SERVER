using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.loot;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ Manor = () => Behav()
        //lord ruthven is waaay unfinished
                    .Init("Lord Ruthven",
                new State(
                    new RealmPortalDrop(),
                    new State("default",
                        new PlayerWithinTransition(8, "spooksters")
                        ),
                    new State("spooksters",
                        new Wander(2),
                        new Shoot(10, shoots: 5, shootAngle: 2, index: 0, coolDown: 900),
                        new TimedTransition(6000, "spooksters2")
                        ),
                    new State("spooksters2",
                        new Wander(1.5),
                        new Shoot(8.4, shoots: 40, index: 1, coolDown: 2750),
                        new Shoot(10, shoots: 5, shootAngle: 2, index: 0, coolDown: 900),
                        new TimedTransition(4000, "spooksters3")
                        ),
                    new State("spooksters3",
                        new Heal(5, group: "Self", amount: 100, coolDown: 1250),
                        new Shoot(8.4, shoots: 40, index: 1, coolDown: 2750),
                        new TimedTransition(4000, "spooksters")
                        )
                    ),
                new Threshold(0.025,
                    new TierLoot(9, ItemType.Weapon, 0.1),
                    new TierLoot(4, ItemType.Ability, 0.1),
                    new TierLoot(9, ItemType.Armor, 0.1),
                    new TierLoot(3, ItemType.Ring, 0.05),
                    new TierLoot(10, ItemType.Armor, 0.05),
                    new TierLoot(10, ItemType.Weapon, 0.05),
                    new TierLoot(4, ItemType.Ring, 0.025),
                    new ItemLoot("Potion of Attack", 1),
                    new ItemLoot("Holy Water", 0.5),
                    new ItemLoot("Death Tarot Card", 0.05),
                    new ItemLoot("Wine Cellar Incantation", 0.01),
                    new ItemLoot("Chasuble of Holy Light", 0.01),
                    new ItemLoot("St. Abraham's Wand", 0.01),
                    new ItemLoot("Tome of Purification", 0.001),
                    new ItemLoot("Ring of Divine Faith", 0.01),
                    new ItemLoot("Bone Dagger", 0.08)
                    )
            )
            .Init("Hellhound",
                new State(
                    new Chase(1.25, 8, 1, coolDown: 275),
                    new Shoot(10, shoots: 5, shootAngle: 7, coolDown: 2000)
                    ),
                new ItemLoot("Health Potion", 0.05),
                new Threshold(0.5,
                    new ItemLoot("Timelock Orb", 0.01)
                    )
            )
                    .Init("Vampire Bat Swarmer",
                new State(
                    new Chase(1.5, 8, 1),
                    new Shoot(10, shoots: 1, coolDown: 6)
                    )
            )
                    .Init("Lil Feratu",
                new State(
                    new Chase(0.35, 8, 1),
                    new Shoot(10, shoots: 6, shootAngle: 2, coolDown: 900)
                    ),
                new ItemLoot("Magic Potion", 0.05),
                new Threshold(0.5,
                    new ItemLoot("Steel Helm", 0.01)
                    )
            )
                            .Init("Lesser Bald Vampire",
                new State(
                    new Chase(0.35, 8, 1),
                    new Shoot(10, shoots: 5, shootAngle: 6, coolDown: 1000)
                    )
            )
                  .Init("Nosferatu",
                new State(
                    new Wander(0.25),
                    new Shoot(10, shoots: 5, shootAngle: 2, index: 1, coolDown: 1000),
                    new Shoot(10, shoots: 6, shootAngle: 90, index: 0, coolDown: 1500)
                    ),
                new ItemLoot("Magic Potion", 0.05),
                new Threshold(0.5,
                    new ItemLoot("Bone Dagger", 0.01),
                    new ItemLoot("Wand of Death", 0.05),
                    new ItemLoot("Golden Bow", 0.04),
                    new ItemLoot("Steel Helm", 0.05),
                    new ItemLoot("Ring of Paramount Defense", 0.09)
                    )
            )
                          .Init("Armor Guard",
                new State(
                    new Wander(0.2),
                    new TossObject("RockBomb", 7, coolDown: 3000, randomToss: true),
                    new Shoot(10, shoots: 1, index: 0, aim: 7, coolDown: 1000),
                    new Shoot(10, shoots: 1, index: 1, coolDown: 750)
                    ),
                new ItemLoot("Health Potion", 0.05),
                new Threshold(0.5,
                    new ItemLoot("Glass Sword", 0.01),
                    new ItemLoot("Staff of Destruction", 0.01),
                    new ItemLoot("Golden Shield", 0.01),
                    new ItemLoot("Ring of Paramount Speed", 0.01)
                    )
            )
                                  .Init("Coffin Creature",
                new State(
                    new Spawn("Lil Feratu", initialSpawn: 1, maxChildren: 2, coolDown: 2250),
                    new Shoot(10, shoots: 1, index: 0, coolDown: 700)
                    ),
                new ItemLoot("Health Potion", 0.05)
            )
                              .Init("RockBomb",
                        new State(
                    new State("BOUTTOEXPLODE",
                    new TimedTransition(1111, "boom")
                        ),
                    new State("boom",
                        new Shoot(8.4, shoots: 1, angleOffset: 0, index: 0, coolDown: 1000),
                        new Shoot(8.4, shoots: 1, angleOffset: 90, index: 0, coolDown: 1000),
                        new Shoot(8.4, shoots: 1, angleOffset: 180, index: 0, coolDown: 1000),
                        new Shoot(8.4, shoots: 1, angleOffset: 270, index: 0, coolDown: 1000),
                        new Shoot(8.4, shoots: 1, angleOffset: 45, index: 0, coolDown: 1000),
                        new Shoot(8.4, shoots: 1, angleOffset: 135, index: 0, coolDown: 1000),
                        new Shoot(8.4, shoots: 1, angleOffset: 235, index: 0, coolDown: 1000),
                        new Shoot(8.4, shoots: 1, angleOffset: 315, index: 0, coolDown: 1000),
                       new Suicide()
                    )
            )
    )
           .Init("Coffin",
                        new State(
                    new State("Coffin1",
                        new HpLessTransition(0.75, "Coffin2")
                        ),
                    new State("Coffin2",
                        new Spawn("Vampire Bat Swarmer", initialSpawn: 1, maxChildren: 15, coolDown: 99999),
                         new HpLessTransition(0.40, "Coffin3")
                        ),
                       new State("Coffin3",
                           new Spawn("Vampire Bat Swarmer", initialSpawn: 1, maxChildren: 8, coolDown: 99999),
                            new Spawn("Nosferatu", initialSpawn: 1, maxChildren: 2, coolDown: 99999)
                        )
                ),
                new Threshold(0.5,
                    new ItemLoot("Holy Water", 1.00),
                     new ItemLoot("Potion of Attack", 0.5),
                     new ItemLoot("Wine Cellar Incantation", 0.01),
                     new ItemLoot("Chasuble of Holy Light", 0.01),
                     new ItemLoot("St. Abraham's Wand", 0.01),
                     new ItemLoot("Tome of Purification", 0.001),
                     new ItemLoot("Ring of Divine Faith", 0.01),
                     new ItemLoot("Bone Dagger", 0.08),
                     new TierLoot(7, ItemType.Weapon, 0.05),
                     new TierLoot(6, ItemType.Armor, 0.2),
                     new TierLoot(4, ItemType.Ability, 0.15)
                    )
            );
    }
}