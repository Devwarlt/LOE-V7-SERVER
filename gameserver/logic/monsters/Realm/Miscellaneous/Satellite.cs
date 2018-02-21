using LoESoft.GameServer.logic.behaviors;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ MiscellaneousSatellite = () => Behav()
            .Init("Red Satellite",
                new State(
                    new Prioritize(
                        new Circle(17, 2, target: "Fire Golem", sightRange: 15, speedVariance: 0, radiusVariance: 0),
                        new Circle(17, 2, target: "Metal Golem", sightRange: 15, speedVariance: 0, radiusVariance: 0)
                        ),
                    new Decay(16000)
                    )
            )

            .Init("Green Satellite",
                new State(
                    new Prioritize(
                        new Circle(11, 2, target: "Darkness Golem", sightRange: 15, speedVariance: 0, radiusVariance: 0),
                        new Circle(11, 2, target: "Earth Golem", sightRange: 15, speedVariance: 0, radiusVariance: 0)
                        ),
                    new Decay(16000)
                    )
            )

            .Init("Blue Satellite",
                new State(
                    new Prioritize(
                        new Circle(11, 2, target: "Clockwork Golem", sightRange: 15, speedVariance: 0, radiusVariance: 0),
                        new Circle(11, 2, target: "Paper Golem", sightRange: 15, speedVariance: 0, radiusVariance: 0)
                        ),
                    new Decay(16000)
                    )
            )

            .Init("Gray Satellite 1",
                new State(
                    new Shoot(6, shoots: 3, shootAngle: 34, aim: 0.3, coolDown: 850),
                    new Prioritize(
                        new Circle(22, 0.75, target: "Red Satellite", sightRange: 15, speedVariance: 0, radiusVariance: 0),
                        new Circle(22, 0.75, target: "Blue Satellite", sightRange: 15, speedVariance: 0, radiusVariance: 0)
                        ),
                    new Decay(16000)
                    )
            )

            .Init("Gray Satellite 2",
                new State(
                    new Shoot(7, aim: 0.3, coolDown: 600),
                    new Prioritize(
                        new Circle(22, 0.75, target: "Green Satellite", sightRange: 15, speedVariance: 0, radiusVariance: 0),
                        new Circle(22, 0.75, target: "Blue Satellite", sightRange: 15, speedVariance: 0, radiusVariance: 0)
                        ),
                    new Decay(16000)
                    )
            )

            .Init("Gray Satellite 3",
                new State(
                    new Shoot(7, shoots: 5, shootAngle: 72, coolDown: 3200, coolDownOffset: 600),
                    new Shoot(7, shoots: 4, shootAngle: 90, coolDown: 3200, coolDownOffset: 1400),
                    new Shoot(7, shoots: 5, shootAngle: 72, defaultAngle: 36, coolDown: 3200, coolDownOffset: 2200),
                    new Shoot(7, shoots: 4, shootAngle: 90, defaultAngle: 45, coolDown: 3200, coolDownOffset: 3000),
                    new Prioritize(
                        new Circle(22, 0.75, target: "Red Satellite", sightRange: 15, speedVariance: 0, radiusVariance: 0),
                        new Circle(22, 0.75, target: "Green Satellite", sightRange: 15, speedVariance: 0, radiusVariance: 0)
                        ),
                    new Decay(16000)
                    )
            )
        ;
    }
}