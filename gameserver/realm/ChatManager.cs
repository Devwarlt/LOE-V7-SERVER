#region

using LoESoft.Core;
using log4net;
using LoESoft.GameServer.networking.outgoing;
using LoESoft.GameServer.realm.entity.player;
using System.Collections.Generic;
using System;

#endregion

namespace LoESoft.GameServer.realm
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

#pragma warning disable CS0649 // Field 'ChatManager.Message.ObjId' is never assigned to, and will always have its default value 0
            public int ObjId;
#pragma warning restore CS0649 // Field 'ChatManager.Message.ObjId' is never assigned to, and will always have its default value 0
#pragma warning disable CS0649 // Field 'ChatManager.Message.Stars' is never assigned to, and will always have its default value 0
            public int Stars;
#pragma warning restore CS0649 // Field 'ChatManager.Message.Stars' is never assigned to, and will always have its default value 0
#pragma warning disable CS0649 // Field 'ChatManager.Message.From' is never assigned to, and will always have its default value null
            public string From;
#pragma warning restore CS0649 // Field 'ChatManager.Message.From' is never assigned to, and will always have its default value null

#pragma warning disable CS0649 // Field 'ChatManager.Message.To' is never assigned to, and will always have its default value null
            public string To;
#pragma warning restore CS0649 // Field 'ChatManager.Message.To' is never assigned to, and will always have its default value null
            public string Text;
        }

        private static ILog log = LogManager.GetLogger(nameof(ChatManager));

        private RealmManager manager;

        public ChatManager(RealmManager manager)
        {
            this.manager = manager;
            manager.InterServer.AddHandler<Message>(ISManager.CHAT, HandleChat);
        }

        public static Dictionary<string, Tuple<DateTime, string>> ChatDataCache = new Dictionary<string, Tuple<DateTime, string>>(); // store only latest player message

        public void Say(Player player, string chatText)
        {
            if (!player.NameChosen)
                return;
            if (!ChatDataCache.ContainsKey(player.Name))
                ChatDataCache.Add(player.Name, Tuple.Create(DateTime.Now, chatText));
            else
                ChatDataCache[player.Name] = Tuple.Create(DateTime.Now, chatText);
            ChatColor color = new ChatColor(player.Stars, player.AccountType);
            TEXT _text = new TEXT();
            _text.Name = player.Name;
            _text.ObjectId = player.Id;
            _text.Stars = player.Stars;
            _text.Admin = player.client.Account.Admin ? 1 : 0;
            _text.BubbleTime = 5;
            _text.Recipient = "";
            _text.Text = chatText;
            _text.CleanText = chatText;
            _text.NameColor = color.GetColor();
            _text.TextColor = 0x123456;
            player.Owner.BroadcastPacket(_text, null);
            log.Info($"[{player.Owner.Name} ({player.Owner.Id})] <{player.Name}> {chatText}");
        }

        public void Announce(string text)
        {
            Message _message = new Message();
            _message.Type = ANNOUNCE;
            _message.Inst = manager.InstanceId;
            _message.Text = text;
            manager.InterServer.Publish(ISManager.CHAT, _message);
        }

        public void Oryx(World world, string text)
        {
            TEXT _text = new TEXT();
            _text.Name = "#Oryx the Mad God";
            _text.Text = text;
            _text.BubbleTime = 0;
            _text.Stars = -1;
            _text.NameColor = _text.TextColor = 0x123456;
            world.BroadcastPacket(_text, null);
            log.Info($"[{world.Name} ({world.Id})] <Oryx the Mad God> {text}");
        }

        public void Guild(Player player, string text, bool announce = false)
        {
            if (announce)
            {
                foreach (ClientData cData in Program.Manager.ClientManager.Values)
                    if (cData.Client != null)
                        cData.Client.Player.SendInfo(text.ToSafeText());
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

                player.client.SendMessage(_text);
            }
        }

        public void Tell(Player player, string BOT_NAME, string callback)
        {
            TEXT _text = new TEXT();
            _text.ObjectId = -1;
            _text.BubbleTime = 10;
            _text.Stars = 70;
            _text.Name = BOT_NAME;
            _text.Admin = 0;
            _text.Recipient = player.Name;
            _text.Text = callback.ToSafeText();
            _text.CleanText = "";
            _text.NameColor = _text.TextColor = 0x123456;
            player.client.SendMessage(_text);
        }

        private void HandleChat(object sender, InterServerEventArgs<Message> e) { }
    }
}