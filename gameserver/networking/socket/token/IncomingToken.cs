namespace LoESoft.GameServer.networking
{
    internal partial class NetworkHandler
    {
        private class OutgoingToken
        {
            public int Length { get; set; }
            public Message Message { get; set; }
        }
    }
}