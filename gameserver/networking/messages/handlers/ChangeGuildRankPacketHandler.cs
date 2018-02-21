#region

using LoESoft.GameServer.networking.incoming;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class ChangeGuildRankPacketHandler : MessageHandlers<CHANGEGUILDRANK>
    {
        public override MessageID ID => MessageID.CHANGEGUILDRANK;

        protected override void HandleMessage(Client client, CHANGEGUILDRANK message) => NotImplementedMessageHandler();
    }
}