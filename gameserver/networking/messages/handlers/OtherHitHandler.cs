#region

using gameserver.networking.incoming;

#endregion

namespace gameserver.networking.handlers
{
    internal class OtherHitHandler : MessageHandlers<OTHERHIT>
    {
        public override MessageID ID => MessageID.OTHERHIT;

        protected override void HandleMessage(Client client, OTHERHIT message) => NotImplementedMessageHandler();
    }
}