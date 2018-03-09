using LoESoft.Core.models;
using System;

namespace LoESoft.GameServer.networking
{
    internal partial class NetworkHandler
    {
        private sealed class IncomingToken
        {
            public Message Message { get; set; }
            public int Length { get; set; }
        }
    }
}