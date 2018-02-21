using System;
using LoESoft.Dungeon.utils;
using RotMG.Common.Rasterizer;
using LoESoft.Dungeon.engine;

namespace LoESoft.Dungeon.templates.Difficult_4.Abyss_of_Demons
{
    internal class TreasureRoom : FixedRoom
    {
        public override RoomType Type { get { return RoomType.Special; } }

        public override int Width { get { return 15; } }

        public override int Height { get { return 21; } }

        static readonly Tuple<Direction, int>[] connections = {
            Tuple.Create(Direction.South, 6)
        };

        public override Tuple<Direction, int>[] ConnectionPoints { get { return connections; } }

        public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand)
        {
            rasterizer.Copy(AbyssTemplate.MapTemplate, new Rect(70, 10, 85, 31), Pos, tile => tile.TileType.Name == "Space");

            var bounds = Bounds;
            var buf = rasterizer.Bitmap;
            for (int x = bounds.X; x < bounds.MaxX; x++)
                for (int y = bounds.Y; y < bounds.MaxY; y++)
                {
                    if (buf[x, y].TileType != AbyssTemplate.Space)
                        buf[x, y].Region = "Treasure";
                }
        }
    }
}