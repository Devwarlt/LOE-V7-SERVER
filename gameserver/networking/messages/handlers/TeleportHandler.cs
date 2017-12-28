#region

using gameserver.networking.incoming;
using gameserver.realm;

#endregion

namespace gameserver.networking.handlers
{
    internal class TeleportHandler : MessageHandlers<TELEPORT>
    {
        public override MessageID ID => MessageID.TELEPORT;

        protected override void HandleMessage(Client client, TELEPORT message)
        {
            if (client.Player.Owner == null)
                return;

            client.Manager.Logic.AddPendingAction(t => client.Player.Teleport(t, message), PendingPriority.Networking);
        }
    }
}