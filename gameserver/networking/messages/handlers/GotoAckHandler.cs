#region

using gameserver.networking.incoming;

#endregion

namespace gameserver.networking.handlers
{
    internal class GotoAckHandler : MessageHandlers<GOTOACK>
    {
        public override MessageID ID => MessageID.GOTOACK;

        protected override void HandleMessage(Client client, GOTOACK message) => NotImplementedMessageHandler();
    }
}