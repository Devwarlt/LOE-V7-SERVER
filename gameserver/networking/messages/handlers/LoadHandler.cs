#region

using LoESoft.GameServer.networking.incoming;
using LoESoft.GameServer.networking.outgoing;
using LoESoft.GameServer.realm.entity.player;
using FAILURE = LoESoft.GameServer.networking.outgoing.FAILURE;
using LoESoft.GameServer.realm;
using static LoESoft.GameServer.networking.Client;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class LoadHandler : MessageHandlers<LOAD>
    {
        public override MessageID ID => MessageID.LOAD;

        protected override void HandleMessage(Client client, LOAD message)
        {
            client.Character = Manager.Database.LoadCharacter(client.Account, message.CharacterId);
            if (client.Character != null)
            {
                World target = Manager.Worlds[client.TargetWorld];
                client.SendMessage(new CREATE_SUCCESS
                {
                    CharacterID = client.Character.CharId,
                    ObjectID = Manager.Worlds[client.TargetWorld].EnterWorld(client.Player = new Player(client))
                });
                client.State = ProtocolState.Ready;
            }
            else
            {
                client.SendMessage(new FAILURE
                {
                    ErrorId = (int)FailureIDs.DEFAULT,
                    ErrorDescription = "Failed to Load character."
                });
                Manager.TryDisconnect(client, DisconnectReason.FAILED_TO_LOAD_CHARACTER);
            }
        }
    }
}