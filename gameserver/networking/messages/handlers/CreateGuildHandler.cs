#region

using common;
using gameserver.networking.incoming;
using gameserver.networking.outgoing;
using gameserver.realm.entity.player;

#endregion

namespace gameserver.networking.handlers
{
    internal class CreateGuildHandler : MessageHandlers<CREATEGUILD>
    {
        public override MessageID ID => MessageID.CREATEGUILD;

        protected override void HandleMessage(Client client, CREATEGUILD message) => Handle(client, message.Name);

        private void Handle(Client client, string name)
        {
            if (client?.Player == null)
                return;

            Player player = client.Player;

            DbAccount acc = client.Account;

            if (acc.Fame < 1000)
            {
                SendError(client, "Insufficient funds.");
                return;
            }

            if (!acc.NameChosen)
            {
                SendError(client, "Must pick a character name, before creating a guild.");
                return;
            }

            if (acc.GuildId != "-1")
            {
                SendError(client, "Already in a guild");
                return;
            }


            GuildCreateStatus gstatus = Manager.Database.CreateGuild(name, out DbGuild guild);

            if (gstatus != GuildCreateStatus.OK)
            {
                SendError(client, gstatus.ToString());
                return;
            }

            AddGuildMemberStatus mstatus = Manager.Database.AddGuildMember(guild, acc, true);

            if (mstatus != AddGuildMemberStatus.OK)
            {
                SendError(client, mstatus.ToString());
                return;
            }

            Manager.Database.UpdateFame(acc, -1000);
            client.Player.CurrentFame = acc.Fame;
            client.Player.Guild = guild.Name;
            client.Player.GuildRank = 40;
            SendSuccess(client);
        }

        private void SendSuccess(Client client)
        {
            client.SendMessage(new GUILDRESULT()
            {
                Success = true,
                ErrorText = "Success!"
            });
        }

        private void SendError(Client client, string message = null)
        {
            client.SendMessage(new GUILDRESULT()
            {
                Success = false,
                ErrorText = "Guild Creation Error: " + message
            });
        }
    }
}