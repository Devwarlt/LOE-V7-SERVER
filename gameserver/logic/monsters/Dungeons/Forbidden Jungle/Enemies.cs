using gameserver.logic.behaviors;
using gameserver.logic.loot;
using gameserver.logic.transitions;

namespace gameserver.logic
{
    partial class BehaviorDb
    {
        private _ ForbiddenJungle = () => Behav()
            .Init("Mixcoatl the Masked God",
                new State(
                    new DropPortalOnDeath("Glowing Realm Portal", 100, PortalDespawnTimeSec: 360),
                    new State("default",
                        new PlayerWithinTransition(8, "basic")
                        ),
                    new State("basic",
                        new Prioritize(
                        
                            new Wander(1)
                            ),
                        new Shoot(10, aim: 1, coolDown: 800),
                        new TimedTransition(10000, "shrink")
                        ),
                    new State("shrink",
                        new Wander(1),
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(1000, "smallAttack")
                        ),
                    new State("smallAttack",
                        new Prioritize(
                            new Chase(1, sightRange: 15, range: 8),
                            new Wander(10)
                            ),
                        new Flashing(0x66FF00, 0.6, 9),
                        new Shoot(10, aim: 1, coolDown: 750),
                        new Shoot(10, 9, index: 1, aim: 1, coolDown: 1000),
                        new TimedTransition(10000, "grow")
                        ),
                    new State("grow",
                        new Wander(1),
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(1050, "bigAttack")
                        ),
                    new State("bigAttack",
                        new Prioritize(
                            new Chase(0.2),
                            new Wander(1)
                            ),
                        new Flashing(0x66FF00, 0.6, 9),
                        new Shoot(10, index: 2, aim: 1, coolDown: 2000),
                        new Shoot(10, index: 0, aim: 1, coolDownOffset: 300, coolDown: 2000),
                        new Shoot(10, 4, index: 1, aim: 1, coolDownOffset: 100, coolDown: 2000),
                        new Shoot(10, 2, index: 0, aim: 1, coolDownOffset: 400, coolDown: 2000),
                        new TimedTransition(10000, "normalize")
                        ),
                    new State("normalize",
                        new Wander(3),
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(1000, "basic")
                        )
                    ),
                //LootTemplates.DefaultEggLoot(EggRarity.Legendary),
                
                new Threshold(0.025,
                    new ItemLoot("Staff of the Crystal Serpent", 0.11),

                    new ItemLoot("Cracked Crystal Skull", 0.11),

                    new ItemLoot("Crystal Bone Ring", 0.11),

                    new ItemLoot("Robe of the Tlatoani", 0.11)
                )
            )
           
            .Init("Mask Shaman",
                new State(
                    new Wander(8.75),
                    new Shoot(8, 5, 10, coolDown: 1000)
                    
                    )
            )
            .Init("Basilisk",
                new State(
                    new Prioritize(
                        new Chase(1, 8, 5),
                        new Wander(2.5)
                        ),
                    new Shoot(8, 3, shootAngle: 10, coolDown: 1000)

                    )
            )
            .Init("Mask Warrior",
                new State(
                    new Prioritize(
                        new Chase(1, 8, 5),
                        new Wander(2.5)
                        ),
                    new Shoot(8, 1, shootAngle: 10, coolDown: 1000)
                    )
            )
            .Init("Mask Hunter",
                new State(
                    new Prioritize(
                        new Chase(1, 8, 5),
                        new Wander(2.5)
                        ),
                    new Shoot(8, 1, shootAngle: 10, coolDown: 1000)
                    )
            )
            .Init("Basilisk Baby",
                new State(
                    new Prioritize(
                        new Chase(1.5, 8, 1),
                        new Wander(2.5)
                        ),
                    new Shoot(8, 1, shootAngle: 10, coolDown: 500)
           
                    )
            )
            ;
    }
}