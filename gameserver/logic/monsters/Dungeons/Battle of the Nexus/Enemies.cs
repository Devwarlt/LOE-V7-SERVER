using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.loot;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ BattlefortheNexus = () => Behav()
            .Init("Oryx the Mad God Deux",
                new State(
                    new State("Nope",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new EntityNotExistsTransition("NM Green Dragon God Deux", 999, "Attack")
                        ),
                    new State("Attack",
                        new Wander(.05),
                        new Shoot(25, index: 0, shoots: 8, shootAngle: 45, coolDown: 1500, coolDownOffset: 1500),
                        new Shoot(25, index: 1, shoots: 3, shootAngle: 10, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, index: 2, shoots: 3, shootAngle: 10, aim: 0.2, coolDown: 1000,
                            coolDownOffset: 1000),
                        new Shoot(25, index: 3, shoots: 2, shootAngle: 10, aim: 0.4, coolDown: 1000,
                            coolDownOffset: 1000),
                        new Shoot(25, index: 4, shoots: 3, shootAngle: 10, aim: 0.6, coolDown: 1000,
                            coolDownOffset: 1000),
                        new Shoot(25, index: 5, shoots: 2, shootAngle: 10, aim: 0.8, coolDown: 1000,
                            coolDownOffset: 1000),
                        new Shoot(25, index: 6, shoots: 3, shootAngle: 10, aim: 1, coolDown: 1000,
                            coolDownOffset: 1000),
                        new Taunt(1, 6000, "Puny mortals! My {HP} HP will annihilate you!"),
                        new Spawn("Henchman of Oryx", 5, coolDown: 5000),
                        new HpLessTransition(.2, "prepareRage")
                    ),
                    new State("prepareRage",
                        new Chase(.1, 15, 3),
                        new Taunt("Can't... keep... henchmen... alive... anymore! ARGHHH!!!"),
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Shoot(25, 30, angleOffset: 0, index: 7, coolDown: 4000, coolDownOffset: 4000),
                        new Shoot(25, 30, angleOffset: 30, index: 8, coolDown: 4000, coolDownOffset: 4000),
                        new TimedTransition(10000, "rage")
                    ),
                    new State("rage",
                        new Chase(.1, 15, 3),
                        new Shoot(25, 30, index: 7, coolDown: 90000001, coolDownOffset: 8000),
                        new Shoot(25, 30, index: 8, coolDown: 90000001, coolDownOffset: 8500),
                        new Shoot(25, index: 0, shoots: 8, shootAngle: 45, coolDown: 1500, coolDownOffset: 1500),
                        new Shoot(25, index: 1, shoots: 3, shootAngle: 10, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, index: 2, shoots: 3, shootAngle: 10, aim: 0.2, coolDown: 1000,
                            coolDownOffset: 1000),
                        new Shoot(25, index: 3, shoots: 2, shootAngle: 10, aim: 0.4, coolDown: 1000,
                            coolDownOffset: 1000),
                        new Shoot(25, index: 4, shoots: 3, shootAngle: 10, aim: 0.6, coolDown: 1000,
                            coolDownOffset: 1000),
                        new Shoot(25, index: 5, shoots: 2, shootAngle: 10, aim: 0.8, coolDown: 1000,
                            coolDownOffset: 1000),
                        new Shoot(25, index: 6, shoots: 3, shootAngle: 10, aim: 1, coolDown: 1000,
                            coolDownOffset: 1000),
                        new Taunt(1, 6000, "Puny mortals! My {HP} HP will annihilate you!")
                    )
                ),
                new Threshold(0.1,
                    new ItemLoot("Sunshine Shiv", 0.02),
                    new ItemLoot("Spicy Wand of Spice", 0.02),
                    new ItemLoot("Doctor Swordsworth", 0.02),
                    new ItemLoot("KoalaPOW", 0.02),
                    new ItemLoot("Robobow", 0.02)
                )
              )
            .Init("NM Green Dragon God Deux",
                new State(
                    new SetAltTexture(1),
                    new StayCloseToSpawn(0.5, 24),
                    new HpLessTransition(0.05, "19"),
                    new State("Nope",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new EntityNotExistsTransition("Archdemon Malphas Deux", 999, "Idle")
                        ),
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new State("1",
                            new PlayerWithinTransition(3, "3")
                            ),
                        new State("3",
                            new SetAltTexture(0),
                            new TimedTransition(1000, "4")
                            ),
                        new State("4",
                            new TossObject("NM Green Dragon Shield", 3, 0),
                            new TossObject("NM Green Dragon Shield", 3, 45),
                            new TossObject("NM Green Dragon Shield", 3, 90),
                            new TossObject("NM Green Dragon Shield", 3, 135),
                            new TossObject("NM Green Dragon Shield", 3, 180),
                            new TossObject("NM Green Dragon Shield", 3, 225),
                            new TossObject("NM Green Dragon Shield", 3, 270),
                            new TossObject("NM Green Dragon Shield", 3, 315),
                            new TimedTransition(0, "5")
                            ),
                        new State("5",
                            new EntityExistsTransition("NM Green Dragon Shield", 999, "6")
                            ),
                        new State("6",
                            new EntitiesNotExistsTransition(999, "7", "NM Green Dragon Shield"),
                            new Shoot(99, 12, 30, 0, 0, 45, coolDown: 1500),
                            new Shoot(99, 2, 15, 6, 0, 0, coolDown: 1000),
                            new Shoot(99, 2, 15, 6, 0, 90, coolDown: 1000),
                            new Shoot(99, 2, 15, 6, 0, 180, coolDown: 1000),
                            new Shoot(99, 2, 15, 6, 0, 270, coolDown: 1000),
                            new Shoot(99, 10, 36, 5, 0, 10, coolDown: 2000),
                            new Shoot(20, 2, 15, 4, coolDown: 1000)
                            )
                        ),
                    new State("7",
                        new HpLessTransition(0.5, "8"),
                        new Chase(0.4, 20, 1),
                        new Spawn("NM Green Dragon Minion", 5, 0),
                        new Shoot(99, 12, 30, 0, 0, 45, coolDown: 1500),
                        new Shoot(99, 2, 15, 6, 0, 0, coolDown: 1000),
                        new Shoot(99, 2, 15, 6, 0, 90, coolDown: 1000),
                        new Shoot(99, 2, 15, 6, 0, 180, coolDown: 1000),
                        new Shoot(99, 2, 15, 6, 0, 270, coolDown: 1000),
                        new Shoot(99, 10, 36, 5, 0, 10, coolDown: 2000),
                        new Shoot(20, 2, 15, 4, coolDown: 1000)
                        ),
                    new State("8",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new State("9",
                            new ReturnToSpawn(true, 0.7),
                            new Flashing(0xFFFFFF, 0.2, 12),
                            new TimedTransition(4000, "10")
                            ),
                        new State("10",
                            new TossObject("NM Green Dragon Shield", 3, 0),
                            new TossObject("NM Green Dragon Shield", 3, 45),
                            new TossObject("NM Green Dragon Shield", 3, 90),
                            new TossObject("NM Green Dragon Shield", 3, 135),
                            new TossObject("NM Green Dragon Shield", 3, 180),
                            new TossObject("NM Green Dragon Shield", 3, 225),
                            new TossObject("NM Green Dragon Shield", 3, 270),
                            new TossObject("NM Green Dragon Shield", 3, 315),
                            new TimedTransition(0, "11")
                            ),
                        new State("11",
                            new EntityExistsTransition("NM Green Dragon Shield", 999, "12")
                            ),
                        new State("12",
                            new EntitiesNotExistsTransition(999, "18", "NM Green Dragon Shield"),
                            new Shoot(99, 2, 15, 6, 0, 0, coolDown: 1000),
                            new Shoot(99, 2, 15, 6, 0, 90, coolDown: 1000),
                            new Shoot(99, 2, 15, 6, 0, 180, coolDown: 1000),
                            new Shoot(99, 2, 15, 6, 0, 270, coolDown: 1000),
                            new Shoot(99, 10, 36, 5, 0, 10, coolDown: 2000),
                            new Shoot(20, 2, 15, 4, coolDown: 1000),
                            new State("13",
                                new Shoot(99, 12, 30, 0, 0, 45, coolDown: 1500),
                                new TimedTransition(4500, "14")
                                ),
                            new State("14",
                                new Shoot(99, 4, 90, 0, 0, 45, coolDownOffset: 200),
                                new Shoot(99, 4, 90, 0, 0, 52, coolDownOffset: 400),
                                new Shoot(99, 4, 90, 0, 0, 59, coolDownOffset: 600),
                                new Shoot(99, 4, 90, 0, 0, 66, coolDownOffset: 800),
                                new Shoot(99, 4, 90, 0, 0, 73, coolDownOffset: 1000),
                                new Shoot(99, 4, 90, 0, 0, 80, coolDownOffset: 1200),
                                new Shoot(99, 4, 90, 0, 0, 87, coolDownOffset: 1400),
                                new Shoot(99, 4, 90, 0, 0, 90, coolDownOffset: 1600),
                                new TimedTransition(1600, "15")
                                ),
                            new State("15",
                                new Shoot(99, 4, 90, 0, 0, 90, coolDown: 200),
                                new TimedTransition(400, "16")
                                ),
                            new State("16",
                                new Shoot(99, 4, 90, 0, 0, 90, coolDownOffset: 200),
                                new Shoot(99, 4, 90, 0, 0, 87, coolDownOffset: 400),
                                new Shoot(99, 4, 90, 0, 0, 80, coolDownOffset: 600),
                                new Shoot(99, 4, 90, 0, 0, 73, coolDownOffset: 800),
                                new Shoot(99, 4, 90, 0, 0, 66, coolDownOffset: 1000),
                                new Shoot(99, 4, 90, 0, 0, 59, coolDownOffset: 1200),
                                new Shoot(99, 4, 90, 0, 0, 52, coolDownOffset: 1400),
                                new Shoot(99, 4, 90, 0, 0, 45, coolDownOffset: 1600),
                                new TimedTransition(1600, "17")
                                ),
                            new State("17",
                                new Shoot(99, 4, 90, 0, 0, 45, coolDown: 200),
                                new TimedTransition(400, "13")
                                )
                            )
                        ),
                    new State("18",
                        new Chase(0.4, 20, 1),
                        new Spawn("NM Green Dragon Minion", 5, 0),
                        new Shoot(99, 12, 30, 0, 0, 45, coolDown: 1500),
                        new Shoot(99, 2, 15, 6, 0, 0, coolDown: 1000),
                        new Shoot(99, 2, 15, 6, 0, 90, coolDown: 1000),
                        new Shoot(99, 2, 15, 6, 0, 180, coolDown: 1000),
                        new Shoot(99, 2, 15, 6, 0, 270, coolDown: 1000),
                        new Shoot(99, 10, 36, 5, 0, 10, coolDown: 2000),
                        new Shoot(20, 2, 15, 4, coolDown: 1000)
                        ),
                    new State("19",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new State("20",
                            new ReturnToSpawn(true, 0.7),
                            new Taunt(true, "Flee my servants, I can no longer protect you as you have protected me..."),
                            new TimedTransition(3000, "21")
                            ),
                        new State("21",
                            new Suicide()
                    ))
                ),
                new Threshold(0.1,
                    new ItemLoot("Sunshine Shiv", 0.02),
                    new ItemLoot("Spicy Wand of Spice", 0.02),
                    new ItemLoot("Doctor Swordsworth", 0.02),
                    new ItemLoot("KoalaPOW", 0.02),
                    new ItemLoot("Robobow", 0.02)
                )
              )
               .Init("Archdemon Malphas Deux",
                new State(
                    new State("Nope",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new EntityNotExistsTransition("Murderous Megamoth Deux", 999, "default")
                        ),
                    new State("default",
                        new PlayerWithinTransition(12, "basic")
                        ),
                    new State("basic",
                        new Prioritize(
                            new Chase(0.3),
                            new Wander(2)
                            ),
                        new Reproduce("Malphas Missile", max: 1, radius: 1, coolDown: 1800),
                        new Spawn(children: "Malphas Protector", maxChildren: 3),
                        new Shoot(12, aim: 1, coolDown: 900),
                        new TimedTransition(11500, "shrink"),
                        new State("1",
                            new AddCond(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(2500, "2")
                            ),
                        new State("2")
                        ),
                    new State("shrink",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Wander(4),
                        new ChangeSize(-15, 25),
                        new TimedTransition(1000, "smallAttack")
                        ),
                    new State("smallAttack",
                        new Prioritize(
                            new Chase(1, sightRange: 15, range: 8),
                            new Wander(12)
                            ),
                        new Shoot(10, aim: 1, coolDown: 900),
                        new Shoot(10, 6, index: 1, aim: 1, coolDown: 1200),
                        new TimedTransition(11000, "grow")
                        ),
                    new State("grow",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Wander(1),
                        new ChangeSize(35, 200),
                        new TimedTransition(1400, "bigAttack")
                        ),
                    new State("bigAttack",
                        new Prioritize(
                            new Chase(0.2),
                            new Wander(1)
                            ),
                        new Shoot(10, index: 2, aim: 1, coolDown: 700),
                        new Shoot(10, 3, index: 3, aim: 1, coolDown: 900),
                        new TimedTransition(11600, "normalize"),
                        new State("WaitToSetShield1",
                            new TimedTransition(1400, "SetShield1")
                            ),
                        new State("SetShield1",
                            new AddCond(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(1400, "WaitToSetShield2")
                            ),
                        new State("WaitToSetShield2",
                            new TimedTransition(1400, "SetShield2")
                            ),
                        new State("SetShield2",
                            new AddCond(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(1400, "WaitToSetShield3")
                            ),
                        new State("WaitToSetShield3",
                            new TimedTransition(1400, "SetShield3")
                            ),
                        new State("SetShield3",
                            new AddCond(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(1400, "WaitToSetShield4")
                            ),
                        new State("WaitToSetShield4",
                            new TimedTransition(1400, "SetShield4")
                            ),
                        new State("SetShield4",
                            new AddCond(ConditionEffectIndex.Invulnerable)
                            )
                        ),
                    new State("normalize",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Wander(3),
                        new ChangeSize(-20, 100),
                        new TimedTransition(3200, "spawnMalphasFlamer")
                        ),
                    new State("spawnMalphasFlamer",
                        new Prioritize(
                            new Chase(0.3),
                            new Wander(2)
                            ),
                        new Spawn(children: "Malphas Protector", maxChildren: 4),
                        new Spawn(children: "Malphas Flamer", maxChildren: 5),
                        new Shoot(12, aim: 1, coolDown: 3200),
                        new TimedTransition(8000, "basic")
                        )
                    ),
                 new Threshold(1.0,
                    new ItemLoot("Sunshine Shiv", 0.02),
                    new ItemLoot("Spicy Wand of Spice", 0.02),
                    new ItemLoot("Doctor Swordsworth", 0.02),
                    new ItemLoot("KoalaPOW", 0.02),
                    new ItemLoot("Robobow", 0.02)
                    )
            )
            .Init("Murderous Megamoth Deux",
             new State(
                 new State("idle",
                     new Wander(0.2),
                     new Chase(5.0, 10, coolDown: 0),
                     new Spawn("Mini Larva", maxChildren: 10, initialSpawn: 0),
                     new Reproduce("Mini Larva", coolDown: 500, max: 20, range: 99, radius: 0),
                     new Shoot(25, index: 0, shoots: 2, shootAngle: 10, coolDown: 500, coolDownOffset: 500),
                     new Shoot(25, index: 1, shoots: 1, shootAngle: 0, coolDown: 1, coolDownOffset: 1)
                     )
                 ),
                new Threshold(0.01,
                    new ItemLoot("Sunshine Shiv", 0.02),
                    new ItemLoot("Spicy Wand of Spice", 0.02),
                    new ItemLoot("Doctor Swordsworth", 0.02),
                    new ItemLoot("KoalaPOW", 0.02),
                    new ItemLoot("Robobow", 0.02)
                        )
            );
    }
}
