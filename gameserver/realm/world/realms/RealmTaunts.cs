using System;

namespace LoESoft.GameServer.realm
{
    internal partial class Realm
    {
        private struct TauntData
        {
            public string[] final;
            public string[] killed;
            public string[] numberOfEnemies;
            public string[] spawn;
        }

        #region "Taunt data"

        private static readonly Tuple<string, TauntData>[] criticalEnemies =
        {
            Tuple.Create("Lich", new TauntData
            {
                numberOfEnemies = new[]
                {
                    "I am invincible while my {COUNT} Liches still stand!",
                    "My {COUNT} Liches will feast on your essence!"
                },
                final = new[]
                {
                    "My final Lich shall consume your souls!",
                    "My final Lich will protect me forever!"
                }
            }),
            Tuple.Create("Ent Ancient", new TauntData
            {
                numberOfEnemies = new[]
                {
                    "Mortal scum! My {COUNT} Ent Ancients will defend me forever!",
                    "My forest of {COUNT} Ent Ancients is all the protection I need!"
                },
                final = new[]
                {
                    "My final Ent Ancient will destroy you all!"
                }
            }),
            Tuple.Create("Oasis Giant", new TauntData
            {
                numberOfEnemies = new[]
                {
                    "My {COUNT} Oasis Giants will feast on your flesh!",
                    "You have no hope against my {COUNT} Oasis Giants!"
                },
                final = new[]
                {
                    "A powerful Oasis Giant still fights for me!",
                    "You will never defeat me while an Oasis Giant remains!"
                }
            }),
            Tuple.Create("Phoenix Lord", new TauntData
            {
                numberOfEnemies = new[]
                {
                    "Maggots! My {COUNT} Phoenix Lord will burn you to ash!"
                },
                final = new[]
                {
                    "My final Phoenix Lord will never fall!",
                    "My last Phoenix Lord will blacken your bones!"
                }
            }),
            Tuple.Create("Ghost King", new TauntData
            {
                numberOfEnemies = new[]
                {
                    "My {COUNT} Ghost Kings give me more than enough protection!",
                    "Pathetic humans! My {COUNT} Ghost Kings shall destroy you utterly!"
                },
                final = new[]
                {
                    "A mighty Ghost King remains to guard me!",
                    "My final Ghost King is untouchable!"
                }
            }),
            Tuple.Create("Cyclops God", new TauntData
            {
                numberOfEnemies = new[]
                {
                    "Cretins! I have {COUNT} Cyclops Gods to guard me!",
                    "My {COUNT} powerful Cyclops Gods will smash you!"
                },
                final = new[]
                {
                    "My last Cyclops God will smash you to pieces!"
                }
            }),
            Tuple.Create("Red Demon", new TauntData
            {
                numberOfEnemies = new[]
                {
                    "Fools! There is no escape from my {COUNT} Red Demons!",
                    "My legion of {COUNT} Red Demons live only to serve me!"
                },
                final = new[]
                {
                    "My final Red Demon is unassailable!"
                }
            }),
            Tuple.Create("Skull Shrine", new TauntData
            {
                spawn = new[]
                {
                    "Your futile efforts are no match for a Skull Shrine!"
                },
                numberOfEnemies = new[]
                {
                    "Imbeciles! My {COUNT} Skull Shrines make me invincible!"
                },
                killed = new[]
                {
                    "{PLAYER} defaced a Skull Shrine! Minions, to arms!",
                    "{PLAYER} razed one of my Skull Shrines -- I WILL HAVE MY REVENGE!",
                    "{PLAYER}, you will rue the day you dared to defile my Skull Shrine!",
                    "{PLAYER}, you contemptible pig! Ruining my Skull Shrine will be the last mistake you ever make!",
                    "{PLAYER}, you insignificant cur! The penalty for destroying a Skull Shrine is death!"
                }
            }),
            Tuple.Create("Cube God", new TauntData
            {
                spawn = new[]
                {
                    "Your meager abilities cannot possibly challenge a Cube God!"
                },
                numberOfEnemies = new[]
                {
                    "Filthy vermin! My {COUNT} Cube Gods will exterminate you!",
                    "Loathsome slugs! My {COUNT} Cube Gods will defeat you!",
                    "You piteous cretins! {COUNT} Cube Gods still guard me!",
                    "Your pathetic rabble will never survive against my {COUNT} Cube Gods!"
                },
                final = new[]
                {
                    "Worthless mortals! A mighty Cube God defends me!"
                },
                killed = new[]
                {
                    "You have dispatched my Cube God, {PLAYER}, but you will never escape my Realm!",
                    "I have many more Cube Gods, {PLAYER}!",
                    "{PLAYER}, you wretched dog! You killed my Cube God!",
                    "{PLAYER}, you pathetic swine! How dare you assault my Cube God!"
                }
            }),
            Tuple.Create("Pentaract", new TauntData
            {
                spawn = new[]
                {
                    "Behold my Pentaract, and despair!"
                },
                numberOfEnemies = new[]
                {
                    "Wretched creatures! {COUNT} Pentaracts remain!",
                    "You detestable humans will never defeat my {COUNT} Pentaracts!",
                    "Defiance is useless! My {COUNT} Pentaracts will crush you!"
                },
                final = new[]
                {
                    "Ignorant fools! A Pentaract guards me still!"
                },
                killed = new[]
                {
                    "That was but one of many Pentaracts, {PLAYER}!",
                    "{PLAYER}, you flea-ridden animal! You destoryed my Pentaract!",
                    "{PLAYER}, by destroying my Pentaract you have sealed your own doom!"
                }
            }),
            Tuple.Create("Grand Sphinx", new TauntData
            {
                spawn = new[]
                {
                    "At last, a Grand Sphinx will teach you to respect!",
                    "A Grand Sphinx is more than a match for this rabble."
                },
                numberOfEnemies = new[]
                {
                    "You dull-spirited apes! You shall pose no challenge for {COUNT} Grand Sphinxes!",
                    "Regret your choices, blasphemers! My {COUNT} Grand Sphinxes will teach you respect!",
                    "My Grand Sphinxes will bewitch you with their beauty!"
                },
                final = new[]
                {
                    "You festering rat-catchers! A Grand Sphinx will make you doubt your purpose!",
                    "Gaze upon the beauty of the Grand Sphinx and feel your last hopes drain away."
                },
                killed = new[]
                {
                    "My Grand Sphinx, she was so beautiful. I will kill you myself, {PLAYER}!",
                    "My Grand Sphinx had lived for thousands of years! You, {PLAYER}, will not survive the day!",
                    "{PLAYER}, you up-jumped goat herder! You shall pay for defeating my Grand Sphinx!",
                    "{PLAYER}, you pestiferous lout! I will not forget what you did to my Grand Sphinx!",
                    "{PLAYER}, you foul ruffian! Do not think I forget your defiling of my Grand Sphinx!"
                }
            }),
            Tuple.Create("Lord of the Lost Lands", new TauntData
            {
                spawn = new[]
                {
                    "Pathetic fools! My Lord of the Lost Lands will crush you all!",
                    "My Lord of the Lost Lands will make short work of you!"
                },
                //numberOfEnemies = new string[] {
                //    "You dull-spirited apes! You shall pose no challenge for {COUNT} Grand Sphinxes!",
                //    "Regret your choices, blasphemers! My {COUNT} Grand Sphinxes will teach you respect!",
                //    "My Grand Sphinxes will bewitch you with their beauty!"
                //},
                //final = new string[] {
                //    "You festering rat-catchers! A Grand Sphinx will make you doubt your purpose!",
                //    "Gaze upon the beauty of the Grand Sphinx and feel your last hopes drain away."
                //},
                killed = new[]
                {
                    "How dare you foul-mouthed hooligans treat my Lord of the Lost Lands with such indignity!",
                    "What trickery is this?! My Lord of the Lost Lands was invincible!"
                }
            }),
            Tuple.Create("Hermit God", new TauntData
            {
                spawn = new[]
                {
                    "My Hermit God's thousand tentacles shall drag you to a watery grave!"
                },
                //numberOfEnemies = new string[] {
                //    "You dull-spirited apes! You shall pose no challenge for {COUNT} Grand Sphinxes!",
                //    "Regret your choices, blasphemers! My {COUNT} Grand Sphinxes will teach you respect!",
                //    "My Grand Sphinxes will bewitch you with their beauty!"
                //},
                //final = new string[] {
                //    "You festering rat-catchers! A Grand Sphinx will make you doubt your purpose!",
                //    "Gaze upon the beauty of the Grand Sphinx and feel your last hopes drain away."
                //},
                killed = new[]
                {
                    "My Hermit God was more than you'll ever be, {PLAYER}. I will kill you myself!",
                    "You naive imbecile, {PLAYER}! Without my Hermit God, Dreadstump is free to roam the seas without fear!"
                }
            }),
            Tuple.Create("Ghost Ship", new TauntData
            {
                spawn = new[]
                {
                    "My Ghost Ship will terrorize you pathetic peasants!",
                    "A Ghost Ship has entered the Realm."
                },
                //numberOfEnemies = new string[] {
                //    "You dull-spirited apes! You shall pose no challenge for {COUNT} Grand Sphinxes!",
                //    "Regret your choices, blasphemers! My {COUNT} Grand Sphinxes will teach you respect!",
                //    "My Grand Sphinxes will bewitch you with their beauty!"
                //},
                //final = new string[] {
                //    "You festering rat-catchers! A Grand Sphinx will make you doubt your purpose!",
                //    "Gaze upon the beauty of the Grand Sphinx and feel your last hopes drain away."
                //},
                killed = new[]
                {
                    "How could a creature like {PLAYER} defeat my dreaded Ghost Ship?!",
                    "{PLAYER}, has crossed me for the last time! My Ghost Ship shall be avenged.",
                    "The spirits of the sea will seek revenge on your worthless soul, {PLAYER}!"
                }
            }),
               Tuple.Create("Frog King", new TauntData
            {
                spawn = new[]
                {
                    "Behold! My Frog King will take care of the likeness of you!"
                },
                numberOfEnemies = new[]
                {
                    "Stupid Mortal! My {COUNT} Frog Kings will annihilate you!",
                    "My {COUNT} Frog Kings will be enough to destroy the likeness of you fools!",
                    "Defiance is useless! My {COUNT} Frog Kings will crush you!"
                },
                final = new[]
                {
                    "Short-Sighted Noobs! A Frog King will guard me to the very end!"
                },
                killed = new[]
                {
                    "REEEEEE!! WHY TF DID YOU KILL MY FROG KING, {PLAYER}!",
                    "IDIOT!NOW FILISHA HIMSELF WILL COME AFTER YOU FOR KILLING HIS PET FROG, {PLAYER}!"

                }
            })
        };

        #endregion "Taunt data"
    }
}