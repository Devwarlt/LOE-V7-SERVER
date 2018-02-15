﻿#region

using common.config;
using System;
using System.Collections.Generic;
using System.Linq;
using gameserver.networking.outgoing;
using gameserver.networking.incoming;
using gameserver.networking;
using gameserver.networking.error;
using FAILURE = gameserver.networking.incoming.FAILURE;
using gameserver.realm.world;

#endregion

namespace gameserver.realm.entity.player
{
    partial class Player
    {
        public enum PlayerShootStatus
        {
            OK,
            ITEM_MISMATCH,
            COOLDOWN_STILL_ACTIVE,
            NUM_PROJECTILE_MISMATCH,
            CLIENT_TOO_SLOW,
            CLIENT_TOO_FAST
        }

        public class TimeCop
        {
            private readonly int[] _clientDeltaLog;
            private readonly int[] _serverDeltaLog;
            private readonly int _capacity;
            private int _index;
            private int _clientElapsed;
            private int _serverElapsed;
            private int _lastClientTime;
            private int _lastServerTime;
            private int _count;

            public TimeCop(int capacity = 20)
            {
                _capacity = capacity;
                _clientDeltaLog = new int[_capacity];
                _serverDeltaLog = new int[_capacity];
            }

            public void Push(int clientTime, int serverTime)
            {
                int dtClient = 0;
                int dtServer = 0;
                if (_count != 0)
                {
                    dtClient = clientTime - _lastClientTime;
                    dtServer = serverTime - _lastServerTime;
                }
                _count++;
                _index = (_index + 1) % _capacity;
                _clientElapsed += dtClient - _clientDeltaLog[_index];
                _serverElapsed += dtServer - _serverDeltaLog[_index];
                _clientDeltaLog[_index] = dtClient;
                _serverDeltaLog[_index] = dtServer;
                _lastClientTime = clientTime;
                _lastServerTime = serverTime;
            }

            public int LastClientTime() => _lastClientTime;

            public int LastServerTime() => _lastServerTime;

            public float TimeDiff() => _count < _capacity ? 1 : (float)_clientElapsed / _serverElapsed;
        }

        public PlayerShootStatus ValidatePlayerShoot(Item item, int time)
        {
            if (item != Inventory[0])
                return PlayerShootStatus.ITEM_MISMATCH;

            int dt = (int)(1 / StatsManager.GetAttackFrequency() * 1 / item.RateOfFire);

            if (time < _time.LastClientTime() + dt)
                return PlayerShootStatus.COOLDOWN_STILL_ACTIVE;

            if (time != _lastShootTime)
            {
                _lastShootTime = time;

                if (_shotsLeft != 0 && _shotsLeft < item.NumProjectiles)
                {
                    _shotsLeft = 0;
                    _time.Push(time, Environment.TickCount);
                    return PlayerShootStatus.NUM_PROJECTILE_MISMATCH;
                }
                _shotsLeft = 0;
            }

            _shotsLeft++;

            if (_shotsLeft >= item.NumProjectiles)
                _time.Push(time, Environment.TickCount);

            float timeDiff = _time.TimeDiff();

            if (timeDiff < MinTimeDiff)
                return PlayerShootStatus.CLIENT_TOO_SLOW;
            if (timeDiff > MaxTimeDiff)
                return PlayerShootStatus.CLIENT_TOO_FAST;

            return PlayerShootStatus.OK;
        }

        public void DropNextRandom() => client.Random.NextInt();

        public static class Resize16x16Skins
        {
            public static List<int> RotMGSkins16x16 = new List<int>
            {
                0x0403, // Olive Gladiator
                0x0404, // Ivory Gladiator
                0x0405, // Rosen Blade
                0x0406, // Djinja
                0x2add  // Beefcake Rogue
            };

            public static bool IsSkin16x16Type(int objectId) => RotMGSkins16x16.Contains(objectId);
        }

        ~Player()
        {
            WorldInstance = null;
            Quest = null;
        }

        public enum AccType : byte
        {
            VIP_ACCOUNT,
            LEGENDS_OF_LOE_ACCOUNT,
            NULL
        }

