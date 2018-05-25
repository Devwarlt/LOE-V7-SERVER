#region

using System;
using System.Collections.Generic;
using System.Linq;
using LoESoft.GameServer.networking.outgoing;
using LoESoft.GameServer.networking.incoming;
using LoESoft.GameServer.networking;
using LoESoft.GameServer.networking.error;
using FAILURE = LoESoft.GameServer.networking.incoming.FAILURE;
using LoESoft.GameServer.realm.world;
using LoESoft.Core;

#endregion

namespace LoESoft.GameServer.realm.entity.player
{
    partial class Player
    {
        public static void HandleQuests(EmbeddedData data)
        {
            foreach (var i in data.ObjectDescs.Values)
                if (i.Enemy && i.Quest)
                    RealmManager.QuestPortraits.Add(i.ObjectId, i.Level);
        }

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

        public void DropNextRandom() => Client.Random.NextInt();

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
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

        private Tuple<bool, AccType> GetAccountType() => (AccountType >= (int)Core.config.AccountType.VIP_ACCOUNT && AccountType <= (int)Core.config.AccountType.LEGENDS_OF_LOE_ACCOUNT) ? Tuple.Create(true, AccountType == (int)Core.config.AccountType.VIP_ACCOUNT ? AccType.VIP_ACCOUNT : AccType.LEGENDS_OF_LOE_ACCOUNT) : Tuple.Create(false, AccType.NULL);

        public bool CompareName(string name) => name.ToLower().Split(' ')[0].StartsWith("[") || Name.Split(' ').Length == 1 ? Name.ToLower().StartsWith(name.ToLower()) : Name.Split(' ')[1].ToLower().StartsWith(name.ToLower());

        public void SaveToCharacter()
        {
            var chr = Client.Character;
            chr.Experience = Experience;
            chr.Level = Level;
            chr.HP = HP;
            chr.MP = MP;
            chr.Equipments = Inventory.Select(_ => _?.ObjectType ?? -1).ToArray();
        }

        private void HandleRegen(RealmTime time)
        {
            // TODO: implement regen.
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

        private bool CanHpRegen() => !(HasConditionEffect(ConditionEffectIndex.Sick) || HasConditionEffect(ConditionEffectIndex.Bleeding));

        private bool CanMpRegen() => !HasConditionEffect(ConditionEffectIndex.Quiet);

        internal void SetNewbiePeriod() => newbieTime = 3000;

        internal void SetTPDisabledPeriod() => CanTPCooldownTime = 10 * 1000;

        public bool TPCooledDown() => CanTPCooldownTime > 0 ? false : true;

        public string ResolveGuildChatName() => Name;

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
                _clientTimeLog.TryDequeue(out int ignore);
                _serverTimeLog.TryDequeue(out ignore);
            }
        }

        public bool KeepAlive(RealmTime time)
        {
            try
            {
                if (Client == null)
                    return false;

                if (_pingTime == -1)
                {
                    _pingTime = time.TotalElapsedMs - PingPeriod;
                    _pongTime = time.TotalElapsedMs;
                }

                if (time.TotalElapsedMs - _pongTime > DcThresold)
                {

                    string[] labels = new string[] { "{CLIENT_NAME}" };
                    string[] arguments = new string[] { (Client?.Account?.Name ?? "_null_") };

                    if (arguments == new string[] { "_null_" })
                        return false;
                    else
                        Client?.SendMessage(new FAILURE
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

                Client.SendMessage(new PING()
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
                Client?.Save();
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
            catch (Exception) { }
        }

        #region "Feature: Infinite Leveling"
        private static int GetLevelExperience(int lvl)
            => lvl == 1 ? 0 : (75 * (lvl ^ 3) - 125 * (lvl ^ 2) + 900 * lvl) / 3;

        private static int GetNextLevelExperience(int lvl)
            => GetLevelExperience(lvl + 1);

        private bool VerifyLevel()
        {
            if (Experience >= ExperienceGoal)
            {
                Level++;
                ExperienceGoal = GetNextLevelExperience(Level);

                /* TODO: implement level up regular stats increment.
                 * - Hit Points
                 * - Magic Points
                 * - Speed Points
                 */

                UpdateCount++;

                // Announce to all players online when user advanced in level (every 50 levels).
                if (Level % 50 == 0)
                    foreach (var i in GameServer.Manager.ClientManager.Values)
                        i.Client.Player.SendInfo($"{Name} advanced from Level {Level - 1} to Level {Level}.");

                Quest = null;
                return true;
            }

            return false;
        }
        #endregion

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

            Client.SendMessage(new ACCOUNTLIST
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
                Owner.BroadcastMessageSync(packet, cond);
            else
                pendingMessages.Enqueue(Tuple.Create(packet, cond));
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
                    foreach (var j in pendingMessages.Where(j => j.Item2(i)))
                        i.Client.SendMessage(j.Item1);
            }

            pendingMessages.Clear();
        }

        public void ChangeTrade(RealmTime time, CHANGETRADE pkt) => HandleTrade?.TradeChanged(this, pkt.Offers);

        public void AcceptTrade(RealmTime time, ACCEPTTRADE pkt) => HandleTrade?.AcceptTrade(this, pkt);

        public void CancelTrade(RealmTime time, CANCELTRADE pkt) => HandleTrade?.CancelTrade(this);

        public void TradeCanceled() => HandleTrade = null;
    }
}