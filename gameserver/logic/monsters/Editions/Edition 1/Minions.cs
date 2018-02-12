﻿using gameserver.logic;
using gameserver.logic.behaviors;
using gameserver.logic.transitions;
using gameserver.logic.loot;

namespace gameserver.logic

{
	partial class BehaviorDb
	{
		private _ Edition1Minions = () => Behav()
			.Init("Fiery Succubus",
				new State(
			  new State("Shoot1",
						new Shoot(30, shoots: 1, shootAngle: 10, index: 0, aim: 0.3, coolDown: 400),
						new Shoot(30, shoots: 3, shootAngle: 20, index: 1, aim: 0.2, coolDown: 800),
						new HpLessTransition(0.67, "Shoot2")
						),
			  new State("Shoot2",
						new Shoot(30, shoots: 8, shootAngle: 45, index: 2, coolDown: 1000),
						new Shoot(30, shoots: 1, shootAngle: 10, index: 0, aim: 0.3, coolDown: 400),
						new HpLessTransition(0.34, "Shoot3")
						),
			  new State("Shoot3",
						new Shoot(30, shoots: 5, shootAngle: 65, index: 1, aim: 0.1, coolDown: 700),
						new Shoot(30, shoots: 3, shootAngle: 20, index: 2, aim: 0.2, coolDown: 800),
						new Shoot(20, shoots: 1, shootAngle: 10, index: 0, aim: 0.3, coolDown: 400),
						new HpLessTransition(0.12, "Suicide")
						),
			  new State("Suicide",
										new Shoot(0, shoots: 10, index: 2, shootAngle: 36, direction: 0),
										new Suicide()
					  )
			  )
		  )

			.Init("Fiery Twin Succubus",
				  new State(
			  new State("Shooting1",
		  new AddCond(ConditionEffectIndex.Armored),
				  new Shoot(30, index: 0, shoots: 5, coolDown: 3000, coolDownOffset: 0, shootAngle: 72),
				  new Shoot(30, index: 1, shoots: 5, coolDown: 3000, coolDownOffset: 800, shootAngle: 72),
		  new HpLessTransition(0.7, "Shooting2")
				  ),
			  new State("Shooting2",
				  new Wander(2),
				  new Shoot(30, index: 1, shoots: 8, coolDown: 2000, coolDownOffset: 0, angleOffset: 0, shootAngle: 45),
				  new Shoot(30, index: 1, shoots: 8, coolDown: 2000, coolDownOffset: 1000, angleOffset: 22.5, shootAngle: 45),
		  new HpLessTransition(0.5, "Shooting3")
				  ),
			  new State("Shooting3",
				  new Wander(2),
				  new Shoot(30, index: 0, shoots: 1, coolDown: 1000),
		  new Shoot(30, index: 1, shoots: 8, shootAngle: 45, coolDown: 1300),
		  new HpLessTransition(0.32, "Shooting4")
				  ),
			  new State("Shooting4",
				  new Wander(2),
				  new Shoot(30, index: 1, shoots: 8, angleOffset: 0, shootAngle: 45),
				  new Shoot(30, index: 0, shoots: 8, angleOffset: 22.5, shootAngle: 45),
		  new HpLessTransition(0.15, "SpawnMinion")
		  ),
		new State("SpawnMinion",
		  new AddCond(ConditionEffectIndex.Invincible),
		  new Spawn("Fiery Succubus", maxChildren: 6, coolDown: 10000),
		  new TimedTransition(5000, "Suicide")
		  ),
		new State("Suicide",
			new Shoot(0, shoots: 10, index: 4, shootAngle: 36, direction: 0),
			new Suicide()
				  )
			  )
		  )

			.Init("Icy Succubus",
				new State()
			)

			.Init("Icy Twin Succubus",
				new State()
			)
			;
	}
}
