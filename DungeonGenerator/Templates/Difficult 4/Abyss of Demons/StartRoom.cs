using System;
using DungeonGenerator.Dungeon;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator.Templates.Difficult_4.Abyss_of_Demons
{
    internal class StartRoom : Room
    {
        readonly int len;
        internal Point portalPos;

        public StartRoom(int len)
        {
            this.len = len;
        }

        public override RoomType Type { get { return RoomType.Start; } }

        public override int Width { get { return len; } }

        public override int Height { get { return len; } }

        public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand)
        {
            rasterizer.FillRect(Bounds, new DungeonTile
            {
                TileType = AbyssTemplate.RedSmallChecks
            });

            var buf = rasterizer.Bitmap;
            var bounds = Bounds;

            bool portalPlaced = false;
            while (!portalPlaced)
            {
                int x = rand.Next(bounds.X + 2, bounds.MaxX - 4);
                int y = rand.Next(bounds.Y + 2, bounds.MaxY - 4);
                if (buf[x, y].Object != null)
                    continue;

                buf[x, y].Region = "Spawn";
                buf[x, y].Object = new DungeonObject
                {
                    ObjectType = AbyssTemplate.CowardicePortal
                };
                portalPos = new Point(x, y);
                portalPlaced = true;
            }
        }
    }
}