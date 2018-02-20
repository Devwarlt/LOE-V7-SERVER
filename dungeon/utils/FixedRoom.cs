using dungeon.engine;
using System;

namespace dungeon.utils
{
    public abstract class FixedRoom : Room
    {
        public abstract Tuple<Direction, int>[] ConnectionPoints { get; }

        public override Range NumBranches { get { return new Range(1, ConnectionPoints.Length); } }
    }
}