using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.loot;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ HeroesofOryxLichHero = () => Behav()
            .Init("Lich",
                new State(
                    new State("Idle",
                        new StayCloseToSpawn(5, range: 5),
                        new Wander(5),
                        new HpLessTransition(0.99999, "EvaluationStart1")
                        ),
                    new State("EvaluationStart1",
                        new Taunt("New recruits for my undead army? How delightful!"),
                        new AddCond(ConditionEffectIndex.Invulnerable), // ok
                        new Prioritize(
                            new StayCloseToSpawn(3.5, range: 5),
                            new Wander(3.5)
                            ),
                        new TimedTransition(2500, "EvaluationStart2")
                        ),
                    new State("EvaluationStart2",
                        new RemCond(ConditionEffectIndex.Invulnerable), // ok
                        new Flashing(0x0000ff, 0.1, 60),
                        new Prioritize(
                            new StayCloseToSpawn(3.5, range: 5),
                            new Wander(3.5)
                            ),
                        new Shoot(10, index: 1, shoots: 3, shootAngle: 120, coolDown: 100000, coolDownOffset: 200),
                        new Shoot(10, index: 1, shoots: 3, shootAngle: 120, coolDown: 100000, coolDownOffset: 400),
                        new Shoot(10, index: 1, shoots: 3, shootAngle: 120, coolDown: 100000, coolDownOffset: 2200),
                        new Shoot(10, index: 1, shoots: 3, shootAngle: 120, coolDown: 100000, coolDownOffset: 2400),
                        new Shoot(10, index: 1, shoots: 3, shootAngle: 120, coolDown: 100000, coolDownOffset: 4200),
                        new Shoot(10, index: 1, shoots: 3, shootAngle: 120, coolDown: 100000, coolDownOffset: 4400),
                        new HpLessTransition(0.87, "EvaluationEnd"),
                        new TimedTransition(6000, "EvaluationEnd")
                        ),
                    new State("EvaluationEnd",
                        new Taunt("Time to meet your future brothers and sisters..."),
                        new HpLessTransition(0.875, "HugeMob"),
                        new HpLessTransition(0.952, "Mob"),
                        new HpLessTransition(0.985, "SmallGroup"),
                        new HpLessTransition(0.99999, "Solo")
                        ),
                    new State("HugeMob",
                        new Taunt("...there's an ARMY of them! HahaHahaaaa!!!"),
                        new Flashing(0x00ff00, 0.2, 300),
                        new Spawn("Haunted Spirit", maxChildren: 5, initialSpawn: 0, coolDown: 3000),
                        new TossObject("Phylactery Bearer", range: 5.5, angle: 0, coolDown: 100000),
                        new TossObject("Phylactery Bearer", range: 5.5, angle: 120, coolDown: 100000),
                        new TossObject("Phylactery Bearer", range: 5.5, angle: 240, coolDown: 100000),
                        new TossObject("Phylactery Bearer", range: 3, angle: 60, coolDown: 100000),
                        new TossObject("Phylactery Bearer", range: 3, angle: 180, coolDown: 100000),
                        new Prioritize(
                            new Protect(9, "Phylactery Bearer", sightRange: 15, protectRange: 2, reprotectRange: 2),
                            new Wander(9)
                            ),
                        new TimedTransition(25000, "HugeMob2")
                        ),
                    new State("HugeMob2",
                        new Taunt("My minions have stolen your life force and fed it to me!"),
                        new Flashing(0x00ff00, 0.2, 300),
                        new Spawn("Haunted Spirit", maxChildren: 5, initialSpawn: 0, coolDown: 3000),
                        new TossObject("Phylactery Bearer", range: 5.5, angle: 0, coolDown: 100000),
                        new TossObject("Phylactery Bearer", range: 5.5, angle: 120, coolDown: 100000),
                        new TossObject("Phylactery Bearer", range: 5.5, angle: 240, coolDown: 100000),
                        new Prioritize(
                            new Protect(9, "Phylactery Bearer", sightRange: 15, protectRange: 2, reprotectRange: 2),
                            new Wander(9)
                            ),
                        new TimedTransition(5000, "Wait")
                        ),
                    new State("Mob",
                        new Taunt("...there's a lot of them! Hahaha!!"),
                        new Flashing(0x00ff00, 0.2, 300),
                        new Spawn("Haunted Spirit", maxChildren: 2, initialSpawn: 0, coolDown: 2000),
                        new TossObject("Phylactery Bearer", range: 5.5, angle: 0, coolDown: 100000),
                        new TossObject("Phylactery Bearer", range: 5.5, angle: 120, coolDown: 100000),
                        new TossObject("Phylactery Bearer", range: 5.5, angle: 240, coolDown: 100000),
                        new Prioritize(
                            new Protect(9, "Phylactery Bearer", sightRange: 15, protectRange: 2, reprotectRange: 2),
                            new Wander(9)
                            ),
                        new TimedTransition(22000, "Mob2")
                        ),
                    new State("Mob2",
                        new Taunt("My minions have stolen your life force and fed it to me!"),
                        new Spawn("Haunted Spirit", maxChildren: 2, initialSpawn: 0, coolDown: 2000),
                        new TossObject("Phylactery Bearer", range: 5.5, angle: 0, coolDown: 100000),
                        new TossObject("Phylactery Bearer", range: 5.5, angle: 120, coolDown: 100000),
                        new TossObject("Phylactery Bearer", range: 5.5, angle: 240, coolDown: 100000),
                        new Prioritize(
                            new Protect(9, "Phylactery Bearer", sightRange: 15, protectRange: 2, reprotectRange: 2),
                            new Wander(9)
                            ),
                        new TimedTransition(5000, "Wait")
                        ),
                    new State("SmallGroup",
                        new Taunt("...and there's more where they came from!"),
                        new Flashing(0x00ff00, 0.2, 300),
                        new TossObject("Phylactery Bearer", range: 5.5, angle: 0, coolDown: 100000),
                        new TossObject("Phylactery Bearer", range: 5.5, angle: 240, coolDown: 100000),
                        new Prioritize(
                            new Protect(9, "Phylactery Bearer", sightRange: 15, protectRange: 2, reprotectRange: 2),
                            new Wander(9)
                            ),
                        new TimedTransition(15000, "SmallGroup2")
                        ),
                    new State("SmallGroup2",
                        new Taunt("My minions have stolen your life force and fed it to me!"),
                        new Spawn("Haunted Spirit", maxChildren: 1, initialSpawn: 1, coolDown: 9000),
                        new Prioritize(
                            new Protect(9, "Phylactery Bearer", sightRange: 15, protectRange: 2, reprotectRange: 2),
                            new Wander(9)
                            ),
                        new TimedTransition(5000, "Wait")
                        ),
                    new State("Solo",
                        new Taunt("...it's a small family, but you'll enjoy being part of it!"),
                        new Flashing(0x00ff00, 0.2, 10),
                        new Wander(5),
                        new TimedTransition(3000, "Wait")
                        ),
                    new State("Wait",
                        new Taunt("Kneel before me! I am the master of life and death!"),
                        new Transform("Actual Lich")
                        )
                    )
            )

            .Init("Actual Lich",
                new State(
                    new Prioritize(
                        new Protect(9, "Phylactery Bearer", sightRange: 15, protectRange: 2, reprotectRange: 2),
                        new Wander(5)
                        ),
                    new Spawn("Mummy", maxChildren: 4, coolDown: 4000),
                    new Spawn("Mummy King", maxChildren: 2, coolDown: 4000),
                    new Spawn("Mummy Pharaoh", maxChildren: 1, coolDown: 4000),
                    new State("typeA",
                        new Shoot(10, index: 0, shoots: 2, shootAngle: 7, coolDown: 800),
                        new TimedTransition(8000, "typeB")
                        ),
                    new State("typeB",
                        new Taunt(0.7,
                            "All that I touch turns to dust!",
                            "You will drown in a sea of undead!"
                            ),
                        new Shoot(10, index: 1, shoots: 4, shootAngle: 7, coolDown: 1000),
                        new Shoot(10, index: 0, shoots: 2, shootAngle: 7, coolDown: 800),
                        new TimedTransition(6000, "typeA")
                        )
                    ),
			     new PinkBag(ItemType.Weapon, 5),
				 new PinkBag(ItemType.Weapon, 6),
				 new PinkBag(ItemType.Weapon, 7),
				 new PinkBag(ItemType.Armor, 5),
				 new PinkBag(ItemType.Armor, 6),
				 new PinkBag(ItemType.Armor, 7),
				 new PinkBag(ItemType.Ability, 1),
				 new PinkBag(ItemType.Ability, 2),
				 new PinkBag(ItemType.Ring, 2),
				 new Drops(
					new OnlyOne(
						new PurpleBag(ItemType.Ring, 3),
						new PurpleBag(ItemType.Ability, 3)
						)
					 ),
				new ItemLoot("Health Potion", 0.7),
				new ItemLoot("Magic Potion", 0.7)
			)

			.Init("Phylactery Bearer",
                new State(
                    new HealGroup(15, "Heros", coolDown: 200),
                    new State("Attack1",
                        new Shoot(10, index: 0, shoots: 3, shootAngle: 120, coolDown: 900, coolDownOffset: 400),
                        new State("AttackX",
                            new Prioritize(
                                new StayCloseToSpawn(5.5, range: 5),
                                new Circle(5.5, 4, sightRange: 5)
                                ),
                            new TimedTransition(1500, "AttackY")
                            ),
                        new State("AttackY",
                            new Taunt(0.05, "We feed the master!"),
                            new Prioritize(
                                new StayCloseToSpawn(5.5, range: 5),
                                new Retreat(5.5, range: 2),
                                new Wander(5.5)
                                ),
                            new TimedTransition(1500, "AttackX")
                            ),
                        new HpLessTransition(0.65, "Attack2")
                        ),
                    new State("Attack2",
                        new Shoot(10, index: 0, shoots: 3, shootAngle: 15, aim: 0.1, coolDown: 600, coolDownOffset: 200),
                        new State("AttackX",
                            new Prioritize(
                                new StayCloseToSpawn(6.5, range: 5),
                                new Circle(6.5, 4, sightRange: 10)
                                ),
                            new TimedTransition(1500, "AttackY")
                            ),
                        new State("AttackY",
                            new Taunt(0.05, "We feed the master!"),
                            new Prioritize(
                                new StayCloseToSpawn(6.5, range: 5),
                                new Buzz(),
                                new Wander(6.5)
                                ),
                            new TimedTransition(1500, "AttackX")
                            ),
                        new HpLessTransition(0.3, "Attack3")
                        ),
                    new State("Attack3",
                        new Shoot(10, index: 1, coolDown: 800),
                        new State("AttackX",
                            new AddCond(ConditionEffectIndex.Invulnerable), // ok
                            new Prioritize(
                                new StayCloseToSpawn(13, range: 5),
                                new Wander(13)
                                ),
                            new TimedTransition(2500, "AttackY")
                            ),
                        new State("AttackY",
                            new RemCond(ConditionEffectIndex.Invulnerable), // ok
                            new Taunt(0.02, "We feed the master!"),
                            new Prioritize(
                                new StayCloseToSpawn(10, range: 5),
                                new Wander(10)
                                ),
                            new TimedTransition(2500, "AttackX")
                            )
                        ),
                    new Decay(130000)
                    ),
                new ItemLoot("Tincture of Defense", 0.02),
                new ItemLoot("Orange Drake Egg", 0.06),
                new ItemLoot("Magic Potion", 0.03)
            )

            .Init("Haunted Spirit",
                new State(
                    new State("NewLocation",
                        new Taunt(0.1, "XxxXxxxXxXxXxxx..."),
                        new AddCond(ConditionEffectIndex.Invulnerable), // ok
                        new Shoot(10, aim: 0.2, coolDown: 700),
                        new Prioritize(
                            new StayCloseToSpawn(10, range: 11),
                            new Wander(10)
                            ),
                        new TimedTransition(7000, "Attack")
                        ),
                    new State("Attack",
                        new RemCond(ConditionEffectIndex.Invulnerable), // ok
                        new Taunt(0.1, "Hungry..."),
                        new Shoot(10, aim: 0.3, coolDown: 700),
                        new Shoot(10, shoots: 2, shootAngle: 70, coolDown: 700, coolDownOffset: 200),
                        new TimedTransition(3000, "NewLocation")
                        ),
                    new Decay(90000)
                    ),
                new TierLoot(8, ItemType.Weapon, 0.02),
                new ItemLoot("Magic Potion", 0.02),
                new ItemLoot("Ring of Magic", 0.02),
                new ItemLoot("Ring of Attack", 0.02),
                new ItemLoot("Tincture of Dexterity", 0.06),
                new ItemLoot("Tincture of Mana", 0.09),
                new ItemLoot("Tincture of Life", 0.04)
            )

            .Init("Mummy",
                new State(
                    new Prioritize(
                        new Protect(10, "Lich", protectRange: 10),
                        new Chase(12, range: 7),
                        new Wander()
                        ),
                    new Shoot(10)
                    ),
                new ItemLoot("Magic Potion", 0.02),
                new ItemLoot("Spirit Salve Tome", 0.02)
            )

            .Init("Mummy King",
                new State(
                    new Prioritize(
                        new Protect(10, "Lich", protectRange: 10),
                        new Chase(12, range: 7),
                        new Wander()
                        ),
                    new Shoot(10)
                    ),
                new ItemLoot("Magic Potion", 0.02),
                new ItemLoot("Spirit Salve Tome", 0.02)
            )

            .Init("Mummy Pharaoh",
                new State(
                    new Prioritize(
                        new Protect(10, "Lich", protectRange: 10),
                        new Chase(12, range: 7),
                        new Wander()
                        ),
                    new Shoot(10)
                    ),
                new ItemLoot("Hell's Fire Wand", 0.02),
                new ItemLoot("Slayer Staff", 0.02),
                new ItemLoot("Golden Sword", 0.02),
                new ItemLoot("Golden Dagger", 0.02)
            )
        ;
    }
}