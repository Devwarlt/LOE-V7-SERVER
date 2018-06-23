#region

using LoESoft.GameServer.networking.incoming;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class PongHandler : MessageHandlers<PONG>
    {
        public override MessageID ID => MessageID.PONG;

        protected override void HandleMessage(Client client, PONG message)
            => Manager.Logic.AddPendingAction(t => client.Player.Pong(t, message));
    }
}