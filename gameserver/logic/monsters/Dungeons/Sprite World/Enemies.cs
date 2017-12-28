using gameserver.logic.behaviors;

namespace gameserver.logic
{
	partial class BehaviorDb
	{
		private _ SpriteWorldEnemies = () => Behav()
			.Init("Native Fire Sprite",
				new State(
                    new StayAbove(speed: 5, altitude: 95),
                    new Shoot(range: 10, shoots: 2, shootAngle: 7, index: 0, coolDown: 300),
                    new Wander(speed: 14)
				)
			)
            
            .Init("Native Ice Sprite",
				new State(
					new StayAbove(speed: 5, altitude: 105),
                    new Shoot(range: 10, shoots: 3, shootAngle: 7, index: 0, coolDown: 1000),
                    new Wander(speed: 14)
                )
            )

            .Init("Native Magic Sprite",
				new State(
					new StayAbove(speed: 5, altitude: 115),
                    new Shoot(range: 10, shoots: 4, shootAngle: 7, index: 0, coolDown: 1000),
                    new Wander(speed: 14)
                )
            )

            .Init("Native Nature Sprite",
				new State(
                    new Shoot(range: 10, shoots: 5, shootAngle: 7, index: 0, coolDown: 1000),
                    new Wander(speed: 16)
                )
            )

            .Init("Native Darkness Sprite",
				new State(
					new Shoot(range: 10, shoots: 5, shootAngle: 7, index: 0, coolDown: 1000),
                    new Wander(speed: 16)
                )
            )

            .Init("Native Sprite God",
				new State(
                    new StayAbove(speed: 5, altitude: 200),
					new Shoot(range: 12, shoots: 4, shootAngle: 10, index: 0, coolDown: 1000),
                    new Shoot(range: 10, index: 1, aim: 1, coolDown: 1000),
                    new Wander(speed: 4)
                )
            )
		;
	}
}