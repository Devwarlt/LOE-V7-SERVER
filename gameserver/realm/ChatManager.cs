#region

using common;
using log4net;
using System.Linq;
using gameserver.networking.outgoing;
using gameserver.realm.entity.player;
using gameserver.networking;

#endregion

namespace gameserver.realm
{
    public class ChatManager
    {
        private const char TELL = 't';
        private const char GUILD = 'g';
        private const char ANNOUNCE = 'a';

        private struct Message
        {
            public char Type;
            public string Inst;

            public int ObjId;
            public int Stars;
            public string From;

            public string To;
            public string Text;
        }

        private static ILog log = LogManager.GetLogger(nameof(ChatManager));

        private RealmManager manager;

        public ChatManager(RealmManager manager)
        {
            this.manager = manager;
            manager.InterServer.AddHandler<Message>(ISManager.CHAT, HandleChat);
        }

        public void Say(Player src, string text)
        {
            src.Owner.BroadcastPacket(new TEXT()
            {
                Name = (src.Client.Account.Admin ? "@" : "") + src.Name,
                ObjectId = src.Id,
                Stars = src.Stars,
                Admin = src.Client.Account.Admin ? 1 : 0,
                BubbleTime = 5,
                Recipient = "",
                Text = text,
                CleanText = text,
                NameColor = src.Client.Account.Admin ? 0xFF0000 : 0x123456,
                TextColor = src.Client.Account.Admin ? 0x696969 : 0x123456
            }, null);
            log.Info($"[{src.Owner.Name} ({src.Owner.Id})] <{src.Name}> {text}");
        }

        public void Announce(string text)
        {
            manager.InterServer.Publish(ISManager.CHAT, new Message()
            {
                Type = ANNOUNCE,
                Inst = manager.InstanceId,
                Text = text
            });
        }

        public void Oryx(World world, string text)
        {
            world.BroadcastPacket(new TEXT()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "#Oryx the Mad God",
                Text = text,
                NameColor = 0x123456,
                TextColor = 0x123456
            }, null);
            log.Info($"[{world.Name} ({world.Id})] <Oryx the Mad God> {text}");
        }

        public void Guild(Player player, string text, bool announce = false)
        {
            if (announce)
            {
                foreach (Client i in player.Manager.Clients.Values)
                    if (i != null)
                        i.Player.SendInfo(text.ToSafeText());
            }
            else
            {
                TEXT _text = new TEXT();
                _text.BubbleTime = 10;
                _text.CleanText = "";
                _text.Name = player.Name;
                _text.ObjectId = player.Id;
                _text.Recipient = "*Guild*";
                _text.Stars = player.Stars;
                _text.NameColor = 0x123456;
                _text.TextColor = 0x123456;
                _text.Text = text.ToSafeText();

                player.Client.SendMessage(_text);
            }
        }

        public void Tell(Player player, string BOT_NAME, string callback)
        {
            player.Client.SendMessage(new TEXT() {
                ObjectId = -1,
                BubbleTime = 10,
                Stars = 70,
                Name = BOT_NAME,
                Admin = 0,
                Recipient = player.Name,
                Text = callback.ToSafeText(),
                CleanText = "",
                TextColor = 0x123456,
                NameColor = 0x123456
            });
        }

        private void HandleChat(object sender, InterServerEventArgs<Message> e)
        {
            switch (e.Content.Type)
            {
                case TELL:
                    {
                        string from = manager.Database.ResolveIgn(e.Content.From);
                        string to = manager.Database.ResolveIgn(e.Content.To);
                        foreach (var i in manager.Clients.Values
                            .Where(x => x.Player != null)
                            .Where(x => x.Account.AccountId == e.Content.From ||
                                        x.Account.AccountId == e.Content.To)
                            .Select(x => x.Player))
                        {
                            //i.TellReceived(
                            //    e.Content.Inst == manager.InstanceId ? e.Content.ObjId : -1,
                            //    e.Content.Stars, from, to, e.Content.Text);
                        }
                    }
                    break;
                case GUILD:
                    {
                        string from = manager.Database.ResolveIgn(e.Content.From);
                        foreach (var i in manager.Clients.Values
                            .Where(x => x.Player != null)
                            .Where(x => x.Account.GuildId == e.Content.To)
                            .Select(x => x.Player))
                        {
                           // i.GuildReceived(
                           //     e.Content.Inst == manager.InstanceId ? e.Content.ObjId : -1,
                           //     e.Content.Stars, from, e.Content.Text);
                        }
                    }
                    break;
                case ANNOUNCE:
                    {
                        foreach (var i in manager.Clients.Values
                            .Where(x => x.Player != null)
                            .Select(x => x.Player))
                        {
                          //  i.AnnouncementReceived(e.Content.Text);
                        }
                    }
                    break;
            }
        }
    }
}