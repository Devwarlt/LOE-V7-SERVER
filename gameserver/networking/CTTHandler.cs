using LoESoft.GameServer.networking.incoming;
using LoESoft.GameServer.networking.outgoing;
using System;
using System.Threading;
using static LoESoft.GameServer.networking.Client;

namespace LoESoft.GameServer.networking
{
    /// <summary>
    /// CTTHandler
    /// Author: DV
    /// </summary>

    public class CTTSendHandler
    {
        public Client Client { get; set; }
        private int Counter { get; set; }

        public void SendRequest()
        {
            if (Client == null)
                return;

            if (!Client.Socket.Connected)
                return;

            Client.Player.ApplyConditionEffect(ConditionEffectIndex.Paused);

            Client.Player.SendInfo("The server is validating your request, please wait.");

            Client.SendMessage(new CTT_SEND
            {
                CTTSendMessage = "{\"name\":\"Closed Test Token Request\", \"message\":\"In order to access the Closed Test server, you need to insert a valid token bellow for approvation:\"}"
            });
        }
    }

    internal class CTTReceiveHandler : MessageHandlers<CTT_RECEIVE>
    {
        public override MessageID ID => MessageID.CTT_RECEIVE;

        protected override void HandleMessage(Client client, CTT_RECEIVE message)
        {
            if (client == null)
                return;

            if (!client.Socket.Connected)
                return;

            switch (ProcessRequest(client, message.CTTAuth))
            {
                case CTTStatus.MaxNumberAttempts:
                    client.Player.SendInfo($"You reached max number of attempts ({MaxNumberAttemptsPerCTT}) to validate your token and cannot access server while in closed test.");

                    int i = 5;
                    do
                    {
                        Thread.Sleep(1 * 1000);
                        i--;
                    } while (i != 0);

                    if (client != null)
                        if (client.Socket.Connected)
                            Manager.TryDisconnect(client, DisconnectReason.SERVER_MODE_CLOSED_TEST_ONLY);
                    break;
                case CTTStatus.InvalidCTT:
                    Client.SendMessage(new CTT_SEND
                    {
                        CTTSendMessage = "{\"name\":\"Closed Test Token Request\", \"message\":\"In order to access the Closed Test server, you need to insert a valid token below for approvation:\"}"
                    });

                    client.Player.SendInfo($"Token '{message.CTTAuth}' is invalid, you have {client.CTTUses} of {MaxNumberAttemptsPerCTT} attempts until disconnect.");
                    break;
                case CTTStatus.Approved:
                default:
                    client.Player.ApplyConditionEffect(ConditionEffectIndex.Paused, 0);
                    client.Player.SendInfo($"Congratulations {client.Player.Name}, your Account ID {client.Account.AccountId} has been approved to access the closed test server, enjoy!");
                    break;
            }
        }

        private CTTStatus ProcessRequest(Client client, string token)
        {
            if (GameServer.CTTManager.CTTData.TryGetValue(token, out CTT cTT))
            {
                client.Account.ClosedTester = true;
                client.CTTUses = 0;

                client.Account.Flush();
                client.Account.Reload();

                cTT.UpdateUse();

                GameServer.CTTManager.Save();

                return CTTStatus.Approved;
            }
            else
            {
                client.CTTUses++;

                if (client.CTTUses >= MaxNumberAttemptsPerCTT)
                    return CTTStatus.MaxNumberAttempts;
                else
                    return CTTStatus.InvalidCTT;
            }
        }
    }

    [Flags]
    public enum CTTStatus
    {
        MaxNumberAttempts,
        InvalidCTT,
        Approved,
        RequestCTT
    }
}
