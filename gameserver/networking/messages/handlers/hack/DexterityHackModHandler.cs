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
        protected bool hasBeserk { get; set; }
        protected float minAttackFrequency { get; set; }
        protected float maxAttackFrequency { get; set; }
        protected float attackVariance { get; set; }

        public DexterityHackModHandler(
            Player _player,
            short _item,
            bool _isAbility
            )
        {
            player = _player;
            item = Program.Manager.GameData.Items[(ushort)_item];
            dexterity = _player.StatsManager.GetStats(7);
            isAbility = _isAbility;
            byPass = _player.AccountType == (int)accountType.LOESOFT_ACCOUNT;
            hasBeserk = _player.HasConditionEffect(ConditionEffectIndex.Berserk);
            minAttackFrequency = .0015f;
            maxAttackFrequency = .008f;
            attackVariance = .25f;
        }

        public void Validate()
        {
            if (item == player.Inventory[1] || item == player.Inventory[2] || item == player.Inventory[3])
                return;

            if (isAbility && !byPass)
                return;

            if (player.numProjs + 1 > ProcessRateOfFire())
                return;
        }

        private float ProcessRateOfFire() =>
            item.NumProjectiles
            * (item.RateOfFire == 0 ? 1 : item.RateOfFire)
            * ((hasBeserk ? 1.5f : 1)
            * (minAttackFrequency
            + (dexterity / 75f)
            * (maxAttackFrequency - minAttackFrequency)))
            + (float)Math.Round((item.Projectiles[0].LifetimeMS / 1000), 1)
            + attackVariance;
    }
}
