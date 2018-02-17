using System;
using DungeonGenerator.Dungeon;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator.Templates.Difficult_5.Mad_Lab
{
    internal class BossRoom : FixedRoom
    {
        static readonly Rect template = new Rect(0, 0, 24, 50);

        public override RoomType Type { get { return RoomType.Target; } }

        public override int Width { get { return template.MaxX - template.X; } }

        public override int Height { get { return template.MaxY - template.Y; } }

        static readonly Tuple<Direction, int>[] connections = {
            Tuple.Create(Direction.South, 10)
        };

        public override Tuple<Direction, int>[] ConnectionPoints { get { return connections; } }

        public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand)
        {
            rasterizer.Copy(LabTemplate.MapTemplate, template, Pos);
            LabTemplate.DrawSpiderWeb(rasterizer, Bounds, rand);
        }
    }
}