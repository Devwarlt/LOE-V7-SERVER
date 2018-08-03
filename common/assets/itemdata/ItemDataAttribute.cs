using Newtonsoft.Json;
using System;
using System.Xml.Linq;

namespace LoESoft.Core.assets.itemdata
{
    public abstract class GameItem
    {
        public string Name { get; set; }
        public string Description { get; set; }

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

    public class ItemDataAttribute : Attribute
    {
        public SlotTypes SlotType { get; private set; }

        public ItemDataAttribute(SlotTypes slotTypes)
        {
            SlotType = slotTypes;
        }
    }
}