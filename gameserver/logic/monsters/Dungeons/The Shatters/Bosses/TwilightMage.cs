using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.engine;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic.monsters.Dungeons.The_Shatters.Regular_Enemies
{
	public class TwilightMage : IBehavior
	{
		public State Behavior() =>
			  new State(
				  new State("FrogFTW",
					new SetLootState("archmage"),
					new CopyLootState("shtrs encounterchestspawner", 20),
					new HpLessTransition(.1, "Death"),
					new State("Idle",

						new AddCond(ConditionEffectIndex.Invulnerable),
						new EntityNotExistsTransition2("shtrs Glassier Archmage", "shtrs Archmage of Flame", 15, "Wake")
					),
					new State("Wake",

						new State("Comment1",

						new AddCond(ConditionEffectIndex.Invulnerable),
							new SetAltTexture(1),
							new Taunt("Ha...ha........hahahahahaha! You will make a fine sacrifice!"),
							new TimedTransition(3000, "Comment2")
						),
						new SetAltTexture(1),
						new State("Comment2",

						new AddCond(ConditionEffectIndex.Invulnerable),
							new Taunt("You will find that it was...unwise...to wake me."),
							new TimedTransition(1000, "Comment3")
						),
						new State("Comment3",

						new AddCond(ConditionEffectIndex.Invulnerable),
							new SetAltTexture(1),
							new Taunt("Let us see what can conjure up!"),
							new TimedTransition(1000, "Comment4")
						),
						new State("Comment4",

						new AddCond(ConditionEffectIndex.Invulnerable),
							new SetAltTexture(1),
							new Taunt("I will freeze the life from you!"),
							new TimedTransition(1000, "Shoot")
						)
					),
					new State("TossShit",

						new TossObject("shtrs Ice Portal", 10, coolDown: 25000, randomToss: false),
						new TossObject("shtrs FireBomb", 15, coolDown: 25000, randomToss: true),
						new TossObject("shtrs FireBomb", 15, coolDown: 25000, randomToss: true),
						new TossObject("shtrs FireBomb", 7, coolDown: 25000, randomToss: true),
						new TossObject("shtrs FireBomb", 1, coolDown: 25000, randomToss: true),
						new TossObject("shtrs FireBomb", 4, coolDown: 25000, randomToss: true),
						new TossObject("shtrs FireBomb", 8, coolDown: 25000, randomToss: true),
						new TossObject("shtrs FireBomb", 9, coolDown: 25000, randomToss: true),        //NOT IN USE!
						new TossObject("shtrs FireBomb", 5, coolDown: 25000, randomToss: true),
						new TossObject("shtrs FireBomb", 7, coolDown: 25000, randomToss: true),
						new TossObject("shtrs FireBomb", 11, coolDown: 25000, randomToss: true),
						new TossObject("shtrs FireBomb", 13, coolDown: 25000, randomToss: true),
						new TossObject("shtrs FireBomb", 12, coolDown: 25000, randomToss: true),
						new TossObject("shtrs FireBomb", 10, coolDown: 25000, randomToss: true),
						new Spawn("shtrs Ice Shield 2", maxChildren: 1, initialSpawn: 1, coolDown: 25000),
						new TimedTransition(1, "Shoot")
						),
				  new State("Shoot",

					new Shoot(15, 5, 5, index: 1, coolDown: 800),
					new Shoot(15, 5, 5, index: 1, coolDown: 800, coolDownOffset: 200),
					new Shoot(15, 5, 5, index: 1, coolDown: 800, coolDownOffset: 400),
					new Shoot(15, 5, 5, index: 1, coolDown: 800, coolDownOffset: 600),
					new Shoot(15, 5, 5, index: 1, coolDown: 800, coolDownOffset: 800),
					new TimedTransition(800, "Shoot"),
					new HpLessTransition(0.50, "Pre Birds")
						),
					new State("Pre Birds",

						new AddCond(ConditionEffectIndex.Invulnerable),
						new Taunt("You leave me no choice...Inferno! Blizzard!"),
						new TimedTransition(2000, "Birds")
						),
					new State("Birds",

						new AddCond(ConditionEffectIndex.Invulnerable),
						new Spawn("shtrs Inferno", maxChildren: 1, initialSpawn: 1, coolDown: 1000000000),
						new Spawn("shtrs Blizzard", maxChildren: 1, initialSpawn: 1, coolDown: 1000000000),
						new EntitiesNotExistsTransition(500, "PreNewShit2", "shtrs Inferno", "shtrs Blizzard")
						),
					new State("PreNewShit2",

						new AddCond(ConditionEffectIndex.Invulnerable),
						new TimedTransition(3000, "NewShit2")
						),
					new State("NewShit2",

						new AddCond(ConditionEffectIndex.Invulnerable),
						new MoveTo(0, -6, 1),
						new TimedTransition(3000, "Active2")
						),
					new State("Active2",

						new Taunt("THE POWER...IT CONSUMES ME!"),
						new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 100),
						new Shoot(15, 20, index: 3, coolDown: 100000000, coolDownOffset: 200),
						new Shoot(15, 20, index: 4, coolDown: 100000000, coolDownOffset: 300),
						new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 400),
						new Shoot(15, 20, index: 5, coolDown: 100000000, coolDownOffset: 500),
						new Shoot(15, 20, index: 6, coolDown: 100000000, coolDownOffset: 600),
						new TimedTransition(2000, "NewShit3")
						),
					new State("NewShit3",

						new AddCond(ConditionEffectIndex.Invulnerable),
						new MoveTo(4, 0, 1),
						new TimedTransition(3000, "Active3")
						),
					new State("Active3",

						new Taunt("THE POWER...IT CONSUMES ME!"),
						new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 100),
						new Shoot(15, 20, index: 3, coolDown: 100000000, coolDownOffset: 200),
						new Shoot(15, 20, index: 4, coolDown: 100000000, coolDownOffset: 300),
						new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 400),
						new Shoot(15, 20, index: 5, coolDown: 100000000, coolDownOffset: 500),
						new Shoot(15, 20, index: 6, coolDown: 100000000, coolDownOffset: 600),
						new TimedTransition(2000, "NewShit4")
						),
					new State("NewShit4",

						new AddCond(ConditionEffectIndex.Invulnerable),
						new MoveTo(0, 13, 1),
						new TimedTransition(3000, "Active4")
							),
					new State("Active4",

						new Taunt("THE POWER...IT CONSUMES ME!"),
						new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 100),
						new Shoot(15, 20, index: 3, coolDown: 100000000, coolDownOffset: 200),
						new Shoot(15, 20, index: 4, coolDown: 100000000, coolDownOffset: 300),
						new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 400),
						new Shoot(15, 20, index: 5, coolDown: 100000000, coolDownOffset: 500),
						new Shoot(15, 20, index: 6, coolDown: 100000000, coolDownOffset: 600),
						new TimedTransition(2000, "NewShit5")
						),
					new State("NewShit5",

						new AddCond(ConditionEffectIndex.Invulnerable),
						new MoveTo(-4, 0, 1),
						new TimedTransition(3000, "Active5")
							),
					new State("Active5",

						new Taunt("THE POWER...IT CONSUMES ME!"),
						new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 100),
						new Shoot(15, 20, index: 3, coolDown: 100000000, coolDownOffset: 200),
						new Shoot(15, 20, index: 4, coolDown: 100000000, coolDownOffset: 300),
						new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 400),
						new Shoot(15, 20, index: 5, coolDown: 100000000, coolDownOffset: 500),
						new Shoot(15, 20, index: 6, coolDown: 100000000, coolDownOffset: 600),
						new TimedTransition(2000, "NewShit6")
						),
					new State("NewShit6",

						new AddCond(ConditionEffectIndex.Invulnerable),
						new MoveTo(-4, 0, 1),
						new TimedTransition(3000, "Active6")
							),
					new State("Active6",

						new Taunt("THE POWER...IT CONSUMES ME!"),
						new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 100),
						new Shoot(15, 20, index: 3, coolDown: 100000000, coolDownOffset: 200),
						new Shoot(15, 20, index: 4, coolDown: 100000000, coolDownOffset: 300),
						new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 400),
						new Shoot(15, 20, index: 5, coolDown: 100000000, coolDownOffset: 500),
						new Shoot(15, 20, index: 6, coolDown: 100000000, coolDownOffset: 600),
						new TimedTransition(2000, "NewShit7")
						),
					new State("NewShit7",

						new AddCond(ConditionEffectIndex.Invulnerable),
						new MoveTo(0, -13, 1),
						new TimedTransition(3000, "Active7")
							),
					new State("Active7",

						new Taunt("THE POWER...IT CONSUMES ME!"),
						new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 100),
						new Shoot(15, 20, index: 3, coolDown: 100000000, coolDownOffset: 200),
						new Shoot(15, 20, index: 4, coolDown: 100000000, coolDownOffset: 300),
						new Shoot(15, 20, index: 2, coolDown: 100000000, coolDownOffset: 400),
						new Shoot(15, 20, index: 5, coolDown: 100000000, coolDownOffset: 500),
						new Shoot(15, 20, index: 6, coolDown: 100000000, coolDownOffset: 600),
						new TimedTransition(2000, "Death")
						),
						new State("Death",

							new AddCond(ConditionEffectIndex.Invulnerable),
							new Taunt("IM..POSSI...BLE!"),
							new CopyDamageOnDeath("shtrs Loot Balloon Mage"),
							new EntityOrder(1, "shtrs Chest Spawner 2", "Open"),
							new TimedTransition(2000, "Suicide")
							),
						new State("Suicide",

							new Shoot(35, index: 0, shoots: 30),
							new Suicide()
					)
				)
			)
			;
	}
}