﻿#region

using System;
using System.Globalization;
using System.IO;
using System.Threading;
using LoESoft.Core;
using log4net.Config;
using LoESoft.GameServer.networking;
using LoESoft.GameServer.realm;
using LoESoft.Core.config;
using System.Diagnostics;
using System.Threading.Tasks;
using static LoESoft.GameServer.networking.Client;
using LoESoft.Core.models;
using log4net;

#endregion

namespace LoESoft.GameServer
{
    internal static class Empty<T>
    {
        public static T[] Array = new T[0];
    }

    internal static class GameServer
    {
        public static DateTime Uptime { get; private set; }

        public static readonly ILog log = LogManager.GetLogger("Server");

        private static readonly ManualResetEvent Shutdown = new ManualResetEvent(false);

        public static int GameUsage { get; private set; }
        public static bool AutoRestart { get; private set; }

        public static ChatManager Chat { get; set; }

        public static RealmManager Manager;

        public static DateTime WhiteListTurnOff { get; private set; }

        private static void Main(string[] args)
        {
            Console.Title = "Loading...";

            XmlConfigurator.ConfigureAndWatch(new FileInfo("_gameserver.config"));

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.Name = "Entry";

            using (var db = new Database())
            {
                GameUsage = -1;

                Manager = new RealmManager(db);

                AutoRestart = Settings.NETWORKING.RESTART.ENABLE_RESTART;

                Manager.Initialize();
                Manager.Run();

                Log._("Message", Message.Messages.Count);

                Server server = new Server(Manager);
                PolicyServer policy = new PolicyServer();

                Console.CancelKeyPress += (sender, e) => e.Cancel = true;

                Settings.DISPLAY_SUPPORTED_VERSIONS();

                Log.Info("Initializing GameServer...");

                policy.Start();
                server.Start();

                if (AutoRestart)
                {
                    Chat = Manager.Chat;
                    Uptime = DateTime.Now;
                    Restart();
                    Usage();
                }

                Console.Title = Settings.GAMESERVER.TITLE;

                Log.Info("Initializing GameServer... OK!");

                Console.CancelKeyPress += delegate
                {
                    Shutdown?.Set();
                };

                while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;

                Log.Info("Terminating...");

                server?.Stop();
                policy?.Stop();
                Manager?.Stop();
                Shutdown?.Dispose();

                Log.Warn("Terminated GameServer.");

                Thread.Sleep(1000);

                Environment.Exit(0);
            }
        }

        static int ToMiliseconds(int minutes) => minutes * 60 * 1000;

        public static void Usage()
        {
            Thread parallel_thread = new Thread(() =>
            {
                do
                {
                    Thread.Sleep(ToMiliseconds(Settings.GAMESERVER.TTL) / 60);
                    GameUsage = Manager.ClientManager.Count;
                } while (true);
            });

            parallel_thread.Start();
        }

        public async static void ForceShutdown(Exception ex = null)
        {
            Task task = Task.Delay(1000);

            await task;

            task.Dispose();

            Process.Start(Settings.GAMESERVER.FILE);

            Environment.Exit(0);

            if (ex != null)
                Log.Error(ex.ToString());
        }

        public static void Restart()
        {
            Thread parallel_thread = new Thread(() =>
            {
                Thread.Sleep(ToMiliseconds((Settings.NETWORKING.RESTART.RESTART_DELAY_MINUTES <= 5 ? 6 : Settings.NETWORKING.RESTART.RESTART_DELAY_MINUTES) - 5));
                string message = null;
                int i = 5;
                do
                {
                    message = $"Server will be restarted in {i} minute{(i <= 1 ? "" : "s")}.";
                    Log.Info(message);
                    try
                    {
                        foreach (ClientData cData in Manager.ClientManager.Values)
                            cData.Client.Player.SendInfo(message);
                    }
                    catch (Exception ex)
                    {
                        ForceShutdown(ex);
                    }
                    Thread.Sleep(ToMiliseconds(1));
                    i--;
                } while (i != 0);
                message = "Server is now offline.";
                Log.Warn(message);
                try
                {
                    foreach (ClientData cData in Manager.ClientManager.Values)
                        cData.Client.Player.SendInfo(message);
                }
                catch (Exception ex)
                {
                    ForceShutdown(ex);
                }
                Thread.Sleep(2000);
                try
                {
                    foreach (ClientData cData in Manager.ClientManager.Values)
                        Manager.TryDisconnect(cData.Client, DisconnectReason.RESTART);
                }
                catch (Exception ex)
                {
                    ForceShutdown(ex);
                }
                Process.Start(Settings.GAMESERVER.FILE);
                Environment.Exit(0);
            });

            parallel_thread.Start();
        }

        public static void Stop(Task task = null)
        {
            if (task != null)
                Log.Error(task.Exception.ToString());

            Shutdown.Set();
        }
    }
}