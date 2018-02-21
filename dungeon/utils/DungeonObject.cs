using System.Collections.Generic;
using RotMG.Common;

namespace LoESoft.Dungeon.utils
{
    public struct ObjectType
    {
        public readonly uint Id;
        public readonly string Name;

        public ObjectType(uint id, string name)
        {
            Id = id;
            Name = name;
        }

        public static bool operator ==(ObjectType a, ObjectType b)
        {
            return a.Id == b.Id || a.Name == b.Name;
        }

        public static bool operator !=(ObjectType a, ObjectType b)
        {
            return a.Id != b.Id && a.Name != b.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is ObjectType && (ObjectType)obj == this;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class DungeonObject
    {
        public ObjectType ObjectType;
        public KeyValuePair<string, string>[] Attributes = Empty<KeyValuePair<string, string>>.Array;
    }
}