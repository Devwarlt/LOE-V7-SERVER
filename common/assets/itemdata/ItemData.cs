using Newtonsoft.Json;
using System;
using System.Xml.Linq;

namespace LoESoft.Core.assets.itemdata
{
    /// <summary>
    /// Item Data
    /// Author: DV
    /// </summary>
    public abstract class ItemData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual SlotTypes SlotType { get; protected set; }

        public string Export<SlotData>(SlotData slotData)
            => JsonConvert.SerializeObject(slotData);

        public SlotData Import<SlotData>(string slotData)
            => (SlotData)JsonConvert.DeserializeObject(slotData);

        public static T GetData<T>(XElement parent, string child)
        {
            if (typeof(T) == typeof(int))
                return (T)(object)int.Parse(parent.Element(child).Value);
            if (typeof(T) == typeof(string))
                return (T)(object)parent.Element(child).Value;
            if (typeof(T) == typeof(bool))
                return (T)(object)bool.Parse(parent.Element(child).Value);

            return (T)(object) null;
        }
    }
}