using LoESoft.GameServer.realm.entity;
using LoESoft.GameServer.realm.entity.player;
using LoESoft.GameServer.realm.mapsetpiece;
using System.Collections.Generic;

namespace LoESoft.GameServer.realm
{
    internal partial class Realm
    {
        public readonly double RealmEventProbability = .25; // 25%

        public readonly List<RealmEvent> RealmEventCache = new List<RealmEvent>
        {
            new RealmEvent("Skull Shrine",  new SkullShrine()),
            new RealmEvent("Pentaract",  new Pentaract()),
            new RealmEvent("Grand Sphinx",  new Sphinx()),
            new RealmEvent("Cube God",  new CubeGod()),
            new RealmEvent("Frog King",  new FrogKing())
        };

        public void HandleRealmEvent(Enemy enemy, Player killer)
        {
            if (enemy.ObjectDesc != null && enemy.ObjectDesc.Quest)
            {
                TauntData? dat = null;

                foreach (var i in criticalEnemies)
                    if ((enemy.ObjectDesc.DisplayId ?? enemy.ObjectDesc.ObjectId) == i.Item1)
                    {
                        dat = i.Item2;
                        break;
                    }

                if (dat == null)
                    return;

                if (dat.Value.killed != null)
                {
                    string[] arr = dat.Value.killed;
                    string msg = arr[rand.Next(0, arr.Length)];

                    while (killer == null && msg.Contains("{PLAYER}"))
                        msg = arr[rand.Next(0, arr.Length)];

                    msg = msg.Replace("{PLAYER}", killer.Name);

                    BroadcastMsg(msg);
                }

                if (rand.NextDouble() < RealmEventProbability)
                {
                    RealmEvent evt = RealmEventCache[rand.Next(0, RealmEventCache.Count)];

                    if (Program.Manager.GameData.ObjectDescs[Program.Manager.GameData.IdToObjectType[evt.Name]].PerRealmMax == 1)
                        RealmEventCache.Remove(evt);

                    SpawnEvent(evt.Name, evt.MapSetPiece);

                    dat = null;

                    foreach (var i in criticalEnemies)
                        if (evt.Name == i.Item1)
                        {
                            dat = i.Item2;
                            break;
                        }

                    if (dat == null)
                        return;

                    if (dat.Value.spawn != null)
                    {
                        string[] arr = dat.Value.spawn;
                        string msg = arr[rand.Next(0, arr.Length)];

                        BroadcastMsg(msg);
                    }
                }
            }
        }

        public class RealmEvent
        {
            public string Name { get; set; }
            public MapSetPiece MapSetPiece { get; set; }

            public RealmEvent(
                string Name,
                MapSetPiece MapSetPiece
                )
            {
                this.Name = Name;
                this.MapSetPiece = MapSetPiece;
            }
        }
    }
}