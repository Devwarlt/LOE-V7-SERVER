﻿namespace LoESoft.GameServer.networking
{
    internal partial class NetworkHandler
    {
        private enum OutgoingState
        {
            Awaiting,
            ReceivingHdr,
            ReceivingBody,
            Processing
        }
    }
}
