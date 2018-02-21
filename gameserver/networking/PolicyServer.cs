#region

using System;
using System.Net;
using System.Net.Sockets;
using LoESoft.Core;
using LoESoft.Core.config;

#endregion

namespace LoESoft.GameServer.networking
{
    internal class PolicyServer
    {
        private readonly TcpListener listener;
        private bool started;

        public PolicyServer()
        {
            listener = new TcpListener(IPAddress.Any, 843);
        }

        private static void ServePolicyFile(IAsyncResult ar)
        {
            try
            {
                TcpClient cli = (ar.AsyncState as TcpListener).EndAcceptTcpClient(ar);
                (ar.AsyncState as TcpListener).BeginAcceptTcpClient(ServePolicyFile, ar.AsyncState);
                NetworkStream s = cli.GetStream();
                NReader rdr = new NReader(s);
                NWriter wtr = new NWriter(s);
                if (rdr.ReadNullTerminatedString() == "<policy-file-request/>")
                {
                    wtr.WriteNullTerminatedString(Settings.IS_PRODUCTION ? Settings.NETWORKING.INTERNAL.SELECTED_DOMAINS : Settings.NETWORKING.INTERNAL.LOCALHOST_DOMAINS);
                    wtr.Write((byte)'\r');
                    wtr.Write((byte)'\n');
                }
                cli.Close();
            }
            catch (ObjectDisposedException) { }
            catch (Exception) { }
        }

        public void Start()
        {
            try
            {
                listener.Start();
                listener.BeginAcceptTcpClient(ServePolicyFile, listener);
                started = true;
            }
            catch (ObjectDisposedException) { }
            catch (Exception)
            {
                started = false;
            }
        }

        public void Stop()
        {
            if (started)
                listener.Stop();
        }
    }
}