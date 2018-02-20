#region

using System.Linq;
using common;
using gameserver.networking.incoming;
using gameserver.networking.outgoing;
using gameserver.realm.entity.player;
using gameserver.realm;
using FAILURE = gameserver.networking.outgoing.FAILURE;
using static gameserver.networking.Client;

#endregion

namespace gameserver.networking.handlers
{
    internal class CreateHandler : MessageHandlers<CREATE>
    {
        public override MessageID ID => MessageID.CREATE;

        protected override void HandleMessage(Client client, CREATE message) => Handle(client, message);

        private void Handle(Client client, CREATE message)
        {
            int skin = client.Account.OwnedSkins.Contains(message.SkinType) ? message.SkinType : 0;
            CreateStatus status = Manager.Database.CreateCharacter(Manager.GameData, client.Account, (ushort)message.ClassType, skin, out DbChar character);
            if (status == CreateStatus.ReachCharLimit)
            {
                client.SendMessage(new FAILURE
                {
                    ErrorDescription = "Failed to Load character."
                });
                Manager.TryDisconnect(client, DisconnectReason.FAILED_TO_LOAD_CHARACTER);
                return;
            }
            client.Character = character;
            World target = Manager.Worlds[client.TargetWorld];
            target.Timers.Add(new WorldTimer(5000, (w, t) =>
            {
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
                        ErrorDescription = "Failed to Load character."
                    });
                }
            }));
        }
    }
}