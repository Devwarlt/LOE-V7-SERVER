#region

using System;
using System.Collections.Generic;
using System.Linq;
using gameserver.logic;
using gameserver.networking;
using gameserver.networking.incoming;
using gameserver.networking.outgoing;
using System.Xml.Linq;
using gameserver.realm.terrain;
using common.config;
using static gameserver.networking.Client;
using common;
using gameserver.logic.skills.Pets;

#endregion

namespace gameserver.realm.entity.player
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
                if (client.Account.Admin == true)
                    Admin = 1;
                AccountType = client.Account.AccountType;
                AccountPerks = new AccountTypePerks(AccountType);
                AccountLifetime = client.Account.AccountLifetime;
                isVip = AccountLifetime != DateTime.MinValue;
                this.client = client;
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
                Stars = AccountType >= (int)accountType.LEGENDS_OF_LOE_ACCOUNT ? 70 : GetStars();
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
                FameGoal = (AccountType >= (int)accountType.LEGENDS_OF_LOE_ACCOUNT) ? 0 : GetFameGoal(FameCounter.ClassStats[ObjectType].BestFame);
                Glowing = false;
                DbGuild guild = Program.Manager.Database.GetGuild(client.Account.GuildId);
                if (guild != null)
                {
                    Guild = Program.Manager.Database.GetGuild(client.Account.GuildId).Name;
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
                PlayerSkin = this.client.Account.OwnedSkins.Contains(this.client.Character.Skin) ? this.client.Character.Skin : 0;
                HealthPotions = client.Character.HealthPotions < 0 ? 0 : client.Character.HealthPotions;
                MagicPotions = client.Character.MagicPotions < 0 ? 0 : client.Character.MagicPotions;

                try
                {
                    Locked = client.Account.Database.GetLockeds(client.Account);
                    Ignored = client.Account.Database.GetIgnoreds(client.Account);
                    Muted = client.Account.Muted;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                if (HasBackpack)
                {
                    Item[] inv =
                        client.Character.Items.Select(
                            _ =>
                                _ == -1
                                    ? null
                                    : (Program.Manager.GameData.Items.ContainsKey((ushort)_) ? Program.Manager.GameData.Items[(ushort)_] : null))
                            .ToArray();
                    Item[] backpack =
                        client.Character.Backpack.Select(
                            _ =>
                                _ == -1
                                    ? null
                                    : (Program.Manager.GameData.Items.ContainsKey((ushort)_) ? Program.Manager.GameData.Items[(ushort)_] : null))
                            .ToArray();

                    Inventory = inv.Concat(backpack).ToArray();
                    XElement xElement = Program.Manager.GameData.ObjectTypeToElement[ObjectType].Element("SlotTypes");
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
                                        : (Program.Manager.GameData.Items.ContainsKey((ushort)_) ? Program.Manager.GameData.Items[(ushort)_] : null))
                                .ToArray();
                    XElement xElement = Program.Manager.GameData.ObjectTypeToElement[ObjectType].Element("SlotTypes");
                    if (xElement != null)
                        SlotTypes =
                            Utils.FromCommaSepString32(
                                xElement.Value);
                }
                Stats = (int[])client.Character.Stats.Clone();

                for (var i = 0; i < SlotTypes.Length; i++)
                    if (SlotTypes[i] == 0) SlotTypes[i] = 10;

                if (this.client.Account.AccountType >= (int)accountType.TUTOR_ACCOUNT)
                    return;

                for (var i = 0; i < 4; i++)
                    if (Inventory[i]?.SlotType != SlotTypes[i])
                        Inventory[i] = null;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        public void Death(string killer, ObjectDesc desc = null)
        {
            if (dying) return;
            dying = true;
            switch (Owner.Name)
            {
                case "Arena":
                    {
                        client.SendMessage(new ARENA_DEATH
                        {
                            RestartPrice = 100
                        });
                        HP = (int)ObjectDesc.MaxHP;
                        ApplyConditionEffect(new ConditionEffect
                        {
                            Effect = ConditionEffectIndex.Paused,
                            DurationMS = -1
                        });
                        return;
                    }
            }
            if (client.State == ProtocolState.Disconnected || resurrecting)
                return;
            if (CheckResurrection())
                return;

            if (client.Character.Dead)
            {
                Program.Manager.TryDisconnect(client, DisconnectReason.CHARACTER_IS_DEAD);
                return;
            }
            GenerateGravestone();
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
                    Owner.BroadcastPacket(new TEXT
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

            try
            {
                client.Character.Dead = true;

                SaveToCharacter();

                Program.Manager.Database.SaveCharacter(client.Account, client.Character, true);
                Program.Manager.Database.Death(Program.Manager.GameData, client.Account, client.Character, FameCounter.Stats, killer);

                if (Owner.Id != -6)
                {
                    DEATH _death = new DEATH();
                    _death.AccountId = AccountId;
                    _death.CharId = client.Character.CharId;
                    _death.Killer = killer;
                    _death.zombieId = -1;
                    _death.zombieType = -1;

                    client.SendMessage(_death);

                    Log.Write($"Message details type '{_death.ID}':\n{_death}");

                    Owner.Timers.Add(new WorldTimer(1000, (w, t) => Program.Manager.TryDisconnect(client, DisconnectReason.CHARACTER_IS_DEAD)));

                    Log.Write($"Removing from world '{Owner.Name}' player '{Name}' (Account ID: {AccountId}).");

                    Owner.LeaveWorld(this);
                }
                else
                    Program.Manager.TryDisconnect(client, DisconnectReason.CHARACTER_IS_DEAD_ERROR);
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        public override void Init(World owner)
        {
            visibleTiles = new Dictionary<IntPoint, bool>();
            WorldInstance = owner;
            Random rand = new Random();
            int x, y;
            do
            {
                x = rand.Next(0, owner.Map.Width);
                y = rand.Next(0, owner.Map.Height);
            } while (owner.Map[x, y].Region != TileRegion.Spawn);
            Move(x + 0.5f, y + 0.5f);
            tiles = new byte[owner.Map.Width, owner.Map.Height];
            SetNewbiePeriod();
            base.Init(owner);
            List<int> gifts = client.Account.Gifts.ToList();
            if (owner.Id == World.NEXUS_ID || owner.Name == "Vault")
            {
                client.SendMessage(new GLOBAL_NOTIFICATION
                {
                    Type = 0,
                    Text = gifts.Count > 0 ? "giftChestOccupied" : "giftChestEmpty"
                });
            }
            if (client.Character.Pet != 0)
            {
                HatchlingPet = false;
                Pet = Resolve((ushort)PetID);
                Pet.Move(x, y);
                Pet.SetPlayerOwner(this);
                Owner.EnterWorld(Pet);
                Pet.IsPet = true;
            }

            SendAccountList(Locked, ACCOUNTLIST.LOCKED_LIST_ID);
            SendAccountList(Ignored, ACCOUNTLIST.IGNORED_LIST_ID);

            CheckSetTypeSkin();

            if ((accountType)AccountType == accountType.LOESOFT_ACCOUNT)
            {
                ConditionEffect invincible = new ConditionEffect();
                invincible.Effect = ConditionEffectIndex.Invincible;
                invincible.DurationMS = -1;

                ApplyConditionEffect(invincible);

                ConditionEffect invulnerable = new ConditionEffect();
                invulnerable.Effect = ConditionEffectIndex.Invulnerable;
                invulnerable.DurationMS = -1;

                ApplyConditionEffect(invulnerable);
            }

            ApplyConditionEffect(AccountPerks.SetAccountTypeIcon());
        }

        public void Teleport(RealmTime time, TELEPORT packet)
        {
            var obj = client.Player.Owner.GetEntity(packet.ObjectId);
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
                var player = obj as Player;
                if (player != null && !player.NameChosen)
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
            catch (Exception ex)
            {
                log.Error(ex);
                SendError("player.cannotTeleportTo");
                return;
            }
            Owner.BroadcastPacket(new GOTO
            {
                ObjectId = Id,
                Position = new Position
                {
                    X = X,
                    Y = Y
                }
            }, null);
            Owner.BroadcastPacket(new SHOWEFFECT
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

        private Entity FindQuest()
        {
            Entity ret = null;
            try
            {
                float bestScore = 0;
                foreach (var i in Owner.Quests.Values
                    .OrderBy(quest => MathsUtils.DistSqr(quest.X, quest.Y, X, Y)).Where(i => i.ObjectDesc != null && i.ObjectDesc.Quest))
                {
                    Tuple<int, int, int> x;
                    if (!questPortraits.TryGetValue(i.ObjectDesc.ObjectId, out x)) continue;

                    if ((Level < x.Item2 || Level > x.Item3)) continue;
                    var score = (20 - Math.Abs((i.ObjectDesc.Level ?? 0) - Level)) * x.Item1 -
                                //priority * level diff
                                Dist(this, i) / 100; //minus 1 for every 100 tile distance
                    if (score < 0)
                        score = 1;
                    if (!(score > bestScore)) continue;
                    bestScore = score;
                    ret = i;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return ret;
        }

        private void HandleQuest(RealmTime time)
        {
            if (time.TickCount % 500 != 0 && Quest?.Owner != null) return;
            var newQuest = FindQuest();
            if (newQuest == null || newQuest == Quest) return;
            Owner.Timers.Add(new WorldTimer(100, (w, t) =>
            {
                client.SendMessage(new QUESTOBJID
                {
                    ObjectId = newQuest.Id
                });
            }));
            Quest = newQuest;
        }

        private void CalculateFame()
        {
            int newFame;
            if (Experience < 200 * 1000) newFame = Experience / 1000;
            else newFame = 200 + (Experience - 200 * 1000) / 1000;
            if (newFame == Fame) return;
            Fame = newFame;
            int newGoal;
            var stats = FameCounter.ClassStats[ObjectType];
            if (stats.BestFame > Fame)
                newGoal = GetFameGoal(stats.BestFame);
            else
                newGoal = GetFameGoal(Fame);
            if (newGoal > FameGoal && AccountType < (int)accountType.LEGENDS_OF_LOE_ACCOUNT)
            {
                Owner.BroadcastPacket(new NOTIFICATION
                {
                    ObjectId = Id,
                    Color = new ARGB(0xFF00FF00),
                    Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"Class Quest Complete!\"}}",
                }, null);
                Stars = GetStars();
            }
            FameGoal = (AccountType >= (int)accountType.LEGENDS_OF_LOE_ACCOUNT) ? 0 : newGoal;
            UpdateCount++;
        }

        private bool CheckLevelUp()
        {
            if (Experience - GetLevelExp(Level) >= ExperienceGoal && Level < 20)
            {
                Level++;
                ExperienceGoal = GetExpGoal(Level);
                foreach (var i in Program.Manager.GameData.ObjectTypeToElement[ObjectType].Elements("LevelIncrease"))
                {
                    var rand = new Random();
                    var min = int.Parse(i.Attribute("min").Value);
                    var max = int.Parse(i.Attribute("max").Value) + 1;
                    var xElement = Program.Manager.GameData.ObjectTypeToElement[ObjectType].Element(i.Value);
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
                Owner.BroadcastPacket(new NOTIFICATION
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
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
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
            return CreateProjectile(desc, objType, (int)StatsManager.GetAttackDamage(desc.MinDamage, desc.MaxDamage), time, position, angle);
        }

        public override void Tick(RealmTime time)
        {
            if (client == null)
                return;

            if (!KeepAlive(time))
                return;

            try
            {
                if (client.State == ProtocolState.Disconnected)
                {
                    if (Owner != null)
                        Owner.LeaveWorld(this);
                    else
                        WorldInstance.LeaveWorld(this);
                    return;
                }
            }
            catch (Exception e)
            {
                log.Error(e);
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

            TradeHandler?.Tick(time);
            HandleQuest(time);
            HandleEffects(time);
            HandleBoosts();

            if (MP < 0)
                MP = 0;

            if (Owner != null)
            {
                SendNewTick(time);
                SendUpdate(time);
            }

            if (HP < 0 && !dying)
            {
                Death("Unknown");
                return;
            }

            base.Tick(time);
        }
    }
}