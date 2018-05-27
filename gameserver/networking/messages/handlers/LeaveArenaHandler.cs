#region

using LoESoft.GameServer.networking.incoming;
using LoESoft.GameServer.networking.outgoing;
using LoESoft.GameServer.realm;
using LoESoft.Core.config;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class LeaveArenaHandler : MessageHandlers<ACCEPT_ARENA_DEATH>
    {
        public override MessageID ID => MessageID.ACCEPT_ARENA_DEATH;

        protected override void HandleMessage(Client client, ACCEPT_ARENA_DEATH message)
        {
            if (client.Player.Owner == null) return;
            World world = Manager.GetWorld(client.Player.Owner.Id);
            if (world.Id == (int)WorldID.ISLE_OF_APPRENTICES)
            {
                client.SendMessage(new TEXT
                {
                    Stars = -1,
                    BubbleTime = 0,
                    Name = "",
                    Text = "server.already_nexus",
                    NameColor = 0x123456,
                    TextColor = 0x123456
                });
                return;
            }
            client.Reconnect(new RECONNECT
            {
                Host = "",
                Port = Settings.GAMESERVER.PORT,
                GameId = (int)WorldID.ISLE_OF_APPRENTICES,
                Name = "nexus.Nexus",
                Key = Empty<byte>.Array,
            });
        }
    }
}