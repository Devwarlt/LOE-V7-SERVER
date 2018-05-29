#region

using LoESoft.Core.config;
using LoESoft.GameServer.networking.incoming;
using LoESoft.GameServer.networking.outgoing;
using LoESoft.GameServer.realm;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class EscapeHandler : MessageHandlers<ESCAPE>
    {
        public override MessageID ID => MessageID.ESCAPE;

        protected override void HandleMessage(Client client, ESCAPE message) => NotImplementedMessageHandler();
    }
}