#region

using LoESoft.GameServer.networking.incoming;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class LeaveArenaHandler : MessageHandlers<ACCEPT_ARENA_DEATH>
    {
        public override MessageID ID => MessageID.ACCEPT_ARENA_DEATH;

        protected override void HandleMessage(Client client, ACCEPT_ARENA_DEATH message)
            => NotImplementedMessageHandler();
    }
}