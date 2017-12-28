#region

using gameserver.networking.incoming;

#endregion

namespace gameserver.networking.handlers
{
    internal class ShootAckHandler : MessageHandlers<SHOOTACK>
    {
        public override MessageID ID => MessageID.SHOOTACK;

        protected override void HandleMessage(Client client, SHOOTACK packet) => NotImplementedMessageHandler();
    }
}