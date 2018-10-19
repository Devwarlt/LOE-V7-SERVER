#region

using LoESoft.GameServer.networking;
using System;
using System.Collections.Concurrent;
using System.Threading;
using static LoESoft.GameServer.networking.Client;

#endregion

namespace LoESoft.GameServer.realm
{
    #region

    using Work = Tuple<Client, Message>;

    #endregion

    public class NetworkTicker
    {
        private static readonly ConcurrentQueue<Work> pendings = new ConcurrentQueue<Work>();
        private static SpinWait loopLock = new SpinWait();

        public NetworkTicker(RealmManager manager)
        {
            Manager = manager;
        }

        public RealmManager Manager { get; private set; }

        public void AddPendingPacket(Client parrent, Message pkt) => pendings.Enqueue(new Work(parrent, pkt));

        public void TickLoop()
        {
            do
            {
                try
                {
                    if (Manager.Terminating)
                        break;

                    loopLock.Reset();

                    while (pendings.TryDequeue(out Work work))
                    {
                        try
                        {
                            if (Manager.Terminating)
                                return;

                            if (work.Item1.State == ProtocolState.Disconnected)
                            {
                                Manager.TryDisconnect(work.Item1, DisconnectReason.NETWORK_TICKER_DISCONNECT);
                                continue;
                            }
                            try
                            {
                                work.Item1.ProcessMessage(work.Item2);
                            }
                            catch (Exception) { }
                        }
                        catch (Exception) { }
                    }
                    while (pendings.Count == 0 && !Manager.Terminating)
                        loopLock.SpinOnce();
                }
                catch (Exception) { }
            } while (true);
        }
    }
}