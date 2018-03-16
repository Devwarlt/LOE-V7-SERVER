using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.engine;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic.monsters.Dungeons.The_Shatters.Regular_Enemies
{
	public class Inferno : IBehavior
	{
		public State Behavior() =>
			new State("Start",
				new State(
					new Circle(0.5, 4, 15, "shtrs Blizzard"),
							new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 15, coolDown: 4333),
							new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 30, coolDown: 3500),
							new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 60, coolDown: 7250),
							new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 90, coolDown: 10000)
				)
			)
			;
	}
}