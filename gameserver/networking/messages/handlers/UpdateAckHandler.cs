#region

using gameserver.networking.incoming;

#endregion

namespace gameserver.networking.handlers
{
    internal class UpdateAckHandler : MessageHandlers<UPDATEACK>
    {
        public override MessageID ID => MessageID.UPDATEACK;

        protected override void HandleMessage(Client client, UPDATEACK message) => client.Player.UpdatesReceived++;
    }
}