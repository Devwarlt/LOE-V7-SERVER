#region

using LoESoft.Core;
using LoESoft.GameServer.networking.error;
using LoESoft.GameServer.networking.incoming;
using LoESoft.GameServer.networking.outgoing;
using LoESoft.GameServer.realm.entity.player;
using static LoESoft.GameServer.networking.Client;
using FAILURE = LoESoft.GameServer.networking.outgoing.FAILURE;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class CreateHandler : MessageHandlers<CREATE>
    {
        public override MessageID ID => MessageID.CREATE;

        protected override void HandleMessage(Client client, CREATE message) => Handle(client, message);

        private void Handle(Client client, CREATE message)
        {
            CreateStatus status = Manager.Database.CreateCharacter(Manager.GameData, client.Account, (ushort)message.ClassType, 0, out DbChar character);

            if (status == CreateStatus.ReachCharLimit)
            {
                client.SendMessage(new FAILURE
                {
                    ErrorId = (int)FailureIDs.JSON_DIALOG,
                    ErrorDescription =
                        JSONErrorIDHandler.
                            FormatedJSONError(
                                errorID: ErrorIDs.NORMAL_CONNECTION,
                                labels: new[] { "{UNKNOW_ERROR_INSTANCE}" },
                                arguments: new[] { "You reached <b>max</b> limit of characters per account." }
                            )
                });

                Manager.TryDisconnect(client, DisconnectReason.FAILED_TO_LOAD_CHARACTER);

                return;
            }

            client.Character = character;

            if (status == CreateStatus.OK)
            {
                client.SendMessage(new CREATE_SUCCESS
                {
                    CharacterID = client.Character.CharId,
                    ObjectID =
                        Manager.Worlds[client.TargetWorld].EnterWorld(
                            client.Player = new Player(client))
                });

                client.State = ProtocolState.Ready;
            }
            else
            {
                client.SendMessage(new FAILURE
                {
                    ErrorId = (int)FailureIDs.JSON_DIALOG,
                    ErrorDescription =
                        JSONErrorIDHandler.
                            FormatedJSONError(
                                errorID: ErrorIDs.NORMAL_CONNECTION,
                                labels: new[] { "{UNKNOW_ERROR_INSTANCE}" },
                                arguments: new[] { "You couldn't create your character, try again later." }
                            )
                });

                Manager.TryDisconnect(client, DisconnectReason.FAILED_TO_LOAD_CHARACTER);

                return;
            }
        }
    }
}