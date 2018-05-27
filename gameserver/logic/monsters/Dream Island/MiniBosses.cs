//using LoESoft.GameServer.logic.behaviors;
//using LoESoft.GameServer.logic.transitions;
//using System;

//namespace LoESoft.GameServer.logic
//{
//    partial class BehaviorDb
//    {
//        private _ DreamIslandMiniBosses = () => Behav()
//            /// Hawkeye the Dream Eater
//            /// index 0: Muscle Ball (dmg: 135 -190)
//            /// index 1: Muscle Shuriken (dmg: 115)
//            /// index 2: Nightmare Blasterball (dmg: 215)
//            /// index 3: Heavy Nightmare Blasterball (dmg: 260)
//            .Init("Hawkeye the Dream Eater",
//                new State(
//                    new State("healing",
//                        new Heal(range: 0, amount: 1000, coolDown: 5000),
//                        new ReturnToSpawn(speed: 10, once: true),
//                        new AddCond(effect: ConditionEffectIndex.Invulnerable),
//                        new PlayerWithinTransition(range: 12, targetState: "attack 1")
//                    ),
//                    new State("attack 1",
//                        new RemCond(effect: ConditionEffectIndex.Invulnerable),
//                        new Chase(speed: 8, range: 0),
//                        new Shoot(shoots: 2, shootAngle: 10, coolDown: 1000),
//                        //Muscle Shotgun
//                        new Shoot(range: 4, index: 1, coolDown: 4000),
//                        new Shoot(range: 4, shoots: 2, shootAngle: 10, index: 1, coolDown: 4000, coolDownOffset: 250),
//                        new Shoot(range: 4, shoots: 3, shootAngle: 10, index: 1, coolDown: 4000, coolDownOffset: 500),
//                        new Shoot(range: 4, shoots: 4, shootAngle: 10, index: 1, coolDown: 4000, coolDownOffset: 750),
//                        //--
//                        new Wander(speed: 1),
//                        new TimedTransition(targetState: "prepare attack 2", coolDown: 20000),
//                        new NoPlayerWithinTransition(range: 12, targetState: "healing")
//                    ),
//                    new State("prepare attack 2",
//                        new AddCond(effect: ConditionEffectIndex.Armored, duration: 25000),
//                        new Flashing(color: 0, flashPeriod: 0.2, flashRepeats: 25),
//                        new Shoot(shoots: 2, shootAngle: 10, coolDown: 1000),
//                        new Wander(speed: 1),
//                        new TimedTransition(targetState: "attack 2", coolDown: 5000)
//                    ),
//                    new State("attack 2",
//                        new Chase(speed: 12, range: 0),
//                        new Flashing(color: 0xFF0000, flashPeriod: 0.5, flashRepeats: 40),
//                        new Shoot(shoots: 2, shootAngle: 10, coolDown: 1000),
//                        //Muscle Shotgun
//                        new Shoot(range: 4, index: 1, aim: 1, coolDown: 4000),
//                        new Shoot(range: 4, shoots: 2, shootAngle: 10, index: 1, coolDown: 4000, coolDownOffset: 250),
//                        new Shoot(range: 4, shoots: 3, shootAngle: 10, index: 1, coolDown: 4000, coolDownOffset: 500),
//                        new Shoot(range: 4, shoots: 4, shootAngle: 10, index: 1, coolDown: 4000, coolDownOffset: 750),
//                        //--
//                        //Muscle Spiral (clockwise + reverse clockwise)
//                        new Shoot(range: 6, shoots: 4, index: 1, shootAngle: 90, direction: 0, coolDown: 2400),
//                        new Shoot(range: 6, shoots: 4, index: 1, shootAngle: 90, direction: 15, coolDown: 2400, coolDownOffset: 500),
//                        new Shoot(range: 6, shoots: 4, index: 1, shootAngle: 90, direction: 30, coolDown: 2400, coolDownOffset: 500*2),
//                        new Shoot(range: 6, shoots: 4, index: 1, shootAngle: 90, direction: 45, coolDown: 2400, coolDownOffset: 500*3),
//                        new Shoot(range: 6, shoots: 4, index: 1, shootAngle: 90, direction: 60, coolDown: 2400, coolDownOffset: 500*4),
//                        new Shoot(range: 6, shoots: 4, index: 1, shootAngle: 90, direction: 75, coolDown: 2400, coolDownOffset: 500*5),
//                        new Shoot(range: 6, shoots: 4, index: 1, shootAngle: 90, direction: 90, coolDown: 2400, coolDownOffset: 500*6),
//                        new Shoot(range: 6, shoots: 4, index: 1, shootAngle: 90, direction: 75, coolDown: 2400, coolDownOffset: 500*7),
//                        new Shoot(range: 6, shoots: 4, index: 1, shootAngle: 90, direction: 60, coolDown: 2400, coolDownOffset: 500*8),
//                        new Shoot(range: 6, shoots: 4, index: 1, shootAngle: 90, direction: 45, coolDown: 2400, coolDownOffset: 500*9),
//                        new Shoot(range: 6, shoots: 4, index: 1, shootAngle: 90, direction: 30, coolDown: 2400, coolDownOffset: 500*10),
//                        new Shoot(range: 6, shoots: 4, index: 1, shootAngle: 90, direction: 15, coolDown: 2400, coolDownOffset: 500*11),
//                        //--
//                        //Nightmare Bomb
//                        new Shoot(shoots: 16, shootAngle: 360 / 16, index: 2, aim: 1, coolDown: 10000),
//                        //--
//                        new Wander(speed: 1),
//                        new TimedTransition(targetState: "attack 3", coolDown: 20000)
//                    ),
//                    new State("attack 3",
//                        new Heal(range: 0, amount: 500, coolDown: 5000),
//                        new Chase(),
//                        //Darkness Bomb
//                        new Grenade(radius: new Random().Next(2, 3), damage: new Random().Next(35, 55), range: new Random().Next(3, 5), direction: 0, color: 0, effect: ConditionEffectIndex.Sick, effectDuration: 5000, coolDown: 500),
//                        new Grenade(radius: new Random().Next(2, 3), damage: new Random().Next(35, 55), range: new Random().Next(3, 5), direction: 45, color: 0, effect: ConditionEffectIndex.Sick, effectDuration: 5000, coolDown: 500),
//                        new Grenade(radius: new Random().Next(2, 3), damage: new Random().Next(35, 55), range: new Random().Next(3, 5), direction: 90, color: 0, effect: ConditionEffectIndex.Sick, effectDuration: 5000, coolDown: 500),
//                        new Grenade(radius: new Random().Next(2, 3), damage: new Random().Next(35, 55), range: new Random().Next(3, 5), direction: 135, color: 0, effect: ConditionEffectIndex.Sick, effectDuration: 5000, coolDown: 500),
//                        new Grenade(radius: new Random().Next(2, 3), damage: new Random().Next(35, 55), range: new Random().Next(3, 5), direction: 180, color: 0, effect: ConditionEffectIndex.Sick, effectDuration: 5000, coolDown: 500),
//                        new Grenade(radius: new Random().Next(2, 3), damage: new Random().Next(35, 55), range: new Random().Next(3, 5), direction: 225, color: 0, effect: ConditionEffectIndex.Sick, effectDuration: 5000, coolDown: 500),
//                        new Grenade(radius: new Random().Next(2, 3), damage: new Random().Next(35, 55), range: new Random().Next(3, 5), direction: 270, color: 0, effect: ConditionEffectIndex.Sick, effectDuration: 5000, coolDown: 500),
//                        new Grenade(radius: new Random().Next(2, 3), damage: new Random().Next(35, 55), range: new Random().Next(3, 5), direction: 315, color: 0, effect: ConditionEffectIndex.Sick, effectDuration: 5000, coolDown: 500),
//                        //--
//                        new Flashing(color: 0x8B0000, flashPeriod: 0.25, flashRepeats: 120),
//                        //Heavy Nightmare Spiral (clockwise + reverse clockwise)
//                        new Shoot(range: 6, shoots: 8, index: 3, shootAngle: 45, direction: 0, coolDown: 2000),
//                        new Shoot(range: 6, shoots: 8, index: 3, shootAngle: 45, direction: 15, coolDown: 2000, coolDownOffset: 500),
//                        new Shoot(range: 6, shoots: 8, index: 3, shootAngle: 45, direction: 30, coolDown: 2000, coolDownOffset: 500*2),
//                        new Shoot(range: 6, shoots: 8, index: 3, shootAngle: 45, direction: 45, coolDown: 2000, coolDownOffset: 500*3),
//                        new Shoot(range: 6, shoots: 8, index: 3, shootAngle: 45, direction: 60, coolDown: 2000, coolDownOffset: 500*4),
//                        new Shoot(range: 6, shoots: 8, index: 3, shootAngle: 45, direction: 75, coolDown: 2000, coolDownOffset: 500*5),
//                        new Shoot(range: 6, shoots: 8, index: 3, shootAngle: 45, direction: 90, coolDown: 2000, coolDownOffset: 500*6),
//                        new TimedTransition(targetState: "attack 1", coolDown: 30000)
//                    )
//                )
//            )

