#region

using gameserver.logic.behaviors;
using gameserver.logic.transitions;

#endregion

namespace gameserver.logic
{
    partial class BehaviorDb
    {
        private _ Tutorial = () => Behav()
            .Init("West Tutorial Gun",
                new State(
                    new Shoot(32, direction: 180, coolDown: new Cooldown(3000, 1000))
                    )
            )
            .Init("North Tutorial Gun",
                new State(
                    new Shoot(32, direction: 270, coolDown: new Cooldown(3000, 1000))
                    )
            )
            .Init("East Tutorial Gun",
                new State(
                    new Shoot(32, direction: 0, coolDown: new Cooldown(3000, 1000))
                    )
            )
            .Init("South Tutorial Gun",
                new State(
                    new Shoot(32, direction: 90, coolDown: new Cooldown(3000, 1000))
                    )
            )
            .Init("Evil Chicken",
                new State(
                    new Wander(3)
                    )
            )
            .Init("Evil Chicken Minion",
                new State(
                    new Wander(3),
                    new Protect(3, "Evil Chicken God")
                    )
            )
            .Init("Evil Chicken God",
                new State(
                    new Prioritize(
                        new Chase(0.4, range: 5),
                        new Wander(3)
                        ),
                    new Reproduce("Evil Chicken Minion", max: 12)
                    )
            )
            .Init("Evil Hen",
                new State(
                    new Wander(3)
                    )
            )
            .Init("Kitchen Guard",
                new State(
                    new Prioritize(
                        new Chase(6, range: 6),
                        new Wander(4)
                        ),
                    new Shoot(7)
                    )
            )
            .Init("Butcher",
                new State(
                    new Prioritize(
                        new Chase(8, range: 1),
                        new Wander(4)
                        ),
                    new Shoot(3)
                    )
            )
            .Init("Bonegrind the Butcher",
                new State(
                    new State("Begin",
                        new Wander(6),
                        new PlayerWithinTransition(10, "AttackX")
                        ),
                    new State("AttackX",
                        new Taunt("Ah, fresh meat for the minions!"),
                        new Shoot(6, coolDown: 1400),
                        new Prioritize(
                            new Chase(6, 9, 3),
                            new Wander(6)
                            ),
                        new TimedTransition(4500, "AttackY"),
                        new HpLessTransition(0.3, "Flee")
                        ),
                    new State("AttackY",
                        new Prioritize(
                            new Chase(6, 9, 3),
                            new Wander(6)
                            ),
                        new Sequence(
                            new Shoot(7, 4, direction: 25),
                            new Shoot(7, 4, direction: 50),
                            new Shoot(7, 4, direction: 75),
                            new Shoot(7, 4, direction: 100),
                            new Shoot(7, 4, direction: 125)
                            ),
                        new TimedTransition(5200, "AttackX"),
                        new HpLessTransition(0.3, "Flee")
                        ),
                    new State("Flee",
                        new Taunt("The meat ain't supposed to bite back! Waaaaa!!"),
                        new Flashing(0xff000000, 10, 100),
                        new Prioritize(
                            new Retreat(5, 6),
                            new Wander(5)
                            )
                        )
                    )
            )
        ;
    }
}