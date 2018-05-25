﻿using LoESoft.GameServer.realm.entity.player;
using LoESoft.Core.config;
using System.Collections.Generic;
using System;
using LoESoft.GameServer.realm.entity;

namespace LoESoft.GameServer.realm.commands
{
    public class TestingCommands : Command
    {
        public TestingCommands() : base("test", (int)AccountType.LOESOFT_ACCOUNT) { }

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

                        if (cmd == "all")
                            foreach (KeyValuePair<string, Tuple<DateTime, string>> messageInfos in ChatManager.ChatDataCache)
                                player.SendInfo($"[ChatData] [{ChatManager.ChatDataCache[messageInfos.Key].Item1}] <{messageInfos.Key}> {ChatManager.ChatDataCache[messageInfos.Key].Item2}");
                    }
                    break;
                case "clients":
                    {
                        foreach (KeyValuePair<string, ClientData> i in GameServer.Manager.ClientManager)
                            player.SendInfo($"[Clients] [ID: {i.Key}] Client '{i.Value.Client.Account.Name}' joined network at {i.Value.Registered}.");
                    }
                    break;
                case "projectiles":
                    {
                        if (cmd == "ids")
                            foreach (KeyValuePair<int, byte> i in player.Owner.Projectiles.Keys)
                                player.SendInfo($"[Projectiles] [Player ID: {i.Key} / Projectile ID: {i.Value}]");

                        if (cmd == "all")
                            foreach (KeyValuePair<KeyValuePair<int, byte>, Projectile> i in player.Owner.Projectiles)
                                player.SendInfo($"[Projectiles] [Player ID: {i.Key.Key} / Projectile ID: {i.Key.Value} / Damage: {i.Value}]");
                    }
                    break;
                case "id":
                    {
                        if (cmd == "mine")
                            player.SendInfo($"Your ID is: {player.Id}");
                    }
                    break;
                default:
                    player.SendHelp("Available testing commands: 'chatdata' (my / all), 'clients', 'projectiles' (ids / all) and 'id' (mine).");
                    break;
            }
            return true;
        }
    }
}