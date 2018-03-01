#region

using LoESoft.Core.config;
using LoESoft.GameServer.realm.entity.player;
using System;

#endregion

namespace LoESoft.GameServer.realm.entity.npc.npcs
{
    public class Gazer : NPC
    {
        public override void Welcome(Player player) => Callback(player, $"Hello {player.Name}! I'm Gazer, how can I help you?");

        public override void Leave(Player player, bool polite) => Callback(player, polite ? _NPCLeaveMessage.Replace("{PLAYER}", player.Name) : "How rude!");

        public override void Commands(Player player, string command)
        {
            string callback = null;
            switch (command)
            {
                #region "Uptime"
                case "uptime":
                    {
                        TimeSpan uptime = DateTime.Now - GameServer.Uptime;
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
                #region "Online"
                case "online":
                    {
                        int serverMaxUsage = Settings.NETWORKING.MAX_CONNECTIONS;
                        int serverCurrentUsage = GameServer.Manager.ClientManager.Count;
                        int worldCurrentUsage = player.Owner.Players.Keys.Count;
                        callback = $"Server: {serverCurrentUsage}/{serverMaxUsage} player{(serverCurrentUsage > 1 ? "s" : "")} | {player.Owner.Name}: {worldCurrentUsage} player{(worldCurrentUsage > 1 ? "s" : "")}.";
                    }
                    break;
                #endregion
                case "hi":
                case "hello":
                case "hey":
                case "good morning":
                case "good afternoon":
                case "good evening":
                    NoRepeat(player);
                    Callback(player, command, false); // player only (self)
                    return;
                case "bye":
                case "goodbye":
                case "goodnight":
                    RemovePlayer(player);
                    Callback(player, command, false); // player only (self)
                    Leave(player, true);
                    return;
                default:
                    callback = "Sorry, I don't understand.";
                    break;
            }
            Callback(player, command, false); // player only (self)
            Callback(player, callback); // to NPC
            ChatManager.ChatDataCache.Remove(player.Name); // Removing player from chat data cache.
        }
    }
}