﻿using gameserver.networking.incoming;
using gameserver.realm.entity.player;
using log4net.Core;
using System.Collections.Generic;

namespace gameserver.networking
{
    public partial class Client
    {
        public ProtocolState State { get; internal set; }
        public Player Player { get; internal set; }

        internal void ProcessMessage(Message msg)
        {
            try
            {
                Program.Logger.Logger.Log(typeof(Client), Level.Verbose, $"Handling message '{msg}'...", null);

                if (msg.ID == (MessageID)255)
                    return;

                IMessage handler;

                if (!MessageHandler.Handlers.TryGetValue(msg.ID, out handler))
                    Program.Logger.Warn($"Unhandled message '{msg.ID}'.");
                else
                    handler.Handle(this, (IncomingMessage)msg);
            }
            catch
            {
                Program.Logger.Error($"Message error\t->\t{msg.ID}");
                Disconnect(DisconnectReason.ERROR_WHEN_HANDLING_MESSAGE);
            }
        }

        public bool IsReady() => State == ProtocolState.Disconnected ? false : (State != ProtocolState.Ready || (Player != null && (Player == null || Player.Owner != null)));

        public void SendMessage(Message msg) => handler?.IncomingMessage(msg);

        public void SendMessage(IEnumerable<Message> msgs) => handler?.IncomingMessage(msgs);
    }
}