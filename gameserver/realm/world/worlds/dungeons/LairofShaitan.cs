namespace gameserver.realm.world
{
    public class LairofShaitan : World
    {
        public LairofShaitan()
        {
            Name = "Lair of Shaitan";
            ClientWorldName = "dungeons.Lair_of_Shaitan";
            Background = 0;
            AllowTeleport = true;
        }

        protected override void Init() => LoadMap("shaitansmap", MapType.Wmap);
    }
}