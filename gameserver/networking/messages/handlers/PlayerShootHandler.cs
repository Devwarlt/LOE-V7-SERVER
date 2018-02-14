#region

using gameserver.logic.loot;
using gameserver.networking.incoming;
using gameserver.networking.messages.handlers.hack;
using gameserver.networking.outgoing;
using gameserver.realm;
using gameserver.realm.entity;
using gameserver.realm.entity.player;
using System.Linq;

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

            if (!Program.Manager.GameData.Items.TryGetValue((ushort)message.ContainerType, out item))
                return;

            bool isAbility = TierLoot.AbilitySlotType.ToList().Contains(item.SlotType);

            DexterityHackModHandler dexMod = new DexterityHackModHandler(player, message.ContainerType, isAbility);
            dexMod.Validate();

            Projectile prj = player.PlayerShootProjectile(
                message.BulletId, item.Projectiles[0], item.ObjectType,
                message.Time, message.Position, message.Angle, !isAbility);

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