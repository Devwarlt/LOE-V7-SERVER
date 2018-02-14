#region

using gameserver.networking.incoming;
using gameserver.networking.outgoing;
using gameserver.realm;
using gameserver.realm.entity;
using gameserver.realm.entity.player;

#endregion

namespace gameserver.networking.handlers
{
    internal class PlayerShootPacketHandler : MessageHandlers<PLAYERSHOOT>
    {
        public override MessageID ID => MessageID.PLAYERSHOOT;

        protected override void HandleMessage(Client client, PLAYERSHOOT message) => Handle(client.Player, message);

        private void Handle(Player player, PLAYERSHOOT message)
        {
            Item item;
            if (!player.Manager.GameData.Items.TryGetValue((ushort)message.ContainerType, out item))
                return;

            if (item == player.Inventory[1] || item == player.Inventory[2] || item == player.Inventory[3])
                return;
            
            Projectile prj = player.PlayerShootProjectile(
                message.BulletId, item.Projectiles[0], item.ObjectType,
                message.Time, message.Position, message.Angle);

            player.Owner.EnterWorld(prj);

            player.BroadcastSync(new ALLYSHOOT()
            {
                OwnerId = player.Id,
                Angle = message.Angle,
                ContainerType = message.ContainerType,
                BulletId = message.BulletId
            }, p => p != player && p.Dist(player) <= 12);

            player.FameCounter.Shoot(prj);
        }
    }
}