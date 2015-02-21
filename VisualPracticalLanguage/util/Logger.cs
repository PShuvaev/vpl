using System;
using System.IO;

namespace VisualPracticalLanguage
{
	public class Logger
	{
		public Logger ()
		{
		}

		public static void Log(object str){
			File.AppendAllText ("/home/ps/projects/VisualPracticalLanguage/VisualPracticalLanguage/log.txt", str+"\n");
			Console.WriteLine (str);
		}
	}
}

