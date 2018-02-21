#region

using LoESoft.GameServer.networking;

#endregion

namespace LoESoft.GameServer.realm.world
{
    public class Tutorial : World, IDungeon
    {
        private readonly bool isLimbo;

        public Tutorial(bool isLimbo)
        {
            Id = (int)WorldID.TUT_ID;
            Name = "Tutorial";
            ClientWorldName = "server.tutorial";
            Background = 0;
            this.isLimbo = isLimbo;
        }

        protected override void Init()
        {
            if (!(IsLimbo = isLimbo))
                LoadMap("tutorial", MapType.Wmap);
        }

        public override World GetInstance(Client psr) => Program.Manager.AddWorld(new Tutorial(false));
    }
}