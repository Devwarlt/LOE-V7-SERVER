#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Ionic.Zlib;
using Newtonsoft.Json;
using common;

#endregion

namespace realm.engine
{
    public class Json2Wmap
    {
        public static byte[] Convert(EmbeddedData data, string json)
        {
            var obj = JsonConvert.DeserializeObject<json_dat>(json);
            var dat = ZlibStream.UncompressBuffer(obj.data);

            Dictionary<short, RealmTile> tileDict = new Dictionary<short, RealmTile>();
            for (int i = 0; i < obj.dict.Length; i++)
            {
                var o = obj.dict[i];
                tileDict[(short)i] = new RealmTile()
                {
                    TileId = o.ground == null ? (ushort)0xff : data.IdToTileType[o.ground],
                    TileObj = o.objs == null ? null : o.objs[0].id,
                    Name = o.objs == null ? "" : o.objs[0].name ?? "",
                    Terrain = RealmTerrainType.None,
                    Region = o.regions == null ? RealmTileRegion.None : (RealmTileRegion)Enum.Parse(typeof(RealmTileRegion), o.regions[0].id.Replace(' ', '_'))
                };
            }

            var tiles = new RealmTile[obj.width, obj.height];
            ushort objType;
            //creates a new case insensitive dictionary based on the XmlDatas
            Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(
                data.IdToObjectType,
                StringComparer.OrdinalIgnoreCase);
            using (NReader rdr = new NReader(new MemoryStream(dat)))
                for (int y = 0; y < obj.height; y++)
                {
                    for (int x = 0; x < obj.width; x++)
                    {
                        tiles[x, y] = tileDict[rdr.ReadInt16()];
                        if (tiles[x, y].TileId.ToString().Length == 2)
                        {
                            File.AppendAllText("Tiles.txt", tiles[x, y].TileId.ToString() + "  ");
                        }
                        else if (String.IsNullOrEmpty(tiles[x, y].TileId.ToString()))
                        {
                            File.AppendAllText("Tiles.txt", "   ");
                        }
                        else
                        {
                            File.AppendAllText("Tiles.txt", tiles[x, y].TileId.ToString() + " ");
                        }
                        if (tiles[x, y].TileObj == null)
                        {
                            File.AppendAllText("Objects.txt", "     ");
                        }
                        else
                        {
                            if (!icdatas.TryGetValue(tiles[x, y].TileObj, out objType) ||
                                !data.ObjectDescs.ContainsKey(objType))
                            {
                            }
                            if (objType.ToString().Length == 3)
                                File.AppendAllText("Objects.txt", objType.ToString() + "  ");
                            else
                                File.AppendAllText("Objects.txt", objType.ToString() + " ");
                        }
                    }
                    File.AppendAllText("Objects.txt", Environment.NewLine);
                    File.AppendAllText("Tiles.txt", Environment.NewLine);
                }
            return WorldMapExporter.Export(tiles);
        }

        public static void Convert(EmbeddedData data, string from, string to)
        {
            byte[] buffer = Convert(data, File.ReadAllText(from));
            File.WriteAllBytes(to, buffer);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct json_dat
        {
            public byte[] data;
            public int width;
            public int height;
            public loc[] dict;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct loc
        {
            public string ground;
            public obj[] objs;
            public obj[] regions;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct obj
        {
            public string name;
            public string id;
        }
    }
}