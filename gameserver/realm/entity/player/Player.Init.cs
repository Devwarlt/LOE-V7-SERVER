#region

using System;
using System.Collections.Generic;
using System.Linq;
using LoESoft.GameServer.logic;
using LoESoft.GameServer.networking;
using LoESoft.GameServer.networking.incoming;
using LoESoft.GameServer.networking.outgoing;
using System.Xml.Linq;
using LoESoft.Core.config;
using static LoESoft.GameServer.networking.Client;
using LoESoft.Core;
using LoESoft.GameServer.logic.skills.Pets;
using LoESoft.Core.assets.itemdata;

#endregion

namespace LoESoft.GameServer.realm.entity.player
{
    internal interface IPlayer
    {
        void Damage(int dmg, Entity chr, bool NoDef, bool manaDrain = false);
        bool IsVisibleToEnemy();
    }

    public static class ComparableExtension
    {
        public static bool InRange<T>(this T value, T from, T to) where T : IComparable<T> => value.CompareTo(from) >= 1 && value.CompareTo(to) <= -1;
    }

    public partial class Player : Character, IContainer, IPlayer
    {
        public Player(Client client) : base(client.Character.ObjectType, client.Random)
        {
            try
            {
                var @char = client.Character;
                CharLevel = @char.CharLevel;
                CharExperience = Convert.ToDouble(@char.CharExperience);
                CharHealthPoints = @char.CharHealthPoints;
                CharMagicPoints = @char.CharMagicPoints;
                CharAttackLevel = @char.CharAttackLevel;
                CharAttackExperience = Convert.ToDouble(@char.CharAttackExperience);
                CharDefenseLevel = @char.CharDefenseLevel;
                CharDefenseExperience = Convert.ToDouble(@char.CharDefenseExperience);
                CharSpeed = @char.CharSpeed;
                CharMaxHealthPoints = @char.CharMaxHealthPoints;
                CharMaxMagicPoints = @char.CharMaxMagicPoints;
                CharNextExperience = Convert.ToDouble(@char.CharNextExperience);
                CharNextAttackExperience = Convert.ToDouble(@char.CharNextAttackExperience);
                CharNextDefenseExperience = Convert.ToDouble(@char.CharNextDefenseExperience);
                CharPosition = ProcessPosition(@char.CharPosition);
                CharTownID = @char.CharTownID;

                if (client.Account.Admin == true)
                    Admin = 1;
                AccountType = client.Account.AccountType;
                AccountPerks = new AccountTypePerks(AccountType);
                AccountLifetime = client.Account.AccountLifetime;
                IsVip = AccountLifetime != DateTime.MinValue;
                Client = client;
                StatsManager = new StatsManager(this, client.Random.CurrentSeed);
                Name = client.Account.Name;
                AccountId = client.Account.AccountId;
                FameCounter = new FameCounter(this);
                //TaskManager = new TaskManager(this); // pending addition for future versions
                Tokens = client.Account.FortuneTokens;
                HpPotionPrice = 5;
                MpPotionPrice = 5;
                Level = client.Character.Level == 0 ? 1 : client.Character.Level;
                Experience = client.Character.Experience;
                ExperienceGoal = GetExpGoal(Level);
                Stars = AccountType >= (int)Core.config.AccountType.LEGENDS_OF_LOE_ACCOUNT ? 70 : GetStars();
                Texture1 = client.Character.Tex1;
                Texture2 = client.Character.Tex2;
                Credits = client.Account.Credits;
                NameChosen = client.Account.NameChosen;
                CurrentFame = client.Account.Fame;
                Fame = client.Character.Fame;
                PetHealing = null;
                PetAttack = null;
                if (client.Character.Pet != 0)
                {
                    PetHealing = new List<List<int>>();
                    PetAttack = new List<int>();
                    PetID = client.Character.Pet;
                    Tuple<int, int, double> HPData = PetHPHealing.MinMaxBonus(Resolve((ushort)PetID).ObjectDesc.HPTier, Stars);
                    Tuple<int, int, double> MPData = PetMPHealing.MinMaxBonus(Resolve((ushort)PetID).ObjectDesc.MPTier, Stars);
                    PetHealing.Add(new List<int> { HPData.Item1, HPData.Item2, (int)((HPData.Item3 - 1) * 100) });
                    PetHealing.Add(new List<int> { MPData.Item1, MPData.Item2, (int)((MPData.Item3 - 1) * 100) });
                    PetAttack.Add(7750 - Stars * 100);
                    PetAttack.Add(30 + Stars);
                    PetAttack.Add(Resolve((ushort)PetID).ObjectDesc.Projectiles[0].MinDamage);
                    PetAttack.Add(Resolve((ushort)PetID).ObjectDesc.Projectiles[0].MaxDamage);
                }
                LootDropBoostTimeLeft = client.Character.LootDropTimer;
                lootDropBoostFreeTimer = LootDropBoost;
                LootTierBoostTimeLeft = client.Character.LootTierTimer;
                lootTierBoostFreeTimer = LootTierBoost;
                FameGoal = (AccountType >= (int)Core.config.AccountType.LEGENDS_OF_LOE_ACCOUNT) ? 0 : GetFameGoal(FameCounter.ClassStats[ObjectType].BestFame);
                Glowing = false;
                DbGuild guild = GameServer.Manager.Database.GetGuild(client.Account.GuildId);
                if (guild != null)
                {
                    Guild = GameServer.Manager.Database.GetGuild(client.Account.GuildId).Name;
                    GuildRank = client.Account.GuildRank;
                }
                else
                {
                    Guild = "";
                    GuildRank = -1;
                }
                HP = client.Character.HP <= 0 ? (int)ObjectDesc.MaxHP : client.Character.HP;
                MP = client.Character.MP;
                ConditionEffects = 0;
                OxygenBar = 100;
                HasBackpack = client.Character.HasBackpack == true;
                PlayerSkin = Client.Account.OwnedSkins.Contains(Client.Character.Skin) ? Client.Character.Skin : 0;
                HealthPotions = client.Character.HealthPotions < 0 ? 0 : client.Character.HealthPotions;
                MagicPotions = client.Character.MagicPotions < 0 ? 0 : client.Character.MagicPotions;

                try
                {
                    Locked = client.Account.Database.GetLockeds(client.Account);
                    Ignored = client.Account.Database.GetIgnoreds(client.Account);
                    Muted = client.Account.Muted;
                }
                catch (Exception) { }

                if (HasBackpack)
                {
                    Item[] inv =
                        client.Character.Items.Select(
                            _ =>
                                _ == -1
                                    ? null
                                    : (GameServer.Manager.GameData.Items.ContainsKey((ushort)_) ? GameServer.Manager.GameData.Items[(ushort)_] : null))
                            .ToArray();
                    Item[] backpack =
                        client.Character.Backpack.Select(
                            _ =>
                                _ == -1
                                    ? null
                                    : (GameServer.Manager.GameData.Items.ContainsKey((ushort)_) ? GameServer.Manager.GameData.Items[(ushort)_] : null))
                            .ToArray();

                    Inventory = inv.Concat(backpack).ToArray();
                    XElement xElement = GameServer.Manager.GameData.ObjectTypeToElement[ObjectType].Element("SlotTypes");
                    if (xElement != null)
                    {
                        int[] slotTypes =
                            Utils.FromCommaSepString32(
                                xElement.Value);
                        Array.Resize(ref slotTypes, 20);
                        SlotTypes = slotTypes;
                    }
                }
                else
                {
                    Inventory =
                            client.Character.Items.Select(
                                _ =>
                                    _ == -1
                                        ? null
                                        : (GameServer.Manager.GameData.Items.ContainsKey((ushort)_) ? GameServer.Manager.GameData.Items[(ushort)_] : null))
                                .ToArray();
                    XElement xElement = GameServer.Manager.GameData.ObjectTypeToElement[ObjectType].Element("SlotTypes");
                    if (xElement != null)
                        SlotTypes =
                            Utils.FromCommaSepString32(
                                xElement.Value);
                }
                Stats = (int[])client.Character.Stats.Clone();

                for (var i = 0; i < SlotTypes.Length; i++)
                    if (SlotTypes[i] == 0) SlotTypes[i] = 10;

                if (Client.Account.AccountType >= (int)Core.config.AccountType.TUTOR_ACCOUNT)
                    return;

                for (var i = 0; i < 4; i++)
                    if (Inventory[i]?.SlotType != SlotTypes[i])
                        Inventory[i] = null;
            }
            catch (Exception) { }
        }

