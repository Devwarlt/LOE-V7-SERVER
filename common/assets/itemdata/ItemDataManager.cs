﻿using LoESoft.Core.models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;

namespace LoESoft.Core.assets.itemdata
{
    public class ItemDataManager
    {
        private string DirectoryAssemblyPath => Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "assets/items");
        private readonly Dictionary<string, GameItem> itemDatas;

        private readonly Dictionary<SlotTypes, string> Slots = new Dictionary<SlotTypes, string>
        {
            { SlotTypes.AMULET_SLOT, "//Amulet" },
            { SlotTypes.ARMOR_SLOT, "//Armor" },
            { SlotTypes.BOOT_SLOT, "//Boot" },
            { SlotTypes.CORPSE_SLOT, "//Corpse" },
            { SlotTypes.HELMET_SLOT, "//Helmet" },
            { SlotTypes.OBJECT_SLOT, "//Object" },
            { SlotTypes.RING_SLOT, "//Ring" },
            { SlotTypes.SHIELD_SLOT, "//Shield" },
            { SlotTypes.TROUSER_SLOT, "//Trouser" },
            { SlotTypes.WEAPON_SLOT, "//Weapon" }
        };

        private ConcurrentDictionary<SlotTypes, int> ItemDatasCount { get; set; }

        public IDictionary<string, GameItem> ItemDatas { get; private set; }

        public ItemDataManager()
        {
            ItemDatasCount = new ConcurrentDictionary<SlotTypes, int>();

            foreach (var i in Slots.Keys)
                ItemDatasCount.TryAdd(i, 0);

            ItemDatas = new ReadOnlyDictionary<string, GameItem>(itemDatas = new Dictionary<string, GameItem>());
        }

        public void Start()
        {
            bool loaded = false;
            string[] xmls = Directory.EnumerateFiles(DirectoryAssemblyPath, "*.xml", SearchOption.AllDirectories).ToArray();
            try
            {
                for (int i = 0; i < xmls.Length; i++)
                {
                    using (Stream stream = File.OpenRead(xmls[i]))
                        Items(XElement.Load(stream));

                    if (i + 1 == xmls.Length)
                        loaded = true;
                }

                int total = 0;

                foreach (var i in Slots)
                    total += ItemDatasCount[i.Key];

                Log._($"item data assets and {total} item{(total > 1 ? "s" : "")}", xmls.Length, false, "Loaded ", " ", ".");

                foreach (var i in Slots)
                    Log._(i.Value.Replace("//", null).ToLower(), ItemDatasCount[i.Key]);
            }
            catch (Exception ex) { Log.Error(ex.ToString()); }

            if (!loaded)
                return;
        }

        private void Items(XElement root)
        {
            Amulets(root);
            Armors(root);
            Boots(root);
            Corpses(root);
            Helmets(root);
            Objects(root);
            Rings(root);
            Shields(root);
            Trousers(root);
            Weapons(root);
        }

        private void Amulets(XElement root)
        {
            foreach (XElement e in root.XPathSelectElements(Slots[SlotTypes.AMULET_SLOT]))
            {
                string id = e.Attribute("id").Value;

                if (itemDatas.ContainsKey(id))
                {
                    Log.Warn($"Duplicated ItemData for 'Amulets': {id}.");
                    continue;
                }

                XElement assetData = e.Element("AssetData");
                XElement itemData = e.Element("ItemData");

                itemDatas[id] =
                    new Amulet()
                    {
                        File = GameItem.GetInheritData<string>(assetData, "File"),
                        Index = GameItem.GetInheritData<uint>(assetData, "Index"),
                        Name = e.Attribute("name").Value,
                        Description = e.Element("Description").Value,
                        AttackBonus = GameItem.GetInheritData<int>(itemData, "AttackBonus"),
                        DefenseBonus = GameItem.GetInheritData<int>(itemData, "DefenseBonus"),
                        HPBonus = GameItem.GetInheritData<int>(itemData, "HPBonus"),
                        MPBonus = GameItem.GetInheritData<int>(itemData, "MPBonus"),
                        Vocation = GameItem.GetInheritData<Vocations>(itemData, "Vocation"),
                        LevelBase = GameItem.GetInheritData<int>(itemData, "LevelBase"),
                        Arm = GameItem.GetInheritData<int>(itemData, "Arm"),
                    };

                ItemDatasCount[SlotTypes.AMULET_SLOT] += 1;
            }
        }

