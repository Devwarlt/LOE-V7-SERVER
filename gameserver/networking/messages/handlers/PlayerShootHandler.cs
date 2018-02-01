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

            if (item == player.Inventory[1])
                return;

            var prjDesc = item.Projectiles[0];
            Projectile prj = player.PlayerShootProjectile(
                message.BulletId, prjDesc, item.ObjectType,
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