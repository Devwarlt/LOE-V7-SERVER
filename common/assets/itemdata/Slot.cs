namespace LoESoft.Core.assets.itemdata
{
    public class Amulet : ItemData
    {
        public override SlotTypes SlotType { get => base.SlotType; protected set => base.SlotType = SlotTypes.AMULET_SLOT; }
        public int Arm { get; set; }
    }

    public class Head : ItemData
    {
        public override SlotTypes SlotType { get => base.SlotType; protected set => base.SlotType = SlotTypes.HEAD_SLOT; }
        public int Arm { get; set; }
    }

    public class Weapon : ItemData
    {
        public override SlotTypes SlotType { get => base.SlotType; protected set => base.SlotType = SlotTypes.WEAPON_SLOT; }
        public int Attack { get; set; }
        public bool TwoHanded { get; set; }
    }

    public class Armor : ItemData
    {
        public override SlotTypes SlotType { get => base.SlotType; protected set => base.SlotType = SlotTypes.ARMOR_SLOT; }
        public int Arm { get; set; }
    }

    public class Shield : ItemData
    {
        public override SlotTypes SlotType { get => base.SlotType; protected set => base.SlotType = SlotTypes.SHIELD_SLOT; }
        public int Defense { get; set; }
    }

    public class Ring : ItemData
    {
        public override SlotTypes SlotType { get => base.SlotType; protected set => base.SlotType = SlotTypes.RING_SLOT; }
        public int Arm { get; set; }
    }

    public class Legs : ItemData
    {
        public override SlotTypes SlotType { get => base.SlotType; protected set => base.SlotType = SlotTypes.LEGS_SLOT; }
        public int Arm { get; set; }
    }

    public class Foot : ItemData
    {
        public override SlotTypes SlotType { get => base.SlotType; protected set => base.SlotType = SlotTypes.FOOT_SLOT; }
        public int Arm { get; set; }
    }
}
