using gameserver.realm.entity.player;
using common.config;
using System.Collections.Generic;
using System;

namespace gameserver.realm.commands
{
    public class TestingCommands : Command
    {
        public TestingCommands() : base("test", (int)accountType.LOESOFT_ACCOUNT) { }

        private readonly bool AllowTestingCommands = true;

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (!AllowTestingCommands)
            {
                player.SendInfo("Testing commands disabled.");
                return false;
            }
            string cmd = string.Join(" ", args, 1, args.Length - 1);
            switch (args[0].Trim())
            {
                case "chatdata":
                    {
                        // returns only your chat data
                        if (cmd == "my")
                            player.SendInfo($"[ChatData] [{ChatManager.ChatDataCache[player.Name].Item1}] <{player.Name}> {ChatManager.ChatDataCache[player.Name].Item2}");

                        // can cause lag!
                        // returns all chat data
                        if (cmd == "all")
                            foreach (KeyValuePair<string, Tuple<DateTime, string>> messageInfos in ChatManager.ChatDataCache)
                                player.SendInfo($"[ChatData] [{ChatManager.ChatDataCache[messageInfos.Key].Item1}] <{messageInfos.Key}> {ChatManager.ChatDataCache[messageInfos.Key].Item2}");
                    }
                    break;
                case "clients":
                    {
                        foreach (KeyValuePair<string, ClientData> i in Program.manager.ClientManager)
                            player.SendInfo($"[Clients] [ID: {i.Key}] Client '{i.Value.client.Account.Name}' joined network at {i.Value.registered}.");
                    }
                    break;
                default:
                    player.SendHelp("Available testing commands: 'chatdata' (my / all) and 'clients'.");
                    break;
            }
            return true;
        }
    }
}