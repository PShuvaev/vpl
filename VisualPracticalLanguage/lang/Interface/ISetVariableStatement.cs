using System;

namespace VisualPracticalLanguage.Interface
{
	public interface ISetVariableStatement : IStatement
	{
		IVariable variable { get; }
		IExpression expression { get; }
	}
}

