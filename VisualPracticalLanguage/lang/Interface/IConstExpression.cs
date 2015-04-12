using System;

namespace VisualPracticalLanguage.Interface
{
	public interface IConstExpression : IExpression
	{
		object constValue { get; }
	}
}

