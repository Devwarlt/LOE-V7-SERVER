#region

using LoESoft.GameServer.networking.incoming;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class EnterArenaPacketHandler : MessageHandlers<ENTER_ARENA>
    {
        public override MessageID ID => MessageID.ENTER_ARENA;

        protected override void HandleMessage(Client client, ENTER_ARENA message)
            => NotImplementedMessageHandler();
    }
}