using System;

namespace VisualPracticalLanguage.Interface
{
	/// <summary>
	/// Выражение - вызов функции.
	/// e.g. "print(42);"
	/// </summary>
	public interface IFunCallStatement
	{
		IFunctionCall functionCall { get; set; }
	}
}

