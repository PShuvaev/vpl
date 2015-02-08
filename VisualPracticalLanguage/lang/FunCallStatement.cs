using System;

namespace VisualPracticalLanguage
{
	/// <summary>
	/// Выражение - вызов функции.
	/// e.g. "print(42);"
	/// </summary>
	public class FunCallStatement
	{
		public FunctionCall functionCall { get; set; }

		public FunCallStatement ()
		{
		}
	}
}

