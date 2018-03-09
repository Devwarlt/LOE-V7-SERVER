using LoESoft.Core.models;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static LoESoft.GameServer.networking.Client;

namespace LoESoft.GameServer.networking
{
    internal partial class NetworkHandler
    {
        private void ProcessIncomingMessage(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                if (!skt.Connected)
                {
                    Manager.TryDisconnect(client, DisconnectReason.SOCKET_IS_NOT_CONNECTED);
                    return;
                }

                if (e.SocketError != SocketError.Success)
                {
                    DisconnectReason dr = DisconnectReason.SOCKET_ERROR;

                    if (e.SocketError != SocketError.ConnectionReset)
                        dr = DisconnectReason.CONNECTION_RESET;

                    Manager.TryDisconnect(client, dr);
                    return;
                }

                var token = (IncomingToken)e.UserToken;
                token.BytesRead = e.BytesTransferred;
                token.MessageBytes = e.Buffer;
                token.MessageLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(token.MessageBytes, 0)) - 5;

                Log.Warn("New incoming token received!");
                Log.Info($"Bytes read: {token.BytesRead}.");
                Log.Info($"Message length: {token.MessageLength}");

                if (token.BytesRead == MESSAGE_SIZE)
                {
                    if (token.MessageBytes[0] == 0xae
                        && token.MessageBytes[1] == 0x7a
                        && token.MessageBytes[2] == 0xf2
                        && token.MessageBytes[3] == 0xb2
                        && token.MessageBytes[4] == 0x95)
                    {
                        byte[] c = Encoding.ASCII.GetBytes($"{Manager.MaxClients}:{GameServer.GameUsage}");
                        skt.Send(c);

                        token.Reset();
                        return;
                    }

                    if (token.MessageBytes[0] == 0x3c
                        && token.MessageBytes[1] == 0x70
                        && token.MessageBytes[2] == 0x6f
                        && token.MessageBytes[3] == 0x6c
                        && token.MessageBytes[4] == 0x69)
                    {
                        ProcessPolicyFile();
                        return;
                    }
                }

                if (token.MessageLength < MESSAGE_SIZE || token.MessageLength > BUFFER_SIZE)
                {
                    token.Reset();
                    return;
                }

                token.Message = Message.Messages[token.GetMessageID()].CreateInstance();
                token.Message.Read(client, token.MessageBytes, 0, token.MessageLength);

                Log.Info($"Receiving new message: '{token.Message}'.");

                if (client.IsReady())
                    GameServer.Manager.Network.AddPendingPacket(client, token.Message);
                else
                    GameServer.Manager.TryDisconnect(client, DisconnectReason.CONNECTION_LOST);

                token.Reset();

                e.SetBuffer(0, token.MessageLength);
                skt.ReceiveAsync(e);
            }
            catch (Exception)
            { OnError(); }
        }
    }
}