        private void Armors(XElement root)
        {
            foreach (XElement e in root.XPathSelectElements(Slots[SlotTypes.ARMOR_SLOT]))
            {
                string id = e.Attribute("id").Value;

                if (itemDatas.ContainsKey(id))
                {
                    Log.Warn($"Duplicated ItemData for 'Armors': {id}.");
                    continue;
                }

                XElement assetData = e.Element("AssetData");
                XElement itemData = e.Element("ItemData");

                itemDatas[id] =
                    new Armor()
                    {
                        File = GameItem.GetInheritData<string>(assetData, "File"),
                        Index = GameItem.GetInheritData<uint>(assetData, "Index"),
                        Name = e.Attribute("name").Value,
                        Description = e.Element("Description").Value,
                        AttackBonus = GameItem.GetInheritData<int>(itemData, "AttackBonus"),
                        DefenseBonus = GameItem.GetInheritData<int>(itemData, "DefenseBonus"),
                        HPBonus = GameItem.GetInheritData<int>(itemData, "HPBonus"),
                        MPBonus = GameItem.GetInheritData<int>(itemData, "MPBonus"),
                        Vocation = GameItem.GetInheritData<Vocations>(itemData, "Vocation"),
                        LevelBase = GameItem.GetInheritData<int>(itemData, "LevelBase"),
                        Arm = GameItem.GetInheritData<int>(itemData, "Arm")
                    };

                ItemDatasCount[SlotTypes.ARMOR_SLOT] += 1;
            }
        }

        private void Boots(XElement root)
        {
            foreach (XElement e in root.XPathSelectElements(Slots[SlotTypes.BOOT_SLOT]))
            {
                string id = e.Attribute("id").Value;

                if (itemDatas.ContainsKey(id))
                {
                    Log.Warn($"Duplicated ItemData for 'Boots': {id}.");
                    continue;
                }

                XElement assetData = e.Element("AssetData");
                XElement itemData = e.Element("ItemData");

                itemDatas[id] =
                    new Boot()
                    {
                        File = GameItem.GetInheritData<string>(assetData, "File"),
                        Index = GameItem.GetInheritData<uint>(assetData, "Index"),
                        Name = e.Attribute("name").Value,
                        Description = e.Element("Description").Value,
                        AttackBonus = GameItem.GetInheritData<int>(itemData, "AttackBonus"),
                        DefenseBonus = GameItem.GetInheritData<int>(itemData, "DefenseBonus"),
                        HPBonus = GameItem.GetInheritData<int>(itemData, "HPBonus"),
                        MPBonus = GameItem.GetInheritData<int>(itemData, "MPBonus"),
                        Vocation = GameItem.GetInheritData<Vocations>(itemData, "Vocation"),
                        LevelBase = GameItem.GetInheritData<int>(itemData, "LevelBase"),
                        Arm = GameItem.GetInheritData<int>(itemData, "Arm")
                    };

                ItemDatasCount[SlotTypes.BOOT_SLOT] += 1;
            }
        }

        private void Corpses(XElement root) => ProcessObjectsByType(root, SlotTypes.CORPSE_SLOT);

        private void Helmets(XElement root)
        {
            foreach (XElement e in root.XPathSelectElements(Slots[SlotTypes.HELMET_SLOT]))
            {
                string id = e.Attribute("id").Value;

                if (itemDatas.ContainsKey(id))
                {
                    Log.Warn($"Duplicated ItemData for 'Helmets': {id}.");
                    continue;
                }

                XElement assetData = e.Element("AssetData");
                XElement itemData = e.Element("ItemData");

                itemDatas[id] =
                    new Helmet()
                    {
                        File = GameItem.GetInheritData<string>(assetData, "File"),
                        Index = GameItem.GetInheritData<uint>(assetData, "Index"),
                        Name = e.Attribute("name").Value,
                        Description = e.Element("Description").Value,
                        AttackBonus = GameItem.GetInheritData<int>(itemData, "AttackBonus"),
                        DefenseBonus = GameItem.GetInheritData<int>(itemData, "DefenseBonus"),
                        HPBonus = GameItem.GetInheritData<int>(itemData, "HPBonus"),
                        MPBonus = GameItem.GetInheritData<int>(itemData, "MPBonus"),
                        Vocation = GameItem.GetInheritData<Vocations>(itemData, "Vocation"),
                        LevelBase = GameItem.GetInheritData<int>(itemData, "LevelBase"),
                        Arm = GameItem.GetInheritData<int>(itemData, "Arm")
                    };

                ItemDatasCount[SlotTypes.HELMET_SLOT] += 1;
            }
        }

