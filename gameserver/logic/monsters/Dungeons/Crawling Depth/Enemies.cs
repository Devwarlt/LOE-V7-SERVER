using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.loot;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ EpicSpiderDen = () => Behav()
            .Init("Son of Arachna",
                 new State(
                    new DropPortalOnDeath("Glowing Realm Portal", 100, PortalDespawnTimeSec: 360),
                     new State("idle",
                         new AddCond(ConditionEffectIndex.Invulnerable), // ok
                         new PlayerWithinTransition(12, "WEB!")
                         ),
                     new State("WEB!",
                         new RemCond(ConditionEffectIndex.Invulnerable), // ok
                         new TossObject("Arachna Web Spoke 7", 6, 0, 100000),
                         new TossObject("Arachna Web Spoke 8", 6, 120, 100000),
                         new TossObject("Arachna Web Spoke 9", 6, 240, 100000),
                         new TossObject("Arachna Web Spoke 1", 10, 0, 100000),
                         new TossObject("Arachna Web Spoke 2", 10, 60, 100000),
                         new TossObject("Arachna Web Spoke 3", 10, 120, 100000),
                         new TossObject("Arachna Web Spoke 4", 10, 180, 100000),
                         new TossObject("Arachna Web Spoke 5", 10, 240, 100000),
                         new TossObject("Arachna Web Spoke 6", 10, 300, 100000),
                         new TimedTransition(2000, "attack")
                         ),
                     new State("attack",
                         new Wander(10),
                         new Shoot(3000, shoots: 12, index: 0, angleOffset: fixedAngle_RingAttack2),
                         new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 0, aim: 1, coolDown: 1000, coolDownOffset: 0),
                         new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 1, aim: 1, coolDown: 2000, coolDownOffset: 0)
                         )
                     ),
                 new Threshold(0.32,
                    new ItemLoot("Potion of Mana", 1),
                    new ItemLoot("Doku No Ken", 0.007)
                     )
            )

        .Init("Crawling Green Spider",
            new State(
                new State("idle",
                    new Wander(8),
                    new Charge(0.9, 20f, 2000),
                    new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 0, aim: 1,
                    coolDown: 500, coolDownOffset: 0)
                         )
                     ),
                    new ItemLoot("Healing Ichor", 0.2)
            )
        .Init("Crawling Grey Spider",
            new State(
                new State("idle",
                    new Wander(8),
                    new Charge(0.9, 40f, 2000),
                    new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 0, aim: 1,
                    coolDown: 500, coolDownOffset: 0)
                         )
                     ),
                    new ItemLoot("Healing Ichor", 0.2)
            )
       .Init("Crawling Grey Spotted Spider",
            new State(
                new State("idle",
                    new Wander(8),
                    new Chase(0.8, 0.3, 0),
                    new Shoot(10, 3, 20, angleOffset: 0 / 3, index: 0, coolDown: 500)
                    )
                ),
                new ItemLoot("Healing Ichor", 0.2)
           )
       .Init("Crawling Spider Hatchling",
            new State(
                new State("idle",
                    new Wander(0),
                    new Chase(0.8, 0.8, 0),
                    new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 0, aim: 1,
                    coolDown: 1000, coolDownOffset: 0)
                    )
                )
             )
       .Init("Crawling Depths Egg Sac",
            new State(
                new TransformOnDeath("Crawling Spider Hatchling", 2, 7),
                new State("idle",
                    new PlayerWithinTransition(0.5, "suicide")
                    ),
                new State("suicide",
                    new Suicide()
                    )
                )
            )
       .Init("Crawling Red Spotted Spider",
            new State(
                new State("idle",
                    new Wander(0),
                    new Chase(1.0, 0.8, 0),
                    new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 0, aim: 1,
                    coolDown: 500, coolDownOffset: 0)
                    )
                ),
                new ItemLoot("Healing Ichor", 0.2)




            );
    }
}
