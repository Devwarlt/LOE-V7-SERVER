#region

using LoESoft.Core.config;
using LoESoft.GameServer.realm.entity.player;
using System;
using log4net;

#endregion

namespace LoESoft.GameServer.networking.messages.handlers.hack
{
    public class DexterityCheatHandler : ICheatHandler
    {
        private bool SAVE_LOG = true;
        private ILog log = LogManager.GetLogger(typeof(DexterityCheatHandler));
        private Player Player { get; set; }
        private Item Item { get; set; }
        private bool IsAbility { get; set; }
        private double AttackPeriod { get; set; }
        private int AttackAmount { get; set; }

        public static int NUMBER_OF_ENTRIES = 0;

        public DexterityCheatHandler()
        { GlobalContext.Properties["LogPathName"] = "DexterityCheatHandler"; }

        public void SetPlayer(Player player) => Player = player;

        public void SetItem(Item item) => Item = item;

        public void SetAbility(bool isAbility) => IsAbility = isAbility;

        public void SetPeriod(float attackPeriod) => AttackPeriod = (1 / Math.Round(attackPeriod, 4));

        public void SetAmount(int attackAmount) => AttackAmount = attackAmount;

        private bool ByPass
        { get { return Player.AccountType == (int)AccountType.LOESOFT_ACCOUNT; } }

        CheatID ICheatHandler.ID
        { get { return CheatID.DEXTERITY; } }

        public void Handler()
        {
            if (Item == Player.Inventory[1] || Item == Player.Inventory[2] || Item == Player.Inventory[3])
                return;

            if (IsAbility)
                return;

            if ((AttackPeriod > ProcessAttackPeriod() || AttackAmount != Item.NumProjectiles) && !ByPass)
            {
                if (SAVE_LOG) // save all detailed data before disconnect
                {
                    log.Info($"Dexterity Cheat Handler ID '{NUMBER_OF_ENTRIES++}':" +
                        $"\n\t- Player: {Player.Name} (ID: {Player.AccountId})," +
                        $"\n\t- World: {Player.Owner.Name}," +
                        $"\n\t- Item: {Item.DisplayId} (type: 0x'{Item.ObjectType:x4}')," +
                        $"\n\t- Attack period: {AttackPeriod} (valid: {ProcessAttackPeriod()})," +
                        $"\n\t- Attack amount: {AttackAmount} (valid: {Item.NumProjectiles})." +
                        $"\n\t- Beserk? {Player.HasConditionEffect(ConditionEffectIndex.Berserk)}," +
                        $"\n\t- Dazed? {Player.HasConditionEffect(ConditionEffectIndex.Dazed)}.");
                }
                // disable for now
                //Program.Manager.TryDisconnect(Player.Client, Client.DisconnectReason.DEXTERITY_HACK_MOD);
                return;
            }
        }

        private double ProcessAttackPeriod() =>
            1 / Math.Round((1 / Player.StatsManager.GetAttackFrequency())
            * (1 / Item.RateOfFire), 4);
    }
}
