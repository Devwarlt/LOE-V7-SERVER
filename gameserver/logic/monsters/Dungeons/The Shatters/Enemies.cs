#region
using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.loot;
using LoESoft.GameServer.logic.transitions;
#endregion
//credits: mike, as always.
namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ Shatters = () => Behav()
        #region restofmobs
            .Init("shtrs Stone Paladin",
                new State(
                    new State("Idle",
                        new Prioritize(
                            new Wander(8)
                            ),
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Reproduce(max: 4),
                        new PlayerWithinTransition(8, "Attacking")
                        ),
                    new State("Attacking",
                        new State("Bullet",
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 90, coolDownOffset: 0, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 100, coolDownOffset: 200, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 110, coolDownOffset: 400, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 120, coolDownOffset: 600, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 130, coolDownOffset: 800, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 140, coolDownOffset: 1000, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 150, coolDownOffset: 1200, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 160, coolDownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 170, coolDownOffset: 1600, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 180, coolDownOffset: 1800, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 180, coolDownOffset: 2000, shootAngle: 45),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 180, coolDownOffset: 0, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 170, coolDownOffset: 200, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 160, coolDownOffset: 400, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 150, coolDownOffset: 600, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 140, coolDownOffset: 800, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 130, coolDownOffset: 1000, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 120, coolDownOffset: 1200, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 110, coolDownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 100, coolDownOffset: 1600, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 90, coolDownOffset: 1800, shootAngle: 90),
                            new Shoot(1, 4, coolDown: 10000, angleOffset: 90, coolDownOffset: 2000, shootAngle: 22.5),
                            new TimedTransition(2000, "Wait")
                            ),
                        new State("Wait",
                            new Chase(0.4, range: 2),
                            new Flashing(0xff00ff00, 0.1, 20),
                            new AddCond(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(2000, "Bullet")
                            ),
                        new NoPlayerWithinTransition(13, "Idle")
                        )
                    )
            )
            .Init("shtrs Stone Knight",
            new State(
                new State("Follow",
                        new Chase(0.6, 10, 5),
                        new PlayerWithinTransition(5, "Charge")
                    ),
                    new State("Charge",
                        new TimedTransition(2000, "Follow"),
                        new Charge(4, 5),
                        new Shoot(5, 6, index: 0, coolDown: 3000)
                        )
                    )
            )
        .Init("shtrs Lava Souls",
                new State(
                    new State("active",
                        new Chase(.7, range: 0),
                        new PlayerWithinTransition(2, "blink")
                    ),
                    new State("blink",
                        new Flashing(0xfFF0000, flashRepeats: 10000, flashPeriod: 0.1),
                        new TimedTransition(2000, "explode")
                    ),
                    new State("explode",
                        new Flashing(0xfFF0000, flashRepeats: 5, flashPeriod: 0.1),
                        new Shoot(5, 9),
                        new Suicide()
                    )
                )
            )
            .Init("shtrs Glassier Archmage",
            new State(
                    new Retreat(0.5, 5),
                new State("1st",
                    new Chase(0.8, range: 7),
                    new Shoot(20, index: 2, shoots: 1, coolDown: 50),
                    new TimedTransition(5000, "next")
                    ),
                new State("next",
                    new Shoot(35, index: 0, shoots: 25, coolDown: 5000),
                    new TimedTransition(25, "1st")
                    )
               )
        )
            .Init("shtrs Ice Adept",
            new State(
                new State("Main",
                    new TimedTransition(5000, "Throw"),
                    new Chase(0.8, range: 1),
                    new Shoot(10, 1, index: 0, coolDown: 100, aim: 1),
                    new Shoot(10, 3, index: 1, shootAngle: 10, coolDown: 4000, aim: 1)
                ),
                new State("Throw",
                    new TossObject("shtrs Ice Portal", 5, coolDown: 8000, coolDownOffset: 7000, randomToss: false),
                    new TimedTransition(1000, "Main")
                )
                  ))

            .Init("shtrs Fire Adept",
            new State(
                new State("Main",
                    new TimedTransition(5000, "Throw"),
                    new Chase(0.8, range: 1),
                    new Shoot(10, 1, index: 0, coolDown: 100, aim: 1),
                    new Shoot(10, 3, index: 1, shootAngle: 10, coolDown: 4000, aim: 1)
                ),
                new State("Throw",
                    new TossObject("shtrs Fire Portal", 5, coolDown: 8000, coolDownOffset: 7000, randomToss: true),
                    new TimedTransition(1000, "Main")
                )
                  ))
        #endregion restofmobs
        #region generators
            .Init("shtrs MagiGenerators",
            new State(
                new State("Main",
                    new AddCond(ConditionEffectIndex.Invulnerable),
                    new Shoot(15, 10, coolDown: 1000),
                    new Shoot(15, 1, index: 1, coolDown: 2500),
                    new EntitiesNotExistsTransition(30, "Hide", "Shtrs Twilight Archmage", "shtrs Inferno", "shtrs Blizzard")
                ),
                new State("Hide",
                    new SetAltTexture(1),
                    new AddCond(ConditionEffectIndex.Invulnerable)
                    ),
                new State("Despawn",
                    new Decay()
                    )
                  ))
        #endregion generators
        #region portals
            .Init("shtrs Ice Portal",
                new State(
                    new State("Idle",
                        new TimedTransition(1000, "Spin")
                    ),
                    new State("Spin",
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 0, coolDown: 1200),
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 15, coolDown: 1200, coolDownOffset: 200),
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 30, coolDown: 1200, coolDownOffset: 400),
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 45, coolDown: 1200, coolDownOffset: 600),
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 60, coolDown: 1200, coolDownOffset: 800),
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 75, coolDown: 1200, coolDownOffset: 1000),
                            new TimedTransition(1200, "Pause")
                    ),
                    new State("Pause",
                       new TimedTransition(5000, "Idle")
                    )
                )
            )
            .Init("shtrs Fire Portal",
                new State(
                    new State("Idle",
                        new TimedTransition(1000, "Spin")
                    ),
                    new State("Spin",
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 0, coolDown: 1200),
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 15, coolDown: 1200, coolDownOffset: 200),
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 30, coolDown: 1200, coolDownOffset: 400),
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 45, coolDown: 1200, coolDownOffset: 600),
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 60, coolDown: 1200, coolDownOffset: 800),
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 75, coolDown: 1200, coolDownOffset: 1000),
                            new TimedTransition(1200, "Pause")
                    ),
                    new State("Pause",
                       new TimedTransition(5000, "Idle")
                    )
                )
            )
            .Init("shtrs Ice Shield",
                new State(
                    new HpLessTransition(.2, "Death"),
                    new State(
                        new Charge(0.6, 7, coolDown: 5000),
                        new Shoot(3, 6, 60, index: 0, angleOffset: 0, coolDown: 1200),
                        new Shoot(3, 6, 60, index: 0, angleOffset: 10, coolDown: 1200, coolDownOffset: 200),
                        new Shoot(3, 6, 60, index: 0, angleOffset: 20, coolDown: 1200, coolDownOffset: 400),
                        new Shoot(3, 6, 60, index: 0, angleOffset: 30, coolDown: 1200, coolDownOffset: 600),
                        new Shoot(3, 6, 60, index: 0, angleOffset: 40, coolDown: 1200, coolDownOffset: 800),
                        new Shoot(3, 6, 60, index: 0, angleOffset: 50, coolDown: 1200, coolDownOffset: 1000)
                    ),
                    new State("Death",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Shoot(13, 45, 8, index: 1, angleOffset: 1, coolDown: 10000),
                        new Timed(1000, new Suicide())
                    )
                )
            )
            .Init("shtrs Ice Shield 2",
            new State(
                new HpLessTransition(0.3, "Death"),
                new State(
                    new Circle(0.5, 5, 1, "shtrs Twilight Archmage"),
                    new Charge(0.1, 6, coolDown: 10000),
                new Shoot(13, 10, 8, index: 0, coolDown: 1000, angleOffset: 1)
                ),
            new State("Death",
                new AddCond(ConditionEffectIndex.Invincible),
                new Shoot(13, 45, index: 1, coolDown: 10000),
                new Timed(1000, new Suicide())
                )
                )
            )
        #endregion portals
        #region 1stboss
            .Init("shtrs Bridge Sentinel",
                new State(
                    new Shoot(2, index: 5, shoots: 3, angleOffset: 0, coolDown: 10),
                    new Shoot(2, index: 5, shoots: 3, angleOffset: 45, coolDown: 10),
                    new Shoot(2, index: 5, shoots: 3, angleOffset: 90, coolDown: 10),
                    new Shoot(2, index: 5, shoots: 3, angleOffset: 135, coolDown: 10),
                    new Shoot(2, index: 5, shoots: 3, angleOffset: 180, coolDown: 10),
                    new HpLessTransition(0.1, "Death"),
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(15, "Close Bridge")
                        ),
                    new State("Close Bridge",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityOrder(46, "shtrs Bridge Closer", "Closer"),
                        new TimedTransition(5000, "Close Bridge2")
                        ),
                    new State("Close Bridge2",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityOrder(46, "shtrs Bridge Closer2", "Closer"),
                        new TimedTransition(5000, "Close Bridge3")
                        ),
                    new State("Close Bridge3",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityOrder(46, "shtrs Bridge Closer3", "Closer"),
                        new TimedTransition(5000, "Close Bridge4")
                        ),
                    new State("Close Bridge4",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityOrder(46, "shtrs Bridge Closer4", "Closer"),
                        new TimedTransition(6000, "BEGIN")
                        ),
                    new State("BEGIN",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(30, "Wake", "shtrs Bridge Obelisk A", "shtrs Bridge Obelisk B", "shtrs Bridge Obelisk D", "shtrs Bridge Obelisk E")
                    ),
                        new State("Wake",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Taunt("Who has woken me...? Leave this place."),
                        new Timed(2100, new Shoot(15, 15, 12, index: 0, angleOffset: 180, coolDown: 700, coolDownOffset: 3000)),
                        new TimedTransition(8000, "Swirl Shot")
                        ),
                        new State("Swirl Shot",
                            new Taunt("Go."),
                            new TimedTransition(10000, "Blobomb"),
                            new State("Swirl1",
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 102, angleOffset: 102, coolDown: 6000),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 114, angleOffset: 114, coolDown: 6000, coolDownOffset: 200),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 126, angleOffset: 126, coolDown: 6000, coolDownOffset: 400),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 138, angleOffset: 138, coolDown: 6000, coolDownOffset: 600),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 150, angleOffset: 150, coolDown: 6000, coolDownOffset: 800),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 162, angleOffset: 162, coolDown: 6000, coolDownOffset: 1000),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 174, angleOffset: 174, coolDown: 6000, coolDownOffset: 1200),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 186, angleOffset: 186, coolDown: 6000, coolDownOffset: 1400),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 198, angleOffset: 198, coolDown: 6000, coolDownOffset: 1600),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 210, angleOffset: 210, coolDown: 6000, coolDownOffset: 1800),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 222, angleOffset: 222, coolDown: 6000, coolDownOffset: 2000),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 234, angleOffset: 234, coolDown: 6000, coolDownOffset: 2200),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 246, angleOffset: 246, coolDown: 6000, coolDownOffset: 2400),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 258, angleOffset: 258, coolDown: 6000, coolDownOffset: 2600),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 270, angleOffset: 270, coolDown: 6000, coolDownOffset: 2800),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 282, angleOffset: 282, coolDown: 6000, coolDownOffset: 3000),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 270, angleOffset: 270, coolDown: 6000, coolDownOffset: 3200),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 258, angleOffset: 258, coolDown: 6000, coolDownOffset: 3400),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 246, angleOffset: 246, coolDown: 6000, coolDownOffset: 3600),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 234, angleOffset: 234, coolDown: 6000, coolDownOffset: 3800),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 222, angleOffset: 222, coolDown: 6000, coolDownOffset: 4000),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 210, angleOffset: 210, coolDown: 6000, coolDownOffset: 4200),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 198, angleOffset: 198, coolDown: 6000, coolDownOffset: 4400),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 186, angleOffset: 186, coolDown: 6000, coolDownOffset: 4600),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 174, angleOffset: 174, coolDown: 6000, coolDownOffset: 4800),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 162, angleOffset: 162, coolDown: 6000, coolDownOffset: 5000),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 150, angleOffset: 150, coolDown: 6000, coolDownOffset: 5200),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 138, angleOffset: 138, coolDown: 6000, coolDownOffset: 5400),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 126, angleOffset: 126, coolDown: 6000, coolDownOffset: 5600),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 114, angleOffset: 114, coolDown: 6000, coolDownOffset: 5800),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 102, angleOffset: 102, coolDown: 6000, coolDownOffset: 6000),
                            new TimedTransition(6000, "Swirl1")
                            )
                            ),
                            new State("Blobomb",
                            new Taunt("You live still? DO NOT TEMPT FATE!"),
                            new Taunt("CONSUME!"),
                            new EntityOrder(20, "shtrs blobomb maker", "Spawn"),
                            new EntityNotExistsTransition("shtrs Blobomb", 30, "SwirlAndShoot")
                                ),
                                new State("SwirlAndShoot",
                                    new TimedTransition(10000, "Blobomb"),
                                    new Taunt("FOOLS! YOU DO NOT UNDERSTAND!"),
                                    new ChangeSize(20, 130),
                            new Shoot(15, 15, 11, index: 0, angleOffset: 180, coolDown: 700, coolDownOffset: 700),
                                    new State("Swirl1_2",
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 102, angleOffset: 102, coolDown: 6000),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 114, angleOffset: 114, coolDown: 6000, coolDownOffset: 200),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 126, angleOffset: 126, coolDown: 6000, coolDownOffset: 400),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 138, angleOffset: 138, coolDown: 6000, coolDownOffset: 600),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 150, angleOffset: 150, coolDown: 6000, coolDownOffset: 800),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 162, angleOffset: 162, coolDown: 6000, coolDownOffset: 1000),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 174, angleOffset: 174, coolDown: 6000, coolDownOffset: 1200),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 186, angleOffset: 186, coolDown: 6000, coolDownOffset: 1400),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 198, angleOffset: 198, coolDown: 6000, coolDownOffset: 1600),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 210, angleOffset: 210, coolDown: 6000, coolDownOffset: 1800),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 222, angleOffset: 222, coolDown: 6000, coolDownOffset: 2000),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 234, angleOffset: 234, coolDown: 6000, coolDownOffset: 2200),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 246, angleOffset: 246, coolDown: 6000, coolDownOffset: 2400),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 258, angleOffset: 258, coolDown: 6000, coolDownOffset: 2600),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 270, angleOffset: 270, coolDown: 6000, coolDownOffset: 2800),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 282, angleOffset: 282, coolDown: 6000, coolDownOffset: 3000),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 270, angleOffset: 270, coolDown: 6000, coolDownOffset: 3200),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 258, angleOffset: 258, coolDown: 6000, coolDownOffset: 3400),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 246, angleOffset: 246, coolDown: 6000, coolDownOffset: 3600),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 234, angleOffset: 234, coolDown: 6000, coolDownOffset: 3800),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 222, angleOffset: 222, coolDown: 6000, coolDownOffset: 4000),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 210, angleOffset: 210, coolDown: 6000, coolDownOffset: 4200),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 198, angleOffset: 198, coolDown: 6000, coolDownOffset: 4400),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 186, angleOffset: 186, coolDown: 6000, coolDownOffset: 4600),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 174, angleOffset: 174, coolDown: 6000, coolDownOffset: 4800),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 162, angleOffset: 162, coolDown: 6000, coolDownOffset: 5000),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 150, angleOffset: 150, coolDown: 6000, coolDownOffset: 5200),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 138, angleOffset: 138, coolDown: 6000, coolDownOffset: 5400),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 126, angleOffset: 126, coolDown: 6000, coolDownOffset: 5600),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 114, angleOffset: 114, coolDown: 6000, coolDownOffset: 5800),
                            new Shoot(50, index: 0, shoots: 1, shootAngle: 102, angleOffset: 102, coolDown: 6000, coolDownOffset: 6000),
                            new TimedTransition(6000, "Swirl1_2")
                            )
                                ),
                        new State("Death",
                            new AddCond(ConditionEffectIndex.Invulnerable),
                            new CopyDamageOnDeath("shtrs Loot Balloon Bridge"),
                            new Taunt("I tried to protect you... I have failed. You release a great evil upon this realm...."),
                            new TimedTransition(2000, "Suicide")
                            ),
                        new State("Suicide",
                            new Shoot(35, index: 0, shoots: 30),
                            new EntityOrder(1, "shtrs Chest Spawner 1", "Open"),
                            new EntityOrder(46, "shtrs Spawn Bridge", "Open"),
                            new Suicide()
                    )
                )
            )
        #endregion 1stboss
        #region blobomb
            .Init("shtrs Blobomb",
                new State(
                    new State("active",
                        new Chase(.7, range: 0),
                        new PlayerWithinTransition(2, "blink")
                    ),
                    new State("blink",
                        new Flashing(0xfFF0000, flashRepeats: 10000, flashPeriod: 0.1),
                        new TimedTransition(2000, "explode")
                    ),
                    new State("explode",
                        new Flashing(0xfFF0000, flashRepeats: 5, flashPeriod: 0.1),
                        new Shoot(30, 36, angleOffset: 0),
                        new Suicide()
                    )
                )
            )
        #endregion blobomb
        #region 2ndboss
            .Init("shtrs Twilight Archmage",
                new State(
                    new SetLootState("archmage"),
                    new CopyLootState("shtrs encounterchestspawner", 20),
                    new HpLessTransition(.1, "Death"),
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition2("shtrs Glassier Archmage", "shtrs Archmage of Flame", 15, "Wake")
                    ),
                    new State("Wake",
                        new State("Comment1",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                            new SetAltTexture(1),
                            new Taunt("Ha...ha........hahahahahaha! You will make a fine sacrifice!"),
                            new TimedTransition(3000, "Comment2")
                        ),
                        new SetAltTexture(1),
                        new State("Comment2",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                            new Taunt("You will find that it was...unwise...to wake me."),
                            new TimedTransition(1000, "Comment3")
                        ),
                        new State("Comment3",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                            new SetAltTexture(1),
                            new Taunt("Let us see what can conjure up!"),
                            new TimedTransition(1000, "Comment4")
                        ),
                        new State("Comment4",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                            new SetAltTexture(1),
                            new Taunt("I will freeze the life from you!"),
                            new TimedTransition(1000, "Shoot")
                        )
                    ),
                    new State("TossShit",
                        new TossObject("shtrs Ice Portal", 10, coolDown: 25000, randomToss: false),
                        new TossObject("shtrs FireBomb", 15, coolDown: 25000, randomToss: true),
                        new TossObject("shtrs FireBomb", 15, coolDown: 25000, randomToss: true),
                        new TossObject("shtrs FireBomb", 7, coolDown: 25000, randomToss: true),
                        new TossObject("shtrs FireBomb", 1, coolDown: 25000, randomToss: true),
                        new TossObject("shtrs FireBomb", 4, coolDown: 25000, randomToss: true),
                        new TossObject("shtrs FireBomb", 8, coolDown: 25000, randomToss: true),
                        new TossObject("shtrs FireBomb", 9, coolDown: 25000, randomToss: true),        //NOT IN USE!
                        new TossObject("shtrs FireBomb", 5, coolDown: 25000, randomToss: true),
                        new TossObject("shtrs FireBomb", 7, coolDown: 25000, randomToss: true),
                        new TossObject("shtrs FireBomb", 11, coolDown: 25000, randomToss: true),
                        new TossObject("shtrs FireBomb", 13, coolDown: 25000, randomToss: true),
                        new TossObject("shtrs FireBomb", 12, coolDown: 25000, randomToss: true),
                        new TossObject("shtrs FireBomb", 10, coolDown: 25000, randomToss: true),
                        new Spawn("shtrs Ice Shield 2", maxChildren: 1, initialSpawn: 1, coolDown: 25000),
                        new TimedTransition(1, "Shoot")
                        ),
                  new State("Shoot",
                    new Shoot(15, 5, 5, index: 1, coolDown: 800),
                    new Shoot(15, 5, 5, index: 1, coolDown: 800, coolDownOffset: 200),
                    new Shoot(15, 5, 5, index: 1, coolDown: 800, coolDownOffset: 400),
                    new Shoot(15, 5, 5, index: 1, coolDown: 800, coolDownOffset: 600),
                    new Shoot(15, 5, 5, index: 1, coolDown: 800, coolDownOffset: 800),
                    new TimedTransition(800, "Shoot"),
                    new HpLessTransition(0.50, "Pre Birds")
                        ),
                    new State("Pre Birds",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Taunt("You leave me no choice...Inferno! Blizzard!"),
                        new TimedTransition(2000, "Birds")
                        ),
                    new State("Birds",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Spawn("shtrs Inferno", maxChildren: 1, initialSpawn: 1, coolDown: 1000000000),
                        new Spawn("shtrs Blizzard", maxChildren: 1, initialSpawn: 1, coolDown: 1000000000),
                        new EntitiesNotExistsTransition(500, "PreNewShit2", "shtrs Inferno", "shtrs Blizzard")
                        ),
                    new State("PreNewShit2",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3000, "NewShit2")
                        ),
                    new State("NewShit2",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new MoveTo(0, -6, 1),
                        new TimedTransition(3000, "Active2")
                        ),
                    new State("Active2",
                        new Taunt("THE POWER...IT CONSUMES ME!"),
                        new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 100),
                        new Shoot(15, 20, index: 3, coolDown: 100000000, coolDownOffset: 200),
                        new Shoot(15, 20, index: 4, coolDown: 100000000, coolDownOffset: 300),
                        new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 400),
                        new Shoot(15, 20, index: 5, coolDown: 100000000, coolDownOffset: 500),
                        new Shoot(15, 20, index: 6, coolDown: 100000000, coolDownOffset: 600),
                        new TimedTransition(2000, "NewShit3")
                        ),
                    new State("NewShit3",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new MoveTo(4, 0, 1),
                        new TimedTransition(3000, "Active3")
                        ),
                    new State("Active3",
                        new Taunt("THE POWER...IT CONSUMES ME!"),
                        new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 100),
                        new Shoot(15, 20, index: 3, coolDown: 100000000, coolDownOffset: 200),
                        new Shoot(15, 20, index: 4, coolDown: 100000000, coolDownOffset: 300),
                        new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 400),
                        new Shoot(15, 20, index: 5, coolDown: 100000000, coolDownOffset: 500),
                        new Shoot(15, 20, index: 6, coolDown: 100000000, coolDownOffset: 600),
                        new TimedTransition(2000, "NewShit4")
                        ),
                    new State("NewShit4",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new MoveTo(0, 13, 1),
                        new TimedTransition(3000, "Active4")
                            ),
                    new State("Active4",
                        new Taunt("THE POWER...IT CONSUMES ME!"),
                        new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 100),
                        new Shoot(15, 20, index: 3, coolDown: 100000000, coolDownOffset: 200),
                        new Shoot(15, 20, index: 4, coolDown: 100000000, coolDownOffset: 300),
                        new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 400),
                        new Shoot(15, 20, index: 5, coolDown: 100000000, coolDownOffset: 500),
                        new Shoot(15, 20, index: 6, coolDown: 100000000, coolDownOffset: 600),
                        new TimedTransition(2000, "NewShit5")
                        ),
                    new State("NewShit5",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new MoveTo(-4, 0, 1),
                        new TimedTransition(3000, "Active5")
                            ),
                    new State("Active5",
                        new Taunt("THE POWER...IT CONSUMES ME!"),
                        new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 100),
                        new Shoot(15, 20, index: 3, coolDown: 100000000, coolDownOffset: 200),
                        new Shoot(15, 20, index: 4, coolDown: 100000000, coolDownOffset: 300),
                        new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 400),
                        new Shoot(15, 20, index: 5, coolDown: 100000000, coolDownOffset: 500),
                        new Shoot(15, 20, index: 6, coolDown: 100000000, coolDownOffset: 600),
                        new TimedTransition(2000, "NewShit6")
                        ),
                    new State("NewShit6",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new MoveTo(-4, 0, 1),
                        new TimedTransition(3000, "Active6")
                            ),
                    new State("Active6",
                        new Taunt("THE POWER...IT CONSUMES ME!"),
                        new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 100),
                        new Shoot(15, 20, index: 3, coolDown: 100000000, coolDownOffset: 200),
                        new Shoot(15, 20, index: 4, coolDown: 100000000, coolDownOffset: 300),
                        new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 400),
                        new Shoot(15, 20, index: 5, coolDown: 100000000, coolDownOffset: 500),
                        new Shoot(15, 20, index: 6, coolDown: 100000000, coolDownOffset: 600),
                        new TimedTransition(2000, "NewShit7")
                        ),
                    new State("NewShit7",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new MoveTo(0, -13, 1),
                        new TimedTransition(3000, "Active7")
                            ),
                    new State("Active7",
                        new Taunt("THE POWER...IT CONSUMES ME!"),
                        new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 100),
                        new Shoot(15, 20, index: 3, coolDown: 100000000, coolDownOffset: 200),
                        new Shoot(15, 20, index: 4, coolDown: 100000000, coolDownOffset: 300),
                        new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 400),
                        new Shoot(15, 20, index: 5, coolDown: 100000000, coolDownOffset: 500),
                        new Shoot(15, 20, index: 6, coolDown: 100000000, coolDownOffset: 600),
                        new TimedTransition(2000, "Death")
                        ),
                        new State("Death",
                            new AddCond(ConditionEffectIndex.Invulnerable),
                            new Taunt("IM..POSSI...BLE!"),
                            new CopyDamageOnDeath("shtrs Loot Balloon Mage"),
                            new EntityOrder(1, "shtrs Chest Spawner 2", "Open"),
                            new TimedTransition(2000, "Suicide")
                            ),
                        new State("Suicide",
                            new Shoot(35, index: 0, shoots: 30),
                            new Suicide()
                    )
                )
            )
        #endregion 2ndboss
        #region birds
            .Init("shtrs Inferno",
                new State(
                    new Circle(0.5, 4, 15, "shtrs Blizzard"),
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 15, coolDown: 4333),
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 30, coolDown: 3500),
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 60, coolDown: 7250),
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 90, coolDown: 10000)
                )
            )

            .Init("shtrs Blizzard",
                new State(
                    new State("Follow",
                    new Chase(0.3, range: 1, coolDown: 1000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 25),
                            new TimedTransition(7000, "Spin")
                    ),
                    new State("Spin",
                        new State("Quadforce1",
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 0, coolDown: 300),
                            new TimedTransition(10, "Quadforce2")
                        ),
                        new State("Quadforce2",
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 15, coolDown: 300),
                            new TimedTransition(10, "Quadforce3")
                        ),
                        new State("Quadforce3",
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 30, coolDown: 300),
                            new TimedTransition(10, "Quadforce4")
                        ),
                        new State("Quadforce4",
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 45, coolDown: 300),
                            new TimedTransition(10, "Quadforce5")
                        ),
                        new State("Quadforce5",
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 60, coolDown: 300),
                            new TimedTransition(10, "Quadforce6")
                        ),
                        new State("Quadforce6",
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 75, coolDown: 300),
                            new TimedTransition(10, "Quadforce7")
                        ),
                        new State("Quadforce7",
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 90, coolDown: 300),
                            new TimedTransition(10, "Quadforce8")
                        ),
                        new State("Quadforce8",
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 105, coolDown: 300),
                            new TimedTransition(10, "Quadforce9")
                        ),
                        new State("Quadforce9",
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 120, coolDown: 300),
                            new TimedTransition(10, "Quadforce10")
                        ),
                        new State("Quadforce10",
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 135, coolDown: 300),
                            new TimedTransition(10, "Quadforce11")
                        ),
                        new State("Quadforce11",
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 150, coolDown: 300),
                            new TimedTransition(10, "Quadforce12")
                        ),
                        new State("Quadforce12",
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 165, coolDown: 300),
                            new TimedTransition(10, "Quadforce13")
                        ),
                        new State("Quadforce13",
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 180, coolDown: 300),
                            new TimedTransition(10, "Quadforce14")
                        ),
                        new State("Quadforce14",
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 195, coolDown: 300),
                            new TimedTransition(10, "Quadforce15")
                        ),
                        new State("Quadforce15",
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 210, coolDown: 300),
                            new TimedTransition(10, "Quadforce16")
                        ),
                        new State("Quadforce16",
                            new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 225, coolDown: 300),
                            new TimedTransition(10, "Follow")

                            ))
                )
            )
        #endregion birds
        #region 1stbosschest
            .Init("shtrs Loot Balloon Bridge",
                new State(
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "Bridge")
                    ),
                    new State("Bridge")
                ),
                new Threshold(0.1,
                    new TierLoot(11, ItemType.Weapon, 1),
                    new TierLoot(12, ItemType.Weapon, 1),
                    new TierLoot(6, ItemType.Ability, 1),
                    new TierLoot(12, ItemType.Armor, 1),
                    new TierLoot(13, ItemType.Armor, 1),
                    new TierLoot(6, ItemType.Ring, 1),
                new Threshold(0.32,
                    new ItemLoot("Potion of Attack", 1),
                    new ItemLoot("Potion of Defense", 1),
                    new ItemLoot("Bracer of the Guardian", 0.3)
                    )
                )
            )
        #endregion 1stbosschest
        #region 2ndbosschest
            .Init("shtrs Loot Balloon Mage",
                new State(
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "Mage")
                    ),
                    new State("Mage")
                ),
                new Threshold(0.1,
                    new TierLoot(11, ItemType.Weapon, 1),
                    new TierLoot(12, ItemType.Weapon, 1),
                    new TierLoot(6, ItemType.Ability, 1),
                    new TierLoot(12, ItemType.Armor, 1),
                    new TierLoot(13, ItemType.Armor, 1),
                    new TierLoot(6, ItemType.Ring, 1),
                new Threshold(0.32,
                    new ItemLoot("Potion of Mana", 1),
                    new ItemLoot("The Twilight Gemstone", 0.30)
                    )
                )
            )
        #endregion 2ndbosschest
        #region BridgeStatues
            .Init("shtrs Bridge Obelisk A",
                new State(
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Shtrs Bridge Closer4", 100, "TALK")
                        ),
                    new State("TALK",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Taunt("DO NOT WAKE THE BRIDGE GUARDIAN!"),
                        new TimedTransition(2000, "AFK")
                        ),
                    new State("AFK",
                            new AddCond(ConditionEffectIndex.Invulnerable),
                            new Flashing(0x0000FF0C, 0.5, 4),
                            new TimedTransition(2500, "Shoot")
                        ),
                    new State("Shoot",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 10000),
                            new TimedTransition(10000, "Pause")
                        ),
                    new State("Pause",
                        new TimedTransition(7000, "Shoot")
                        )
                    )
            )
            .Init("shtrs Bridge Obelisk B",
                new State(
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Shtrs Bridge Closer4", 100, "TALK")
                        ),
                    new State("TALK",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Taunt("DO NOT WAKE THE BRIDGE GUARDIAN!"),
                        new TimedTransition(2000, "AFK")
                        ),
                    new State("AFK",
                            new AddCond(ConditionEffectIndex.Invulnerable),
                            new Flashing(0x0000FF0C, 0.5, 4),
                            new TimedTransition(2500, "Shoot")
                        ),
                    new State("Shoot",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                                new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 10000),
                            new TimedTransition(10000, "Pause")
                        ),
                    new State("Pause",
                        new TimedTransition(7000, "Shoot")
                        )
                    )
            )
            .Init("shtrs Bridge Obelisk D",
                new State(
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Shtrs Bridge Closer4", 100, "TALK")
                        ),
                    new State("TALK",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Taunt("DO NOT WAKE THE BRIDGE GUARDIAN!"),
                        new TimedTransition(2000, "AFK")
                        ),
                    new State("AFK",
                            new AddCond(ConditionEffectIndex.Invulnerable),
                            new Flashing(0x0000FF0C, 0.5, 4),
                            new TimedTransition(2500, "Shoot")
                        ),
                    new State("Shoot",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 10000),
                            new TimedTransition(10000, "Pause")
                        ),
                    new State("Pause",
                        new TimedTransition(7000, "Shoot")
                        )
                    )
            )
            .Init("shtrs Bridge Obelisk E",
                new State(
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Shtrs Bridge Closer4", 100, "TALK")
                        ),
                    new State("TALK",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Taunt("DO NOT WAKE THE BRIDGE GUARDIAN!"),
                        new TimedTransition(2000, "AFK")
                        ),
                    new State("AFK",
                            new AddCond(ConditionEffectIndex.Invulnerable),
                            new Flashing(0x0000FF0C, 0.5, 4),
                            new TimedTransition(2500, "Shoot")
                        ),
                    new State("Shoot",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                                new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 10000),
                            new TimedTransition(10000, "Pause")
                        ),
                    new State("Pause",
                        new TimedTransition(7000, "Shoot")
                        )
                    )
            )
            .Init("shtrs Bridge Obelisk C",                                                     //YELLOW TOWERS!
                new State(
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new AddCond(ConditionEffectIndex.Armored),
                        new EntityNotExistsTransition("Shtrs Bridge Closer4", 100, "JustKillMe")
                        ),
                    new State("JustKillMe",
                        new AddCond(ConditionEffectIndex.Armored),
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(2000, "AFK")
                        ),
                    new State("AFK",
                        new AddCond(ConditionEffectIndex.Armored),
                            new AddCond(ConditionEffectIndex.Invulnerable),
                            new Flashing(0x0000FF0C, 0.5, 4),
                            new TimedTransition(2500, "Shoot")
                        ),
                    new State("Shoot",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                                new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 10000),
                            new TimedTransition(10000, "Pause")
                        ),
                    new State("Pause",
                        new AddCond(ConditionEffectIndex.Armored),
                        new TimedTransition(7000, "Shoot")
                        )
                    )
            )
            .Init("shtrs Bridge Obelisk F",                                                     //YELLOW TOWERS!
                new State(
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new AddCond(ConditionEffectIndex.Armored),
                        new EntityNotExistsTransition("Shtrs Bridge Closer4", 100, "JustKillMe")
                        ),
                    new State("JustKillMe",
                        new AddCond(ConditionEffectIndex.Armored),
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(2000, "AFK")
                        ),
                    new State("AFK",
                            new AddCond(ConditionEffectIndex.Invulnerable),
                            new AddCond(ConditionEffectIndex.Armored),
                            new Flashing(0x0000FF0C, 0.5, 4),
                            new TimedTransition(2500, "Shoot")
                        ),
                    new State("Shoot",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 1800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 2800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 3800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 4800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 5800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 6800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 7800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 8800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9000),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9200),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9400),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9600),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 9800),
                            new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 10000, coolDownOffset: 10000),
                            new TimedTransition(10000, "Pause")
                        ),
                    new State("Pause",
                        new AddCond(ConditionEffectIndex.Armored),
                        new TimedTransition(7000, "Shoot")
                        )
                    )
            )
        #endregion BridgeStatues
        #region SomeMobs
            .Init("shtrs Titanum",
                new State(
                    new State("Wait",
                        new PlayerWithinTransition(5, "spawn")
                            ),
                    new State("spawn",
                        new Spawn("shtrs Stone Knight", maxChildren: 1, initialSpawn: 1, coolDown: 5000),
                        new Spawn("shtrs Stone Mage", maxChildren: 1, initialSpawn: 1, coolDown: 7500)
                        )
                    )
            )
            .Init("shtrs Paladin Obelisk",
                new State(
                    new State("Wait",
                        new PlayerWithinTransition(5, "spawn")
                            ),
                    new State("spawn",
                        new Spawn("shtrs Stone Paladin", maxChildren: 1, initialSpawn: 1, coolDown: 7500)
                        )
                    )
            )
            .Init("shtrs Ice Mage",
                new State(
                    new State("Wait",
                        new PlayerWithinTransition(5, "fire")
                            ),
                    new State("fire",
                        new Chase(0.5, range: 1),
                        new Shoot(10, 5, 10, index: 0, coolDown: 1500),
                        new TimedTransition(15000, "Spawn")
                        ),
                    new State("Spawn",
                        new Spawn("shtrs Ice Shield", maxChildren: 1, initialSpawn: 1, coolDown: 750000000),
                        new TimedTransition(25, "fire")
                        )
                    )
            )
            .Init("shtrs Archmage of Flame",
            new State(
                new State("wait",
                    new AddCond(ConditionEffectIndex.Invincible),
                    new PlayerWithinTransition(7, "Follow")
                    ),
                new State("Follow",
                    new Chase(1, range: 1),
                    new TimedTransition(5000, "Throw")
                    ),
                new State("Throw",
                    new AddCond(ConditionEffectIndex.Invulnerable),
                    new TossObject("shtrs Firebomb", 1, randomToss: true, coolDown: 5000),
                    new TossObject("shtrs Firebomb", 2, randomToss: true, coolDown: 5000),
                    new TossObject("shtrs Firebomb", 3, randomToss: true, coolDown: 5000),
                    new TossObject("shtrs Firebomb", 4, randomToss: true, coolDown: 5000),
                    new TossObject("shtrs Firebomb", 5, randomToss: true, coolDown: 5000),
                    new TossObject("shtrs Firebomb", 6, randomToss: true, coolDown: 5000),
                    new TossObject("shtrs Firebomb", 6, randomToss: true, coolDown: 5000),
                    new TossObject("shtrs Firebomb", 5, randomToss: true, coolDown: 5000),
                    new TossObject("shtrs Firebomb", 4, randomToss: true, coolDown: 5000),
                    new TossObject("shtrs Firebomb", 3, randomToss: true, coolDown: 5000),
                    new TossObject("shtrs Firebomb", 2, randomToss: true, coolDown: 5000),
                    new TossObject("shtrs Firebomb", 1, randomToss: true, coolDown: 5000),
                    new TimedTransition(4000, "Fire")
                    ),
                new State("Fire",
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 45, angleOffset: 45, coolDown: 1, coolDownOffset: 0),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 90, angleOffset: 90, coolDown: 1, coolDownOffset: 0),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 135, angleOffset: 135, coolDown: 1, coolDownOffset: 0),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 180, angleOffset: 180, coolDown: 1, coolDownOffset: 0),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 225, angleOffset: 225, coolDown: 1, coolDownOffset: 0),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 270, angleOffset: 270, coolDown: 1, coolDownOffset: 0),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 315, angleOffset: 315, coolDown: 1, coolDownOffset: 0),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 360, angleOffset: 360, coolDown: 1, coolDownOffset: 0),
                    new TimedTransition(5000, "wait")
                    )
                )
            )

            .Init("shtrs Firebomb",
                new State(
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new TimedTransition(2000, "Explode")
                        ),
                    new State("Explode",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new Shoot(100, index: 0, shoots: 8),
                        new Suicide()
                        )
                    )
            )



            .Init("shtrs Fire Mage",
                new State(
                    new State("Wait",
                        new PlayerWithinTransition(5, "fire")
                            ),
                    new State("fire",
                        new Chase(0.5, range: 1),
                        new Shoot(10, 5, 10, index: 0, coolDown: 1500),
                        new TimedTransition(10000, "nothing")
                            ),
                    new State("nothing",
                        new TimedTransition(1000, "fire")
                        )
                    )
            )
            .Init("shtrs Stone Mage",
                new State(
                    new State("Wait",
                        new PlayerWithinTransition(5, "fire")
                            ),
                    new State("fire",
                        new Chase(0.5, range: 1),
                        new Shoot(10, 2, 10, index: 1, coolDown: 200),
                        new TimedTransition(10000, "invulnerable")
                            ),
                    new State("invulnerable",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Shoot(10, 2, 10, index: 0, coolDown: 200),
                        new TimedTransition(3000, "fire")
                        )
                    )
            )
        #endregion SomeMobs
        #region WOODENGATESSWITCHESBRIDGES
            .Init("shtrs Wooden Gate 3",
                new State(
                    new State("Despawn",
                        new Decay(0)
                        )
                    )
            )
            //.Init("OBJECTHERE",
            //new State(
            //      new EntityNotExistTransition("shtrs Abandoned Switch 1", 10, "OPENGATE")
            //        ),
            //      new State("OPENGATE",
            //            new OpenGate("shtrs Wooden Gate", 10)
            //              )
            //        )
            //      )
            .Init("shtrs Wooden Gate",
                new State(
                    new State("Idle",
                        new EntityNotExistsTransition("shtrs Abandoned Switch 1", 10, "Despawn")
                        ),
                    new State("Despawn",
                        new Decay(0)
                        )
                    )
            )
            .Init("shtrs Wooden Gate 2",
                new State(
                    new State("Idle",
                        new EntityNotExistsTransition("shtrs Abandoned Switch 2", 60, "Despawn")
                        ),
                    new State("Despawn",
                        new Decay(0)
                        )
                    )
            )
        .Init("shtrs Bridge Closer",
            new State(
                new State("Idle",
                    new AddCond(ConditionEffectIndex.Invincible)
                    ),
                new State("Closer",
                    new ChangeGroundOnDeath(new[] { "shtrs Bridge" }, new[] { "shtrs Pure Evil" },
                        1),
                    new Suicide()
                    ),
                new State("TwilightClose",
                    new ChangeGroundOnDeath(new[] { "shtrs Shattered Floor", "shtrs Disaster Floor" }, new[] { "shtrs Pure Evil" },
                        1),
                    new Suicide()

                    )
                )
            )
        .Init("shtrs Bridge Closer2",
            new State(
                new State("Idle",
                    new AddCond(ConditionEffectIndex.Invincible)
                    ),
                new State("Closer",
                    new ChangeGroundOnDeath(new[] { "shtrs Bridge" }, new[] { "shtrs Pure Evil" },
                        1),
                    new Suicide()
                    ),
                new State("TwilightClose",
                    new ChangeGroundOnDeath(new[] { "shtrs Shattered Floor", "shtrs Disaster Floor" }, new[] { "shtrs Pure Evil" },
                        1),
                    new Suicide()

                    )
                )
            )
        .Init("shtrs Bridge Closer3",
            new State(
                new State("Idle",
                    new AddCond(ConditionEffectIndex.Invincible)
                    ),
                new State("Closer",
                    new ChangeGroundOnDeath(new[] { "shtrs Bridge" }, new[] { "shtrs Pure Evil" },
                        1),
                    new Suicide()
                    ),
                new State("TwilightClose",
                    new ChangeGroundOnDeath(new[] { "shtrs Shattered Floor", "shtrs Disaster Floor" }, new[] { "shtrs Pure Evil" },
                        1),
                    new Suicide()

                    )
                )
            )
        .Init("shtrs Bridge Closer4",
            new State(
                new State("Idle",
                    new AddCond(ConditionEffectIndex.Invincible)
                    ),
                new State("Closer",
                    new ChangeGroundOnDeath(new[] { "shtrs Bridge" }, new[] { "shtrs Pure Evil" },
                        1),
                    new Suicide()
                    ),
                new State("TwilightClose",
                    new ChangeGroundOnDeath(new[] { "shtrs Shattered Floor", "shtrs Disaster Floor" }, new[] { "shtrs Pure Evil" },
                        1),
                    new Suicide()

                    )
                )
            )
        .Init("shtrs Spawn Bridge",
            new State(
                new State("Idle",
                    new AddCond(ConditionEffectIndex.Invincible)
                    ),
                new State("Open",
                    new ChangeGroundOnDeath(new[] { "shtrs Pure Evil" }, new[] { "shtrs Bridge" },
                        1),
                    new Suicide()
                    )
                )
            )
        .Init("shtrs Spawn Bridge 2",
            new State(
                new State("Idle",
                    new AddCond(ConditionEffectIndex.Invincible),
                    new EntityNotExistsTransition("shtrs Abandoned Switch 3", 500, "Open")
                    ),
                new State("Open",
                    new ChangeGroundOnDeath(new[] { "shtrs Pure Evil" }, new[] { "shtrs Shattered Floor" },
                        1),
                    new Suicide()
                    ),
                new State("CloseBridge2",
                    new ChangeGroundOnDeath(new[] { "shtrs Shattered Floor" }, new[] { "shtrs Pure Evil" },
                        1),
                    new Suicide()
                    )
                )
            )
        .Init("shtrs Spawn Bridge 3",
            new State(
                new State("Idle",
                    new AddCond(ConditionEffectIndex.Invincible),
                    new EntityNotExistsTransition("shtrs Twilight Archmage", 500, "Open")
                    ),
                new State("Open",
                    new ChangeGroundOnDeath(new[] { "shtrs Pure Evil" }, new[] { "shtrs Shattered Floor" },
                        1),
                    new Suicide()
                    )
                )
            )
        .Init("shtrs Spawn Bridge 5",
            new State(
                new State("Idle",
                    new AddCond(ConditionEffectIndex.Invincible),
                    new EntityNotExistsTransition("shtrs Royal Guardian L", 100, "Open")
                    ),
                new State("Open",
                    new ChangeGroundOnDeath(new[] { "Dark Cobblestone" }, new[] { "Hot Lava" },
                        1),
                    new Suicide()
                    )
                )
            )
        #endregion WOODENGATESSWITCHESBRIDGES
        #region 3rdboss
            .Init("shtrs The Forgotten King",
                new State(
                    new HpLessTransition(0.1, "Death"),
                    new CopyLootState("shtrs Loot Balloon King", 20),
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new AddCond(ConditionEffectIndex.Invisible),
                        new AddCond(ConditionEffectIndex.Stasis),
                        new TimedTransition(2000, "1st")
                    ),
                    new State("1st",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new AddCond(ConditionEffectIndex.Invisible),
                        new AddCond(ConditionEffectIndex.Stasis),
                        new Spawn("shtrs Green Crystal", 1, 1),
                        new Spawn("shtrs Yellow Crystal", 1, 1),
                        new Spawn("shtrs Red Crystal", 1, 1),
                        new Spawn("shtrs Blue Crystal", 1, 1),
                        new EntityNotExistsTransition("shtrs Green Crystal", 50, "yellow")
                        ),
                    new State("yellow",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new AddCond(ConditionEffectIndex.Invisible),
                        new AddCond(ConditionEffectIndex.Stasis),
                        new EntityNotExistsTransition("shtrs Yellow Crystal", 50, "blue")
                        ),
                    new State("blue",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new AddCond(ConditionEffectIndex.Invisible),
                        new AddCond(ConditionEffectIndex.Stasis),
                        new EntityNotExistsTransition("shtrs Blue Crystal", 50, "red")
                        ),
                    new State("red",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new AddCond(ConditionEffectIndex.Invisible),
                        new AddCond(ConditionEffectIndex.Stasis),
                        new EntityNotExistsTransition("shtrs Red Crystal", 50, "Swirl1")
                            ),
                        new State("Swirl1",
                            new Shoot(15, 1, index: 1, angleOffset: 85, coolDown: 100),
                            new Shoot(15, 1, index: 1, angleOffset: 95, coolDown: 100),
                            new TimedTransition(50, "Swirl2")
                            ),
                        new State("Swirl2",
                            new Shoot(15, 1, index: 1, angleOffset: 75, coolDown: 100),
                            new Shoot(15, 1, index: 1, angleOffset: 70, coolDown: 100),
                            new Shoot(15, 1, index: 1, angleOffset: 105, coolDown: 100),
                            new Shoot(15, 1, index: 1, angleOffset: 110, coolDown: 100),
                            new TimedTransition(50, "Swirl3")
                            ),
                        new State("Swirl3",
                            new Shoot(15, 1, index: 1, angleOffset: 60, coolDown: 100),
                            new Shoot(15, 1, index: 1, angleOffset: 55, coolDown: 100),
                            new Shoot(15, 1, index: 1, angleOffset: 120, coolDown: 100),
                            new Shoot(15, 1, index: 1, angleOffset: 125, coolDown: 100),
                            new TimedTransition(50, "Swirl4")
                            ),
                        new State("Swirl4",
                            new Shoot(15, 1, index: 1, angleOffset: 45, coolDown: 100),
                            new Shoot(15, 1, index: 1, angleOffset: 40, coolDown: 100),
                            new Shoot(15, 1, index: 1, angleOffset: 135, coolDown: 100),
                            new Shoot(15, 1, index: 1, angleOffset: 140, coolDown: 100),
                            new TimedTransition(50, "Swirl5")
                            ),
                        new State("Swirl5",
                            new Shoot(15, 1, index: 1, angleOffset: 30, coolDown: 100),
                            new Shoot(15, 1, index: 1, angleOffset: 25, coolDown: 100),
                            new Shoot(15, 1, index: 1, angleOffset: 150, coolDown: 100),
                            new Shoot(15, 1, index: 1, angleOffset: 155, coolDown: 100),
                            new TimedTransition(50, "Swirl6")
                            ),
                        new State("Swirl6",
                            new Shoot(15, 1, index: 1, angleOffset: 20, coolDown: 100),
                            new Shoot(15, 1, index: 1, angleOffset: 15, coolDown: 100),
                            new Shoot(15, 1, index: 1, angleOffset: 165, coolDown: 100),
                            new Shoot(15, 1, index: 1, angleOffset: 170, coolDown: 100),
                            new TimedTransition(50, "Swirl7")
                            ),
                        new State("Swirl7",
                            new Shoot(15, 1, index: 1, angleOffset: 5, coolDown: 100),
                            new Shoot(15, 1, index: 1, angleOffset: 0, coolDown: 100),
                            new Shoot(15, 1, index: 1, angleOffset: 180, coolDown: 100),
                            new Shoot(15, 1, index: 1, angleOffset: 175, coolDown: 100),
                            new TimedTransition(50, "Swirl8")
                            ),
                        new State("Swirl8",
                            new TimedTransition(50, "Swirl9")
                            ),
                        new State("Swirl9",
                            new TimedTransition(50, "middle")
                            ),
                        new State("middle",
                            new AddCond(ConditionEffectIndex.Invincible),
                            new AddCond(ConditionEffectIndex.Invisible),
                            new AddCond(ConditionEffectIndex.Stasis),
                            new MoveTo(0, 8, 0.5),
                            new TimedTransition(3000, "J Guardians")
                            ),
                        new State("J Guardians",
                            new AddCond(ConditionEffectIndex.Invincible),
                            new AddCond(ConditionEffectIndex.Invisible),
                            new AddCond(ConditionEffectIndex.Stasis),
                            new Spawn("shtrs Royal Guardian J", 10),
                            new TimedTransition(50, "waiting")
                            ),
                        new State("waiting",
                            new AddCond(ConditionEffectIndex.Invincible),
                            new AddCond(ConditionEffectIndex.Invisible),
                            new AddCond(ConditionEffectIndex.Stasis),
                            new EntityNotExistsTransition("shtrs Royal Guardian J", 10, "littlerage")
                            ),
                        new State("littlerage",
                            new ChangeSize(100, 330),
                            new Shoot(10, 1, index: 2, coolDown: 10),
                            new Shoot(10, 1, index: 3, coolDown: 10),
                            new TimedTransition(100, "Shoot1")
                            ),
                        new State("Shoot1",
                            new Shoot(10, 1, index: 2, coolDown: 10),
                            new Shoot(10, 1, index: 3, coolDown: 10),
                            new TimedTransition(100, "Shoot2")
                            ),
                        new State("Shoot2",
                            new Shoot(10, 1, index: 2, coolDown: 10),
                            new Shoot(10, 1, index: 3, coolDown: 10),
                            new TimedTransition(100, "Shoot3")
                            ),
                        new State("Shoot3",
                            new Shoot(10, 1, index: 2, coolDown: 10),
                            new Shoot(10, 1, index: 3, coolDown: 10),
                            new TimedTransition(100, "Shoot4")
                            ),
                        new State("Shoot4",
                            new Shoot(10, 1, index: 2, coolDown: 10),
                            new Shoot(10, 1, index: 3, coolDown: 10),
                            new TimedTransition(100, "Raged1")
                            ),
                        new State("Raged1",
                            new Taunt("TENTACLES OF WRATH"),
                            new AddCond(ConditionEffectIndex.Invincible),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 3, coolDown: 10800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 4, coolDown: 10800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 6, coolDown: 10800, coolDownOffset: 200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 7, coolDown: 10800, coolDownOffset: 200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 9, coolDown: 10800, coolDownOffset: 400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 10, coolDown: 10800, coolDownOffset: 400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 12, coolDown: 10800, coolDownOffset: 600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 13, coolDown: 10800, coolDownOffset: 600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 15, coolDown: 10800, coolDownOffset: 800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 16, coolDown: 10800, coolDownOffset: 800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 18, coolDown: 10800, coolDownOffset: 1000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 19, coolDown: 10800, coolDownOffset: 1000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 21, coolDown: 10800, coolDownOffset: 1200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 22, coolDown: 10800, coolDownOffset: 1200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 24, coolDown: 10800, coolDownOffset: 1400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 25, coolDown: 10800, coolDownOffset: 1400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 27, coolDown: 10800, coolDownOffset: 1600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 28, coolDown: 10800, coolDownOffset: 1600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 30, coolDown: 10800, coolDownOffset: 1800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 31, coolDown: 10800, coolDownOffset: 1800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 33, coolDown: 10800, coolDownOffset: 2000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 34, coolDown: 10800, coolDownOffset: 2000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 36, coolDown: 10800, coolDownOffset: 2200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 37, coolDown: 10800, coolDownOffset: 2200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 39, coolDown: 10800, coolDownOffset: 2400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 40, coolDown: 10800, coolDownOffset: 2400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 42, coolDown: 10800, coolDownOffset: 2600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 43, coolDown: 10800, coolDownOffset: 2600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 45, coolDown: 10800, coolDownOffset: 2800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 46, coolDown: 10800, coolDownOffset: 2800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 48, coolDown: 10800, coolDownOffset: 3000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 49, coolDown: 10800, coolDownOffset: 3000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 51, coolDown: 10800, coolDownOffset: 3200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 52, coolDown: 10800, coolDownOffset: 3200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 54, coolDown: 10800, coolDownOffset: 3400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 55, coolDown: 10800, coolDownOffset: 3400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 57, coolDown: 10800, coolDownOffset: 3600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 58, coolDown: 10800, coolDownOffset: 3600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 60, coolDown: 10800, coolDownOffset: 3800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 61, coolDown: 10800, coolDownOffset: 3800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 63, coolDown: 10800, coolDownOffset: 4000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 64, coolDown: 10800, coolDownOffset: 4000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 66, coolDown: 10800, coolDownOffset: 4200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 67, coolDown: 10800, coolDownOffset: 4200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 69, coolDown: 10800, coolDownOffset: 4400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 70, coolDown: 10800, coolDownOffset: 4400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 72, coolDown: 10800, coolDownOffset: 4600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 73, coolDown: 10800, coolDownOffset: 4600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 75, coolDown: 10800, coolDownOffset: 4800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 76, coolDown: 10800, coolDownOffset: 4800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 78, coolDown: 10800, coolDownOffset: 5000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 79, coolDown: 10800, coolDownOffset: 5000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 81, coolDown: 10800, coolDownOffset: 5200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 82, coolDown: 10800, coolDownOffset: 5200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 84, coolDown: 10800, coolDownOffset: 5400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 85, coolDown: 10800, coolDownOffset: 5400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 87, coolDown: 10800, coolDownOffset: 5600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 88, coolDown: 10800, coolDownOffset: 5600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 90, coolDown: 10800, coolDownOffset: 5800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 91, coolDown: 10800, coolDownOffset: 5800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 93, coolDown: 10800, coolDownOffset: 6000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 94, coolDown: 10800, coolDownOffset: 6000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 96, coolDown: 10800, coolDownOffset: 6200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 97, coolDown: 10800, coolDownOffset: 6200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 99, coolDown: 10800, coolDownOffset: 6400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 100, coolDown: 10800, coolDownOffset: 6400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 102, coolDown: 10800, coolDownOffset: 6600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 103, coolDown: 10800, coolDownOffset: 6600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 105, coolDown: 10800, coolDownOffset: 6800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 106, coolDown: 10800, coolDownOffset: 6800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 108, coolDown: 10800, coolDownOffset: 7000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 109, coolDown: 10800, coolDownOffset: 7000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 111, coolDown: 10800, coolDownOffset: 7200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 112, coolDown: 10800, coolDownOffset: 7200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 114, coolDown: 10800, coolDownOffset: 7400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 115, coolDown: 10800, coolDownOffset: 7400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 117, coolDown: 10800, coolDownOffset: 7600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 118, coolDown: 10800, coolDownOffset: 7600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 120, coolDown: 10800, coolDownOffset: 7800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 121, coolDown: 10800, coolDownOffset: 7800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 123, coolDown: 10800, coolDownOffset: 8000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 124, coolDown: 10800, coolDownOffset: 8000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 126, coolDown: 10800, coolDownOffset: 8200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 127, coolDown: 10800, coolDownOffset: 8200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 129, coolDown: 10800, coolDownOffset: 8400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 130, coolDown: 10800, coolDownOffset: 8400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 132, coolDown: 10800, coolDownOffset: 8600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 133, coolDown: 10800, coolDownOffset: 8600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 135, coolDown: 10800, coolDownOffset: 8800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 136, coolDown: 10800, coolDownOffset: 8800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 138, coolDown: 10800, coolDownOffset: 9000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 139, coolDown: 10800, coolDownOffset: 9000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 141, coolDown: 10800, coolDownOffset: 9200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 142, coolDown: 10800, coolDownOffset: 9200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 144, coolDown: 10800, coolDownOffset: 9400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 145, coolDown: 10800, coolDownOffset: 9400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 147, coolDown: 10800, coolDownOffset: 9600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 148, coolDown: 10800, coolDownOffset: 9600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 150, coolDown: 10800, coolDownOffset: 9800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 151, coolDown: 10800, coolDownOffset: 9800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 153, coolDown: 10800, coolDownOffset: 10000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 154, coolDown: 10800, coolDownOffset: 10000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 156, coolDown: 10800, coolDownOffset: 10200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 157, coolDown: 10800, coolDownOffset: 10200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 159, coolDown: 10800, coolDownOffset: 10400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 160, coolDown: 10800, coolDownOffset: 10400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 162, coolDown: 10800, coolDownOffset: 10600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 163, coolDown: 10800, coolDownOffset: 10600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 165, coolDown: 10800, coolDownOffset: 10800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 166, coolDown: 10800, coolDownOffset: 10800),
                            new HpLessTransition(0.5, "MoveTo"),
                            new TimedTransition(10800, "Pause")
                            ),
                        new State("Pause",
                            new AddCond(ConditionEffectIndex.Invincible),
                            new TimedTransition(4000, "littlerage")
                            ),
                        new State("MoveTo",
                            new AddCond(ConditionEffectIndex.Invincible),
                            new MoveTo(0, -8, 0.5),
                            new TimedTransition(1800, "God")
                            ),
                        new State("God",
                            new Taunt("YOU TEST THE PATIENCE OF A GOD"),
                            new AddCond(ConditionEffectIndex.Invincible),
                            new Shoot(15, 1, index: 1, angleOffset: 85, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 95, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 75, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 70, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 105, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 110, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 60, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 55, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 120, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 125, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 45, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 40, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 135, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 140, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 30, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 25, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 150, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 155, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 20, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 15, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 165, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 170, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 5, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 0, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 180, coolDown: 500),
                            new Shoot(15, 1, index: 1, angleOffset: 175, coolDown: 500),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 3, coolDown: 10800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 4, coolDown: 10800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 6, coolDown: 10800, coolDownOffset: 200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 7, coolDown: 10800, coolDownOffset: 200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 9, coolDown: 10800, coolDownOffset: 400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 10, coolDown: 10800, coolDownOffset: 400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 12, coolDown: 10800, coolDownOffset: 600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 13, coolDown: 10800, coolDownOffset: 600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 15, coolDown: 10800, coolDownOffset: 800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 16, coolDown: 10800, coolDownOffset: 800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 18, coolDown: 10800, coolDownOffset: 1000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 19, coolDown: 10800, coolDownOffset: 1000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 21, coolDown: 10800, coolDownOffset: 1200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 22, coolDown: 10800, coolDownOffset: 1200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 24, coolDown: 10800, coolDownOffset: 1400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 25, coolDown: 10800, coolDownOffset: 1400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 27, coolDown: 10800, coolDownOffset: 1600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 28, coolDown: 10800, coolDownOffset: 1600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 30, coolDown: 10800, coolDownOffset: 1800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 31, coolDown: 10800, coolDownOffset: 1800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 33, coolDown: 10800, coolDownOffset: 2000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 34, coolDown: 10800, coolDownOffset: 2000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 36, coolDown: 10800, coolDownOffset: 2200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 37, coolDown: 10800, coolDownOffset: 2200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 39, coolDown: 10800, coolDownOffset: 2400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 40, coolDown: 10800, coolDownOffset: 2400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 42, coolDown: 10800, coolDownOffset: 2600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 43, coolDown: 10800, coolDownOffset: 2600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 45, coolDown: 10800, coolDownOffset: 2800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 46, coolDown: 10800, coolDownOffset: 2800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 48, coolDown: 10800, coolDownOffset: 3000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 49, coolDown: 10800, coolDownOffset: 3000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 51, coolDown: 10800, coolDownOffset: 3200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 52, coolDown: 10800, coolDownOffset: 3200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 54, coolDown: 10800, coolDownOffset: 3400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 55, coolDown: 10800, coolDownOffset: 3400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 57, coolDown: 10800, coolDownOffset: 3600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 58, coolDown: 10800, coolDownOffset: 3600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 60, coolDown: 10800, coolDownOffset: 3800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 61, coolDown: 10800, coolDownOffset: 3800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 63, coolDown: 10800, coolDownOffset: 4000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 64, coolDown: 10800, coolDownOffset: 4000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 66, coolDown: 10800, coolDownOffset: 4200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 67, coolDown: 10800, coolDownOffset: 4200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 69, coolDown: 10800, coolDownOffset: 4400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 70, coolDown: 10800, coolDownOffset: 4400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 72, coolDown: 10800, coolDownOffset: 4600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 73, coolDown: 10800, coolDownOffset: 4600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 75, coolDown: 10800, coolDownOffset: 4800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 76, coolDown: 10800, coolDownOffset: 4800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 78, coolDown: 10800, coolDownOffset: 5000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 79, coolDown: 10800, coolDownOffset: 5000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 81, coolDown: 10800, coolDownOffset: 5200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 82, coolDown: 10800, coolDownOffset: 5200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 84, coolDown: 10800, coolDownOffset: 5400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 85, coolDown: 10800, coolDownOffset: 5400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 87, coolDown: 10800, coolDownOffset: 5600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 88, coolDown: 10800, coolDownOffset: 5600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 90, coolDown: 10800, coolDownOffset: 5800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 91, coolDown: 10800, coolDownOffset: 5800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 93, coolDown: 10800, coolDownOffset: 6000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 94, coolDown: 10800, coolDownOffset: 6000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 96, coolDown: 10800, coolDownOffset: 6200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 97, coolDown: 10800, coolDownOffset: 6200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 99, coolDown: 10800, coolDownOffset: 6400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 100, coolDown: 10800, coolDownOffset: 6400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 102, coolDown: 10800, coolDownOffset: 6600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 103, coolDown: 10800, coolDownOffset: 6600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 105, coolDown: 10800, coolDownOffset: 6800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 106, coolDown: 10800, coolDownOffset: 6800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 108, coolDown: 10800, coolDownOffset: 7000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 109, coolDown: 10800, coolDownOffset: 7000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 111, coolDown: 10800, coolDownOffset: 7200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 112, coolDown: 10800, coolDownOffset: 7200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 114, coolDown: 10800, coolDownOffset: 7400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 115, coolDown: 10800, coolDownOffset: 7400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 117, coolDown: 10800, coolDownOffset: 7600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 118, coolDown: 10800, coolDownOffset: 7600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 120, coolDown: 10800, coolDownOffset: 7800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 121, coolDown: 10800, coolDownOffset: 7800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 123, coolDown: 10800, coolDownOffset: 8000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 124, coolDown: 10800, coolDownOffset: 8000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 126, coolDown: 10800, coolDownOffset: 8200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 127, coolDown: 10800, coolDownOffset: 8200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 129, coolDown: 10800, coolDownOffset: 8400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 130, coolDown: 10800, coolDownOffset: 8400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 132, coolDown: 10800, coolDownOffset: 8600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 133, coolDown: 10800, coolDownOffset: 8600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 135, coolDown: 10800, coolDownOffset: 8800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 136, coolDown: 10800, coolDownOffset: 8800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 138, coolDown: 10800, coolDownOffset: 9000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 139, coolDown: 10800, coolDownOffset: 9000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 141, coolDown: 10800, coolDownOffset: 9200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 142, coolDown: 10800, coolDownOffset: 9200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 144, coolDown: 10800, coolDownOffset: 9400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 145, coolDown: 10800, coolDownOffset: 9400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 147, coolDown: 10800, coolDownOffset: 9600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 148, coolDown: 10800, coolDownOffset: 9600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 150, coolDown: 10800, coolDownOffset: 9800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 151, coolDown: 10800, coolDownOffset: 9800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 153, coolDown: 10800, coolDownOffset: 10000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 154, coolDown: 10800, coolDownOffset: 10000),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 156, coolDown: 10800, coolDownOffset: 10200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 157, coolDown: 10800, coolDownOffset: 10200),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 159, coolDown: 10800, coolDownOffset: 10400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 160, coolDown: 10800, coolDownOffset: 10400),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 162, coolDown: 10800, coolDownOffset: 10600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 163, coolDown: 10800, coolDownOffset: 10600),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 165, coolDown: 10800, coolDownOffset: 10800),
                            new Shoot(50, index: 4, shoots: 6, shootAngle: 60, angleOffset: 166, coolDown: 10800, coolDownOffset: 10800),
                            new TimedTransition(10800, "Hahaha")
                            ),
                        new State("Hahaha",
                            new Taunt("Ha....Haaaa...haaaa"),
                            new TimedTransition(4000, "God")
                            ),
                        new State("Death",
                            new AddCond(ConditionEffectIndex.Invulnerable),
                            new CopyDamageOnDeath("shtrs Loot Balloon King"),
                            new EntityOrder(1, "shtrs Chest Spawner 3", "Open"),
                            new Taunt("Impossible..........IMPOSSIBLE!"),
                            new TimedTransition(2000, "Suicide")
                            ),
                        new State("Suicide",
                            new Shoot(35, index: 0, shoots: 30),
                            new Suicide()
                    )
                )
            )
            .Init("shtrs Royal Guardian J",
                new State(
                    new State("shoot",
                        new Circle(1.0, 2, 5, "shtrs The Forgotten King"),
                        new Shoot(15, 8, index: 0)
                        )
                    )
            )
            .Init("shtrs Royal Guardian L",
                new State(
                    new State("1st",
                        new Chase(1, 8, 5),
                        new Shoot(15, 20, index: 0),
                        new TimedTransition(1000, "2nd")
                        ),
                    new State("2nd",
                        new Chase(1, 8, 5),
                        new Shoot(10, index: 1),
                        new TimedTransition(1000, "3rd")
                        ),
                    new State("3rd",
                        new Chase(1, 8, 5),
                        new Shoot(10, index: 1),
                        new TimedTransition(1000, "1st")
                        )
                    )
            )
            .Init("shtrs Green Crystal",
                new State(
                    new Heal(5, 1000, "idkanymore", coolDown: 10000),
                    new State("orbit",
                        new Circle(1.0, 2, 5, "shtrs The Forgotten King"),
                        new TimedTransition(8000, "dafuq")
                        ),
                    new State("dafuq",
                        new Circle(1.0, 2, 5, "shtrs The Forgotten King")
                        )
                    )
            )
            .Init("shtrs Yellow Crystal",
                new State(
                    new State("orbit",
                        new Circle(1.0, 2, 5, "shtrs The Forgotten King"),
                        new TimedTransition(25, "shoot")
                        ),
                    new State("shoot",
                        new Shoot(5, 4, 4, index: 0),
                        new TimedTransition(1, "dafuq")
                        ),
                    new State("dafuq",
                        new Circle(1.0, 2, 5, "shtrs The Forgotten King"),
                        new Shoot(5, 4, 4, index: 0)
                        )
                    )
            )
            .Init("shtrs Red Crystal",
                new State(
                    new State("orbit",
                        new Circle(1.0, 2, 5, "shtrs The Forgotten King"),
                        new TimedTransition(8000, "dafuq")
                        ),
                    new State("dafuq",
                        new Circle(1.0, 2, 5, "shtrs The Forgotten King"),
                        new TimedTransition(15000, "ThrowPortal")
                        ),
                    new State("ThrowPortal",
                        new TossObject("shtrs Fire Portal", 5, coolDown: 8000, coolDownOffset: 7000, randomToss: false),
                        new TimedTransition(25, "orbit")
                        )
                    )
            )
            .Init("shtrs Blue Crystal",
                new State(
                    new State("orbit",
                        new Circle(1.0, 2, 5, "shtrs The Forgotten King"),
                        new TimedTransition(8000, "dafuq")
                        ),
                    new State("dafuq",
                        new Circle(1.0, 2, 5, "shtrs The Forgotten King"),
                        new TimedTransition(15000, "ThrowPortal")
                        ),
                    new State("ThrowPortal",
                        new TossObject("shtrs Ice Portal", 5, coolDown: 8000, coolDownOffset: 7000, randomToss: false),
                        new TimedTransition(25, "orbit")
                        )
                    )
            )
        .Init("shtrs The Cursed Crown",
            new State(
                new State("Idle",
                    new AddCond(ConditionEffectIndex.Invincible),
                    new EntityNotExistsTransition("shtrs Royal Guardian L", 100, "Open")
                    ),
                new State("Open",
                    new AddCond(ConditionEffectIndex.Invincible),
                    new MoveTo(0, -15, 0.5),
                    new TimedTransition(3000, "WADAFAK")
                    ),
                new State("WADAFAK",
                    new TransformOnDeath("shtrs The Forgotten King"),
                    new Suicide()
                    )
                )
            )
        #endregion 3rdboss
        #region 3rdbosschest
            .Init("shtrs Loot Balloon King",
                new State(
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "Crown")
                    ),
                    new State("Crown")
                ),
                new Threshold(0.1,
                    new TierLoot(11, ItemType.Weapon, 1),
                    new TierLoot(12, ItemType.Weapon, 1),
                    new TierLoot(6, ItemType.Ability, 1),
                    new TierLoot(12, ItemType.Armor, 1),
                    new TierLoot(13, ItemType.Armor, 1),
                    new TierLoot(6, ItemType.Ring, 1),
                new Threshold(0.32,
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("The Forgotten Crown", 1)
                    )
                )
            )
        #endregion 3rdbosschest
        // Use this for other stuff.
        #region NotInUse
        //      .Init("shtrs Spawn Bridge 6",
        //          new State(
        //              new State("Idle",
        //                  new ConditionalEffect(ConditionEffectIndex.Invincible, true),
        //                  new EntityNotExistsTransition("shtrs Royal Guardian L", 100, "Open")
        //                  ),
        //              new State("Open",
        //                  new ChangeGroundOnDeath(new[] { "Green BigSmall Squared" }, new[] { "Hot Lava" },
        //                      1),
        //                  new Suicide()
        //                  )
        //              )
        //          )
        //      .Init("shtrs Spawn Bridge 7",
        //          new State(
        //              new State("Idle",
        //                  new ConditionalEffect(ConditionEffectIndex.Invincible, true),
        //                  new EntityNotExistsTransition("shtrs Royal Guardian L", 100, "Open")
        //                  ),
        //              new State("Open",
        //                  new ChangeGroundOnDeath(new[] { "Gold Tile" }, new[] { "Hot Lava" },
        //                      1),
        //                  new Suicide()
        //                  )
        //              )
        //          )
        //      .Init("shtrs Spawn Bridge 8",
        //          new State(
        //              new State("Idle",
        //                  new ConditionalEffect(ConditionEffectIndex.Invincible, true),
        //                  new EntityNotExistsTransition("shtrs Royal Guardian L", 100, "Open")
        //                  ),
        //              new State("Open",
        //                  new ChangeGroundOnDeath(new[] { "Shattered Floor" }, new[] { "Hot Lava" },
        //                      1),
        //                  new Suicide()
        //                  )
        //              )
        //          )
        #endregion NotInUse
        #region MISC
        .Init("shtrs Chest Spawner 1",
            new State(
                new State("Idle",
                    new AddCond(ConditionEffectIndex.Invincible),
                    new EntityNotExistsTransition("shtrs Bridge Sentinel", 500, "Open")
                    ),
                new State("Open",
                    new TransformOnDeath("shtrs Loot Balloon Bridge"),
                    new Suicide()
                    )
                )
            )
        .Init("shtrs Chest Spawner 2",
            new State(
                new State("Idle",
                    new AddCond(ConditionEffectIndex.Invincible),
                    new EntityNotExistsTransition("shtrs Twilight Archmage", 500, "Open")
                    ),
                new State("Open",
                    new TransformOnDeath("shtrs Loot Balloon Mage"),
                    new Suicide()
                    )
                )
            )
        .Init("shtrs Chest Spawner 3",
            new State(
                new State("Idle",
                    new AddCond(ConditionEffectIndex.Invincible),
                    new EntitiesNotExistsTransition(30, "Open", "shtrs The Cursed Crown", "shtrs The Forgotten King")
                    ),
                new State("Open",
                    new TransformOnDeath("shtrs Loot Balloon King"),
                    new Suicide()
                    )
                )
            )
        #endregion MISC
            ;
    }
}