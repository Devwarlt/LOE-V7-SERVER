#region

using LoESoft.GameServer.networking.incoming;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class PetYardCommandHandler : MessageHandlers<PETUPGRADEREQUEST>
    {
        public override MessageID ID => MessageID.PETUPGRADEREQUEST;

        protected override void HandleMessage(Client client, PETUPGRADEREQUEST message) => NotImplementedMessageHandler();
    }
}