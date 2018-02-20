#region

using gameserver.networking;

#endregion

namespace gameserver.realm.world
{
    public class Tutorial : World
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