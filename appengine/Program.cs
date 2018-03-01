#region

using LoESoft.Core;
using log4net.Config;
using System;
using System.IO;
using LoESoft.Core.config;
using LoESoft.Core.models;

#endregion

namespace LoESoft.AppEngine
{
    internal class Program
    {
        public static bool Restart
        { get { return Settings.NETWORKING.RESTART.ENABLE_RESTART; } }

        public static string Message
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

        internal static AppEngine AppEngineInstance
        { get; set; }

        private static void Main(string[] args)
        {
            Message = null;

            Message = "Loading...";

            Console.Title = Message;

            XmlConfigurator.ConfigureAndWatch(new FileInfo("_appengine.config"));

            using (Database = new Database())
            {
                GameData = new EmbeddedData();
                InstanceId = Guid.NewGuid().ToString();

                Manager = new ISManager();
                Manager.Run();

                Log.Info("Initializing AppEngine...");

                AppEngineInstance = new AppEngine(Restart);
                AppEngineInstance.Start();

                Console.Title = Settings.APPENGINE.TITLE;

                while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;

                AppEngineInstance._shutdown = true;

                Log.Warn("Terminating AppEngine, disposing all instances.");

                IAsyncResult webSocketIAsyncResult = new WebSocketDelegate(AppEngineInstance.SafeShutdown).BeginInvoke(new AsyncCallback(AppEngineInstance.SafeDispose), null);
                webSocketIAsyncResult.AsyncWaitHandle.WaitOne();
            }
        }
    }
}