#region

using System.Linq;
using System.Net;
using System.Net.Sockets;
using LoESoft.GameServer.realm;
using static LoESoft.GameServer.networking.Client;
using System.Threading;
using LoESoft.Core.config;
using LoESoft.GameServer.networking.network;
using System;

#endregion

namespace LoESoft.GameServer.networking
{
    public class Server
    {
        public Server(
            RealmManager manager,
            int port,
            int maxConnections
            )
        {
            _manager = manager;
            _port = port;
            _maxConnections = maxConnections;
            _buffManager = new BufferManager((maxConnections + 1) * BufferSize * OpsToPreAllocate, BufferSize);
            _eventArgsPoolAccept = new SocketAsyncEventArgsManager(MaxSimultaneousAcceptOps);
            _clientManager = new ClientManager(maxConnections + 1);
            _maxConnectionsEnforcer = new Semaphore(maxConnections, maxConnections);
            _outgoingCipher = Settings.NETWORKING.OUTGOING_CIPHER;
            _incomingCipher = Settings.NETWORKING.INCOMING_CIPHER;

            Init();
        }

        private void Init()
        {
            _buffManager.InitBuffer();

            for (int i = 0; i < MaxSimultaneousAcceptOps; i++)
                _eventArgsPoolAccept.Push(CreateNewAcceptEventArgs());

            for (int i = 0; i < _maxConnections + 1; i++)
            {
                var outgoing = CreateNewSendEventArgs();
                var incoming = CreateNewReceiveEventArgs();
                _clientManager.Push(new Client(this, _manager, outgoing, incoming, _outgoingCipher, _incomingCipher));
            }
        }

        private readonly BufferManager _buffManager;
        private readonly SocketAsyncEventArgsManager _eventArgsPoolAccept;
        private readonly Semaphore _maxConnectionsEnforcer;
        private readonly byte[] _outgoingCipher;
        private readonly byte[] _incomingCipher;

        private const int MaxSimultaneousAcceptOps = 10;
        private const int OpsToPreAllocate = 2;
        private const int BackLog = 1024;

        private Socket _listenSocket { get; set; }

        internal readonly ClientManager _clientManager;

        public const int BufferSize = 0x20000;

        public RealmManager _manager { get; private set; }
        public int _port { get; private set; }
        public int _maxConnections { get; private set; }

        public void Start()
        {
            var lep = new IPEndPoint(IPAddress.Any, _port);
            _listenSocket = new Socket(lep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _listenSocket.Bind(lep);
            _listenSocket.Listen(BackLog);

            StartAccept();
        }

        private SocketAsyncEventArgs CreateNewAcceptEventArgs()
        {
            var acceptEventArg = new SocketAsyncEventArgs();
            acceptEventArg.Completed += AcceptEventArg_Completed;
            return acceptEventArg;
        }

        private SocketAsyncEventArgs CreateNewSendEventArgs()
        {
            var eventArgs = new SocketAsyncEventArgs();
            _buffManager.SetBuffer(eventArgs);
            eventArgs.UserToken = new OutgoingToken(eventArgs.Offset);
            return eventArgs;
        }

        private SocketAsyncEventArgs CreateNewReceiveEventArgs()
        {
            var eventArgs = new SocketAsyncEventArgs();
            _buffManager.SetBuffer(eventArgs);
            eventArgs.UserToken = new IncomingToken(eventArgs.Offset);
            return eventArgs;
        }

        private void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void StartAccept()
        {
            SocketAsyncEventArgs acceptEventArg;

            if (_eventArgsPoolAccept.Count > 1)
                try
                {
                    acceptEventArg = _eventArgsPoolAccept.Pop();
                }
                catch
                {
                    acceptEventArg = CreateNewAcceptEventArgs();
                }
            else
                acceptEventArg = CreateNewAcceptEventArgs();
            
            _maxConnectionsEnforcer.WaitOne();

            try
            {
                bool willRaiseEvent = _listenSocket.AcceptAsync(acceptEventArg);
                if (!willRaiseEvent)
                    ProcessAccept(acceptEventArg);
            }
            catch { }
        }

        private void ProcessAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            if (acceptEventArgs.SocketError != SocketError.Success)
            {
                StartAccept();
                HandleBadAccept(acceptEventArgs);
                return;
            }
            
            acceptEventArgs.AcceptSocket.NoDelay = Settings.NETWORKING.DISABLE_NAGLES_ALGORITHM;
            (_clientManager.Pop()).BeginHandling(acceptEventArgs.AcceptSocket);
            
            acceptEventArgs.AcceptSocket = null;
            _eventArgsPoolAccept.Push(acceptEventArgs);
            
            StartAccept();
        }

        private void HandleBadAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            acceptEventArgs.AcceptSocket.Close();
            _eventArgsPoolAccept.Push(acceptEventArgs);
        }

        public void Stop()
        {
            try
            {
                _listenSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception e)
            {
                var se = e as SocketException;
                if (se == null || se.SocketErrorCode != SocketError.NotConnected)
                    GameServer.log.Error(e);
            }
            _listenSocket.Close();

            foreach (ClientData cData in _manager.ClientManager.Values.ToArray())
            {
                cData.Client.Save();
                _manager.TryDisconnect(cData.Client, DisconnectReason.STOPING_SERVER);
            }

            DisposeAllSaeaObjects();
        }

        private void DisposeAllSaeaObjects()
        {
            while (_eventArgsPoolAccept.Count > 0)
            {
                var eventArgs = _eventArgsPoolAccept.Pop();
                eventArgs.Dispose();
            }

            while (_clientManager.Count > 0)
            {
                var client = _clientManager.Pop();
                client = null;
            }
        }

        public void TryDisconnect(Client client)
        {
            try
            {
                client.Socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception e)
            {
                var se = e as SocketException;
                if (se == null || se.SocketErrorCode != SocketError.NotConnected)
                    GameServer.log.Error(e);
            }
            client.Socket.Close();

            client.Reset();
            _clientManager.Push(client);

            try
            {
                _maxConnectionsEnforcer.Release();
            }
            catch (SemaphoreFullException e)
            {
                GameServer.log.Error(e);
            }
        }
    }
}