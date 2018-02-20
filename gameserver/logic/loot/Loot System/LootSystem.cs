using common.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace gameserver.logic.loot.Loot_System
{
    public partial class LootSystem
    {
        public enum LootType : byte
        {
            Equipment = 0,
            WhiteBag = 1,
            Any = 3
        }

        public static readonly Dictionary<string, ILootDef[]> loots = new Dictionary<string, ILootDef[]>();

        public static ILootDef[] GenerateLoot(
            LootType lootType,
            ItemType itemType,
            byte tier,
            string itemName,
            double probability,
            bool eventChest
            )
        {
            switch (lootType)
            {
                case LootType.Equipment:
                    return ProcessEquipmentsLoot(new List<Tuple<ItemType, byte>> { Tuple.Create(itemType, tier) });
                case LootType.Any:
                    {
                        List<string> items = new List<string>();
                        if (itemName.Contains('|'))
                            items.Concat(itemName.Split('|').ToArray());
                        else
                            items.Add(itemName);
                        return ProcessAnyLoot(items, probability);
                    }
                case LootType.WhiteBag:
                    {
                        List<string> items = new List<string>();
                        if (itemName.Contains('|'))
                            items.Concat(itemName.Split('|').ToArray());
                        else
                            items.Add(itemName);
                        return ProcessWhiteBagLoot(items, eventChest);
                    }
                default:
                    return null;
            }
        }

        private static ILootDef[] ProcessEquipmentsLoot(List<Tuple<ItemType, byte>> loot)
        {
            List<ILootDef> acquireLoot = new List<ILootDef>();
            for (int i = 0; i < loot.Count; i++)
                acquireLoot.Add(new MostDamagers(5, new TierLoot(loot[i].Item2, loot[i].Item1, ReturnProbability(loot[i].Item1, loot[i].Item2))));
            return acquireLoot.ToArray();
        }

        private static ILootDef[] ProcessAnyLoot(List<string> itemName, double probability)
        {
            List<ILootDef> acquireLoot = new List<ILootDef>();
            for (int i = 0; i < itemName.Count; i++)
                acquireLoot.Add(new MostDamagers(5, new ItemLoot(itemName[i], probability)));
            return acquireLoot.ToArray();
        }

        private static ILootDef[] ProcessWhiteBagLoot(List<string> itemName, bool eventChest)
        {
            List<ILootDef> acquireLoot = new List<ILootDef>();
            for (int i = 0; i < itemName.Count; i++)
                acquireLoot.Add(new ProcessWhiteBag(false, new MostDamagers(5, new ItemLoot(itemName[i], eventChest ? .01 : .05))));
            return acquireLoot.ToArray();
        }

        public class LootSerialization
        {
            public string Name { get; set; }
            public int LootID { get; set; }
            public ItemType ItemType { get; set; }
            public byte Tier { get; set; }
            public string ItemName { get; set; }
            public double ItemProbability { get; set; }
            public bool EventChest { get; set; }
            public LootType LootType { get; set; }

            private static ItemType SerializeItemType(string type)
            {
                if (type == "weapon")
                    return ItemType.Weapon;
                else if (type == "ability")
                    return ItemType.Ability;
                else if (type == "armor")
                    return ItemType.Armor;
                else if (type == "ring")
                    return ItemType.Ring;
                else
                    return ItemType.Any;
            }

            private static LootType SerializeLootType(string type)
            {
                if (type == "equipment")
                    return LootType.Equipment;
                else if (type == "whitebag")
                    return LootType.WhiteBag;
                else
                    return LootType.Any;
            }

            public static void PopulateLoot()
            {
                string[] lootAssets = Directory.EnumerateFiles("logic/loot/Loot System/xmls", "*.xml", SearchOption.AllDirectories).ToArray();
                Log.Info("Loots", $"Loaded {lootAssets.Length} enem{(lootAssets.Length > 1 ? "ies" : "y")} loot{(lootAssets.Length > 1 ? "s" : "")}.");
                for (int j = 0; j < lootAssets.Length; j++)
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(File.ReadAllText(lootAssets[j]));
                    XmlNodeList enemyLoot = xmldoc.GetElementsByTagName("Enemy");
                    List<Tuple<string, int>> addedLoot = new List<Tuple<string, int>>();
                    if (enemyLoot.Count > 0)
                        for (int i = 0; i < enemyLoot.Count; i++)
                            if (!addedLoot.Contains(Tuple.Create(enemyLoot[i].Attributes["name"].Value, Convert.ToInt32(enemyLoot[i]["Loot"].Attributes["id"].Value))))
                            {
                                LootSerialization lootSerialized = new LootSerialization();
                                lootSerialized.Name = enemyLoot[i].Attributes["name"].Value;
                                lootSerialized.LootID = Convert.ToInt32(enemyLoot[i]["Loot"].Attributes["id"].Value);
                                lootSerialized.ItemType =
                                    enemyLoot[i]["Loot"].Attributes["itemType"] == null ?
                                    ItemType.Any :
                                    SerializeItemType(enemyLoot[i]["Loot"].Attributes["itemType"].Value.ToLower());
                                lootSerialized.Tier =
                                    enemyLoot[i]["Loot"].Attributes["tier"] == null ?
                                    byte.MinValue :
                                    Convert.ToByte(enemyLoot[i]["Loot"].Attributes["tier"].Value);
                                lootSerialized.ItemName =
                                    enemyLoot[i]["Loot"].Attributes["itemName"] == null ?
                                    null :
                                    enemyLoot[i]["Loot"].Attributes["itemName"].Value;
                                lootSerialized.ItemProbability =
                                    enemyLoot[i]["Loot"].Attributes["prob"] == null ?
                                    double.MinValue :
                                    Convert.ToDouble(enemyLoot[i]["Loot"].Attributes["prob"].Value);
                                lootSerialized.EventChest =
                                    enemyLoot[i]["Loot"].Attributes["chest"] == null ?
                                    false :
                                    Convert.ToBoolean(enemyLoot[i]["Loot"].Attributes["chest"].Value);
                                lootSerialized.LootType = SerializeLootType(enemyLoot[i]["Loot"].InnerText.ToLower());
                                loots.Add(
                                    lootSerialized.Name,
                                    GenerateLoot(
                                        lootSerialized.LootType,
                                        lootSerialized.ItemType,
                                        lootSerialized.Tier,
                                        lootSerialized.ItemName,
                                        lootSerialized.ItemProbability,
                                        lootSerialized.EventChest
                                    )
                                );
                                addedLoot.Add(Tuple.Create(lootSerialized.Name, lootSerialized.LootID));
                            }
                }
            }
        }

        private static double ReturnProbability(ItemType type, byte tier)
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
    }
}