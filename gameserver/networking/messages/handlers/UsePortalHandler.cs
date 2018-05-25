#region

using LoESoft.Core.config;
using System;
using System.Globalization;
using LoESoft.GameServer.networking.incoming;
using LoESoft.GameServer.networking.outgoing;
using LoESoft.GameServer.realm;
using LoESoft.GameServer.realm.entity;
using LoESoft.GameServer.realm.world;
using FAILURE = LoESoft.GameServer.networking.outgoing.FAILURE;
using LoESoft.GameServer.realm.entity.player;
using System.Collections.Generic;

#endregion

namespace LoESoft.GameServer.networking.handlers
{
    internal class UsePortalHandler : MessageHandlers<USEPORTAL>
    {
        public override MessageID ID => MessageID.USEPORTAL;

        protected override void HandleMessage(Client client, USEPORTAL message) => Manager.Logic.AddPendingAction(t => Handle(client, client.Player, message), PendingPriority.Networking);

        private readonly List<string> Blacklist = new List<string>();

        private void Handle(Client client, Player player, USEPORTAL message)
        {
            if (player?.Owner == null)
                return;

            Portal portal = player.Owner.GetEntity(message.ObjectId) as Portal;

            if (portal == null)
                return;

            if (!portal.Usable)
            {
                player.SendError("{\"key\":\"server.realm_full\"}");
                return;
            }

            World world = portal.WorldInstance;

            if (world == null)
            {
                bool setWorldInstance = true;

                PortalDesc desc = portal.ObjectDesc;

                if (desc == null)
                {
                    client.SendMessage(new FAILURE
                    {
                        ErrorId = (int)FailureIDs.DEFAULT,
                        ErrorDescription = "Portal not found!"
                    });
                }
                else
                {
                    if (Blacklist.Contains(desc.DungeonName))
                    {
                        player.SendHelp($"'{desc.DungeonName}' is under maintenance and disabled to access until further notice from LoESoft Games.");
                        return;
                    }

                    Type worldType =
                        Type.GetType("LoESoft.GameServer.realm.world." + desc.DungeonName.Replace(" ", string.Empty).Replace("'", string.Empty));
                    if (worldType != null)
                    {
                        try
                        {
                            world = Manager.AddWorld((World)Activator.CreateInstance(worldType,
                            System.Reflection.BindingFlags.CreateInstance, null, null,
                            CultureInfo.InvariantCulture, null));
                        }
                        catch
                        {
                            player.SendError($"Dungeon instance \"{desc.DungeonName}\" isn't declared yet and under maintenance until further notice.");
                        }
                    }
                    else
                        player.SendHelp($"Dungeon instance \"{desc.DungeonName}\" isn't declared yet and under maintenance until further notice.");
                }
                if (setWorldInstance)
                    portal.WorldInstance = world;
            }

            if (world != null)
            {
                if (world.IsFull)
                {
                    player.SendError("{\"key\":\"server.dungeon_full\"}");
                    return;
                }

                if (GameServer.Manager.LastWorld.ContainsKey(player.AccountId))
                {
                    GameServer.Manager.LastWorld.TryRemove(player.AccountId, out World dummy);
                }
                if (player.Owner is Isle_of_Apprentices)
                    GameServer.Manager.LastWorld.TryAdd(player.AccountId, player.Owner);

                client?.Reconnect(new RECONNECT
                {
                    Host = "",
                    Port = Settings.GAMESERVER.PORT,
                    GameId = world.Id,
                    Name = string.IsNullOrEmpty(world.Name) ? world.ClientWorldName : world.Name,
                    Key = world.PortalKey,
                });
            }
        }
    }
}