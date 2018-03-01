#region

using LoESoft.Core;
using LoESoft.Core.config;
using LoESoft.GameServer.realm;
using System;
using System.Collections.Generic;
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

            IncomingCipher = ProcessRC4(Settings.NETWORKING.INCOMING_CIPHER);
            OutgoingCipher = ProcessRC4(Settings.NETWORKING.OUTGOING_CIPHER);

            BeginProcess();
        }

        public RC4 ProcessRC4(byte[] cipher) => new RC4(cipher);

        public void BeginProcess()
        {
            handler = new NetworkHandler(this);
            handler.BeginHandling();
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
            if (disposed)
                return;
            handler?.Dispose();
            handler = null;
            IncomingCipher = null;
            OutgoingCipher = null;
            Manager = null;
            Socket = null;
            Character = null;
            Account = null;
            if (Player.PetID != 0 && Player.Pet != null)
                Player.Owner.LeaveWorld(Player.Pet);
            Player = null;
            Random = null;
            ConnectedBuild = null;
            disposed = true;
        }

        private class GameVersion
        {
            public string version;
            public bool access;
        }
    }
}