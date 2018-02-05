#region

using gameserver.networking;
using gameserver.networking.error;
using gameserver.networking.outgoing;
using gameserver.realm.entity;
using gameserver.realm.entity.player;
using System.Collections.Generic;
using System.Threading.Tasks;
using static gameserver.networking.Client;

#endregion

namespace gameserver.realm.world
{
    public class Nexus : World
    {
        private bool validate = true;

        public const string WINTER_RESOURCE = "nexus_winter";
        public const string SUMMER_RESOURCE = "nexus_summer";
        public const string LoE_RESOURCE = "LoE-test";

        public Nexus()
        {
            Id = NEXUS_ID;
            Name = "Nexus";
            ClientWorldName = "server.nexus";
            Background = 2;
            AllowTeleport = false;
            Difficulty = -1;
            Dungeon = false;
            SafePlace = true;
        }

        protected override void Init()
        {
            LoadMap(LoE_RESOURCE, MapType.Json);
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time);

            UpdatePortals();

            if (validate)
                Overseer();
        }

        private async void Overseer()
        {
            validate = false;
            foreach (KeyValuePair<int, World> w in Manager.Worlds)
                foreach (KeyValuePair<int, World> x in Manager.Worlds)
                    foreach (KeyValuePair<int, Player> y in w.Value.Players)
                        foreach (KeyValuePair<int, Player> z in x.Value.Players)
                            if (y.Value.AccountId == z.Value.AccountId && y.Value != z.Value)
                            {
                                try
                                {
                                    if (y.Value.client != null)
                                    {
                                        y.Value.client.SendMessage(new FAILURE
                                        {
                                            ErrorId = (int)FailureIDs.JSON_DIALOG,
                                            ErrorDescription =
                                                JSONErrorIDHandler.
                                                    FormatedJSONError(
                                                        errorID: ErrorIDs.LOST_CONNECTION,
                                                        labels: new[] { "{CLIENT_NAME}" },
                                                        arguments: new[] { y.Value.client.Account.Name }
                                                    )
                                        });
                                        y.Value.client.Disconnect(DisconnectReason.DUPER_DISCONNECT);
                                    }
                                }
                                catch
                                {
                                    Log.Write(nameof(Nexus), "Client is null.");
                                    return;
                                }
                                try
                                {
                                    if (z.Value.client != null)
                                    {
                                        z.Value.client.SendMessage(new FAILURE
                                        {
                                            ErrorId = (int)FailureIDs.JSON_DIALOG,
                                            ErrorDescription =
                                                JSONErrorIDHandler.
                                                    FormatedJSONError(
                                                        errorID: ErrorIDs.LOST_CONNECTION,
                                                        labels: new[] { "{CLIENT_NAME}" },
                                                        arguments: new[] { z.Value.client.Account.Name }
                                                    )
                                        });
                                        z.Value.client.Disconnect(DisconnectReason.DUPER_DISCONNECT);
                                    }
                                }
                                catch
                                {
                                    Log.Write("Client is null.");
                                    return;
                                }
                            }
            await Task.Delay(500);
            validate = true;
        }

        private void UpdatePortals()
        {
            foreach (var i in Manager.Monitor.portals)
            {
                foreach (var j in RealmManager.CurrentRealmNames)
                {
                    if (i.Value.Name.StartsWith(j))
                    {
                        if (i.Value.Name == j) (i.Value as Portal).PortalName = i.Value.Name;
                        i.Value.Name = j + " (" + i.Key.Players.Count + "/" + RealmManager.MAX_REALM_PLAYERS + ")";
                        i.Value.UpdateCount++;
                        break;
                    }
                }
            }
        }
    }
}