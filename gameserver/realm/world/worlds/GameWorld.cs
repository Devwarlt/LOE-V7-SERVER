#region

using System;
using log4net;
using LoESoft.GameServer.realm.entity;
using LoESoft.GameServer.realm.entity.player;
using LoESoft.GameServer.realm.mapsetpiece;

#endregion

namespace LoESoft.GameServer.realm.world
{
    public interface IRealm { }

    internal class GameWorld : World, IDungeon, IRealm
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GameWorld));

        private readonly int mapId;
        private readonly bool oryxPresent;
        private string displayname;

        public GameWorld(int mapId, string name, bool oryxPresent)
        {
            displayname = name;
            Name = name;
            ClientWorldName = name;
            Background = 0;
            Difficulty = -1;
            this.oryxPresent = oryxPresent;
            this.mapId = mapId;
        }

        public Realm Overseer { get; private set; }

        protected override void Init()
        {
            LoadMap("world" + mapId, MapType.Wmap);
            SetPieces.ApplySetPieces(this);
            if (oryxPresent)
                Overseer = new Realm(this);
            else
                Overseer = null;
        }

        public static GameWorld AutoName(int mapId, bool oryxPresent)
        {
            string name = RealmManager.Realms[new Random().Next(RealmManager.Realms.Count)];
            RealmManager.Realms.Remove(name);
            RealmManager.CurrentRealmNames.Add(name);
            return new GameWorld(mapId, name, oryxPresent);
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time);
            if (Overseer != null)
                Overseer.Tick(time);
        }

        public void EnemyKilled(Enemy enemy, Player killer)
        {
            if (Overseer != null)
                Overseer.HandleRealmEvent(enemy, killer);
        }

        public override int EnterWorld(Entity entity)
        {
            int ret = base.EnterWorld(entity);
            if (entity is Player)
                Overseer.OnPlayerEntered(entity as Player);
            return ret;
        }

        public override void Dispose()
        {
            if (Overseer != null)
                Overseer.Dispose();
            base.Dispose();
        }
    }
}