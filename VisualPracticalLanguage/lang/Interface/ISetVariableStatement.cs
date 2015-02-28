using System;

namespace VisualPracticalLanguage.Interface
{
	public interface ISetVariableStatement : IStatement
	{
		IVariable variable { get; set; }
		IExpression expression { get; set; }
	}
}

