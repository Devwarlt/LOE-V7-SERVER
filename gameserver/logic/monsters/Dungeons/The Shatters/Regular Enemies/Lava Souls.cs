using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.engine;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic.monsters.Dungeons.The_Shatters.Regular_Enemies
{
    public class LavaSouls : IBehavior
    {
        public State Behavior() =>
            new State(
                new State("active",
                    new Chase(.7, range: 0),
                    new PlayerWithinTransition(2, "blink")
                    ),
                new State("blink",
                    new Flashing(0xfFF0000, flashRepeats: 10000, flashPeriod: 0.1),
                    new TimedTransition(2000, "explode")
                    ),
                new State("explode",
                    new Flashing(0xfFF0000, flashRepeats: 5, flashPeriod: 0.1),
                    new Shoot(5, 9),
                    new Suicide()
                    )
                );
    }
}
