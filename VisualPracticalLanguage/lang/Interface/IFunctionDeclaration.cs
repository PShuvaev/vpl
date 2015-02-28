using System;

namespace VisualPracticalLanguage.Interface
{
	public interface IFunctionDeclaration
	{
		string fnamespace { get; set; }
		string fclass { get; set; }
		string name { get; set; }

		bool isBinOperation { get; set;}
		bool isReturnVoid { get; set;}
	}
}

