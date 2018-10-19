﻿using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.loot;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ Edition1MiniBosses = () => Behav()
            .Init("Ayrin",
            new State(
                    new State("Awaken",
              new AddCond(ConditionEffectIndex.Invulnerable), // ok
              new PlayerWithinTransition(10, "Start")
                        ),
              new State("Start",
                        new Taunt("My mother... What have you done..."),
                        new RemCond(ConditionEffectIndex.Invulnerable), // ok
                        new Shoot(15, shoots: 2, shootAngle: 16, index: 0, aim: 0.4, coolDown: 800),
                        new Grenade(2.5, 100, 10),
                        new Shoot(20, shoots: 8, shootAngle: 45, index: 1, coolDown: 1100),
                           new HpLessTransition(0.9, "Fuck this shit")
                        ),
                        new State("Fuck this shit",
                            new AddCond(ConditionEffectIndex.Armored),
                                  new Taunt("I will avenge you..."),
                                  new Shoot(25, shoots: 3, shootAngle: 20, index: 0, aim: 0.3, coolDown: 1000),
                                  new Shoot(20, shoots: 6, shootAngle: 40, index: 2, aim: 0.4, coolDown: 900),
                                  new ManaDrainBomb(6, 200, 15, coolDown: 400),
                                  new HpLessTransition(0.77, "Ur actually gay")
                                  ),
                                  new State("Ur actually gay",
                                  new RemCond(ConditionEffectIndex.Armored),
                                  new Taunt("It does not make sense... How do puny mortals like you have such power??"),
                                  new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 90, coolDownOffset: 0, coolDown: 10000),
                                  new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 100, coolDownOffset: 200, coolDown: 10000),
                                  new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 110, coolDownOffset: 400, coolDown: 10000),
                                  new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 120, coolDownOffset: 600, coolDown: 10000),
                                  new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 130, coolDownOffset: 800, coolDown: 10000),
                                  new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 140, coolDownOffset: 1000, coolDown: 10000),
                                  new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 150, coolDownOffset: 1200, coolDown: 10000),
                                  new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 160, coolDownOffset: 1400, coolDown: 10000),
                                  new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 160, coolDownOffset: 0, coolDown: 10000),
                                  new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 150, coolDownOffset: 200, coolDown: 10000),
                                  new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 140, coolDownOffset: 400, coolDown: 10000),
                                  new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 130, coolDownOffset: 600, coolDown: 10000),
                                  new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 120, coolDownOffset: 800, coolDown: 10000),
                                  new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 110, coolDownOffset: 1000, coolDown: 10000),
                                  new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 100, coolDownOffset: 1200, coolDown: 10000),
                                  new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 90, coolDownOffset: 1400, coolDown: 10000),
                                  new HpLessTransition(0.50, "K bye")
                                  ),
                                  new State("K bye",
                                            new Prioritize(
                                            new Chase(speed: 5),
                                              new Wander(speed: 2)
                                              ),
                                            new Taunt("How am I getting overpowered...I was made to be undefeatable..."),
                                            new Shoot(30, shoots: 4, shootAngle: 25, index: 3, aim: 0.5, coolDown: 1000),
                                            new Shoot(25, shoots: 8, shootAngle: 45, index: 1, coolDown: 1200),
                                            new Grenade(2.5, 100, 10),
                                            new Shoot(50, shoots: 1, shootAngle: 1, index: 1, aim: 0.8, coolDown: 700),
                                            new HpLessTransition(0.35, "SpawnMinion")
                                            ),
                                  new State("SpawnMinion",
                                            new AddCond(ConditionEffectIndex.Invulnerable), // ok
                                            new Taunt("Come forth minions, protect me!"),
                                            new Spawn("Icy Twin Succubus", maxChildren: 5, coolDown: 10000),
                                            new TimedTransition(2000, "Checkifdead")
                                                                                        ),
                                  new State("Checkifdead",
                                            new EntitiesNotExistsTransition(50, "Jeff", "Icy Twin Succubus")
                                            ),
                                  new State("Jeff",
                                            new RemCond(ConditionEffectIndex.Invulnerable), // ok
                                            new Taunt("NOOOO MY END IS NEAR!"),
                                            new Shoot(30, shoots: 2, shootAngle: 16, index: 0, aim: 0.4, coolDown: 600),
                                            new Shoot(30, shoots: 8, shootAngle: 45, index: 1, aim: 0.2, coolDown: 1100),
                                            new Grenade(2.5, 100, 10),
                                            new ManaDrainBomb(6, 200, 15, coolDown: 300),
                                            new Shoot(25, shoots: 3, shootAngle: 20, index: 3, aim: 0.3, coolDown: 900),
                                            new HpLessTransition(0.20, "DeathSuicide")
                                            ),
                new State("DeathSuicide",
                          new EntitiesNotExistsTransition(50, "Dead1", "Tamir"),
                          new EntitiesNotExistsTransition(50, "Dead2", "Tamir")
                          ),
              new State("Dead1",
                        new AddCond(ConditionEffectIndex.Armored),
                        new Taunt("Killing my brother is going to be your last mistake..."),
                        new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 90, coolDownOffset: 0, coolDown: 10000),
                        new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 100, coolDownOffset: 200, coolDown: 10000),
                        new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 110, coolDownOffset: 400, coolDown: 10000),
                        new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 120, coolDownOffset: 600, coolDown: 10000),
                        new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 130, coolDownOffset: 800, coolDown: 10000),
                        new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 140, coolDownOffset: 1000, coolDown: 10000),
                        new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 150, coolDownOffset: 1200, coolDown: 10000),
                        new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 160, coolDownOffset: 1400, coolDown: 10000),
                        new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 160, coolDownOffset: 0, coolDown: 10000),
                        new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 150, coolDownOffset: 200, coolDown: 10000),
                        new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 140, coolDownOffset: 400, coolDown: 10000),
                        new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 130, coolDownOffset: 600, coolDown: 10000),
                        new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 120, coolDownOffset: 800, coolDown: 10000),
                        new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 110, coolDownOffset: 1000, coolDown: 10000),
                        new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 100, coolDownOffset: 1200, coolDown: 10000),
                        new Shoot(50, shoots: 8, shootAngle: 45, index: 1, direction: 90, coolDownOffset: 1400, coolDown: 10000),
                        new Shoot(30, shoots: 1, shootAngle: 10, index: 0, aim: 0.5, coolDown: 800),
                        new Shoot(25, shoots: 3, shootAngle: 20, index: 3, aim: 0.3, coolDown: 1100),
                        new HpLessTransition(0.1, "Final Death")
                        ),
              new State("Dead2",
                        new Taunt("Hmph, maybe humans are stronger then I could have imagined..."),

                                  new Shoot(50, shoots: 8, shootAngle: 45, index: 1, coolDown: 1200),
                                  new Shoot(50, shoots: 1, shootAngle: 10, index: 0, aim: 0.4, coolDown: 750),
                                  new Shoot(50, shoots: 4, shootAngle: 25, index: 3, aim: 0.2, coolDown: 900),
                                  new HpLessTransition(0.1, "Final Death")
                                  ),
                                  new State("Final Death",
                                        new AddCond(ConditionEffectIndex.Invulnerable), // ok
                                        new Flashing(0xff0000, 0.8, 60),
                                         new Taunt("Mother...Brother... I am sorry..."),
                                        new TimedTransition(1000, "2")
                                    ),
                                  new State("2",
                                    new RemCond(ConditionEffectIndex.Invulnerable), // ok
                                    new Flashing(0xff0000, 0.6, 60),
                                    new Taunt("Enjoy while you can humans, we shall be back..."),
                                    new TimedTransition(1000, "1")
                                    ),
                                  new State("1",
                                    new AddCond(ConditionEffectIndex.Invulnerable), // ok
                                    new Flashing(0xff0000, 0.3, 70),
                                    new Taunt("Goodbye mortals."),
                                    new TimedTransition(1000, "Goodbye")
                                    ),
                                  new State("Goodbye",
                                                new Shoot(0, shoots: 10, index: 4, shootAngle: 36, direction: 0),
                                                new Suicide()
                                            )
                                          ),

                            new Threshold(0.7,
                                      new ItemLoot("Potion of Attack", 0.8),
                                      new ItemLoot("Potion of Mana", 0.5),
                                      new ItemLoot("Potion of Defense", 0.8),
                                      new ItemLoot("Potion of Speed", 0.8)
                                    ///		  new ItemLoot("Succubus Horn", 0.3)
                                    ),
                                   new Threshold(1,
                    new TierLoot(12, ItemType.Armor, 0.5),
                    new TierLoot(11, ItemType.Armor, 0.4),
                    new TierLoot(10, ItemType.Armor, 0.3)
                              )
            )
         .Init("Tamir",
             new State(
              new State("Awaken",
              new AddCond(ConditionEffectIndex.Invulnerable), // ok
              new PlayerWithinTransition(10, "Start")
                        ),
              new State("Start",
                         new Taunt("My mother... What have you done..."),
                        new RemCond(ConditionEffectIndex.Invulnerable), // ok
                        new Shoot(15, shoots: 2, shootAngle: 16, index: 0, aim: 0.4, coolDown: 800),
                        new Grenade(2.5, 100, 10),
                        new Shoot(20, shoots: 8, shootAngle: 45, index: 1, coolDown: 1100),
                           new HpLessTransition(0.9, "begin")
                        ),
                        new State("begin",
                                  new Taunt("I will avenge you..."),
                                  new Heal(range: 0, amount: 500, coolDown: 7500),
                                              new ManaDrainBomb(radius: 2, damage: 75, range: 6, coolDown: 4000, effect: ConditionEffectIndex.Paralyzed, effectDuration: 1000),
                                            new Shoot(range: 8, shoots: 6, shootAngle: 30, index: 0, coolDown: 1200),
                                              new Shoot(range: 12, index: 2, coolDown: 2000),
                                  new Shoot(range: 4, index: 1, coolDown: 6000),
                                              new Shoot(range: 2, shoots: 2, shootAngle: 22.5, index: 3, coolDown: 6000, coolDownOffset: 200),
                                  new Shoot(range: 3, shoots: 3, shootAngle: 22.5, index: 3, coolDown: 6000, coolDownOffset: 400),
                                  new Shoot(range: 4, shoots: 4, shootAngle: 22.5, index: 3, coolDown: 6000, coolDownOffset: 600),
                                              new Shoot(range: 5, shoots: 5, shootAngle: 22.5, index: 3, coolDown: 6000, coolDownOffset: 800),
                                  new Shoot(range: 8, shoots: 12, shootAngle: 360 / 12, index: 1, coolDown: 4750),
                                              new Shoot(range: 8, shoots: 8, shootAngle: 360 / 8, index: 2, coolDown: 4750, coolDownOffset: 200),
                                  new HpLessTransition(0.77, "Fight")
                                  ),
              new State("Fight",
              new Taunt(1.0, true, "I obtain my Mother's power...", " You will PAY for killing Wingus!"),
                            new AddCond(ConditionEffectIndex.Armored),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
                            new Shoot(range: 50, shoots: 6, shootAngle: 60, index: 1, coolDown: 200, coolDownOffset: 2000),
              new HpLessTransition(0.55, "SpawnMinion")
              ),
                     new State("SpawnMinion",
                     new AddCond(ConditionEffectIndex.Invulnerable), // ok
                     new Spawn("Fiery Twin Succubus", maxChildren: 5, coolDown: 10000),
                     new TimedTransition(2000, "Checkifdead")
                           ),
                     new State("Checkifdead",
                     new EntitiesNotExistsTransition(50, "Warning", "Fiery Twin Succubus")
                           ),
              new State("Warning",
                          new RemCond(ConditionEffectIndex.Invulnerable), // ok
                          new Flashing(0xff0000, 0.5, 60),
                          new Taunt("Hmm.. So you have beaten me... interesting."),
                          new Wander(1),
              new Shoot(10, shoots: 8, index: 3, coolDown: 2000),
              new HpLessTransition(0.2, "Death Encounter")
                        ),
                    new State("Death Encounter",
                         new AddCond(ConditionEffectIndex.Invulnerable), // ok
                         new Flashing(0xff0000, 0.8, 60),
                         new Taunt("I failed..mother... forgive me.."),
                         new TimedTransition(1000, "2")
                        ),
                    new State("2",
                        new RemCond(ConditionEffectIndex.Invulnerable), // ok
                        new Flashing(0xff0000, 0.6, 60),
                        new Taunt("I am dying..."),
                        new TimedTransition(1000, "1")
                        ),
                    new State("1",
                        new AddCond(ConditionEffectIndex.Invulnerable), // ok
                        new Flashing(0xff0000, 0.3, 70),
                        new Taunt("Goodbye."),
                        new TimedTransition(1000, "Goodbye")
                        ),
                    new State("Goodbye",
                        new Shoot(0, shoots: 10, index: 4, shootAngle: 36, direction: 0),
                        new Suicide()
                    )
                      ),
                      new Threshold(0.7,

                                      new ItemLoot("Potion of Dexterity", 0.8),
                                      new ItemLoot("Potion of Mana", 0.5),
                                      new ItemLoot("Potion of Vitality", 0.8),
                                      new ItemLoot("Potion of Wisdom", 0.8)
                                     ///  new ItemLoot("Succubus Horn", 0.3)
                                     ),
                      new Threshold(1,
                    new TierLoot(9, ItemType.Weapon, 0.5),
                    new TierLoot(10, ItemType.Weapon, 0.4),
                    new TierLoot(11, ItemType.Weapon, 0.3)

                                )
                        )
            ;
    }
}