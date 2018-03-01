using LoESoft.Core.models;
using LoESoft.GameServer.networking.error;
using LoESoft.GameServer.networking.outgoing;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static LoESoft.GameServer.networking.Client;

namespace LoESoft.GameServer.networking
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
                {
                    DisconnectReason dr = DisconnectReason.SOCKET_ERROR;

                    if (e.SocketError != SocketError.ConnectionReset)
                        dr = DisconnectReason.CONNECTION_RESET;

                    Manager.TryDisconnect(client, dr);
                    return;
                }

                switch (_outgoingState)
                {
                    case OutgoingState.ReceivingHdr:
                        if (e.BytesTransferred < 5)
                        {
                            Manager.TryDisconnect(client, DisconnectReason.INVALID_BUFFER_LENGTH);
                            return;
                        }

                        if (e.Buffer[0] == 0xae
                            && e.Buffer[1] == 0x7a
                            && e.Buffer[2] == 0xf2
                            && e.Buffer[3] == 0xb2
                            && e.Buffer[4] == 0x95)
                        {
                            byte[] c = Encoding.ASCII.GetBytes($"{Manager.MaxClients}:{GameServer.GameUsage}");
                            skt.Send(c);
                            return;
                        }

                        if (e.Buffer[0] == 0x3c
                            && e.Buffer[1] == 0x70
                            && e.Buffer[2] == 0x6f
                            && e.Buffer[3] == 0x6c
                            && e.Buffer[4] == 0x69)
                        {
                            ProcessPolicyFile();
                            return;
                        }

                        int len = (e.UserToken as IncomingToken).Length =
                            IPAddress.NetworkToHostOrder(BitConverter.ToInt32(e.Buffer, 0)) - 5;

                        if (len < 0 || len > BUFFER_SIZE)
                            throw new InternalBufferOverflowException();

                        Message message = null;

                        try
                        {
                            message = Message.Messages[(MessageID)e.Buffer[4]].CreateInstance();
                        }
                        catch
                        {
                            log.Error($"Message not added: {e.Buffer[4]}");
                        }

                        (e.UserToken as IncomingToken).Message = message;

                        _outgoingState = OutgoingState.ReceivingBody;

                        e.SetBuffer(0, len);
                        skt.ReceiveAsync(e);
                        break;
                    case OutgoingState.ReceivingBody:
                        if (e.BytesTransferred < (e.UserToken as IncomingToken).Length)
                        {
                            Log.Error($"(Bytes [{e.Buffer.Length}]: {e.BytesTransferred}/{(e.UserToken as IncomingToken).Length}) Error in message '{(e.UserToken as IncomingToken).Message.ID}':\n{(e.UserToken as IncomingToken).Message}");
                            
                            foreach (var i in e.Buffer)
                                Log.Error($"{i}");

                            string[] labels = new string[] { "{CLIENT_NAME}" };
                            string[] arguments = new string[] { client.Account.Name };

                            client.SendMessage(new FAILURE
                            {
                                ErrorId = (int)FailureIDs.JSON_DIALOG,
                                ErrorDescription =
                                    JSONErrorIDHandler.
                                        FormatedJSONError(
                                            errorID: ErrorIDs.LOST_CONNECTION,
                                            labels: labels,
                                            arguments: arguments
                                        )
                            });

                            Manager.TryDisconnect(client, DisconnectReason.RECEIVING_BODY);

                            return;
                        }

                        Message newMessage = (e.UserToken as IncomingToken).Message;
                        newMessage.Read(client, e.Buffer, 0, (e.UserToken as IncomingToken).Length);

                        _outgoingState = OutgoingState.Processing;

                        bool cont = IncomingMessageReceived(newMessage);

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
            catch (Exception)
            {
                OnError();
            }
        }
    }
}

/*using LoESoft.Core;
using LoESoft.Core.models;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static LoESoft.GameServer.networking.Client;

namespace LoESoft.GameServer.networking
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
                //throw new SocketException((int)e.SocketError);
                {
                    DisconnectReason dr = DisconnectReason.SOCKET_ERROR;

                    if (e.SocketError != SocketError.ConnectionReset)
                        dr = DisconnectReason.CONNECTION_RESET;

                    Manager.TryDisconnect(client, dr);
                    return;
                }

                switch (_outgoingState)
                {
                    case OutgoingState.ReceivingBody:
                        if (e.BytesTransferred < (e.UserToken as IncomingToken).Length)
                        {
                            Log.Error($"(Bytes: {e.BytesTransferred}/{(e.UserToken as IncomingToken).Length}) Error in message '{(e.UserToken as IncomingToken).Message.ID}':\n{(e.UserToken as IncomingToken).Message}");

                            Manager.TryDisconnect(client, DisconnectReason.RECEIVING_BODY);
                            return;
                        }

                        Message message = (e.UserToken as IncomingToken).Message;
                        message.Read(client, e.Buffer, 0, (e.UserToken as IncomingToken).Length);

                        _outgoingState = OutgoingState.Processing;

                        bool cont = IncomingMessageReceived(message);

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
}*/
