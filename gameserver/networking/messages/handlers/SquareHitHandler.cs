#region

using LoESoft.GameServer.networking.incoming;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class SquareHitHandler : MessageHandlers<SQUAREHIT>
    {
        public override MessageID ID => MessageID.SQUAREHIT;

        protected override void HandleMessage(Client client, SQUAREHIT packet) => NotImplementedMessageHandler();
    }
}