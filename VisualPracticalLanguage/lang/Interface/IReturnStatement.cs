using System;

namespace VisualPracticalLanguage.Interface
{
	public interface IReturnStatement : IStatement
	{
		IExpression expression { get; }
	}
}

