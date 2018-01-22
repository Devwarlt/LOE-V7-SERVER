#region

using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using gameserver.realm.terrain;

#endregion

namespace gameserver.realm.mapsetpiece
{
    internal class SetPieces
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SetPieces));

        private static readonly List<Tuple<MapSetPiece, int, int, bool, DayOfWeek, WmapTerrain[]>> setPieces = new List<Tuple<MapSetPiece, int, int, bool, DayOfWeek, WmapTerrain[]>>
        {
            SetPiece(piece: new Building(), min: 80, max: 100, terrains: new WmapTerrain[3] { WmapTerrain.LowForest, WmapTerrain.LowPlains, WmapTerrain.MidForest }),
            SetPiece(piece: new Graveyard(), min: 5, max: 10, terrains: new WmapTerrain[2] {WmapTerrain.LowSand, WmapTerrain.LowPlains }),
            SetPiece(piece: new Grove(), min: 17, max: 25, terrains: new WmapTerrain[2] { WmapTerrain.MidForest, WmapTerrain.MidPlains }),
            SetPiece(piece: new LichyTemple(), min: 4, max: 7, terrains: new WmapTerrain[2] { WmapTerrain.MidForest, WmapTerrain.MidPlains }),
            SetPiece(piece: new Castle(), min: 4, max: 7, terrains: new WmapTerrain[2] { WmapTerrain.HighForest, WmapTerrain.HighPlains }),
            SetPiece(piece: new Tower(), min: 8, max: 15, terrains: new WmapTerrain[2] { WmapTerrain.HighForest, WmapTerrain.HighPlains }),
            SetPiece(piece: new TempleA(), min: 10, max: 20, terrains: new WmapTerrain[2] { WmapTerrain.MidForest, WmapTerrain.MidPlains }),
            SetPiece(piece: new TempleB(), min: 10, max: 20, terrains: new WmapTerrain[2] { WmapTerrain.MidForest, WmapTerrain.MidPlains }),
            SetPiece(piece: new Oasis(), min: 0, max: 5, terrains: new WmapTerrain[2] { WmapTerrain.LowSand, WmapTerrain.MidSand }),
            SetPiece(piece: new Pyre(), min: 0, max: 5, terrains: new WmapTerrain[2] { WmapTerrain.MidSand, WmapTerrain.HighSand }),
            SetPiece(piece: new LavaFissure(), min: 3, max: 5, terrains: new WmapTerrain[1] { WmapTerrain.Mountains }),
            //SetPiece(piece: new Event(), min: 1, max: 1, terrains: new WmapTerrain[indexes] { }, isEvent: true, weekDay: DayOfWeek.Friday),
        };

        private static Tuple<MapSetPiece, int, int, bool, DayOfWeek, WmapTerrain[]> SetPiece(MapSetPiece piece, int min, int max, bool isEvent = false, DayOfWeek weekDay = DayOfWeek.Monday, params WmapTerrain[] terrains)
        {
            return Tuple.Create(piece, min, max, isEvent, weekDay, terrains);
        }

        public static int[,] rotateCW(int[,] mat)
        {
            int M = mat.GetLength(0);
            int N = mat.GetLength(1);
            int[,] ret = new int[N, M];
            for (int r = 0; r < M; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    ret[c, M - 1 - r] = mat[r, c];
                }
            }
            return ret;
        }

        public static int[,] reflectVert(int[,] mat)
        {
            int M = mat.GetLength(0);
            int N = mat.GetLength(1);
            int[,] ret = new int[M, N];
            for (int x = 0; x < M; x++)
                for (int y = 0; y < N; y++)
                    ret[x, N - y - 1] = mat[x, y];
            return ret;
        }

        public static int[,] reflectHori(int[,] mat)
        {
            int M = mat.GetLength(0);
            int N = mat.GetLength(1);
            int[,] ret = new int[M, N];
            for (int x = 0; x < M; x++)
                for (int y = 0; y < N; y++)
                    ret[M - x - 1, y] = mat[x, y];
            return ret;
        }

        //private static int DistSqr(IntPoint a, IntPoint b)
        //{
        //    return (a.X - b.X)*(a.X - b.X) + (a.Y - b.Y)*(a.Y - b.Y);
        //}

        public static void ApplySetPieces(World world)
        {
            log.InfoFormat("Applying set pieces to world {0}({1}).", world.Id, world.Name);

            Wmap map = world.Map;
            int w = map.Width, h = map.Height;
            DateTime today = DateTime.Now; // = DateTime.Today;

            Random rand = new Random();
            HashSet<Rect> rects = new HashSet<Rect>();
            foreach (Tuple<MapSetPiece, int, int, bool, DayOfWeek, WmapTerrain[]> dat in setPieces)
            {
                int size = dat.Item1.Size;
                int count = rand.Next(dat.Item2, dat.Item3);
                if (!dat.Item4 || (dat.Item4 && today.DayOfWeek == dat.Item5))
                {
                    for (int i = 0; i < count; i++)
                    {
                        IntPoint pt = new IntPoint();
                        Rect rect;

                        int max = 50;
                        do
                        {
                            pt.X = rand.Next(0, w);
                            pt.Y = rand.Next(0, h);
                            rect = new Rect { x = pt.X, y = pt.Y, w = size, h = size };
                            max--;
                        } while ((Array.IndexOf(dat.Item6, map[pt.X, pt.Y].Terrain) == -1 ||
                                  rects.Any(_ => Rect.Intersects(rect, _))) &&
                                 max > 0);
                        if (max <= 0) continue;

                        dat.Item1.RenderSetPiece(world, pt);
                        rects.Add(rect);
                    }
                }
            }

            log.Info("Set pieces applied.");
        }

        private struct Rect
        {
            public int h;
            public int w;
            public int x;
            public int y;

            public static bool Intersects(Rect r1, Rect r2)
            {
                return !(r2.x > r1.x + r1.w ||
                         r2.x + r2.w < r1.x ||
                         r2.y > r1.y + r1.h ||
                         r2.y + r2.h < r1.y);
            }
        }
    }
}