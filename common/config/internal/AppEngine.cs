﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;

namespace common.config
{
    public partial class Settings
    {
        public static class APPENGINE
        {
            public static string TITLE = "[LoESoft] AppEngine";

            public static string FILE = ProcessFile("appengine");

            public static int TTL = 1;

            public static readonly List<Tuple<string, string, double>> SERVERS = new List<Tuple<string, string, double>> {
                Tuple.Create("Test Server", "<crossdomain>", 0.0),
                Tuple.Create("Localhost", "127.0.0.1", 0.8)
            };

            public static int PRODUCTION_PORT = 5555;
            public static int TESTING_PORT = 3000;
            public static int AMOUNT = SERVERS.Count;

            public class ServerItem
            {
                public string Name { get; set; }
                public string DNS { get; set; }
                public double Lat { get; set; }
                public double Long { get; set; }
                public double Usage { get; set; }
                public bool AdminOnly { get; set; }

                public XElement ToXml()
                {
                    return
                        new XElement("Server",
                        new XElement("Name", Name),
                        new XElement("DNS", DNS),
                        new XElement("Lat", Lat),
                        new XElement("Long", Long),
                        new XElement("Usage", Usage),
                        new XElement("AdminOnly", AdminOnly)
                        );
                }
            }

            public static string checkDDNS(string ddns, int srv)
            {
                switch (ddns)
                {
                    case "<crossdomain>": return NETWORKING.INTERNAL.PRODUCTION_DDNS[srv];
                    default: return ddns;
                }
            }

            public static List<ServerItem> returnServerItem()
            {
                List<ServerItem> gameserver = new List<ServerItem>();
                int serverAmount = AMOUNT;
                if (serverAmount == 1)
                {
                    gameserver.Add(new ServerItem()
                    {
                        Name = SERVERS[0].Item1,
                        DNS = checkDDNS(SERVERS[0].Item2, 0),
                        Lat = 0,
                        Long = 0,
                        Usage = SERVERS[0].Item3,
                        AdminOnly = false
                    });
                }
                else
                {
                    for (int i = 0; i < serverAmount; i++)
                        gameserver.Add(new ServerItem()
                        {
                            Name = SERVERS[i].Item1,
                            DNS = checkDDNS(SERVERS[i].Item2, i),
                            Lat = 0,
                            Long = 0,
                            Usage = IS_PRODUCTION ? GetUsage(checkDDNS(SERVERS[i].Item2, i), GAMESERVER.PORT) : SERVERS[i].Item3,
                            AdminOnly = false
                        });
                }
                return gameserver;
            }

            //Usage
            //[0.0-0.79]: Normal, [0.8-0.99]: Crowded, [1.0]: Full

            /// <summary>
            /// Server Usage function
            /// If AppEngine cannot connect to the server or even server is unreachable
            /// it'll return -1 otherwise AMOUNT OF PLAYERS divided by MAX NUMBER OF
            /// CONNECTIONS.
            /// </summary>
            /// <param name="dns"></param>
            /// <param name="port"></param>
            /// <returns></returns>
            public static double GetUsage(string dns, int port)
            {
                IPAddress[] IPs = Dns.GetHostAddresses(dns);

                if (IsListening(dns, port))
                    try
                    {
                        using (TcpClient tcp = new TcpClient(dns, port))
                        {
                            tcp.NoDelay = false;

                            NetworkStream stream = tcp.GetStream();

                            string[] data = null;

                            byte[] response = new byte[tcp.ReceiveBufferSize];
                            byte[] usage = new byte[5] { 0xae, 0x7a, 0xf2, 0xb2, 0x95 };

                            stream.Write(usage, 0, 5);
                            Array.Resize(ref response, tcp.Client.Receive(response));

                            data = Encoding.ASCII.GetString(response).Split(':');

                            double serverUsage = double.Parse(data[1]) / double.Parse(data[0]);

                            tcp.Close();

                            return serverUsage;
                        }
                    }
                    catch (ObjectDisposedException) { return -1; }
                else
                    return -1;
            }

            public static bool IsListening(string dns, int port)
            {
                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    try
                    {
                        if (!socket.BeginConnect(dns, port, null, null).AsyncWaitHandle.WaitOne(2000, true))
                        {
                            socket.Close();
                            return false;
                        }
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode == SocketError.ConnectionRefused || ex.SocketErrorCode == SocketError.TimedOut)
                        {
                            socket.Close();
                            return false;
                        }
                    }
                    return true;
                }
            }
        }
    }
}