        private Tuple<bool, AccType> GetAccountType() => (AccountType >= (int)accountType.VIP_ACCOUNT && AccountType <= (int)accountType.LEGENDS_OF_LOE_ACCOUNT) ? Tuple.Create(true, AccountType == (int)accountType.VIP_ACCOUNT ? AccType.VIP_ACCOUNT : AccType.LEGENDS_OF_LOE_ACCOUNT) : Tuple.Create(false, AccType.NULL);

        public void CalculateBoost()
        {


            CheckSetTypeSkin();

            if (Boost == null)
                Boost = new int[12];

            if (ActivateBoost == null)
            {
                ActivateBoost = new ActivateBoost[8];
                for (int i = 0; i < 8; i++)
                    ActivateBoost[i] = new ActivateBoost();
            }

            for (var i = 0; i < Boost.Length; i++)
                Boost[i] = 0;

            for (var i = 0; i < 4; i++)
            {
                if (Inventory.Length < i || Inventory.Length == 0)
                    return;

                if (Inventory[i] == null)
                    continue;

                foreach (var pair in Inventory[i].StatsBoost)
                {
                    if (pair.Key == StatsType.MAX_HP_STAT)
                        Boost[0] += GetAccountType().Item1 ? (Stats[0] / (GetAccountType().Item2 == AccType.VIP_ACCOUNT ? 10 : 20 / 3)) + pair.Value : pair.Value;
                    if (pair.Key == StatsType.MAX_MP_STAT)
                        Boost[1] += GetAccountType().Item1 ? (Stats[1] / (GetAccountType().Item2 == AccType.VIP_ACCOUNT ? 10 : 20 / 3)) + pair.Value : pair.Value;
                    if (pair.Key == StatsType.ATTACK_STAT)
                        Boost[2] += GetAccountType().Item1 ? (Stats[2] / (GetAccountType().Item2 == AccType.VIP_ACCOUNT ? 10 : 20 / 3)) + pair.Value : pair.Value;
                    if (pair.Key == StatsType.DEFENSE_STAT)
                        Boost[3] += GetAccountType().Item1 ? (Stats[3] / (GetAccountType().Item2 == AccType.VIP_ACCOUNT ? 10 : 20 / 3)) + pair.Value : pair.Value;
                    if (pair.Key == StatsType.SPEED_STAT)
                        Boost[4] += GetAccountType().Item1 ? (Stats[4] / (GetAccountType().Item2 == AccType.VIP_ACCOUNT ? 10 : 20 / 3)) + pair.Value : pair.Value;
                    if (pair.Key == StatsType.VITALITY_STAT)
                        Boost[5] += GetAccountType().Item1 ? (Stats[5] / (GetAccountType().Item2 == AccType.VIP_ACCOUNT ? 10 : 20 / 3)) + pair.Value : pair.Value;
                    if (pair.Key == StatsType.WISDOM_STAT)
                        Boost[6] += GetAccountType().Item1 ? (Stats[6] / (GetAccountType().Item2 == AccType.VIP_ACCOUNT ? 10 : 20 / 3)) + pair.Value : pair.Value;
                    if (pair.Key == StatsType.DEXTERITY_STAT)
                        Boost[7] += GetAccountType().Item1 ? (Stats[7] / (GetAccountType().Item2 == AccType.VIP_ACCOUNT ? 10 : 20 / 3)) + pair.Value : pair.Value;
                }
            }

            for (int i = 0; i < 8; i++)
                Boost[i] += ActivateBoost[i].GetBoost();

            if (setTypeBoosts == null)
                return;

            for (var i = 0; i < 8; i++)
                Boost[i] += setTypeBoosts[i];
        }

        public bool CompareName(string name) => name.ToLower().Split(' ')[0].StartsWith("[") || Name.Split(' ').Length == 1 ? Name.ToLower().StartsWith(name.ToLower()) : Name.Split(' ')[1].ToLower().StartsWith(name.ToLower());

