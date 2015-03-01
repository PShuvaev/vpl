using System;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class FunctionDeclaration : IFunctionDeclaration
	{
		public string fnamespace { get; set; }
		public string fclass { get; set; }
		public string name { get; set; }
		public int argumentsCount { get; set; }

		public bool isBinOperation { get; set;}
		public bool isReturnVoid { get; set;}

		public FunctionDeclaration ()
		{
		}
	}
}

