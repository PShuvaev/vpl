using System;

namespace VisualPracticalLanguage
{
	public class FunctionDeclaration
	{
		public string fnamespace { get; set; }
		public string fclass { get; set; }
		public string name { get; set; }

		public bool IsBinOperation { get; set;}
		public bool ReturnVoid { get; set;}

		public FunctionDeclaration ()
		{
		}
	}
}

