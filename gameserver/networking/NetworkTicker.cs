#region

using System;
using System.Collections.Concurrent;
using System.Threading;
using log4net;
using LoESoft.GameServer.networking;

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
        private readonly ILog log = LogManager.GetLogger(typeof(NetworkTicker));

        public NetworkTicker(RealmManager manager)
        {
            Manager = manager;
        }

        public RealmManager Manager { get; private set; }

        public void AddPendingPacket(Client parrent, Message pkt) => pendings.Enqueue(new Work(parrent, pkt));

        public void TickLoop()
        {
            log.Info("Network loop started.");
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
                                Manager.TryDisconnect(work.Item1);
                                continue;
                            }
                            try
                            {
                                work.Item1.ProcessMessage(work.Item2);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                    }
                    while (pendings.Count == 0 && !Manager.Terminating)
                        loopLock.SpinOnce();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            } while (true);
            log.Info("Network loop stopped.");
        }
    }
}