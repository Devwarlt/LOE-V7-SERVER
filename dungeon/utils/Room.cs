using System;
using System.Collections.Generic;
using RotMG.Common.Rasterizer;
using LoESoft.Dungeon.engine;

namespace LoESoft.Dungeon.utils
{
    public enum RoomType
    {
        Normal,
        Start,
        Target,
        Special
    }

    public abstract class Room
    {
        protected Room()
        {
            Edges = new List<Edge>(4);
        }

        public IList<Edge> Edges { get; private set; }
        public int Depth { get; internal set; }

        public abstract RoomType Type { get; }
        public abstract int Width { get; }
        public abstract int Height { get; }

        public Point Pos { get; set; }

        public Rect Bounds { get { return new Rect(Pos.X, Pos.Y, Pos.X + Width, Pos.Y + Height); } }

        public virtual Range NumBranches { get { return new Range(1, 4); } }

        public abstract void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand);
    }
}