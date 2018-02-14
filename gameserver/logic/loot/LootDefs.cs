#region

using System;
using System.Collections.Generic;
using System.Linq;
using common;
using gameserver.realm.entity;
using gameserver.realm.entity.player;

#endregion

namespace gameserver.logic.loot
{
    public interface ILootDef
    {
        string Lootstate { get; set; }

        void Populate(
            Enemy enemy,
            Tuple<Player, int> playerDat,
            Random rand,
            string lootState,
            IList<LootDef> lootDefs
            );
    }

    public class ProcessWhiteBag : ILootDef
    {
        private readonly bool eventChest;
        private readonly ILootDef[] loot;

        public string Lootstate { get; set; }

        public ProcessWhiteBag(bool eventChest = false, params ILootDef[] loot)
        {
            this.eventChest = eventChest;
            this.loot = loot;
        }

        public void Populate(
            Enemy enemy,
            Tuple<Player, int> playerData,
            Random rnd,
            string lootState,
            IList<LootDef> lootDefs
            )
        {
            Lootstate = lootState;

            if (playerData == null)
                return;

            Tuple<Player, int>[] enemyData = enemy.DamageCounter.GetPlayerData();

            int damageData = GetDamageData(enemyData);
            double enemyHP = enemy.ObjectDesc.MaxHP;

            if (damageData >= enemyHP * .2)
            {
                double chance = eventChest ? .01 : .05;
                double rng = rnd.NextDouble();

                if (rng <= chance)
                    foreach (ILootDef i in loot)
                        i.Populate(enemy, playerData, rnd, Lootstate, lootDefs);
            }
        }

        private int GetDamageData(IEnumerable<Tuple<Player, int>> data)
        {
            List<int> damages = data.Select(_ => _.Item2).ToList();
            int totalDamage = 0;
            for (int i = 0; i < damages.Count; i++)
                totalDamage += damages[i];
            return totalDamage;
        }
    }

    public class WhiteBag : ILootDef
    {
        private readonly bool eventChest;
        private readonly ILootDef[] loot;

        public string Lootstate { get; set; }

        public WhiteBag(string itemName, bool eventChest = false)
        {
            this.eventChest = eventChest;
            loot = new ILootDef[] { new MostDamagers(5, new ItemLoot(itemName, eventChest ? .01 : .05)) };
        }

        public WhiteBag(string[] itemNames, bool eventChest = false)
        {
            this.eventChest = eventChest;
            loot = new ILootDef[] { new MostDamagers(5, new ItemLoot(ProcessOnlyOne(itemNames), eventChest ? .01 : .05)) };
        }

        private string ProcessOnlyOne(string[] itemNames)
        {
            List<string> items = itemNames.ToList();
            return items[new Random().Next(0, items.Count)];
        }

        public void Populate(
            Enemy enemy,
            Tuple<Player, int> playerData,
            Random rnd,
            string lootState,
            IList<LootDef> lootDefs
            )
        {
            Lootstate = lootState;

            if (playerData == null)
                return;

            Tuple<Player, int>[] enemyData = enemy.DamageCounter.GetPlayerData();

            int damageData = GetDamageData(enemyData);
            double enemyHP = enemy.ObjectDesc.MaxHP;

            if (damageData >= enemyHP * .2)
            {
                double chance = eventChest ? .01 : .05;
                double rng = rnd.NextDouble();

                if (rng <= chance)
                    foreach (ILootDef i in loot)
                        i.Populate(enemy, playerData, rnd, Lootstate, lootDefs);
            }
        }

        private int GetDamageData(IEnumerable<Tuple<Player, int>> data)
        {
            List<int> damages = data.Select(_ => _.Item2).ToList();
            int totalDamage = 0;
            for (int i = 0; i < damages.Count; i++)
                totalDamage += damages[i];
            return totalDamage;
        }
    }

    public class ItemLoot : ILootDef
    {
        private readonly string item;
        private readonly double probability;

        public string Lootstate { get; set; }

        public ItemLoot(string item, double probability)
        {
            this.item = item;
            this.probability = probability;
        }

        public void Populate(
            Enemy enemy,
            Tuple<Player, int> playerDat,
            Random rand,
            string lootState,
            IList<LootDef> lootDefs
            )
        {
            Lootstate = lootState;
            if (playerDat != null) return;
            EmbeddedData dat = Program.Manager.GameData;
            lootDefs.Add(new LootDef(dat.Items[dat.IdToObjectType[item]], probability, lootState));
        }
    }

