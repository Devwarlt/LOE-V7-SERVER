#region

using System.Collections.Generic;
using LoESoft.GameServer.realm.entity.player;
using static LoESoft.GameServer.networking.Client;

#endregion

namespace LoESoft.GameServer.realm.world
{
    public class Test : World, IDungeon
    {
        public string js = null;

        public Test()
        {
            Id = (int)WorldID.TEST_ID;
            Name = "Test";
            Background = 0;
            Dungeon = true;
        }

        public void LoadJson(string json)
        {
            js = json;
            LoadMap(json);
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time);

            foreach (KeyValuePair<int, Player> i in Players)
            {
                if (i.Value.client.Account.AccountType != (int)LoESoft.Core.config.AccountType.LOESOFT_ACCOUNT || !i.Value.client.Account.Admin)
                {
                    i.Value.SendError(string.Format("[Staff Member: {0}] You cannot access Test world with account type {1}.", i.Value.client.Account.Admin, nameof(i.Value.client.Account.AccountType)));
                    Program.Manager.TryDisconnect(i.Value.client, DisconnectReason.ACCESS_DENIED);
                }
            }
        }

        protected override void Init() { }
    }
}