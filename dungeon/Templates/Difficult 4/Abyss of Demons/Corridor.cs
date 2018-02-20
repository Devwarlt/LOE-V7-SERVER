using dungeon.utils;
using RotMG.Common.Rasterizer;

namespace dungeon.templates.Difficult_4.Abyss_of_Demons
{
    internal class Corridor : MapCorridor
    {
        public override void Rasterize(Room src, Room dst, Point srcPos, Point dstPos)
        {
            Default(srcPos, dstPos, new DungeonTile
            {
                TileType = AbyssTemplate.RedSmallChecks
            });
        }
    }
}