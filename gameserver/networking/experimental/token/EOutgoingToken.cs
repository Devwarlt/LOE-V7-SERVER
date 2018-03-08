namespace LoESoft.GameServer.networking.experimental
{
    internal sealed class EOutgoingToken
    {
        public readonly int BufferOffset;

        public int BytesAvailable;
        public int BytesSent;

        public readonly byte[] Data;

        public EOutgoingToken(int offset)
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
