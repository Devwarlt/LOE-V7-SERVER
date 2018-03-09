using LoESoft.Core.models;
using LoESoft.GameServer.networking.experimental;
using LoESoft.GameServer.realm;
using System.Net.Sockets;

namespace LoESoft.GameServer.networking
{
    public enum EProtocolState
    {
        Connected,
        Handshaked,
        Ready,
        Disconnected
    }

    public partial class Client
    {
        internal readonly object DcLock = new object();

        public readonly EServer _eserver;

        private readonly ENetworkManager _ehandler;

        public Client(
            EServer eserver,
            RealmManager manager,
            SocketAsyncEventArgs outgoing,
            SocketAsyncEventArgs incoming,
            byte[] outgoingCipher,
            byte[] incomingCipher
            )
        {
            _eserver = eserver;
            _ehandler = new ENetworkManager(this, outgoing, incoming);

            Manager = manager;

            IncomingCipher = new RC4(incomingCipher);
            OutgoingCipher = new RC4(outgoingCipher);
        }

        public void BeginHandling(Socket skt)
        {
            Log.Info($"Receiving new client from socket DNS '{skt.RemoteEndPoint.ToString().Split(':')[0]}'.");

            Socket = skt;
            _ehandler.BeginHandling(Socket);
        }

        public void Reset()
        {
            Account = null;
            Character = null;
            Player = null;

            _ehandler.Reset();
        }
    }
}
