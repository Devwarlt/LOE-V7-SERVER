#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoESoft.GameServer.networking.outgoing;
using LoESoft.GameServer.realm.entity.player;
using LoESoft.GameServer.realm.mapsetpiece;
using LoESoft.GameServer.realm.world;
using LoESoft.Core.config;
using static LoESoft.GameServer.networking.Client;
using System.Threading;
using LoESoft.GameServer.networking;

#endregion

namespace LoESoft.GameServer.realm.commands
{
    class VisitCommand : Command
    {
        public VisitCommand() : base("visit", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length < 1)
            {
                player.SendHelp("Usage: /visit <player>");
                return false;
            }

            foreach (ClientData i in GameServer.Manager.ClientManager.Values)
            {
                Player target = i.Client.Player;

                if (target.Owner is Vault)
                {
                    player.SendInfo($"Player {target.Name} is at Vault.");
                    return false;
                }

                if (target.Name.EqualsIgnoreCase(args[0]))
                {
                    if (player.Owner == target.Owner)
                    {
                        player.SendInfo($"You are already in the same world as this player {target.Name}.");
                        return false;
                    }

                    player.Client.Reconnect(new RECONNECT
                    {
                        GameId = target.Owner.Id,
                        Host = "",
                        IsFromArena = false,
                        Key = target.Owner.PortalKey,
                        KeyTime = -1,
                        Name = target.Owner.Name,
                        Port = -1
                    });
                    return true;
                }
            }

