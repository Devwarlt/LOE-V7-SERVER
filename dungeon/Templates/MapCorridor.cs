﻿using System;
using LoESoft.Dungeon.utils;
using RotMG.Common;
using RotMG.Common.Rasterizer;

namespace LoESoft.Dungeon.templates
{
    public class MapCorridor
    {
        protected BitmapRasterizer<DungeonTile> Rasterizer { get; private set; }
        protected DungeonGraph Graph { get; private set; }
        protected Random Rand { get; private set; }

        internal void Init(BitmapRasterizer<DungeonTile> rasterizer, DungeonGraph graph, Random rand)
        {
            Rasterizer = rasterizer;
            Graph = graph;
            Rand = rand;
        }

        public virtual void Rasterize(Room src, Room dst, Point srcPos, Point dstPos)
        {
        }

        protected void Default(Point srcPos, Point dstPos, DungeonTile tile)
        {
            if (srcPos.X == dstPos.X)
            {
                if (srcPos.Y > dstPos.Y)
                    Utils.Swap(ref srcPos, ref dstPos);
                Rasterizer.FillRect(new Rect(srcPos.X, srcPos.Y, srcPos.X + Graph.Template.CorridorWidth, dstPos.Y), tile);
            }
            else if (srcPos.Y == dstPos.Y)
            {
                if (srcPos.X > dstPos.X)
                    Utils.Swap(ref srcPos, ref dstPos);
                Rasterizer.FillRect(new Rect(srcPos.X, srcPos.Y, dstPos.X, srcPos.Y + Graph.Template.CorridorWidth), tile);
            }
        }
    }
}