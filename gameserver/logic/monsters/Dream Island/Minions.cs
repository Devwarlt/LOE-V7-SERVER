﻿using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        /// Muzzlereaper
        /// index 0: Muscle Ball (dmg: 90-135)
        /// index 1: Muscle Shuriken (dmg: 85)
        /// index 2: Nightmare Blasterball (dmg: 215)
        private class Muzzlereaper
        {
            public static int muscle_ball = 0;
            public static int muscle_shuriken = 1;
            public static int nightmare_blasterball = 2;
        }

        /// Guzzlereaper
        /// index 0: Muscle Ball (dmg: 135 -190)
        /// index 1: Muscle Shuriken (dmg: 115)
        /// index 2: Nightmare Blasterball (dmg: 215)
        /// index 3: Heavy Nightmare Blasterball (dmg: 260)
        private class Guzzlereaper
        {
            public static int muscle_ball = 0;
            public static int muscle_shuriken = 1;
            public static int nightmare_blasterball = 2;
            public static int heavy_nightmare_blasterball = 3;
        }

        /// Silencer
        /// index 0: Silencer Roar (dmg: 75)
        /// index 1: Silencer Slash (dmg 150)
        /// index 2: Nightmare Spin (dmg: 125-150)
        /// index 3: Nightmare Blasterball (dmg: 215)
        /// index 4: Heavy Nightmare Blasterball (dmg: 260)
        private class Silencer
        {
            public static int silencer_roar = 0;
            public static int silencer_slash = 1;
            public static int nightmare_spin = 2;
            public static int nightmare_blasterball = 3;
            public static int heavy_nightmare_blasterball = 4;
        }

        /// Eyeguard of Surrender
        /// index 0: Eyeguard Slash (dmg: 185-235)
        /// index 1: Eyeguard Megapunch (dmg: 600)
        /// index 2: Enchanted Spear of the Wrath (dmg: 850)
        /// index 3: Nightmare Blasterball (dmg: 215)
        /// index 4: Heavy Nightmare Blasterball (dmg: 260)
        private class Eyeguard_of_Surrender
        {
            public static int eyeguard_slash = 0;
            public static int eyeguard_megapunch = 1;
            public static int enchanted_spear_of_the_wrath = 2;
            public static int nightmare_blasterball = 3;
            public static int heavy_nightmare_blasterball = 4;
        }

        /// Lost Prisoner Soul
        /// index 0: Critical Slash (dmg: 150-215)
        /// index 1: Acid (dmg: 100)
        /// index 2: Bloody Punch (dmg: 250)
        /// index 3: Heavy Rock (dmg: 300)
        private class Lost_Prisoner_Soul
        {
            public static int critical_slash = 0;
            public static int acid = 1;
            public static int bloody_punch = 2;
            public static int heavy_rock = 3;
        }

        /// Nightmare
        /// index 0: Critical Slash (dmg: 150-215)
        /// index 1: Acid (dmg: 100)
        /// index 2: Nightmare Blasterball (dmg: 215)
        /// index 3: Heavy Nightmare Blasterball (dmg: 260)
        private class Nightmare
        {
            public static int critical_slash = 0;
            public static int acid = 1;
            public static int nightmare_blasterball = 2;
            public static int heavy_nightmare_blasterball = 3;
        }

        private _ DreamIslandEnemies = () => Behav()
            .Init("Muzzlereaper",
                new State(
                    new State("idle",
                        new AddCond(effect: ConditionEffectIndex.Invulnerable), // ok
                        new Heal(range: 0, amount: 750, coolDown: 1000),
                        new PlayerWithinTransition(targetState: "begin fight", range: 12)
                    ),
                    new State("begin fight",
                        new RemCond(effect: ConditionEffectIndex.Invulnerable), // ok
                        new Prioritize(
                            new Protect(speed: 8, target: "Guzzlereaper", protectRange: 2),
                            new Chase(speed: 3),
                            new Circle(target: "Guzzlereaper"),
                            new Wander(speed: 2)
                        ),
                        new Heal(range: 0, amount: 250, coolDown: 7500),
                        new Shoot(range: 6, index: Muzzlereaper.muscle_ball, coolDown: 1000),
                        new Shoot(range: 4, shoots: 3, shootAngle: 10, index: Muzzlereaper.muscle_shuriken, coolDown: 2350),
                        new Shoot(range: 6, shoots: 8, shootAngle: 360 / 8, index: Muzzlereaper.nightmare_blasterball, coolDown: 4750),
                        new NoPlayerWithinTransition(targetState: "idle", range: 12)
                    )
                )
            )

            .Init("Guzzlereaper",
                new State(
                    new State("idle",
                        new ReturnToSpawn(),
                        new AddCond(effect: ConditionEffectIndex.Invulnerable), // ok
                        new Heal(range: 0, amount: 1500, coolDown: 1000),
                        new Reproduce(name: "Muzzlereaper", range: 8, max: 3, coolDown: 100),
                        new PlayerWithinTransition(targetState: "begin fight", range: 12)
                    ),
                    new State("begin fight",
                        new RemCond(effect: ConditionEffectIndex.Invulnerable), // ok
                        new Prioritize(
                            new Chase(speed: 4),
                            new Wander(speed: 1)
                        ),
                        new Heal(range: 0, amount: 750, coolDown: 9000),
                        new Shoot(range: 8, shoots: 2, shootAngle: 10, index: Guzzlereaper.muscle_ball, coolDown: 1200),
                        new Shoot(range: 6, shoots: 4, shootAngle: 12, index: Guzzlereaper.muscle_shuriken, coolDown: 2550),
                        new Shoot(range: 8, shoots: 12, shootAngle: 360 / 12, index: Guzzlereaper.nightmare_blasterball, coolDown: 4750),
                        new Shoot(range: 8, shoots: 8, shootAngle: 360 / 8, index: Guzzlereaper.heavy_nightmare_blasterball, coolDown: 4750, coolDownOffset: 200),
                        new NoPlayerWithinTransition(targetState: "idle", range: 12)
                    )
                )
            )

            .Init("Silencer",
                new State(
                    new State("idle",
                        new ReturnToSpawn(),
                        new AddCond(effect: ConditionEffectIndex.Invulnerable), // ok
                        new Heal(range: 0, amount: 1000, coolDown: 1000),
                        new PlayerWithinTransition(targetState: "begin fight", range: 12)
                    ),
                    new State("begin fight",
                        new RemCond(effect: ConditionEffectIndex.Invulnerable), // ok
                        new Prioritize(
                            new Chase(speed: 4),
                            new Wander(speed: 1)
                        ),
                        new Heal(range: 0, amount: 500, coolDown: 7500),
                        new ManaDrainBomb(radius: 2, damage: 75, range: 6, coolDown: 4000, effect: ConditionEffectIndex.Blind, effectDuration: 3000),
                        new Shoot(range: 8, shoots: 6, shootAngle: 30, index: Silencer.silencer_roar, coolDown: 1200),
                        new Shoot(range: 12, index: Silencer.nightmare_spin, coolDown: 2000),
                        new Shoot(range: 4, index: Silencer.silencer_slash, coolDown: 6000),
                        new Shoot(range: 2, shoots: 2, shootAngle: 22.5, index: Silencer.silencer_slash, coolDown: 6000, coolDownOffset: 200),
                        new Shoot(range: 3, shoots: 3, shootAngle: 22.5, index: Silencer.silencer_slash, coolDown: 6000, coolDownOffset: 400),
                        new Shoot(range: 4, shoots: 4, shootAngle: 22.5, index: Silencer.silencer_slash, coolDown: 6000, coolDownOffset: 600),
                        new Shoot(range: 5, shoots: 5, shootAngle: 22.5, index: Silencer.silencer_slash, coolDown: 6000, coolDownOffset: 800),
                        new Shoot(range: 8, shoots: 12, shootAngle: 360 / 12, index: Silencer.nightmare_blasterball, coolDown: 4750),
                        new Shoot(range: 8, shoots: 8, shootAngle: 360 / 8, index: Silencer.heavy_nightmare_blasterball, coolDown: 4750, coolDownOffset: 200),
                        new NoPlayerWithinTransition(targetState: "idle", range: 12)
                    )
                )
            )

            .Init("Eyeguard of Surrender",
                new State(
                    new State("idle",
                        new ReturnToSpawn(),
                        new AddCond(effect: ConditionEffectIndex.Invulnerable), // ok
                        new Heal(range: 0, amount: 3500, coolDown: 1000),
                        new PlayerWithinTransition(targetState: "begin fight", range: 12)
                    ),
                    new State("begin fight",
                        new RemCond(effect: ConditionEffectIndex.Invulnerable), // ok
                        new Prioritize(
                            new Chase(speed: 4),
                            new Wander(speed: 1)
                        ),
                        new Heal(range: 0, amount: 1750, coolDown: 7500),
                        new ManaDrainBomb(radius: 2, damage: 150, range: 8, coolDown: 6000, effect: ConditionEffectIndex.Blind, effectDuration: 4000),
                        new Grenade(radius: 3, damage: 225, range: 8, coolDown: 5000, effect: ConditionEffectIndex.Dazed, effectDuration: 2000, color: 0xFFFF00),
                        new Shoot(range: 4, index: Eyeguard_of_Surrender.eyeguard_slash, coolDown: 6000),
                        new Shoot(range: 2, shoots: 2, shootAngle: 22.5, index: Eyeguard_of_Surrender.eyeguard_slash, coolDown: 6000, coolDownOffset: 200),
                        new Shoot(range: 3, shoots: 3, shootAngle: 22.5, index: Eyeguard_of_Surrender.eyeguard_slash, coolDown: 6000, coolDownOffset: 400),
                        new Shoot(range: 4, shoots: 4, shootAngle: 22.5, index: Eyeguard_of_Surrender.eyeguard_slash, coolDown: 6000, coolDownOffset: 600),
                        new Shoot(range: 5, shoots: 5, shootAngle: 22.5, index: Eyeguard_of_Surrender.eyeguard_slash, coolDown: 6000, coolDownOffset: 800),
                        new Shoot(range: 8, shoots: 12, shootAngle: 360 / 12, index: Eyeguard_of_Surrender.nightmare_blasterball, coolDown: 4750),
                        new Shoot(range: 8, shoots: 8, shootAngle: 360 / 8, index: Eyeguard_of_Surrender.heavy_nightmare_blasterball, coolDown: 4750, coolDownOffset: 200),
                        new Shoot(index: Eyeguard_of_Surrender.eyeguard_megapunch, coolDown: 7500),
                        new Shoot(index: Eyeguard_of_Surrender.enchanted_spear_of_the_wrath, coolDown: 10000),
                        new NoPlayerWithinTransition(targetState: "idle", range: 12)
                    )
                )
            )

            .Init("Lost Prisoner Soul",
                new State(
                    new State("idle",
                        new ReturnToSpawn(),
                        new AddCond(effect: ConditionEffectIndex.Invulnerable), // ok
                        new Heal(range: 0, amount: 1250, coolDown: 1000),
                        new PlayerWithinTransition(targetState: "begin fight", range: 12)
                    ),
                    new State("begin fight",
                        new RemCond(effect: ConditionEffectIndex.Invulnerable), // ok
                        new Prioritize(
                            new Chase(speed: 4),
                            new Wander(speed: 1)
                        ),
                        new Heal(range: 0, amount: 625, coolDown: 7500),
                        new Shoot(range: 4, index: Lost_Prisoner_Soul.critical_slash, coolDown: 6000),
                        new Shoot(range: 2, shoots: 2, shootAngle: 22.5, index: Lost_Prisoner_Soul.critical_slash, coolDown: 6000, coolDownOffset: 200),
                        new Shoot(range: 3, shoots: 3, shootAngle: 22.5, index: Lost_Prisoner_Soul.critical_slash, coolDown: 6000, coolDownOffset: 400),
                        new Shoot(range: 4, shoots: 4, shootAngle: 22.5, index: Lost_Prisoner_Soul.critical_slash, coolDown: 6000, coolDownOffset: 600),
                        new Shoot(range: 5, shoots: 5, shootAngle: 22.5, index: Lost_Prisoner_Soul.critical_slash, coolDown: 6000, coolDownOffset: 800),
                        new Shoot(range: 4, index: Lost_Prisoner_Soul.acid, coolDown: 3000),
                        new Shoot(range: 4, shoots: 2, shootAngle: 10, index: Lost_Prisoner_Soul.acid, coolDown: 3000, coolDownOffset: 200),
                        new Shoot(shoots: 3, shootAngle: 10, index: Lost_Prisoner_Soul.bloody_punch, coolDown: 3750),
                        new Shoot(index: Lost_Prisoner_Soul.heavy_rock, coolDown: 7500),
                        new NoPlayerWithinTransition(targetState: "idle", range: 12)
                    )
                )
            )

            .Init("Nightmare",
                new State(
                    new State("idle",
                        new ReturnToSpawn(),
                        new AddCond(effect: ConditionEffectIndex.Invulnerable), // ok
                        new Heal(range: 0, amount: 1250, coolDown: 1000),
                        new PlayerWithinTransition(targetState: "begin fight", range: 12)
                    ),
                    new State("begin fight",
                        new RemCond(effect: ConditionEffectIndex.Invulnerable), // ok
                        new Prioritize(
                            new Chase(speed: 4),
                            new Wander(speed: 1)
                        ),
                        new Heal(range: 0, amount: 625, coolDown: 7500),
                        new Shoot(range: 4, index: Nightmare.critical_slash, coolDown: 6000),
                        new Shoot(range: 2, shoots: 2, shootAngle: 22.5, index: Nightmare.critical_slash, coolDown: 6000, coolDownOffset: 200),
                        new Shoot(range: 3, shoots: 3, shootAngle: 22.5, index: Nightmare.critical_slash, coolDown: 6000, coolDownOffset: 400),
                        new Shoot(range: 4, shoots: 4, shootAngle: 22.5, index: Nightmare.critical_slash, coolDown: 6000, coolDownOffset: 600),
                        new Shoot(range: 5, shoots: 5, shootAngle: 22.5, index: Nightmare.critical_slash, coolDown: 6000, coolDownOffset: 800),
                        new Shoot(range: 4, index: Nightmare.acid, coolDown: 3000),
                        new Shoot(range: 4, shoots: 2, shootAngle: 10, index: Nightmare.acid, coolDown: 3000, coolDownOffset: 200),
                        new Shoot(range: 8, shoots: 12, shootAngle: 360 / 12, index: Nightmare.nightmare_blasterball, coolDown: 4750),
                        new Shoot(range: 8, shoots: 8, shootAngle: 360 / 8, index: Nightmare.heavy_nightmare_blasterball, coolDown: 4750, coolDownOffset: 200),
                        new NoPlayerWithinTransition(targetState: "idle", range: 12)
                    )
                )
            )
        ;
    }
}