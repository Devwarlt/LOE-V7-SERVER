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
        private void ProcessIncomingMessage(object sender, SocketAsyncEventArgs e) =>
            RIMM(e);

        #region "Regular Incoming Message Manager"
        private void RIMM(SocketAsyncEventArgs e)
        {
            try
            {
                if (!skt.Connected)
                {
                    GameServer.Manager.TryDisconnect(client, DisconnectReason.CONNECTION_LOST);
                    return;
                }

                if (e.SocketError != SocketError.Success)
                {
                    GameServer.Manager.TryDisconnect(client, DisconnectReason.SOCKET_ERROR);
                    return;
                }

                if (_incomingState == IncomingStage.ReceivingMessage)
                    RPRM(e);
                else if (_incomingState == IncomingStage.ReceivingData)
                    RPRD(e);
                else
                {
                    GameServer.Manager.TryDisconnect(client, DisconnectReason.CONNECTION_RESET);
                    return;
                }
            }
            catch (ObjectDisposedException)
            { return; }
        }

        private void RPRM(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred < 5)
            {
                Manager.TryDisconnect(client, DisconnectReason.RECEIVING_MESSAGE);
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

            try
            { (e.UserToken as IncomingToken).Message = Message.Messages[(MessageID)e.Buffer[4]].CreateInstance(); }
            catch
            { log.ErrorFormat("Message ID not found: {0}", e.Buffer[4]); }

            _incomingState = IncomingStage.ReceivingData;

            e.SetBuffer(0, len);

            skt.ReceiveAsync(e);
        }

        private void RPRD(SocketAsyncEventArgs e)
        {
            // bytes are not ready yet, then keep them in a loop until dispatch properly
            if (e.BytesTransferred < (e.UserToken as IncomingToken).Length)
                return;

            Message dummy = (e.UserToken as IncomingToken).Message;

            bool cont = false;

            try
            {
                dummy.Read(client, e.Buffer, 0, (e.UserToken as IncomingToken).Length);

                cont = IncomingMessageReceived(dummy);
            }
            catch (EndOfStreamException)
            { cont = false; }
            finally
            { _incomingState = IncomingStage.ProcessingMessage; }

            if (cont && skt.Connected)
            {
                _incomingState = IncomingStage.ReceivingMessage;

                e.SetBuffer(0, 5);
                skt.ReceiveAsync(e);
            }
        }
        #endregion
    }
}