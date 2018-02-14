﻿#region

using gameserver.networking.incoming;
using gameserver.networking.outgoing;
using gameserver.realm.entity.player;
using FAILURE = gameserver.networking.outgoing.FAILURE;
using gameserver.realm;
using static gameserver.networking.Client;

#endregion

namespace gameserver.networking.handlers
{
    internal class LoadHandler : MessageHandlers<LOAD>
    {
        public override MessageID ID => MessageID.LOAD;

        protected override void HandleMessage(Client client, LOAD message)
        {
            client.Character = Manager.Database.LoadCharacter(client.Account, message.CharacterId);
            if (client.Character != null)
            {
                if (client.Character.Dead)
                {
                    client.SendMessage(new FAILURE
                    {
                        ErrorId = (int)FailureIDs.DEFAULT,
                        ErrorDescription = "Character is dead."
                    });
                    Manager.TryDisconnect(client, DisconnectReason.CHARACTER_IS_DEAD);
                }
                else
                {
                    World target = Manager.Worlds[client.TargetWorld];
                    client.SendMessage(new CREATE_SUCCESS
                    {
                        CharacterID = client.Character.CharId,
                        ObjectID = Manager.Worlds[client.TargetWorld].EnterWorld(client.Player = new Player(client))
                    });
                    client.State = ProtocolState.Ready;
                }
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