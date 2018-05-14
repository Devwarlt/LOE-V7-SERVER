using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.engine;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic.monsters.Dungeons.The_Shatters.Regular_Enemies
{
    public class GlassierArchmage : IBehavior
    {
        public State Behavior() =>
            new State(
                new Retreat(0.5, 5),
                new State("1st",
                    new Chase(0.8, range: 7),
                    new Shoot(20, index: 2, shoots: 1, coolDown: 50),
                    new TimedTransition(5000, "next")
                    ),
                new State("next",
                    new Shoot(35, index: 0, shoots: 25, coolDown: 5000),
                    new TimedTransition(25, "1st")
                    )
                );
    }
}
