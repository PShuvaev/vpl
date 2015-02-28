using System;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	/// <summary>
	/// Выражение - вызов функции.
	/// e.g. "print(42);"
	/// </summary>
	public class FunCallStatement : IFunCallStatement
	{
		public IFunctionCall functionCall { get; set; }

		public FunCallStatement ()
		{
		}
	}
}

