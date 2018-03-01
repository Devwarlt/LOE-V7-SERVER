#region

using LoESoft.Core;
using LoESoft.Core.config;
using LoESoft.GameServer.networking.network;
using LoESoft.GameServer.realm;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

#endregion

namespace LoESoft.GameServer.networking
{
    public enum ProtocolState
    {
        Connected,
        Handshaked,
        Ready,
        Disconnected
    }

    public partial class Client : IDisposable
    {
        internal readonly object DcLock = new object();

        public readonly Server _server;
        private readonly NetworkManager _handler;

        public DbChar Character { get; internal set; }
        public DbAccount Account { get; internal set; }
        public wRandom Random { get; internal set; }
        public int TargetWorld { get; internal set; }
        public string ConnectedBuild { get; internal set; }
        public Socket Socket { get; internal set; }
        public RealmManager _manager { get; private set; }
        public RC4 IncomingCipher { get; private set; }
        public RC4 OutgoingCipher { get; private set; }

        private bool Disposed { get; set; }

        public Client(
            Server server,
            RealmManager manager,
            SocketAsyncEventArgs outgoing,
            SocketAsyncEventArgs incoming,
            byte[] outgoingCipher,
            byte[] incomingCipher
            )
        {
            _server = server;
            _manager = manager;

            IncomingCipher = new RC4(incomingCipher);
            OutgoingCipher = new RC4(outgoingCipher);

            _handler = new NetworkManager(this, outgoing, incoming);
        }

        public void BeginHandling(Socket skt)
        {
            Socket = skt;
            _handler.BeginHandling(Socket);
        }

        public void Reset()
        {
            Account = null;
            Character = null;
            Player = null;

            _handler.Reset();
        }

        public static Tuple<string, bool> CheckGameVersion(string build)
        {
            List<GameVersion> gameVersions = new List<GameVersion>();

            foreach (Tuple<string, bool> i in Settings.NETWORKING.SUPPORTED_VERSIONS())
                gameVersions.Add(
                    new GameVersion()
                    {
                        version = i.Item1,
                        access = i.Item2
                    });

            foreach (GameVersion j in gameVersions)
                if (build == j.version && j.access)
                    return Tuple.Create(build, false);

            return Tuple.Create(gameVersions[gameVersions.Count - 1].version, true);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "handler")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            if (Disposed)
                return;
            
            IncomingCipher = null;
            OutgoingCipher = null;
            _manager = null;
            Socket = null;
            Character = null;
            Account = null;

            if (Player.PetID != 0 && Player.Pet != null)
                Player.Owner.LeaveWorld(Player.Pet);

            Player = null;
            Random = null;
            ConnectedBuild = null;
            Disposed = true;
        }

        private class GameVersion
        {
            public string version;
            public bool access;
        }
    }
}