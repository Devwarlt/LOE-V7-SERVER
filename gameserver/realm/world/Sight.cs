#region

using LoESoft.GameServer.realm.entity.player;
using System.Collections.Generic;

#endregion

namespace LoESoft.GameServer.realm
{
    public static class Sight
    {
        public static List<IntPoint> GetSquare(Player player, int radius)
        {
            var square = new List<IntPoint>();

            for (var x = -radius; x <= radius; x++)
                for (var y = -radius; y <= radius; y++)
                    square.Add(new IntPoint(x, y));

            return square;
        }
    }
}