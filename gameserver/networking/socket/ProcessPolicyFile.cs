using LoESoft.Core;
using LoESoft.Core.config;
using System.Net.Sockets;
using static LoESoft.GameServer.networking.Client;

namespace LoESoft.GameServer.networking
{
    internal partial class NetworkHandler
    {
        private void ProcessPolicyFile()
        {
            NetworkStream s = new NetworkStream(skt);
            NWriter wtr = new NWriter(s);
            wtr.WriteNullTerminatedString(Settings.IS_PRODUCTION ? Settings.NETWORKING.INTERNAL.SELECTED_DOMAINS : Settings.NETWORKING.INTERNAL.LOCALHOST_DOMAINS);
            wtr.Write((byte)'\r');
            wtr.Write((byte)'\n');
            Manager.TryDisconnect(client, DisconnectReason.PROCESS_POLICY_FILE);
        }
    }
}