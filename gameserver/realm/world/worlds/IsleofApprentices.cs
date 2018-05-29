namespace LoESoft.GameServer.realm.world
{
    public class IsleofApprentices : World, IDungeon
    {
        public IsleofApprentices()
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

        protected override void Init() => LoadMap("IsleofApprentices", MapType.Json);
    }
}