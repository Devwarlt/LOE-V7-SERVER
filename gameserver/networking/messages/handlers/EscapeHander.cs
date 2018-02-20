﻿#region

using common.config;
using gameserver.networking.incoming;
using gameserver.networking.outgoing;
using gameserver.realm;

#endregion

namespace gameserver.networking.handlers
{
    internal class EscapeHandler : MessageHandlers<ESCAPE>
    {
        public override MessageID ID => MessageID.ESCAPE;

        protected override void HandleMessage(Client client, ESCAPE message) => Handle(client, message);

        private void Handle(Client client, ESCAPE message)
        {
            if (client.Player.Owner == null)
                return;

            var world = Manager.GetWorld(client.Player.Owner.Id);

            if (world.Id == (int)WorldID.NEXUS_ID)
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
                GameId = (int)WorldID.NEXUS_ID,
                Name = "Nexus",
                Key = Empty<byte>.Array,
            });
        }
    }
}