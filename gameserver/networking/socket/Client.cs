using common;
using common.config;
using gameserver.realm;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace gameserver.networking
{
    public partial class Client : IDisposable
    {
        private bool disposed;

        public Socket Socket { get; internal set; }
        public RealmManager Manager { get; private set; }
        public RC4 IncomingCipher { get; private set; }
        public RC4 OutgoingCipher { get; private set; }
        private NetworkHandler handler;

        private class GameVersion
        {
            public string version;
            public bool access;
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

        public DbChar Character { get; internal set; }
        public DbAccount Account { get; internal set; }

        public wRandom Random { get; internal set; }

        public int Id { get; internal set; }
        public int TargetWorld { get; internal set; }
        public string ConnectedBuild { get; internal set; }

        public byte[] _IncomingCipher => new byte[] { 0x3D, 0xC1, 0xC4, 0x44, 0xF5, 0x78, 0xC1, 0xEC, 0x7B, 0xF4, 0x0A, 0x4D, 0xCA, 0x94, 0x93, 0xA2 };
        public byte[] _OutgoingCipher => new byte[] { 0x78, 0x9A, 0x63, 0x2F, 0x43, 0xA2, 0xF5, 0x5C, 0xB0, 0xA4, 0xC3, 0x99, 0x9C, 0x32, 0x4D, 0xA0 };

        public RC4 ProcessRC4(byte[] cipher) => new RC4(cipher);

        public Client(RealmManager manager, Socket skt)
        {
            Socket = skt;
            Manager = manager;
            IncomingCipher = ProcessRC4(_IncomingCipher);
            OutgoingCipher = ProcessRC4(_OutgoingCipher);
            BeginProcess();
        }

        public void BeginProcess()
        {
            //log.InfoFormat($"Received client @ {Socket.RemoteEndPoint}.");
            handler = new NetworkHandler(this, Socket);
            handler.BeginHandling();
        }

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
            Player?.Dispose();
            Player = null;
            Random = null;
            ConnectedBuild = null;
            disposed = true;
        }
    }
}