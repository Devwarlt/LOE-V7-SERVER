using gameserver.logic.behaviors;
using gameserver.logic.loot;
using gameserver.logic.transitions;

namespace gameserver.logic
{
    partial class BehaviorDb
    {
        private _ LairofShaitan = () => Behav()
            .Init("md1 Head of Shaitan",
                new State(
                    new State("1",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new State("2",
                            new PlayerWithinTransition(4, "3")
                            ),
                        new State("3",
                            new EntityOrder(999, "md1 Right Hand of Shaitan", "2"),
                            new EntityOrder(999, "md1 Left Hand of Shaitan", "2"),
                            new TimedTransition(0, "4")
                            )
                        ),
                    new State("4",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new ChangeSize(25, 250),
                        new State("nothin"),
                        new State("5",
                            new Shoot(9 + 1 / 2, 1, index: 0, coolDown: 1000),
                            new EntitiesNotExistsTransition(999, "6", "md1 Right Hand of Shaitan", "md1 Left Hand of Shaitan")
                            ),
                        new State("6",
                            new AddCond(ConditionEffectIndex.Invulnerable),
                            new SetAltTexture(1),
                            new EntityOrder(999, "md1 Governor", "1"),
                            new MoveTo(13.5f, 5, 0.8, true, true),
                            new TimedTransition(2000, "7")
                            )
                        ),
                    new State("7",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(0, "8")
                        ),
                    new State("8",
                        new HpLessTransition(0.8, "18"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "9")
                        ),
                    new State("9",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(8.5f, 5, 0.8, true, true),
                        new TimedTransition(2000, "10")
                        ),
                    new State("10",
                        new HpLessTransition(0.8, "18"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "11")
                        ),
                    new State("11",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(3.5f, 5, 0.8, true, true),
                        new TimedTransition(2000, "12")
                        ),
                    new State("12",
                        new HpLessTransition(0.8, "18"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "13")
                        ),
                    new State("13",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(true, 0.8),
                        new TimedTransition(2000, "14")
                        ),
                    new State("14",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityOrder(999, "md1 Right Hand spawner", "1"),
                        new EntityOrder(999, "md1 Left Hand spawner", "1"),
                        new EntityExistsTransition("md1 Right Hand of Shaitan", 999, "15")
                        ),
                    new State("15",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityExistsTransition("md1 Left Hand of Shaitan", 999, "16")
                        ),
                    new State("16",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityOrder(999, "md1 Right Hand of Shaitan", "TEST"),
                        new EntityOrder(999, "md1 Left Hand of Shaitan", "TEST"),
                        new TimedTransition(0, "17")
                        ),
                    new State("17",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Shoot(9 + 1 / 2, 1, index: 0, coolDown: 1000),
                        new EntitiesNotExistsTransition(999, "5", "md1 Right Hand of Shaitan", "md1 Left Hand of Shaitan")
                        ),
                    //Phase 2
                    new State("18",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(0),
                        new ReturnToSpawn(true, 0.8),
                        new Taunt("Let loose the fists of war!"),
                        new Shoot(9 + 1 / 2, 1, index: 0, coolDown: 1000),
                        new State("19",
                            new EntityOrder(999, "md1 Governor", "1"),
                            new EntityOrder(999, "md1 Right Hand spawner", "2"),
                            new EntityOrder(999, "md1 Left Hand spawner", "2"),
                            new TossObject("md1 CreepyHead", 5, 90),
                            new TimedTransition(0, "20")
                            ),
                        new State("20",
                            new TossObject("md1 Lava Makers", 14, coolDown: 500),
                            new TimedTransition(5500, "21")
                            )
                        ),
                    new State("21",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new EntityOrder(999, "md1 Governor", "1"),
                        new MoveTo(13.5f, 5, 0.8, true, true),
                        new TimedTransition(2000, "22")
                        ),
                    new State("22",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityOrder(999, "md1 Right Burst", "2"),
                        new EntityOrder(999, "md1 Mid Burst", "2"),
                        new EntityOrder(999, "md1 Left Burst", "2"),
                        new TimedTransition(0, "23")
                        ),
                    new State("23",
                        new HpLessTransition(0.6, "43"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "24")
                        ),
                    new State("24",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(8.5f, 5, 0.8, true, true),
                        new TimedTransition(2000, "25")
                        ),
                    new State("25",
                        new HpLessTransition(0.6, "43"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "26")
                        ),
                    new State("26",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(3.5f, 5, 0.8, true, true),
                        new TimedTransition(2000, "27")
                        ),
                    new State("27",
                        new HpLessTransition(0.6, "43"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "28")
                        ),
                    new State("28",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(true, 0.8),
                        new TimedTransition(2000, "29")
                        ),
                    new State("29",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityOrder(999, "md1 Right Hand spawner", "1"),
                        new EntityOrder(999, "md1 Left Hand spawner", "1"),
                        new EntityExistsTransition("md1 Right Hand of Shaitan", 999, "30")
                        ),
                    new State("30",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityExistsTransition("md1 Left Hand of Shaitan", 999, "31")
                        ),
                    new State("31",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityOrder(999, "md1 Right Hand of Shaitan", "TEST"),
                        new EntityOrder(999, "md1 Left Hand of Shaitan", "TEST"),
                        new TimedTransition(0, "32")
                        ),
                    new State("32",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Shoot(9 + 1 / 2, 1, index: 0, coolDown: 1000),
                        new EntitiesNotExistsTransition(999, "33", "md1 Right Hand of Shaitan", "md1 Left Hand of Shaitan")
                        ),
                    new State("33",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(0),
                        new ReturnToSpawn(true, 0.8),
                        new Shoot(9 + 1 / 2, 1, index: 0, coolDown: 1000),
                        new State("34",
                            new EntityOrder(999, "md1 Governor", "1"),
                            new EntityOrder(999, "md1 Right Hand spawner", "2"),
                            new EntityOrder(999, "md1 Left Hand spawner", "2"),
                            new TossObject("md1 CreepyHead", 5, 90),
                            new TimedTransition(0, "35")
                            ),
                        new State("35",
                            new TossObject("md1 Lava Makers", 14, coolDown: 500),
                            new TimedTransition(5500, "36")
                            )
                        ),
                    new State("36",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new EntityOrder(999, "md1 Governor", "1"),
                        new MoveTo(13.5f, 5, 0.8, true, true),
                        new TimedTransition(2000, "37")
                        ),
                    new State("37",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityOrder(999, "md1 Right Burst", "2"),
                        new EntityOrder(999, "md1 Mid Burst", "2"),
                        new EntityOrder(999, "md1 Left Burst", "2"),
                        new TimedTransition(0, "38")
                        ),
                    new State("38",
                        new HpLessTransition(0.6, "43"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "39")
                        ),
                    new State("39",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(8.5f, 5, 0.8, true, true),
                        new TimedTransition(2000, "40")
                        ),
                    new State("40",
                        new HpLessTransition(0.6, "43"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "41")
                        ),
                    new State("41",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(3.5f, 5, 0.8, true, true),
                        new TimedTransition(2000, "42")
                        ),
                    new State("42",
                        new HpLessTransition(0.6, "43"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "28")
                        ),
                    //Phase 3
                    new State("43",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(0),
                        new ReturnToSpawn(true, 0.8),
                        new Taunt("YOUR ARE BEGINNING TO UPSET ME. ENJOY A FAST DEATH!"),
                        new Shoot(9 + 1 / 2, 1, index: 0, coolDown: 1000),
                        new State("44",
                            new EntityOrder(999, "md1 Governor", "1"),
                            new EntityOrder(999, "md1 Right Hand spawner", "2"),
                            new EntityOrder(999, "md1 Left Hand spawner", "2"),
                            new TossObject("md1 CreepyHead", 5, 65),
                            new TossObject("md1 CreepyHead", 5, 105),
                            new TimedTransition(0, "45")
                            ),
                        new State("45",
                            new TossObject("md1 Lava Makers", 14, coolDown: 500),
                            new TimedTransition(5500, "46")
                            )
                        ),
                    new State("46",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new EntityOrder(999, "md1 Governor", "1"),
                        new MoveTo(13.5f, 5, 0.8, true, true),
                        new TimedTransition(2000, "47")
                        ),
                    new State("47",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityOrder(999, "md1 Right Burst", "2"),
                        new EntityOrder(999, "md1 Mid Burst", "2"),
                        new EntityOrder(999, "md1 Left Burst", "2"),
                        new TimedTransition(0, "48")
                        ),
                    new State("48",
                        new HpLessTransition(0.4, "68"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "49")
                        ),
                    new State("49",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(8.5f, 5, 0.8, true, true),
                        new TimedTransition(2000, "50")
                        ),
                    new State("50",
                        new HpLessTransition(0.4, "68"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "51")
                        ),
                    new State("51",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(3.5f, 5, 0.8, true, true),
                        new TimedTransition(2000, "52")
                        ),
                    new State("52",
                        new HpLessTransition(0.4, "68"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "53")
                        ),
                    new State("53",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(true, 0.8),
                        new TimedTransition(2000, "54")
                        ),
                    new State("54",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityOrder(999, "md1 Right Hand spawner", "1"),
                        new EntityOrder(999, "md1 Left Hand spawner", "1"),
                        new EntityExistsTransition("md1 Right Hand of Shaitan", 999, "55")
                        ),
                    new State("55",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityExistsTransition("md1 Left Hand of Shaitan", 999, "56")
                        ),
                    new State("56",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityOrder(999, "md1 Right Hand of Shaitan", "TEST"),
                        new EntityOrder(999, "md1 Left Hand of Shaitan", "TEST"),
                        new TimedTransition(0, "57")
                        ),
                    new State("57",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Shoot(9 + 1 / 2, 1, index: 0, coolDown: 1000),
                        new EntitiesNotExistsTransition(999, "58", "md1 Right Hand of Shaitan", "md1 Left Hand of Shaitan")
                        ),
                    new State("58",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(0),
                        new ReturnToSpawn(true, 0.8),
                        new Shoot(9 + 1 / 2, 1, index: 0, coolDown: 1000),
                        new State("59",
                            new EntityOrder(999, "md1 Governor", "1"),
                            new EntityOrder(999, "md1 Right Hand spawner", "2"),
                            new EntityOrder(999, "md1 Left Hand spawner", "2"),
                            new TossObject("md1 CreepyHead", 5, 65),
                            new TossObject("md1 CreepyHead", 5, 105),
                            new TimedTransition(0, "60")
                            ),
                        new State("60",
                            new TossObject("md1 Lava Makers", 14, coolDown: 500),
                            new TimedTransition(5500, "61")
                            )
                        ),
                    new State("61",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new EntityOrder(999, "md1 Governor", "1"),
                        new MoveTo(13.5f, 5, 0.8, true, true),
                        new TimedTransition(2000, "62")
                        ),
                    new State("62",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityOrder(999, "md1 Right Burst", "2"),
                        new EntityOrder(999, "md1 Mid Burst", "2"),
                        new EntityOrder(999, "md1 Left Burst", "2"),
                        new TimedTransition(0, "63")
                        ),
                    new State("63",
                        new HpLessTransition(0.4, "68"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "64")
                        ),
                    new State("64",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(8.5f, 5, 0.8, true, true),
                        new TimedTransition(2000, "65")
                        ),
                    new State("65",
                        new HpLessTransition(0.4, "68"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "66")
                        ),
                    new State("66",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(3.5f, 5, 0.8, true, true),
                        new TimedTransition(2000, "67")
                        ),
                    new State("67",
                        new HpLessTransition(0.4, "68"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "53")
                        ),
                    //Phase 4
                    new State("68",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(0),
                        new ReturnToSpawn(true, 0.8),
                        new Taunt("BE CONSUMED BY FLAMES!"),
                        new Shoot(9 + 1 / 2, 1, index: 0, coolDown: 1000),
                        new State("69",
                            new EntityOrder(999, "md1 Governor", "1"),
                            new EntityOrder(999, "md1 Right Hand spawner", "2"),
                            new EntityOrder(999, "md1 Left Hand spawner", "2"),
                            new TossObject("md1 CreepyHead", 5, 65),
                            new TossObject("md1 CreepyHead", 5, 90),
                            new TossObject("md1 CreepyHead", 5, 105),
                            new TimedTransition(0, "70")
                            ),
                        new State("70",
                            new TossObject("md1 Lava Makers", 14, coolDown: 500),
                            new TimedTransition(5500, "71")
                            )
                        ),
                    new State("71",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new EntityOrder(999, "md1 Governor", "1"),
                        new MoveTo(13.5f, 5, 0.8, true, true),
                        new TimedTransition(2000, "72")
                        ),
                    new State("72",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityOrder(999, "md1 Right Burst", "2"),
                        new EntityOrder(999, "md1 Mid Burst", "2"),
                        new EntityOrder(999, "md1 Left Burst", "2"),
                        new TimedTransition(0, "73")
                        ),
                    new State("73",
                        new HpLessTransition(0.2, "94"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "74")
                        ),
                    new State("74",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(8.5f, 5, 0.8, true, true),
                        new TimedTransition(2000, "75")
                        ),
                    new State("75",
                        new HpLessTransition(0.2, "94"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "76")
                        ),
                    new State("76",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(3.5f, 5, 0.8, true, true),
                        new TimedTransition(2000, "77")
                        ),
                    new State("77",
                        new HpLessTransition(0.2, "94"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "78")
                        ),
                    new State("78",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(true, 0.8),
                        new TimedTransition(2000, "79")
                        ),
                    new State("79",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityOrder(999, "md1 Right Hand spawner", "1"),
                        new EntityOrder(999, "md1 Left Hand spawner", "1"),
                        new EntityExistsTransition("md1 Right Hand of Shaitan", 999, "80")
                        ),
                    new State("80",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityExistsTransition("md1 Left Hand of Shaitan", 999, "81")
                        ),
                    new State("81",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityOrder(999, "md1 Right Hand of Shaitan", "TEST"),
                        new EntityOrder(999, "md1 Left Hand of Shaitan", "TEST"),
                        new TimedTransition(0, "82")
                        ),
                    new State("82",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Shoot(9 + 1 / 2, 1, index: 0, coolDown: 1000),
                        new EntitiesNotExistsTransition(999, "83", "md1 Right Hand of Shaitan", "md1 Left Hand of Shaitan")
                        ),
                    new State("83",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(0),
                        new ReturnToSpawn(true, 0.8),
                        new Shoot(9 + 1 / 2, 1, index: 0, coolDown: 1000),
                        new State("84",
                            new EntityOrder(999, "md1 Governor", "1"),
                            new EntityOrder(999, "md1 Right Hand spawner", "2"),
                            new EntityOrder(999, "md1 Left Hand spawner", "2"),
                            new TossObject("md1 CreepyHead", 5, 65),
                            new TossObject("md1 CreepyHead", 5, 90),
                            new TossObject("md1 CreepyHead", 5, 105),
                            new TimedTransition(0, "85")
                            ),
                        new State("85",
                            new TossObject("md1 Lava Makers", 14, coolDown: 500),
                            new TimedTransition(5500, "86")
                            )
                        ),
                    new State("86",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new EntityOrder(999, "md1 Governor", "1"),
                        new MoveTo(13.5f, 5, 0.8, true, true),
                        new TimedTransition(2000, "87")
                        ),
                    new State("87",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new EntityOrder(999, "md1 Right Burst", "2"),
                        new EntityOrder(999, "md1 Mid Burst", "2"),
                        new EntityOrder(999, "md1 Left Burst", "2"),
                        new TimedTransition(0, "88")
                        ),
                    new State("88",
                        new HpLessTransition(0.2, "94"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "89")
                        ),
                    new State("89",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(8.5f, 5, 0.8, true, true),
                        new TimedTransition(2000, "90")
                        ),
                    new State("90",
                        new HpLessTransition(0.2, "94"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "91")
                        ),
                    new State("91",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new MoveTo(3.5f, 5, 0.8, true, true),
                        new TimedTransition(2000, "92")
                        ),
                    new State("92",
                        new HpLessTransition(0.2, "94"),
                        new SetAltTexture(0),
                        new Shoot(8, 7, 15, 0, aim: 1, coolDown: 1000),
                        new TimedTransition(2000, "80")
                        ),
                    new State("94",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Taunt(true, "THE MAD GOD SHALL KNOW OF THESE TRANSGRESSIONS!"),
                        new TimedTransition(2000, "95")
                        ),
                    new State("95",
                        new Shoot(999, 5, 72, 0, 0, 0),
                        new Suicide()
                        )

                    )
            )
            .Init("md1 LeftHandSmash",
                new State(
                    new AddCond(ConditionEffectIndex.Invincible),
                    new State("1",
                        new EntityNotExistsTransition("md1 Head of Shaitan", 999, "2")
                        ),
                    new State("2",
                        new TimedTransition(2000, "3")
                        ),
                    new State("3",
                        new Spawn("md1 Loot Balloon Shaitan", 1, 0),
                        new Suicide()
                        )
                    )
            )
            .Init("md1 Governor",
                new State(
                    new AddCond(ConditionEffectIndex.Invincible),
                    new State("x"),
                    new State("1",
                        new Spawn("md1 CreepyHands", 2, 0),
                        new TimedTransition(1000, "x")
                        )
                    )
            )
            .Init("md1 CreepyHands",
                new State(
                    new Chase(0.5, 10, 2),
                    new Wander(2),
                    new Shoot(4, 6, 60, 0, coolDown: 500)
                    )
            )
            .Init("md1 Right Burst",
                new State(
                    new AddCond(ConditionEffectIndex.Invincible),
                    new ChangeGroundOnDeath(new[] { "Hot Lava" }, new[] { "Earth Light" }, 6),
                    new TransformOnDeath("md1 Right Burst"),
                    new State("1"),
                    new State("2",
                        new Suicide()
                        )
                    )
            )
            .Init("md1 Mid Burst",
                new State(
                    new AddCond(ConditionEffectIndex.Invincible),
                    new ChangeGroundOnDeath(new[] { "Hot Lava" }, new[] { "Earth Light" }, 6),
                    new TransformOnDeath("md1 Mid Burst"),
                    new State("1"),
                    new State("2",
                        new Suicide()
                        )
                    )
            )
            .Init("md1 Left Burst",
                new State(
                    new AddCond(ConditionEffectIndex.Invincible),
                    new ChangeGroundOnDeath(new[] { "Hot Lava" }, new[] { "Earth Light" }, 6),
                    new TransformOnDeath("md1 Left Burst"),
                    new State("1"),
                    new State("2",
                        new Suicide()
                        )
                    )
            )
            .Init("md1 Right Smashing Hand",
                new State(
                    new Chase(2, 15, 0),
                    new PlayerWithinTransition(2, "2"),
                    new State("1"),
                    new State("2",
                        new Shoot(999, 10, 36, 0, 0),
                        new Suicide()
                        )
                    )
            )
            .Init("md1 Left Smashing Hand",
                new State(
                    new Chase(2, 15, 0),
                    new PlayerWithinTransition(2, "2"),
                    new State("1"),
                    new State("2",
                        new Shoot(999, 10, 36, 0, 0),
                        new Suicide()
                        )
                    )
            )
            .Init("md1 Lava Makers",
                new State(
                    new ChangeGroundOnDeath(new[] { "Earth Light" }, new[] { "Hot Lava" }, 1),
                    new Suicide()
                    )
            )
            .Init("md1 CreepyHead",
                new State(
                    new Chase(0.2, 15, 4),
                    new Shoot(6, 2, 10, 0, coolDown: 400)
                    )
            )
            .Init("md1 Right Hand spawner",
                new State(
                    new AddCond(ConditionEffectIndex.Invincible),
                    new State("x"),
                    new State("1",
                        new Spawn("md1 Right Hand of Shaitan", 1, 0),
                        new TimedTransition(1000, "x")
                        ),
                    new State("2",
                        new Spawn("md1 Right Smashing Hand", 1, 0),
                        new TimedTransition(2000, "3")
                        ),
                    new State("3",
                        new Spawn("md1 Right Smashing Hand", 1, 0),
                        new TimedTransition(2000, "x")
                        )
                    )
            )
            .Init("md1 Left Hand spawner",
                new State(
                    new AddCond(ConditionEffectIndex.Invincible),
                    new State("x"),
                    new State("1",
                        new Spawn("md1 Left Hand of Shaitan", 1, 0),
                        new TimedTransition(1000, "x")
                        ),
                    new State("2",
                        new Spawn("md1 Left Smashing Hand", 1, 0),
                        new TimedTransition(2000, "3")
                        ),
                    new State("3",
                        new Spawn("md1 Left Smashing Hand", 1, 0),
                        new TimedTransition(2000, "x")
                        )
                    )
            )
            .Init("md1 Right Hand of Shaitan",
                new State(
                    new State("1",
                        new AddCond(ConditionEffectIndex.Invincible)
                        ),
                    new State("2",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new ChangeSize(20, 200),
                        new TimedTransition(7000, "3")
                        ),
                    new State("3",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new Taunt(true, "Hah. Weakings. This will be slightly enjoyable.", "You are in the presence of demi-god, motral", "What hath the keepers brought Shaitan?", "You disturb an ancient evil...", "You think it wise to use such cheap tricks?", "You make a foolish mistake, mortal."),
                        new State("4",
                            new SetAltTexture(1),
                            new Spawn("md1 Right Hand spawner", 1, 0),
                            new TimedTransition(400, "5")
                            ),
                        new State("5",
                            new SetAltTexture(2),
                            new TimedTransition(400, "6")
                            ),
                        new State("6",
                            new SetAltTexture(1),
                            new TimedTransition(400, "7")
                            ),
                        new State("7",
                            new SetAltTexture(2),
                            new TimedTransition(400, "8")
                            ),
                        new State("8",
                            new SetAltTexture(1),
                            new TimedTransition(400, "9")
                            ),
                        new State("9",
                            new SetAltTexture(2),
                            new TimedTransition(400, "10")
                            ),
                        new State("10",
                            new SetAltTexture(1),
                            new TimedTransition(400, "11")
                            ),
                        new State("11",
                            new SetAltTexture(2),
                            new TimedTransition(400, "12")
                            ),
                        new State("12",
                            new EntityOrder(999, "md1 Left Hand of Shaitan", "3"),
                            new TimedTransition(0, "13")
                            ),
                        new State("13",
                            new SetAltTexture(0)
                            )
                        ),
                        new State("14",//unset conditional effect
                            new AddCond(ConditionEffectIndex.Invulnerable),
                            new EntityOrder(999, "md1 Head of Shaitan", "5"),
                            new TimedTransition(0, "15")
                            ),
                        new State("TEST",
                            new AddCond(ConditionEffectIndex.Invincible),
                            new TimedTransition(0, "15")
                            ),
                        new State("15",
                            new Shoot(999, 6, 25, 0, 0, 45, coolDown: 500),
                            new TimedTransition(4000, "16")
                            ),
                        new State("16",
                            new Chase(0.8, 99, 1),
                            new Shoot(999, 6, 60, 2, 0, 25, coolDown: 750),
                            new TimedTransition(2000, "17")
                            ),
                        new State("17",
                            new Taunt(true, "Hah. Weakings. This will be slightly enjoyable.", "You are in the presence of demi-god, motral", "What hath the keepers brought Shaitan?", "You disturb an ancient evil...", "You think it wise to use such cheap tricks?", "You make a foolish mistake, mortal."),
                            new ReturnToSpawn(true, 0.8),
                            new Flashing(0xFF0000, .2, 12),
                            new State("18",
                                new SetAltTexture(1),
                                new TimedTransition(400, "19")
                                ),
                            new State("19",
                                new SetAltTexture(2),
                                new TimedTransition(400, "20")
                                ),
                            new State("20",
                                new SetAltTexture(1),
                                new TimedTransition(400, "21")
                                ),
                            new State("21",
                                new SetAltTexture(2),
                                new TimedTransition(400, "22")
                                ),
                            new State("22",
                                new SetAltTexture(0),
                                new TimedTransition(0, "15")
                                )
                            )
                        )
                    )
            .Init("md1 Left Hand of Shaitan",
                new State(
                    new State("1",
                        new AddCond(ConditionEffectIndex.Invincible)
                        ),
                    new State("2",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new ChangeSize(20, 200)
                        ),
                    new State("3",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new AddCond(ConditionEffectIndex.Invincible),
                        new Taunt(true, "Yes, little mortals. Meet your doom at the hands of SHAITAN!", "My firey fingers of frustrating flame force foes to fumble, fall, and fail!", "You think it wise to use such cheap tricks?", "You make a foolish mistake, mortal."),
                        new State("4",
                            new SetAltTexture(1),
                            new Spawn("md1 Left Hand spawner", 1, 0),
                            new TimedTransition(400, "5")
                            ),
                        new State("5",
                            new SetAltTexture(2),
                            new TimedTransition(400, "6")
                            ),
                        new State("6",
                            new SetAltTexture(1),
                            new TimedTransition(400, "7")
                            ),
                        new State("7",
                            new SetAltTexture(2),
                            new TimedTransition(400, "8")
                            ),
                        new State("8",
                            new SetAltTexture(1),
                            new TimedTransition(400, "9")
                            ),
                        new State("9",
                            new SetAltTexture(2),
                            new TimedTransition(400, "10")
                            ),
                        new State("10",
                            new SetAltTexture(1),
                            new TimedTransition(400, "11")
                            ),
                        new State("11",
                            new SetAltTexture(2),
                            new TimedTransition(400, "12")
                            ),
                        new State("12",
                            new EntityOrder(999, "md1 Right Hand of Shaitan", "14"),
                            new TimedTransition(0, "13")
                            )
                        ),
                        new State("TEST",
                            new AddCond(ConditionEffectIndex.Invincible),
                            new TimedTransition(0, "13")
                            ),
                        new State("13",
                            new SetAltTexture(0),
                            new Shoot(999, 6, 25, 0, 0, 135, coolDown: 500),
                            new TimedTransition(4000, "14")
                            ),
                        new State("14",
                            new Chase(0.8, 99, 1),
                            new Shoot(999, 6, 60, 2, 0, 25, coolDown: 750),
                            new TimedTransition(2000, "15")
                            ),
                        new State("15",
                           new Taunt(true, "Yes, little mortals. Meet your doom at the hands of SHAITAN!", "My firey fingers of frustrating flame force foes to fumble, fall, and fail!", "You think it wise to use such cheap tricks?", "You make a foolish mistake, mortal."),
                            new ReturnToSpawn(true, 0.8),
                            new Flashing(0xFF0000, .2, 12),
                            new State("16",
                                new SetAltTexture(1),
                                new TimedTransition(400, "17")
                                ),
                            new State("17",
                                new SetAltTexture(2),
                                new TimedTransition(400, "18")
                                ),
                            new State("18",
                                new SetAltTexture(1),
                                new TimedTransition(400, "19")
                                ),
                            new State("19",
                                new SetAltTexture(2),
                                new TimedTransition(400, "13")
                                )
                            )
                        )
                    )
            .Init("md1 Loot Balloon Shaitan",
                new State(
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "UnsetEffect")
                    ),
                    new State("UnsetEffect")
                ),
                new MostDamagers(1,
                    new ItemLoot("Potion of Attack", 1)
                    ),
                new MostDamagers(4,
                    new ItemLoot("Potion of Defense", 0.25)
                    ),
                new Threshold(0.01,
                    new TierLoot(10, ItemType.Weapon, 0.1),
                    new TierLoot(11, ItemType.Weapon, 0.05),
                    new TierLoot(10, ItemType.Armor, 0.1),
                    new ItemLoot("Effusion of Defense", 0.06),
                    new ItemLoot("Tincture of Dexterity", 0.06),
                    new ItemLoot("Large Flame Cloth", 0.05),
                    new ItemLoot("Small Flame Cloth", 0.05),
                    new ItemLoot("Large Crossbox Cloth", 0.05),
                    new ItemLoot("Small Crossbox Cloth", 0.05),
                    new ItemLoot("Large Heavy Chainmail Cloth", 0.03),
                    new ItemLoot("Small Heavy Chainmail Cloth", 0.03),
                    new ItemLoot("The Chariot Tarot Card", 0.04),
                    new ItemLoot("Skull of Endless Torment", 0.01)
                )
            );
    }
}