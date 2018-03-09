using LoESoft.Core;
using LoESoft.Core.models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static LoESoft.GameServer.networking.Client;

namespace LoESoft.GameServer.networking.experimental
{
    public class ENetworkManager
    {
        private readonly int _prefixLength;
        private readonly int _bufferSize;
        private readonly Client _client;
        private readonly SocketAsyncEventArgs _outgoing;
        private readonly SocketAsyncEventArgs _incoming;
        private ConcurrentQueue<Message>[] _pendings;

        public ENetworkManager(
            Client client,
            SocketAsyncEventArgs outgoing,
            SocketAsyncEventArgs incoming
            )
        {
            _prefixLength = EIncomingToken.PrefixLength;
            _bufferSize = EServer.BufferSize;
            _client = client;

            _incoming = incoming;
            _incoming.Completed += ProcessIncomingMessage;

            _outgoing = outgoing;
            _outgoing.Completed += ProcessOutgoingMessage;

            _pendings = new ConcurrentQueue<Message>[3];

            for (int i = 0; i < 3; i++)
                _pendings[i] = new ConcurrentQueue<Message>();
        }

        public void BeginHandling(Socket skt)
        {
            _outgoing.AcceptSocket = skt;
            _incoming.AcceptSocket = skt;

            _client.State = ProtocolState.Connected;
            
            /*StartReceive(_incoming);
            StartSend(_outgoing);*/
        }

        private void ProcessIncomingMessage(object sender, SocketAsyncEventArgs e)
        {
            var r = (EIncomingToken)e.UserToken;

            if (_client.State == ProtocolState.Disconnected || !_client.Socket.Connected)
            {
                GameServer.Manager.TryDisconnect(_client, DisconnectReason.SOCKET_IS_NOT_CONNECTED);
                r.Reset();
                return;
            }

            if (e.SocketError != SocketError.Success)
            {
                DisconnectReason dr = DisconnectReason.SOCKET_ERROR;

                if (e.SocketError != SocketError.ConnectionReset)
                    dr = DisconnectReason.CONNECTION_RESET;

                GameServer.Manager.TryDisconnect(_client, dr);
                return;
            }

            var bytesNotRead = e.BytesTransferred;

            if (bytesNotRead == 0)
            {
                GameServer.Manager.TryDisconnect(_client, DisconnectReason.BYTES_NOT_READY);
                return;
            }

            while (bytesNotRead > 0)
            {
                bytesNotRead = ReadMessageBytes(e, r, bytesNotRead);

                if (r.BytesRead == _prefixLength)
                {
                    r.MessageLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(r.MessageBytes, 0));

                    if (r.MessageBytes[0] == 0x3c
                            && r.MessageBytes[1] == 0x70
                            && r.MessageBytes[2] == 0x6f
                            && r.MessageBytes[3] == 0x6c
                            && r.MessageBytes[4] == 0x69)
                    {
                        var s = new NetworkStream(_client.Socket);
                        var wtr = new NWriter(s);
                        wtr.WriteNullTerminatedString(
                            @"<cross-domain-policy>" +
                            @"<allow-access-from domain=""*"" to-ports=""*"" />" +
                            @"</cross-domain-policy>");
                        wtr.Write((byte)'\r');
                        wtr.Write((byte)'\n');

                        r.Reset();
                        break;
                    }

                    if (r.MessageBytes[0] == 0xae
                        && r.MessageBytes[1] == 0x7a
                        && r.MessageBytes[2] == 0xf2
                        && r.MessageBytes[3] == 0xb2
                        && r.MessageBytes[4] == 0x95)
                    {
                        _client.Socket.Send(Encoding.ASCII.GetBytes($"{GameServer.Manager.MaxClients}:{GameServer.GameUsage}"));
                        break;
                    }

                    // discard invalid messages
                    if (r.MessageLength < _prefixLength
                        || r.MessageLength > _bufferSize)
                    {
                        r.Reset();
                        break;
                    }
                }

                if (r.BytesRead == r.MessageLength)
                {
                    if (_client.IsReady())
                        GameServer.Manager.ENetwork.AddPendingMessage(_client, r.GetMessageID(), r.GetMessageBody());

                    r.Reset();
                }
            }

            StartReceive(e);
        }

        private static int ReadMessageBytes(SocketAsyncEventArgs e, EIncomingToken r, int bytesNotRead)
        {
            var offset = r.BufferOffset + e.BytesTransferred - bytesNotRead;
            var remainingBytes = r.MessageLength - r.BytesRead;

            if (bytesNotRead < remainingBytes)
            {
                Buffer.BlockCopy(e.Buffer, offset, r.MessageBytes, r.BytesRead, bytesNotRead);
                r.BytesRead += bytesNotRead;
                return 0;
            }

            Buffer.BlockCopy(e.Buffer, offset, r.MessageBytes, r.BytesRead, remainingBytes);
            r.BytesRead = r.MessageLength;

            return bytesNotRead - remainingBytes;
        }