        private float LastPosX { get; set; }
        private float LastPosY { get; set; }

        public override void Move(float x, float y)
        {
            if (Pet != null)
            {
                if (Dist(this, Pet) > 20f)
                {
                    Pet.Move(X, Y);
                    UpdateCount++;
                }
            }

            base.Move(x, y);
        }

        public void TeleportToTemple()
        {
            Client.Reconnect(new RECONNECT
            {
                GameId = CharTownID,
                Host = string.Empty,
                Key = Empty<byte>.Array,
                Name = "Town Temple",
                Port = Settings.GAMESERVER.PORT
            });
        }

        public void Death(string killer, ObjectDesc desc = null)
        {
            if (dying)
                return;

            dying = true;

            if (Client.State == ProtocolState.Disconnected)
                return;

            if (Client.Character.Dead)
            {
                GameServer.Manager.TryDisconnect(Client, DisconnectReason.CHARACTER_IS_DEAD);
                return;
            }

            if (desc != null)
                if (desc.DisplayId != null)
                    killer = desc.DisplayId;
                else
                    killer = desc.ObjectId;

            switch (killer)
            {
                case "":
                case "Unknown":
                    break;

                default:
                    Owner.BroadcastMessage(new TEXT
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "{\"key\":\"server.death\",\"tokens\":{\"player\":\"" + Name + "\",\"level\":\"" + Level + "\",\"enemy\":\"" + killer + "\"}}",
                        NameColor = 0x123456,
                        TextColor = 0x123456
                    }, null);
                    break;
            }

