using System;
using System.Threading;
using webserver.engine;

namespace webserver
{
    public static class Constants
    {
        private static int MAJOR_VERSION = 1;
        private static int MINOR_VERSION = 0;

        public static string VERSION = $"v{MAJOR_VERSION}.{MINOR_VERSION}";
        public static int PORT = 3333;
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = $"[LoESoft] WebServer {Constants.VERSION}";

            Log.Info("Initializing WebServer...");

            WebServer.Start();

            while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            WebServer.shutdown = true;

            Log.Warn("Terminating WebServer, disposing all instances.");

            do
            {
                Log.Info($"Awaiting {WebServer._webqueue.Count} queued item{(WebServer._webqueue.Count > 1 ? "s" : "")} to dispose, retrying in 1 second...", ConsoleColor.Green);
                Thread.Sleep(1000);
            } while (WebServer._webqueue.Count > 0);

            WebServer._websocket?.Stop();
            WebServer._webevent.Set();

            Log.Warn("Terminated WebServer.");

            Thread.Sleep(1000);

            Environment.Exit(0);
        }
    }
}