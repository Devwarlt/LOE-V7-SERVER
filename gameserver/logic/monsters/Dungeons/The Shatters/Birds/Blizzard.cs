using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.engine;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic.monsters.Dungeons.The_Shatters.Regular_Enemies
{
	public class Blizzard : IBehavior
	{
		public State Behavior() =>
			new State("Start",
				new State(
					new State("Follow",
					new Chase(0.3, range: 1, coolDown: 1000),
							new Shoot(0, index: 0, shoots: 4, shootAngle: 90, angleOffset: 45, coolDown: 25),
							new TimedTransition(7000, "Spin")
					),
					new State("Spin",
						new State("Quadforce1",
							new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 0, coolDown: 300),
							new TimedTransition(10, "Quadforce2")
						),
						new State("Quadforce2",
							new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 15, coolDown: 300),
							new TimedTransition(10, "Quadforce3")
						),
						new State("Quadforce3",
							new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 30, coolDown: 300),
							new TimedTransition(10, "Quadforce4")
						),
						new State("Quadforce4",
							new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 45, coolDown: 300),
							new TimedTransition(10, "Quadforce5")
						),
						new State("Quadforce5",
							new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 60, coolDown: 300),
							new TimedTransition(10, "Quadforce6")
						),
						new State("Quadforce6",
							new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 75, coolDown: 300),
							new TimedTransition(10, "Quadforce7")
						),
						new State("Quadforce7",
							new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 90, coolDown: 300),
							new TimedTransition(10, "Quadforce8")
						),
						new State("Quadforce8",
							new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 105, coolDown: 300),
							new TimedTransition(10, "Quadforce9")
						),
						new State("Quadforce9",
							new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 120, coolDown: 300),
							new TimedTransition(10, "Quadforce10")
						),
						new State("Quadforce10",
							new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 135, coolDown: 300),
							new TimedTransition(10, "Quadforce11")
						),
						new State("Quadforce11",
							new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 150, coolDown: 300),
							new TimedTransition(10, "Quadforce12")
						),
						new State("Quadforce12",
							new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 165, coolDown: 300),
							new TimedTransition(10, "Quadforce13")
						),
						new State("Quadforce13",
							new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 180, coolDown: 300),
							new TimedTransition(10, "Quadforce14")
						),
						new State("Quadforce14",
							new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 195, coolDown: 300),
							new TimedTransition(10, "Quadforce15")
						),
						new State("Quadforce15",
							new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 210, coolDown: 300),
							new TimedTransition(10, "Quadforce16")
						),
						new State("Quadforce16",
							new Shoot(0, index: 0, shoots: 6, shootAngle: 60, angleOffset: 225, coolDown: 300),
							new TimedTransition(10, "Follow")

							))
				)
			)
			;
	}
}