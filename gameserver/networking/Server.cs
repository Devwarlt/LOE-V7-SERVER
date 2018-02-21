﻿#region

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using log4net;
using LoESoft.GameServer.realm;
using LoESoft.Core.config;
using static LoESoft.GameServer.networking.Client;

#endregion

namespace LoESoft.GameServer.networking
{
    internal class Server
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Server));

        public Server(RealmManager manager)
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Manager = manager;
        }

        public Socket Socket { get; private set; }
        public RealmManager Manager { get; private set; }

        public void Start()
        {
            log.Info("Starting server...");
            Socket.Bind(new IPEndPoint(IPAddress.Any, Settings.GAMESERVER.PORT));
            Socket.Listen(0xff);
            Socket.BeginAccept(Listen, null);
        }

        private void Listen(IAsyncResult ar)
        {
            Socket skt = null;
            try
            {
                skt = Socket.EndAccept(ar);
            }
            catch (ObjectDisposedException)
            {
            }
            try
            {
                Socket.BeginAccept(Listen, null);
            }
            catch (ObjectDisposedException)
            {
            }
            if (skt != null)
                new Client(Manager, skt);
        }

        public void Stop()
        {
            log.Info("Stoping server...");
            foreach (ClientData cData in Manager.ClientManager.Values.ToArray())
            {
                cData.Client.Save();
                Manager.TryDisconnect(cData.Client, DisconnectReason.STOPING_SERVER);
            }
            Socket.Close();
        }
    }
}