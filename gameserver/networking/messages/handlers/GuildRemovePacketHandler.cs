#region

using common;
using gameserver.networking.incoming;
using gameserver.realm;
using gameserver.realm.entity.player;
using System;
using System.Linq;

#endregion

namespace gameserver.networking.handlers
{
    internal class GuildRemovePacketHandler : MessageHandlers<GUILDREMOVE>
    {
        public override MessageID ID => MessageID.GUILDREMOVE;

        protected override void HandleMessage(Client client, GUILDREMOVE message) => Handle(client, message.Name);

        private void Handle(Client client, string name)
        {
            if (client?.Player == null)
                return;

            Player player = client.Player;

            RealmManager manager = client.Manager;

            if (client.Account.Name.Equals(name))
            {
                manager.Chat.Guild(player, $"{player.Name} has left the guild.", true);

                if (!manager.Database.RemoveFromGuild(client.Account))
                {
                    player.SendError("Guild not found.");
                    return;
                }

                player.Guild = "";
                player.GuildRank = -1;

                client.Account.Flush();

                return;
            }

            int targetAccId = Convert.ToInt32(client.Manager.Database.ResolveId(name));

            if (targetAccId == 0)
            {
                player.SendError("Player not found");
                return;
            }

            Client targetClient = (from newClient in client.Manager.Clients.Values where newClient.Account != null where newClient.Account.AccountId == targetAccId.ToString() select newClient).FirstOrDefault();

            if (targetClient != null)
            {
                if (client.Account.GuildRank >= 20 && client.Account.GuildId == targetClient.Account.GuildId && client.Account.GuildRank > targetClient.Account.GuildRank)
                {
                    Player targetPlayer = targetClient.Player;

                    if (!manager.Database.RemoveFromGuild(targetClient.Account))
                    {
                        player.SendError("Guild not found.");
                        return;
                    }

                    targetPlayer.Guild = "";
                    targetPlayer.GuildRank = -1;

                    client.Account.Flush();

                    manager.Chat.Guild(player, $"{targetPlayer.Name} has been kicked from the guild by {player.Name}.", true);
                    targetPlayer.SendInfo("You have been kicked from the guild.");
                    return;
                }
                else
                {
                    player.SendError("Can't remove member. Insufficient privileges.");
                    return;
                }
            }

            DbAccount targetAccount = manager.Database.GetAccountById(targetAccId.ToString());

            if (client.Account.GuildRank >= 20 && client.Account.GuildId == targetAccount.GuildId && client.Account.GuildRank > targetAccount.GuildRank)
            {
                if (!manager.Database.RemoveFromGuild(targetAccount))
                {
                    player.SendError("Guild not found.");
                    return;
                }

                manager.Chat.Guild(player, $"{targetAccount.Name} has been kicked from the guild by {player.Name}.", true);
                player.SendInfo("You have been kicked from the guild.");
                return;
            }

            player.SendError("Can't remove member. Insufficient privileges.");
        }
    }
}