    public class LootState : ILootDef
    {
        private readonly ILootDef[] children;

        public string Lootstate { get; set; }

        public LootState(string subState, params ILootDef[] lootDefs)
        {
            children = lootDefs;
            Lootstate = subState;
        }

        public void Populate(
            Enemy enemy,
            Tuple<Player, int> playerDat,
            Random rand,
            string lootState,
            IList<LootDef> lootDefs
            )
        {
            foreach (ILootDef i in children)
                i.Populate(enemy, playerDat, rand, Lootstate, lootDefs);
        }
    }

    public enum ItemType
    {
        Weapon,
        Ability,
        Armor,
        Ring,
        Potion,
        Any
    }

    public enum EggRarity
    {
        Common = 0,
        Uncommon = 14,
        Rare = 28,
        Legendary = 42,
        Divine = 49
    }

    public class EggLoot : ILootDef
    {
        private readonly EggRarity rarity;
        private readonly double probability;

        public string Lootstate { get; set; }

        public EggLoot(EggRarity rarity, double probability)
        {
            this.probability = probability;
            this.rarity = rarity;
        }

        public void Populate(
            Enemy enemy,
            Tuple<Player, int> playerDat,
            Random rand,
            string lootState,
            IList<LootDef> lootDefs
            )
        {
            Lootstate = lootState;
            if (playerDat != null) return;
            Item[] candidates = Program.Manager.GameData.Items
                .Where(item => item.Value.SlotType == 9000)
                .Where(item => item.Value.minStars <= (int)rarity)
                .Select(item => item.Value)
                .ToArray();
            foreach (Item i in candidates)
                lootDefs.Add(new LootDef(i, probability / candidates.Length, lootState));
        }
    }

    public class TierLoot : ILootDef
    {
        public static readonly int[] WeaponSlotType = { 1, 2, 3, 8, 17, 24 };
        public static readonly int[] AbilitySlotType = { 4, 5, 11, 12, 13, 15, 16, 18, 19, 20, 21, 22, 23, 25 };
        public static readonly int[] ArmorSlotType = { 6, 7, 14 };
        public static readonly int[] RingSlotType = { 9 };
        public static readonly int[] PotionSlotType = { 10 };
        private readonly double probability;

        private readonly byte tier;
        private readonly int[] types;

        public string Lootstate { get; set; }

        public TierLoot(byte tier, ItemType type, double probability = 0)
        {
            this.tier = tier;
            switch (type)
            {
                case ItemType.Weapon:
                    types = WeaponSlotType;
                    break;
                case ItemType.Ability:
                    types = AbilitySlotType;
                    break;
                case ItemType.Armor:
                    types = ArmorSlotType;
                    break;
                case ItemType.Ring:
                    types = RingSlotType;
                    break;
                case ItemType.Potion:
                    types = PotionSlotType;
                    break;
                default:
                    throw new NotSupportedException(type.ToString());
            }
            this.probability = probability != 0 ? probability : ReturnProbability(type, tier);
        }

        private double ReturnProbability(ItemType type, byte tier)
        {
            double _tier = tier + 1;
            double probability = 0;
            switch (type)
            {
                case ItemType.Weapon:
                case ItemType.Armor: probability = 5 / (_tier * 10); break;
                case ItemType.Ability:
                case ItemType.Ring: probability = 2.5 / (_tier * 10); break;
            }
            return probability;
        }

        public void Populate(
            Enemy enemy,
            Tuple<Player, int> playerDat,
            Random rand,
            string lootState,
            IList<LootDef> lootDefs
            )
        {
            Lootstate = lootState;
            if (playerDat != null) return;
            Item[] candidates = Program.Manager.GameData.Items
                .Where(item => Array.IndexOf(types, item.Value.SlotType) != -1)
                .Where(item => item.Value.Tier == tier)
                .Select(item => item.Value)
                .ToArray();
            foreach (Item i in candidates)
                lootDefs.Add(new LootDef(i, probability / candidates.Length, lootState));
        }
    }

    public class Threshold : ILootDef
    {
        private readonly ILootDef[] children;
        private readonly double threshold;

        public string Lootstate { get; set; }

        public Threshold(double threshold, params ILootDef[] children)
        {
            this.threshold = threshold;
            this.children = children;
        }

