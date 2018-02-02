#region

using gameserver.networking.incoming;
using gameserver.realm;
using gameserver.realm.entity;
using gameserver.realm.entity.player;

#endregion

namespace gameserver.networking.handlers
{
    internal class BuyHandler : MessageHandlers<BUY>
    {
        public override MessageID ID => MessageID.BUY;

        protected override void HandleMessage(Client client, BUY message) => client.Manager.Logic.AddPendingAction(t => Handle(client.Player, message.ObjectId, message.Quantity), PendingPriority.Networking);

        private void Handle(Player player, int objectId, int quantity)
        {
            if (player.Owner == null)
                return;
            SellableObject obj = player.Owner.GetEntity(objectId) as SellableObject;
            if (obj != null)
                for (int i = 0; i < quantity; i++)
                    obj.Buy(player);
        }
    }
}