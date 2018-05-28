using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.skills.Pets;

namespace LoESoft.GameServer.logic
{
    /// <summary>
    /// Regular pets with tier between 0 to 7.
    /// </summary>

    partial class BehaviorDb
    {
        private _ RegularPets = () => Behav()
           .Init("Little Ghost",
               new State(
                   new PetFollow(),
                   new PetShoot(),
                   new Taunt(0.3, false, new Cooldown(45000, 15000),
                       "Buuu!",
                       "Buh!"
                   ),
                   new PetHPHealing(),
                   new PetMPHealing()
               )
           )

           .Init("Chilling Fire",
               new State(
                   new PetFollow(),
                   new PetShoot(),
                   new Taunt(0.3, false, new Cooldown(45000, 15000),
                       "Fire!",
                       "Tss..."
                   ),
                   new PetHPHealing(),
                   new PetMPHealing()
               )
           )

           .Init("Mini Tree",
               new State(
                   new PetFollow(),
                   new PetShoot(),
                   new Taunt(0.3, false, new Cooldown(45000, 15000),
                       "Kraak! Kraak!"
                       ),
                   new PetHPHealing(),
                   new PetMPHealing()
               )
           )

           .Init("Mini Icycle",
               new State(
                   new PetFollow(),
                   new PetShoot(),
                   new Taunt(0.3, false, new Cooldown(45000, 15000),
                       "Frzz...",
                       "*blink*"
                       ),
                   new PetHPHealing(),
                   new PetMPHealing()
               )
           )

           .Init("Volcano Guard",
               new State(
                   new PetFollow(),
                   new PetShoot(),
                   new behaviors.Taunt(0.3, false, new Cooldown(45000, 15000),
                       "Lava!",
                       "Tss..."
                       ),
                   new PetHPHealing(),
                   new PetMPHealing()
               )
           )

           .Init("Blaze Guard",
               new State(
                   new PetFollow(),
                   new PetShoot(),
                   new Taunt(0.3, false, new Cooldown(45000, 15000),
                       "Blaze!",
                       "Tss..."
                       ),
                   new PetHPHealing(),
                   new PetMPHealing()
               )
           )

           .Init("Sudden Death",
               new State(
                   new PetFollow(),
                   new PetShoot(),
                   new Taunt(0.3, false, new Cooldown(45000, 15000),
                       "Grr...",
                       "Dum, dum, dum..."
                       ),
                   new PetHPHealing(),
                   new PetMPHealing()
               )
           )

           .Init("Evil Blob",
               new State(
                   new PetFollow(),
                   new PetShoot(),
                   new Taunt(0.3, false, new Cooldown(45000, 15000),
                       "Brzk! U'th DazK...",
                       "..."
                       ),
                   new PetHPHealing(),
                   new PetMPHealing()
               )
           )
        ;
    }
}