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

#endregion

namespace LoESoft.GameServer.realm.commands
{
    class ZombifyCommand : Command
    {
        public ZombifyCommand() : base("zombify", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            Entity en = Entity.Resolve("Zombie Wizard");
            en.Move(player.X, player.Y);
            player.Owner.EnterWorld(en);
            player.UpdateCount++;
            return true;
        }
    }

    class PosCmd : Command
    {
        public PosCmd() : base("p", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            player.SendInfo("X: " + (int)player.X + " - Y: " + (int)player.Y);
            return true;
        }
    }

    class AddRealmCommand : Command
    {
        public AddRealmCommand() : base("addrealm", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            Task.Factory.StartNew(() => GameWorld.AutoName(1, true)).ContinueWith(_ => Program.Manager.AddWorld(_.Result), TaskScheduler.Default);
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
                    Program.Manager.GameData.IdToObjectType,
                    StringComparer.OrdinalIgnoreCase);
                if (!icdatas.TryGetValue(name, out ushort objType) ||
                    !Program.Manager.GameData.ObjectDescs.ContainsKey(objType))
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
                    Program.Manager.GameData.IdToObjectType,
                    StringComparer.OrdinalIgnoreCase);
                if (!icdatas.TryGetValue(name, out ushort objType) ||
                    !Program.Manager.GameData.ObjectDescs.ContainsKey(objType))
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

