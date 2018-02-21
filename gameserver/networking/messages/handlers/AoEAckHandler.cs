#region

using LoESoft.GameServer.networking.incoming;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class AOEAckHandler : MessageHandlers<AOEACK>
    {
        public override MessageID ID => MessageID.AOEACK;

        protected override void HandleMessage(Client client, AOEACK message) => NotImplementedMessageHandler();
    }
}