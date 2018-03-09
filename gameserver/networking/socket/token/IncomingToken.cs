using LoESoft.Core.models;
using System;

namespace LoESoft.GameServer.networking
{
    internal partial class NetworkHandler
    {
        private sealed class IncomingToken
        {
            public Message Message { get; set; }
            public int MessageLength { get; set; }
            public int BytesRead { get; set; }
            public byte[] MessageBytes { get; set; }

            public MessageID GetMessageID()
            {
                if (BytesRead < 5)
                    throw new Exception("Message ID not read yet.");

                return (MessageID)MessageBytes[4];
            }

            public void Reset()
            {
                Log.Warn("Incoming token reseted.");

                MessageLength = 0;
                BytesRead = 0;
            }
        }
    }
}