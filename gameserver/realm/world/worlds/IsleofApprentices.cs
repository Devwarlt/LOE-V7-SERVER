namespace LoESoft.GameServer.realm.world
{
    public class IsleofApprentices : World, IDungeon
    {
        public IsleofApprentices()
        {
            Id = (int)TownID.ISLE_OF_APPRENTICES_ID;
            Name = "Isle of Apprentices";
            ClientWorldName = "Isle of Apprentices";
            Background = 0;
            AllowTeleport = false;
            Difficulty = -1;
            Dungeon = false;
            SafePlace = true;
        }

        protected override void Init() => LoadMap("IsleofApprenticesBETAv1", MapType.Json);
    }
}