        public void Populate(
            Enemy enemy,
            Tuple<Player, int> playerDat,
            Random rand,
            string lootState,
            IList<LootDef> lootDefs
            )
        {
            Lootstate = lootState;
            if (playerDat != null && playerDat.Item2 / enemy.ObjectDesc.MaxHP >= threshold)
            {
                foreach (ILootDef i in children)
                    i.Populate(enemy, null, rand, lootState, lootDefs);
            }
        }
    }

    public class Drops : ILootDef
    {
        private readonly ILootDef[] children;

        public string Lootstate { get; set; }

        public Drops(params ILootDef[] children)
        {
            this.children = children;
        }

        public void Populate(
            Enemy enemy,
            Tuple<Player, int> playerDat,
            Random rand,
            string lootState,
            IList<LootDef> lootDefs
            )
        {
            Lootstate = lootState;
            if (playerDat != null)
            {
                foreach (ILootDef i in children)
                    i.Populate(enemy, null, rand, lootState, lootDefs);
            }
        }
    }

    internal class MostDamagers : ILootDef
    {
        private readonly ILootDef[] loots;
        private readonly int amount;

        public MostDamagers(int amount, params ILootDef[] loots)
        {
            this.amount = amount;
            this.loots = loots;
        }

        public string Lootstate { get; set; }

        public void Populate(
            Enemy enemy,
            Tuple<Player, int> playerDat,
            Random rand,
            string lootState,
            IList<LootDef> lootDefs
            )
        {
            var data = enemy.DamageCounter.GetPlayerData();
            var mostDamage = GetMostDamage(data);
            foreach (var loot in mostDamage.Where(pl => pl.Equals(playerDat)).SelectMany(pl => loots))
                loot.Populate(enemy, null, rand, lootState, lootDefs);
        }

        private IEnumerable<Tuple<Player, int>> GetMostDamage(IEnumerable<Tuple<Player, int>> data)
        {
            var damages = data.Select(_ => _.Item2).ToList();
            var len = damages.Count < amount ? damages.Count : amount;
            for (var i = 0; i < len; i++)
            {
                var val = damages.Max();
                yield return data.FirstOrDefault(_ => _.Item2 == val);
                damages.Remove(val);
            }
        }
    }

    public class OnlyOne : ILootDef
    {
        private readonly ILootDef[] loots;

        public OnlyOne(params ILootDef[] loots)
        {
            this.loots = loots;
        }

        public string Lootstate { get; set; }

        public void Populate(
            Enemy enemy,
            Tuple<Player, int> playerDat,
            Random rand,
            string lootState,
            IList<LootDef> lootDefs
            )
        {
            loots[rand.Next(0, loots.Length)].Populate(enemy, playerDat, rand, lootState, lootDefs);
        }
    }

    public static class LootTemplates
    {
        public static ILootDef[] DefaultEggLoot(EggRarity maxRarity)
        {
            switch (maxRarity)
            {
                case EggRarity.Common:
                    return new ILootDef[1] { new EggLoot(EggRarity.Common, 0.1) };
                case EggRarity.Uncommon:
                    return new ILootDef[2] { new EggLoot(EggRarity.Common, 0.1), new EggLoot(EggRarity.Uncommon, 0.05) };
                case EggRarity.Rare:
                    return new ILootDef[3] { new EggLoot(EggRarity.Common, 0.1), new EggLoot(EggRarity.Uncommon, 0.05), new EggLoot(EggRarity.Rare, 0.01) };
                case EggRarity.Legendary:
                    return new ILootDef[4] { new EggLoot(EggRarity.Common, 0.1), new EggLoot(EggRarity.Uncommon, 0.05), new EggLoot(EggRarity.Rare, 0.01), new EggLoot(EggRarity.Legendary, 0.001) };
                default:
                    throw new InvalidOperationException("Not a valid Egg Rarity");
            }
        }

        public static ILootDef[] StatIncreasePotionsLoot()
        {
            return new ILootDef[]
            {
                new OnlyOne(
                    new ItemLoot("Potion of Defense", 1),
                    new ItemLoot("Potion of Attack", 1),
                    new ItemLoot("Potion of Speed", 1),
                    new ItemLoot("Potion of Vitality", 1),
                    new ItemLoot("Potion of Wisdom", 1),
                    new ItemLoot("Potion of Dexterity", 1)
                )
            };
        }
    }
}