using VisualPracticalLanguage.Interface;
using System.IO;

namespace VisualPracticalLanguage
{
	public static class Persistence
	{
		public static void Save(string path, INamespace @namespace){
			var codeSource = new StringWriter ();
			new Generator (codeSource).Generate (@namespace);
			File.WriteAllText (path, codeSource.ToString ());
		}

		public static INamespace Load(string path){
			return null;
		}
	}
}