            CharPosition = World.TownTemples[(TownID)CharTownID];

            SaveToCharacter();

            TeleportToTemple();
        }

        public override void Init(World owner)
        {
            MaxHackEntries = 0;

            visibleTiles = new Dictionary<IntPoint, bool>();

            WorldInstance = owner;

            Random rand = new Random();

            float x = LastPosX = CharPosition.X;
            float y = LastPosY = CharPosition.Y;

            if (Client.CheckReconnect)
            {
                SendInfo("You have been reconnected to the game.");

                ApplyConditionEffect(
                    new ConditionEffect
                    {
                        Effect = ConditionEffectIndex.Invincible,
                        DurationMS = 5000
                    },
                    new ConditionEffect
                    {
                        Effect = ConditionEffectIndex.Paused,
                        DurationMS = 3000
                    });

                Move(Client.ReconnectManager[AccountId].X, Client.ReconnectManager[AccountId].Y);

                SaveToCharacter();

                Client.RemoveReconnect();
            }
            else
                Move(x, y);

            tiles = new byte[owner.Map.Width, owner.Map.Height];

            SetNewbiePeriod();

            base.Init(owner);

            if (owner.Id == (int)TownID.ISLE_OF_APPRENTICES_ID || owner.Id == (int)TownID.VAULT_ID)
            {
                List<int> gifts = Client.Account.Gifts.ToList();

                Client.SendMessage(new GLOBAL_NOTIFICATION
                {
                    Type = 0,
                    Text = gifts.Count > 0 ? "giftChestOccupied" : "giftChestEmpty"
                });
            }

            if (Client.Character.Pet != 0)
            {
                HatchlingPet = false;
                HatchlingNotification = false;
                Pet = Resolve((ushort)PetID);
                Pet.Move(x, y);
                Pet.SetPlayerOwner(this);
                Owner.EnterWorld(Pet);
                Pet.IsPet = true;
            }

            SendAccountList(Locked, ACCOUNTLIST.LOCKED_LIST_ID);
            SendAccountList(Ignored, ACCOUNTLIST.IGNORED_LIST_ID);

            CheckSetTypeSkin();

            if ((AccountType)AccountType == Core.config.AccountType.LOESOFT_ACCOUNT)
            {
                ConditionEffect invincible = new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Invincible,
                    DurationMS = -1
                };

                ApplyConditionEffect(invincible);

                ConditionEffect invulnerable = new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Invulnerable,
                    DurationMS = -1
                };

