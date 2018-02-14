﻿#region

using System.Linq;
using gameserver.realm;

#endregion

namespace gameserver.logic.transitions
{
    public class ChatTransition : Transition
    {
        private readonly string[] texts;
        private bool transit;

        public ChatTransition(string targetState, params string[] texts)
            : base(targetState)
        {
            this.texts = texts ?? Empty<string>.Array;
            transit = false;
        }

        protected override bool TickCore(Entity host, RealmTime time, ref object state)
        {
            return transit;
        }

        public void OnChatReceived(string text)
        {
            if (texts.Contains(text))
                transit = true;
        }
    }
}
