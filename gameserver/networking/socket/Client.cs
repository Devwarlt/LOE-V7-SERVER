#region

using LoESoft.Core;
using LoESoft.Core.config;
using LoESoft.GameServer.realm;
using System;
using System.Net.Sockets;

#endregion

namespace LoESoft.GameServer.networking
{
    public partial class Client : IDisposable
    {
        public DbChar Character { get; internal set; }
        public DbAccount Account { get; internal set; }
        public wRandom Random { get; internal set; }
        public int TargetWorld { get; internal set; }
        public string ConnectedBuild { get; internal set; }
        public Socket Socket { get; internal set; }
        public RealmManager Manager { get; private set; }
        public RC4 IncomingCipher { get; private set; }
        public RC4 OutgoingCipher { get; private set; }

        private NetworkHandler handler;
        private bool disposed;

        public Client(RealmManager manager, Socket skt)
        {
            Socket = skt;
            Manager = manager;

            IncomingCipher = new RC4(Settings.NETWORKING.INCOMING_CIPHER);
            OutgoingCipher = new RC4(Settings.NETWORKING.OUTGOING_CIPHER);

            handler = new NetworkHandler(this, Socket);
            handler.BeginHandling();
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