﻿using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ TheShattersTheForgottenKing = () => Behav()
            .Init("shtrs The Cursed Crown",
                new State(
                    new State("Main",
                        new TimedTransition(targetState: "1st", coolDown: 3000)
                    ),
                    new State("1st",
                        new MoveTo(X: 0, Y: -8, speed: 5, once: true, isMapPosition: false, instant: false)
                    )
                )
            )
        ;
    }
}