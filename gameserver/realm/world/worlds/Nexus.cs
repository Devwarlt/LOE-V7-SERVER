#region

using LoESoft.GameServer.realm.entity;

#endregion

namespace LoESoft.GameServer.realm.world
{
    public class Nexus : World, IDungeon
    {
        public Nexus()
        {
            Id = (int)WorldID.ISLE_OF_APPRENTICES;
            Name = "Isle of Apprentices";
            ClientWorldName = "Isle of Apprentices";
            Background = 2;
            AllowTeleport = false;
            Difficulty = -1;
            Dungeon = false;
            SafePlace = true;
        }
    }
}