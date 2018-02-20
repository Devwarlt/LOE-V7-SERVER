#region

using System;
using System.Globalization;
using System.IO;
using System.Threading;
using common;
using log4net;
using log4net.Config;
using gameserver.networking;
using gameserver.realm;
using common.config;
using System.Diagnostics;
using System.Threading.Tasks;
using static gameserver.networking.Client;

#endregion

namespace gameserver
{
    internal static class Program
    {
        public static DateTime Uptime { get; private set; }
        public static readonly ILog Logger = LogManager.GetLogger("Server");

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

                Server server = new Server(Manager);
                PolicyServer policy = new PolicyServer();

                Console.CancelKeyPress += (sender, e) => e.Cancel = true;

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

                Logger.Info("Server initialized.");

                Logger.Info($"Game Versions (max 5):\n\t * {Settings.NETWORKING.SUPPORTED_VERSIONS_DISPLAY()}");

                Console.CancelKeyPress += delegate
                {
                    Shutdown?.Set();
                };

                while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;

                Logger.Info("Terminating...");
                server?.Stop();
                policy?.Stop();
                Manager?.Stop();
                Shutdown?.Dispose();
                Logger.Info("Server terminated.");
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
                Logger.Error(ex);
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
                    Logger.Info(message);
                    try
                    {
                        foreach (ClientData cData in Manager.ClientManager.Values)
                            Chat.Tell(cData.client.Player, "(!) Notification (!)", ("Hey (PLAYER_NAME), prepare to disconnect. " + message).Replace("(PLAYER_NAME)", cData.client.Player.Name));
                    }
                    catch (Exception ex)
                    {
                        ForceShutdown(ex);
                    }
                    Thread.Sleep(ToMiliseconds(1));
                    i--;
                } while (i != 0);
                message = "Server is now offline.";
                Logger.Warn(message);
                try
                {
                    foreach (ClientData cData in Manager.ClientManager.Values)
                        Chat.Tell(cData.client.Player, "(!) Notification (!)", message);
                }
                catch (Exception ex)
                {
                    ForceShutdown(ex);
                }
                Thread.Sleep(2000);
                try
                {
                    foreach (ClientData cData in Manager.ClientManager.Values)
                        Manager.TryDisconnect(cData.client, DisconnectReason.RESTART);
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
                Logger.Fatal(task.Exception);

            Shutdown.Set();
        }
    }
}