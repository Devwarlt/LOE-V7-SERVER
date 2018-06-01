using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.engine;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic.monsters.Dungeons.The_Shatters.Regular_Enemies
{
	public class BridgeSentinel : IBehavior
	{
		public State Behavior() =>
			  new State(
				new State("Start",
					new Shoot(2, index: 5, shoots: 3, angleOffset: 0, coolDown: 10),
					new Shoot(2, index: 5, shoots: 3, angleOffset: 45, coolDown: 10),
					new Shoot(2, index: 5, shoots: 3, angleOffset: 90, coolDown: 10),
					new Shoot(2, index: 5, shoots: 3, angleOffset: 135, coolDown: 10),
					new Shoot(2, index: 5, shoots: 3, angleOffset: 180, coolDown: 10),
					new HpLessTransition(0.1, "Death"),
					new State("Idle",
						new AddCond(ConditionEffectIndex.Invulnerable),
						new PlayerWithinTransition(15, "Close Bridge")
						),
					new State("Close Bridge",
						new AddCond(ConditionEffectIndex.Invulnerable),
						new EntityOrder(46, "shtrs Bridge Closer", "Closer"),
						new TimedTransition(5000, "Close Bridge2")
						),
					new State("Close Bridge2",

						new AddCond(ConditionEffectIndex.Invulnerable),
						new EntityOrder(46, "shtrs Bridge Closer2", "Closer"),
						new TimedTransition(5000, "Close Bridge3")
						),
					new State("Close Bridge3",

						new AddCond(ConditionEffectIndex.Invulnerable),
						new EntityOrder(46, "shtrs Bridge Closer3", "Closer"),
						new TimedTransition(5000, "Close Bridge4")
						),
					new State("Close Bridge4",

						new AddCond(ConditionEffectIndex.Invulnerable),
						new EntityOrder(46, "shtrs Bridge Closer4", "Closer"),
						new TimedTransition(6000, "BEGIN")
						),
					new State("BEGIN",

						new AddCond(ConditionEffectIndex.Invulnerable),
						new EntitiesNotExistsTransition(30, "Wake", "shtrs Bridge Obelisk A", "shtrs Bridge Obelisk B", "shtrs Bridge Obelisk D", "shtrs Bridge Obelisk E")
					),
						new State("Wake",

						new AddCond(ConditionEffectIndex.Invulnerable),
						new Taunt("Who has woken me...? Leave this place."),
						new Timed(2100, new Shoot(15, 15, 12, index: 0, angleOffset: 180, coolDown: 700, coolDownOffset: 3000)),
						new TimedTransition(8000, "Swirl Shot")
						),
						new State("Swirl Shot",

							new Taunt("Go."),
							new TimedTransition(10000, "Blobomb"),
							new State("Swirl1",

							new Shoot(50, index: 0, shoots: 1, shootAngle: 102, angleOffset: 102, coolDown: 6000),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 114, angleOffset: 114, coolDown: 6000, coolDownOffset: 200),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 126, angleOffset: 126, coolDown: 6000, coolDownOffset: 400),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 138, angleOffset: 138, coolDown: 6000, coolDownOffset: 600),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 150, angleOffset: 150, coolDown: 6000, coolDownOffset: 800),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 162, angleOffset: 162, coolDown: 6000, coolDownOffset: 1000),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 174, angleOffset: 174, coolDown: 6000, coolDownOffset: 1200),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 186, angleOffset: 186, coolDown: 6000, coolDownOffset: 1400),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 198, angleOffset: 198, coolDown: 6000, coolDownOffset: 1600),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 210, angleOffset: 210, coolDown: 6000, coolDownOffset: 1800),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 222, angleOffset: 222, coolDown: 6000, coolDownOffset: 2000),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 234, angleOffset: 234, coolDown: 6000, coolDownOffset: 2200),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 246, angleOffset: 246, coolDown: 6000, coolDownOffset: 2400),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 258, angleOffset: 258, coolDown: 6000, coolDownOffset: 2600),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 270, angleOffset: 270, coolDown: 6000, coolDownOffset: 2800),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 282, angleOffset: 282, coolDown: 6000, coolDownOffset: 3000),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 270, angleOffset: 270, coolDown: 6000, coolDownOffset: 3200),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 258, angleOffset: 258, coolDown: 6000, coolDownOffset: 3400),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 246, angleOffset: 246, coolDown: 6000, coolDownOffset: 3600),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 234, angleOffset: 234, coolDown: 6000, coolDownOffset: 3800),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 222, angleOffset: 222, coolDown: 6000, coolDownOffset: 4000),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 210, angleOffset: 210, coolDown: 6000, coolDownOffset: 4200),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 198, angleOffset: 198, coolDown: 6000, coolDownOffset: 4400),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 186, angleOffset: 186, coolDown: 6000, coolDownOffset: 4600),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 174, angleOffset: 174, coolDown: 6000, coolDownOffset: 4800),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 162, angleOffset: 162, coolDown: 6000, coolDownOffset: 5000),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 150, angleOffset: 150, coolDown: 6000, coolDownOffset: 5200),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 138, angleOffset: 138, coolDown: 6000, coolDownOffset: 5400),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 126, angleOffset: 126, coolDown: 6000, coolDownOffset: 5600),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 114, angleOffset: 114, coolDown: 6000, coolDownOffset: 5800),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 102, angleOffset: 102, coolDown: 6000, coolDownOffset: 6000),
							new TimedTransition(6000, "Swirl1")
							)
							),
							new State("Blobomb",

							new Taunt("You live still? DO NOT TEMPT FATE!"),
							new Taunt("CONSUME!"),
							new EntityOrder(20, "shtrs blobomb maker", "Spawn"),
							new EntityNotExistsTransition("shtrs Blobomb", 30, "SwirlAndShoot")
								),
								new State("SwirlAndShoot",

									new TimedTransition(10000, "Blobomb"),
									new Taunt("FOOLS! YOU DO NOT UNDERSTAND!"),
									new ChangeSize(20, 130),
							new Shoot(15, 15, 11, index: 0, angleOffset: 180, coolDown: 700, coolDownOffset: 700),
									new State("Swirl1_2",

							new Shoot(50, index: 0, shoots: 1, shootAngle: 102, angleOffset: 102, coolDown: 6000),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 114, angleOffset: 114, coolDown: 6000, coolDownOffset: 200),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 126, angleOffset: 126, coolDown: 6000, coolDownOffset: 400),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 138, angleOffset: 138, coolDown: 6000, coolDownOffset: 600),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 150, angleOffset: 150, coolDown: 6000, coolDownOffset: 800),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 162, angleOffset: 162, coolDown: 6000, coolDownOffset: 1000),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 174, angleOffset: 174, coolDown: 6000, coolDownOffset: 1200),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 186, angleOffset: 186, coolDown: 6000, coolDownOffset: 1400),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 198, angleOffset: 198, coolDown: 6000, coolDownOffset: 1600),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 210, angleOffset: 210, coolDown: 6000, coolDownOffset: 1800),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 222, angleOffset: 222, coolDown: 6000, coolDownOffset: 2000),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 234, angleOffset: 234, coolDown: 6000, coolDownOffset: 2200),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 246, angleOffset: 246, coolDown: 6000, coolDownOffset: 2400),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 258, angleOffset: 258, coolDown: 6000, coolDownOffset: 2600),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 270, angleOffset: 270, coolDown: 6000, coolDownOffset: 2800),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 282, angleOffset: 282, coolDown: 6000, coolDownOffset: 3000),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 270, angleOffset: 270, coolDown: 6000, coolDownOffset: 3200),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 258, angleOffset: 258, coolDown: 6000, coolDownOffset: 3400),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 246, angleOffset: 246, coolDown: 6000, coolDownOffset: 3600),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 234, angleOffset: 234, coolDown: 6000, coolDownOffset: 3800),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 222, angleOffset: 222, coolDown: 6000, coolDownOffset: 4000),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 210, angleOffset: 210, coolDown: 6000, coolDownOffset: 4200),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 198, angleOffset: 198, coolDown: 6000, coolDownOffset: 4400),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 186, angleOffset: 186, coolDown: 6000, coolDownOffset: 4600),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 174, angleOffset: 174, coolDown: 6000, coolDownOffset: 4800),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 162, angleOffset: 162, coolDown: 6000, coolDownOffset: 5000),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 150, angleOffset: 150, coolDown: 6000, coolDownOffset: 5200),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 138, angleOffset: 138, coolDown: 6000, coolDownOffset: 5400),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 126, angleOffset: 126, coolDown: 6000, coolDownOffset: 5600),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 114, angleOffset: 114, coolDown: 6000, coolDownOffset: 5800),
							new Shoot(50, index: 0, shoots: 1, shootAngle: 102, angleOffset: 102, coolDown: 6000, coolDownOffset: 6000),
							new TimedTransition(6000, "Swirl1_2")
							)
								),
						new State("Death",

							new AddCond(ConditionEffectIndex.Invulnerable),
							new CopyDamageOnDeath("shtrs Loot Balloon Bridge"),
							new Taunt("I tried to protect you... I have failed. You release a great evil upon this realm...."),
							new TimedTransition(2000, "Suicide")
							),
						new State("Suicide",

							new Shoot(35, index: 0, shoots: 30),
							new EntityOrder(1, "shtrs Chest Spawner 1", "Open"),
							new EntityOrder(46, "shtrs Spawn Bridge", "Open"),
							new Suicide()
					)
				)
			)
			;
	}
}