#region

using LoESoft.GameServer.networking.incoming;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class JoinGuildPacketHandler : MessageHandlers<JOINGUILD>
    {
        public override MessageID ID => MessageID.JOINGUILD;

        protected override void HandleMessage(Client client, JOINGUILD message) => NotImplementedMessageHandler();
    }
}