using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.loot;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ Puppet = () => Behav()
            .Init("Puppet Knight",
                new State(
                    new Prioritize(
                        new Chase(0.58, 8, 1),
                        new Wander(2)
                        ),
                    new Shoot(8.4, shoots: 1, index: 0, coolDown: 1750),
                    new Shoot(8, shoots: 1, index: 1, coolDown: 2000)
                    ),
                new ItemLoot("Health Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Obsidian Dagger", 0.02),
                    new ItemLoot("Steel Helm", 0.02)
                    )
            )
        .Init("Archer Puppet",
                new State(
                    new Wander(4.2),
                    new Shoot(8.4, shoots: 3, index: 0, coolDown: 1550),
                    new Shoot(8, shoots: 1, index: 1, coolDown: 2700)
                    ),
                new ItemLoot("Health Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Obsidian Dagger", 0.02),
                    new ItemLoot("Steel Helm", 0.02)
                    )
            )
         .Init("Paladin Puppet",
                new State(
                    new Prioritize(
                        new Chase(0.35, 8, 1),
                        new Wander(2)
                        ),
                    new Heal(8, amount: 100, coolDown: 5000),
                    new Shoot(8.4, shoots: 1, index: 0, coolDown: 1000)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Mithril Dagger", 0.02),
                    new ItemLoot("Mithril Chainmail", 0.02)
                    )
            )
        .Init("Assassin Puppet",
                new State(
                    new Prioritize(
                        new Chase(0.35, 8, 1),
                        new Wander(2)
                        ),
                    new Grenade(3, 100, 4, coolDown: 3000),
                    new Shoot(8.4, shoots: 1, index: 0, coolDown: 1500)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Mithril Dagger", 0.02),
                    new ItemLoot("Mithril Chainmail", 0.02)
                    )
            )
                 .Init("Warrior Puppet",
            new State(
                new State("jugg",
                    new AddCond(ConditionEffectIndex.Armored),
                    new Wander(6),
                    new TimedTransition(4700, "berserk")
                    ),
                new State("berserk",
                    new Shoot(8.4, shoots: 1, index: 0, coolDown: 1),
                    new TimedTransition(4700, "jugg")
                    )
                )
            )
         .Init("Rogue Puppet",
            new State(
                new State("jugg",
                    new SetAltTexture(1),
                    new Wander(6),
                    new TimedTransition(3000, "berserk")
                    ),
                new State("berserk",
                     new Wander(0.4),
                     new SetAltTexture(0),
                    new Shoot(8.4, shoots: 1, index: 0, coolDown: 1500),
                    new TimedTransition(3000, "jugg")
                    )
                )
            )
        .Init("Trickster Puppet",
            new State(
                new State("wait",
                    new Wander(0.3),
                    new PlayerWithinTransition(6, "start")
                    ),
                new State("start",
                    new Reproduce("Trickster Decoys", max: 1, range: 1, radius: 1, coolDown: 5250),
                    new Prioritize(
                    new Wander(6),
                    new Retreat(0.3, 3)
                        ),
                    new Shoot(8.4, shoots: 1, index: 0, coolDown: 1300)
                    )

                )
            )
        .Init("Trickster Decoys",
                new State(
                    new Wander(3.1),
                    new Shoot(8.4, shoots: 1, index: 0, coolDown: 1400)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Mithril Dagger", 0.02),
                    new ItemLoot("Mithril Chainmail", 0.02)
                    )
            )
         .Init("Mystic Puppet",
                new State(
                    new Wander(0.27),
                    new Shoot(8.4, shoots: 1, index: 0, coolDown: 1350),
                    new Shoot(8.4, shoots: 1, index: 1, coolDown: 1950)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Mithril Dagger", 0.02),
                    new ItemLoot("Mithril Chainmail", 0.02)
                    )
            )
        .Init("Puppet Wizard",
                new State(
                    new Prioritize(
                           new Circle(0.37, 4, 20, "The Puppet Master"),
                           new Wander(4)
                        ),
                    new Shoot(8.4, shoots: 10, index: 0, coolDown: 2650)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Golden Bow", 0.02),
                    new ItemLoot("Demon Edge", 0.02)
                    )
            )

        .Init("Puppet Priest",
                new State(
                    new Circle(0.37, 4, 20, "The Puppet Master"),
                    new SpecificHeal(8, 75, "Master", coolDown: 4500)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Golden Bow", 0.02),
                    new ItemLoot("Demon Edge", 0.02)
                    )
            )

        .Init("Healer Puppet",
                new State(
                    new Wander(0.4),
                    new Shoot(8.4, shoots: 1, index: 0, coolDown: 1250),
                    new Heal(10, 75, "Healers", coolDown: 3000)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new Threshold(0.1,
                    new ItemLoot("Golden Bow", 0.02),
                    new ItemLoot("Demon Edge", 0.02)
                    )
            )

        .Init("Sorcerer Puppet",
                new State(
                     new Prioritize(
                           new Chase(0.2, 8, 1),
                           new Wander(6)
                        ),
                     new TossObject("Puppet Bomb", 5, coolDown: 7500),
                    new Shoot(8.4, shoots: 1, index: 0, coolDown: 2000)
                    ),
                new ItemLoot("Health Potion", 0.1)
            )
              .Init("Puppet Bomb",
                        new State(
                            new TransformOnDeath("Sorc Bomb Thrower", 1, 1, 1),
                    new State("BOUTTOEXPLODE",
                    new TimedTransition(2750, "boom")
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
         .Init("Puppet Bomb 2",
                        new State(
                    new State("BOUTTOEXPLODE",
                    new TimedTransition(2750, "boom")
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
        .Init("Sorc Bomb Thrower",
                new State(
                     new AddCond(ConditionEffectIndex.Invincible),
                    new State("throw",
                        new TossObject("Puppet Bomb 2", 5, angle: 135, coolDown: 1500),
                        new TossObject("Puppet Bomb 2", 5, angle: 60, coolDown: 1500),
                    new TimedTransition(1000, "die")
                      ),
                      new State("die",
                       new Suicide()
                      )
            )
    )
        .Init("Oryx Puppet",
            new State(
                new State("default",
                    new PlayerWithinTransition(8, "idle")
                    ),
                new State("idle",
                    new AddCond(ConditionEffectIndex.Invulnerable), //  ok
                    new Taunt("Am I not an uncanny likeness of Oryx Himself?"),
                    new TimedTransition(4500, "Ready Phase")
                    ),
                new State("Ready Phase",
                    new Taunt("You don't see the similarity? Well then, let me show you!"),
                    new TimedTransition(4500, "Fight1")
                    ),
                new State("Fight1",
                    new RemCond(ConditionEffectIndex.Invulnerable), // ok
                    new Shoot(25, index: 0, shoots: 8, shootAngle: 10, coolDown: 2000, coolDownOffset: 1500),
                    new HpLessTransition(.50, "Fight2")
                    ),
                new State("Fight2",
                    new Spawn("Minion Puppet", initialSpawn: 1, maxChildren: 3, coolDown: 2500),
                    new AddCond(ConditionEffectIndex.Invulnerable), // ok
                    new TimedTransition(4500, "Move Phase")
                    ),
                new State("Move Phase",
                    new RemCond(ConditionEffectIndex.Invulnerable), // ok
                    new Grenade(4, 150, 8, coolDown: 500),
                    new Shoot(8.4, shoots: 1, index: 1, coolDown: 4500),
                    new Shoot(8.4, shoots: 1, index: 1, coolDown: 6500),
                    new Wander(0.3),
                    new HpLessTransition(.25, "Artifacts")
                    ),
                new State("Artifacts",
                    new AddCond(ConditionEffectIndex.Invulnerable), // ok
                    new Taunt(1.00, "I may not have all the power of Oryx, but my artifacts should still be enough to kill the likes of you!"),
                    new TossObject("Mini Guardian Element", 1, 0, 90000001, 1000),
                    new TossObject("Mini Guardian Element", 1, 90, 90000001, 1000),
                    new TossObject("Mini Guardian Element", 1, 180, 90000001, 1000),
                    new TossObject("Mini Guardian Element", 1, 270, 90000001, 1000),
                    new TimedTransition(12000, "Fight3")
                    ),
                new State("Fight3",
                    new RemCond(ConditionEffectIndex.Invulnerable), // ok
                    new AddCond(ConditionEffectIndex.Armored),
                    new Shoot(25, index: 2, shoots: 20, shootAngle: 10, coolDown: 2500, coolDownOffset: 1500),
                    new HpLessTransition(.10, "Dying")
                    ),
                new State("Dying",
                    new AddCond(ConditionEffectIndex.Invulnerable), // ok
                    new Taunt("Nooooo! This cannot be!"),
                    new TimedTransition(4500, "Dead")
                    ),
                new State("Dead",
                    new Suicide()
                    )
                ),
            new MostDamagers(2,
                new ItemLoot("Potion of Attack", 1.00)
                ),
            new Threshold(0.025,
                new ItemLoot("Prism of Dancing Swords", 0.003),
                new ItemLoot("Puppet Master Skin", 0.002),
                new ItemLoot("Jester Skin", 0.002),
                new ItemLoot("Wine Cellar Incantation", 0.002)
                )
            )
        
        .Init("Minion Puppet",
                new State(
                    new State(
                        new Wander(0.5),
                        new Shoot(25, index: 0, coolDown: 500, coolDownOffset: 1500),
                        new Shoot(25, index: 1, shoots: 3, shootAngle: 20, coolDown: 1500, coolDownOffset: 1500)
                                   )
                ),
                new Threshold(0.6,
                new ItemLoot("Health Potion", 2),
                    new ItemLoot("Magic Potion", 2)
                        )
            )
               .Init("Mini Guardian Element",
                new State(
                    new State(
                        new Circle(1, 1, target: "Oryx Puppet", radiusVariance: 0),
                        new AddCond(ConditionEffectIndex.Invulnerable), // ok
                        new Shoot(25, index: 0, shoots: 3, shootAngle: 20, coolDown: 2000, coolDownOffset: 1500),
                        new TimedTransition(8000, "Despawn")
                            ),
                    new State("Despawn",
                        new Suicide()
                        )
                    )
            )
        .Init("False Puppet Master",
            new State(
                new Taunt(1.00, "Find me if you can, hero, or die trying!"),
                new TransformOnDeath("Rogue Puppet", 1, 1, 1),
                new TransformOnDeath("Assassin Puppet", 1, 1, 1),
                new State(
                    new Swirl(0.45, 6, 6),
                    new Shoot(8.4, shoots: 7, index: 0, coolDown: 2850),
                    new Shoot(8.4, shoots: 3, shootAngle: 30, index: 2, coolDown: 3500),
                    new HpLessTransition(0.11, "dead")
                    ),
                 new State("dead",
                     new AddCond(ConditionEffectIndex.Invulnerable), // ok
                    new Taunt(1.00, "You may have killed me, but I am only a pretender. Get ready for the plot twist!"),
                    new TimedTransition(2500, "die")
                    ),
                 new State("die",
                     new Shoot(8.4, shoots: 8, index: 1, coolDown: 2850),
                     new Suicide()
                    )
                 )
            )

        .Init("The Puppet Master",
                new State(
                    new RealmPortalDrop(),
                    new State("default",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new PlayerWithinTransition(6, "First Stage")
                        ),
                       new State("First Stage",
                        new AddCond(ConditionEffectIndex.Invulnerable), // ok
                        new Taunt(1.00, "Welcome to the Final Act, my friends. My puppets require life essence in order to continue performing…"),
                        new TimedTransition(4250, "move")
                         ),
                     new State("move",
                        new Flashing(0xFFFFFF, 2, 2),
                        new RemCond(ConditionEffectIndex.Invulnerable), // ok
                        new MoveTo(38, 65, speed: 1, isMapPosition: true, once: true),
                        new Taunt(1.00, "It’s not much, but your lives will have to do for now!"),
                        new TimedTransition(5250, "middleShots")
                        ),
                    new State("middleShots",
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 90, coolDownOffset: 0, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 100, coolDownOffset: 200, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 110, coolDownOffset: 400, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 120, coolDownOffset: 600, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 130, coolDownOffset: 800, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 140, coolDownOffset: 1000, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 150, coolDownOffset: 1200, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 160, coolDownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 170, coolDownOffset: 1600, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 180, coolDownOffset: 1800, shootAngle: 90),
                            new Shoot(1, 8, index: 3, coolDown: 4575, angleOffset: 180, coolDownOffset: 2000, shootAngle: 45),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 180, coolDownOffset: 0, shootAngle: 90),
                         new DamageTakenTransition(5200, "sadTime")
                        ),
                    new State("sadTime",
                        new Chase(0.34, 8, 5),
                        new Shoot(10, index: 2, aim: 0.5, coolDown: 1000),
                        new HpLessTransition(.75, "customspiral")
                        ),
                    new State("customspiral",
                        new Taunt(1.00, "Watch them dance hero, as they drain your life away!"),
                        new Spawn("Puppet Wizard 2", initialSpawn: 1, maxChildren: 1, coolDown: 6000),
                        new Spawn("Puppet Priest", initialSpawn: 1, maxChildren: 1, coolDown: 6000),
                        new Spawn("Puppet Knight", initialSpawn: 1, maxChildren: 1, coolDown: 6000),
                       new Shoot(7, shoots: 7, index: 0, coolDown: 10000),
                       new TimedTransition(3000, "middleshots2"),
                       new HpLessTransition(.5, "spookclone")
                        ),
                     new State("customspiral2",
                        new Taunt(1.00, "Watch them dance hero, as they drain your life away!"),
                       new Spawn("Puppet Wizard 2", initialSpawn: 1, maxChildren: 1, coolDown: 6000),
                        new Spawn("Puppet Priest", initialSpawn: 1, maxChildren: 1, coolDown: 6000),
                        new Spawn("Puppet Knight", initialSpawn: 1, maxChildren: 1, coolDown: 6000),
                       new Shoot(7, shoots: 7, index: 0, coolDown: 10000),
                       new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 90, coolDownOffset: 0, shootAngle: 90),
                       new TimedTransition(3000, "middleshots3"),
                       new HpLessTransition(.5, "spookclone")
                          ),
                      new State("middleshots2",
                             new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 90, coolDownOffset: 0, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 100, coolDownOffset: 200, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 110, coolDownOffset: 400, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 120, coolDownOffset: 600, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 130, coolDownOffset: 800, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 140, coolDownOffset: 1000, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 150, coolDownOffset: 1200, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 160, coolDownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 170, coolDownOffset: 1600, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 180, coolDownOffset: 1800, shootAngle: 90),
                            new Shoot(1, 8, index: 3, coolDown: 4575, angleOffset: 180, coolDownOffset: 2000, shootAngle: 45),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 180, coolDownOffset: 0, shootAngle: 90),
                            new TimedTransition(9150, "customspiral"),
                            new HpLessTransition(.5, "spookclone")
                            ),
                            new State("middleshots3",
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 90, coolDownOffset: 0, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 100, coolDownOffset: 200, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 110, coolDownOffset: 400, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 120, coolDownOffset: 600, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 130, coolDownOffset: 800, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 140, coolDownOffset: 1000, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 150, coolDownOffset: 1200, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 160, coolDownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 170, coolDownOffset: 1600, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 180, coolDownOffset: 1800, shootAngle: 90),
                            new Shoot(1, 8, index: 3, coolDown: 4575, angleOffset: 180, coolDownOffset: 2000, shootAngle: 45),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 180, coolDownOffset: 0, shootAngle: 90),
                            new TimedTransition(9150, "customspiral"),
                            new HpLessTransition(.5, "spookclone")
                            ),
                    new State("spookclone",
                        new Spawn("False Puppet Master", maxChildren: 5, coolDown: 500),
                        new Taunt(1.00, "Find me if you can, hero, or die trying!"),
                            new AddCond(ConditionEffectIndex.Armored),
                        new Wander(0.3),
                        new Shoot(7, shoots: 7, index: 0, coolDown: 4500),
                        new Shoot(8, shoots: 3, shootAngle: 16, index: 2, coolDown: 3000),
                        new HpLessTransition(.2, "luckyguess")
                        ),
                    new State("luckyguess",
                        new RemoveEntity(9999, "False Puppet Master"),
                        new Flashing(0xFFFFFF, 2, 2),
                        new Taunt(1.00, "Lucky guess hero, but I've run out of time to play games with you. It is time that you die!"),
                         new AddCond(ConditionEffectIndex.Invulnerable), // ok
                        new MoveTo(38, 54, speed: 0.7, isMapPosition: true, once: true),
                        new TimedTransition(3250, "OutofTime")
                        ),
                   new State("OutofTime",
                       new RemCond(ConditionEffectIndex.Invulnerable), // ok
                            new Spawn("Puppet Wizard", initialSpawn: 1, maxChildren: 2, coolDown: 4500),
                            new Spawn("Puppet Priest", initialSpawn: 1, maxChildren: 1, coolDown: 5000),
                            new Spawn("Puppet Knight", initialSpawn: 1, maxChildren: 1, coolDown: 5000),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 90, coolDownOffset: 0, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 100, coolDownOffset: 200, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 110, coolDownOffset: 400, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 120, coolDownOffset: 600, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 130, coolDownOffset: 800, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 140, coolDownOffset: 1000, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 150, coolDownOffset: 1200, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 160, coolDownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 170, coolDownOffset: 1600, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 180, coolDownOffset: 1800, shootAngle: 90),
                            new Shoot(1, 8, index: 3, coolDown: 4575, angleOffset: 180, coolDownOffset: 2000, shootAngle: 45),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 180, coolDownOffset: 0, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 170, coolDownOffset: 200, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 160, coolDownOffset: 400, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 150, coolDownOffset: 600, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 140, coolDownOffset: 800, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 130, coolDownOffset: 1000, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 120, coolDownOffset: 1200, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 110, coolDownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, index: 3, coolDown: 4575, angleOffset: 100, coolDownOffset: 1600, shootAngle: 90),
                            new HpLessTransition(.1, "NopeImDead")
                        ),
                    new State("NopeImDead",
                        new AddCond(ConditionEffectIndex.Invulnerable), // ok
                        new MoveTo(38, 54, speed: 1, isMapPosition: true, once: true),
                        new Taunt(1.00, "NO!! This cannot be how my story ends!! I WILL HAVE MY ENCORE, HERO!"),
                        new TimedTransition(3250, "YepDead")
                        ),
                   new State("YepDead",
                        new Shoot(7, shoots: 8, index: 1, coolDown: 5000),
                        new Suicide()
                        )
                ),
                new MostDamagers(2,
                    new ItemLoot("Potion of Attack", 1.00)
                ),
                new Threshold(0.025,
                  new TierLoot(8, ItemType.Weapon, 0.05),
                  new TierLoot(9, ItemType.Weapon, 0.03),
                  new TierLoot(3, ItemType.Ability, 0.05),
                  new TierLoot(4, ItemType.Ability, 0.03),
                  new TierLoot(8, ItemType.Armor, 0.05),
                  new TierLoot(9, ItemType.Armor, 0.03),
                  new ItemLoot("Harlequin Armor", 0.0014),
                  new ItemLoot("Prism of Dancing Swords", 0.003),
                  new ItemLoot("Puppet Master Skin", 0.002),
                  new ItemLoot("Jester Skin", 0.002),
                  new ItemLoot("Large Jester Argyle Cloth", 0.05),
                  new ItemLoot("Small Jester Argyle Cloth", 0.05),
                  new ItemLoot("The Fool Tarot Card", 0.045)
                 )
            )
        ;

    }
}