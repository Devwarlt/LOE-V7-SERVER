#region

using gameserver.networking.incoming;

#endregion

namespace gameserver.networking.handlers
{
    internal class PetYardCommandHandler : MessageHandlers<PETUPGRADEREQUEST>
    {
        public override MessageID ID => MessageID.PETUPGRADEREQUEST;

        protected override void HandleMessage(Client client, PETUPGRADEREQUEST message) => NotImplementedMessageHandler();
    }
}