using LoESoft.GameServer.logic.behaviors;
using LoESoft.GameServer.logic.transitions;

namespace LoESoft.GameServer.logic
{
    partial class BehaviorDb
    {
        private _ NexusCrier = () => Behav()
 .Init("Nexus Crier",
   new State("Taunt",
                    new State("Taunt1",
                        new Wander(2),
                        new Taunt(0.3, true, 10000,
                            "Welcome to LoE Realm! This is the official server of LoE Team. You will have no more deleting accounts! Can play quiet, your data is in good hands.",
                            "If you want to perform a donation to help us go to the main menu and click on the banner Support us!",
                            "For questions please contact one of our Community Moderators!",
                            "Never tell your password and login data to other players! Or for members of staff!!!",
                            "If you want more information ask Gazer is very simple use the following command: /tell gazer help",
                            "The LoE Realm server is online since 1st October 2015.",
                            "The server automatically restarts every hour (60 minutes). Use the following command: /tell gazer uptime.",
                            "Visit our official website www.loerealm.com!",
                            "Follow us on Discord for Updates and Events!  https://discord.gg/5QyJ5Xy"),
                        new TimedTransition(11000, "Taunt1")
                       )
                     )
                   )
        ;
    }
}