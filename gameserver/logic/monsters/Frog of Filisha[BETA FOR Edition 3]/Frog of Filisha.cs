using gameserver.logic.behaviors;
using gameserver.logic.transitions;
using gameserver.logic.loot;
using System;

namespace gameserver.logic
{
	partial class BehaviorDb
	{
		private _ FrogOfFilisha = () => Behav()
		 .Init("Frog Minion",
		  new State(
			  new Wander(2),
			  new AddCond(ConditionEffectIndex.Armored),
			  new State("Shooting1",
				  new Shoot(30, index: 0, shoots: 5, coolDown: 3000, coolDownOffset: 0, shootAngle: 72),
				  new Shoot(30, index: 1, shoots: 5, coolDown: 3000, coolDownOffset: 1500, shootAngle: 72)
				  ),
			  new Wander(2),
			  new State("Shooting2",
				  new Shoot(30, index: 1, shoots: 8, coolDown: 2000, coolDownOffset: 0, angleOffset: 0, shootAngle: 45),
				  new Shoot(30, index: 1, shoots: 8, coolDown: 2000, coolDownOffset: 1000, angleOffset: 22.5, shootAngle: 45)
				  ),
			
			  new State("Shooting3",
					new Wander(2),
				  new Shoot(30, index: 0, shoots: 1, coolDown: 1000)
				  )
			  ),
		  new ItemLoot("Frog King Skin", 0.0001)
			)
	   .Init("Frog King",
			new State(
				new State("Idle",
					new StayCloseToSpawn(0.1, 6),
					new Wander(1),
					new HpLessTransition(0.99999, "Uh oh")
					),
				new State("Uh oh",
					new Flashing(0x00ff00, 0.1, 50),
					new Taunt("Begone, thot!"),
					new AddCond(ConditionEffectIndex.Invulnerable),
					new TimedTransition(5000, "Go away")
					),
				new State("Go away",
					new RemCond(ConditionEffectIndex.Invulnerable),
					new Shoot(10, index: 0, shoots: 15, shootAngle: 24, coolDown: 1500),
					new HpLessTransition(0.80, "Reee")
					),
				new State("Reee",
					new Taunt("REEEEEE"),
					new Prioritize(
							new Chase(1.5, range: 7),
							new Wander(5)
							),
					new Shoot(20, index: 3, shoots: 15, shootAngle: 24, coolDown: 1000),
					new Shoot(15, index: 1, shoots: 3, shootAngle: 15, coolDown: 250),
					new HpLessTransition(0.65, "Kys1")
					),
				new State("Kys1",
					new Flashing(0x00ff00, 1, 2),
					new Taunt("You should kill yourself."),
					new AddCond(ConditionEffectIndex.Armored),
					new TimedTransition(500, "Pause"),
					new HpLessTransition(0.30, "Rage")
					),
				new State("Kys2",
					new Flashing(0x00ff00, 1, 2),
					new Shoot(20, index: 3, shoots: 15, shootAngle: 24, coolDown: 100),
					new Shoot(15, index: 1, shoots: 3, shootAngle: 15, coolDown: 500),
					new TimedTransition(2000, "Pause"),
					new HpLessTransition(0.30, "Rage")
					),
				new State("Pause",
					new AddCond(ConditionEffectIndex.Armored),
					new Shoot(15, index: 2, shoots: 40, shootAngle: 9, coolDown: 1200),
					new TimedTransition(1000, "Kys2"),
					new HpLessTransition(0.30, "Rage")
					),
				new State("Rage",
					new Flashing(0xff0000, 5, 999999),
					new Taunt("DIE!"),
					new Prioritize(
							new Chase(1.5, range: 7),
							new Wander(5)
							),
				  new TossObject("Frog Minion", range: 8.89949, angle: 45, coolDown: 10000),
				  new TossObject("Frog Minion", range: 8.89949, angle: 135, coolDown: 10000),
				  new TossObject("Frog Minion", range: 8.89949, angle: 225, coolDown: 10000),
				  new TossObject("Frog Minion", range: 8.89949, angle: 315, coolDown: 10000),
					new Shoot(10, index: 0, shoots: 10, shootAngle: 36, coolDown: 1000),
					new Shoot(20, index: 3, shoots: 3, shootAngle: 24, coolDown: 1000),
					new Shoot(15, index: 1, shoots: 3, shootAngle: 15, coolDown: 250),
					new HpLessTransition(0.005, "Die")
					),
				new State("Die",
					new Taunt("Feels bad, man..."),
					new Flashing(0x0000ff, 0.3, 9999999)
					)
				),
					new ItemLoot("Potion of Speed", 1),
					new ItemLoot("Potion of Defense", 0.5),
					new ItemLoot("Frog King Skin", 0.001),
					new ItemLoot("Prism of Chromatic Light", 0.001),
					new ItemLoot("Rainbow Sword", 0.001),
					new ItemLoot("Edge of Time", 0.001),
					new ItemLoot("Robe of Elemental Mastery", 0.001),
					new ItemLoot("Orb of Time", 0.001),
					new ItemLoot("Rain of Elemental Energy", 0.001),

					new TierLoot(10, ItemType.Weapon, 0.2),
					new TierLoot(11, ItemType.Weapon, 0.1),
					new TierLoot(12, ItemType.Weapon, 0.08),

					new TierLoot(11, ItemType.Armor, 0.2),
					new TierLoot(12, ItemType.Armor, 0.1),
					new TierLoot(13, ItemType.Armor, 0.08),

					new TierLoot(5, ItemType.Ability, 0.2),
					new TierLoot(6, ItemType.Ability, 0.1),

					new TierLoot(5, ItemType.Ring, 0.15),
					new TierLoot(6, ItemType.Ring, 0.08));
	
		}
}