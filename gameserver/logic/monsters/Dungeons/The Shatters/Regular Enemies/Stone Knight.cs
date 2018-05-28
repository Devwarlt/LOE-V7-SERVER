using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.engine;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic.monsters.Dungeons.The_Shatters.Regular_Enemies
{
    public class StoneKnight : IBehavior
    {
        public State Behavior() =>
            new State(
                new State("Follow",
                    new Chase(0.6, 10, 5),
                    new PlayerWithinTransition(5, "Charge")
                    ),
                new State("Charge",
                    new TimedTransition(2000, "Follow"),
                    new Charge(4, 5),
                    new Shoot(5, 6, index: 0, coolDown: 3000)
                    )
                );
    }
}
