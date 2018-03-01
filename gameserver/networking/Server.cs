#region

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using LoESoft.GameServer.realm;
using LoESoft.Core.config;
using static LoESoft.GameServer.networking.Client;

#endregion

namespace LoESoft.GameServer.networking
{
    internal class Server
    {
        public Server(RealmManager manager)
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Manager = manager;
        }

        private Socket Socket { get; set; }

        public RealmManager Manager { get; private set; }

        public void Start()
        {
            try
            {
                var lep = new IPEndPoint(IPAddress.Any, Settings.GAMESERVER.PORT);
                Socket = new Socket(lep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                Socket.Bind(lep);
                Socket.Listen(1024);
                Socket.BeginAccept(Listen, null);
            }
            catch (Exception ex)
            {
                GameServer.ForceShutdown(ex);
            }
        }

        private void Listen(IAsyncResult ar)
        {
            Socket skt = null;

            try
            {
                skt = Socket.EndAccept(ar);
            }
            catch (ObjectDisposedException) { }

            try
            {
                Socket.BeginAccept(Listen, null);
            }
            catch (ObjectDisposedException) { }

            if (skt != null)
                new Client(Manager, skt);
        }

        public void Stop()
        {
            foreach (ClientData cData in Manager.ClientManager.Values.ToArray())
            {
                cData.Client.Save();
                Manager.TryDisconnect(cData.Client, DisconnectReason.STOPING_SERVER);
            }

            Socket.Close();
        }
    }
}