using gameserver.logic.behaviors;
using gameserver.logic.transitions;

namespace gameserver.logic
{
    partial class BehaviorDb
    {
        private _ SetpieceBossesKageKamiSetpiece = () => Behav()
            .Init("Kage Kami",
                new State(
                    new DropPortalOnDeath("Manor of the Immortals Portal",0.5),
                    // Bug
                    //new State("Grave",
                    //    new SetAltTexture(0),
                    //    new HpLessTransition(.90,"yay i am good")
                    //    ),
                    new State("yay i am good",
                        new Taunt(0.5,"Kyoufu no kage!"),
                        new AddCond(ConditionEffectIndex.Invincible),
                        new ChangeSize(20, 120),
                        new SetAltTexture(1),
                        new TimedTransition(2000, "Attack")
                        ),
                    new State("Attack",
                        new Wander(),
                        new SetAltTexture(1),
                        new TimedTransition(5000, "Charge"),
                        new TossObject("Specter Mine",coolDown:2000),
                        new State("Shoot1",
                            new Shoot(0, shoots: 1, defaultAngle: 0, rotateAngle: 30, coolDown: 300, index: 1),
                            new Shoot(0, shoots: 1, defaultAngle: 180, rotateAngle: 30, coolDown: 300, index: 1),
                            new TimedTransition(1000, "Shoot2")
                            ),
                        new State("Shoot2",
                            new Shoot(20, shoots: 2, shootAngle: 180, coolDown: 400, index: 1),
                            new TimedTransition(400, "Shoot1")
                            )
                        ),
                    new State("Charge",
                        new AddCond(ConditionEffectIndex.Invincible),
                        new TossObject("Specter Mine", coolDown: 2000),
                        new SetAltTexture(2),
                        new Chase(8, 20, 1),
                        new Shoot(20, shoots: 2, shootAngle: 50, coolDown: 400),
                        new TimedTransition(4000, "Attack")
                        )
                    )
            )

            .Init("Specter Mine",
                new State(
                    new State("Waiting",
                        new PlayerWithinTransition(3, "Suicide"),
                        new TimedTransition(4000, "Suicide")
                        ),
                    new State("Suicide",
                        new Shoot(60, shoots: 4, shootAngle: 45),
                        new Suicide()
                        )
                    )
            )
        ;
    }
}