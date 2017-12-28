#region

using gameserver.networking.incoming;

#endregion

namespace gameserver.networking.handlers
{
    internal class ChangeGuildRankPacketHandler : MessageHandlers<CHANGEGUILDRANK>
    {
        public override MessageID ID => MessageID.CHANGEGUILDRANK;

        protected override void HandleMessage(Client client, CHANGEGUILDRANK message) => NotImplementedMessageHandler();
    }
}