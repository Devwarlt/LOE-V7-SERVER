using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.engine;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic.monsters.Dungeons.The_Shatters.Regular_Enemies
{
    public class FireAdept : IBehavior
    {
        public State Behavior() =>
            new State(
                new State("Main",
                    new TimedTransition(5000, "Throw"),
                    new Chase(0.8, range: 1),
                    new Shoot(10, 1, index: 0, coolDown: 100, aim: 1),
                    new Shoot(10, 3, index: 1, shootAngle: 10, coolDown: 4000, aim: 1)
                    ),
                new State("Throw",
                    new TossObject("shtrs Fire Portal", 5, coolDown: 8000, coolDownOffset: 7000, randomToss: true),
                    new TimedTransition(1000, "Main")
                    )
                );
    }
}
