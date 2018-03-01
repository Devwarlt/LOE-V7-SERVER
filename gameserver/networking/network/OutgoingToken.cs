namespace LoESoft.GameServer.networking.network
{
    internal sealed class OutgoingToken
    {
        public readonly int BufferOffset;

        public int BytesAvailable;
        public int BytesSent;

        public readonly byte[] Data;

        public OutgoingToken(int offset)
        {
            BufferOffset = offset;
            Data = new byte[0x100000];
        }

        public void Reset()
        {
            BytesAvailable = 0;
            BytesSent = 0;
        }
    }
}