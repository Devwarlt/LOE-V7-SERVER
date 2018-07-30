using Newtonsoft.Json;

namespace LoESoft.Core.models
{
    /// <summary>
    /// Item Data
    /// Author: DV
    /// </summary>
    public abstract class ItemData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public SlotTypes SlotType { get; set; }
        public string Parameters { get; set; }
    }

    public class AmuletSlotData : ItemData
    {
        public AmuletSlotData(string Name, string Description, int Arm, string Parameters)
        {
            this.Name = Name;
            this.Description = Description;
            this.Arm = Arm;
            this.Parameters = Parameters;

            SlotType = SlotTypes.AMULET_SLOT;
        }

        public int Arm { get; set; }

        public string Export(AmuletSlotData itemData)
            => JsonConvert.SerializeObject(itemData);

        public AmuletSlotData Import(string itemData)
            => (AmuletSlotData)JsonConvert.DeserializeObject(itemData);
    }

    public class HeadSlotData : ItemData
    {
        public HeadSlotData(string Name, string Description, int Arm, string Parameters)
        {
            this.Name = Name;
            this.Description = Description;
            this.Arm = Arm;
            this.Parameters = Parameters;

            SlotType = SlotTypes.HEAD_SLOT;
        }

        public int Arm { get; set; }

        public string Export(HeadSlotData itemData)
            => JsonConvert.SerializeObject(itemData);

        public HeadSlotData Import(string itemData)
            => (HeadSlotData)JsonConvert.DeserializeObject(itemData);
    }

    public class LeftHandSlotData : ItemData
    {
        public LeftHandSlotData(string Name, string Description, int Attack, bool TwoHanded, string Parameters)
        {
            this.Name = Name;
            this.Description = Description;
            this.Attack = Attack;
            this.TwoHanded = TwoHanded;
            this.Parameters = Parameters;

            SlotType = SlotTypes.LEFT_HAND_SLOT;
        }

        public int Attack { get; set; }
        public bool TwoHanded { get; set; }

        public string Export(LeftHandSlotData itemData)
            => JsonConvert.SerializeObject(itemData);

        public LeftHandSlotData Import(string itemData)
            => (LeftHandSlotData)JsonConvert.DeserializeObject(itemData);
    }

    public class ArmorSlotData : ItemData
    {
        public ArmorSlotData(string Name, string Description, int Arm, string Parameters)
        {
            this.Name = Name;
            this.Description = Description;
            this.Arm = Arm;
            this.Parameters = Parameters;

            SlotType = SlotTypes.ARMOR_SLOT;
        }

        public int Arm { get; set; }

        public string Export(ArmorSlotData itemData)
            => JsonConvert.SerializeObject(itemData);

        public ArmorSlotData Import(string itemData)
            => (ArmorSlotData)JsonConvert.DeserializeObject(itemData);
    }

    public class RightHandSlotData : ItemData
    {
        public RightHandSlotData(string Name, string Description, int Defense, string Parameters)
        {
            this.Name = Name;
            this.Description = Description;
            this.Defense = Defense;
            this.Parameters = Parameters;

            SlotType = SlotTypes.RIGHT_HAND_SLOT;
        }
        
        public int Defense { get; set; }

        public string Export(RightHandSlotData itemData)
            => JsonConvert.SerializeObject(itemData);

        public RightHandSlotData Import(string itemData)
            => (RightHandSlotData)JsonConvert.DeserializeObject(itemData);
    }

    public class RingSlotData : ItemData
    {
        public RingSlotData(string Name, string Description, int Arm, string Parameters)
        {
            this.Name = Name;
            this.Description = Description;
            this.Arm = Arm;
            this.Parameters = Parameters;

            SlotType = SlotTypes.RING_SLOT;
        }
        
        public int Arm { get; set; }

        public string Export(RingSlotData itemData)
            => JsonConvert.SerializeObject(itemData);

        public RingSlotData Import(string itemData)
            => (RingSlotData)JsonConvert.DeserializeObject(itemData);
    }

    public class LegsSlotData : ItemData
    {
        public LegsSlotData(string Name, string Description, int Arm, string Parameters)
        {
            this.Name = Name;
            this.Description = Description;
            this.Arm = Arm;
            this.Parameters = Parameters;

            SlotType = SlotTypes.LEGS_SLOT;
        }
        
        public int Arm { get; set; }

        public string Export(LegsSlotData itemData)
            => JsonConvert.SerializeObject(itemData);

        public LegsSlotData Import(string itemData)
            => (LegsSlotData)JsonConvert.DeserializeObject(itemData);
    }

    public class FootSlotData : ItemData
    {
        public FootSlotData(string Name, string Description, int Arm, string Parameters)
        {
            this.Name = Name;
            this.Description = Description;
            this.Arm = Arm;
            this.Parameters = Parameters;

            SlotType = SlotTypes.FOOT_SLOT;
        }
        
        public int Arm { get; set; }

        public string Export(FootSlotData itemData)
            => JsonConvert.SerializeObject(itemData);

        public FootSlotData Import(string itemData)
            => (FootSlotData)JsonConvert.DeserializeObject(itemData);
    }

    public enum SlotTypes
    {
        AMULET_SLOT = 0,
        HEAD_SLOT = 1,
        LEFT_HAND_SLOT = 2,
        ARMOR_SLOT = 3,
        RIGHT_HAND_SLOT = 4,
        RING_SLOT = 5,
        LEGS_SLOT = 6,
        FOOT_SLOT = 7
    }
}