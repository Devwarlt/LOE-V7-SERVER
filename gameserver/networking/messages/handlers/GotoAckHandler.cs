#region

using LoESoft.GameServer.networking.incoming;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class GotoAckHandler : MessageHandlers<GOTOACK>
    {
        public override MessageID ID => MessageID.GOTOACK;

        protected override void HandleMessage(Client client, GOTOACK message) => NotImplementedMessageHandler();
    }
}