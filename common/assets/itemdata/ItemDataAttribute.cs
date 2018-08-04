using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace LoESoft.Core.assets.itemdata
{
    public abstract class GameItem
    {
        public string File { get; set; }
        public uint Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AttackBonus { get; set; }
        public int DefenseBonus { get; set; }
        public int HPBonus { get; set; }
        public int MPBonus { get; set; }
        public Vocations Vocation { get; set; }
        public int LevelBase { get; set; }

        public virtual SlotTypes SlotType => GetAttribute.SlotType;

        private ItemDataAttribute GetAttribute => GetType().GetCustomAttributes(typeof(ItemDataAttribute), true).FirstOrDefault() as ItemDataAttribute;

        public static Slot Import<Slot>(string slot) => (Slot)JsonConvert.DeserializeObject(slot);

        public static string Export<Slot>(Slot slot) => JsonConvert.SerializeObject(slot);

        public static T GetInheritData<T>(XElement parent, string child)
        {
            string data = !(parent.Element(child) == null) ? parent.Element(child).Value : null;

            if (typeof(T) == typeof(int))
                return (T)(object)(data == null ? 0 : int.Parse(data));
            if (typeof(T) == typeof(uint))
                return (T)(object)(data == null ? 0 : (data.Contains("0x") ? uint.Parse(data.Replace("0x", null), NumberStyles.AllowHexSpecifier) : uint.Parse(data)));
            if (typeof(T) == typeof(string))
                return (T)(object)data;
            if (typeof(T) == typeof(bool))
                return (T)(object)(data == null ? false : bool.Parse(data));
            if (typeof(T) == typeof(double))
                return (T)(object)(data == null ? 0 : double.Parse(data));
            if (typeof(T) == typeof(Vocations))
                return (T)(object)(data == null ? Vocations.ANY : (Vocations)int.Parse(data));

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