//            /// Berunir the Hidden Murderer
//            /// index 0: Silencer Roar (dmg: 75)
//            /// index 1: Silencer Slash (dmg 150)
//            /// index 2: Nightmare Spin (dmg: 125-150)
//            /// index 3: Nightmare Blasterball (dmg: 215)
//            /// index 4: Heavy Nightmare Blasterball (dmg: 260)
//            .Init("Berunir the Hidden Murderer",
//                new State(
//                    new State("healing",
//                        new Heal(range: 0, amount: 1000, coolDown: 5000),
//                        new ReturnToSpawn(speed: 10, once: true),
//                        new AddCond(effect: ConditionEffectIndex.Invulnerable),
//                        new PlayerWithinTransition(range: 12, targetState: "attack 1")
//                    ),
//                    new State("attack 1",
//                        new RemCond(effect: ConditionEffectIndex.Invulnerable),
//                        new ManaDrainBomb(radius: 2, damage: 75, range: 8, coolDown: 4000, effect: ConditionEffectIndex.Blind, effectDuration: 3000),
//                        new Shoot(aim: 1, coolDown: 1000),
//                        //Slash Shotgun
//                        new Shoot(range: 4, index: 1, aim: 1, coolDown: 3000),
//                        new Shoot(range: 4, shoots: 2, shootAngle: 10, index: 1, coolDown: 3000, coolDownOffset: 250),
//                        new Shoot(range: 4, shoots: 3, shootAngle: 10, index: 1, coolDown: 3000, coolDownOffset: 500),
//                        new Shoot(range: 4, shoots: 4, shootAngle: 10, index: 1, coolDown: 3000, coolDownOffset: 750),
//                        //--
//                        new Retreat(speed: 10, range: 6),
//                        new TimedTransition(targetState: "attack 2", coolDown: 10000),
//                        new NoPlayerWithinTransition(targetState: "healing", range: 12)
//                    ),
//                    new State("attack 2",
//                        new ManaDrainBomb(radius: 2, damage: 75, range: 8, coolDown: 4000, effect: ConditionEffectIndex.Blind, effectDuration: 3000),
//                        new Shoot(aim: 1, coolDown: 1000),
//                        //Nightmare Spin Wave
//                        new Shoot(range: 4, index: 2, aim: 1, coolDown: 3000),
//                        new Shoot(range: 4, shoots: 2, shootAngle: 10, index: 2, coolDown: 3000, coolDownOffset: 250),
//                        new Shoot(range: 4, shoots: 3, shootAngle: 10, index: 2, coolDown: 3000, coolDownOffset: 500),
//                        new Shoot(range: 4, shoots: 4, shootAngle: 10, index: 2, coolDown: 3000, coolDownOffset: 750),
//                        //--
//                        new Retreat(speed: 12, range: 6),
//                        new TimedTransition(targetState: "attack 3", coolDown: 20000),
//                        new NoPlayerWithinTransition(targetState: "healing", range: 12)
//                    ),
//                    new State("attack 3",
//                        new TimedTransition(targetState: "attack 1", coolDown: 10000),
//                        new State("phase 1",
//                            new HpLessTransition(targetState: "random wandering", threshold: 0.5),
//                            new Heal(range: 0, amount: 500, coolDown: 3000),
//                            new Retreat(speed: 12, range: 6),
//                            new Shoot(aim: 1, coolDown: 1000),
//                            //Nightmare Spin Wave
//                            new Shoot(range: 4, index: 2, aim: 1, coolDown: 3000),
//                            new Shoot(range: 4, shoots: 2, shootAngle: 10, index: 2, coolDown: 3000, coolDownOffset: 250),
//                            new Shoot(range: 4, shoots: 3, shootAngle: 10, index: 2, coolDown: 3000, coolDownOffset: 500),
//                            new Shoot(range: 4, shoots: 4, shootAngle: 10, index: 2, coolDown: 3000, coolDownOffset: 750),
//                            //--
//                            new PlayerWithinTransition(targetState: "phase 2", range: 4)
//                        ),
//                        new State("phase 2",
//                            new HpLessTransition(targetState: "random wandering", threshold: 0.5),
//                            new Heal(range: 0, amount: 500, coolDown: 3000),
//                            new Retreat(speed: 12),
//                            //Nightmare Explosion 1
//                            new Shoot(range: 4, shoots: 16, shootAngle: 360 / 16, index: 3, coolDown: 2000),
//                            //--
//                            //Nightmare Explosion 2
//                            new Shoot(range: 6, shoots: 8, shootAngle: 360 / 8, index: 4, coolDown: 2000, coolDownOffset: 500),
//                            //--
//                            new NoPlayerWithinTransition(targetState: "phase 2", range: 4)
//                        )
//                    ),
//                    new State("random wandering",
//                        new Buzz(speed: 10, range: 8, coolDown: 5000),
//                        new Heal(amount: 500, range: 0, coolDown: 1000),
//                        new ManaDrainBomb(radius: 2, damage: 75, range: 8, coolDown: 4000, effect: ConditionEffectIndex.Blind, effectDuration: 3000),
//                        //Nightmare Explosion 1
//                        new Shoot(range: 4, shoots: 16, shootAngle: 360 / 16, index: 3, coolDown: 2000),
//                        //--
//                        //Nightmare Explosion 2
//                        new Shoot(range: 6, shoots: 8, shootAngle: 360 / 8, index: 4, coolDown: 2000, coolDownOffset: 500),
//                        //--
//                        new Flashing(color: 0xFF0000, flashPeriod: 0.25, flashRepeats: 40),
//                        new TimedTransition(targetState: "healing 2", coolDown: 10000),
//                        new NoPlayerWithinTransition(targetState: "healing", range: 12)
//                    ),
//                    new State("healing 2",
//                        new AddCond(effect: ConditionEffectIndex.Armored, duration: 5000),
//                        new Heal(amount: 500, range: 0, coolDown: 1000),
//                        new Shoot(aim: 1, coolDown: 1000),
//                        //Nightmare Spin Wave
//                        new Shoot(range: 4, index: 2, aim: 1, coolDown: 3000),
//                        new Shoot(range: 4, shoots: 2, shootAngle: 10, index: 2, coolDown: 3000, coolDownOffset: 250),
//                        new Shoot(range: 4, shoots: 3, shootAngle: 10, index: 2, coolDown: 3000, coolDownOffset: 500),
//                        new Shoot(range: 4, shoots: 4, shootAngle: 10, index: 2, coolDown: 3000, coolDownOffset: 750),
//                        //--
//                        new TimedTransition(targetState: "attack 1", coolDown: 10000),
//                        new NoPlayerWithinTransition(targetState: "healing", range: 12)
//                    )
//                )
//            )