    class AddEffCommand : Command
    {
        public AddEffCommand() : base("addeff", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /addeff <Effectname or Effectnumber>");
                return false;
            }
            try
            {
                player.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = (ConditionEffectIndex)Enum.Parse(typeof(ConditionEffectIndex), args[0].Trim(), true),
                    DurationMS = -1
                });
                {
                    player.SendInfo("Success!");
                }
            }
            catch
            {
                player.SendError("Invalid effect!");
                return false;
            }
            return true;
        }
    }

    class RemoveEffCommand : Command
    {
        public RemoveEffCommand() : base("remeff", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /remeff <Effectname or Effectnumber>");
                return false;
            }
            try
            {
                player.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = (ConditionEffectIndex)Enum.Parse(typeof(ConditionEffectIndex), args[0].Trim(), true),
                    DurationMS = 0
                });
                player.SendInfo("Success!");
            }
            catch
            {
                player.SendError("Invalid effect!");
                return false;
            }
            return true;
        }
    }

    class GiveCommand : Command
    {
        public GiveCommand() : base("give") { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /give <Itemname>");
                return false;
            }
            string name = string.Join(" ", args.ToArray()).Trim();
            Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(Program.Manager.GameData.IdToObjectType,
                StringComparer.OrdinalIgnoreCase);
            if (!icdatas.TryGetValue(name, out ushort objType))
            {
                player.SendError("Unknown type!");
                return false;
            }
            if (!Program.Manager.GameData.Items[objType].Secret || player.Client.Account.Admin)
            {
                for (int i = 4; i < player.Inventory.Length; i++)
                    if (player.Inventory[i] == null)
                    {
                        player.Inventory[i] = Program.Manager.GameData.Items[objType];
                        player.UpdateCount++;
                        player.SaveToCharacter();
                        player.SendInfo("Success!");
                        break;
                    }
            }
            else
            {
                player.SendError("Item cannot be given!");
                return false;
            }
            return true;
        }
    }

    class TpCommand : Command
    {
        public TpCommand() : base("tp", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0 || args.Length == 1)
            {
                player.SendHelp("Usage: /tp <X coordinate> <Y coordinate>");
            }
            else
            {
                int x, y;
                try
                {
                    x = int.Parse(args[0]);
                    y = int.Parse(args[1]);
                }
                catch
                {
                    player.SendError("Invalid coordinates!");
                    return false;
                }
                player.Move(x + 0.5f, y + 0.5f);
                player.UpdateCount++;
                player.Owner.BroadcastPacket(new GOTO
                {
                    ObjectId = player.Id,
                    Position = new Position
                    {
                        X = player.X,
                        Y = player.Y
                    }
                }, null);
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
                        Program.Manager.TryDisconnect(i.Value.Client, DisconnectReason.PLAYER_KICK);
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

    class Max : Command
    {
        public Max() : base("max", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            try
            {
                player.Stats[0] = player.ObjectDesc.MaxHitPoints;
                player.Stats[1] = player.ObjectDesc.MaxMagicPoints;
                player.Stats[2] = player.ObjectDesc.MaxAttack;
                player.Stats[3] = player.ObjectDesc.MaxDefense;
                player.Stats[4] = player.ObjectDesc.MaxSpeed;
                player.Stats[5] = player.ObjectDesc.MaxHpRegen;
                player.Stats[6] = player.ObjectDesc.MaxMpRegen;
                player.Stats[7] = player.ObjectDesc.MaxDexterity;
                player.SaveToCharacter();
                player.UpdateCount++;
                player.SendInfo("Success");
            }
            catch
            {
                player.SendError("Error while maxing stats");
                return false;
            }
            return true;
        }
    }

    class OryxSay : Command
    {
        public OryxSay() : base("osay", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /oryxsay <saytext>");
                return false;
            }
            string saytext = string.Join(" ", args);
            player.SendEnemy("Oryx the Mad God", saytext);
            return true;
        }
    }

    class OnlineCommand : Command //get all players from all worlds (this may become too large!)
    {
        public OnlineCommand() : base("online", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            StringBuilder sb = new StringBuilder("Online at this moment: ");

            foreach (KeyValuePair<int, World> w in Program.Manager.Worlds)
            {
                World world = w.Value;
                if (w.Key != 0)
                {
                    Player[] copy = world.Players.Values.ToArray();
                    if (copy.Length != 0)
                    {
                        for (int i = 0; i < copy.Length; i++)
                        {
                            sb.Append(copy[i].Name);
                            sb.Append(", ");
                        }
                    }
                }
            }
            string fixedString = sb.ToString().TrimEnd(',', ' '); //clean up trailing ", "s

            player.SendInfo(fixedString);
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

            foreach (ClientData cData in Program.Manager.ClientManager.Values)
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
            foreach (ClientData cData in Program.Manager.ClientManager.Values)
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
            foreach (KeyValuePair<int, World> w in Program.Manager.Worlds)
            {
                World world = w.Value;
                if (w.Key != 0)
                    world.BroadcastPacket(new TEXT
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

            Program.ForceShutdown();

            return true;
        }
    }

    class TqCommand : Command
    {
        public TqCommand() : base("tq", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (player.Quest == null)
            {
                player.SendInfo("There is no quest to teleport!");
                return false;
            }
            player.Move(player.Quest.X + 0.5f, player.Quest.Y + 0.5f);
            player.UpdateCount++;
            player.Owner.BroadcastPacket(new GOTO
            {
                ObjectId = player.Id,
                Position = new Position
                {
                    X = player.Quest.X,
                    Y = player.Quest.Y
                }
            }, null);
            player.SendInfo("Success!");
            return true;
        }
    }

    class LevelCommand : Command
    {
        public LevelCommand() : base("level", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    player.SendHelp("Use /level <ammount>");
                    return false;
                }
                if (args.Length == 1)
                {
                    player.Client.Character.Level = (int.Parse(args[0]) >= 1 && int.Parse(args[0]) <= 20) ? int.Parse(args[0]) : player.Client.Character.Level;
                    player.Client.Player.Level = (int.Parse(args[0]) >= 1 && int.Parse(args[0]) <= 20) ? int.Parse(args[0]) : player.Client.Player.Level;
                    player.UpdateCount++;
                    player.SendInfo(string.Format("Success! Level changed from level {0} to level {1}.",
                        player.Client.Player.Level, (int.Parse(args[0]) >= 1 && int.Parse(args[0]) <= 20) ? int.Parse(args[0]) : player.Client.Player.Level));
                }
            }
            catch
            {
                player.SendError("Error!");
                return false;
            }
            return true;
        }
    }

    class SetCommand : Command
    {
        public SetCommand() : base("setStat", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 2)
            {
                try
                {
                    string stat = args[0].ToLower();
                    int amount = int.Parse(args[1]);
                    switch (stat)
                    {
                        case "health":
                        case "hp":
                            player.Stats[0] = amount;
                            break;
                        case "mana":
                        case "mp":
                            player.Stats[1] = amount;
                            break;
                        case "att":
                        case "atk":
                        case "attack":
                            player.Stats[2] = amount;
                            break;
                        case "def":
                        case "defence":
                            player.Stats[3] = amount;
                            break;
                        case "spd":
                        case "speed":
                            player.Stats[4] = amount;
                            break;
                        case "vit":
                        case "vitality":
                            player.Stats[5] = amount;
                            break;
                        case "wis":
                        case "wisdom":
                            player.Stats[6] = amount;
                            break;
                        case "dex":
                        case "dexterity":
                            player.Stats[7] = amount;
                            break;
                        default:
                            player.SendError("Invalid Stat");
                            player.SendHelp("Stats: Health, Mana, Attack, Defence, Speed, Vitality, Wisdom, Dexterity");
                            player.SendHelp("Shortcuts: Hp, Mp, Atk, Def, Spd, Vit, Wis, Dex");
                            return false;
                    }
                    player.SaveToCharacter();
                    player.UpdateCount++;
                    player.SendInfo("Success");
                }
                catch
                {
                    player.SendError("Error while setting stat");
                    return false;
                }
                return true;
            }
            else if (args.Length == 3)
            {
                foreach (ClientData cData in Program.Manager.ClientManager.Values)
                {
                    if (cData.Client.Account.Name.EqualsIgnoreCase(args[0]))
                    {
                        try
                        {
                            string stat = args[1].ToLower();
                            int amount = int.Parse(args[2]);
                            switch (stat)
                            {
                                case "health":
                                case "hp":
                                    cData.Client.Player.Stats[0] = amount;
                                    break;
                                case "mana":
                                case "mp":
                                    cData.Client.Player.Stats[1] = amount;
                                    break;
                                case "att":
                                case "atk":
                                case "attack":
                                    cData.Client.Player.Stats[2] = amount;
                                    break;
                                case "def":
                                case "defence":
                                    cData.Client.Player.Stats[3] = amount;
                                    break;
                                case "spd":
                                case "speed":
                                    cData.Client.Player.Stats[4] = amount;
                                    break;
                                case "vit":
                                case "vitality":
                                    cData.Client.Player.Stats[5] = amount;
                                    break;
                                case "wis":
                                case "wisdom":
                                    cData.Client.Player.Stats[6] = amount;
                                    break;
                                case "dex":
                                case "dexterity":
                                    cData.Client.Player.Stats[7] = amount;
                                    break;
                                default:
                                    player.SendError("Invalid Stat");
                                    player.SendHelp("Stats: Health, Mana, Attack, Defence, Speed, Vitality, Wisdom, Dexterity");
                                    player.SendHelp("Shortcuts: Hp, Mp, Atk, Def, Spd, Vit, Wis, Dex");
                                    return false;
                            }
                            cData.Client.Player.SaveToCharacter();
                            cData.Client.Player.UpdateCount++;
                            player.SendInfo("Success");
                        }
                        catch
                        {
                            player.SendError("Error while setting stat");
                            return false;
                        }
                        return true;
                    }
                }
                player.SendError(string.Format("Player '{0}' could not be found!", args));
                return false;
            }
            else
            {
                player.SendHelp("Usage: /setStat <stat> <amount>");
                player.SendHelp("or");
                player.SendHelp("Usage: /setStat <player> <stat> <amount>");
                player.SendHelp("Shortcuts: Hp, Mp, Atk, Def, Spd, Vit, Wis, Dex");
                return false;
            }
        }
    }

    class SetpieceCommand : Command
    {
        public SetpieceCommand() : base("setpiece", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            try
            {
                MapSetPiece piece = (MapSetPiece)Activator.CreateInstance(System.Type.GetType(
                    "LoESoft.GameServer.realm.mapsetpieces." + args[0], true, true));
                piece.RenderSetPiece(player.Owner, new IntPoint((int)player.X + 1, (int)player.Y + 1));
                return true;
            }
            catch
            {
                player.SendError("Invalid SetPiece.");
                return false;
            }

        }
    }

    class ListCommands : Command
    {
        public ListCommands() : base("commands", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            Dictionary<string, Command> cmds = new Dictionary<string, Command>();
            System.Type t = typeof(Command);
            foreach (System.Type i in t.Assembly.GetTypes())
                if (t.IsAssignableFrom(i) && i != t)
                {
                    Command instance = (Command)Activator.CreateInstance(i);
                    cmds.Add(instance.CommandName, instance);
                }
            StringBuilder sb = new StringBuilder("");
            Command[] copy = cmds.Values.ToArray();
            for (int i = 0; i < copy.Length; i++)
            {
                if (i != 0) sb.Append(", ");
                sb.Append(copy[i].CommandName);
            }

            player.SendInfo(sb.ToString());
            return true;
        }
    }

    class Mute : Command
    {
        public Mute() : base("mute", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            try
            {
                foreach (KeyValuePair<int, Player> i in player.Owner.Players)
                {
                    if (i.Value.Name.ToLower() == args[0].ToLower().Trim())
                    {
                        i.Value.Muted = true;
                        i.Value.Client.Manager.Database.MuteAccount(i.Value.Client.Account);
                        player.SendInfo("Player Muted.");
                    }
                }
            }
            catch
            {
                player.SendError("Cannot mute!");
                return false;
            }
            return true;
        }
    }

    class Unmute : Command
    {
        public Unmute() : base("unmute", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            try
            {
                foreach (KeyValuePair<int, Player> i in player.Owner.Players)
                {
                    if (i.Value.Name.ToLower() == args[0].ToLower().Trim())
                    {
                        i.Value.Muted = false;
                        i.Value.Client.Manager.Database.UnmuteAccount(i.Value.Client.Account);
                        player.SendInfo("Player Unmuted.");
                    }
                }
            }
            catch
            {
                player.SendError("Cannot unmute!");
                return false;
            }
            return true;
        }
    }

    class BanCommand : Command
    {
        public BanCommand() : base("ban", (int)AccountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            try
            {
                Player p = Program.Manager.FindPlayer(args[0]);
                if (p == null)
                {
                    player.SendError("Player not found");
                    return false;
                }
                p.Client.Manager.Database.BanAccount(p.Client.Account);
                Program.Manager.TryDisconnect(p.Client, DisconnectReason.PLAYER_BANNED);
                return true;
            }
            catch
            {
                player.SendError("Cannot ban!");
                return false;
            }
        }
    }
}