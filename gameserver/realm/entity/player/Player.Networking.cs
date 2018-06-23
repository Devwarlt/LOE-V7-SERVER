#region

using System;
using LoESoft.GameServer.networking.outgoing;
using LoESoft.GameServer.networking.incoming;
using LoESoft.GameServer.networking;
using LoESoft.Core.models;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using LoESoft.Core.config;

#endregion

namespace LoESoft.GameServer.realm.entity.player
{
    partial class Player
    {
        private int PingPeriod => 5000;
        private int LagLatency => 10000;

        private readonly ConcurrentQueue<long> _updateAckTimeout = new ConcurrentQueue<long>();

        public void PingReset(long time, bool save = false, bool sendPing = false)
        {
            _pingTime = time - PingPeriod;
            _pongTime = time;

            if (save)
                SaveToCharacter();

            if (sendPing)
                Client.SendMessage(new PING() { Serial = (int)time });
        }

        public bool PlayerNetworkingHandler(RealmTime time)
        {
            try
            {
                long elapsedTime = time.TotalElapsedMs;
                bool onPing = elapsedTime - _pingTime >= PingPeriod;
                bool onLag = elapsedTime - _pongTime >= LagLatency;

                if (Client == null)
                    return false;

                if (onLag)
                {
                    SendHelp($"Connection lost, reconnecting to world {Owner.Name}...");

                    Thread.Sleep(3 * 1000);

                    Client.AddReconnect(new Position(X, Y));
                    Client.Reconnect(new RECONNECT
                    {
                        Host = "",
                        Port = Settings.SERVER_MODE != Settings.ServerMode.Local ? Settings.APPENGINE.PRODUCTION_PORT : Settings.APPENGINE.TESTING_PORT,
                        GameId = Owner.Id,
                        Name = Owner.Name,
                        Key = Empty<byte>.Array,
                    });

                    return false;
                }

                if (onPing)
                    PingReset(elapsedTime, true, true);

                return true;
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return false;
            }
        }

        public void Pong(RealmTime time, PONG pkt)
        {
            try
            {
                _cnt++;

                _sum += time.TotalElapsedMs - pkt.Time;
                TimeMap = _sum / _cnt;

                _latSum += (time.TotalElapsedMs - pkt.Serial) / 2;
                Latency = (int)_latSum / _cnt;

                _pongTime = time.TotalElapsedMs;
            }
            catch (Exception) { }
        }

        public void AwaitUpdateAck(long serverTime)
            => _updateAckTimeout.Enqueue(serverTime + LagLatency);

        public void UpdateAckReceived()
        {
            if (!_updateAckTimeout.TryDequeue(out long ignored))
                return;
        }

        public long C2STime(long clientTime)
            => clientTime + TimeMap;

        public void Flush()
        {
            if (Owner != null)
            {
                foreach (var i in Owner.Players.Values)
                    foreach (var j in pendingPackets.Where(j => j.Item2(i)))
                        i.Client.SendMessage(j.Item1);
            }

            pendingPackets.Clear();
        }

        public void BroadcastSync(Message message)
            => BroadcastSync(message, _ => true);

        public void BroadcastSync(Message message, Predicate<Player> cond)
        {
            if (worldBroadcast)
                Owner.BroadcastMessageSync(message, cond);
            else
                pendingPackets.Enqueue(Tuple.Create(message, cond));
        }

        private void BroadcastSync(IEnumerable<Message> messages)
        {
            foreach (var i in messages)
                BroadcastSync(i, _ => true);
        }

        private void BroadcastSync(IEnumerable<Message> messages, Predicate<Player> cond)
        {
            foreach (var i in messages)
                BroadcastSync(i, cond);
        }
    }
}