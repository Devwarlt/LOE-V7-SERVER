using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.transitions;
using LoESoft.GameServer.logic.loot;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ Edition1Bosses = () => Behav()
            .Init("Wingus the Succubus Queen",
                      new State(
                          new State("Pause",
                            new AddCond(ConditionEffectIndex.Invulnerable),
                            new PlayerWithinTransition(12, "Start")
                            ),
                          new State("Start",
                            new Taunt("Where am I... who are you people..."),
                            new AddCond(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(2000, "Fight")
                            ),
                          new State("Fight",
                          new Taunt("I do not know of whom you are... but I will make sure you suffer for waking me up!"),
                            new RemCond(ConditionEffectIndex.Invulnerable),
                            new ManaDrainBomb(6, 200, 15, coolDown: 400),
                            new HpLessTransition(0.97, "Fight2")
                            ),
                          new State("Fight2",
                            new Prioritize(
                                new Chase(speed: 4),
                                new Wander(speed: 1)
                              ),
                            new Taunt("Hmm.. humans are strange creatures... it is of no concern to me though."),
                            new Heal(range: 0, amount: 2000, coolDown: 7500),
                       new Shoot(1, shoots: 4, coolDown: 10000, direction: 90, coolDownOffset: 0, shootAngle: 90),
                                    new Shoot(1, shoots: 4, coolDown: 10000, direction: 100, coolDownOffset: 200, shootAngle: 90),
                                    new Shoot(1, shoots: 4, coolDown: 10000, direction: 110, coolDownOffset: 400, shootAngle: 90),
                                    new Shoot(1, shoots: 4, coolDown: 10000, direction: 120, coolDownOffset: 600, shootAngle: 90),
                                    new Shoot(1, shoots: 4, coolDown: 10000, direction: 130, coolDownOffset: 800, shootAngle: 90),
                                    new Shoot(1, shoots: 4, coolDown: 10000, direction: 140, coolDownOffset: 1000, shootAngle: 90),
                                    new Shoot(1, shoots: 4, coolDown: 10000, direction: 150, coolDownOffset: 1200, shootAngle: 90),
                                    new Shoot(1, shoots: 4, coolDown: 10000, direction: 160, coolDownOffset: 1400, shootAngle: 90),
                                    new Shoot(1, shoots: 4, coolDown: 10000, direction: 170, coolDownOffset: 1600, shootAngle: 90),
                                    new Shoot(1, shoots: 4, coolDown: 10000, direction: 180, coolDownOffset: 1800, shootAngle: 90),
                                    new Shoot(1, shoots: 8, coolDown: 10000, direction: 180, coolDownOffset: 2000, shootAngle: 45),
                                    new Shoot(1, shoots: 4, coolDown: 10000, direction: 180, coolDownOffset: 0, shootAngle: 90),
                                    new Shoot(1, shoots: 4, coolDown: 10000, direction: 170, coolDownOffset: 200, shootAngle: 90),
                                    new Shoot(1, shoots: 4, coolDown: 10000, direction: 160, coolDownOffset: 400, shootAngle: 90),
                                    new Shoot(1, shoots: 4, coolDown: 10000, direction: 150, coolDownOffset: 600, shootAngle: 90),
                                    new Shoot(1, shoots: 4, coolDown: 10000, direction: 140, coolDownOffset: 800, shootAngle: 90),
                                    new Shoot(1, shoots: 4, coolDown: 10000, direction: 130, coolDownOffset: 1000, shootAngle: 90),
                                    new Shoot(1, shoots: 4, coolDown: 10000, direction: 120, coolDownOffset: 1200, shootAngle: 90),
                                    new Shoot(1, shoots: 4, coolDown: 10000, direction: 110, coolDownOffset: 1400, shootAngle: 90),
                                    new Shoot(1, shoots: 4, coolDown: 10000, direction: 100, coolDownOffset: 1600, shootAngle: 90),
                                    new Shoot(1, shoots: 4, coolDown: 10000, direction: 90, coolDownOffset: 1800, shootAngle: 90),
                                    new Shoot(1, shoots: 4, coolDown: 10000, direction: 90, coolDownOffset: 2000, shootAngle: 22.5),
                                    new TimedTransition(2000, "Wait")
                                ),
                  new State("Wait",
                                    new AddCond(ConditionEffectIndex.Invulnerable),
                                    new Taunt("Hold up... I don't even know why I am toying with you.  Let me end this."),
                                    new RemCond(ConditionEffectIndex.Invulnerable),
                                    new Chase(7, range: 0.5),
                                    new Shoot(10, shoots: 3, shootAngle: 20, aim: 1, coolDown: 500),
                                    new Flashing(0xff00ff00, 0.1, 20),
                    new HpLessTransition(0.7, "Nightmare")
                                    ),
                new State("Nightmare",
                                  new Taunt("Consider me your worst Nightmare, The Forgotten King has nothing on me!"),
                                     new Flashing(0xff00ff00, 0.2, 30),
                  new Shoot(50, index: 1, shoots: 6, coolDown: 200 * 20, coolDownOffset: 0, angleOffset: 0, shootAngle: 60),
                    new Shoot(50, index: 1, shoots: 6, coolDown: 200 * 20, coolDownOffset: 200 * 1, angleOffset: 5 * 1, shootAngle: 60),
                  new Shoot(50, index: 1, shoots: 6, coolDown: 200 * 20, coolDownOffset: 200 * 2, angleOffset: 5 * 2, shootAngle: 60),
                  new Shoot(50, index: 1, shoots: 6, coolDown: 200 * 20, coolDownOffset: 200 * 3, angleOffset: 5 * 3, shootAngle: 60),
                  new Shoot(50, index: 1, shoots: 6, coolDown: 200 * 20, coolDownOffset: 200 * 4, angleOffset: 5 * 4, shootAngle: 60),
                  new Shoot(50, index: 1, shoots: 6, coolDown: 200 * 20, coolDownOffset: 200 * 5, angleOffset: 5 * 5, shootAngle: 60),
                  new Shoot(50, index: 1, shoots: 6, coolDown: 200 * 20, coolDownOffset: 200 * 6, angleOffset: 5 * 6, shootAngle: 60),
                  new Shoot(50, index: 1, shoots: 6, coolDown: 200 * 20, coolDownOffset: 200 * 7, angleOffset: 5 * 7, shootAngle: 60),
                  new Shoot(50, index: 1, shoots: 6, coolDown: 200 * 20, coolDownOffset: 200 * 8, angleOffset: 5 * 8, shootAngle: 60),
                  new Shoot(50, index: 1, shoots: 6, coolDown: 200 * 20, coolDownOffset: 200 * 9, angleOffset: 5 * 9, shootAngle: 60),
                  new Shoot(50, index: 1, shoots: 6, coolDown: 200 * 20, coolDownOffset: 200 * 10, angleOffset: 5 * 10, shootAngle: 60),
                  new Shoot(50, index: 1, shoots: 6, coolDown: 200 * 20, coolDownOffset: 200 * 11, angleOffset: 5 * 11, shootAngle: 60),
                  new Shoot(50, index: 1, shoots: 6, coolDown: 200 * 20, coolDownOffset: 200 * 12, angleOffset: 5 * 12, shootAngle: 60),
                  new Shoot(50, index: 1, shoots: 6, coolDown: 200 * 20, coolDownOffset: 200 * 13, angleOffset: 5 * 13, shootAngle: 60),
                  new Shoot(50, index: 1, shoots: 6, coolDown: 200 * 20, coolDownOffset: 200 * 14, angleOffset: 5 * 14, shootAngle: 60),
                  new Shoot(50, index: 1, shoots: 6, coolDown: 200 * 20, coolDownOffset: 200 * 15, angleOffset: 5 * 15, shootAngle: 60),
                  new Shoot(50, index: 1, shoots: 6, coolDown: 200 * 20, coolDownOffset: 200 * 16, angleOffset: 5 * 16, shootAngle: 60),
                  new Shoot(50, index: 1, shoots: 6, coolDown: 200 * 20, coolDownOffset: 200 * 17, angleOffset: 5 * 17, shootAngle: 60),
                  new Shoot(50, index: 1, shoots: 6, coolDown: 200 * 20, coolDownOffset: 200 * 18, angleOffset: 5 * 18, shootAngle: 60),
                  new HpLessTransition(0.5, "Nightmare2")
                                  ),
              new State("Nightmare2",
                  new Taunt("Every possible battle option you have in mind will end in failure."),
                  new Shoot(30, index: 1, shoots: 1, coolDown: 1000),
                  new Shoot(50, index: 4, shoots: 5, coolDown: 4000, coolDownOffset: 0, angleOffset: 0, shootAngle: 9),
                  new Shoot(50, index: 4, shoots: 7, coolDown: 4000, coolDownOffset: 400, angleOffset: 0, shootAngle: 9),
                  new Shoot(50, index: 4, shoots: 9, coolDown: 4000, coolDownOffset: 800, angleOffset: 0, shootAngle: 9),
                  new Shoot(50, index: 4, shoots: 9, coolDown: 4000, coolDownOffset: 1200, angleOffset: 0, shootAngle: 9),
                  new Shoot(50, index: 4, shoots: 9, coolDown: 4000, coolDownOffset: 1600, angleOffset: 0, shootAngle: 9),
                  new Shoot(50, index: 4, shoots: 5, coolDown: 4000, coolDownOffset: 0, angleOffset: 180, shootAngle: 9),
                  new Shoot(50, index: 4, shoots: 7, coolDown: 4000, coolDownOffset: 400, angleOffset: 180, shootAngle: 9),
                  new Shoot(50, index: 4, shoots: 9, coolDown: 4000, coolDownOffset: 800, angleOffset: 180, shootAngle: 9),
                  new Shoot(50, index: 4, shoots: 9, coolDown: 4000, coolDownOffset: 1200, angleOffset: 180, shootAngle: 9),
                  new Shoot(50, index: 4, shoots: 9, coolDown: 4000, coolDownOffset: 1600, angleOffset: 180, shootAngle: 9),
                  new Shoot(50, index: 4, shoots: 5, coolDown: 4000, coolDownOffset: 2000, angleOffset: 90, shootAngle: 9),
                  new Shoot(50, index: 4, shoots: 7, coolDown: 4000, coolDownOffset: 2400, angleOffset: 90, shootAngle: 9),
                  new Shoot(50, index: 4, shoots: 9, coolDown: 4000, coolDownOffset: 2800, angleOffset: 90, shootAngle: 9),
                  new Shoot(50, index: 4, shoots: 9, coolDown: 4000, coolDownOffset: 3200, angleOffset: 90, shootAngle: 9),
                  new Shoot(50, index: 4, shoots: 9, coolDown: 4000, coolDownOffset: 3600, angleOffset: 90, shootAngle: 9),
                  new Shoot(50, index: 4, shoots: 5, coolDown: 4000, coolDownOffset: 2000, angleOffset: 270, shootAngle: 9),
                  new Shoot(50, index: 4, shoots: 7, coolDown: 4000, coolDownOffset: 2400, angleOffset: 270, shootAngle: 9),
                  new Shoot(50, index: 4, shoots: 9, coolDown: 4000, coolDownOffset: 2800, angleOffset: 270, shootAngle: 9),
                  new Shoot(50, index: 4, shoots: 9, coolDown: 4000, coolDownOffset: 3200, angleOffset: 270, shootAngle: 9),
                  new Shoot(50, index: 4, shoots: 9, coolDown: 4000, coolDownOffset: 3600, angleOffset: 270, shootAngle: 9),
                  new HpLessTransition(0.4, "Waiting")
                                    ),
                          new State("Waiting",
                                  new AddCond(ConditionEffectIndex.Invulnerable),
                  new Flashing(0xFF0000, .1, 1000),
                  new RemCond(ConditionEffectIndex.Invulnerable),
                  new Shoot(10, shoots: 3, shootAngle: 20, aim: 1, coolDown: 500),
                  new HpLessTransition(0.25, "Spawn")
                                  ),
              new State("Spawn",
                    new AddCond(ConditionEffectIndex.Invulnerable),
                  new Spawn("Ayrin", maxChildren: 1),
                    new Spawn("Tamir", maxChildren: 1),
                  new TimedTransition(2500, "CheckifDead")
                                  ),

                  new State("CheckifDead",
                  new AddCond(ConditionEffectIndex.Invincible),
                  new TimedTransition(5000, "idle"),
                  new EntitiesNotExistsTransition(75, "Dead", "Ayrin", "Tamir")
                ),
              new State("idle",
                  new AddCond(ConditionEffectIndex.Invincible),
                  new TimedTransition(5000, "CheckifDead")
                        ),
              new State("Dead",
                  new AddCond(ConditionEffectIndex.Invulnerable),
                  new Taunt(0.99, "Finally, peace and rest at last."),
                  new Flashing(0xFF0000, .1, 1000),
                  new TimedTransition(2000, "Suicide")
                                  ),
                            new State("Suicide",
                  new AddCond(ConditionEffectIndex.StunImmune),
                  new Shoot(0, 8, direction: 360 / 8, index: 1),
                  new Suicide()
                  )
                  ),
             new MostDamagers(5,
                  new ItemLoot("Potion of Life", 0.5),
                  new ItemLoot("Potion of Mana", 0.5),
                  new ItemLoot("Potion of Defense", 1)
                                    ),
              ///  new MostDamagers(7,
              ///	  new ItemLoot("Succubus Horn", 1)
              ///	  ),

              new MostDamagers(1,
                  new WhiteBag("Succubus BloodStone", eventChest: false)

                                   )
            )
            ;
    }
}