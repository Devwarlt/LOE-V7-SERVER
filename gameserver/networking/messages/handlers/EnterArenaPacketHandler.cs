#region

using gameserver.networking.incoming;
using gameserver.networking.outgoing;
using gameserver.realm;
using gameserver.realm.world;
using common.config;

#endregion

namespace gameserver.networking.handlers
{
    internal class EnterArenaPacketHandler : MessageHandlers<ENTER_ARENA>
    {
        public override MessageID ID => MessageID.ENTER_ARENA;

        protected override void HandleMessage(Client client, ENTER_ARENA message) => Handle(client, message);

        private void Handle(Client client, ENTER_ARENA message)
        {
            if (message.Currency == 1)
                if (client.Account.Fame >= 500)
                {
                    client.Manager.Database.UpdateFame(client.Account, -500);
                }
            else
                if (client.Account.Credits >= 50)
                {
                    client.Manager.Database.UpdateCredit(client.Account, -50);
                }
            client.Player.UpdateCount++;
            client.Player.SaveToCharacter();
            World world = client.Player.Manager.AddWorld(new Arena());
            client.Reconnect(new RECONNECT
            {
                Host = "",
                Port = Settings.GAMESERVER.PORT,
                GameId = world.Id,
                Name = world.Name,
                Key = Empty<byte>.Array,
            });
        }
    }
}