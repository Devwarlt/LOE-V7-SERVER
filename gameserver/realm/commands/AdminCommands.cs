﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gameserver.networking;
using gameserver.networking.outgoing;
using gameserver.realm.entity.player;
using gameserver.realm.mapsetpiece;
using gameserver.realm.world;
using common.config;
using static gameserver.networking.Client;
using System.Threading;

#endregion

namespace gameserver.realm.commands
{
	internal class GlandCommand : Command
	{
		public GlandCommand()
			: base("glands", (int)accountType.FREE_ACCOUNT)
		{
		}

		protected override bool Process(Player player, RealmTime time, string[] args)
		{
			if (args.Length == 1000 || args.Length == 1000)
			{
				player.SendHelp("Usage: /glands to tp to glands");
			}
			else
			{
				int x, y;
				try
				{
					x = int.Parse("1000");
					y = int.Parse("1000");
				}
				catch
				{
					player.SendError("Invalid coordinates!");
					return false;
				}
				player.Move(x + 0.5f, y + 0.5f);
				if (player.Pet != null)
					player.Pet.Move(x + 0.5f, y + 0.5f);
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
	internal class GlobalChatCommand : Command
	{
		public GlobalChatCommand()
			: base("global", (int)accountType.FREE_ACCOUNT)
		{
		}

		protected override bool Process(Player player, RealmTime time, string[] args)
		{
			if (args.Length == 0)
			{
				player.SendInfo("Usage: /global <saytext>");
				return false;
			}
			string saytext = string.Join(" ", args);

			foreach (Client i in player.Manager.Clients.Values)

			{
				i.SendMessage(new TEXT
				{
					BubbleTime = 5,
					Stars = player.Stars,
					Name = "[Global]",
					Text = " " + saytext,
					TextColor = 0xffffff,
					NameColor = 0xffffff
				});
			}
			return true;
		}
	}
	internal class SpawnOnCommand : Command
	{
		public SpawnOnCommand()
			: base("spawnon", (int)accountType.LOESOFT_ACCOUNT)
		{
		}


		protected override bool Process(Player player, RealmTime time, string[] args)
		{
			if (string.IsNullOrEmpty(args[0]))
			{
				player.SendHelp("Usage: /spawnon <Player> <Int> <Entity>");
				return false;
			}


			foreach (Client i in player.Manager.Clients.Values)
			{


				if (i.Account.Name.EqualsIgnoreCase(args[0]))
				{
					int num;
					if (args[1].Length > 0 && int.TryParse(args[1], out num)) //multi
					{
						string name = string.Join(" ", args.Skip(2).ToArray());
						ushort objType;
						Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(
							player.Manager.GameData.IdToObjectType,
							StringComparer.OrdinalIgnoreCase);
						if (!icdatas.TryGetValue(name, out objType) ||
							!player.Manager.GameData.ObjectDescs.ContainsKey(objType))
						{
							player.SendInfo("Unknown entity!");
							return false;
						}
						else
						{
							for (int u = 0; u < num; u++)
							{
								Entity entity = Entity.Resolve(player.Manager, objType);
								entity.Move(i.Player.X, i.Player.Y);
								i.Player.Owner.EnterWorld(entity);
							}
							player.SendInfo("Sent " + i.Player.Name + " " + string.Join(" ", args.Skip(1).ToArray()) + "'s");
							return true;
						}
					}
				}
				else
				{
					if (i.Account.Name.EqualsIgnoreCase(args[0]))
					{
						{
							string name = string.Join(" ", args.Skip(1).ToArray());
							ushort objType;
							Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(
								player.Manager.GameData.IdToObjectType,
								StringComparer.OrdinalIgnoreCase);
							if (!icdatas.TryGetValue(name, out objType) ||
								!player.Manager.GameData.ObjectDescs.ContainsKey(objType))
							{ }
							Entity entity = Entity.Resolve(player.Manager, objType);
							entity.Move(i.Player.X, i.Player.Y);
							i.Player.Owner.EnterWorld(entity);
						}
						player.SendInfo("Sent " + i.Player.Name + " " + string.Join(" ", args.Skip(1).ToArray()));
						return true;
					}
				}
			}
			return false;
		}

	}
	internal class RogueCommand : Command
	{
		public RogueCommand() : base("rogue", (int)accountType.VIP_ACCOUNT)
		{
		}
		protected override bool Process(Player player, RealmTime time, string[] args)
		{
			for (int i = 0; i < player.Inventory.Length; i++)
			{
				player.Inventory[4] = player.Manager.GameData.Items[0x9c6]; // Sinister Deed
				player.Inventory[5] = player.Manager.GameData.Items[0xb27]; // GhostlyConcealment
				player.Inventory[6] = player.Manager.GameData.Items[0x9c1]; // Wyrmhide
				player.Inventory[7] = player.Manager.GameData.Items[0xba9]; // UBHP
				player.UpdateCount++;
			}
			player.SendInfo("Set Given");
			return true;
		}
	}
	internal class ArcherCommand : Command
	{
		public ArcherCommand() : base("archer", (int)accountType.VIP_ACCOUNT)
		{
		}
		protected override bool Process(Player player, RealmTime time, string[] args)
		{
			for (int i = 0; i < player.Inventory.Length; i++)
			{
				player.Inventory[4] = player.Manager.GameData.Items[0x9cc]; // Mystical Energy
				player.Inventory[5] = player.Manager.GameData.Items[0xb28]; // Elvish Mastery
				player.Inventory[6] = player.Manager.GameData.Items[0x9c1]; // Wyrmhide
				player.Inventory[7] = player.Manager.GameData.Items[0xba9]; // UBHP
				player.UpdateCount++;
			}
			player.SendInfo("Set Given");
			return true;
		}
	}
	internal class WizardCommand : Command
	{
		public WizardCommand() : base("wizard", (int)accountType.VIP_ACCOUNT)
		{
		}
		protected override bool Process(Player player, RealmTime time, string[] args)
		{
			for (int i = 0; i < player.Inventory.Length; i++)
			{
				player.Inventory[4] = player.Manager.GameData.Items[0x9d1]; // Vital Unity
				player.Inventory[5] = player.Manager.GameData.Items[0xb24]; // Elemental Denotation
				player.Inventory[6] = player.Manager.GameData.Items[0x9cf]; // Star Mother
				player.Inventory[7] = player.Manager.GameData.Items[0xba9]; // UBHP
				player.UpdateCount++;
			}
			player.SendInfo("Set Given");
			return true;
		}
	}
	internal class PriestCommand : Command
	{
		public PriestCommand() : base("priest", (int)accountType.VIP_ACCOUNT)
		{
		}
		protected override bool Process(Player player, RealmTime time, string[] args)
		{
			for (int i = 0; i < player.Inventory.Length; i++)
			{
				player.Inventory[4] = player.Manager.GameData.Items[0x9ca]; // Retribution
				player.Inventory[5] = player.Manager.GameData.Items[0xb25]; // T6 Tome
				player.Inventory[6] = player.Manager.GameData.Items[0x9cf]; // Star Mother
				player.Inventory[7] = player.Manager.GameData.Items[0xba9]; // UBHP
				player.UpdateCount++;
			}
			player.SendInfo("Set Given");
			return true;
		}
	}
	internal class WarriorCommand : Command
	{
		public WarriorCommand() : base("warrior", (int)accountType.VIP_ACCOUNT)
		{
		}
		protected override bool Process(Player player, RealmTime time, string[] args)
		{
			for (int i = 0; i < player.Inventory.Length; i++)
			{
				player.Inventory[4] = player.Manager.GameData.Items[0x9c8]; // Splendor
				player.Inventory[5] = player.Manager.GameData.Items[0xb29]; // Great General Helm
				player.Inventory[6] = player.Manager.GameData.Items[0x9c4]; // Dominion
				player.Inventory[7] = player.Manager.GameData.Items[0xba9]; // UBHP
				player.UpdateCount++;
			}
			player.SendInfo("Set Given");
			return true;
		}
	}
	internal class KnightCommand : Command
	{
		public KnightCommand() : base("knight", (int)accountType.VIP_ACCOUNT)
		{
		}
		protected override bool Process(Player player, RealmTime time, string[] args)
		{
			for (int i = 0; i < player.Inventory.Length; i++)
			{
				player.Inventory[4] = player.Manager.GameData.Items[0x9c8]; // Splendor
				player.Inventory[5] = player.Manager.GameData.Items[0xb22]; // Colossus Shield
				player.Inventory[6] = player.Manager.GameData.Items[0x9c4]; // Dominion
				player.Inventory[7] = player.Manager.GameData.Items[0xba9]; // UBHP
				player.UpdateCount++;
			}
			player.SendInfo("Set Given");
			return true;
		}
	}
	internal class PaladinCommand : Command
	{
		public PaladinCommand() : base("paladin", (int)accountType.VIP_ACCOUNT)
		{
		}
		protected override bool Process(Player player, RealmTime time, string[] args)
		{
			for (int i = 0; i < player.Inventory.Length; i++)
			{
				player.Inventory[4] = player.Manager.GameData.Items[0x9c8]; // Splendor
				player.Inventory[5] = player.Manager.GameData.Items[0xb26]; // T6 Seal
				player.Inventory[6] = player.Manager.GameData.Items[0x9c4]; // Dominion
				player.Inventory[7] = player.Manager.GameData.Items[0xba9]; // UBHP
				player.UpdateCount++;
			}
			player.SendInfo("Set Given");
			return true;
		}
	}
	internal class AssassinCommand : Command
	{
		public AssassinCommand() : base("assassin", (int)accountType.VIP_ACCOUNT)
		{
		}
		protected override bool Process(Player player, RealmTime time, string[] args)
		{
			for (int i = 0; i < player.Inventory.Length; i++)
			{
				player.Inventory[4] = player.Manager.GameData.Items[0x9c6]; // Sinister Deed
				player.Inventory[5] = player.Manager.GameData.Items[0xb2a]; // Baneserpent
				player.Inventory[6] = player.Manager.GameData.Items[0x9c1]; // Wyrmhide
				player.Inventory[7] = player.Manager.GameData.Items[0xba9]; // UBHP
				player.UpdateCount++;
			}
			player.SendInfo("Set Given");
			return true;
		}
	}
	internal class NecromanserCommand : Command
	{
		public NecromanserCommand() : base("necromanser", (int)accountType.VIP_ACCOUNT)
		{
		}
		protected override bool Process(Player player, RealmTime time, string[] args)
		{
			for (int i = 0; i < player.Inventory.Length; i++)
			{
				player.Inventory[4] = player.Manager.GameData.Items[0x9d1]; // Vital Unity
				player.Inventory[5] = player.Manager.GameData.Items[0xb2b]; // Bloodsucker
				player.Inventory[6] = player.Manager.GameData.Items[0x9cf]; // Star Mother
				player.Inventory[7] = player.Manager.GameData.Items[0xba9]; // UBHP
				player.UpdateCount++;
			}
			player.SendInfo("Set Given");
			return true;
		}
	}
	internal class HuntressCommand : Command
	{
		public HuntressCommand() : base("huntress", (int)accountType.VIP_ACCOUNT)
		{
		}
		protected override bool Process(Player player, RealmTime time, string[] args)
		{
			for (int i = 0; i < player.Inventory.Length; i++)
			{
				player.Inventory[4] = player.Manager.GameData.Items[0x9cc]; // Mystical Energy
				player.Inventory[5] = player.Manager.GameData.Items[0xb2c]; // giantcatcher
				player.Inventory[6] = player.Manager.GameData.Items[0x9c1]; // Wyrmhide
				player.Inventory[7] = player.Manager.GameData.Items[0xba9]; // UBHP
				player.UpdateCount++;
			}
			player.SendInfo("Set Given");
			return true;
		}
	}
	internal class MysticCommand : Command
	{
		public MysticCommand() :
			base("mystic", (int)accountType.VIP_ACCOUNT)
		{
		}
		protected override bool Process(Player player, RealmTime time, string[] args)
		{
			for (int i = 0; i < player.Inventory.Length; i++)
			{
				player.Inventory[4] = player.Manager.GameData.Items[0x9d1]; // Vital Unity
				player.Inventory[5] = player.Manager.GameData.Items[0xb2d]; // plantfetter Orb
				player.Inventory[6] = player.Manager.GameData.Items[0x9cf]; // Star Mother
				player.Inventory[7] = player.Manager.GameData.Items[0xba9]; // UBHP
				player.UpdateCount++;
			}
			player.SendInfo("Set Given");
			return true;
		}
	}
	internal class TricksterCommand : Command
	{
		public TricksterCommand() : base("trickster", (int)accountType.VIP_ACCOUNT)
		{
		}
		protected override bool Process(Player player, RealmTime time, string[] args)
		{
			for (int i = 0; i < player.Inventory.Length; i++)
			{
				player.Inventory[4] = player.Manager.GameData.Items[0x9c6]; // Sinister Deed
				player.Inventory[5] = player.Manager.GameData.Items[0xb23]; // Apparitions
				player.Inventory[6] = player.Manager.GameData.Items[0x9c1]; // Wyrmhide
				player.Inventory[7] = player.Manager.GameData.Items[0xba9]; // UBHP
				player.UpdateCount++;
			}
			player.SendInfo("Set Given");
			return true;
		}
	}
	internal class SorcererCommand : Command
	{
		public SorcererCommand() : base("Sorcerer", (int)accountType.VIP_ACCOUNT)
		{
		}
		protected override bool Process(Player player, RealmTime time, string[] args)
		{
			for (int i = 0; i < player.Inventory.Length; i++)
			{
				player.Inventory[4] = player.Manager.GameData.Items[0x9ca]; // Retribution
				player.Inventory[5] = player.Manager.GameData.Items[0xb33]; // storm
				player.Inventory[6] = player.Manager.GameData.Items[0x9cf]; // Star Mother
				player.Inventory[7] = player.Manager.GameData.Items[0xba9]; // UBHP
				player.UpdateCount++;
			}
			player.SendInfo("Set Given");
			return true;
		}
	}
	internal class NinjaCommand : Command
	{
		public NinjaCommand() : base("ninja", (int)accountType.VIP_ACCOUNT)
		{
		}
		protected override bool Process(Player player, RealmTime time, string[] args)
		{
			for (int i = 0; i < player.Inventory.Length; i++)
			{
				player.Inventory[4] = player.Manager.GameData.Items[0xc4f]; // Muramasa
				player.Inventory[5] = player.Manager.GameData.Items[0xc59]; // doom circle
				player.Inventory[6] = player.Manager.GameData.Items[0x9c1]; // Wyrmhide
				player.Inventory[7] = player.Manager.GameData.Items[0xba9]; // UBHP
				player.UpdateCount++;
			}
			player.SendInfo("Set Given");
			return true;
		}
	}
	internal class GiftCommand : Command
	{
		public GiftCommand()
			: base("gift", (int)accountType.LOESOFT_ACCOUNT)
		{
		}
		protected override bool Process(Player player, RealmTime time, string[] args)
		{
			if (args.Length == 1)
			{
				player.SendHelp("Usage: /gift <Playername> <Itemname>");
				return false;
			}
			string name = string.Join(" ", args.Skip(1).ToArray()).Trim();
			var plr = player.Manager.FindPlayer(args[0]);
			ushort objType;
			Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(player.Manager.GameData.IdToObjectType,
				StringComparer.OrdinalIgnoreCase);
			if (!icdatas.TryGetValue(name, out objType))
			{
				player.SendError("Item not found, PERHAPS YOUR A RETARD LIKE LYNX WHO CAN'T SPELL SHIT?");
				return false;
			}
			if (!player.Manager.GameData.Items[objType].Secret || player.Client.Account.AccountType >= 4)
			{
				for (int i = 0; i < plr.Inventory.Length; i++)
					if (plr.Inventory[i] == null)
					{
						plr.Inventory[i] = player.Manager.GameData.Items[objType];
						plr.UpdateCount++;
						plr.SaveToCharacter();
						player.SendInfo("Success sending " + name + " to " + plr.Name);
						plr.SendInfo("You got a " + name + " from " + player.Name);
						break;
					}
			}
			else
			{
				player.SendError("Item failed sending to " + plr.Name + ", make sure you spelt the command right, and their name!");
				return false;
			}
			return true;
		}
	}
	internal class Size : Command
	{
		public Size() : base("size", (int)accountType.VIP_ACCOUNT) { }
		protected override bool Process(Player player, RealmTime time, string[] args)
		{
			if (args.Length != 3)
			{
				player.SendHelp("Usage: /size <Playername> <PlayerOrPet> <Amount>");
				return false;
			}
			foreach (KeyValuePair<string, Client> i in player.Manager.Clients
				.Where(i => i.Value.Player.Name.EqualsIgnoreCase(args[0])))
			{
				int size;
				if (!int.TryParse(args[2], out size))
				{
					player.SendError("Invalid Amount");
					return false;
				}
				switch (args[1].ToLower())
				{
					case "player":
						if (!string.Equals(player.Name.ToLower(), args[0].ToLower()))
						{
							i.Value.Player.Size = size;
							return true;
						}
						player.Size = size;
						return true;
					case "pet":
						if (!string.Equals(player.Name.ToLower(), args[0].ToLower()))
						{
							i.Value.Player.Pet.Size = size;
							return true;
						}
						player.Pet.Size = size;
						return true;
					default:
						player.SendError("Player or Pet");
						return false;
				}
			}
			player.SendError($"Player {args[0]} could not be found");
			return false;
		}
	}
	class TestCommand : Command
    {
        public TestCommand() : base("test", (int) accountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            Entity en = Entity.Resolve(player.Manager, "Zombie Wizard");
            en.Move(player.X, player.Y);
            player.Owner.EnterWorld(en);
            player.UpdateCount++;
            return true;
        }
    }

    class posCmd : Command
    {
        public posCmd() : base("p", (int) accountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            player.SendInfo("X: " + (int)player.X + " - Y: " + (int)player.Y);
            return true;
        }
    }

    class AddRealmCommand : Command
    {
        public AddRealmCommand() : base("addrealm", (int) accountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            Task.Factory.StartNew(() => GameWorld.AutoName(1, true)).ContinueWith(_ => player.Manager.AddWorld(_.Result), TaskScheduler.Default);
            return true;
        }
    }


    class SpawnCommand : Command
    {
        public SpawnCommand() : base("spawn", (int) accountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (player.Owner.Name != "Nexus") {
                int num;
                if (args.Length > 0 && int.TryParse(args[0], out num)) //multi
                {
                    string name = string.Join(" ", args.Skip(1).ToArray());
                    ushort objType;
                    //creates a new case insensitive dictionary based on the XmlDatas
                    Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(
                        player.Manager.GameData.IdToObjectType,
                        StringComparer.OrdinalIgnoreCase);
                    if (!icdatas.TryGetValue(name, out objType) ||
                        !player.Manager.GameData.ObjectDescs.ContainsKey(objType))
                    {
                        player.SendInfo("Unknown entity!");
                        return false;
                    }
                    int c = int.Parse(args[0]);
                    for (int i = 0; i < num; i++)
                    {
                        Entity entity = Entity.Resolve(player.Manager, objType);
                        entity.Move(player.X, player.Y);
                        player.Owner.EnterWorld(entity);
                    }
                    player.SendInfo("Success!");
                }
                else
                {
                    string name = string.Join(" ", args);
                    ushort objType;
                    //creates a new case insensitive dictionary based on the XmlDatas
                    Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(
                        player.Manager.GameData.IdToObjectType,
                        StringComparer.OrdinalIgnoreCase);
                    if (!icdatas.TryGetValue(name, out objType) ||
                        !player.Manager.GameData.ObjectDescs.ContainsKey(objType))
                    {
                        player.SendHelp("Usage: /spawn <entityname>");
                        return false;
                    }
                    Entity entity = Entity.Resolve(player.Manager, objType);
                    entity.Move(player.X, player.Y);
                    player.Owner.EnterWorld(entity);
                }
            } else
            {
                player.SendInfo("You cannot spawn in Nexus.");
                return false;
            }
            return true;
        }
    }

    class AddEffCommand : Command
    {
        public AddEffCommand() : base("addeff", (int) accountType.LOESOFT_ACCOUNT) { }

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
        public RemoveEffCommand() : base("remeff", (int) accountType.LOESOFT_ACCOUNT) { }

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
            ushort objType;
            Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(player.Manager.GameData.IdToObjectType,
                StringComparer.OrdinalIgnoreCase);
            if (!icdatas.TryGetValue(name, out objType))
            {
                player.SendError("Unknown type!");
                return false;
            }
            if (!player.Manager.GameData.Items[objType].Secret || player.Client.Account.Admin)
            {
                for (int i = 4; i < player.Inventory.Length; i++)
                    if (player.Inventory[i] == null)
                    {
                        player.Inventory[i] = player.Manager.GameData.Items[objType];
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
        public TpCommand() : base("tp", (int) accountType.LOESOFT_ACCOUNT) { }

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
        public KillAll() : base("killAll", (int) accountType.LOESOFT_ACCOUNT) { }
        
        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            var iterations = 0;
            var lastKilled = -1;
            var killed = 0;

            var mobName = args.Aggregate((s, a) => string.Concat(s, " ", a));
            while (killed != lastKilled)
            {
                lastKilled = killed;
                foreach (var i in player.Owner.Enemies.Values.Where(e =>
                    e.ObjectDesc?.ObjectId != null && e.ObjectDesc.ObjectId.ContainsIgnoreCase(mobName)))
                {
                    i.Death(time);
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
        public Kick() : base("kick", (int) accountType.LOESOFT_ACCOUNT) { }

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
                        i.Value.Client.Disconnect(DisconnectReason.PLAYER_KICK);
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
        public Max() : base("max", (int) accountType.LOESOFT_ACCOUNT) { }

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
        public OryxSay() : base("osay", (int) accountType.LOESOFT_ACCOUNT) { }

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
        public OnlineCommand() : base("online", (int) accountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            StringBuilder sb = new StringBuilder("Online at this moment: ");

            foreach (KeyValuePair<int, World> w in player.Manager.Worlds)
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
        public Announcement() : base("announce", (int) accountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /announce <saytext>");
                return false;
            }
            string saytext = string.Join(" ", args);

            foreach (Client i in player.Manager.Clients.Values)
            {
                i.SendMessage(new TEXT
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
        public KillPlayerCommand() : base("kill", (int) accountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            foreach (Client i in player.Manager.Clients.Values)
            {
                if (i.Account.Name.EqualsIgnoreCase(args[0]))
                {
                    i.Player.HP = 0;
                    i.Player.Death("server.game_admin");
                    player.SendInfo($"Player {i.Account.Name} has been killed!");
                    return true;
                }
            }
            player.SendInfo(string.Format("Player '{0}' could not be found!", args));
            return false;
        }
    }

    class RestartCommand : Command
    {
        public RestartCommand() : base("restart", (int) accountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            foreach (KeyValuePair<int, World> w in player.Manager.Worlds)
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
        public TqCommand() : base("tq", (int) accountType.LOESOFT_ACCOUNT) { }

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
        public LevelCommand() : base("level", (int) accountType.LOESOFT_ACCOUNT) { }

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
        public SetCommand() : base("setStat", (int) accountType.LOESOFT_ACCOUNT) { }

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
                foreach (Client i in player.Manager.Clients.Values)
                {
                    if (i.Account.Name.EqualsIgnoreCase(args[0]))
                    {
                        try
                        {
                            string stat = args[1].ToLower();
                            int amount = int.Parse(args[2]);
                            switch (stat)
                            {
                                case "health":
                                case "hp":
                                    i.Player.Stats[0] = amount;
                                    break;
                                case "mana":
                                case "mp":
                                    i.Player.Stats[1] = amount;
                                    break;
                                case "att":
                                case "atk":
                                case "attack":
                                    i.Player.Stats[2] = amount;
                                    break;
                                case "def":
                                case "defence":
                                    i.Player.Stats[3] = amount;
                                    break;
                                case "spd":
                                case "speed":
                                    i.Player.Stats[4] = amount;
                                    break;
                                case "vit":
                                case "vitality":
                                    i.Player.Stats[5] = amount;
                                    break;
                                case "wis":
                                case "wisdom":
                                    i.Player.Stats[6] = amount;
                                    break;
                                case "dex":
                                case "dexterity":
                                    i.Player.Stats[7] = amount;
                                    break;
                                default:
                                    player.SendError("Invalid Stat");
                                    player.SendHelp("Stats: Health, Mana, Attack, Defence, Speed, Vitality, Wisdom, Dexterity");
                                    player.SendHelp("Shortcuts: Hp, Mp, Atk, Def, Spd, Vit, Wis, Dex");
                                    return false;
                            }
                            i.Player.SaveToCharacter();
                            i.Player.UpdateCount++;
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
        public SetpieceCommand() : base("setpiece", (int) accountType.LOESOFT_ACCOUNT) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            try
            {
                MapSetPiece piece = (MapSetPiece)Activator.CreateInstance(System.Type.GetType(
                    "gameserver.realm.mapsetpieces.setpieces." + args[0], true, true));
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
        public ListCommands() : base("commands", (int) accountType.LOESOFT_ACCOUNT) { }

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
    	public Mute() : base("mute", (int) accountType.LOESOFT_ACCOUNT) { }

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
    	public Unmute() : base("unmute", (int) accountType.LOESOFT_ACCOUNT) { }

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
    	public BanCommand() : base("ban", (int) accountType.LOESOFT_ACCOUNT) { }

    	protected override bool Process(Player player, RealmTime time, string[] args)
    	{
            try
            {
                Player p = player.Manager.FindPlayer(args[0]);
                if (p == null)
                {
                    player.SendError("Player not found");
                    return false;
                }
                p.Client.Manager.Database.BanAccount(p.Client.Account);
                p.Client.Disconnect(DisconnectReason.PLAYER_BANNED);
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