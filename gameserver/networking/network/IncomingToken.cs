using System;

namespace LoESoft.GameServer.networking.network
{
    internal sealed class IncomingToken
    {
        public const int PrefixLength = 5;

        public readonly int BufferOffset;

        public int BytesRead;
        public int MessageLength;
        public readonly byte[] MessageBytes;

        public IncomingToken(int offset)
        {
            BufferOffset = offset;
            MessageBytes = new byte[Server.BufferSize];
            MessageLength = PrefixLength;
        }

        public byte[] GetMessageBody()
        {
            if (BytesRead < PrefixLength)
                throw new Exception("Message ID not read yet.");

            var packetBody = new byte[MessageLength - PrefixLength];
            Array.Copy(MessageBytes, PrefixLength, packetBody, 0, packetBody.Length);
            return packetBody;
        }

        public MessageID GetMessageID()
        {
            if (BytesRead < PrefixLength)
                throw new Exception("Message ID not read yet.");

            return (MessageID)MessageBytes[4];
        }

        public void Reset()
        {
            MessageLength = PrefixLength;
            BytesRead = 0;
        }
    }
}