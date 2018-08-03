using Newtonsoft.Json;
using System;
using System.Linq;
using System.Xml.Linq;

namespace LoESoft.Core.assets.itemdata
{
    public abstract class GameItem
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual SlotTypes SlotType { get { return GetAttribute.SlotType; } }

        private ItemDataAttribute GetAttribute => GetType().GetCustomAttributes(typeof(ItemDataAttribute), true).FirstOrDefault() as ItemDataAttribute;

        public Slot Import<Slot>(string slot) => (Slot)JsonConvert.DeserializeObject(slot);

        public string Export<Slot>(Slot slot) => JsonConvert.SerializeObject(slot);

        public static T GetInheritData<T>(XElement parent, string child)
        {
            if (typeof(T) == typeof(int))
                return (T)(object)int.Parse(parent.Element(child).Value);
            if (typeof(T) == typeof(string))
                return (T)(object)parent.Element(child).Value;
            if (typeof(T) == typeof(bool))
                return (T)(object)bool.Parse(parent.Element(child).Value);

            return (T)(object)null;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ItemDataAttribute : Attribute
    {
        public SlotTypes SlotType { get; set; }

        public ItemDataAttribute(SlotTypes slotType)
        {
            SlotType = slotType;
        }
    }
}