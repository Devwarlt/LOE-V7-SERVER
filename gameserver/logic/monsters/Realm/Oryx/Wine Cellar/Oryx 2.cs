﻿using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.loot;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ OryxWineCellarOryx = () => Behav()
        .Init("Oryx the Mad God 2",
            new State(
                new State("Attack",
                    new Chase(1, 15, 3),
                    new Wander(.5),
                    new Shoot(25, index: 0, shoots: 8, shootAngle: 45, coolDown: 1500, coolDownOffset: 1500),
                    new Shoot(25, index: 1, shoots: 3, shootAngle: 10, coolDown: 1000, coolDownOffset: 1000),
                    new Shoot(25, index: 2, shoots: 3, shootAngle: 10, aim: 0.2, coolDown: 1000, coolDownOffset: 1000),
                    new Shoot(25, index: 3, shoots: 2, shootAngle: 10, aim: 0.4, coolDown: 1000, coolDownOffset: 1000),
                    new Shoot(25, index: 4, shoots: 3, shootAngle: 10, aim: 0.6, coolDown: 1000, coolDownOffset: 1000),
                    new Shoot(25, index: 5, shoots: 2, shootAngle: 10, aim: 0.8, coolDown: 1000, coolDownOffset: 1000),
                    new Shoot(25, index: 6, shoots: 3, shootAngle: 10, aim: 1, coolDown: 1000, coolDownOffset: 1000),
                    new Taunt(1, 6000, "Puny mortals! My {HP} HP will annihilate you!"),
                    new Reproduce("Henchman of Oryx", 8, 3, 1, 10000),
                    new HpLessTransition(.3, "prepareRage")
                    ),
                new State("prepareRage",
                    new Chase(3, 15, 3),
                    new Taunt("Can't... keep... henchmen... alive... anymore! ARGHHH!!!"),
                    new AddCond(ConditionEffectIndex.Invulnerable), // ok
                    new Shoot(25, 30, shootAngle: 0, index: 7, coolDown: 4000, coolDownOffset: 4000),
                    new Shoot(25, 30, shootAngle: 30, index: 8, coolDown: 4000, coolDownOffset: 4000),
                    new TimedTransition(10000, "beforeRage")
                    ),
                new State("beforeRage",
                    new Chase(15, 3),
                    new Shoot(25, 30, index: 7, coolDown: 3000, coolDownOffset: 1000),
                    new Shoot(25, 30, index: 8, coolDown: 3000, coolDownOffset: 1500),
                    new Shoot(25, index: 0, shoots: 8, shootAngle: 45, coolDown: 1000, coolDownOffset: 1500),
                    new Shoot(25, index: 1, shoots: 3, shootAngle: 10, coolDown: 1000, coolDownOffset: 1000),
                    new Shoot(25, index: 2, shoots: 3, shootAngle: 10, aim: 0.2, coolDown: 1000, coolDownOffset: 1000),
                    new Shoot(25, index: 3, shoots: 2, shootAngle: 10, aim: 0.4, coolDown: 1000, coolDownOffset: 1000),
                    new Shoot(25, index: 4, shoots: 3, shootAngle: 10, aim: 0.6, coolDown: 1000, coolDownOffset: 1000),
                    new Shoot(25, index: 5, shoots: 2, shootAngle: 10, aim: 0.8, coolDown: 1000, coolDownOffset: 1000),
                    new Shoot(25, index: 6, shoots: 3, shootAngle: 10, aim: 1, coolDown: 1000, coolDownOffset: 1000),
                    new Shoot(25, 30, shootAngle: 0, index: 7, coolDown: 4000, coolDownOffset: 4000),
                    new Shoot(25, 30, shootAngle: 30, index: 8, coolDown: 4000, coolDownOffset: 4000),
                    new TimedTransition(10000, "rage")
                    ),
                new State("rage",
                    new RemCond(ConditionEffectIndex.Invulnerable), // ok
                    new State("rageTaunt",
                        new Taunt(1, 6000, "Puny mortals! My {HP} HP will annihilate you!"),
                        new PlayerWithinTransition(5, "rageToss")
                        ),
                    new State("rageToss",
                        new StayAbove(4, 20),
                        new Chase(8, 15, 3),
                        new Shoot(25, 30, index: 7, coolDown: 90000001, coolDownOffset: 8000),
                        new Shoot(25, 30, index: 8, coolDown: 90000001, coolDownOffset: 8500),
                        new Shoot(25, index: 0, shoots: 8, shootAngle: 45, coolDown: 1000, coolDownOffset: 1500),
                        new Shoot(25, index: 1, shoots: 3, shootAngle: 10, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, index: 2, shoots: 3, shootAngle: 10, aim: 0.2, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, index: 3, shoots: 2, shootAngle: 10, aim: 0.4, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, index: 4, shoots: 3, shootAngle: 10, aim: 0.6, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, index: 5, shoots: 2, shootAngle: 10, aim: 0.8, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, index: 6, shoots: 3, shootAngle: 10, aim: 1, coolDown: 1000, coolDownOffset: 1000),
                        new TossObject("Monstrosity Scarab", 7, 0, coolDown: 1500),
                        new TossObject("Monstrosity Scarab", 7, 60, coolDown: 1500),
                        new TossObject("Monstrosity Scarab", 7, 120, coolDown: 1500),
                        new TossObject("Monstrosity Scarab", 7, 180, coolDown: 1500),
                        new TossObject("Monstrosity Scarab", 7, 240, coolDown: 1500),
                        new TossObject("Monstrosity Scarab", 7, 300, coolDown: 1500),
                        new TimedTransition(10000, "rageTaunt")
                        )
                    )
                ),
            new Drops(
                new OnlyOne(
                    new CyanBag(ItemType.Weapon, 10),
                    new CyanBag(ItemType.Weapon, 11),
                    new CyanBag(ItemType.Weapon, 12),
                    new CyanBag(ItemType.Armor, 11),
                    new CyanBag(ItemType.Armor, 12),
                    new CyanBag(ItemType.Armor, 13),
                    new CyanBag(ItemType.Ability, 5),
                    new CyanBag(ItemType.Ability, 6),
                    new CyanBag(ItemType.Ring, 5)
                    ),
                new OnlyOne(
                    new BlueBag(new[] { Potions.POTION_OF_DEFENSE, Potions.POTION_OF_ATTACK, Potions.POTION_OF_WISDOM, Potions.POTION_OF_VITALITY }, new bool[] { false, false, false, false })
                    ),
                new WhiteBag(new[] { "Sword of the Mad God", "Onyx Shield of the Mad God", "Almandine Armor of Anger", "Almandine Ring of Wrath" })
                )
            )
        ;
    }
}