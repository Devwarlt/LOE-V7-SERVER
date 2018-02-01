﻿#region

using common.config;
using System;
using System.Collections.Generic;
using System.Linq;
using gameserver.networking.outgoing;
using gameserver.realm.entity.player;

#endregion

namespace gameserver.realm.commands.gazer
{
    internal static class Gazer_Dictionary
    {
        internal static readonly string BOT_NAME = "Gazer";

        public static readonly Dictionary<Gazer_PacketID, string> PacketID = new Dictionary<Gazer_PacketID, string>
        {
            { Gazer_PacketID.UPTIME, "uptime" },
            { Gazer_PacketID.ONLINE, "online" },
            { Gazer_PacketID.GAZER, "gazer" },
            { Gazer_PacketID.NOTHING, "" }
        };

        public enum Gazer_PacketID : byte
        {
            UPTIME = 0,
            ONLINE = 1,
            GAZER = 254,
            NOTHING = 255
        }

        internal static bool GetType(string command)
        {
            if (PacketID.ContainsValue(command)) return true;
            else return false;
        }

        public static bool HandleCommands(string command, Player player)
        {
            string callback;
            Gazer_PacketID packet = GetType(command) ? PacketID.FirstOrDefault(i => i.Value == command).Key : Gazer_PacketID.NOTHING;
            switch (packet)
            {
                #region "Command: Uptime"
                case Gazer_PacketID.UPTIME:
                    {
                        TimeSpan uptime = DateTime.Now - Program.uptime;
                        double thisUptime = uptime.TotalMinutes;
                        if (thisUptime <= 1)
                            callback = "Server started recently.";
                        else if (thisUptime > 1 && thisUptime <= 59)
                            callback = string.Format("Uptime: {0}{1}{2}{3}.",
                                $"{uptime.Minutes:n0}",
                                (uptime.Minutes >= 1 && uptime.Minutes < 2) ? " minute" : " minutes",
                                uptime.Seconds < 1 ? "" : $" and {uptime.Seconds:n0}",
                                uptime.Seconds < 1 ? "" : (uptime.Seconds >= 1 && uptime.Seconds < 2) ? " second" : " seconds");
                        else
                            callback = string.Format("Uptime: {0}{1}{2}{3}{4}{5}.",
                                $"{uptime.Hours:n0}",
                                (uptime.Hours >= 1 && uptime.Hours < 2) ? " hour" : " hours",
                                uptime.Minutes < 1 ? "" : $", {uptime.Minutes:n0}",
                                uptime.Minutes < 1 ? "" : (uptime.Minutes >= 1 && uptime.Minutes < 2) ? " minute" : " minutes",
                                uptime.Seconds < 1 ? "" : $" and {uptime.Seconds:n0}",
                                uptime.Seconds < 1 ? "" : (uptime.Seconds >= 1 && uptime.Seconds < 2) ? " second" : " seconds");
                    }
                    break;
                #endregion
                #region "Command: Online"
                case Gazer_PacketID.ONLINE:
                    {
                        int serverMaxUsage = Settings.NETWORKING.MAX_CONNECTIONS;
                        int serverCurrentUsage = player.Manager.Clients.Keys.Count;
                        int worldCurrentUsage = player.Owner.Players.Keys.Count;
                        callback = $"Server: {serverCurrentUsage}/{serverMaxUsage} player{(serverCurrentUsage > 1 ? "s" : "")} | {player.Owner.Name}: {worldCurrentUsage} player{(worldCurrentUsage > 1 ? "s" : "")}.";
                    }
                    break;
                #endregion
                #region "Command: Null & Default"
                case Gazer_PacketID.GAZER:
                case Gazer_PacketID.NOTHING:
                default: callback = $"Hi {player.Name}! Sorry, I don't understand. If you need help, then visit me at realmeye.com/mreyeball"; break;
                    #endregion
            }
            player.Client.SendMessage(new TEXT()
            {
                ObjectId = player.Id,
                BubbleTime = 10,
                Stars = player.Stars,
                Name = player.Name,
                Admin = 0,
                Recipient = BOT_NAME,
                Text = command.ToSafeText(),
                CleanText = "",
                TextColor = 0x123456,
                NameColor = 0x123456
            });
            player.Client.SendMessage(new TEXT()
            {
                ObjectId = -1,
                BubbleTime = 10,
                Stars = 70,
                Name = BOT_NAME,
                Admin = 0,
                Recipient = player.Name,
                Text = callback.ToSafeText(),
                CleanText = "",
                TextColor = 0x123456,
                NameColor = 0x123456
            });
            return true;
        }
    }
}
