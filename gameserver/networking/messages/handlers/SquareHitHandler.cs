#region

using gameserver.networking.incoming;

#endregion

namespace gameserver.networking.handlers
{
    internal class SquareHitHandler : MessageHandlers<SQUAREHIT>
    {
        public override MessageID ID => MessageID.SQUAREHIT;

        protected override void HandleMessage(Client client, SQUAREHIT packet) => NotImplementedMessageHandler();
    }
}