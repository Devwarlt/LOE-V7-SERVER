#region

using LoESoft.GameServer.networking.incoming;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class ShootAckHandler : MessageHandlers<SHOOTACK>
    {
        public override MessageID ID => MessageID.SHOOTACK;

        protected override void HandleMessage(Client client, SHOOTACK packet)
            => NotImplementedMessageHandler();
    }
}