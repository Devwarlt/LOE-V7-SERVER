using Newtonsoft.Json;
using System;
using System.Linq;
using System.Xml.Linq;

namespace LoESoft.Core.assets.itemdata
{
    public abstract class GameItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int AttackBonus { get; set; }
        public int DefenseBonus { get; set; }
        public int HPBonus { get; set; }
        public int MPBonus { get; set; }

        public virtual SlotTypes SlotType => GetAttribute.SlotType;

        private ItemDataAttribute GetAttribute => GetType().GetCustomAttributes(typeof(ItemDataAttribute), true).FirstOrDefault() as ItemDataAttribute;

        public Slot Import<Slot>(string slot) => (Slot)JsonConvert.DeserializeObject(slot);

        public string Export<Slot>(Slot slot) => JsonConvert.SerializeObject(slot);

        public static T GetInheritData<T>(XElement parent, string child)
        {
            if (typeof(T) == typeof(int))
                return (T)(object)int.Parse(parent.Element(child) == null ? parent.Element(child).Value : "0");
            if (typeof(T) == typeof(string))
                return (T)(object)(parent.Element(child) == null ? parent.Element(child).Value : "null");
            if (typeof(T) == typeof(bool))
                return (T)(object)bool.Parse(parent.Element(child) == null ? parent.Element(child).Value : "false");
            if (typeof(T) == typeof(double))
                return (T)(object)double.Parse(parent.Element(child) == null ? parent.Element(child).Value : "0");

            return (T)(object)null;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ItemDataAttribute : Attribute
    {
        public SlotTypes SlotType { get; }

        public ItemDataAttribute(SlotTypes slotType)
        {
            SlotType = slotType;
        }
    }
}