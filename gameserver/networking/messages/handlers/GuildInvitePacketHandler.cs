#region

using LoESoft.GameServer.networking.incoming;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class GuildInvitePacketHandler : MessageHandlers<GUILDINVITE>
    {
        public override MessageID ID => MessageID.GUILDINVITE;

        protected override void HandleMessage(Client client, GUILDINVITE message) => NotImplementedMessageHandler();
    }
}