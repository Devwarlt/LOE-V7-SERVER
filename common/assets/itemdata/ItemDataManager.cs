﻿using LoESoft.Core.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            { SlotTypes.HELMET_SLOT, "//Helmet" },
            { SlotTypes.RING_SLOT, "//Ring" },
            { SlotTypes.SHIELD_SLOT, "//Shield" },
            { SlotTypes.TROUSER_SLOT, "//Trouser" },
            { SlotTypes.WEAPON_SLOT, "//Weapon" }
        };

        public IDictionary<string, GameItem> ItemDatas { get; private set; }

        public ItemDataManager()
        {
            ItemDatas = new ReadOnlyDictionary<string, GameItem>(itemDatas = new Dictionary<string, GameItem>());
        }

        public void Load()
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
            Helmets(root);
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

                itemDatas[id] =
                    new Amulet()
                    {
                        Name = e.Attribute("name").Value,
                        Description = e.Element("Description").Value,
                        Arm = GameItem.GetInheritData<int>(e.Element("ItemData"), "Arm")
                    };
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

                itemDatas[id] =
                    new Armor()
                    {
                        Name = e.Attribute("name").Value,
                        Description = e.Element("Description").Value,
                        Arm = GameItem.GetInheritData<int>(e.Element("ItemData"), "Arm")
                    };
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

                itemDatas[id] =
                    new Boot()
                    {
                        Name = e.Attribute("name").Value,
                        Description = e.Element("Description").Value,
                        Arm = GameItem.GetInheritData<int>(e.Element("ItemData"), "Arm")
                    };
            }
        }

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

                itemDatas[id] =
                    new Helmet()
                    {
                        Name = e.Attribute("name").Value,
                        Description = e.Element("Description").Value,
                        Arm = GameItem.GetInheritData<int>(e.Element("ItemData"), "Arm")
                    };
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

                itemDatas[id] =
                    new Ring()
                    {
                        Name = e.Attribute("name").Value,
                        Description = e.Element("Description").Value,
                        Arm = GameItem.GetInheritData<int>(e.Element("ItemData"), "Arm")
                    };
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

                itemDatas[id] =
                    new Shield()
                    {
                        Name = e.Attribute("name").Value,
                        Description = e.Element("Description").Value,
                        Defense = GameItem.GetInheritData<int>(e.Element("ItemData"), "Defense")
                    };
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

                itemDatas[id] =
                    new Trouser()
                    {
                        Name = e.Attribute("name").Value,
                        Description = e.Element("Description").Value,
                        Arm = GameItem.GetInheritData<int>(e.Element("ItemData"), "Arm")
                    };
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

                itemDatas[id] =
                    new Weapon()
                    {
                        Name = e.Attribute("name").Value,
                        Description = e.Element("Description").Value,
                        Attack = GameItem.GetInheritData<int>(e.Element("ItemData"), "Attack"),
                        TwoHanded = GameItem.GetInheritData<bool>(e.Element("ItemData"), "TwoHanded")
                    };
            }
        }
    }
}