                ApplyConditionEffect(invulnerable);
            }

            ApplyConditionEffect(AccountPerks.SetAccountTypeIcon());

            string DNS = Client.Socket.RemoteEndPoint.ToString().Split(':')[0];

            switch (Settings.SERVER_MODE)
            {
                case Settings.ServerMode.Local:
                    if (!Settings.ALLOWED_LOCAL_DNS.Contains(DNS))
                        GameServer.Manager.TryDisconnect(Client, DisconnectReason.SERVER_MODE_LOCAL_ONLY);
                    break;
                case Settings.ServerMode.ClosedTest:
                    if (!Client.Account.ClosedTester)
                    {
                        CTTSendHandler CTTHandler = new CTTSendHandler
                        {
                            Client = Client
                        };

                        CTTHandler.SendRequest();
                    }

                    break;
                default:
                    break;
            }
        }

        public void Teleport(RealmTime time, TELEPORT packet)
        {
            var obj = Client.Player.Owner.GetEntity(packet.ObjectId);
            try
            {
                if (obj == null) return;
                if (!TPCooledDown())
                {
                    SendError("Player.teleportCoolDown");
                    return;
                }
                if (obj.HasConditionEffect(ConditionEffectIndex.Invisible))
                {
                    SendError("server.no_teleport_to_invisible");
                    return;
                }
                if (obj.HasConditionEffect(ConditionEffectIndex.Paused))
                {
                    SendError("server.no_teleport_to_paused");
                    return;
                }
                if (obj is Player player && !player.NameChosen)
                {
                    SendError("server.teleport_needs_name");
                    return;
                }
                if (obj.Id == Id)
                {
                    SendError("server.teleport_to_self");
                    return;
                }
                if (!Owner.AllowTeleport)
                {
                    SendError(GetLanguageString("server.no_teleport_in_realm", new KeyValuePair<string, object>("realm", Owner.Name)));
                    return;
                }

                SetTPDisabledPeriod();
                Move(obj.X, obj.Y);
                FameCounter.Teleport();
                SetNewbiePeriod();
                UpdateCount++;
            }
            catch (Exception)
            {
                SendError("player.cannotTeleportTo");
                return;
            }

            Owner.BroadcastMessage(new GOTO
            {
                ObjectId = Id,
                Position = new Position
                {
                    X = X,
                    Y = Y
                }
            }, null);
            Owner.BroadcastMessage(new SHOWEFFECT
            {
                EffectType = EffectType.Teleport,
                TargetId = Id,
                PosA = new Position
                {
                    X = X,
                    Y = Y
                },
                Color = new ARGB(0xFFFFFFFF)
            }, null);
        }

        private int QuestPriority(ObjectDesc enemy)
        {
            int score = 0;

            if (enemy.Oryx)
                score += 100000;

            if (enemy.Cube)
                score += 2500;

            if (enemy.God)
                score += 500;

            if (enemy.Hero)
                score += 1250;

            if (enemy.Encounter)
                score += 5000;

            if (enemy.Quest)
                score += 250;

            score += enemy.MaxHitPoints;
            score += enemy.Defense * enemy.Level;

            return score;
        }

        private void HandleQuest(RealmTime time)
        {
            if (time.TickCount % 5 != 0)
                return;

            int newQuestId = -1;
            int questId = Quest == null ? -1 : Quest.Id;

            HashSet<Enemy> candidates = new HashSet<Enemy>();

            foreach (Enemy i in Owner.Quests.Values
                .OrderBy(j => MathsUtils.DistSqr(j.X, j.Y, X, Y))
                .Where(k => k.ObjectDesc != null && k.ObjectDesc.Quest))
            {
                if (!RealmManager.QuestPortraits.TryGetValue(i.ObjectDesc.ObjectId, out int questLevel))
                    continue;

                if (Level < questLevel)
                    continue;

                if (!RealmManager.QuestPortraits.ContainsKey(i.ObjectDesc.ObjectId))
                    continue;

                candidates.Add(i);
            }

            if (candidates.Count != 0)
            {
                Enemy newQuest = candidates.OrderByDescending(i => QuestPriority(i.ObjectDesc)).Take(3).ToList()[0];

                newQuestId = newQuest.Id;
                Quest = newQuest;
            }

            if (newQuestId == questId)
                return;

            Client.SendMessage(new QUESTOBJID
            {
                ObjectId = newQuestId
            });
        }

        private void CalculateFame()
        {
            double newFame = 0.0;

            newFame += Math.Max(0, Math.Min(20000, Experience)) * 0.001;
            newFame += Math.Max(0, Math.Min(45200, Experience) - 20000) * 0.002;
            newFame += Math.Max(0, Math.Min(80000, Experience) - 45200) * 0.003;
            newFame += Math.Max(0, Math.Min(101200, Experience) - 80000) * 0.002;
            newFame += Math.Max(0, Experience - 101200) * 0.0005;
            newFame += Math.Min(Math.Floor((double)FameCounter.Stats.MinutesActive / 6), 30);

            newFame = Math.Floor(newFame);

            if (newFame == Fame)
                return;

            Fame = (int)newFame;
            int newGoal;
            var stats = FameCounter.ClassStats[ObjectType];
            if (stats.BestFame > Fame)
                newGoal = GetFameGoal(stats.BestFame);
            else
                newGoal = GetFameGoal(Fame);
            if (newGoal > FameGoal && AccountType < (int)Core.config.AccountType.LEGENDS_OF_LOE_ACCOUNT)
            {
                Owner.BroadcastMessage(new NOTIFICATION
                {
                    ObjectId = Id,
                    Color = new ARGB(0xFF00FF00),
                    Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"Class Quest Complete!\"}}",
                }, null);
                Stars = GetStars();
            }
            FameGoal = (AccountType >= (int)Core.config.AccountType.LEGENDS_OF_LOE_ACCOUNT) ? 0 : newGoal;
            UpdateCount++;
        }

        private bool CheckLevelUp()
        {
            if (Experience - GetLevelExp(Level) >= ExperienceGoal && Level < 20)
            {
                Level++;
                ExperienceGoal = GetExpGoal(Level);
                foreach (var i in GameServer.Manager.GameData.ObjectTypeToElement[ObjectType].Elements("LevelIncrease"))
                {
                    var rand = new Random();
                    var min = int.Parse(i.Attribute("min").Value);
                    var max = int.Parse(i.Attribute("max").Value) + 1;
                    var xElement = GameServer.Manager.GameData.ObjectTypeToElement[ObjectType].Element(i.Value);
                    if (xElement == null) continue;
                    var limit =
                        int.Parse(
                            xElement.Attribute("max").Value);
                    var idx = StatsManager.StatsNameToIndex(i.Value);
                    Stats[idx] += rand.Next(min, max);
                    if (Stats[idx] > limit) Stats[idx] = limit;
                }
                HP = Stats[0] + Boost[0];
                MP = Stats[1] + Boost[1];

                UpdateCount++;

                if (Level == 20)
                {
                    foreach (var i in Owner.Players.Values)
                        i.SendInfo(Name + " achieved level 20");
                    XpBoosted = false;
                    XpBoostTimeLeft = 0;
                }
                Quest = null;
                return true;
            }
            CalculateFame();
            return false;
        }

        public bool EnemyKilled(Enemy enemy, int exp, bool killer)
        {
            if (enemy == Quest)
                Owner.BroadcastMessage(new NOTIFICATION
                {
                    ObjectId = Id,
                    Color = new ARGB(0xFF00FF00),
                    Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"Quest Complete!\"}}",
                }, null);
            if (exp > 0)
            {
                if (XpBoosted)
                    Experience += exp * 2;
                else
                    Experience += exp;
                UpdateCount++;
                foreach (var i in Owner.PlayersCollision.HitTest(X, Y, 16).Where(i => i != this).OfType<Player>())
                {
                    try
                    {
                        i.Experience += i.XpBoosted ? exp * 2 : exp;
                        i.UpdateCount++;
                        i.CheckLevelUp();
                    }
                    catch (Exception) { }
                }
            }
            FameCounter.Killed(enemy, killer);
            return CheckLevelUp();
        }

        internal Projectile PlayerShootProjectile(
            byte id,
            ProjectileDesc desc,
            ushort objType,
            long time,
            Position position,
            float angle
            )
        {
            ProjectileId = id;
            return CreateProjectile(desc, objType, (int)StatsManager.GetAttackDamage(desc.MinDamage, desc.MaxDamage), C2STime(time), position, angle);
        }

        public override void Tick(RealmTime time)
        {
            if (Client == null)
                return;

            if (!PlayerNetworkingHandler(time) || Client.State == ProtocolState.Disconnected)
            {
                if (Owner != null)
                    Owner.LeaveWorld(this);
                else
                    WorldInstance.LeaveWorld(this);

                return;
            }

            if (Stats != null && Boost != null)
            {
                MaxHp = Stats[0] + Boost[0];
                MaxMp = Stats[1] + Boost[1];
            }

            if (Boost == null)
                CalculateBoost();

            if (!HasConditionEffect(ConditionEffects.Paused))
            {
                HandleRegen(time);

                HandleGround(time);

                FameCounter.Tick(time);
            }

            HandleTrade?.Tick(time);

            try
            {
                HandleQuest(time);
            }
            catch (NullReferenceException) { }

            HandleEffects(time);

            HandleBoosts();

            if (MP < 0)
                MP = 0;

            if (Owner != null)
            {
                HandleNewTick(time);
                HandleUpdate(time);
            }

            base.Tick(time);
        }
    }
}