using System.Collections.Generic;
using dungeon.utils;
using RotMG.Common;
using RotMG.Common.Rasterizer;

namespace dungeon.engine
{
    public class RoomCollision
    {
        const int GridScale = 3;
        const int GridSize = 1 << GridScale;

        struct RoomKey
        {
            public readonly int XKey;
            public readonly int YKey;

            public RoomKey(int x, int y)
            {
                XKey = x >> GridScale;
                YKey = y >> GridScale;
            }

            public override int GetHashCode()
            {
                return XKey * 7 + YKey;
            }
        }

        readonly Dictionary<RoomKey, HashSet<Room>> rooms = new Dictionary<RoomKey, HashSet<Room>>();

        void Add(int x, int y, Room rm)
        {
            var key = new RoomKey(x, y);
            var roomList = rooms.GetValueOrCreate(key, k => new HashSet<Room>());
            roomList.Add(rm);
        }

        public void Add(Room rm)
        {
            var bounds = rm.Bounds;
            int x = bounds.X, y = bounds.Y;
            for (; y <= bounds.MaxY + GridSize; y += GridSize)
            {
                for (x = bounds.X; x <= bounds.MaxX + 20; x += GridSize)
                    Add(x, y, rm);
            }
        }

        void Remove(int x, int y, Room rm)
        {
            var key = new RoomKey(x, y);
            HashSet<Room> roomList;
            if (rooms.TryGetValue(key, out roomList))
                roomList.Remove(rm);
        }

        public void Remove(Room rm)
        {
            var bounds = rm.Bounds;
            int x = bounds.X, y = bounds.Y;
            for (; y <= bounds.MaxY + GridSize; y += GridSize)
            {
                for (x = bounds.X; x <= bounds.MaxX + 20; x += GridSize)
                    Remove(x, y, rm);
            }
        }

        bool HitTest(int x, int y, Rect bounds)
        {
            var key = new RoomKey(x, y);
            var roomList = rooms.GetValueOrDefault(key, (HashSet<Room>)null);
            if (roomList != null)
            {
                foreach (var room in roomList)
                    if (!room.Bounds.Intersection(bounds).IsEmpty)
                        return true;
            }
            return false;
        }

        public bool HitTest(Room rm)
        {
            var bounds = new Rect(rm.Bounds.X - 1, rm.Bounds.Y - 1, rm.Bounds.MaxX + 1, rm.Bounds.MaxY + 1);

            int x = bounds.X, y = bounds.Y;
            for (; y <= bounds.MaxY + GridSize; y += GridSize)
            {
                for (x = bounds.X; x <= bounds.MaxX + GridSize; x += GridSize)
                {
                    if (HitTest(x, y, bounds))
                        return true;
                }
            }
            return false;
        }
    }
}