            player.SendError($"An error occurred: player {args[0]} couldn't be found.");
            return false;
        }
    }

    class Summon : Command
    {
        public Summon()
            : base("summon", (int)AccountType.LOESOFT_ACCOUNT)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (player.Owner is Vault)
            {
                player.SendInfo($"You cant summon player {args[0]} in your vault.");
                return false;
            }

            foreach (ClientData i in GameServer.Manager.ClientManager.Values)
            {
                Player target = i.Client.Player;

                if (target.Name.EqualsIgnoreCase(args[0]))
                {
                    Message msg;

                    if (target.Owner == player.Owner)
                    {
                        target.Move(player.X, player.Y);

                        msg = new GOTO
                        {
                            ObjectId = target.Id,
                            Position = new Position(player.X, player.Y)
                        };

                        target.UpdateCount++;

                        player.SendInfo($"Player {target.Name} was moved to near you.");
                    }
                    else
                    {
                        msg = new RECONNECT
                        {
                            GameId = player.Owner.Id,
                            Host = "",
                            IsFromArena = false,
                            Key = player.Owner.PortalKey,
                            KeyTime = -1,
                            Name = player.Owner.Name,
                            Port = -1
                        };

                        player.SendInfo($"Player {target.Name} is connecting to {player.Owner.Name}.");
                    }

                    i.Client.SendMessage(msg);

                    return true;
                }
            }

            player.SendError($"An error occurred: player '{args[0]}' couldn't be found.");
            return false;
        }
    }

    class PosCmd : Command
    {
        public PosCmd() : base("pos", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            player.SendInfo("X: " + (int)player.X + " - Y: " + (int)player.Y);
            return true;
        }
    }

    class SpawnCommand : Command
    {
        public SpawnCommand() : base("spawn", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length > 0 && int.TryParse(args[0], out int num)) //multi
            {
                string name = string.Join(" ", args.Skip(1).ToArray());
                //creates a new case insensitive dictionary based on the XmlDatas
                Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(
                    GameServer.Manager.GameData.IdToObjectType,
                    StringComparer.OrdinalIgnoreCase);
                if (!icdatas.TryGetValue(name, out ushort objType) ||
                    !GameServer.Manager.GameData.ObjectDescs.ContainsKey(objType))
                {
                    player.SendInfo("Unknown entity!");
                    return false;
                }
                int c = int.Parse(args[0]);
                for (int i = 0; i < num; i++)
                {
                    Entity entity = Entity.Resolve(objType);
                    entity.Move(player.X, player.Y);
                    player.Owner.EnterWorld(entity);
                }
                player.SendInfo("Success!");
            }
            else
            {
                string name = string.Join(" ", args);
                //creates a new case insensitive dictionary based on the XmlDatas
                Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(
                    GameServer.Manager.GameData.IdToObjectType,
                    StringComparer.OrdinalIgnoreCase);
                if (!icdatas.TryGetValue(name, out ushort objType) ||
                    !GameServer.Manager.GameData.ObjectDescs.ContainsKey(objType))
                {
                    player.SendHelp("Usage: /spawn <entityname>");
                    return false;
                }
                Entity entity = Entity.Resolve(objType);
                entity.Move(player.X, player.Y);
                player.Owner.EnterWorld(entity);
            }
            return true;
        }
    }

    class GiveCommand : Command
    {
        public GiveCommand() : base("give", (int)AccountType.LOESOFT_ACCOUNT) { }

        private List<string> Blacklist = new List<string>
        {
            "admin sword", "admin wand", "admin staff", "admin dagger", "admin bow", "admin katana", "crown",
            "public arena key"
        };

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /give <item name>");
                return false;
            }

            string name = string.Join(" ", args.ToArray()).Trim();

            if (Blacklist.Contains(name.ToLower()) && player.AccountType != (int)AccountType.LOESOFT_ACCOUNT)
            {
                player.SendHelp($"You cannot give '{name}', access denied.");
                return false;
            }

            try
            {
                Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(GameServer.Manager.GameData.IdToObjectType, StringComparer.OrdinalIgnoreCase);

                if (!icdatas.TryGetValue(name, out ushort objType))
                {
                    player.SendError("Unknown type!");
                    return false;
                }

                if (!GameServer.Manager.GameData.Items[objType].Secret || player.Client.Account.Admin)
                {
                    for (int i = 4; i < player.Inventory.Length; i++)
                        if (player.Inventory[i] == null)
                        {
                            player.Inventory[i] = GameServer.Manager.GameData.Items[objType];
                            player.UpdateCount++;
                            player.SaveToCharacter();
                            player.SendInfo("Success!");
                            break;
                        }
                }
                else
                {
                    player.SendError("An error occurred: inventory out of space, item cannot be given.");
                    return false;
                }
            }
            catch (KeyNotFoundException)
            {
                player.SendError($"An error occurred: item '{name}' doesn't exist in game assets.");
                return false;
            }

            return true;
        }
    }

    class KillAll : Command
    {
        public KillAll() : base("killAll", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            var iterations = 0;
            var lastKilled = -1;
            var killed = 0;

            var mobName = args.Aggregate((s, a) => string.Concat(s, " ", a));
            while (killed != lastKilled)
            {
                lastKilled = killed;
                foreach (var i in player.Owner.Enemies.Values
                    .Where(e => e.ObjectDesc.ObjectId != null && e.ObjectDesc.ObjectId.ContainsIgnoreCase(mobName) && !e.IsPet && e.ObjectDesc.Enemy))
                {
                    i.CheckDeath = true;
                    killed++;
                }
                if (++iterations >= 5)
                    break;
            }

            player.SendInfo($"{killed} enemy killed!");
            return true;
        }
    }

    class Kick : Command
    {
        public Kick() : base("kick", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /kick <playername>");
                return false;
            }
            try
            {
                foreach (KeyValuePair<int, Player> i in player.Owner.Players)
                {
                    if (i.Value.Name.ToLower() == args[0].ToLower().Trim())
                    {
                        player.SendInfo($"Player {i.Value.Name} has been disconnected!");
                        GameServer.Manager.TryDisconnect(i.Value.Client, DisconnectReason.PLAYER_KICK);
                    }
                }
            }
            catch
            {
                player.SendError("Cannot kick!");
                return false;
            }
            return true;
        }
    }

    class Announcement : Command
    {
        public Announcement() : base("announce", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /announce <saytext>");
                return false;
            }
            string saytext = string.Join(" ", args);

            foreach (ClientData cData in GameServer.Manager.ClientManager.Values)
            {
                cData.Client.SendMessage(new TEXT
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "@ANNOUNCEMENT",
                    Text = " " + saytext,
                    NameColor = 0x123456,
                    TextColor = 0x123456
                });
            }
            return true;
        }
    }

    class KillPlayerCommand : Command
    {
        public KillPlayerCommand() : base("kill", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            foreach (ClientData cData in GameServer.Manager.ClientManager.Values)
            {
                if (cData.Client.Account.Name.EqualsIgnoreCase(args[0]))
                {
                    cData.Client.Player.HP = 0;
                    cData.Client.Player.Death("server.game_admin");
                    player.SendInfo($"Player {cData.Client.Account.Name} has been killed!");
                    return true;
                }
            }
            player.SendInfo(string.Format("Player '{0}' could not be found!", args));
            return false;
        }
    }

    class RestartCommand : Command
    {
        public RestartCommand() : base("restart", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            foreach (KeyValuePair<int, World> w in GameServer.Manager.Worlds)
            {
                World world = w.Value;
                if (w.Key != 0)
                    world.BroadcastMessage(new TEXT
                    {
                        Name = "@ANNOUNCEMENT",
                        Stars = -1,
                        BubbleTime = 0,
                        Text = "Server restarting soon. Please be ready to disconnect.",
                        NameColor = 0x123456,
                        TextColor = 0x123456
                    }, null);
            }

            Thread.Sleep(4000);

            GameServer.ForceShutdown();

            return true;
        }
    }
}