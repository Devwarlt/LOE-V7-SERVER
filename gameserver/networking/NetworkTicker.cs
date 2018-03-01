#region

using System;
using System.Collections.Concurrent;
using LoESoft.GameServer.networking;
using log4net;

#endregion

namespace LoESoft.GameServer.realm
{
    using Work = Tuple<Client, MessageID, byte[]>;

    public class NetworkTicker
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NetworkTicker));
        private static readonly BlockingCollection<Work> Pendings = new BlockingCollection<Work>();
        private readonly RealmManager _manager;

        public NetworkTicker(RealmManager manager)
        {
            _manager = manager;
        }

        public void AddPendingMessage(Client client, MessageID id, byte[] message) =>
            Pendings.Add(new Work(client, id, message));

        public void TickLoop()
        {
            foreach (var pending in Pendings.GetConsumingEnumerable())
            {
                if (_manager.Terminating)
                {
                    Shutdown();
                    break;
                }

                if (pending.Item1.State == ProtocolState.Disconnected)
                {
                    _manager.TryDisconnect(pending.Item1);
                    continue;
                }

                try
                {
                    var message = Message.Messages[pending.Item2].CreateInstance();
                    message.Read(pending.Item1, pending.Item3, 0, pending.Item3.Length);
                    pending.Item1.ProcessMessage(message);
                }
                catch (Exception e)
                {
                    log.Error($"Error processing message ({(pending.Item1.Account != null ? pending.Item1.Account.Name : "")}, {pending.Item2})\n{e}");
                }
            }
        }

        public void Shutdown() =>
            Pendings.Add(new Work(null, 0, null));
    }
}