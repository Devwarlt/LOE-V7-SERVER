﻿namespace LoESoft.GameServer.networking
{
    internal partial class NetworkHandler
    {
        private enum IncomingStage
        {
            Awaiting,
            Ready,
            Sending
        }
    }
}