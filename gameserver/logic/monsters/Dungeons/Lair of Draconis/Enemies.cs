using gameserver.logic.behaviors;
using gameserver.logic.loot;
using gameserver.logic.transitions;

namespace gameserver.logic
{
    partial class BehaviorDb
    {
        private _ LairofDraconis = () => Behav()
            .Init("NM Altar of Draconis",
                new State(
                    new AddCond(ConditionEffectIndex.Invincible),
                new State("Normal or Hardmode",
                    new TimedTransition(1000, "normal", true),
                    new TimedTransition(1000, "hardmode", true)
                    ),
                new State("normal",
                    new Taunt(true, "Choose the Dragon Soul you wish to commune with!"),
                    new ChatTransition("blue check normal", "blue"),
                    new ChatTransition("red check normal", "red"),
                    new ChatTransition("green check normal", "green"),
                    new ChatTransition("black check normal", "black")
                    ),
                new State("normal2",
                    new EntitiesNotExistsTransition(999, "RealmPortal", "NM Blue Dragon Spawner", "NM Red Dragon Spawner", "NM Green Dragon Spawner", "NM Black Dragon Spawner"),
                    new ChatTransition("blue check normal", "blue"),
                    new ChatTransition("red check normal", "red"),
                    new ChatTransition("green check normal", "green"),
                    new ChatTransition("black check normal", "black")
                    ),
                new State("hardmode",
                    new Taunt(true, "Choose the Dragon Soul you wish to commune with!"),
                    new ChatTransition("blue check hardmode", "blue"),
                    new ChatTransition("red check hardmode", "red"),
                    new ChatTransition("green check hardmode", "green"),
                    new ChatTransition("black check hardmode", "black")
                    ),
                new State("hardmode2",
                    new EntitiesNotExistsTransition(999, "Ivory Wyvern Portal", "NM Blue Dragon Spawner", "NM Red Dragon Spawner", "NM Green Dragon Spawner", "NM Black Dragon Spawner"),
                    new ChatTransition("blue check hardmode", "blue"),
                    new ChatTransition("red check hardmode", "red"),
                    new ChatTransition("green check hardmode", "green"),
                    new ChatTransition("black check hardmode", "black")
                    ),
                new State("RealmPortal",
                    new DropPortalOnDeath("Realm Portal", 100, 0, 6, 0, 120),
                    new Suicide()
                    ),
                new State("Ivory Wyvern Portal",
                    new DropPortalOnDeath("Ivory Wyvern Portal", 100, 0, 6, 0, 120),
                    new Suicide()
                    ),
                new State("blue check normal",
                    new EntitiesNotExistsTransition(999, "normal2", "NM Blue Dragon Spawner"),
                    new EntityExistsTransition("NM Blue Dragon Spawner", 999, "blue normal")
                    ),
                new State("blue normal",
                    new Taunt(true, "Do not let the tranquil surroundigps fool you!"),
                    new EntityOrder(99, "NM Blue Dragon Soul", "despawn"),
                    new EntityOrder(999, "NM Blue Dragon Spawner", "normal"),
                    new EntityOrder(999, "NM Blue Open Wall", "1"),
                    new TimedTransition(1000, "blue death normal check")
                    ),
                new State("blue death normal check",
                    new EntityExistsTransition("NM Blue Dragon Dead", 9999, "normal2")
                    ),
                new State("blue check hardmode",
                    new EntitiesNotExistsTransition(999, "hardmode2", "NM Blue Dragon Spawner"),
                    new EntityExistsTransition("NM Blue Dragon Spawner", 999, "blue hardmode")
                    ),
                new State("blue hardmode",
                    new Taunt(true, "Do not let the tranquil surroundigps fool you!"),
                    new EntityOrder(99, "NM Blue Dragon Soul", "despawn"),
                    new EntityOrder(999, "NM Blue Dragon Spawner", "hardmode"),
                    new EntityOrder(999, "NM Blue Open Wall", "1"),
                    new TimedTransition(1000, "blue death hardmode check")
                    ),
                new State("blue death hardmode check",
                    new EntityExistsTransition("NM Blue Dragon Dead", 9999, "hardmode2")
                    ),
                new State("red check normal",
                    new EntitiesNotExistsTransition(999, "normal2", "NM Red Dragon Spawner"),
                    new EntityExistsTransition("NM Red Dragon Spawner", 999, "red normal")
                    ),
                new State("red normal",
                    new Taunt(true, "Burns!!! Pyyr will rend your flesh and char your bones!"),
                    new EntityOrder(99, "NM Red Dragon Soul", "despawn"),
                    new EntityOrder(999, "NM Red Dragon Spawner", "normal"),
                    new EntityOrder(999, "NM Red Open Wall", "1"),
                    new TimedTransition(500, "red death normal check")
                    ),
                new State("red death normal check",
                    new EntityExistsTransition("NM Red Dragon Dead", 9999, "normal2")
                    ),
                new State("red check hardmode",
                    new EntitiesNotExistsTransition(999, "hardmode2", "NM Red Dragon Spawner"),
                    new EntityExistsTransition("NM Red Dragon Spawner", 999, "red hardmode")
                    ),
                new State("red hardmode",
                    new Taunt(true, "Burns!!! Pyyr will rend your flesh and char your bones!"),
                    new EntityOrder(99, "NM Red Dragon Soul", "despawn"),
                    new EntityOrder(999, "NM Red Dragon Spawner", "hardmode"),
                    new EntityOrder(999, "NM Red Open Wall", "1"),
                    new TimedTransition(1000, "red death hardmode check")
                    ),
                new State("red death hardmode check",
                    new EntityExistsTransition("NM Red Dragon Dead", 9999, "hardmode2")
                    ),
                new State("green check normal",
                    new EntitiesNotExistsTransition(999, "normal2", "NM Green Dragon Spawner"),
                    new EntityExistsTransition("NM Green Dragon Spawner", 999, "green normal")
                    ),
                new State("green normal",
                    new Taunt(true, "Limoz is the nicest of the lot, but he still hates all sub creatures!"),
                    new EntityOrder(99, "NM Green Dragon Soul", "despawn"),
                    new EntityOrder(999, "NM Green Dragon Spawner", "normal"),
                    new EntityOrder(999, "NM Green Open Wall", "1"),
                    new TimedTransition(1000, "green death normal check")
                    ),
                new State("green death normal check",
                    new EntityExistsTransition("NM Green Dragon Dead", 9999, "normal2")
                    ),
                new State("green check hardmode",
                    new EntitiesNotExistsTransition(999, "hardmode2", "NM Green Dragon Spawner"),
                    new EntityExistsTransition("NM Green Dragon Spawner", 999, "green hardmode")
                    ),
                new State("green hardmode",
                    new Taunt(true, "Limoz is the nicest of the lot, but he still hates all sub creatures!"),
                    new EntityOrder(99, "NM Green Dragon Soul", "despawn"),
                    new EntityOrder(999, "NM Green Dragon Spawner", "hardmode"),
                    new EntityOrder(999, "NM Green Open Wall", "1"),
                    new TimedTransition(1000, "green death hardmode check")
                    ),
                new State("green death hardmode check",
                    new EntityExistsTransition("NM Green Dragon Dead", 9999, "hardmode2")
                    ),
                new State("black check normal",
                    new EntitiesNotExistsTransition(999, "normal2", "NM Black Dragon Spawner"),
                    new EntityExistsTransition("NM Black Dragon Spawner", 999, "black normal")
                    ),
                new State("black normal",
                    new Taunt(true, "Gaze into the darkness... Feargus will consume you!"),
                    new EntityOrder(99, "NM Black Dragon Soul", "despawn"),
                    new EntityOrder(999, "NM Black Dragon Spawner", "normal"),
                    new EntityOrder(999, "NM Black Dragon Minion", "2"),
                    new EntityOrder(999, "NM Black Open Wall", "1"),
                    new TimedTransition(1000, "black death normal check")
                    ),
                new State("black death normal check",
                    new EntityExistsTransition("NM Black Dragon Dead", 9999, "normal2")
                    ),
                new State("black check hardmode",
                    new EntitiesNotExistsTransition(999, "hardmode2", "NM Black Dragon Spawner"),
                    new EntityExistsTransition("NM Black Dragon Spawner", 999, "black hardmode")
                    ),
                new State("black hardmode",
                    new Taunt(true, "Gaze into the darkness... Feargus will consume you!"),
                    new EntityOrder(99, "NM Black Dragon Soul", "despawn"),
                    new EntityOrder(999, "NM Black Dragon Spawner", "hardmode"),
                    new EntityOrder(999, "NM Black Dragon Minion", "2"),
                    new EntityOrder(999, "NM Black Open Wall", "1"),
                    new TimedTransition(1000, "black death hardmode check")
                    ),
                new State("black death hardmode check",
                    new EntityExistsTransition("NM Black Dragon Dead", 9999, "hardmode2")
                    )
                )
            )
            .Init("NM Blue Dragon Soul",
                new State(
                    new AddCond(ConditionEffectIndex.Invincible),
                    new State("Orbit",
                        new Circle(0.3, 4, 10, "NM Altar of Draconis")
                        ),
                    new State("despawn",
                        new Aoe(1, false, 0, 0, false, 0xFFFFFF),
                        new Decay(0)
                        )
                    )
            )
            .Init("NM Red Dragon Soul",
                new State(
                    new AddCond(ConditionEffectIndex.Invincible),
                    new State("Orbit",
                        new Circle(0.3, 4, 10, "NM Altar of Draconis")
                        ),
                    new State("despawn",
                        new Aoe(1, false, 0, 0, false, 0xFFFFFF),
                        new Decay(0)
                        )
                    )
            )
            .Init("NM Green Dragon Soul",
                new State(
                    new AddCond(ConditionEffectIndex.Invincible),
                    new State("Orbit",
                        new Circle(0.3, 4, 10, "NM Altar of Draconis")
                        ),
                    new State("despawn",
                        new Aoe(1, false, 0, 0, false, 0xFFFFFF),
                        new Decay(0)
                        )
                    )
            )
            .Init("NM Black Dragon Soul",
                new State(
                    new AddCond(ConditionEffectIndex.Invincible),
                    new State("Orbit",
                        new Circle(0.3, 4, 10, "NM Altar of Draconis")
                        ),
                    new State("despawn",
                        new Aoe(1, false, 0, 0, false, 0xFFFFFF),
                        new Decay(0)
                        )
                    )
            )
            .Init("NM Blue Open Wall",
                new State(
                    new AddCond(ConditionEffectIndex.Invincible),
                    new State("nothin"),
                    new State("1",
                    new ApplySetpiece("BlueOpenWall")
                        )
                    )
                )
            .Init("NM Blue Dragon God",
                new State(
                    new SetAltTexture(1),
                    new StayCloseToSpawn(0.5, 24),
                    new HpLessTransition(0.05, "20"),
                    new TransformOnDeath("lod Blue Loot Balloon"),
                    new OnDeathBehavior(new EntityOrder(99, "NM Blue Dragon Spawner", "despawn")),
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new State("1",
                            new PlayerWithinTransition(3, "pre_2")
                            ),
                        new State("pre_2",
                            new EntityOrder(999, "drac wall blue", "spawn"),
                            new TimedTransition(500, "2")
                            ),
                        new State("2",
                            new Flashing(0xFFFFFF, 0.2, 12),
                            new TimedTransition(2500, "3")
                            ),
                        new State("3",
                            new SetAltTexture(0),
                            new Taunt("My children will feast on your soul!"),
                            new TimedTransition(1000, "3_spawn")
                            ),
                        new State("3_spawn",
                            new EntityOrder(99, "NM Blue Minion Spawner", "spawn"),
                            new TimedTransition(0, "4")
                            )
                        ),
                    new State("4",
                        new HpLessTransition(0.75, "8"),
                        new Prioritize(
                            new Chase(0.3, 20, 1)
                            ),
                        new Shoot(20, 4, 45, 1, 0, 90, coolDown: 2000),
                        new Shoot(20, 4, 15, 2, 0, 90, coolDown: 2000),
                        new Shoot(20, 6, 30, 4, 0, 315, coolDown: 2500),
                        new State("5",
                            new Shoot(20, 6, 25, 3, 0, 0),
                            new Shoot(20, 6, 15, 5, 0, 315),
                            new TimedTransition(500, "6")
                            ),
                        new State("6",
                            new Shoot(20, 6, 25, 3, 0, 90),
                            new Shoot(20, 6, 15, 5, 0, 270),
                            new TimedTransition(500, "7")
                            ),
                        new State("7",
                            new Shoot(20, 6, 15, 5, 0, 225),
                            new Shoot(20, 6, 15, 5, 0, 135),
                            new TimedTransition(500, "5")
                            )
                        ),
                    new State("8",
                        new ReturnToSpawn(true, 0.7),
                        new Flashing(0xFFFFFF, 0.2, 12),
                        new Taunt("Look out! My minions will help me!"),
                        new EntityOrder(99, "NM Blue Dragon Minion", "Protect"),
                        new HpLessTransition(0.5, "12"),
                        new Shoot(20, 4, 45, 1, 0, 90, coolDown: 2000),
                        new Shoot(20, 4, 15, 2, 0, 90, coolDown: 2000),
                        new Shoot(20, 6, 30, 4, 0, 315, coolDown: 2500),
                        new Shoot(20, 1, index: 0, coolDown: 1000),
                        new State("9",
                            new Shoot(20, 6, 25, 3, 0, 0),
                            new Shoot(20, 6, 15, 5, 0, 315),
                            new TimedTransition(500, "10")
                            ),
                        new State("10",
                            new Shoot(20, 6, 25, 3, 0, 90),
                            new Shoot(20, 6, 15, 5, 0, 270),
                            new TimedTransition(500, "11")
                            ),
                        new State("11",
                            new Shoot(20, 6, 15, 5, 0, 225),
                            new Shoot(20, 6, 15, 5, 0, 135),
                            new TimedTransition(500, "9")
                            )
                        ),
                    new State("12",
                        new Flashing(0xFFFFFF, 0.2, 12),
                        new Taunt("You're a nasty little pest!"),
                        new Prioritize(
                            new Chase(0.3, 20, 1)
                            ),
                        new HpLessTransition(0.25, "16"),
                        new Shoot(20, 4, 45, 1, 0, 90, coolDown: 2000),
                        new Shoot(20, 4, 15, 2, 0, 90, coolDown: 2000),
                        new Shoot(20, 6, 30, 4, 0, 315, coolDown: 2500),
                        new Shoot(20, 1, index: 0, coolDown: 1000),
                        new State("13",
                            new Shoot(20, 6, 25, 3, 0, 0),
                            new Shoot(20, 6, 15, 5, 0, 315),
                            new TimedTransition(500, "14")
                            ),
                        new State("14",
                            new Shoot(20, 6, 25, 3, 0, 90),
                            new Shoot(20, 6, 15, 5, 0, 270),
                            new TimedTransition(500, "15")
                            ),
                        new State("15",
                            new Shoot(20, 6, 15, 5, 0, 225),
                            new Shoot(20, 6, 15, 5, 0, 135),
                            new TimedTransition(500, "13")
                            )
                        ),
                    new State("16",
                        new Flashing(0xFFFFFF, 0.2, 12),
                        new Taunt("You cannot handle the full power of my onslaught!"),
                        new Prioritize(
                            new Wander(0.6)
                            ),
                        new HpLessTransition(0.05, "20"),
                        new Shoot(20, 4, 45, 1, 0, 90, coolDown: 2000),
                        new Shoot(20, 4, 15, 2, 0, 90, coolDown: 2000),
                        new Shoot(20, 6, 30, 4, 0, 315, coolDown: 2500),
                        new Shoot(20, 1, index: 0, coolDown: 1000),
                        new Shoot(20, 24, 15, 3, 0, coolDown: 2000),
                        new State("spawn2",
                            new EntityOrder(99, "NM Blue Minion Spawner", "spawn2"),
                            new TimedTransition(100, "17")
                            ),
                        new State("17",
                            new Shoot(20, 6, 15, 5, 0, 315),
                            new TimedTransition(500, "18")
                            ),
                        new State("18",
                            new Shoot(20, 6, 15, 5, 0, 270),
                            new TimedTransition(500, "19")
                            ),
                        new State("19",
                            new Shoot(20, 6, 15, 5, 0, 225),
                            new Shoot(20, 6, 15, 5, 0, 135),
                            new TimedTransition(500, "17")
                            )
                        ),
                    new State("20",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new ReturnToSpawn(true, 0.6),
                        new Taunt("Nooooooo! This cannot be! i have underestimated your power..."),
                        new TimedTransition(3000, "21")
                        ),
                    new State("21",
                        new Suicide()
                        )
                    )
            )
        .Init("NM Blue Minion Spawner",
            new State(
                new AddCond(ConditionEffectIndex.Invincible),
                new State("Waiting"),
                new State("spawn",
                    new Spawn("NM Blue Dragon Minion", 2, 0),
                    new TimedTransition(100, "Wait1")
                    ),
                new State("Wait1"),
                new State("spawn2",
                    new Spawn("NM Blue Dragon Minion", 2, 0),
                    new TimedTransition(100, "Wait1")
                    )
                )
            )
        .Init("NM Blue Dragon Minion",
            new State(
                new EntitiesNotExistsTransition(99, "Die", "NM Blue Dragon God", "NM Blue Dragon God Hardmode"),
                new State("1",
                    new Circle(0.4, 6, 10, null),
                    new Wander(0.2),
                    new Shoot(10, 1, index: 2, coolDown: 1000)
                    ),
                new State("Protect",
                    new Protect(0.6, "NM Blue Dragon God", 99, 4),
                    new Protect(0.6, "NM Blue Dragon God Hardmode", 99, 4),
                    new Wander(0.2),
                    new Shoot(10, 1, index: 2, coolDown: 1000)
                    ),
                new State("Die",
                    new Suicide()
                    )
                )
            )
        .Init("drac floor blue",
            new State(
                new AddCond(ConditionEffectIndex.Invincible)
                )
            )
        .Init("drac wall blue",
            new State(
                new AddCond(ConditionEffectIndex.Invincible),
                new State("nothin"),
                new State("spawn",
                    new ApplySetpiece("BlueCloseWall1"),
                    new TimedTransition(3000, "spawn2")
                    ),
                new State("spawn2",
                    new ApplySetpiece("BlueCloseWall2"),
                    new TimedTransition(3000, "spawn3")
                    ),
                new State("spawn3",
                    new ApplySetpiece("BlueCloseWall3"),
                    new TimedTransition(3000, "spawn4")
                    ),
                new State("spawn4",
                    new ApplySetpiece("BlueCloseWall4"),
                    new TimedTransition(3000, "spawn5")
                    ),
                new State("spawn5",
                    new ApplySetpiece("BlueCloseWall5")
                    )
                )
            )
        .Init("Ice Cave Wall",
            new State(
                new State("2",
                    new EntityExistsTransition("lod Blue Loot Balloon", 999, "3_1"),
                    new EntityExistsTransition("lod Blue HM Loot Balloon", 999, "3_2")
                    ),
                new State("3_1",
                    new EntityNotExistsTransition("lod Blue Loot Balloon", 999, "despawn")
                    ),
                new State("3_2",
                    new EntityNotExistsTransition("lod Blue HM Loot Balloon", 999, "despawn")
                    ),
                new State("despawn",
                    new Decay(0)
                    )
                )
            )
        .Init("NM Blue Dragon Spawner",
            new State(
                new AddCond(ConditionEffectIndex.Invincible),
                new State("nothin"),
                new State("normal",
                    new Spawn("NM Blue Dragon God", 1, 0)
                    ),
                new State("hardmode",
                    new Spawn("NM Blue Dragon God Hardmode", 1, 0)
                    ),
                new State("despawn",
                    new Decay(0)
                    )
                )
            )
            .Init("lod Blue Loot Balloon",
                new State(
                    new TransformOnDeath("NM Blue Dragon Dead"),
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "UnsetEffect")
                    ),
                    new State("UnsetEffect")
                ),
                new MostDamagers(2,
                    new TierLoot(2, ItemType.Potion, 1)
                    ),
                new Threshold(0.01,
                    new TierLoot(12, ItemType.Weapon, 0.03),
                    new TierLoot(11, ItemType.Weapon, 0.04),
                    new TierLoot(10, ItemType.Weapon, 0.05),
                    new TierLoot(13, ItemType.Armor, 0.03),
                    new TierLoot(12, ItemType.Armor, 0.04),
                    new TierLoot(11, ItemType.Armor, 0.05),
                    new ItemLoot("Large Blue Dragon Scale Cloth", 0.08),
                    new ItemLoot("Small Blue Dragon Scale Cloth", 0.08),
                    new ItemLoot("Potion of Mana", 0.03),
                    new ItemLoot("Wine Cellar Incantation", 0.008),
                    new ItemLoot("Water Dragon Silk Robe", 0.008)
                )
            )
            .Init("NM Blue Dragon God Hardmode",
                new State(
                    new SetAltTexture(1),
                    new StayCloseToSpawn(0.5, 24),
                    new HpLessTransition(0.05, "20"),
                    new TransformOnDeath("lod Blue HM Loot Balloon"),
                    new OnDeathBehavior(new EntityOrder(99, "NM Blue Dragon Spawner", "despawn")),
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new State("1",
                            new PlayerWithinTransition(3, "pre_2")
                            ),
                        new State("pre_2",
                            new EntityOrder(999, "drac wall blue", "spawn"),
                            new TimedTransition(500, "2")
                            ),
                        new State("2",
                            new Flashing(0xFFFFFF, 0.2, 12),
                            new TimedTransition(2500, "3")
                            ),
                        new State("3",
                            new SetAltTexture(0),
                            new Taunt("My children will feast on your soul!"),
                            new TimedTransition(1000, "3_spawn")
                            ),
                        new State("3_spawn",
                            new EntityOrder(99, "NM Blue Minion Spawner", "spawn"),
                            new TimedTransition(0, "4")
                            )
                        ),
                    new State("4",
                        new HpLessTransition(0.75, "8"),
                        new Prioritize(
                            new Chase(0.3, 20, 1)
                            ),
                        new Shoot(20, 1, index: 4, coolDown: 1000),
                        new Shoot(20, 4, 45, 1, 0, 90, coolDown: 2000),
                        new Shoot(20, 4, 15, 2, 0, 90, coolDown: 2000),
                        new Shoot(20, 6, 30, 5, 0, 315, coolDown: 2500),
                        new State("5",
                            new Shoot(20, 6, 25, 3, 0, 0),
                            new Shoot(20, 6, 15, 5, 0, 315),
                            new TimedTransition(500, "6")
                            ),
                        new State("6",
                            new Shoot(20, 6, 25, 3, 0, 90),
                            new Shoot(20, 6, 15, 6, 0, 270),
                            new TimedTransition(500, "7")
                            ),
                        new State("7",
                            new Shoot(20, 6, 15, 6, 0, 225),
                            new Shoot(20, 6, 15, 6, 0, 135),
                            new TimedTransition(500, "5")
                            )
                        ),
                    new State("8",
                        new ReturnToSpawn(true, 0.7),
                        new Flashing(0xFFFFFF, 0.2, 12),
                        new Taunt("Look out! My minions will help me!"),
                        new EntityOrder(99, "NM Blue Dragon Minion", "Protect"),
                        new HpLessTransition(0.5, "12"),
                        new Shoot(20, 1, index: 4, coolDown: 1000),
                        new Shoot(20, 4, 45, 1, 0, 90, coolDown: 2000),
                        new Shoot(20, 4, 15, 2, 0, 90, coolDown: 2000),
                        new Shoot(20, 6, 30, 5, 0, 315, coolDown: 2500),
                        new Shoot(20, 1, index: 0, coolDown: 1000),
                        new State("9",
                            new Shoot(20, 6, 25, 3, 0, 0),
                            new Shoot(20, 6, 15, 6, 0, 315),
                            new TimedTransition(500, "10")
                            ),
                        new State("10",
                            new Shoot(20, 6, 25, 3, 0, 90),
                            new Shoot(20, 6, 15, 6, 0, 270),
                            new TimedTransition(500, "11")
                            ),
                        new State("11",
                            new Shoot(20, 6, 15, 6, 0, 225),
                            new Shoot(20, 6, 15, 6, 0, 135),
                            new TimedTransition(500, "9")
                            )
                        ),
                    new State("12",
                        new Flashing(0xFFFFFF, 0.2, 12),
                        new Taunt("You're a nasty little pest!"),
                        new Prioritize(
                            new Chase(0.3, 20, 1)
                            ),
                        new HpLessTransition(0.25, "16"),
                        new Shoot(20, 1, index: 4, coolDown: 1000),
                        new Shoot(20, 4, 45, 1, 0, 90, coolDown: 2000),
                        new Shoot(20, 4, 15, 2, 0, 90, coolDown: 2000),
                        new Shoot(20, 6, 30, 5, 0, 315, coolDown: 2500),
                        new Shoot(20, 1, index: 0, coolDown: 1000),
                        new State("13",
                            new Shoot(20, 6, 25, 3, 0, 0),
                            new Shoot(20, 6, 15, 6, 0, 315),
                            new TimedTransition(500, "14")
                            ),
                        new State("14",
                            new Shoot(20, 6, 25, 3, 0, 90),
                            new Shoot(20, 6, 15, 6, 0, 270),
                            new TimedTransition(500, "15")
                            ),
                        new State("15",
                            new Shoot(20, 6, 15, 6, 0, 225),
                            new Shoot(20, 6, 15, 6, 0, 135),
                            new TimedTransition(500, "13")
                            )
                        ),
                    new State("16",
                        new Flashing(0xFFFFFF, 0.2, 12),
                        new Taunt("You cannot handle the full power of my onslaught!"),
                        new Prioritize(
                            new Wander(0.6)
                            ),
                        new HpLessTransition(0.05, "20"),
                        new Shoot(20, 1, index: 4, coolDown: 1000),
                        new Shoot(20, 4, 45, 1, 0, 90, coolDown: 2000),
                        new Shoot(20, 4, 15, 2, 0, 90, coolDown: 2000),
                        new Shoot(20, 6, 30, 5, 0, 315, coolDown: 2500),
                        new Shoot(20, 1, index: 0, coolDown: 1000),
                        new Shoot(20, 24, 15, 3, 0, coolDown: 2000),
                        new State("spawn2",
                            new EntityOrder(99, "NM Blue Minion Spawner", "spawn2"),
                            new TimedTransition(100, "17")
                            ),
                        new State("17",
                            new Shoot(20, 6, 15, 6, 0, 315),
                            new TimedTransition(500, "18")
                            ),
                        new State("18",
                            new Shoot(20, 6, 15, 6, 0, 270),
                            new TimedTransition(500, "19")
                            ),
                        new State("19",
                            new Shoot(20, 6, 15, 6, 0, 225),
                            new Shoot(20, 6, 15, 6, 0, 135),
                            new TimedTransition(500, "17")
                            )
                        ),
                    new State("20",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new ReturnToSpawn(true, 0.6),
                        new Taunt("Nooooooo! This cannot be! i have underestimated your power..."),
                        new TimedTransition(3000, "21")
                        ),
                    new State("21",
                        new Suicide()
                        )
                    )
            )
            .Init("lod Blue HM Loot Balloon",
                new State(
                    new TransformOnDeath("NM Blue Dragon Dead"),
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "UnsetEffect")
                    ),
                    new State("UnsetEffect")
                ),
                new MostDamagers(3,
                    new TierLoot(2, ItemType.Potion, 1)
                    ),
                new Threshold(0.01,
                    new TierLoot(12, ItemType.Weapon, 0.05),
                    new TierLoot(11, ItemType.Weapon, 0.06),
                    new TierLoot(10, ItemType.Weapon, 0.07),
                    new TierLoot(13, ItemType.Armor, 0.05),
                    new TierLoot(12, ItemType.Armor, 0.06),
                    new TierLoot(11, ItemType.Armor, 0.07),
                    new ItemLoot("Large Blue Dragon Scale Cloth", 0.1),
                    new ItemLoot("Small Blue Dragon Scale Cloth", 0.1),
                    new ItemLoot("Potion of Mana", 0.05),
                    new ItemLoot("Wine Cellar Incantation", 0.01),
                    new ItemLoot("Water Dragon Silk Robe", 0.01)
                )
            )
        .Init("NM Red Dragon Spawner",
            new State(
                new AddCond(ConditionEffectIndex.Invincible),
                new State("nothin"),
                new State("normal",
                    new Spawn("NM Red Dragon God", 1, 0)
                    ),
                new State("hardmode",
                    new Spawn("NM Red Dragon God Hardmode", 1, 0)
                    ),
                new State("despawn",
                    new Decay(0)
                    )
                )
            )
            .Init("NM Red Dragon God",
                new State(
                    new SetAltTexture(1),
                    new StayCloseToSpawn(0.5, 24),
                    new HpLessTransition(0.05, "23"),
                    new TransformOnDeath("lod Red Loot Balloon"),
                    new OnDeathBehavior(new EntityOrder(99, "NM Red Dragon Spawner", "despawn")),
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new State("1",
                            new PlayerWithinTransition(3, "2")
                            ),
                        new State("2",
                            new Flashing(0xFFFFFF, 0.2, 12),
                            new TimedTransition(2500, "pre_3")
                            ),
                        new State("pre_3",
                            new EntityOrder(999, "drac wall red", "spawn"),
                            new TimedTransition(0, "3")
                            ),
                        new State("3",
                            new SetAltTexture(0),
                            new TimedTransition(1000, "4")
                            ),
                        new State("4",
                            new EntityOrder(99, "NM Red Minion Spawner", "1"),
                            new EntityOrder(99, "NM Red Fake Egg", "1"),
                            new TossObject("NM Red Dragon Lava Bomb", 2, 0),
                            new TossObject("NM Red Dragon Lava Bomb", 2, 90),
                            new TossObject("NM Red Dragon Lava Bomb", 2, 180),
                            new TossObject("NM Red Dragon Lava Bomb", 2, 270),
                            new TimedTransition(0, "5_1")
                            ),
                        new State("5_1",
                            new EntityExistsTransition("NM Red Dragon Minion", 999, "5_2")
                            ),
                        new State("5_2",
                            new EntitiesNotExistsTransition(999, "8", "NM Red Dragon Minion"),
                            new Shoot(20, 6, 15, 4, coolDown: 1500),
                            new Shoot(99, 10, 36, 5, 0, 10, coolDown: 2000),
                            new State("6",
                                new Shoot(99, 4, 90, 0, 0, 0),
                                new TimedTransition(500, "7")
                                ),
                            new State("7",
                                new Shoot(99, 4, 90, 0, 0, 45),
                                new TimedTransition(500, "6")
                                )
                            ),
                        new State("8",
                            new EntityOrder(999, "NM Red Dragon Lava Trigger", "8"),
                            new TimedTransition(0, "9")
                            )
                        ),
                    new State("9",
                        new HpLessTransition(0.5, "12"),
                        new Chase(0.4, 20, 1, 6000, 6000),
                        new Shoot(20, 6, 15, 4, coolDown: 1500),
                        new Shoot(99, 10, 36, 5, 0, 10, coolDown: 2000),
                        new Shoot(99, 6, 10, 2, 0, 0, coolDown: 2500),
                        new Shoot(99, 6, 10, 2, 0, 90, coolDown: 2500),
                        new Shoot(99, 6, 10, 2, 0, 180, coolDown: 2500),
                        new Shoot(99, 6, 10, 2, 0, 270, coolDown: 2500),
                        new Shoot(20, 3, 25, 1, coolDown: 2000),
                        new Shoot(20, 1, index: 6, coolDown: 1000),
                        new State("9_2",
                            new Flashing(0x0026FF, 0.2, 12),
                            new TimedTransition(100, "10")
                            ),
                        new State("10",
                            new Shoot(99, 4, 90, 3, 0, 0, coolDownOffset: 200),
                            new Shoot(99, 4, 90, 3, 0, 10, coolDownOffset: 400),
                            new Shoot(99, 4, 90, 3, 0, 20, coolDownOffset: 600),
                            new Shoot(99, 4, 90, 3, 0, 30, coolDownOffset: 800),
                            new Shoot(99, 4, 90, 3, 0, 40, coolDownOffset: 1000),
                            new TimedTransition(1000, "11")
                            ),
                        new State("11",
                            new Shoot(99, 4, 90, 3, 0, 40, coolDownOffset: 200),
                            new Shoot(99, 4, 90, 3, 0, 30, coolDownOffset: 400),
                            new Shoot(99, 4, 90, 3, 0, 20, coolDownOffset: 600),
                            new Shoot(99, 4, 90, 3, 0, 10, coolDownOffset: 800),
                            new Shoot(99, 4, 90, 3, 0, 0, coolDownOffset: 1000),
                            new TimedTransition(1000, "10")
                            )
                        ),
                    new State("12",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new State("13",
                            new ReturnToSpawn(true, 0.7),
                            new TimedTransition(4000, "14")
                            ),
                        new State("14",
                            new EntityOrder(99, "NM Red Minion Spawner", "1"),
                            new EntityOrder(99, "NM Red Fake Egg", "1"),
                            new TossObject("NM Red Dragon Lava Bomb", 2, 0),
                            new TossObject("NM Red Dragon Lava Bomb", 2, 90),
                            new TossObject("NM Red Dragon Lava Bomb", 2, 180),
                            new TossObject("NM Red Dragon Lava Bomb", 2, 270),
                            new TimedTransition(0, "15")
                            ),
                        new State("15",
                            new EntityExistsTransition("NM Red Dragon Minion", 999, "16")
                            ),
                        new State("16",
                            new EntitiesNotExistsTransition(999, "18_2", "NM Red Dragon Minion"),
                            new Shoot(20, 6, 15, 4, coolDown: 1500),
                            new Shoot(99, 10, 36, 5, 0, 10, coolDown: 2000),
                            new State("17",
                                new Shoot(99, 4, 90, 0, 0, 0),
                                new TimedTransition(500, "18")
                                ),
                            new State("18",
                                new Shoot(99, 4, 90, 0, 0, 45),
                                new TimedTransition(500, "17")
                                ),
                            new State("18_2",
                                new EntityOrder(999, "NM Red Dragon Lava Trigger", "8"),
                                new TimedTransition(0, "19")
                                )
                            )
                        ),
                    new State("19",
                        new Chase(0.4, 20, 1, 6000, 6000),
                        new Shoot(20, 6, 15, 4, coolDown: 1500),
                        new Shoot(99, 10, 36, 5, 0, 10, coolDown: 2000),
                        new Shoot(99, 6, 10, 2, 0, 0, coolDown: 2500),
                        new Shoot(99, 6, 10, 2, 0, 90, coolDown: 2500),
                        new Shoot(99, 6, 10, 2, 0, 180, coolDown: 2500),
                        new Shoot(99, 6, 10, 2, 0, 270, coolDown: 2500),
                        new Shoot(20, 3, 25, 1, coolDown: 2000),
                        new Shoot(20, 1, index: 6, coolDown: 1000),
                        new State("20",
                            new Flashing(0x0026FF, 0.2, 12),
                            new TimedTransition(100, "21")
                            ),
                        new State("21",
                            new Shoot(99, 4, 90, 3, 0, 0, coolDownOffset: 200),
                            new Shoot(99, 4, 90, 3, 0, 10, coolDownOffset: 400),
                            new Shoot(99, 4, 90, 3, 0, 20, coolDownOffset: 600),
                            new Shoot(99, 4, 90, 3, 0, 30, coolDownOffset: 800),
                            new Shoot(99, 4, 90, 3, 0, 40, coolDownOffset: 1000),
                            new TimedTransition(1000, "22")
                            ),
                        new State("22",
                            new Shoot(99, 4, 90, 3, 0, 40, coolDownOffset: 200),
                            new Shoot(99, 4, 90, 3, 0, 30, coolDownOffset: 400),
                            new Shoot(99, 4, 90, 3, 0, 20, coolDownOffset: 600),
                            new Shoot(99, 4, 90, 3, 0, 10, coolDownOffset: 800),
                            new Shoot(99, 4, 90, 3, 0, 0, coolDownOffset: 1000),
                            new TimedTransition(1000, "21")
                            )
                        ),
                    new State("23",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new State("24",
                            new ReturnToSpawn(true, 0.7),
                            new Taunt(true, "I will fight you until my last breath..."),
                            new TimedTransition(3000, "25")
                            ),
                        new State("25",
                            new Suicide()
                            )
                        )
                    )
            )
            .Init("NM Red Dragon God Hardmode",
                new State(
                    new SetAltTexture(1),
                    new StayCloseToSpawn(0.5, 24),
                    new HpLessTransition(0.05, "23"),
                    new TransformOnDeath("lod Red HM Loot Balloon"),
                    new OnDeathBehavior(new EntityOrder(99, "NM Red Dragon Spawner", "despawn")),
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new State("1",
                            new PlayerWithinTransition(3, "2")
                            ),
                        new State("2",
                            new Flashing(0xFFFFFF, 0.2, 12),
                            new TimedTransition(2500, "pre_3")
                            ),
                        new State("pre_3",
                            new EntityOrder(999, "drac wall red", "spawn"),
                            new TimedTransition(0, "3")
                            ),
                        new State("3",
                            new SetAltTexture(0),
                            new TimedTransition(1000, "4")
                            ),
                        new State("4",
                            new EntityOrder(99, "NM Red Minion Spawner", "1"),
                            new EntityOrder(99, "NM Red Fake Egg", "1"),
                            new TossObject("NM Red Dragon Lava Bomb", 2, 0),
                            new TossObject("NM Red Dragon Lava Bomb", 2, 90),
                            new TossObject("NM Red Dragon Lava Bomb", 2, 180),
                            new TossObject("NM Red Dragon Lava Bomb", 2, 270),
                            new TimedTransition(0, "5_1")
                            ),
                        new State("5_1",
                            new EntityExistsTransition("NM Red Dragon Minion", 999, "5_2")
                            ),
                        new State("5_2",
                            new EntitiesNotExistsTransition(999, "8", "NM Red Dragon Minion"),
                            new Shoot(20, 6, 15, 4, coolDown: 1500),
                            new Shoot(99, 10, 36, 5, 0, 10, coolDown: 2000),
                            new State("6",
                                new Shoot(99, 4, 90, 0, 0, 0),
                                new TimedTransition(500, "7")
                                ),
                            new State("7",
                                new Shoot(99, 4, 90, 0, 0, 45),
                                new TimedTransition(500, "6")
                                )
                            ),
                        new State("8",
                            new EntityOrder(999, "NM Red Dragon Lava Trigger", "8"),
                            new TimedTransition(0, "9")
                            )
                        ),
                    new State("9",
                        new HpLessTransition(0.5, "12"),
                        new Chase(0.5, 20, 1, 6000, 6000),
                        new Shoot(20, 6, 15, 4, coolDown: 1500),
                        new Shoot(99, 10, 36, 5, 0, 10, coolDown: 2000),
                        new Shoot(99, 6, 10, 2, 0, 0, coolDown: 2500),
                        new Shoot(99, 6, 10, 2, 0, 90, coolDown: 2500),
                        new Shoot(99, 6, 10, 2, 0, 180, coolDown: 2500),
                        new Shoot(99, 6, 10, 2, 0, 270, coolDown: 2500),
                        new Shoot(20, 3, 25, 1, coolDown: 2000),
                        new State("9_2",
                            new Flashing(0x0026FF, 0.2, 12),
                            new TimedTransition(100, "10")
                            ),
                        new State("10",
                            new Shoot(99, 4, 90, 3, 0, 0, coolDownOffset: 200),
                            new Shoot(99, 4, 90, 3, 0, 10, coolDownOffset: 400),
                            new Shoot(99, 4, 90, 3, 0, 20, coolDownOffset: 600),
                            new Shoot(99, 4, 90, 3, 0, 30, coolDownOffset: 800),
                            new Shoot(99, 4, 90, 3, 0, 40, coolDownOffset: 1000),
                            new TimedTransition(1000, "11")
                            ),
                        new State("11",
                            new Shoot(99, 4, 90, 3, 0, 40, coolDownOffset: 200),
                            new Shoot(99, 4, 90, 3, 0, 30, coolDownOffset: 400),
                            new Shoot(99, 4, 90, 3, 0, 20, coolDownOffset: 600),
                            new Shoot(99, 4, 90, 3, 0, 10, coolDownOffset: 800),
                            new Shoot(99, 4, 90, 3, 0, 0, coolDownOffset: 1000),
                            new TimedTransition(1000, "10")
                            )
                        ),
                    new State("12",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new State("13",
                            new ReturnToSpawn(true, 0.7),
                            new TimedTransition(4000, "14")
                            ),
                        new State("14",
                            new EntityOrder(99, "NM Red Minion Spawner", "1"),
                            new EntityOrder(99, "NM Red Fake Egg", "1"),
                            new TossObject("NM Red Dragon Lava Bomb", 2, 0),
                            new TossObject("NM Red Dragon Lava Bomb", 2, 90),
                            new TossObject("NM Red Dragon Lava Bomb", 2, 180),
                            new TossObject("NM Red Dragon Lava Bomb", 2, 270),
                            new TimedTransition(0, "15")
                            ),
                        new State("15",
                            new EntityExistsTransition("NM Red Dragon Minion", 999, "16")
                            ),
                        new State("16",
                            new EntitiesNotExistsTransition(999, "18_2", "NM Red Dragon Minion"),
                            new Shoot(20, 6, 15, 4, coolDown: 1500),
                            new Shoot(99, 10, 36, 5, 0, 10, coolDown: 2000),
                            new State("17",
                                new Shoot(99, 4, 90, 0, 0, 0),
                                new TimedTransition(500, "18")
                                ),
                            new State("18",
                                new Shoot(99, 4, 90, 0, 0, 45),
                                new TimedTransition(500, "17")
                                ),
                            new State("18_2",
                                new EntityOrder(999, "NM Red Dragon Lava Trigger", "8"),
                                new TimedTransition(0, "19")
                                )
                            )
                        ),
                    new State("19",
                        new Chase(0.5, 20, 1, 6000, 6000),
                        new Shoot(20, 6, 15, 4, coolDown: 1500),
                        new Shoot(99, 10, 36, 5, 0, 10, coolDown: 2000),
                        new Shoot(99, 6, 10, 2, 0, 0, coolDown: 2500),
                        new Shoot(99, 6, 10, 2, 0, 90, coolDown: 2500),
                        new Shoot(99, 6, 10, 2, 0, 180, coolDown: 2500),
                        new Shoot(99, 6, 10, 2, 0, 270, coolDown: 2500),
                        new Shoot(20, 3, 25, 1, coolDown: 2000),
                        new State("20",
                            new Flashing(0x0026FF, 0.2, 12),
                            new TimedTransition(100, "21")
                            ),
                        new State("21",
                            new Shoot(99, 4, 90, 3, 0, 0, coolDownOffset: 200),
                            new Shoot(99, 4, 90, 3, 0, 10, coolDownOffset: 400),
                            new Shoot(99, 4, 90, 3, 0, 20, coolDownOffset: 600),
                            new Shoot(99, 4, 90, 3, 0, 30, coolDownOffset: 800),
                            new Shoot(99, 4, 90, 3, 0, 40, coolDownOffset: 1000),
                            new TimedTransition(1000, "22")
                            ),
                        new State("22",
                            new Shoot(99, 4, 90, 3, 0, 40, coolDownOffset: 200),
                            new Shoot(99, 4, 90, 3, 0, 30, coolDownOffset: 400),
                            new Shoot(99, 4, 90, 3, 0, 20, coolDownOffset: 600),
                            new Shoot(99, 4, 90, 3, 0, 10, coolDownOffset: 800),
                            new Shoot(99, 4, 90, 3, 0, 0, coolDownOffset: 1000),
                            new TimedTransition(1000, "21")
                            )
                        ),
                    new State("23",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new State("24",
                            new ReturnToSpawn(true, 0.7),
                            new Taunt(true, "I will fight you until my last breath..."),
                            new TimedTransition(3000, "25")
                            ),
                        new State("25",
                            new Suicide()
                            )
                        )
                    )
            )
        .Init("NM Red Fake Egg",
            new State(
                new AddCond(ConditionEffectIndex.Invincible),
                new State("nothin"),
                new State("1",
                    new Flashing(0xFFFFFF, 0.2, 12)
                    )
                )
            )
        .Init("NM Red Minion Spawner",
            new State(
                new AddCond(ConditionEffectIndex.Invincible),
                new State("nothin"),
                new State("1",
                    new Flashing(0xFFFFFF, 0.2, 12),
                    new TimedTransition(2500, "2")
                    ),
                new State("2",
                    new Spawn("NM Red Dragon Minion", 1, 0),
                    new TimedTransition(100, "nothin")
                    )
                )
            )
        .Init("NM Red Open Wall",
            new State(
                new AddCond(ConditionEffectIndex.Invincible),
                new State("nothin"),
                new State("1",
                    new ApplySetpiece("RedOpenWall")
                    )
                )
            )
        .Init("NM Red Dragon Minion",
            new State(
                new Circle(0.3, 4, 8, null),
                new Shoot(8, 1, index: 0, coolDown: 1500)
                )
            )
        .Init("NM Red Dragon Lava Bomb",
            new State(
                new AddCond(ConditionEffectIndex.Invincible),
                new State("1",
                    new TimedTransition(1000, "2")
                    ),
                new State("2",
                    new EntityOrder(999, "NM Red Dragon Lava Trigger", "3"),
                    new Aoe(1, false, 0, 0, false, 0xFF0000),
                    new Decay(0)
                    )
                )
            )
        .Init("NM Red Dragon Lava Trigger",
            new State(
                new AddCond(ConditionEffectIndex.Invincible),
                new State("1"),
                new State("3",
                    new ApplySetpiece("RedDragonLavaTrigger1"),
                    new TimedTransition(2000, "4")
                    ),
                new State("4",
                    new ApplySetpiece("RedDragonLavaTrigger2"),
                    new TimedTransition(2000, "5")
                    ),
                new State("5",
                    new ApplySetpiece("RedDragonLavaTrigger3"),
                    new TimedTransition(2000, "6")
                    ),
                new State("6",
                    new ApplySetpiece("RedDragonLavaTrigger4"),
                    new TimedTransition(2000, "7")
                    ),
                new State("7",
                    new ApplySetpiece("RedDragonLavaTrigger5")
                    ),
                new State("8",
                    new ApplySetpiece("RedDragonLavaTrigger6"),
                    new TimedTransition(100, "1")
                    )
                )
            )
        .Init("drac wall red",
            new State(
                new AddCond(ConditionEffectIndex.Invincible),
                new State("nothin"),
                new State("spawn",
                    new ApplySetpiece("RedCloseWall1"),
                    new TimedTransition(3000, "spawn2")
                    ),
                new State("spawn2",
                    new ApplySetpiece("RedCloseWall2"),
                    new TimedTransition(3000, "spawn3")
                    ),
                new State("spawn3",
                    new ApplySetpiece("RedCloseWall3"),
                    new TimedTransition(3000, "spawn4")
                    ),
                new State("spawn4",
                    new ApplySetpiece("RedCloseWall4"),
                    new TimedTransition(3000, "spawn5")
                    ),
                new State("spawn5",
                    new ApplySetpiece("RedCloseWall5")
                    )
                )
            )
        .Init("Iron Wall",
            new State(
                new State("2",
                    new EntityExistsTransition("lod Red Loot Balloon", 999, "3_1"),
                    new EntityExistsTransition("lod Red HM Loot Balloon", 999, "3_2")
                    ),
                new State("3_1",
                    new EntityNotExistsTransition("lod Red Loot Balloon", 999, "despawn")
                    ),
                new State("3_2",
                    new EntityNotExistsTransition("lod Red HM Loot Balloon", 999, "despawn")
                    ),
                new State("despawn",
                    new Decay(0)
                    )
                )
            )
            .Init("lod Red HM Loot Balloon",
                new State(
                    new TransformOnDeath("NM Red Dragon Dead"),
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "UnsetEffect")
                    ),
                    new State("UnsetEffect")
                ),
                new MostDamagers(3,
                    new TierLoot(2, ItemType.Potion, 1)
                    ),
                new Threshold(0.01,
                    new TierLoot(12, ItemType.Weapon, 0.05),
                    new TierLoot(11, ItemType.Weapon, 0.06),
                    new TierLoot(10, ItemType.Weapon, 0.07),
                    new TierLoot(13, ItemType.Armor, 0.05),
                    new TierLoot(12, ItemType.Armor, 0.06),
                    new TierLoot(11, ItemType.Armor, 0.07),
                    new ItemLoot("Large Red Dragon Scale Cloth", 0.1),
                    new ItemLoot("Small Red Dragon Scale Cloth", 0.1),
                    new ItemLoot("Wine Cellar Incantation", 0.01),
                    new ItemLoot("Fire Dragon Battle Armor", 0.01)
                )
            )
            .Init("lod Red Loot Balloon",
                new State(
                    new TransformOnDeath("NM Red Dragon Dead"),
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "UnsetEffect")
                    ),
                    new State("UnsetEffect")
                ),
                new MostDamagers(2,
                    new TierLoot(2, ItemType.Potion, 1)
                    ),
                new Threshold(0.01,
                    new TierLoot(12, ItemType.Weapon, 0.03),
                    new TierLoot(11, ItemType.Weapon, 0.04),
                    new TierLoot(10, ItemType.Weapon, 0.05),
                    new TierLoot(13, ItemType.Armor, 0.03),
                    new TierLoot(12, ItemType.Armor, 0.04),
                    new TierLoot(11, ItemType.Armor, 0.05),
                    new ItemLoot("Large Red Dragon Scale Cloth", 0.08),
                    new ItemLoot("Small Red Dragon Scale Cloth", 0.08),
                    new ItemLoot("Wine Cellar Incantation", 0.008),
                    new ItemLoot("Fire Dragon Battle Armor", 0.008)
                )
            )
        //1
            .Init("NM Red Dragon Dead",
                new State(
                    new AddCond(ConditionEffectIndex.Invincible)
                    )
                )
            .Init("NM Green Dragon Dead",
                new State(
                    new AddCond(ConditionEffectIndex.Invincible)
                    )
                )
            .Init("NM Blue Dragon Dead",
                new State(
                    new AddCond(ConditionEffectIndex.Invincible)
                    )
                )
            .Init("NM Black Dragon Dead",
                new State(
                    new AddCond(ConditionEffectIndex.Invincible)
                    )
                )
            .Init("NM Green Dragon God",
                new State(
                    new SetAltTexture(1),
                    new StayCloseToSpawn(0.5, 24),
                    new HpLessTransition(0.05, "19"),
                    new TransformOnDeath("lod Green Loot Balloon"),
                    new OnDeathBehavior(new EntityOrder(99, "NM Green Dragon Spawner", "despawn")),
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new State("1",
                            new PlayerWithinTransition(3, "pre_2")
                            ),
                        new State("pre_2",
                            new EntityOrder(999, "drac wall green", "spawn"),
                            new TimedTransition(500, "2")
                            ),
                        new State("2",
                            new Flashing(0xFFFFFF, 0.2, 12),
                            new TimedTransition(2500, "3")
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
                            )
                        )
                    )
            )
            .Init("NM Green Open Wall",
            new State(
                new AddCond(ConditionEffectIndex.Invincible),
                new State("nothin"),
                new State("1",
                    new ApplySetpiece("GreenOpenWall")
                    )
                )
            )
            .Init("NM Green Dragon God Hardmode",
                new State(
                    new SetAltTexture(1),
                    new StayCloseToSpawn(0.5, 24),
                    new HpLessTransition(0.05, "19"),
                    new TransformOnDeath("lod Green HM Loot Balloon"),
                    new OnDeathBehavior(new EntityOrder(99, "NM Green Dragon Spawner", "despawn")),
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new State("1",
                            new PlayerWithinTransition(3, "pre_2")
                            ),
                        new State("pre_2",
                            new EntityOrder(999, "drac wall green", "spawn"),
                            new TimedTransition(500, "2")
                            ),
                        new State("2",
                            new Flashing(0xFFFFFF, 0.2, 12),
                            new TimedTransition(2500, "3")
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
                            )
                        )
                    )
            )
        .Init("NM Green Dragon Spawner",
            new State(
                new AddCond(ConditionEffectIndex.Invincible),
                new State("nothin"),
                new State("normal",
                    new Spawn("NM Green Dragon God", 1, 0)
                    ),
                new State("hardmode",
                    new Spawn("NM Green Dragon God Hardmode", 1, 0)
                    ),
                new State("despawn",
                    new Decay(0)
                    )
                )
            )
        .Init("NM Green Dragon Shield",
            new State(
                new Circle(0.4, 3, 10, "NM Green Dragon God"),
                new Circle(0.4, 3, 10, "NM Green Dragon God Hardmode"),
                new State("1",
                    new Shoot(20, 1, index: 0, coolDown: 5000),
                    new TimedTransition(10000, "2")
                    ),
                new State("2",
                    new Shoot(20, 1, index: 1, coolDown: 5000),
                    new TimedTransition(10000, "1")
                    )
                )
            )
        .Init("NM Green Dragon Minion",
            new State(
                new Protect(0.5, "NM Green Dragon God", 10, 7),
                new Protect(0.5, "NM Green Dragon God Hardmode", 10, 7),
                new Wander(0.3),
                new Shoot(10, 1, index: 3, coolDown: 1000),
                new State("1",
                    new EntitiesNotExistsTransition(999, "2", "NM Green Dragon God", "NM Green Dragon God Hardmode")
                    ),
                new State("2",
                    new Suicide()
                    )
                )
            )
            .Init("lod Green Loot Balloon",
                new State(
                    new TransformOnDeath("NM Green Dragon Dead"),
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "UnsetEffect")
                    ),
                    new State("UnsetEffect")
                ),
                new MostDamagers(2,
                    new TierLoot(2, ItemType.Potion, 1)
                    ),
                new Threshold(0.01,
                    new TierLoot(12, ItemType.Weapon, 0.03),
                    new TierLoot(11, ItemType.Weapon, 0.04),
                    new TierLoot(10, ItemType.Weapon, 0.05),
                    new TierLoot(13, ItemType.Armor, 0.03),
                    new TierLoot(12, ItemType.Armor, 0.04),
                    new TierLoot(11, ItemType.Armor, 0.05),
                    new ItemLoot("Large Green Dragon Scale Cloth", 0.08),
                    new ItemLoot("Small Green Dragon Scale Cloth", 0.08),
                    new ItemLoot("Wine Cellar Incantation", 0.008),
                    new ItemLoot("Leaf Dragon Hide Armor", 0.008)
                )
            )
            .Init("lod Green HM Loot Balloon",
                new State(
                    new TransformOnDeath("NM Green Dragon Dead"),
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "UnsetEffect")
                    ),
                    new State("UnsetEffect")
                ),
                new MostDamagers(3,
                    new TierLoot(2, ItemType.Potion, 1)
                    ),
                new Threshold(0.01,
                    new TierLoot(12, ItemType.Weapon, 0.05),
                    new TierLoot(11, ItemType.Weapon, 0.06),
                    new TierLoot(10, ItemType.Weapon, 0.07),
                    new TierLoot(13, ItemType.Armor, 0.05),
                    new TierLoot(12, ItemType.Armor, 0.06),
                    new TierLoot(11, ItemType.Armor, 0.07),
                    new ItemLoot("Large Green Dragon Scale Cloth", 0.1),
                    new ItemLoot("Small Green Dragon Scale Cloth", 0.1),
                    new ItemLoot("Wine Cellar Incantation", 0.01),
                    new ItemLoot("Leaf Dragon Hide Armor", 0.01)
                )
            )
        .Init("drac wall green",
            new State(
                new AddCond(ConditionEffectIndex.Invincible),
                new State("nothin"),
                new State("spawn",
                    new ApplySetpiece("GreenCloseWall1"),
                    new TimedTransition(3000, "spawn2")
                    ),
                new State("spawn2",
                    new ApplySetpiece("GreenCloseWall2"),
                    new TimedTransition(3000, "spawn3")
                    ),
                new State("spawn3",
                    new ApplySetpiece("GreenCloseWall3"),
                    new TimedTransition(3000, "spawn4")
                    ),
                new State("spawn4",
                    new ApplySetpiece("GreenCloseWall4"),
                    new TimedTransition(3000, "spawn5")
                    ),
                new State("spawn5",
                    new ApplySetpiece("GreenCloseWall5")
                    )
                )
            )
        .Init("drac floor green",
            new State(
                new AddCond(ConditionEffectIndex.Invincible)
                )
            )
        .Init("Big Trees",
            new State(
                new State("2",
                    new EntityExistsTransition("lod Green Loot Balloon", 999, "3_1"),
                    new EntityExistsTransition("lod Green HM Loot Balloon", 999, "3_2")
                    ),
                new State("3_1",
                    new EntityNotExistsTransition("lod Green Loot Balloon", 999, "despawn")
                    ),
                new State("3_2",
                    new EntityNotExistsTransition("lod Green HM Loot Balloon", 999, "despawn")
                    ),
                new State("despawn",
                    new Decay(0)
                    )
                )
            )
        .Init("Stalagmite",
            new State(
                new State("2",
                    new EntityExistsTransition("lod Black Loot Balloon", 999, "3_1"),
                    new EntityExistsTransition("lod Black HM Loot Balloon", 999, "3_2")
                    ),
                new State("3_1",
                    new EntityNotExistsTransition("lod Black Loot Balloon", 999, "despawn")
                    ),
                new State("3_2",
                    new EntityNotExistsTransition("lod Black HM Loot Balloon", 999, "despawn")
                    ),
                new State("despawn",
                    new Decay(0)
                    )
                )
            )
            .Init("NM Black Open Wall",
                new State(
                    new AddCond(ConditionEffectIndex.Invincible),
                    new State("nothin"),
                    new State("1",
                    new ApplySetpiece("BlackOpenWall")
                        )
                    )
                )
            .Init("drac wall black",
            new State(
                new AddCond(ConditionEffectIndex.Invincible),
                new State("nothin"),
                new State("spawn",
                    new ApplySetpiece("BlackCloseWall1"),
                    new TimedTransition(3000, "spawn2")
                    ),
                new State("spawn2",
                    new ApplySetpiece("BlackCloseWall2"),
                    new TimedTransition(3000, "spawn3")
                    ),
                new State("spawn3",
                    new ApplySetpiece("BlackCloseWall3"),
                    new TimedTransition(3000, "spawn4")
                    ),
                new State("spawn4",
                    new ApplySetpiece("BlackCloseWall4"),
                    new TimedTransition(3000, "spawn5")
                    ),
                new State("spawn5",
                    new ApplySetpiece("BlackCloseWall5")
                    )
                )
            )
            .Init("NM Black Dragon God",
                new State(
                    new SetAltTexture(1),
                    new StayCloseToSpawn(0.5, 24),
                    new HpLessTransition(0.05, "15"),
                    new TransformOnDeath("lod Black Loot Balloon"),
                    new OnDeathBehavior(new EntityOrder(99, "NM Black Dragon Spawner", "despawn")),
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new State("1",
                            new PlayerWithinTransition(3, "pre_2")
                            ),
                        new State("pre_2",
                            new EntityOrder(999, "drac wall black", "spawn"),
                            new TimedTransition(500, "3")
                            ),
                        new State("2",
                            new Flashing(0xFFFFFF, 0.2, 12),
                            new TimedTransition(2500, "3")
                            ),
                        new State("3",
                            new SetAltTexture(0),
                            new Taunt("Prepare to meet your doom. There is no mercy here."),
                            new TimedTransition(1000, "4")
                            )
                        ),
                    new State("4",
                        new Chase(0.4, 10, 3),
                        new StayCloseToSpawn(0.7, 10),
                        new Shoot(12, 5, 36, 0, coolDown: 1000),
                        new Shoot(12, 1, index: 1, coolDown: 500),
                        new Shoot(12, 5, 36, 2, aim: 1, coolDown: 1000),
                        new Shoot(22, 2, 45, 3, coolDown: 1000),
                        new Shoot(12, 5, 36, 5, coolDown: 1500),
                        new Shoot(999, 5, 25, 7, 0, 270, coolDown: 1000),
                        new Grenade(4, 100, 14, coolDown: 7000),
                        new Spawn("NM Black Dragon Shadow", 2, 0),
                        new Reproduce("NM Black Dragon Shadow", 99, 2, 0, 10000),
                        new State("5",
                            new NoPlayerWithinTransition(10, "6"),
                            new State("7",
                                new Shoot(999, 6, 25, 6, 0, 0),
                                new TimedTransition(1000, "8")
                                ),
                            new State("8",
                                new Shoot(999, 6, 25, 6, 0, 90),
                                new TimedTransition(1000, "9")
                                ),
                            new State("9",
                                new Shoot(999, 6, 25, 6, 0, 180),
                                new TimedTransition(1000, "10")
                                ),
                            new State("10",
                                new Shoot(999, 6, 25, 6, 0, 270),
                                new TimedTransition(1000, "7")
                                )
                            ),
                        new State("6",
                            new ReturnToSpawn(true, 0.7),
                            new PlayerWithinTransition(10, "5"),
                            new State("11",
                                new Shoot(999, 6, 25, 6, 0, 0),
                                new TimedTransition(1000, "12")
                                ),
                            new State("12",
                                new Shoot(999, 6, 25, 6, 0, 90),
                                new TimedTransition(1000, "13")
                                ),
                            new State("13",
                                new Shoot(999, 6, 25, 6, 0, 180),
                                new TimedTransition(1000, "14")
                                ),
                            new State("14",
                                new Shoot(999, 6, 25, 6, 0, 270),
                                new TimedTransition(1000, "11")
                                )
                            )
                        ),
                    new State("15",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new State("16",
                            new ReturnToSpawn(true, 0.7),
                            new Taunt(true, "Until we meet again...sub-creature..."),
                            new TimedTransition(3000, "17")
                            ),
                        new State("17",
                            new Suicide()
                            )
                        )
                    )
            )
            .Init("NM Black Dragon God Hardmode",
                new State(
                    new SetAltTexture(1),
                    new StayCloseToSpawn(0.5, 24),
                    new HpLessTransition(0.05, "15"),
                    new TransformOnDeath("lod Black HM Loot Balloon"),
                    new OnDeathBehavior(new EntityOrder(99, "NM Black Dragon Spawner", "despawn")),
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new State("1",
                            new PlayerWithinTransition(3, "pre_2")
                            ),
                        new State("pre_2",
                            new EntityOrder(999, "drac wall black", "spawn"),
                            new TimedTransition(500, "3")
                            ),
                        new State("2",
                            new Flashing(0xFFFFFF, 0.2, 12),
                            new TimedTransition(2500, "3")
                            ),
                        new State("3",
                            new SetAltTexture(0),
                            new Taunt("Prepare to meet your doom. There is no mercy here."),
                            new TimedTransition(1000, "4")
                            )
                        ),
                    new State("4",
                        new Chase(0.4, 10, 3),
                        new StayCloseToSpawn(0.7, 10),
                        new Shoot(12, 5, 36, 0, coolDown: 1000),
                        new Shoot(12, 1, index: 1, coolDown: 500),
                        new Shoot(12, 5, 36, 2, aim: 1, coolDown: 1000),
                        new Shoot(22, 2, 45, 3, coolDown: 1000),
                        new Shoot(12, 5, 36, 5, coolDown: 1500),
                        new Shoot(999, 5, 25, 7, 0, 270, coolDown: 1000),
                        new Grenade(4, 100, 14, coolDown: 7000),
                        new Spawn("NM Black Dragon Shadow", 2, 0),
                        new Reproduce("NM Black Dragon Shadow", 99, 2, 0, 10000),
                        new State("5",
                            new NoPlayerWithinTransition(10, "6"),
                            new State("7",
                                new Shoot(999, 6, 25, 6, 0, 0),
                                new TimedTransition(1000, "8")
                                ),
                            new State("8",
                                new Shoot(999, 6, 25, 6, 0, 90),
                                new TimedTransition(1000, "9")
                                ),
                            new State("9",
                                new Shoot(999, 6, 25, 6, 0, 180),
                                new TimedTransition(1000, "10")
                                ),
                            new State("10",
                                new Shoot(999, 6, 25, 6, 0, 270),
                                new TimedTransition(1000, "7")
                                )
                            ),
                        new State("6",
                            new ReturnToSpawn(true, 0.7),
                            new PlayerWithinTransition(10, "5"),
                            new State("11",
                                new Shoot(999, 6, 25, 6, 0, 0),
                                new TimedTransition(1000, "12")
                                ),
                            new State("12",
                                new Shoot(999, 6, 25, 6, 0, 90),
                                new TimedTransition(1000, "13")
                                ),
                            new State("13",
                                new Shoot(999, 6, 25, 6, 0, 180),
                                new TimedTransition(1000, "14")
                                ),
                            new State("14",
                                new Shoot(999, 6, 25, 6, 0, 270),
                                new TimedTransition(1000, "11")
                                )
                            )
                        ),
                    new State("15",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new State("16",
                            new ReturnToSpawn(true, 0.7),
                            new Taunt(true, "Until we meet again...sub-creature..."),
                            new TimedTransition(3000, "17")
                            ),
                        new State("17",
                            new Suicide()
                            )
                        )
                    )
            )
        .Init("NM Black Dragon Shadow",
            new State(
                new Protect(0.3, "NM Black Dragon God", 30, 10),
                new Protect(0.3, "NM Black Dragon God Hardmode", 30, 10),
                new Wander(0.2),
                new Shoot(22, 2, 30, 0, coolDown: 1000),
                new State("1",
                    new EntityExistsTransition("NM Black Dragon God", 999, "2"),
                    new EntityExistsTransition("NM Black Dragon God Hardmode", 999, "3")
                    ),
                new State("2",
                    new EntityNotExistsTransition("NM Black Dragon God", 999, "4")
                    ),
                new State("3",
                    new EntityNotExistsTransition("NM Black Dragon God Hardmode", 999, "4")
                    ),
                new State("4",
                    new Suicide()
                    )
                )
            )
        .Init("NM Black Dragon Minion",
            new State(
                new State("1",
                    new AddCond(ConditionEffectIndex.Invincible)
                    ),
                new State("2",
                    new SetAltTexture(0),
                    new AddCond(ConditionEffectIndex.Invincible),
                    new PlayerWithinTransition(1, "3")
                    ),
                new State("3",
                    new SetAltTexture(1),
                    new Wander(0.3),
                    new Shoot(10, 1, index: 0, coolDown: 500),
                    new NoPlayerWithinTransition(1, "2")
                    )
                )
            )
        .Init("drac floor black",
            new State(
                new AddCond(ConditionEffectIndex.Invincible)
                )
            )
        .Init("NM Black Dragon Spawner",
            new State(
                new AddCond(ConditionEffectIndex.Invincible),
                new State("nothin"),
                new State("normal",
                    new Spawn("NM Black Dragon God", 1, 0)
                    ),
                new State("hardmode",
                    new Spawn("NM Black Dragon God Hardmode", 1, 0)
                    ),
                new State("despawn",
                    new Decay(0)
                    )
                )
            )
            .Init("lod Black HM Loot Balloon",
                new State(
                    new TransformOnDeath("NM Black Dragon Dead"),
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "UnsetEffect")
                    ),
                    new State("UnsetEffect")
                ),
                new MostDamagers(3,
                    new TierLoot(2, ItemType.Potion, 1)
                    ),
                new Threshold(0.01,
                    new TierLoot(12, ItemType.Weapon, 0.05),
                    new TierLoot(11, ItemType.Weapon, 0.06),
                    new TierLoot(10, ItemType.Weapon, 0.07),
                    new TierLoot(13, ItemType.Armor, 0.05),
                    new TierLoot(12, ItemType.Armor, 0.06),
                    new TierLoot(11, ItemType.Armor, 0.07),
                    new ItemLoot("Large Midnight Dragon Scale Cloth", 0.1),
                    new ItemLoot("Small Midnight Dragon Scale Cloth", 0.1),
                    new ItemLoot("Wine Cellar Incantation", 0.01),
                    new ItemLoot("The World Tarot Card", 0.04),
                    new ItemLoot("Annoying Firecracker Katana", 0.01)
                )
            )
            .Init("lod Black Loot Balloon",
                new State(
                    new TransformOnDeath("NM Black Dragon Dead"),
                    new State("Idle",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "UnsetEffect")
                    ),
                    new State("UnsetEffect")
                ),
                new MostDamagers(2,
                    new TierLoot(2, ItemType.Potion, 1)
                    ),
                new Threshold(0.01,
                    new TierLoot(12, ItemType.Weapon, 0.03),
                    new TierLoot(11, ItemType.Weapon, 0.04),
                    new TierLoot(10, ItemType.Weapon, 0.05),
                    new TierLoot(13, ItemType.Armor, 0.03),
                    new TierLoot(12, ItemType.Armor, 0.04),
                    new TierLoot(11, ItemType.Armor, 0.05),
                    new ItemLoot("Large Midnight Dragon Scale Cloth", 0.08),
                    new ItemLoot("Small Midnight Dragon Scale Cloth", 0.08),
                    new ItemLoot("Wine Cellar Incantation", 0.008),
                    new ItemLoot("The World Tarot Card", 0.02),
                    new ItemLoot("Annoying Firecracker Katana", 0.008)
                )
            )

        ;
    }
}