using System;

namespace gameserver
{
    public class Log
    {
        public static string[] time => DateTime.Now.ToString().Split(' ');

        public static void Write(string message, ConsoleColor color = ConsoleColor.White, bool debug = true)
        {
            if (!debug)
                return;
            string response = $"[{time[1]}] {message}";
            Console.ForegroundColor = color;
            Console.WriteLine(response);
            Console.ResetColor();
        }

        public static void Write(string type, string message, ConsoleColor color = ConsoleColor.Yellow, bool debug = true)
        {
            if (!debug)
                return;
            string response = $"[{time[1]}] [{type}] \t->\t{message}";
            Console.ForegroundColor = color;
            Console.WriteLine(response);
            Console.ResetColor();
        }
    }
}