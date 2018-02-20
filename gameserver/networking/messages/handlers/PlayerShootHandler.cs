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

            if (!Program.Manager.GameData.Items.TryGetValue((ushort)message.ContainerType, out Item item))
                return;

            DexterityCheatHandler _cheatHandler = new DexterityCheatHandler();
            _cheatHandler.SetPlayer(player);
            _cheatHandler.SetItem(item);
            _cheatHandler.SetAbility(TierLoot.AbilitySlotType.ToList().Contains(item.SlotType));
            _cheatHandler.SetPeriod(message.AttackPeriod);
            _cheatHandler.SetAmount(message.AttackAmount);
            _cheatHandler.Handler();

            Projectile _projectile = player.PlayerShootProjectile(message.BulletId, item.Projectiles[0], item.ObjectType, Manager.Logic.CurrentTime.TotalElapsedMs, message.Position, message.Angle);

            player.Owner.EnterWorld(_projectile);

            ALLYSHOOT _allyShoot = new ALLYSHOOT();
            _allyShoot.Angle = message.Angle;
            _allyShoot.BulletId = message.BulletId;
            _allyShoot.ContainerType = message.ContainerType;
            _allyShoot.OwnerId = player.Id;

            player.BroadcastSync(_allyShoot, p => p != player && p.Dist(player) <= 12);

            player.FameCounter.Shoot(_projectile);
        }
    }
}