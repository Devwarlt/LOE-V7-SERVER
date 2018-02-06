#region

using System.Collections.Generic;
using gameserver.realm.entity.player;
using static gameserver.networking.Client;

#endregion

namespace gameserver.realm.world
{
    public class Test : World
    {
        public string js = null;

        public Test()
        {
            Id = TEST_ID;
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
                if (i.Value.client.Account.AccountType != (int)common.config.accountType.LOESOFT_ACCOUNT || !i.Value.client.Account.Admin)
                {
                    i.Value.SendError(string.Format("[Staff Member: {0}] You cannot access Test world with account type {1}.", i.Value.client.Account.Admin, nameof(i.Value.client.Account.AccountType)));
                    Manager.TryDisconnect(i.Value.client, DisconnectReason.ACCESS_DENIED);
                }
            }
        }

        protected override void Init() { }
    }
}