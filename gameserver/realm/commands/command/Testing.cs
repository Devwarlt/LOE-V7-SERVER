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
                            foreach (Tuple<DateTime, string> messageInfo in ChatManager.ChatDataCache[player.Name])
                                player.SendInfo($"[ChatData] [{messageInfo.Item1}] <{player.Name}> {messageInfo.Item2}");

                        // can cause lag!
                        // returns all chat data
                        if (cmd == "all")
                            foreach (KeyValuePair<string, List<Tuple<DateTime, string>>> messageInfos in ChatManager.ChatDataCache)
                                foreach (Tuple<DateTime, string> messageInfo in ChatManager.ChatDataCache[messageInfos.Key])
                                    player.SendInfo($"[ChatData] [{messageInfo.Item1}] <{messageInfos.Key}> {messageInfo.Item2}");
                    }
                    break;
                default:
                    player.SendHelp("Available testing commands: 'chatdata' (my / all).");
                    break;
            }
            return true;
        }
    }
}