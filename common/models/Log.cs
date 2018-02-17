#region

using System;

#endregion

namespace common.models
{
    public class Log
    {
        public static string[] time => DateTime.Now.ToString().Split(' ');

        public static void Info(string message)
        {
            string response = $"[{time[1]}] {message}";
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(response);
            Console.ResetColor();
        }

        public static void Info(string type, string message, ConsoleColor color = ConsoleColor.White)
        {
            string response = $"[{time[1]}] {type}\t->\t{message}";
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(response);
            Console.ResetColor();
        }

        public static void Warn(string message)
        {
            string response = $"[{time[1]}] {message}";
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(response);
            Console.ResetColor();
        }

        public static void Warn(string type, string message)
        {
            string response = $"[{time[1]}] {type}\t->\t{message}";
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(response);
            Console.ResetColor();
        }

        public static void Error(string message)
        {
            string response = $"[{time[1]}] {message}";
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(response);
            Console.ResetColor();
        }

        public static void Error(string type, string message)
        {
            string response = $"[{time[1]}] {type}\t->\t{message}";
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(response);
            Console.ResetColor();
        }
    }
}