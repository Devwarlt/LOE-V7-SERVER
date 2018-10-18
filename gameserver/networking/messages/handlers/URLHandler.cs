#region

using LoESoft.GameServer.networking.incoming;
using System.Collections.Generic;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class URLHandler : MessageHandlers<URL>
    {
        public override MessageID ID => MessageID.URL;

        private readonly List<string> ALLOWED_DOMAINS = new List<string>()
        {
            "dazedlynx.github.io", "unknown domain"
        };

        protected override void HandleMessage(Client client, URL message)
        {
            if (!ALLOWED_DOMAINS.Contains(message.Domain))
                client.IsDomainReceived = false;
        }
    }
}