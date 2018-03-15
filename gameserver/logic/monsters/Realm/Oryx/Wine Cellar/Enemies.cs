using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ OryxWineCellarEnemies = () => Behav()
        .Init("Henchman of Oryx",
            new State(
                new Prioritize(
                    new Protect(12, "Oryx the Mad God 2", 15, 12, 2),
                    new Circle(4, 2, target: "Oryx the Mad God 2", radiusVariance: 1),
                    new Chase(3, 8, 3, coolDown: 0)
                    ),
                new Reproduce("Aberrant of Oryx", 8, 1, 1, 10000),
                new Reproduce("Abomination of Oryx", 8, 2, 1, 11000),
                new Reproduce("Monstrosity of Oryx", 8, 1, 1, 12000),
                new Reproduce("Vintner of Oryx", 8, 2, 1, 13000),
                new Reproduce("Bile of Oryx", 8, 1, 1, 14000),
                new Shoot(8, index: 0, aim: 1, coolDown: 1000),
                new Shoot(8, index: 1, shoots: 3, shootAngle: 20, coolDown: 1500, coolDownOffset: 500)
                )
            )

        .Init("Aberrant of Oryx",
            new State(
                new Protect(8, "Henchman of Oryx", 15, 8, 2),
                new Wander(4),
                new TossObject("Aberrant Blaster", 14, coolDown: 1500)
                )
            )

        .Init("Aberrant Blaster",
            new State(
                new State("searching",
                    new AddCond(ConditionEffectIndex.Invulnerable),
                    new PlayerWithinTransition(8, "creeping")
                    ),
                new State("creeping",
                    new RemCond(ConditionEffectIndex.Invulnerable),
                    new Shoot(range: 10, index: 0, shoots: 6, aim: 1),
                    new Decay(500)
                    )
                )
            )

        .Init("Abomination of Oryx",
            new State(
                new State("start",
                    new Protect(8, "Henchman of Oryx", 15, 12, 2),
                    new Wander(6),
                    new Chase(20, 10, 3000),
                    new Chase(8, range: 0),
                    new Shoot(10, 3, 10, 0, coolDown: 3000),
                    new Shoot(10, 5, 10, 1, coolDown: 3000, coolDownOffset: 100),
                    new Shoot(10, 7, 10, 2, coolDown: 3000, coolDownOffset: 200),
                    new Shoot(10, 5, 10, 3, coolDown: 3000, coolDownOffset: 300),
                    new Shoot(10, 3, 10, 4, coolDown: 3000, coolDownOffset: 400)
                    )
                )
            )

        .Init("Monstrosity of Oryx",
            new State(
                new Protect(8, "Henchman of Oryx", 15, 8, 2),
                new Wander(),
                new TossObject("Monstrosity Scarab", 10, coolDown: 1000)
                )
            )

        .Init("Vintner of Oryx",
            new State(
                new Wander(),
                new Shoot(10, 1, 10, 0, coolDown: 1000, aim: 1)
                )
            )

        .Init("Bile of Oryx",
            new State(
                new Wander()
                //new Reproduce("Purple Goo", 12, 8, 1, 1000)
                )
            )

        // buggy
        .Init("Purple Goo",
            new State(
                new State("main",
                    new Shoot(2, 1, 10, 0, coolDown: 500, aim: 1),
                    new TimedTransition(5000, "Die")
                    ),
                new State("Die",
                    new Shoot(range: 2, shoots: 1, shootAngle: 10, coolDown: 500, aim: 1),
                    new ChangeSize(-10, 0),
                    new Decay(5400)
                    )
                )
            )

        .Init("Monstrosity Scarab",
            new State(
                new State("searching",
                    new Prioritize(
                        new Chase(30, range: 0)
                        ),
                    new PlayerWithinTransition(4, "creeping"),
                    new TimedTransition(5000, "creeping")
                    ),
                new State("creeping",
                    new Shoot(3, 20, 18, direction: 0),
                    new Decay(0)
                    )
                )
            )
        ;
    }
}