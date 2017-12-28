using gameserver.logic.behaviors;
using gameserver.logic.skills.Pets;

namespace gameserver.logic
{
    /// <summary>
    /// Limited pets.
    /// </summary>

    partial class BehaviorDb
    {
        private _ LimitedPets  = () => Behav()
            .Init("Muzzlereaper Minion",
                new State(
                    new PetFollow(),
                    new PetShoot(),
                    new Taunt(0.3, false, new Cooldown(45000, 15000),
                        "Mwaaaahnducate youuuuuu *gurgle*, mwaaah!",
                        "MMMWAHMWAHMWAHMWAAAAH!",
                        "Mwaaahgod! Overmwaaaaah! *gurgle*",
                        "Mmmwahmwahmwhah, mwaaah!"
                    ),
                    new PetHPHealing(),
                    new PetMPHealing()
                )
            )

            .Init("Living Leaf",
                new State(
                    new PetFollow(),
                    new PetShoot(),
                    new Taunt(0.3, false, new Cooldown(45000, 15000),
                        "*crackle*",
                        "*rustle*",
                        "*swwwwishhhh*"
                    ),
                    new PetHPHealing(),
                    new PetMPHealing()
                )
            )
        ;
    }
}