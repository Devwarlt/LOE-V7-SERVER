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
}