        public void SaveToCharacter()
        {
            var chr = client.Character;
            chr.Experience = Experience;
            chr.Level = Level;
            chr.Tex1 = Texture1;
            chr.Tex2 = Texture2;
            chr.Fame = Fame;
            chr.HP = HP;
            chr.MP = MP;
            if (PetID != 0)
                chr.Pet = PetID;
            try
            {
                switch (Inventory.Length)
                {
                    case 20:
                        int[] equip = Inventory.Select(_ => _?.ObjectType ?? -1).ToArray();
                        int[] backpack = new int[8];
                        Array.Copy(equip, 12, backpack, 0, 8);
                        Array.Resize(ref equip, 12);
                        chr.Items = equip;
                        chr.Backpack = backpack;
                        break;
                    default:
                        chr.Items = Inventory.Select(_ => _?.ObjectType ?? -1).ToArray();
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            chr.Stats = Stats;
            chr.HealthPotions = HealthPotions;
            chr.MagicPotions = MagicPotions;
            chr.HasBackpack = HasBackpack;
            chr.Skin = PlayerSkin;
            chr.LootDropTimer = (int)LootDropBoostTimeLeft;
            chr.LootTierTimer = (int)LootTierBoostTimeLeft;
            chr.FameStats = FameCounter.Stats.Write();
            chr.LastSeen = DateTime.Now;
        }

        private bool CheckResurrection()
        {
            for (var i = 0; i < 4; i++)
            {
                var item = Inventory[i];
                if (item == null || !item.Resurrects) continue;

                HP = Stats[0] + Stats[0];
                MP = Stats[1] + Stats[1];
                Inventory[i] = null;
                Owner.BroadcastPacket(new TEXT
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "",
                    Text = $"{Name}'s {item.ObjectId} breaks and he disappears",
                    NameColor = 0x123456,
                    TextColor = 0x123456
                }, null);
                client.Reconnect(new RECONNECT
                {
                    Host = "",
                    Port = Settings.GAMESERVER.PORT,
                    GameId = World.NEXUS_ID,
                    Name = "Nexus",
                    Key = Empty<byte>.Array,
                });

                resurrecting = true;
                return true;
            }
            return false;
        }

        private void GenerateGravestone()
        {
            var maxed = (from i in Program.Manager.GameData.ObjectTypeToElement[ObjectType].Elements("LevelIncrease") let xElement = Program.Manager.GameData.ObjectTypeToElement[ObjectType].Element(i.Value) where xElement != null let limit = int.Parse(xElement.Attribute("max").Value) let idx = StatsManager.StatsNameToIndex(i.Value) where Stats[idx] >= limit select limit).Count();

            ushort objType;
            int? time;
            switch (maxed)
            {
                case 8: { objType = 0x0735; time = null; } break;
                case 7: { objType = 0x0734; time = null; } break;
                case 6: { objType = 0x072b; time = null; } break;
                case 5: { objType = 0x072a; time = null; } break;
                case 4: { objType = 0x0729; time = null; } break;
                case 3: { objType = 0x0728; time = null; } break;
                case 2: { objType = 0x0727; time = null; } break;
                case 1: { objType = 0x0726; time = null; } break;
                default:
                    if (Level <= 1) { objType = 0x0723; time = 30 * 1000; }
                    else if (Level < 20) { objType = 0x0724; time = 60 * 1000; }
                    else { objType = 0x0725; time = 5 * 60 * 1000; }
                    break;
            }
            var obj = new GameObject(objType, time, true, time != null, false);
            obj.Move(X, Y);
            obj.Name = Name;
            Owner.EnterWorld(obj);
        }

        private void HandleRegen(RealmTime time)
        {
            if (HP == Stats[0] + Boost[0] || !CanHpRegen())
                hpRegenCounter = 0;
            else
            {
                hpRegenCounter += StatsManager.GetHPRegen() * time.ElapsedMsDelta / 1000f;
                var regen = (int)hpRegenCounter;
                if (regen > 0)
                {
                    HP = Math.Min(Stats[0] + Boost[0], HP + regen);
                    hpRegenCounter -= regen;
                    UpdateCount++;
                }
            }

            if (MP == Stats[1] + Boost[1] || !CanMpRegen())
                mpRegenCounter = 0;
            else
            {
                mpRegenCounter += StatsManager.GetMPRegen() * time.ElapsedMsDelta / 1000f;
                var regen = (int)mpRegenCounter;
                if (regen <= 0) return;
                MP = Math.Min(Stats[1] + Boost[1], MP + regen);
                mpRegenCounter -= regen;
                UpdateCount++;
            }
        }

        public bool IsVisibleToEnemy()
        {
            if (HasConditionEffect(ConditionEffectIndex.Paused))
                return false;
            if (HasConditionEffect(ConditionEffectIndex.Invisible))
                return false;
            if (newbieTime > 0)
                return false;
            return true;
        }

        private bool CanHpRegen() => (HasConditionEffect(ConditionEffectIndex.Sick) || HasConditionEffect(ConditionEffectIndex.Bleeding) || OxygenBar == 0) ? false : true;

        private bool CanMpRegen() => (HasConditionEffect(ConditionEffectIndex.Quiet) || ninjaShoot) ? false : true;

        internal void SetNewbiePeriod() => newbieTime = 3000;

        internal void SetTPDisabledPeriod() => CanTPCooldownTime = 10 * 1000;

        public bool TPCooledDown() => CanTPCooldownTime > 0 ? false : true;

        public string ResolveGuildChatName() => Name;

        public new void Dispose() => tiles = null;

        public bool HasSlot(int slot) => Inventory[slot] != null;

        public void AwaitMove(int tickId) => _move.Enqueue(tickId);

        public void ClientTick(RealmTime time, MOVE pkt)
        {
            int lastClientTime = LastClientTime;
            long lastServerTime = LastServerTime;

            LastClientTime = pkt.Time;
            LastServerTime = time.TotalElapsedMs;

            if (lastClientTime == -1)
                return;

            _clientTimeLog.Enqueue(pkt.Time - lastClientTime);
            _serverTimeLog.Enqueue((int)(time.TotalElapsedMs - lastServerTime));

            if (_clientTimeLog.Count < 30)
                return;

            if (_clientTimeLog.Count > 30)
            {
                int ignore;
                _clientTimeLog.TryDequeue(out ignore);
                _serverTimeLog.TryDequeue(out ignore);
            }
        }

        public bool KeepAlive(RealmTime time)
        {
            try
            {
                if (client == null)
                    return false;

                if (_pingTime == -1)
                {
                    _pingTime = time.TotalElapsedMs - PingPeriod;
                    _pongTime = time.TotalElapsedMs;
                }

                if (time.TotalElapsedMs - _pongTime > DcThresold)
                {

                    string[] labels = new string[] { "{CLIENT_NAME}" };
                    string[] arguments = new string[] { (client?.Account?.Name == null ? "_null_" : client?.Account?.Name) };

                    if (arguments == new string[] { "_null_" })
                        return false;
                    else
                        client?.SendMessage(new FAILURE
                        {
                            ErrorId = (int)FailureIDs.JSON_DIALOG,
                            ErrorDescription =
                                JSONErrorIDHandler.
                                    FormatedJSONError(
                                        errorID: ErrorIDs.LOST_CONNECTION,
                                        labels: labels,
                                        arguments: arguments
                                    )
                        });
                    return false;
                }

                if (time.TotalElapsedMs - _pingTime < PingPeriod)
                    return true;

                _pingTime = time.TotalElapsedMs;

                client.SendMessage(new PING()
                {
                    Serial = (int)time.TotalElapsedMs
                });

                return UpdateOnPing();
            }
            catch (Exception e)
            {
                log4net.Info(e);
                return false;
            }
        }

        private bool UpdateOnPing()
        {
            try
            {
                if (!(Owner is Test))
                    SaveToCharacter();
                return true;
            }
            catch
            {
                client?.Save();
                return false;
            }
        }

        public void Pong(RealmTime time, PONG pkt)
        {
            try
            {
                updateLastSeen++;

                _cnt++;

                _sum += time.TotalElapsedMs - pkt.Time;
                TimeMap = _sum / _cnt;

                _latSum += (time.TotalElapsedMs - pkt.Serial) / 2;
                Latency = (int)_latSum / _cnt;

                _pongTime = time.TotalElapsedMs;
            }
            catch (Exception ex)
            {
                Log.Write(nameof(Player), $"Pong:\n{ex}");
            }
        }

        private static int GetExpGoal(int level) => 50 + (level - 1) * 100;

        private static int GetLevelExp(int level) => level == 1 ? 0 : 50 * (level - 1) + (level - 2) * (level - 1) * 50;

        private static int GetFameGoal(int fame)
        {
            if (fame >= 2000) return 0;
            if (fame >= 800) return 2000;
            if (fame >= 400) return 800;
            if (fame >= 150) return 400;
            return fame >= 20 ? 150 : 0;
        }

        public int GetStars()
        {
            var ret = 0;
            foreach (var i in FameCounter.ClassStats.AllKeys)
            {
                var entry = FameCounter.ClassStats[ushort.Parse(i)];
                if (entry.BestFame >= 2000) ret += 5;
                else if (entry.BestFame >= 800) ret += 4;
                else if (entry.BestFame >= 400) ret += 3;
                else if (entry.BestFame >= 150) ret += 2;
                else if (entry.BestFame >= 20) ret += 1;
            }
            return ret;
        }

        private static float Dist(Entity a, Entity b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public void SendAccountList(List<string> list, int id)
        {
            for (var i = 0; i < list.Count; i++)
                list[i] = list[i].Trim();

            client.SendMessage(new ACCOUNTLIST
            {
                AccountListId = id,
                AccountIds = list.ToArray(),
                LockAction = -1
            });
        }

        public void BroadcastSync(Message packet) => BroadcastSync(packet, _ => true);

        public void BroadcastSync(Message packet, Predicate<Player> cond)
        {
            if (worldBroadcast)
                Owner.BroadcastPacketSync(packet, cond);
            else
                pendingPackets.Enqueue(Tuple.Create(packet, cond));
        }

        private void BroadcastSync(IEnumerable<Message> packets)
        {
            foreach (var i in packets)
                BroadcastSync(i, _ => true);
        }

        private void BroadcastSync(IEnumerable<Message> packets, Predicate<Player> cond)
        {
            foreach (var i in packets)
                BroadcastSync(i, cond);
        }

        public void Flush()
        {
            if (Owner != null)
            {
                foreach (var i in Owner.Players.Values)
                    foreach (var j in pendingPackets.Where(j => j.Item2(i)))
                        i.client.SendMessage(j.Item1);
            }
            pendingPackets.Clear();
        }

        public void ChangeTrade(RealmTime time, CHANGETRADE pkt) => TradeHandler?.TradeChanged(this, pkt.Offers);

        public void AcceptTrade(RealmTime time, ACCEPTTRADE pkt) => TradeHandler?.AcceptTrade(this, pkt);

        public void CancelTrade(RealmTime time, CANCELTRADE pkt) => TradeHandler?.CancelTrade(this);

        public void TradeCanceled() => TradeHandler = null;

        private float UseWisMod(float value, int offset = 1)
        {
            double totalWisdom = Stats[6] + 2 * Boost[6];

            if (totalWisdom < 30)
                return value;

            double m = (value < 0) ? -1 : 1;
            double n = (value * totalWisdom / 150) + (value * m);
            n = Math.Floor(n * Math.Pow(10, offset)) / Math.Pow(10, offset);
            if (n - (int)n * m >= 1 / Math.Pow(10, offset) * m)
            {
                return ((int)(n * 10)) / 10.0f;
            }

            return (int)n;
        }

        internal static List<ushort> Special = new List<ushort>
        {
            0x750d, 0x750e, 0x222c, 0x222d
        };

        private static bool IsSpecial(ushort objType) => Special.Contains(objType) ? true : false;
    }
}