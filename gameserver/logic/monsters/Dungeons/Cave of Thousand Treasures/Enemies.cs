using gameserver.logic.behaviors;
using gameserver.logic.loot;
using gameserver.logic.transitions;

namespace gameserver.logic
{
    partial class BehaviorDb
    {
        private _ TreasureCave = () => Behav() //this Behavior is made by NoobHereo. If you have it your a lucky man and should give credits. 
            .Init("Golden Oryx Effigy",
             new State(
                    new State("default",
            new PlayerWithinTransition(8, "setsize1")
                        ),
                    new State("setsize1",
                        new ChangeSize(20, 165),
                        new TimedTransition(1100, "spawnminions1")
                        ),
                    new State("spawnminions1",
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TossObject("Gold Planet", range: 9.9, angle: 90),
                        new TossObject("Gold Planet", range: 9.9, angle: 50),
                        new TossObject("Gold Planet", range: 9.9, angle: 360),
                        new TossObject("Gold Planet", range: 9.9, angle: 320),
                        new TossObject("Gold Planet", range: 9.9, angle: 270),
                        new TossObject("Gold Planet", range: 9.9, angle: 220),
                        new TossObject("Gold Planet", range: 9.9, angle: 180),
                        new TossObject("Gold Planet", range: 9.9, angle: 130),
                        new TossObject("Treasure Oryx Defender", range: 2.0, angle: 90),
                        new TossObject("Treasure Oryx Defender", range: 2.0, angle: 360),
                        new TossObject("Treasure Oryx Defender", range: 2.0, angle: 270),
                        new TossObject("Treasure Oryx Defender", range: 2.0, angle: 180),
                        new TimedTransition(750, "setsize2")
                        ),
                      new State("setsize2",
                          new ChangeSize(20, 120),
                          new TimedTransition(250, "checkprotectors")
                           ),
                      new State("checkprotectors",
                          new AddCond(ConditionEffectIndex.Invulnerable),
                          new EntityNotExistsTransition("Treasure Oryx Defender", 50, "blink1")
                                ),
                            new State("blink1",
                                new SetAltTexture(1),
                                new TimedTransition(100, "Grenade")
                                ),
                            new State("Grenade",
                                new Grenade(2, 5, 8, 225, coolDown: 1000),
                                new Grenade(4, 5, 8,315, coolDown: 1000),
                                new Grenade(4, 5, 8,  45, coolDown: 1000),
                                new Grenade(4, 5, 8, 135, coolDown: 1000),
                                new Grenade(2, 5, 2,  270, coolDown: 1300),
                                new Grenade(4, 5, 2,  0, coolDown: 1300),
                                new Grenade(4, 5, 2,  90, coolDown: 1300),
                                new Grenade(4, 5, 2,  180, coolDown: 1300)
                                )))
                       .Init("Treasure Oryx Defender",
                            new State(
                                new Circle(5, 2, sightRange: 10, target: null, speedVariance: null, radiusVariance: null),
                                new Shoot(10, shoots: 7, index: 0, angleOffset: fixedAngle_RingAttack2)
                                ))
                        .Init("Gold Planet",
                              new State(
                                   new Circle(5, 8, sightRange: 10, target: null, speedVariance: null, radiusVariance: null)
                                ));
    }
}