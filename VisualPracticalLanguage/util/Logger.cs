using System;
using System.IO;

namespace VisualPracticalLanguage
{
	public class Logger
	{
		public Logger ()
		{
		}

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

