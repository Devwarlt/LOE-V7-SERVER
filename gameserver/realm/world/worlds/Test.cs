#region

using System.Collections.Generic;
using LoESoft.GameServer.realm.entity.player;
using static LoESoft.GameServer.networking.Client;

#endregion

namespace LoESoft.GameServer.realm.world
{
    public class Test : World, IDungeon
    {
        public string js = null;

        public Test()
        {
            Id = (int)WorldID.TEST_ID;
            Name = "Test";
            Background = 0;
            Dungeon = true;
        }

        public void LoadJson(string json)
        {
            js = json;
            LoadMap(json);
        }
    }
}