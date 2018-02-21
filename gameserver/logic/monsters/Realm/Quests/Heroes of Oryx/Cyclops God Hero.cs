using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.loot;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ HeroesofOryxCyclopsGodHero = () => Behav()
            .Init("Cyclops God",
                new State(
                    new DropPortalOnDeath("Spider Den Portal", 0.1),
                    new State("idle",
                        new PlayerWithinTransition(11, "blade_attack"),
                        new HpLessTransition(0.8, "blade_attack")
                        ),
                    new State("blade_attack",
                        new Prioritize(
                            new Chase(4, range: 7),
                            new Wander()
                            ),
                        new Shoot(10, index: 4, shoots: 1, shootAngle: 15, aim: 0.5, coolDown: 100000),
                        new Shoot(10, index: 4, shoots: 2, shootAngle: 10, aim: 0.5, coolDown: 100000, coolDownOffset: 700),
                        new Shoot(10, index: 4, shoots: 3, shootAngle: 8.5, aim: 0.5, coolDown: 100000, coolDownOffset: 1400),
                        new Shoot(10, index: 4, shoots: 4, shootAngle: 7, aim: 0.5, coolDown: 100000, coolDownOffset: 2100),
                        new TimedTransition(4000, "if_cloaked1")
                        ),
                    new State("if_cloaked1",
                        new Shoot(10, index: 4, shoots: 15, shootAngle: 24, direction: 8, coolDown: 1500, coolDownOffset: 400),
                        new TimedTransition(10000, "wave_attack"),
                        new PlayerWithinTransition(10.5, "wave_attack")
                        ),
                    new State("wave_attack",
                        new Prioritize(
                            new Chase(6, range: 5),
                            new Wander(6)
                            ),
                        new Shoot(9, index: 0, coolDown: 700, coolDownOffset: 700),
                        new Shoot(9, index: 1, coolDown: 700, coolDownOffset: 700),
                        new Shoot(9, index: 2, coolDown: 700, coolDownOffset: 700),
                        new Shoot(9, index: 3, coolDown: 700, coolDownOffset: 700),
                        new TimedTransition(3800, "if_cloaked2")
                        ),
                    new State("if_cloaked2",
                        new Shoot(10, index: 4, shoots: 15, shootAngle: 24, direction: 8, coolDown: 1500, coolDownOffset: 400),
                        new TimedTransition(10000, "idle"),
                        new PlayerWithinTransition(10.5, "idle")
                        ),
                    new Taunt(0.7, 10000,
                        "I will floss with your tendons!",
                        "I smell the blood of an Englishman!",
                        "I will suck the marrow from your bones!",
                        "You will be my food, {PLAYER}!",
                        "Blargh!!",
                        "Leave my castle!",
                        "More wine!"
                        ),
                    new StayCloseToSpawn(12, 5),
                    new Spawn("Cyclops", maxChildren: 5, coolDown: 10000),
                    new Spawn("Cyclops Warrior", maxChildren: 5, coolDown: 10000),
                    new Spawn("Cyclops Noble", maxChildren: 5, coolDown: 10000),
                    new Spawn("Cyclops Prince", maxChildren: 5, coolDown: 10000),
                    new Spawn("Cyclops King", maxChildren: 5, coolDown: 10000)
                    )
            )

            .Init("Cyclops",
                new State(
                    new Prioritize(
                        new StayCloseToSpawn(12, 5),
                        new Chase(12, range: 1),
                        new Wander()
                        ),
                    new Shoot(3)
                    ),
                new ItemLoot("Golden Sword", 0.02),
                new ItemLoot("Studded Leather Armor", 0.02),
                new ItemLoot("Health Potion", 0.05)
            )

            .Init("Cyclops Warrior",
                new State(
                    new Prioritize(
                        new StayCloseToSpawn(12, 5),
                        new Chase(12, range: 1),
                        new Wander()
                        ),
                    new Shoot(3)
                    ),
                new ItemLoot("Golden Sword", 0.03),
                new ItemLoot("Golden Shield", 0.02),
                new ItemLoot("Health Potion", 0.05)
            )

            .Init("Cyclops Noble",
                new State(
                    new Prioritize(
                        new StayCloseToSpawn(12, 5),
                        new Chase(12, range: 1),
                        new Wander()
                        ),
                    new Shoot(3)
                    ),
                new ItemLoot("Golden Dagger", 0.02),
                new ItemLoot("Studded Leather Armor", 0.02),
                new ItemLoot("Health Potion", 0.05)
            )

            .Init("Cyclops Prince",
                new State(
                    new Prioritize(
                        new StayCloseToSpawn(12, 5),
                        new Chase(12, range: 1),
                        new Wander()
                        ),
                    new Shoot(3)
                    ),
                new ItemLoot("Mithril Dagger", 0.02),
                new ItemLoot("Plate Mail", 0.02),
                new ItemLoot("Seal of the Divine", 0.01),
                new ItemLoot("Health Potion", 0.05)
            )

            .Init("Cyclops King",
                new State(
                    new Prioritize(
                        new StayCloseToSpawn(12, 5),
                        new Chase(12, range: 1),
                        new Wander()
                        ),
                    new Shoot(3)
                    ),
                new ItemLoot("Golden Sword", 0.02),
                new ItemLoot("Mithril Armor", 0.02),
                new ItemLoot("Health Potion", 0.05)
            )
        ;
    }
}