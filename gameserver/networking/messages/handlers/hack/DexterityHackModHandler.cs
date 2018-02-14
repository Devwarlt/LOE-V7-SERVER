#region

using common.config;
using gameserver.realm.entity.player;
using System;

#endregion

namespace gameserver.networking.messages.handlers.hack
{
    public class DexterityHackModHandler
    {
        protected Player player { get; set; }
        protected Item item { get; set; }
        protected int dexterity { get; set; }
        protected bool isAbility { get; set; }
        protected bool byPass { get; set; }
        protected double attackPeriod { get; set; }
        protected int attackAmount { get; set; }

        public DexterityHackModHandler(
            Player _player,
            short _item,
            bool _isAbility,
            float _attackPeriod,
            int _attackAmount
            )
        {
            player = _player;
            item = Program.Manager.GameData.Items[(ushort)_item];
            dexterity = _player.StatsManager.GetStats(7);
            isAbility = _isAbility;
            attackPeriod = (1 / Math.Round(_attackPeriod, 4));
            attackAmount = _attackAmount;
            byPass = _player.AccountType == (int)accountType.LOESOFT_ACCOUNT;
        }

        public void Validate()
        {
            if (item == player.Inventory[1] || item == player.Inventory[2] || item == player.Inventory[3])
                return;
            
            if (isAbility)
                return;
            
            if ((attackPeriod > ProcessAttackPeriod() || attackAmount != item.NumProjectiles) && !byPass)
            {
                Program.Manager.TryDisconnect(player.client, Client.DisconnectReason.DEXTERITY_HACK_MOD);
                return;
            }
        }

        private double ProcessAttackPeriod() =>
            1 / Math.Round((1 / player.StatsManager.GetAttackFrequency())
            * (1 / item.RateOfFire), 4);
    }
}
