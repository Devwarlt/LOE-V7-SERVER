namespace LoESoft.GameServer.realm.entity.player
{
    public partial class Player
    {
        public void HandleGround(RealmTime time)
        {
            if (time.TotalElapsedMs - b <= 100)
                return;

            b = time.TotalElapsedMs;
        }
    }
}