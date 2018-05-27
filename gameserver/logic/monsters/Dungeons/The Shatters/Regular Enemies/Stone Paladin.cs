using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.engine;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic.monsters.Dungeons.The_Shatters.Regular_Enemies
{
    public class StonePaladin : IBehavior
    {
        public State Behavior() =>
            new State(
                new State("Idle",
                    new Prioritize(
                        new Wander(8)
                        ),
                    new AddCond(ConditionEffectIndex.Invulnerable),
                    new Reproduce(max: 4),
                    new PlayerWithinTransition(8, "Attacking")
                    ),
                new State("Attacking",
                    new State("Bullet",
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 90, coolDownOffset: 0, shootAngle: 90),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 100, coolDownOffset: 200, shootAngle: 90),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 110, coolDownOffset: 400, shootAngle: 90),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 120, coolDownOffset: 600, shootAngle: 90),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 130, coolDownOffset: 800, shootAngle: 90),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 140, coolDownOffset: 1000, shootAngle: 90),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 150, coolDownOffset: 1200, shootAngle: 90),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 160, coolDownOffset: 1400, shootAngle: 90),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 170, coolDownOffset: 1600, shootAngle: 90),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 180, coolDownOffset: 1800, shootAngle: 90),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 180, coolDownOffset: 2000, shootAngle: 45),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 180, coolDownOffset: 0, shootAngle: 90),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 170, coolDownOffset: 200, shootAngle: 90),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 160, coolDownOffset: 400, shootAngle: 90),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 150, coolDownOffset: 600, shootAngle: 90),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 140, coolDownOffset: 800, shootAngle: 90),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 130, coolDownOffset: 1000, shootAngle: 90),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 120, coolDownOffset: 1200, shootAngle: 90),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 110, coolDownOffset: 1400, shootAngle: 90),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 100, coolDownOffset: 1600, shootAngle: 90),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 90, coolDownOffset: 1800, shootAngle: 90),
                        new Shoot(1, 4, coolDown: 10000, angleOffset: 90, coolDownOffset: 2000, shootAngle: 22.5),
                        new TimedTransition(2000, "Wait")
                        ),
                    new State("Wait",
                        new Chase(0.4, range: 2),
                        new Flashing(0xff00ff00, 0.1, 20),
                        new AddCond(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(2000, "Bullet")
                        ),
                    new NoPlayerWithinTransition(13, "Idle")
                    )
                );
    }
}
