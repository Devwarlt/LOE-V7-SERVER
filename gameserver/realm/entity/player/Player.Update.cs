#region

using System;
using System.Collections.Generic;
using System.Linq;
using LoESoft.Core.models;
using LoESoft.GameServer.networking.outgoing;
using LoESoft.GameServer.realm.terrain;

#endregion

namespace LoESoft.GameServer.realm.entity.player
{
    public partial class Player
    {
        private IEnumerable<Entity> GetNewEntities()
        {
            World world = GameServer.Manager.GetWorld(Owner.Id);

            foreach (var i in Owner.Players.Where(i => clientEntities.Add(i.Value)))
                yield return i.Value;

            foreach (var i in Owner.PlayersCollision.HitTest(X, Y, SIGHTRADIUS).OfType<Decoy>().Where(i => clientEntities.Add(i)))
                yield return i;

            foreach (var i in Owner.EnemiesCollision.HitTest(X, Y, SIGHTRADIUS))
            {
                if (i is Container)
                {
                    var owner = (i as Container).BagOwners?.Length == 1 ? (i as Container).BagOwners[0] : null;
                    if (owner != null && owner != AccountId) continue;

                    if (owner == AccountId)
                        if ((LootDropBoost || LootTierBoost) && (i.ObjectType != 0x500 || i.ObjectType != 0x506))
                            (i as Container).BoostedBag = true; //boosted bag

                }
                if (!(MathsUtils.DistSqr(i.X, i.Y, X, Y) <= SIGHTRADIUS * SIGHTRADIUS)) continue;
                if (visibleTiles.ContainsKey(new IntPoint((int)i.X, (int)i.Y)))
                    if (clientEntities.Add(i))
                        yield return i;
            }
            if (Quest != null && clientEntities.Add(Quest) && (Quest as Enemy).HP >= 0)
                yield return Quest;
        }

        private IEnumerable<int> GetRemovedEntities()
        {
            foreach (var i in clientEntities)
            {
                if (i is Player && i.Owner != null) continue;
                if (MathsUtils.DistSqr(i.X, i.Y, X, Y) > SIGHTRADIUS * SIGHTRADIUS &&
                    !(i is GameObject && (i as GameObject).Static) &&
                    i != Quest)
                    yield return i.Id;
                else if (i.Owner == null)
                    yield return i.Id;
            }
        }

        private IEnumerable<ObjectDef> GetNewStatics(int xBase, int yBase)
        {
            World world = GameServer.Manager.GetWorld(Owner.Id);
            blocksight = (world.Dungeon ? Sight.RayCast(this, 15) : Sight.GetSightCircle(SIGHTRADIUS)).ToList();
            List<ObjectDef> ret = new List<ObjectDef>();
            foreach (IntPoint i in blocksight.ToList())
            {
                var x = i.X + xBase;
                var y = i.Y + yBase;
                if (x < 0 || x >= mapWidth ||
                    y < 0 || y >= mapHeight) continue;

                var tile = Owner.Map[x, y];

                if (tile.ObjId == 0 || tile.ObjType == 0 || !clientStatic.Add(new IntPoint(x, y))) continue;
                var def = tile.ToDef(x, y);
                var cls = GameServer.Manager.GameData.ObjectDescs[tile.ObjType].Class;
                if (cls == "ConnectedWall" || cls == "CaveWall")
                {
                    if (def.Stats.Stats.Count(_ => _.Key == StatsType.CONNECT_STAT && _.Value != null) == 0)
                    {
                        var stats = def.Stats.Stats.ToList();
                        stats.Add(new KeyValuePair<StatsType, object>(StatsType.CONNECT_STAT, (int)ConnectionComputer.Compute((xx, yy) => Owner.Map[x + xx, y + yy].ObjType == tile.ObjType).Bits));
                        def.Stats.Stats = stats.ToArray();
                    }
                }
                ret?.Add(def);
            }
            return ret;
        }

        private IEnumerable<IntPoint> GetRemovedStatics(int xBase, int yBase)
        {
            return from i in clientStatic
                   let dx = i.X - xBase
                   let dy = i.Y - yBase
                   let tile = Owner.Map[i.X, i.Y]
                   where dx * dx + dy * dy > SIGHTRADIUS * SIGHTRADIUS ||
                         tile.ObjType == 0
                   let objId = Owner.Map[i.X, i.Y].ObjId
                   where objId != 0
                   select i;
        }

