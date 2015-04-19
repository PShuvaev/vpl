using System;

namespace VisualPracticalLanguage
{
    public class Logger
    {
        public static void Log(object str)
        {
            Console.WriteLine(str);
        }

        public static void Log(Exception e)
        {
            Console.WriteLine("\r\nError! " + e.Message + " " + e.StackTrace);
        }
    }
}