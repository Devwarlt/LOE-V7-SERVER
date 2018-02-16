#region

using common;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using common.config;
using common.models;

#endregion

namespace appengine
{
    internal class Program
    {
        private static readonly List<HttpListenerContext> currentRequests = new List<HttpListenerContext>();

        private static HttpListener listener
        { get; set; }

        public static ILog Logger
        { get; } = LogManager.GetLogger("AppEngine");

        public static bool restart
        { get { return Settings.NETWORKING.RESTART.ENABLE_RESTART; } }

        public static string message
        { get; private set; }

        public delegate bool WebSocketDelegate();

        internal static Database Database
        { get; set; }

        internal static EmbeddedData GameData
        { get; set; }

        internal static ISManager Manager
        { get; set; }

        internal static string InstanceId
        { get; set; }

        internal static AppEngine appEngine
        { get; set; }

        private static void Main(string[] args)
        {
            message = null;

            message = "Loading...";

            Console.Title = message;

            XmlConfigurator.ConfigureAndWatch(new FileInfo("_appengine.config"));

            using (Database = new Database())
            {
                GameData = new EmbeddedData();
                InstanceId = Guid.NewGuid().ToString();

                Manager = new ISManager();
                Manager.Run();

                Log.Info("Initializing AppEngine...");

                appEngine = new AppEngine(restart);
                appEngine.Start();

                Console.Title = Settings.APPENGINE.TITLE;

                while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;

                appEngine._shutdown = true;

                Log.Warn("Terminating AppEngine, disposing all instances.");
                
                IAsyncResult webSocketIAsyncResult = new WebSocketDelegate(appEngine.SafeShutdown).BeginInvoke(new AsyncCallback(appEngine.SafeDispose), null);
                webSocketIAsyncResult.AsyncWaitHandle.WaitOne();
            }
        }
    }
}