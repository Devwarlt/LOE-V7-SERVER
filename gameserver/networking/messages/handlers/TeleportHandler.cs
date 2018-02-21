#region

using LoESoft.GameServer.networking.incoming;
using LoESoft.GameServer.realm;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class TeleportHandler : MessageHandlers<TELEPORT>
    {
        public override MessageID ID => MessageID.TELEPORT;

        protected override void HandleMessage(Client client, TELEPORT message)
        {
            if (client.Player.Owner == null)
                return;

            Manager.Logic.AddPendingAction(t => client.Player.Teleport(t, message), PendingPriority.Networking);
        }
    }
}