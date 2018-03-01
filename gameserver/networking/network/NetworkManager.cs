using LoESoft.Core;
using LoESoft.Core.config;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static LoESoft.GameServer.networking.Client;

namespace LoESoft.GameServer.networking.network
{
    public class NetworkManager
    {
        private readonly int _prefixLength;
        private readonly int _bufferSize;
        private readonly Client _client;
        private readonly SocketAsyncEventArgs _outgoing;
        private readonly SocketAsyncEventArgs _incoming;
        private ConcurrentQueue<Message>[] _pendings;

        public NetworkManager(
            Client client,
            SocketAsyncEventArgs outgoing,
            SocketAsyncEventArgs incoming
            )
        {
            _prefixLength = IncomingToken.PrefixLength;
            _bufferSize = Server.BufferSize;
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

            StartReceive(_incoming);
            StartSend(_outgoing);
        }

        private void ProcessIncomingMessage(object sender, SocketAsyncEventArgs e)
        {
            var r = (IncomingToken)e.UserToken;

            if (_client.State == ProtocolState.Disconnected)
            {
                _client.Manager.TryDisconnect(_client, DisconnectReason.SOCKET_IS_NOT_CONNECTED);
                r.Reset();
                return;
            }

            if (e.SocketError != SocketError.Success)
            {
                DisconnectReason dr = DisconnectReason.SOCKET_ERROR;

                if (e.SocketError != SocketError.ConnectionReset)
                    dr = DisconnectReason.CONNECTION_RESETED;

                _client.Manager.TryDisconnect(_client, dr);
                return;
            }

            var bytesNotRead = e.BytesTransferred;

            if (bytesNotRead == 0)
            {
                _client.Manager.TryDisconnect(_client, DisconnectReason.BYTES_NOT_READY);
                return;
            }

            while (bytesNotRead > 0)
            {
                bytesNotRead = ReadMessageBytes(e, r, bytesNotRead);

                if (r.BytesRead == _prefixLength)
                {
                    r.MessageLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(r.MessageBytes, 0));

                    if (e.Buffer[0] == 0x3c
                            && e.Buffer[1] == 0x70
                            && e.Buffer[2] == 0x6f
                            && e.Buffer[3] == 0x6c
                            && e.Buffer[4] == 0x69)
                    {
                        NWriter wtr = new NWriter(new NetworkStream(_client.Socket));
                        wtr.WriteNullTerminatedString(Settings.IS_PRODUCTION ? Settings.NETWORKING.INTERNAL.SELECTED_DOMAINS : Settings.NETWORKING.INTERNAL.LOCALHOST_DOMAINS);

                        wtr.Write((byte)'\r');
                        wtr.Write((byte)'\n');

                        _client.Manager.TryDisconnect(_client, DisconnectReason.PROCESS_POLICY_FILE);

                        r.Reset();
                        break;
                    }

                    if (e.Buffer[0] == 0xae
                        && e.Buffer[1] == 0x7a
                        && e.Buffer[2] == 0xf2
                        && e.Buffer[3] == 0xb2
                        && e.Buffer[4] == 0x95)
                    {
                        _client.Socket.Send(Encoding.ASCII.GetBytes($"{_client.Manager.MaxClients}:{GameServer.GameUsage}"));
                        break;
                    }

                    // discard invalid packets
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
                        _client.Manager.Network.AddPendingMessage(_client, r.GetMessageID(), r.GetMessageBody());

                    r.Reset();
                }
            }

            StartReceive(e);
        }

        private static int ReadMessageBytes(SocketAsyncEventArgs e, IncomingToken r, int bytesNotRead)
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
            if (_client.State == ProtocolState.Disconnected)
            {
                _client.Manager.TryDisconnect(_client, DisconnectReason.SOCKET_IS_NOT_CONNECTED);
                return;
            }

            e.SetBuffer(e.Offset, _bufferSize);

            bool willRaiseEvent;
            try
            {
                willRaiseEvent = e.AcceptSocket.ReceiveAsync(e);

            }
            catch (Exception ex)
            {
                GameServer.log.Error(ex);
                _client.Manager.TryDisconnect(_client, DisconnectReason.UNKNOW_ERROR_INSTANCE);
                return;
            }

            if (!willRaiseEvent)
                ProcessIncomingMessage(null, e);
        }

        private void ProcessOutgoingMessage(object sender, SocketAsyncEventArgs e)
        {
            var s = (OutgoingToken)e.UserToken;

            if (_client.State == ProtocolState.Disconnected)
            {
                _client.Manager.TryDisconnect(_client, DisconnectReason.SOCKET_IS_NOT_CONNECTED);
                s.Reset();
                return;
            }

            if (e.SocketError != SocketError.Success)
            {
                _client.Manager.TryDisconnect(_client, DisconnectReason.SOCKET_ERROR);
                return;
            }

            s.BytesSent += e.BytesTransferred;
            s.BytesAvailable -= s.BytesSent;

            var delay = 0;
            if (s.BytesAvailable <= 0)
                delay = _client.Manager.Logic.MsPT;

            StartSend(e, delay);
        }

        private async void StartSend(SocketAsyncEventArgs e, int msDelay = 0)
        {
            if (_client.State == ProtocolState.Disconnected)
                return;

            var s = (OutgoingToken)e.UserToken;

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
                willRaiseEvent = e.AcceptSocket.SendAsync(e);
            }
            catch (Exception ex)
            {
                GameServer.log.Error(ex);
                _client.Manager.TryDisconnect(_client, DisconnectReason.UNKNOW_ERROR_INSTANCE);
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

        private bool FlushPending(OutgoingToken s)
        {
            for (var i = 0; i < 3; i++)
                while (_pendings[i].TryDequeue(out Message message))
                {
                    var bytesWritten = message.Write(_client, s.Data, s.BytesAvailable);

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
            ((OutgoingToken)_outgoing.UserToken).Reset();
            ((IncomingToken)_incoming.UserToken).Reset();

            _pendings = new ConcurrentQueue<Message>[3];

            for (var i = 0; i < 3; i++)
                _pendings[i] = new ConcurrentQueue<Message>();
        }
    }
}