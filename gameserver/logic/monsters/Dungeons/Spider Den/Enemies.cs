using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.loot;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ SpiderDen = () => Behav()
            .Init("Arachna the Spider Queen",
                 new State(
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
                         new Wander(1.0),
                         new Shoot(3000, shoots: 12, index: 0, angleOffset: fixedAngle_RingAttack2),
                         new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 0, aim: 1, coolDown: 1000, coolDownOffset: 0),
                         new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 1, aim: 1, coolDown: 2000, coolDownOffset: 0)
                         )
                     ),
                 new ItemLoot("Golden Dagger", 0.2),
                 new ItemLoot("Spider's Eye Ring", 0.2),
                 new ItemLoot("Poison Fang Dagger", 0.2),
                 new Threshold(0.32,
                     new ItemLoot("Healing Ichor", 1)
                     )
            )
        
        .Init("Arachna Web Spoke 1",
            new State(
                new State(":D",
                    new AddCond(ConditionEffectIndex.Invincible),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 120, angleOffset: 120, coolDown: 5),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 180, angleOffset: 180, coolDown: 5),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 240, angleOffset: 240, coolDown: 5)
                    )
                )
            )
       .Init("Arachna Web Spoke 2",
            new State(
                new State(":D",
                    new AddCond(ConditionEffectIndex.Invincible),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 240, angleOffset: 240, coolDown: 5),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 180, angleOffset: 180, coolDown: 5),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 300, angleOffset: 300, coolDown: 5)
                    )
                )
            )
      .Init("Arachna Web Spoke 3",
            new State(
                new State(":D",
                    new AddCond(ConditionEffectIndex.Invincible),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 300, angleOffset: 300, coolDown: 5),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 240, angleOffset: 240, coolDown: 5),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 0, angleOffset: 0, coolDown: 5)
                    )
                )
            )
       .Init("Arachna Web Spoke 4",
            new State(
                new State(":D",
                    new AddCond(ConditionEffectIndex.Invincible),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 0, angleOffset: 0, coolDown: 5),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 60, angleOffset: 60, coolDown: 5),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 300, angleOffset: 300, coolDown: 5)
                    )
                )
            )
      .Init("Arachna Web Spoke 5",
            new State(
                new State(":D",
                    new AddCond(ConditionEffectIndex.Invincible),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 60, angleOffset: 60, coolDown: 5),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 0, angleOffset: 0, coolDown: 5),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 120, angleOffset: 120, coolDown: 5)
                    )
                )
            )
       .Init("Arachna Web Spoke 6",
            new State(
                new State(":D",
                    new AddCond(ConditionEffectIndex.Invincible),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 120, angleOffset: 120, coolDown: 5),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 60, angleOffset: 60, coolDown: 5),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 180, angleOffset: 180, coolDown: 5)
                    )
                )
            )
        .Init("Arachna Web Spoke 7",
            new State(
                new State(":D",
                    new AddCond(ConditionEffectIndex.Invincible),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 180, angleOffset: 180, coolDown: 5),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 120, angleOffset: 120, coolDown: 5),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 240, angleOffset: 240, coolDown: 5)
                    )
                )
            )
        .Init("Arachna Web Spoke 8",
            new State(
                new State(":D",
                    new AddCond(ConditionEffectIndex.Invincible),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 360, angleOffset: 360, coolDown: 5),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 240, angleOffset: 240, coolDown: 5),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 300, angleOffset: 300, coolDown: 5)
                    )
                )
            )
        .Init("Arachna Web Spoke 9",
            new State(
                new State(":D",
                    new AddCond(ConditionEffectIndex.Invincible),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 0, angleOffset: 0, coolDown: 5),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 60, angleOffset: 60, coolDown: 5),
                    new Shoot(0, index: 0, shoots: 1, shootAngle: 120, angleOffset: 120, coolDown: 5)
                    )
                )
             )
        .Init("Black Den Spider",
            new State(
                new State("idle",
                    new Wander(8),
                    new Charge(0.9, 20f, 2000),
                    new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 0, aim: 1, coolDown: 500, coolDownOffset: 0)
                    )
                ),
            new ItemLoot("Healing Ichor", 0.2)
            )
        .Init("Black Spotted Den Spider",
            new State(
                new State("idle",
                    new Wander(8),
                    new Charge(0.9, 40f, 2000),
                    new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 0, aim: 1, coolDown: 500, coolDownOffset: 0)
                    )
                ),
            new ItemLoot("Healing Ichor", 0.2)
            )
       .Init("Brown Den Spider",
            new State(
                new State("idle",
                    new Wander(8),
                    new Chase(0.8, 0.3, 0),
                    new Shoot(10, 3, 20, angleOffset: 0 / 3, index: 0, coolDown: 500)
                    )
                ),
                new ItemLoot("Healing Ichor", 0.2)
           )
       .Init("Green Den Spider Hatchling",
            new State(
                new State("idle",
                    new Wander(0),
                    new Chase(0.8, 0.8, 0),
                    new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 0, aim: 1,
                    coolDown: 1000, coolDownOffset: 0)
                    )
                )
             )
       .Init("Spider Egg Sac",
            new State(
                new TransformOnDeath("Green Den Spider Hatchling", 2, 7),
                new State("idle",
                    new PlayerWithinTransition(0.5, "suicide")
                    ),
                new State("suicide",
                    new Suicide()
                    )
                )
            )
       .Init("Red Spotted Den Spider",
            new State(
                new State("idle",
                    new Wander(0),
                    new Chase(1.0, 0.8, 0),
                    new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 0, aim: 1,
                    coolDown: 500, coolDownOffset: 0)
                    )
                ),
                new ItemLoot("Healing Ichor", 0.2)
            )
       .Init("Arachna Summoner",
            new State(
                new AddCond(ConditionEffectIndex.Invincible),
                new RealmPortalDrop(),
                new State("idle",
                     new EntitiesNotExistsTransition(300, "Death", "Arachna the Spider Queen")
                    ),
                new State("Death",
                    new Suicide()
                    )
                )
            );
    }
}
