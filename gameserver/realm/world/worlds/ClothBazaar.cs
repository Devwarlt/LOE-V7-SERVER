namespace LoESoft.GameServer.realm.world
{
    public class ClothBazaar : World
    {
        public ClothBazaar()
        {
            Id = (int)WorldID.MARKET;
            Name = "Cloth Bazaar";
            ClientWorldName = "nexus.Cloth_Bazaar";
            Background = 2;
            AllowTeleport = false;
            Difficulty = 0;
            SafePlace = true;
        }

        protected override void Init() => LoadMap("bazzar", MapType.Wmap);
    }
}