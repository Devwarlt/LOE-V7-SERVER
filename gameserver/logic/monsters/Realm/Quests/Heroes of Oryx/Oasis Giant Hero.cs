using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.loot;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ HeroesofOryxOasisGiantHero = () => Behav()
            .Init("Oasis Giant",
                new State(
                    new Shoot(10, shoots: 4, shootAngle: 7, aim: 1),
                    new Prioritize(
                        new StayCloseToSpawn(3, 2),
                        new Wander()
                        ),
                    new SpawnGroup("Oasis", maxChildren: 20, coolDown: 5000),
                    new Taunt(0.7, 10000,
                        "Come closer, {PLAYER}! Yes, closer!",
                        "I rule this place, {PLAYER}!",
                        "Surrender to my aquatic army, {PLAYER}!",
                        "You must be thirsty, {PLAYER}. Enter my waters!",
                        "Minions! We shall have {PLAYER} for dinner!"
                        )
                    )
            )

            .Init("Oasis Ruler",
                new State(
                    new Prioritize(
                        new Protect(5, "Oasis Giant", sightRange: 15, protectRange: 10, reprotectRange: 3),
                        new Chase(10, range: 9),
                        new Wander(5)
                        ),
                    new Shoot(10)
                    ),
                new ItemLoot("Magic Potion", 0.05)
            )

            .Init("Oasis Soldier",
                new State(
                    new Prioritize(
                        new Protect(5, "Oasis Giant", sightRange: 15, protectRange: 10, reprotectRange: 3),
                        new Chase(10, range: 7),
                        new Wander(5)
                        ),
                    new Shoot(10, aim: 0.5)
                    ),
                new ItemLoot("Health Potion", 0.05)
            )

            .Init("Oasis Creature",
                new State(
                    new Prioritize(
                        new Protect(5, "Oasis Giant", sightRange: 15, protectRange: 10, reprotectRange: 3),
                        new Chase(10, range: 5),
                        new Wander(5)
                        ),
                    new Shoot(10, coolDown: 400)
                    ),
                new ItemLoot("Health Potion", 0.05)
            )

            .Init("Oasis Monster",
                new State(
                    new Prioritize(
                        new Protect(5, "Oasis Giant", sightRange: 15, protectRange: 10, reprotectRange: 3),
                        new Chase(10, range: 3),
                        new Wander(5)
                        ),
                    new Shoot(10, aim: 0.5)
                    ),
                new ItemLoot("Magic Potion", 0.05)
            )
        ;
    }
}