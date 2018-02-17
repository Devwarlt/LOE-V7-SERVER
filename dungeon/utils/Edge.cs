using System;
using System.Diagnostics;

namespace dungeon.utils
{
    public enum Direction
    {
        South = 0,
        East = 1,
        North = 2,
        West = 3
    }

    public struct Link
    {
        public readonly Direction Direction;
        public readonly int Offset;

        public Link(Direction direction, int offset)
        {
            Direction = direction;
            Offset = offset;
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}]", Direction, Offset);
        }
    }

    public class Edge
    {
        Edge()
        {
        }

        public Room RoomA { get; private set; }
        public Room RoomB { get; private set; }
        public Link Linkage { get; set; }

        public static void Link(Room a, Room b, Link link)
        {
            Debug.Assert(a != b);
            var edge = new Edge
            {
                RoomA = a,
                RoomB = b,
                Linkage = link
            };
            a.Edges.Add(edge);
            b.Edges.Add(edge);
        }

        public static void UnLink(Room a, Room b)
        {
            Edge edge = null;
            foreach (var ed in a.Edges)
                if (ed.RoomA == b || ed.RoomB == b)
                {
                    edge = ed;
                    break;
                }
            if (edge == null)
                throw new ArgumentException();
            a.Edges.Remove(edge);
            b.Edges.Remove(edge);
        }
    }
}