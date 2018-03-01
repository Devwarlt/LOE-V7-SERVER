using LoESoft.Core.models;
using LoESoft.GameServer.networking.incoming;
using LoESoft.GameServer.realm.entity.player;
using System;
using System.Collections.Generic;

namespace LoESoft.GameServer.networking
{
    public enum MessagePriority
    {
        High,
        Normal,
        Low
    }

    public partial class Client
    {
        public ProtocolState State { get; internal set; }
        public Player Player { get; internal set; }

        internal void ProcessMessage(Message msg)
        {
            try
            {
                if (msg.ID == (MessageID)255)
                    return;

                if (!MessageHandler.Handlers.TryGetValue(msg.ID, out IMessage handler))
                    Log.Warn($"Unhandled message ID '{msg.ID}'.");
                else
                    handler.Handle(this, (IncomingMessage)msg);
            }
            catch (NullReferenceException)
            {
                _manager.TryDisconnect(this, DisconnectReason.ERROR_WHEN_HANDLING_MESSAGE);
            }
        }

        public bool IsReady() => State == ProtocolState.Disconnected ? false : (State != ProtocolState.Ready || (Player != null && (Player == null || Player.Owner != null)));
        
        public void SendMessage(Message pkt, MessagePriority priority = MessagePriority.Normal)
        {
            using (TimedLock.Lock(DcLock))
            {
                if (State != ProtocolState.Disconnected)
                    _handler.SendMessage(pkt, priority);
            }
        }

        public void SendMessages(IEnumerable<Message> msgs, MessagePriority priority = MessagePriority.Normal)
        {
            using (TimedLock.Lock(DcLock))
            {
                if (State != ProtocolState.Disconnected)
                    _handler.SendMessages(msgs, priority);
            }
        }
    }
}