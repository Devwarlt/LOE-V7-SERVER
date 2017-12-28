using DungeonGenerator.Dungeon;

namespace DungeonGenerator.Templates.Difficult_1.Pirate_Cave {
	internal class Overlay : MapRender {
		public override void Rasterize() {
			var wall = new DungeonTile {
				TileType = PirateCaveTemplate.Composite,
				Object = new DungeonObject {
					ObjectType = PirateCaveTemplate.CaveWall
				}
			};
			var water = new DungeonTile {
				TileType = PirateCaveTemplate.ShallowWater
			};
			var space = new DungeonTile {
				TileType = PirateCaveTemplate.Space
			};

			int w = Rasterizer.Width, h = Rasterizer.Height;
			var buf = Rasterizer.Bitmap;
			for (int x = 0; x < w; x++)
				for (int y = 0; y < h; y++) {
					if (buf[x, y].TileType != PirateCaveTemplate.ShallowWater)
						continue;

					bool notWall = false;
					if (x == 0 || y == 0 || x + 1 == w || y + 1 == h)
						notWall = false;
					else if (buf[x + 1, y].TileType == PirateCaveTemplate.BrownLines ||
					         buf[x - 1, y].TileType == PirateCaveTemplate.BrownLines ||
					         buf[x, y + 1].TileType == PirateCaveTemplate.BrownLines ||
					         buf[x, y - 1].TileType == PirateCaveTemplate.BrownLines) {
						notWall = true;
					}
					if (!notWall)
						buf[x, y] = wall;
				}

			var tmp = (DungeonTile[,])buf.Clone();
			for (int x = 0; x < w; x++)
				for (int y = 0; y < h; y++) {
					if (buf[x, y].TileType != PirateCaveTemplate.Composite)
						continue;

					bool nearWater = false;
					if (x == 0 || y == 0 || x + 1 == w || y + 1 == h)
						nearWater = false;
					else if (tmp[x + 1, y].TileType == PirateCaveTemplate.ShallowWater ||
					         tmp[x - 1, y].TileType == PirateCaveTemplate.ShallowWater ||
					         tmp[x, y + 1].TileType == PirateCaveTemplate.ShallowWater ||
					         tmp[x, y - 1].TileType == PirateCaveTemplate.ShallowWater) {
						nearWater = true;
					}
					if (nearWater && Rand.NextDouble() > 0.4)
						buf[x, y] = water;
				}

			tmp = (DungeonTile[,])buf.Clone();
			for (int x = 0; x < w; x++)
				for (int y = 0; y < h; y++) {
					if (buf[x, y].TileType != PirateCaveTemplate.Composite)
						continue;

					bool allWall = false;
					if (x == 0 || y == 0 || x + 1 == w || y + 1 == h)
						allWall = true;
					else {
						allWall = true;
						for (int dx = -1; dx <= 1 && allWall; dx++)
							for (int dy = -1; dy <= 1 && allWall; dy++) {
								if (tmp[x + dx, y + dy].TileType != PirateCaveTemplate.Composite) {
									allWall = false;
									break;
								}
							}
					}
					if (allWall)
						buf[x, y] = space;
				}
		}
	}
}