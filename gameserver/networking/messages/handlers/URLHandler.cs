#region

using LoESoft.Core.models;
using LoESoft.GameServer.networking.incoming;
using LoESoft.GameServer.realm.entity.player;
using System.Collections.Generic;
using System.Threading;
using static LoESoft.GameServer.networking.Client;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class URLHandler : MessageHandlers<URL>
    {
        public override MessageID ID => MessageID.URL;

        private readonly List<string> ALLOWED_DOMAINS = new List<string>()
        {
            "dazedlynx.github.io"
        };

        protected override void HandleMessage(Client client, URL message)
        {
            if (!ALLOWED_DOMAINS.Contains(message.Domain))
            {
                do Thread.Sleep(250);
                while (client.Player == null);

                client.Player.ApplyConditionEffect(ConditionEffectIndex.Paused, -1);

                client.Player.SendInfo("Make sure to play with properly available URL to avoid this issue.");

                Thread.Sleep(3 * 1000);

                if (client != null)
                    Manager.TryDisconnect(client, DisconnectReason.ACCESS_DENIED);
            }
        }
    }
}
