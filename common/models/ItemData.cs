using Newtonsoft.Json;

namespace LoESoft.Core.models
{
    /// <summary>
    /// Item Data Feature
    /// Author: DV
    /// </summary>
    public class ItemData
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string SlotType { get; private set; }
        public string[] Parameters { get; private set; }

        public ItemData(int Id, string Name, string Description, int SlotType, string[] Parameters)
        {
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
            this.SlotType = ((SlotTypes)SlotType).ToString();
            this.Parameters = Parameters;
        }

        public static string GetItemData(ItemData itemData)
            => JsonConvert.SerializeObject(itemData);
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