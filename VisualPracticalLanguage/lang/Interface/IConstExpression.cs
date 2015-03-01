using System;

namespace VisualPracticalLanguage.Interface
{
	public interface IConstExpression : IExpression
	{
		string constValue { get; }
	}
}

