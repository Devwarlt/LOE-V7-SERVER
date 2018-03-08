using System;

namespace LoESoft.GameServer.networking.experimental
{
    internal sealed class EIncomingToken
    {
        public const int PrefixLength = 5;

        public readonly int BufferOffset;

        public int BytesRead;
        public int MessageLength;
        public readonly byte[] MessageBytes;

        public EIncomingToken(int offset)
        {
            BufferOffset = offset;
            MessageBytes = new byte[EServer.BufferSize];
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
