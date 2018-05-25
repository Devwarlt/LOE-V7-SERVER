#region

using System;
using System.Collections.Generic;
using System.Linq;
using LoESoft.GameServer.networking;
using LoESoft.GameServer.networking.outgoing;
using System.Xml.Linq;
using LoESoft.GameServer.realm.terrain;
using LoESoft.Core.config;
using static LoESoft.GameServer.networking.Client;
using LoESoft.Core;

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
        public Player(Client client) : base(client.Character.Vocation, client.Random)
        {
            try
            {
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
                //TaskManager = new TaskManager(this); // pending addition for future versions
                Level = client.Character.Level == 0 ? 1 : client.Character.Level;
                Experience = client.Character.Experience;
                ExperienceGoal = GetNextLevelExperience(Level);
                Stars = AccountType >= (int)Core.config.AccountType.LEGENDS_OF_LOE_ACCOUNT ? 70 : 28;
                Credits = client.Account.Credits;
                NameChosen = client.Account.NameChosen;
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
                PlayerSkin = Client.Character.Outfit;

                try
                {
                    Locked = client.Account.Database.GetLockeds(client.Account);
                    Ignored = client.Account.Database.GetIgnoreds(client.Account);
                    Muted = client.Account.Muted;
                }
                catch (Exception) { }

                Inventory =
                        client.Character.Equipments.Select(
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

        public override void Move(float x, float y)
        { base.Move(x, y); }

        public void Death(string killer, ObjectDesc desc = null)
        {
            if (dying)
                return;

            dying = true;

            if (Client.State == ProtocolState.Disconnected)
                return;
            
            // TODO: implement corpse spawn.

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

            try
            {
                SaveToCharacter();

                GameServer.Manager.Database.SaveCharacter(Client.Account, Client.Character, true);
                GameServer.Manager.Database.Death(GameServer.Manager.GameData, Client.Account, Client.Character, killer);

                if (Owner.Id != -6)
                {
                    DEATH _death = new DEATH
                    {
                        AccountId = AccountId,
                        CharId = Client.Character.CharId,
                        Killer = killer,
                        zombieId = -1,
                        zombieType = -1
                    };

                    Client.SendMessage(_death);

                    Owner.Timers.Add(new WorldTimer(1000, (w, t) => GameServer.Manager.TryDisconnect(Client, DisconnectReason.CHARACTER_IS_DEAD)));

                    Owner.LeaveWorld(this);
                }
                else
                    GameServer.Manager.TryDisconnect(Client, DisconnectReason.CHARACTER_IS_DEAD_ERROR);
            }
            catch (Exception) { }
        }

        public override void Init(World owner)
        {
            MaxHackEntries = 0;

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

            List<int> gifts = Client.Account.Gifts.ToList();

            Client.SendMessage(new GLOBAL_NOTIFICATION
            {
                Type = 0,
                Text = gifts.Count > 0 ? "giftChestOccupied" : "giftChestEmpty"
            });

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
                Experience += exp;
                UpdateCount++;

                foreach (var i in Owner.PlayersCollision.HitTest(X, Y, 16).Where(i => i != this).OfType<Player>())
                {
                    try
                    {
                        i.Experience += exp;
                        i.UpdateCount++;
                        i.VerifyLevel();
                    }
                    catch (Exception) { }
                }
            }

            return VerifyLevel();
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
            if (Client == null)
                return;

            if (!KeepAlive(time) || Client.State == ProtocolState.Disconnected)
            {
                if (Owner != null)
                    Owner.LeaveWorld(this);
                else
                    WorldInstance.LeaveWorld(this);

                return;
            }

            if (!HasConditionEffect(ConditionEffects.Paused))
            {
                HandleRegen(time);

                HandleGround(time);
            }

            HandleTrade?.Tick(time);

            try
            {
                HandleQuest(time);
            }
            catch (NullReferenceException) { }

            HandleEffects(time);

            if (MP < 0)
                MP = 0;

            if (Owner != null)
            {
                HandleNewTick(time);

                HandleUpdate(time);
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