//            /// Haiguz the Ancient Paladin
//            /// index 0: Eyeguard Slash (dmg: 185-235)
//            /// index 1: Eyeguard Megapunch (dmg: 600)
//            /// index 2: Enchanted Spear of the Wrath (dmg: 850)
//            /// index 3: Nightmare Blasterball (dmg: 215)
//            /// index 4: Heavy Nightmare Blasterball (dmg: 260)
//            .Init("Haiguz the Ancient Paladin",
//                new State(
//                    new State("healing",
//                        new Heal(range: 0, amount: 1000, coolDown: 5000),
//                        new ReturnToSpawn(speed: 10, once: true),
//                        new AddCond(effect: ConditionEffectIndex.Invulnerable),
//                        new PlayerWithinTransition(range: 12, targetState: "attack 1")
//                    ),
//                    new State("attack 1",
//                        new Chase(speed: 4),
//                        //Tri Shoot
//                        new Shoot(shoots: 2, shootAngle: 10, coolDown: 1500),
//                        new Shoot(coolDown: 1500, coolDownOffset: 200),
//                        //--
//                        //Quadri Shoot
//                        new Shoot(shoots: 4, shootAngle: 90, coolDown: 5000),
//                        //--
//                        new Grenade(radius: 2, damage: 200, coolDown: 0xFFFF00, effect: ConditionEffectIndex.Bleeding, effectDuration: 3000),
//                        new TimedTransition(targetState: "prepare attack 2", coolDown: 15000),
//                        new NoPlayerWithinTransition(targetState: "healing", range: 12)
//                    ),
//                    new State("prepare attack 2",
//                        new AddCond(effect: ConditionEffectIndex.Invulnerable),
//                        new Flashing(color: 0xFFFF00, flashPeriod: 0.5, flashRepeats: 10),
//                        //Tri Shoot
//                        new Shoot(shoots: 2, shootAngle: 10, coolDown: 1500),
//                        new Shoot(coolDown: 1500, coolDownOffset: 200),
//                        //--
//                        new TimedTransition(targetState: "attack 2", coolDown: 5000)
//                    ),
//                    new State("attack 2",
//                        new RemCond(effect: ConditionEffectIndex.Invulnerable),
//                        new AddCond(effect: ConditionEffectIndex.Armored, duration: 5000),
//                        new Chase(speed: 8),
//                        //Tri Shoot Aimed
//                        new Shoot(shoots: 2, shootAngle: 10, aim: 1, coolDown: 1500),
//                        new Shoot(coolDown: 1500, aim: 1, coolDownOffset: 200),
//                        //--
//                        //Quadri Shoot Aimed
//                        new Shoot(range: 4, shoots: 4, shootAngle: 25, index: 1, coolDown: 5000),
//                        //--
//                        new ManaDrainBomb(radius: 2, damage: 50, coolDown: 3500, effect: ConditionEffectIndex.Blind, effectDuration: 3000),
//                        new Grenade(radius: 2, damage: 200, color: 0xFFFF00, coolDown: 3500, effect: ConditionEffectIndex.Bleeding, effectDuration: 3000),
//                        //Enchanted Spear
//                        new Shoot(range: 24, index: 2, aim: 1, coolDown: 5000, coolDownOffset: 500),
//                        //--
//                        new TimedTransition(targetState: "attack 3", coolDown: 20000),
//                        new NoPlayerWithinTransition(targetState: "healing", range: 12)
//                        //--
//                    ),
//                    new State("attack 3",
//                        new Chase(speed: 10),
//                        //Nightmare Explosion 1
//                        new Shoot(range: 4, shoots: 16, shootAngle: 360 / 16, index: 3, coolDown: 1000),
//                        //--
//                        //Nightmare Explosion 2
//                        new Shoot(range: 6, shoots: 8, shootAngle: 360 / 8, index: 4, coolDown: 1000, coolDownOffset: 500),
//                        //--
//                        new ManaDrainBomb(radius: 2, damage: 50, coolDown: 3500, effect: ConditionEffectIndex.Blind, effectDuration: 3000),
//                        new Grenade(radius: 2, damage: 200, color: 0xFFFF00, coolDown: 3500, effect: ConditionEffectIndex.Bleeding, effectDuration: 3000),
//                        //Enchanted Spear
//                        new Shoot(range: 24, index: 2, aim: 1, coolDown: 5000, coolDownOffset: 500),
//                        //--
//                        new TimedTransition(targetState: "attack 1", coolDown: 20000)
//                    )
//                )
//            )

//            /// Chainblaze the Elder Prisoner
//            /// index 0: Critical Slash (dmg: 150-215)
//            /// index 1: Acid (dmg: 100)
//            /// index 2: Bloody Punch (dmg: 250)
//            /// index 3: Heavy Rock (dmg: 300)
//            .Init("Chainblaze the Elder Prisoner",
//                new State(

//                )
//            )

//            /// Shadownzur the Unrealized Demon
//            /// index 0: Critical Slash (dmg: 150-215)
//            /// index 1: Acid (dmg: 100)
//            /// index 2: Nightmare Blasterball (dmg: 215)
//            /// index 3: Heavy Nightmare Blasterball (dmg: 260)
//            .Init("Nightmare",
//                new State(

//                )
//            )
//        ;
//    }
//}