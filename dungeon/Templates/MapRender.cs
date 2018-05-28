using System;
using LoESoft.Dungeon.utils;
using RotMG.Common.Rasterizer;

namespace LoESoft.Dungeon.templates
{
    public class MapRender
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

        public virtual void Rasterize() { }
    }
}