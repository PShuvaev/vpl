using System;

namespace VisualPracticalLanguage.Interface
{
	public interface IFunctionDeclaration
	{
		string name { get; }
		int argumentsCount { get; }

		bool isBinOperation { get; }
	}
}