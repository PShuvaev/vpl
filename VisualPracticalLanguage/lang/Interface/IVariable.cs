using System;

namespace VisualPracticalLanguage.Interface
{
	public interface IVariable : IExpression
	{
		string varName { get; }
	}
}

