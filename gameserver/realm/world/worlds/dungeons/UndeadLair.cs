namespace LoESoft.GameServer.realm.world
{
    public class UndeadLair : World
    {
        public UndeadLair()
        {
            Name = "Undead Lair";
            ClientWorldName = "dungeons.Undead_Lair";
            Dungeon = true;
            Background = 0;
            AllowTeleport = true;
        }

        protected override void Init() => LoadMap("UDL1", MapType.Wmap);
    }
}