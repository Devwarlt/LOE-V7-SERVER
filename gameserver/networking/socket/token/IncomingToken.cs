namespace LoESoft.GameServer.networking
{
    internal partial class NetworkHandler
    {
        private class IncomingToken
        {
            public int Length { get; set; }
            public Message Message { get; set; }
        }
    }
}