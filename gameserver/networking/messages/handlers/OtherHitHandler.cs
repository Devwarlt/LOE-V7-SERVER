#region

using LoESoft.GameServer.networking.incoming;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class OtherHitHandler : MessageHandlers<OTHERHIT>
    {
        public override MessageID ID => MessageID.OTHERHIT;

        protected override void HandleMessage(Client client, OTHERHIT message) => NotImplementedMessageHandler();
    }
}