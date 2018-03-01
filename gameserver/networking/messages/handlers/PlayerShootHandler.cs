#region

using LoESoft.GameServer.logic.loot;
using LoESoft.GameServer.networking.incoming;
using LoESoft.GameServer.networking.messages.handlers.hack;
using LoESoft.GameServer.networking.outgoing;
using LoESoft.GameServer.realm;
using LoESoft.GameServer.realm.entity;
using LoESoft.GameServer.realm.entity.player;
using System.Linq;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class PlayerShootPacketHandler : MessageHandlers<PLAYERSHOOT>
    {
        public override MessageID ID => MessageID.PLAYERSHOOT;

        protected override void HandleMessage(Client client, PLAYERSHOOT message) => Handle(client.Player, message);

        private void Handle(Player player, PLAYERSHOOT message)
        {

            if (!GameServer.Manager.GameData.Items.TryGetValue((ushort)message.ContainerType, out Item item))
                return;

            DexterityCheatHandler _cheatHandler = new DexterityCheatHandler()
            {
                Player = player,
                Item = item,
                IsAbility = TierLoot.AbilitySlotType.ToList().Contains(item.SlotType),
                AttackAmount = message.AttackAmount,
                MinAttackFrequency = message.MinAttackFrequency,
                MaxAttackFrequency = message.MaxAttackFrequency,
                WeaponRateOfFire = message.WeaponRateOfFire
            };

            _cheatHandler.Handler();

            Projectile _projectile = player.PlayerShootProjectile(message.BulletId, item.Projectiles[0], item.ObjectType, Manager.Logic.CurrentTime.TotalElapsedMs, message.Position, message.Angle);

            player.Owner.EnterWorld(_projectile);

            ALLYSHOOT _allyShoot = new ALLYSHOOT
            {
                Angle = message.Angle,
                BulletId = message.BulletId,
                ContainerType = message.ContainerType,
                OwnerId = player.Id
            };

            player.BroadcastSync(_allyShoot, p => p != player && p.Dist(player) <= 12);

            player.FameCounter.Shoot(_projectile);
        }
    }
}