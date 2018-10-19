#region

using LoESoft.GameServer.networking.outgoing;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace LoESoft.GameServer.realm.entity.player
{
    public partial class Player
    {
        private IEnumerable<Entity> GetNewEntities()
        {
            var newEntities = new List<Entity>();
            var world = GameServer.Manager.GetWorld(Owner.Id);

            Owner.Players.Where(player => clientEntities.Add(player.Value))
            .Select(_ =>
            {
                newEntities.Add(_.Value);
                return _;
            });
            Owner.PlayersCollision.HitTest(X, Y, SIGHTRADIUS).OfType<Decoy>()
            .Where(decoy => clientEntities.Add(decoy))
            .Select(_ =>
            {
                newEntities.Add(_);
                return _;
            });
            Owner.EnemiesCollision.HitTest(X, Y, SIGHTRADIUS)
            .Where(enemy => !(MathsUtils.DistSqr(enemy.X, enemy.Y, X, Y) <= SIGHTRADIUS * SIGHTRADIUS))
            .Select(_ =>
            {
                if (_ is Container)
                {
                    var owner = (_ as Container).BagOwners?.Length == 1 ? (_ as Container).BagOwners[0] : null;

                    if (owner == null)
                        if (owner == AccountId && (LootDropBoost || LootTierBoost) && (_.ObjectType != 0x500 ||
                        _.ObjectType != 0x506))
                            (_ as Container).BoostedBag = true;
                }

                if (visibleTiles.ContainsKey(new IntPoint((int)_.X, (int)_.Y)))
                    if (clientEntities.Add(_))
                        newEntities.Add(_);

                return _;
            });

            if (Quest != null && clientEntities.Add(Quest) && (Quest as Enemy).HP >= 0)
                newEntities.Add(Quest);

            return newEntities;
        }

        private IEnumerable<int> GetRemovedEntities()
        {
            var removedEntities = new List<int>();

            clientEntities.Where(entity => !(entity is Player && entity.Owner != null) &&
            ((MathsUtils.DistSqr(entity.X, entity.Y, X, Y) > SIGHTRADIUS * SIGHTRADIUS &&
            !(entity is GameObject && (entity as GameObject).Static) && entity != Quest) ||
            entity.Owner == null))
            .Select(clientEntity =>
            {
                removedEntities.Add(clientEntity.Id);

                return clientEntities;
            }).ToList();

            return removedEntities;
        }

        private IEnumerable<ObjectDef> GetNewStatics(int xBase, int yBase)
        {
            var world = GameServer.Manager.GetWorld(Owner.Id);
            var ret = new List<ObjectDef>();

            blocksight = world.Dungeon ? Sight.RayCast(this, SIGHTRADIUS).ToList() : Sight.GetSquare(SIGHTRADIUS);
            blocksight.Where(pos =>
            {
                var x = pos.X + xBase;
                var y = pos.Y + yBase;

                return !(x < 0 || x >= mapWidth || y < 0 || y >= mapHeight);
            }).Select(point =>
            {
                var x = point.X + xBase;
                var y = point.Y + yBase;
                var t = Owner.Map[x, y];
                var d = t.ToDef(x, y);
                var c = GameServer.Manager.GameData.ObjectDescs[t.ObjType].Class;

                if (c == "ConnectedWall" || c == "CaveWall")
                    if (d.Stats.Stats.Count(s => s.Key == StatsType.CONNECT_STAT && s.Value != null) == 0)
                        d.Stats.Stats = new KeyValuePair<StatsType, object>[] {
                            new KeyValuePair<StatsType, object>(StatsType.CONNECT_STAT,
                            (int)ConnectionComputer.Compute((xx, yy) => Owner.Map[x + xx, y + yy].ObjType == t.ObjType).Bits)
                        };

                ret.Add(d);

                return point;
            });

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

        public void HandleUpdate(RealmTime time)
        {
            var world = GameServer.Manager.GetWorld(Owner.Id);
            var sendEntities = new HashSet<Entity>(GetNewEntities());
            var tilesUpdate = new List<UPDATE.TileData>(APPOX_AREA_OF_SIGHT);

            mapWidth = Owner.Map.Width;
            mapHeight = Owner.Map.Height;
            blocksight = world.Dungeon ? Sight.RayCast(this, SIGHTRADIUS).ToList() : Sight.GetSquare(SIGHTRADIUS);
            blocksight.Where(pos =>
            {
                var x = pos.X + (int)X;
                var y = pos.Y + (int)Y;

                return !(x < 0 || x >= Owner.Map.Width || y < 0 || y >= Owner.Map.Height ||
                tiles[x, y] >= Owner.Map[x, y].UpdateCount);
            }).Select(point =>
            {
                var x = point.X + (int)X;
                var y = point.Y + (int)Y;
                var t = Owner.Map[x, y];

                if (!visibleTiles.ContainsKey(new IntPoint(x, y)))
                    visibleTiles[new IntPoint(x, y)] = true;

                tilesUpdate.Add(new UPDATE.TileData()
                {
                    X = (short)x,
                    Y = (short)y,
                    Tile = t.TileId
                });

                tiles[x, y] = t.UpdateCount;

                return point;
            });

            var dropEntities = GetRemovedEntities().Distinct().ToArray();

            clientEntities.RemoveWhere(_ => Array.IndexOf(dropEntities, _.Id) != -1);

            var toRemove = lastUpdate.Keys.Where(i => !clientEntities.Contains(i))
            .Select(entity => lastUpdate.TryRemove(entity, out int val)).ToList();

            sendEntities.Select(sendEntity => sendEntity.UpdateCount);

            var newStatics = GetNewStatics((int)X, (int)Y);
            var removeStatics = GetRemovedStatics((int)X, (int)Y);
            var removedIds = new List<int>();

            if (!world.Dungeon)
                removeStatics.Select(removeStatic =>
                {
                    removedIds.Add(Owner.Map[removeStatic.X, removeStatic.Y].ObjId);
                    clientStatic.Remove(removeStatic);

                    return removeStatic;
                });

            if (sendEntities.Count > 0 || tilesUpdate.Count > 0 || dropEntities.Length > 0 || newStatics.ToArray().Length > 0 || removedIds.Count > 0)
            {
                Client.SendMessage(new UPDATE()
                {
                    Tiles = tilesUpdate.ToArray(),
                    NewObjects = sendEntities.Select(_ => _.ToDefinition()).Concat(newStatics.ToArray()).ToArray(),
                    RemovedObjectIds = dropEntities.Concat(removedIds).ToArray()
                });

                AwaitUpdateAck(time.TotalElapsedMs);
            }
        }

        private void HandleNewTick(RealmTime time)
        {
            var sendEntities = new List<Entity>();

            try
            {
                clientEntities.Where(i => i?.UpdateCount > lastUpdate[i] && i != null).Select(clientEntity =>
                {
                    sendEntities.Add(clientEntity);
                    lastUpdate[clientEntity] = clientEntity.UpdateCount;

                    return clientEntity;
                });
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