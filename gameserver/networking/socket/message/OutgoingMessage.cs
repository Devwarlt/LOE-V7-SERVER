using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static gameserver.networking.Client;

namespace gameserver.networking
{
    internal partial class NetworkHandler
    {
        private void OutgoingCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                if (!skt.Connected)
                {
                    Manager.TryDisconnect(client, DisconnectReason.SOCKET_IS_NOT_CONNECTED);
                    return;
                }

                if (e.SocketError != SocketError.Success)
                    throw new SocketException((int)e.SocketError);

                switch (_outgoingState)
                {
                    case OutgoingState.ReceivingHdr:
                        // maybe ignore this bellow?
                        if (e.BytesTransferred < 5)
                        {
                            // Log.Write($"Bytes transferred: {e.BytesTransferred}.");
                            Manager.TryDisconnect(client, DisconnectReason.RECEIVING_HDR);
                            return;
                        }

                        if (e.Buffer[0] == 0xae && e.Buffer[1] == 0x7a && e.Buffer[2] == 0xf2 && e.Buffer[3] == 0xb2 && e.Buffer[4] == 0x95)
                        {
                            byte[] c = Encoding.ASCII.GetBytes($"{Manager.MaxClients}:{Program.GameUsage}");
                            skt.Send(c);
                            return;
                        }

                        if (e.Buffer[0] == 0x3c && e.Buffer[1] == 0x70 &&
                            e.Buffer[2] == 0x6f && e.Buffer[3] == 0x6c && e.Buffer[4] == 0x69)
                        {
                            ProcessPolicyFile();
                            return;
                        }

                        int len = (e.UserToken as IncomingToken).Length =
                            IPAddress.NetworkToHostOrder(BitConverter.ToInt32(e.Buffer, 0)) - 5;
                        if (len < 0 || len > BUFFER_SIZE)
                            throw new InternalBufferOverflowException();
                        try
                        {
                            Message message = Message.Messages[(MessageID)e.Buffer[4]].CreateInstance();
                            (e.UserToken as IncomingToken).Packet = message;
                        }
                        catch
                        {
                            log.ErrorFormat("Packet ID not found: {0}", e.Buffer[4]);
                        }

                        _outgoingState = OutgoingState.ReceivingBody;

                        e.SetBuffer(0, len);
                        skt.ReceiveAsync(e);
                        break;
                    case OutgoingState.ReceivingBody:
                        if (e.BytesTransferred < (e.UserToken as IncomingToken).Length)
                        {
                            Manager.TryDisconnect(client, DisconnectReason.RECEIVING_BODY);
                            return;
                        }

                        Message pkt = (e.UserToken as IncomingToken).Packet;
                        pkt.Read(client, e.Buffer, 0, (e.UserToken as IncomingToken).Length);

                        _outgoingState = OutgoingState.Processing;
                        bool cont = IncomingMessageReceived(pkt);

                        if (cont && skt.Connected)
                        {
                            _outgoingState = OutgoingState.ReceivingHdr;
                            e.SetBuffer(0, 5);
                            skt.ReceiveAsync(e);
                        }
                        break;
                    default:
                        throw new InvalidOperationException(e.LastOperation.ToString());
                }
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }
    }
}