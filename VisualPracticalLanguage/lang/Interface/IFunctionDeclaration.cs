using System;

namespace VisualPracticalLanguage.Interface
{
	public interface IFunctionDeclaration
	{
		string fnamespace { get; }
		string fclass { get; }
		string name { get; }
		int argumentsCount { get; }

		bool isBinOperation { get; }
		bool isReturnVoid { get; }
	}
}