        private void Objects(XElement root) => ProcessObjectsByType(root, SlotTypes.OBJECT_SLOT);

        private void ProcessObjectsByType(XElement root, SlotTypes slot)
        {
            foreach (XElement e in root.XPathSelectElements(Slots[slot]))
            {
                string id = e.Attribute("id").Value;

                if (itemDatas.ContainsKey(id))
                {
                    Log.Warn($"Duplicated ItemData for '{Slots[slot].Replace("//", null)}s': {id}.");
                    continue;
                }

                XElement assetData = e.Element("AssetData");
                XElement itemData = e.Element("ItemData");
                XElement decayData = itemData.Element("DecayData");
                List<DecayData> decayInfo = new List<DecayData>();

                if (decayData != null)
                    foreach (XElement i in decayData.Elements())
                        decayInfo.Add(
                            new DecayData()
                            {
                                File = i.Attribute("file").Value,
                                Index = uint.Parse(i.Attribute("index").Value.Replace("0x", null), NumberStyles.AllowHexSpecifier),
                                Priority = int.Parse(i.Attribute("priority").Value),
                                Time = int.Parse(i.Attribute("time").Value)
                            });

                itemDatas[id] =
                    new Object()
                    {
                        File = GameItem.GetInheritData<string>(assetData, "File"),
                        Index = GameItem.GetInheritData<uint>(assetData, "Index"),
                        Name = e.Attribute("name").Value,
                        Description = e.Element("Description") == null ? null : e.Element("Description").Value,
                        AttackBonus = 0,
                        DefenseBonus = 0,
                        HPBonus = 0,
                        MPBonus = 0,
                        Vocation = Vocations.ANY,
                        LevelBase = -1,
                        Amount = 1,
                        Stackable = GameItem.GetInheritData<bool>(itemData, "Stackable"),
                        Consumable = GameItem.GetInheritData<bool>(itemData, "Consumable"),
                        HP = GameItem.GetInheritData<int>(itemData, "HP"),
                        MP = GameItem.GetInheritData<int>(itemData, "MP"),
                        DecayData = decayInfo
                    };

                ItemDatasCount[slot] += 1;
            }
        }

        private void Rings(XElement root)
        {
            foreach (XElement e in root.XPathSelectElements(Slots[SlotTypes.RING_SLOT]))
            {
                string id = e.Attribute("id").Value;

                if (itemDatas.ContainsKey(id))
                {
                    Log.Warn($"Duplicated ItemData for 'Rings': {id}.");
                    continue;
                }

                XElement assetData = e.Element("AssetData");
                XElement itemData = e.Element("ItemData");

                itemDatas[id] =
                    new Ring()
                    {
                        File = GameItem.GetInheritData<string>(assetData, "File"),
                        Index = GameItem.GetInheritData<uint>(assetData, "Index"),
                        Name = e.Attribute("name").Value,
                        Description = e.Element("Description").Value,
                        AttackBonus = GameItem.GetInheritData<int>(itemData, "AttackBonus"),
                        DefenseBonus = GameItem.GetInheritData<int>(itemData, "DefenseBonus"),
                        HPBonus = GameItem.GetInheritData<int>(itemData, "HPBonus"),
                        MPBonus = GameItem.GetInheritData<int>(itemData, "MPBonus"),
                        Vocation = GameItem.GetInheritData<Vocations>(itemData, "Vocation"),
                        LevelBase = GameItem.GetInheritData<int>(itemData, "LevelBase"),
                        Arm = GameItem.GetInheritData<int>(itemData, "Arm")
                    };

                ItemDatasCount[SlotTypes.RING_SLOT] += 1;
            }
        }

        private void Shields(XElement root)
        {
            foreach (XElement e in root.XPathSelectElements(Slots[SlotTypes.SHIELD_SLOT]))
            {
                string id = e.Attribute("id").Value;

                if (itemDatas.ContainsKey(id))
                {
                    Log.Warn($"Duplicated ItemData for 'Shields': {id}.");
                    continue;
                }

                XElement assetData = e.Element("AssetData");
                XElement itemData = e.Element("ItemData");

                itemDatas[id] =
                    new Shield()
                    {
                        File = GameItem.GetInheritData<string>(assetData, "File"),
                        Index = GameItem.GetInheritData<uint>(assetData, "Index"),
                        Name = e.Attribute("name").Value,
                        Description = e.Element("Description").Value,
                        AttackBonus = GameItem.GetInheritData<int>(itemData, "AttackBonus"),
                        DefenseBonus = GameItem.GetInheritData<int>(itemData, "DefenseBonus"),
                        HPBonus = GameItem.GetInheritData<int>(itemData, "HPBonus"),
                        MPBonus = GameItem.GetInheritData<int>(itemData, "MPBonus"),
                        Vocation = GameItem.GetInheritData<Vocations>(itemData, "Vocation"),
                        LevelBase = GameItem.GetInheritData<int>(itemData, "LevelBase"),
                        Defense = GameItem.GetInheritData<int>(itemData, "Defense"),
                        BlockChance = GameItem.GetInheritData<int>(itemData, "BlockChance")
                    };

                ItemDatasCount[SlotTypes.SHIELD_SLOT] += 1;
            }
        }

        private void Trousers(XElement root)
        {
            foreach (XElement e in root.XPathSelectElements(Slots[SlotTypes.TROUSER_SLOT]))
            {
                string id = e.Attribute("id").Value;

                if (itemDatas.ContainsKey(id))
                {
                    Log.Warn($"Duplicated ItemData for 'Trousers': {id}.");
                    continue;
                }

                XElement assetData = e.Element("AssetData");
                XElement itemData = e.Element("ItemData");

                itemDatas[id] =
                    new Trouser()
                    {
                        File = GameItem.GetInheritData<string>(assetData, "File"),
                        Index = GameItem.GetInheritData<uint>(assetData, "Index"),
                        Name = e.Attribute("name").Value,
                        Description = e.Element("Description").Value,
                        AttackBonus = GameItem.GetInheritData<int>(itemData, "AttackBonus"),
                        DefenseBonus = GameItem.GetInheritData<int>(itemData, "DefenseBonus"),
                        HPBonus = GameItem.GetInheritData<int>(itemData, "HPBonus"),
                        MPBonus = GameItem.GetInheritData<int>(itemData, "MPBonus"),
                        Vocation = GameItem.GetInheritData<Vocations>(itemData, "Vocation"),
                        LevelBase = GameItem.GetInheritData<int>(itemData, "LevelBase"),
                        Arm = GameItem.GetInheritData<int>(e.Element("ItemData"), "Arm")
                    };

                ItemDatasCount[SlotTypes.TROUSER_SLOT] += 1;
            }
        }

        private void Weapons(XElement root)
        {
            foreach (XElement e in root.XPathSelectElements(Slots[SlotTypes.WEAPON_SLOT]))
            {
                string id = e.Attribute("id").Value;

                if (itemDatas.ContainsKey(id))
                {
                    Log.Warn($"Duplicated ItemData for 'Weapons': {id}.");
                    continue;
                }

                XElement assetData = e.Element("AssetData");
                XElement itemData = e.Element("ItemData");

                itemDatas[id] =
                    new Weapon()
                    {
                        File = GameItem.GetInheritData<string>(assetData, "File"),
                        Index = GameItem.GetInheritData<uint>(assetData, "Index"),
                        Name = e.Attribute("name").Value,
                        Description = e.Element("Description").Value,
                        AttackBonus = GameItem.GetInheritData<int>(itemData, "AttackBonus"),
                        DefenseBonus = GameItem.GetInheritData<int>(itemData, "DefenseBonus"),
                        HPBonus = GameItem.GetInheritData<int>(itemData, "HPBonus"),
                        MPBonus = GameItem.GetInheritData<int>(itemData, "MPBonus"),
                        Vocation = GameItem.GetInheritData<Vocations>(itemData, "Vocation"),
                        LevelBase = GameItem.GetInheritData<int>(itemData, "LevelBase"),
                        Attack = GameItem.GetInheritData<int>(itemData, "Attack"),
                        Mana = GameItem.GetInheritData<int>(itemData, "Mana"),
                        HitChance = GameItem.GetInheritData<int>(itemData, "HitChance"),
                        TwoHanded = GameItem.GetInheritData<bool>(itemData, "TwoHanded")
                    };

                ItemDatasCount[SlotTypes.WEAPON_SLOT] += 1;
            }
        }
    }
}