#region

using LoESoft.GameServer.networking.incoming;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class EscapeHandler : MessageHandlers<ESCAPE>
    {
        public override MessageID ID => MessageID.ESCAPE;

        protected override void HandleMessage(Client client, ESCAPE message)
            => NotImplementedMessageHandler();
    }
}