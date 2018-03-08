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
        public void SendMessage(Message pkt, MessagePriority priority = MessagePriority.Normal)
        {
            using (TimedLock.Lock(DcLock))
                if (State != ProtocolState.Disconnected)
                    _ehandler.SendMessage(pkt, priority);
        }

        public void SendMessage(IEnumerable<Message> msgs, MessagePriority priority = MessagePriority.Normal)
        {
            using (TimedLock.Lock(DcLock))
                if (State != ProtocolState.Disconnected)
                    _ehandler.SendMessages(msgs, priority);
        }
    }
}
