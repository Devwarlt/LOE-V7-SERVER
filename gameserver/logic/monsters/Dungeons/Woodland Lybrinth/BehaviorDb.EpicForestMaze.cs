using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.loot;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private const float fixedAngle_RingAttack2 = 22.5f;
        private _ EForestMaze = () => Behav()
        .Init("Murderous Megamoth",
             new State(
                    new DropPortalOnDeath("Glowing Realm Portal", 100, PortalDespawnTimeSec: 360),
                 new State("idle",
                     new Wander(2),
                     new Chase(5.0, 10, coolDown: 0),
                     new Spawn("Mini Larva", coolDown: 500, maxChildren: 10, initialSpawn: 4),
                     new Shoot(25, index: 0, shoots: 2, shootAngle: 10, coolDown: 500, coolDownOffset: 500),
                     new Shoot(25, index: 1, shoots: 1, shootAngle: 0, coolDown: 1, coolDownOffset: 1)
                     )
                 ),
                 new MostDamagers(1,
                    new ItemLoot("Potion of Vitality", 0.9)
                ),
                new Threshold(0.025,
                    new TierLoot(8, ItemType.Armor, 0.035),
                    new TierLoot(9, ItemType.Armor, 0.03),
                    new TierLoot(10, ItemType.Armor, 0.025),
                    new TierLoot(11, ItemType.Armor, 0.02),
                    new TierLoot(12, ItemType.Armor, 0.015),
                    new TierLoot(13, ItemType.Armor, 0.01),
                    new TierLoot(4, ItemType.Ability, 0.03),
                    new TierLoot(8, ItemType.Weapon, 0.01),
                    new TierLoot(9, ItemType.Weapon, 0.01),
                    new TierLoot(12, ItemType.Weapon, 0.01),
                    new ItemLoot("Wine Cellar Incantation", 0.01),
                    new ItemLoot("Leaf Bow", 0.007)
                ),
                new Threshold(0.2
                )
            )
        .Init("Mini Larva",
            new State(
                new State("idle",
                    new Wander(1),
                    new Protect(1, "Murderous Megamoth", 100, 5, 5),
                    new Shoot(10, shoots: 4, index: 0, angleOffset: fixedAngle_RingAttack2)
                    )
                )
            )
        .Init("Epic Mama Megamoth",
            new State(
                new HpLessTransition(.3, "change"),
                new TransformOnDeath("Murderous Megamoth"),
                new State("idle",
                    new Wander(2),
                    new Chase(4.0, 10, coolDown: 0),
                    new Spawn("Woodland Mini Megamoth", coolDown: 500, initialSpawn: 5),
                    new Reproduce("Woodland Mini Megamoth", coolDown: 500, max: 12, radius: 5),
                    new Shoot(25, index: 0, shoots: 3, shootAngle: 10, coolDown: 1, coolDownOffset: 1)
                    ),
                new State("change",
                    new AddCond(ConditionEffectIndex.Invulnerable), // ok
                    new Flashing(0xfFF0000, 1, 900001),
                    new TimedTransition(3000, "suicide")
                    ),
                new State("suicide",
                    new Suicide()
                    )

                )
            )
        .Init("Woodland Mini Megamoth",
            new State(
                new EntityNotExistsTransition("Epic Mama Megamoth", 20, "suicide"),
                new State("idle",
                    new Wander(1),
                    new Shoot(25, index: 0, shoots: 1, shootAngle: 0, coolDown: 0, coolDownOffset: 0),
                    new Protect(1, "Epic Mama Megamoth", 20, 5, 1)
                    ),
                new State("suicide",
                    new Suicide()
                    )
                )
            )
        .Init("Epic Larva",
            new State(
                new Prioritize(
                    new Chase(1.5, 8, 1),
                    new Wander(2.5)
                ),
                new HpLessTransition(.3, "change"),
                new HpLessTransition(.75, "shoot4"),
                new TransformOnDeath("Epic Mama Megamoth"),
                new PlayerWithinTransition(10, "shoot1"),
                new State("shoot1",
                    new Shoot(0, 3, shootAngle: 15, index: 0, angleOffset: 45, coolDownOffset: 1250),
                    new Shoot(0, 3, shootAngle: 15, index: 0, angleOffset: 135, coolDownOffset: 1250),
                    new Shoot(0, 3, shootAngle: 15, index: 0, angleOffset: 225, coolDownOffset: 1250),
                    new Shoot(0, 3, shootAngle: 15, index: 0, angleOffset: 315, coolDownOffset: 1250),
                    new Shoot(10, shoots: 8, index: 0, angleOffset: fixedAngle_RingAttack2, coolDownOffset: 1500),
                    new Shoot(10, shoots: 4, index: 0, angleOffset: fixedAngle_RingAttack2, coolDownOffset: 2000)
                    ),
                new State("shoot4",
                    //toss Larva Puke
                    new Shoot(0, 3, shootAngle: 15, index: 0, angleOffset: 45, coolDownOffset: 1250),
                    new Shoot(0, 3, shootAngle: 15, index: 0, angleOffset: 135, coolDownOffset: 1250),
                    new Shoot(0, 3, shootAngle: 15, index: 0, angleOffset: 225, coolDownOffset: 1250),
                    new Shoot(0, 3, shootAngle: 15, index: 0, angleOffset: 315, coolDownOffset: 1250),
                    new Shoot(10, shoots: 8, index: 0, angleOffset: fixedAngle_RingAttack2, coolDownOffset: 1500),
                    new Shoot(10, shoots: 4, index: 0, angleOffset: fixedAngle_RingAttack2, coolDownOffset: 2000)
                    ),
                new State("change",
                    new AddCond(ConditionEffectIndex.Invulnerable), // ok
                    new Flashing(0xfFF0000, 1, 900001),
                    new TimedTransition(3000, "suicide")
                    ),
                new State("suicide",
                    new Suicide()
                    )

                )
            )
        .Init("Woodland Ultimate Squirrel",
            new State(
                new Prioritize(
                    new Chase(0.4, 6, 1, -1, 0),
                    new Wander(30)
                    ),
                new Shoot(rotateRadius: 7, shoots: 1, index: 0, shootAngle: 20, coolDown: 2000)
                ),
                new Threshold(0.025,
                    new ItemLoot("Speed Sprout", 0.05)
                )
            )
        .Init("Woodland Goblin Mage",
            new State(
                new Prioritize(
                    new Wander(4),
                    new Shoot(rotateRadius: 10, shoots: 2, index: 0, aim: 1, coolDown: 500, shootAngle: 2)
                    )
                ),
                new Threshold(0.025,
                    new ItemLoot("Speed Sprout", 0.05)
                )
            )
        .Init("Woodland Goblin",
            new State(
                new Prioritize(
                    new Wander(4),
                    new Chase(0.7, 10, 3, -1, 0),
                    new Shoot(rotateRadius: 4, shoots: 1, index: 0, coolDown: 500)
                    )
                ),
                new Threshold(0.025,
                    new ItemLoot("Speed Sprout", 0.05)
                )
            )
        .Init("Wooland Armor Squirrel",
            new State(
                new Prioritize(
                    new Chase(0.6, 6, 1, -1, 0),
                    new Wander(7),
                    new Shoot(rotateRadius: 7, shoots: 3, index: 0, aim: 1, coolDown: 1000, shootAngle: 15)
                    )
                ),
                new Threshold(0.025,
                    new ItemLoot("Speed Sprout", 0.05)
                )
            )
        .Init("Woodland Silence Turret",
            new State(
                new AddCond(ConditionEffectIndex.Invincible),
                new State("idle",
                    new Shoot(10, shoots: 8, index: 0, coolDown: 4000, angleOffset: fixedAngle_RingAttack2)
                    )
                )
            )
        .Init("Woodland Weakness Turret",
            new State(
                new AddCond(ConditionEffectIndex.Invincible),
                new State("idle",
                    new Shoot(10, shoots: 8, index: 0, coolDown: 4000, angleOffset: fixedAngle_RingAttack2)
                    )
                )
            )
        .Init("Woodland Paralyze Turret",
            new State(
                new AddCond(ConditionEffectIndex.Invincible),
                new State("idle",
                    new Shoot(10, shoots: 8, index: 0, coolDown: 4000, angleOffset: fixedAngle_RingAttack2)
                    )
                )
            );
    }
}