        private void StartReceive(SocketAsyncEventArgs e)
        {
            if (_client.State == ProtocolState.Disconnected || !_client.Socket.Connected)
            {
                GameServer.Manager.TryDisconnect(_client, DisconnectReason.SOCKET_IS_NOT_CONNECTED);
                return;
            }

            var token = (EIncomingToken)e.UserToken;
            var messageId = token.GetMessageID();
            var message = Message.Messages[messageId];

            Log.Info($"Receiving new incoming message: {message.ID}.");

            e.SetBuffer(e.Offset, _bufferSize);

            bool willRaiseEvent;
            try
            {

                if (e.SocketError != SocketError.Success)
                {
                    DisconnectReason dr = DisconnectReason.SOCKET_ERROR;

                    if (e.SocketError != SocketError.ConnectionReset)
                        dr = DisconnectReason.CONNECTION_RESET;

                    GameServer.Manager.TryDisconnect(_client, dr);
                    return;
                }
                else
                    willRaiseEvent = e.AcceptSocket.ReceiveAsync(e);

            }
            catch (ObjectDisposedException ex)
            {
                GameServer.log.Error(ex);
                return;
            }

            if (!willRaiseEvent)
                ProcessIncomingMessage(null, e);
        }

        private void ProcessOutgoingMessage(object sender, SocketAsyncEventArgs e)
        {
            var s = (EOutgoingToken)e.UserToken;

            if (_client.State == ProtocolState.Disconnected || !_client.Socket.Connected)
            {
                GameServer.Manager.TryDisconnect(_client, DisconnectReason.SOCKET_IS_NOT_CONNECTED);
                s.Reset();
                return;
            }

            if (e.SocketError != SocketError.Success)
            {
                GameServer.Manager.TryDisconnect(_client, DisconnectReason.SOCKET_ERROR);
                return;
            }

            s.BytesSent += e.BytesTransferred;
            s.BytesAvailable -= s.BytesSent;

            var delay = 0;
            if (s.BytesAvailable <= 0)
                delay = GameServer.Manager.Logic.MsPT;

            StartSend(e, delay);
        }

        private async void StartSend(SocketAsyncEventArgs e, int msDelay = 0)
        {
            if (_client.State == ProtocolState.Disconnected)
                return;

            var s = (EOutgoingToken)e.UserToken;

            if (msDelay > 0)
                await Task.Delay(msDelay);

            if (s.BytesAvailable <= 0)
            {
                s.Reset();
                FlushPending(s);
            }

            int bytesToSend = s.BytesAvailable > _bufferSize ?
                _bufferSize : s.BytesAvailable;

            e.SetBuffer(s.BufferOffset, bytesToSend);
            Buffer.BlockCopy(s.Data, s.BytesSent,
                e.Buffer, s.BufferOffset, bytesToSend);

            bool willRaiseEvent;
            try
            {
                if (e.SocketError != SocketError.Success)
                {
                    DisconnectReason dr = DisconnectReason.SOCKET_ERROR;

                    if (e.SocketError != SocketError.ConnectionReset)
                        dr = DisconnectReason.CONNECTION_RESET;

                    GameServer.Manager.TryDisconnect(_client, dr);
                    return;
                }
                else
                    willRaiseEvent = e.AcceptSocket.SendAsync(e);
            }
            catch (ObjectDisposedException ex)
            {
                GameServer.log.Error(ex);
                return;
            }

            if (!willRaiseEvent)
                ProcessOutgoingMessage(null, e);
        }

        public void SendMessage(Message msg, MessagePriority priority) =>
            _pendings[(int)priority].Enqueue(msg);

        public void SendMessages(IEnumerable<Message> msgs, MessagePriority priority)
        {
            foreach (var i in msgs)
                _pendings[(int)priority].Enqueue(i);
        }

        private bool FlushPending(EOutgoingToken s)
        {
            for (var i = 0; i < 3; i++)
                while (_pendings[i].TryDequeue(out Message message))
                {
                    var bytesWritten = message.EWrite(_client, s.Data, s.BytesAvailable);

                    if (bytesWritten == 0)
                    {
                        _pendings[i].Enqueue(message);
                        return true;
                    }

                    s.BytesAvailable += bytesWritten;
                }

            if (s.BytesAvailable <= 0)
                return false;

            return true;
        }

        public void Reset()
        {
            ((EOutgoingToken)_outgoing.UserToken).Reset();
            ((EIncomingToken)_incoming.UserToken).Reset();

            _pendings = new ConcurrentQueue<Message>[3];

            for (var i = 0; i < 3; i++)
                _pendings[i] = new ConcurrentQueue<Message>();
        }
    }
}
