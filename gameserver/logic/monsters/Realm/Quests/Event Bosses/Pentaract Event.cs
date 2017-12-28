using gameserver.logic.behaviors;
using gameserver.logic.loot;
using gameserver.logic.transitions;

namespace gameserver.logic
{
    partial class BehaviorDb
    {
        private _ EventBossesPentaractEvent = () => Behav()
            .Init("Pentaract Eye",
                new State(
                    new Prioritize(
                        new Swirl(20, 8, 20, true),
                        new Protect(20, "Pentaract Tower", 20, 6, 4)
                        ),
                    new Shoot(9, 1, coolDown: 1000)
                    )
            )

            .Init("Pentaract Tower",
                new State(
                    new Spawn("Pentaract Eye", 5, coolDown: 5000),
                    new Grenade(4, 100, 8, coolDown: 5000, effect: ConditionEffectIndex.Slowed, effectDuration: 6000),
                    new TransformOnDeath("Pentaract Tower Corpse"),
                    new CopyDamageOnDeath("Pentaract"),
                    // needed to avoid crash, Oryx.cs needs player name otherwise hangs server (will patch that later)
                    new CopyDamageOnDeath("Pentaract Tower Corpse")
                    )
            )

            .Init("Pentaract",
                new State(
                    new AddCond(ConditionEffectIndex.Invincible),
                    new State("Waiting",
                        new EntityNotExistsTransition("Pentaract Tower", 50, "Die")
                        ),
                    new State("Die",
                        new Suicide()
                        )
                    )
            )

            .Init("Pentaract Tower Corpse",
                new State(
                    new AddCond(ConditionEffectIndex.Invincible),
                    new State("Waiting",
                        new TimedTransition(15000, "Spawn"),
                        new EntityNotExistsTransition("Pentaract Tower", 50, "Die")
                        ),
                    new State("Spawn",
                        new Transform("Pentaract Tower")
                        ),
                    new State("Die",
                        new Suicide()
                        )
                    ),
                new Threshold(0.01,
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
                    new ItemLoot("Seal of Blasphemous Prayer", .004)
                    )
            )
        ;
    }
}