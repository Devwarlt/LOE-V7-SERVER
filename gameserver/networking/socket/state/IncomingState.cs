namespace LoESoft.GameServer.networking
{
    internal partial class NetworkHandler
    {
        private enum OutgoingStage
        {
            Awaiting,
            Ready,
            Sending
        }
    }
}