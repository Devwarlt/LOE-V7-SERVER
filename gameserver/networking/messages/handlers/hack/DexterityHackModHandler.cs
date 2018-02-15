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
        protected bool isAbility { get; set; }
        protected double attackPeriod { get; set; }
        protected int attackAmount { get; set; }

        public DexterityHackModHandler() { }

        public void SetPlayer(Player player) => this.player = player;

        public void SetItem(Item item) => this.item = item;

        public void SetAbility(bool isAbility) => this.isAbility = isAbility;

        public void SetPeriod(float attackPeriod) => this.attackPeriod = (1 / Math.Round(attackPeriod, 4));

        public void SetAmount(int attackAmount) => this.attackAmount = attackAmount;

        private bool byPass
        { get { return player.AccountType == (int)accountType.LOESOFT_ACCOUNT; } }

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
