using System.Collections.Generic;

namespace LoESoft.Core.assets.itemdata
{
    [ItemData(SlotTypes.AMULET_SLOT)]
    public class Amulet : GameItem
    {
        public int Arm { get; set; }
    }

    [ItemData(SlotTypes.HELMET_SLOT)]
    public class Helmet : GameItem
    {
        public int Arm { get; set; }
    }

    [ItemData(SlotTypes.WEAPON_SLOT)]
    public class Weapon : GameItem
    {
        public int Attack { get; set; }
        public int Mana { get; set; }
        public int HitChance { get; set; }
        public bool TwoHanded { get; set; }
    }

    [ItemData(SlotTypes.ARMOR_SLOT)]
    public class Armor : GameItem
    {
        public int Arm { get; set; }
    }

    [ItemData(SlotTypes.SHIELD_SLOT)]
    public class Shield : GameItem
    {
        public int Defense { get; set; }
        public int BlockChance { get; set; }
    }

    [ItemData(SlotTypes.RING_SLOT)]
    public class Ring : GameItem
    {
        public int Arm { get; set; }
    }

    [ItemData(SlotTypes.TROUSER_SLOT)]
    public class Trouser : GameItem
    {
        public int Arm { get; set; }
    }

    [ItemData(SlotTypes.BOOT_SLOT)]
    public class Boot : GameItem
    {
        public int Arm { get; set; }
    }

    [ItemData(SlotTypes.OBJECT_SLOT)]
    public class Object : GameItem
    {
        public int Amount { get; set; }
        public bool Stackable { get; set; }
        public bool Consumable { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public int Capacity { get; set; }
        public List<DecayData> DecayData { get; set; }
    }

    public class DecayData
    {
        public int Priority { get; set; }
        public string File { get; set; }
        public uint Index { get; set; }
        public int Time { get; set; }
    }
}
