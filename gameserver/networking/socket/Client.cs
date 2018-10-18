﻿#region

using LoESoft.Core;
using LoESoft.Core.config;
using LoESoft.GameServer.realm;
using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;

#endregion

namespace LoESoft.GameServer.networking
{
    public partial class Client : IDisposable
    {
        public bool IsDomainReceived;

        public static readonly int MaxNumberAttemptsPerCTT = 5;
        public static readonly int ACCOUNT_IN_USE_TIMEOUT = 10; // in seconds

        public static ConcurrentDictionary<string, DateTime> AccountInUseManager = new ConcurrentDictionary<string, DateTime>();
        public static ConcurrentDictionary<string, Position> ReconnectManager = new ConcurrentDictionary<string, Position>();

        public void AddReconnect(Position position)
            => ReconnectManager.TryAdd(AccountId, position);

        public void RemoveReconnect()
            => ReconnectManager.TryRemove(AccountId, out Position position);

        public bool CheckReconnect
            => ReconnectManager.ContainsKey(AccountId);

        public void AddAccountInUse()
            => AccountInUseManager.TryAdd(AccountId, DateTime.Now);

        public void RemoveAccountInUse()
            => AccountInUseManager.TryRemove(AccountId, out DateTime time);

        public double CheckAccountInUseTimeout
            => AccountInUseManager.ContainsKey(AccountId) ?
            ACCOUNT_IN_USE_TIMEOUT - (DateTime.Now - AccountInUseManager[AccountId]).TotalSeconds :
            -1;

        public string AccountId { get; set; }
        public Thread AccountInUseMonitor { get; private set; }
        public DbChar Character { get; internal set; }
        public DbAccount Account { get; internal set; }
        public wRandom Random { get; internal set; }
        public int TargetWorld { get; internal set; }
        public string ConnectedBuild { get; internal set; }
        public Socket Socket { get; internal set; }
        public RealmManager Manager { get; private set; }
        public RC4 IncomingCipher { get; private set; }
        public RC4 OutgoingCipher { get; private set; }
        public int CTTUses { get; set; }

        private NetworkHandler handler;
        private bool disposed;

        public Client(RealmManager manager, Socket skt)
        {
            CTTUses = 0;

            Socket = skt;
            Manager = manager;

            IncomingCipher = new RC4(Settings.NETWORKING.INCOMING_CIPHER);
            OutgoingCipher = new RC4(Settings.NETWORKING.OUTGOING_CIPHER);

            handler = new NetworkHandler(this, Socket);
            handler.BeginHandling();

            AccountId = "-1";
            AccountInUseMonitor = new Thread(() => InitializeAccountInUseMonitor());
        }

        public void InitializeAccountInUseMonitor()
        {
            bool completed = false;

            do
            {
                double timeout = CheckAccountInUseTimeout;

                // Prevent often account in use notification.
                foreach (var i in AccountInUseManager)
                    if (timeout == -1)
                    {
                        completed = true;
                        break;
                    }
                    else
                    {
                        timeout = ACCOUNT_IN_USE_TIMEOUT - (DateTime.Now - AccountInUseManager[AccountId]).TotalSeconds;
                        break;
                    }

                Thread.Sleep(1 * 1000);

                if (timeout <= 0)
                {
                    completed = true;

                    RemoveAccountInUse();
                    break;
                }
            } while (!completed);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "handler")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            if (disposed)
                return;

            try
            {
                IncomingCipher = null;
                OutgoingCipher = null;
                Socket = null;
                Character = null;
                Account = null;

                if (Player.PetID != 0 && Player.Pet != null)
                    Player.Owner.LeaveWorld(Player.Pet);

                Player = null;
                Random = null;
                ConnectedBuild = null;
            }
            catch
            { return; }
            finally
            { disposed = true; }
        }
    }
}