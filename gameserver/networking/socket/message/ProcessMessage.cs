using LoESoft.Core.models;
using LoESoft.GameServer.networking.incoming;
using LoESoft.GameServer.realm.entity.player;
using System;
using System.Collections.Generic;

namespace LoESoft.GameServer.networking
{
    public partial class Client
    {
        public ProtocolState State { get; internal set; }
        public Player Player { get; internal set; }

        internal void ProcessMessage(Message msg)
        {
            try
            {
                if (!MessageHandler.Handlers.TryGetValue(msg.ID, out IMessage handler))
                    Log.Warn($"Unhandled message ID '{msg.ID}'.");
                else
                    handler.Handle(this, (IncomingMessage)msg);
            }
            catch (NullReferenceException)
            {
                Log.Warn($"Unhandled Message ID '{msg.ID}'.");
                Manager.TryDisconnect(this, DisconnectReason.ERROR_WHEN_HANDLING_MESSAGE);
            }
        }

        public bool IsReady() => State == ProtocolState.Disconnected ? false : (State != ProtocolState.Ready || (Player != null && (Player == null || Player.Owner != null)));

        public void SendMessage(Message msg) =>
            handler?.IncomingMessage(msg);

        public void SendMessage(IEnumerable<Message> msgs) =>
            handler?.IncomingMessage(msgs);
    }
}