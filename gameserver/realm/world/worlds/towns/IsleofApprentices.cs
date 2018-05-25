namespace LoESoft.GameServer.realm.world
{
    public class Isle_of_Apprentices : World, IDungeon
    {
        public Isle_of_Apprentices()
        {
            Id = (int)TownID.ISLE_OF_APPRENTICES;
            ClientWorldName = "server.isle_of_apprentices";
            Background = 0;
            AllowTeleport = false;
            Difficulty = -1;
            Dungeon = false;
            SafePlace = true;
        }
    }
}