        public void ExperimentalHandleUpdate()
        {
            WmapTile tile;
            World world = GameServer.Manager.GetWorld(Owner.Id);
            HashSet<Entity> sendEntities = new HashSet<Entity>(GetNewEntities());
            List<UPDATE.TileData> tilesUpdate = new List<UPDATE.TileData>(APPOX_AREA_OF_SIGHT);

            mapWidth = Owner.Map.Width;
            mapHeight = Owner.Map.Height;
            blocksight = world.Dungeon ? Sight.RayCast(this, SIGHTRADIUS).ToList() : Sight.GetSquare(SIGHTRADIUS);

            foreach (IntPoint i in blocksight.ToList())
            {
                int x = i.X + (int)X;
                int y = i.Y + (int)Y;

                if (x < 0 || x >= Owner.Map.Width || y < 0 || y >= Owner.Map.Height || tiles[x, y] >= (tile = Owner.Map[x, y]).UpdateCount) continue;

                if (!visibleTiles.ContainsKey(new IntPoint(x, y)))
                    visibleTiles[new IntPoint(x, y)] = true;

                tilesUpdate.Add(new UPDATE.TileData
                {
                    X = (short)x,
                    Y = (short)y,
                    Tile = tile.TileId
                });

                tiles[x, y] = tile.UpdateCount;
            }

            int[] dropEntities = GetRemovedEntities().Distinct().ToArray();

            clientEntities.RemoveWhere(_ => Array.IndexOf(dropEntities, _.Id) != -1);

            List<Entity> toRemove = lastUpdate.Keys.Where(i => !clientEntities.Contains(i)).ToList();
            toRemove.ForEach(i => lastUpdate.TryRemove(i, out int val));

            foreach (var i in sendEntities)
                lastUpdate[i] = i.UpdateCount;

            IEnumerable<ObjectDef> newStatics = GetNewStatics((int)X, (int)Y);
            IEnumerable<IntPoint> removeStatics = GetRemovedStatics((int)X, (int)Y);

            List<int> removedIds = new List<int>();

            if (!world.Dungeon)
                foreach (IntPoint i in removeStatics.ToArray())
                {
                    removedIds.Add(Owner.Map[i.X, i.Y].ObjId);
                    clientStatic.Remove(i);
                }

            if (sendEntities.Count > 0 || tilesUpdate.Count > 0 || dropEntities.Length > 0 || newStatics.ToArray().Length > 0 || removedIds.Count > 0)
                Client.SendMessage(new UPDATE()
                {
                    Tiles = tilesUpdate.ToArray(),
                    NewObjects = sendEntities.Select(_ => _.ToDefinition()).Concat(newStatics.ToArray()).ToArray(),
                    RemovedObjectIds = dropEntities.Concat(removedIds).ToArray()
                });
        }

        public void HandleUpdate(RealmTime time)
        {
            Wmap map = Owner.Map;
            WmapTile tile;
            World world = GameServer.Manager.GetWorld(Owner.Id);
            int xBase = (int)X;
            int yBase = (int)Y;
            int sent = 0;
            HashSet<Entity> sendEntities = new HashSet<Entity>(GetNewEntities());
            List<UPDATE.TileData> list = new List<UPDATE.TileData>(APPOX_AREA_OF_SIGHT);

            mapWidth = Owner.Map.Width;
            mapHeight = Owner.Map.Height;
            blocksight = (world.Dungeon ? Sight.RayCast(this, SIGHTRADIUS) : Sight.GetSightCircle(SIGHTRADIUS)).ToList();

            foreach (IntPoint i in blocksight.ToList())
            {
                int x = i.X + xBase;
                int y = i.Y + yBase;

                if (x < 0
                 || x >= mapWidth
                 || y < 0
                 || y >= mapHeight
                 || tiles[x, y] >= (tile = map[x, y]).UpdateCount)
                    continue;

                if (!visibleTiles.ContainsKey(new IntPoint(x, y)))
                    visibleTiles[new IntPoint(x, y)] = true;

                list.Add(new UPDATE.TileData
                {
                    X = (short)x,
                    Y = (short)y,
                    Tile = tile.TileId
                });
                tiles[x, y] = tile.UpdateCount;
                sent++;
            }

            int[] dropEntities = GetRemovedEntities().Distinct().ToArray();
            clientEntities.RemoveWhere(_ => Array.IndexOf(dropEntities, _.Id) != -1);

            List<Entity> toRemove = lastUpdate.Keys.Where(i => !clientEntities.Contains(i)).ToList();
            toRemove.ForEach(i => lastUpdate.TryRemove(i, out int val));

            foreach (var i in sendEntities)
                lastUpdate[i] = i.UpdateCount;

            IEnumerable<ObjectDef> newStatics = GetNewStatics(xBase, yBase);
            IEnumerable<IntPoint> removeStatics = GetRemovedStatics(xBase, yBase);
            List<int> removedIds = new List<int>();

            if (!world.Dungeon)
                foreach (IntPoint i in removeStatics.ToArray())
                {
                    removedIds.Add(Owner.Map[i.X, i.Y].ObjId);
                    clientStatic.Remove(i);
                }

            if (sendEntities.Count <= 0
                && list.Count <= 0
                && dropEntities.Length <= 0
                && newStatics.ToArray().Length <= 0
                && removedIds.Count <= 0)
                return;

            Client.SendMessage(new UPDATE()
            {
                Tiles = list.ToArray(),
                NewObjects = sendEntities.Select(_ => _.ToDefinition()).Concat(newStatics.ToArray()).ToArray(),
                RemovedObjectIds = dropEntities.Concat(removedIds).ToArray()
            });

            UpdatesSend++;
        }

        private void HandleNewTick(RealmTime time)
        {
            List<Entity> sendEntities = new List<Entity>();

            try
            {
                foreach (Entity i in clientEntities.Where(i => i?.UpdateCount > lastUpdate[i]).ToList())
                {
                    sendEntities?.Add(i);
                    lastUpdate[i] = i.UpdateCount;
                }
            }
            catch (Exception) { }

            if (Quest != null &&
                (!lastUpdate.ContainsKey(Quest)
                || Quest.UpdateCount > lastUpdate[Quest]))
            {
                sendEntities.Add(Quest);
                lastUpdate[Quest] = Quest.UpdateCount;
            }

            Client.SendMessage(new NEWTICK()
            {
                TickId = tickId++,
                TickTime = time.ElapsedMsDelta,
                Statuses = sendEntities.Select(_ => _.ExportStats()).ToArray()
            });

            blocksight.Clear();
        }
    }
}
