﻿#region

using LoESoft.GameServer.logic;
using LoESoft.GameServer.networking.outgoing;
using LoESoft.GameServer.realm.entity.npc;
using LoESoft.GameServer.realm.entity.player;

#endregion

namespace LoESoft.GameServer.realm.commands
{
    internal class GuildCommand : Command
    {
        public GuildCommand() : base("g")
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (!player.NameChosen)
            {
                player.SendError("Choose a name!");
                return false;
            }

            if (player.Muted)
            {
                player.SendError("Muted. You can not guild chat at this time.");
                return false;
            }

            if (string.IsNullOrEmpty(player.Guild))
            {
                player.SendError("You need to be in a guild to guild chat.");
                return false;
            }

            GameServer.Manager.Chat.Guild(player, string.Join(" ", args));
            return true;
        }
    }

    internal class TellCommand : Command
    {
        public TellCommand() : base("tell")
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (!player.NameChosen)
            {
                player.SendError("Choose a name!");
                return false;
            }

            if (args.Length < 2)
            {
                player.SendHelp("Usage: /tell <player name> <text>");
                return false;
            }

            string playername = args[0].Trim();
            string msg = string.Join(" ", args, 1, args.Length - 1);

            if (string.Equals(player.Name.ToLower(), playername.ToLower()))
            {
                player.SendInfo("Quit telling yourself!");
                return false;
            }

            if (string.Join(" ", args, 0, 1).ToLower() == "npc")
            {
                string npcName = $"NPC {Utils.FirstCharToUpper(string.Join(" ", args, 1, 1).ToLower())}";
                if (NPCs.Database.ContainsKey($"NPC {Utils.FirstCharToUpper(string.Join(" ", args, 1, 1).ToLower())}"))
                {
                    string npcMsg = args.Length > 2 ? string.Join(" ", args, 2, 1) : null;
                    NPC npc = NPCs.Database.ContainsKey(npcName) ? NPCs.Database[npcName] : null;
                    if (npcMsg == null || npcMsg == string.Empty || npcMsg == "" || npcMsg == " ")
                    {
                        player.SendInfo($"Send a valid message to NPC {Utils.FirstCharToUpper(string.Join(" ", args, 1, 1).ToLower())}.");
                        return false;
                    }
                    else
                    {
                        if (npc == null)
                        {
                            player.SendInfo($"Oh! {npcName} found in database but not declared yet, try again later.");
                            return false;
                        }
                        else
                        {
                            if (!npc.ReturnPlayersCache().Contains(player.Name))
                            {
                                player.SendInfo($"You need to initialize conversation with {npcName}.");
                                return false;
                            }
                            else
                            {
                                npc.Commands(player, npcMsg.ToLower()); // handle all commands, redirecting to properly NPC instance
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    player.SendInfo($"There is no {npcName} found in our database.");
                    return false;
                }
            }

            foreach (ClientData cData in GameServer.Manager.ClientManager.Values)
            {
                if (cData.Client.Account.NameChosen && cData.Client.Account.Name.EqualsIgnoreCase(playername))
                {
                    player.Client.SendMessage(new TEXT()
                    {
                        ObjectId = player.Id,
                        BubbleTime = 10,
                        Stars = player.Stars,
                        Name = player.Name,
                        Admin = 0,
                        Recipient = cData.Client.Account.Name,
                        Text = msg.ToSafeText(),
                        CleanText = "",
                        TextColor = 0x123456,
                        NameColor = 0x123456
                    });

                    cData.Client.SendMessage(new TEXT()
                    {
                        ObjectId = cData.Client.Player.Owner.Id,
                        BubbleTime = 10,
                        Stars = player.Stars,
                        Name = player.Name,
                        Admin = 0,
                        Recipient = cData.Client.Account.Name,
                        Text = msg.ToSafeText(),
                        CleanText = "",
                        TextColor = 0x123456,
                        NameColor = 0x123456
                    });
                    return true;
                }
            }
            player.SendInfo($"{playername} not found.");
            return false;
        }
    }
}