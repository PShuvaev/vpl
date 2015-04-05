using System;

namespace VisualPracticalLanguage.Interface
{
	public interface IVariableRef : IExpression
	{
		string varName { get; }
	}
}

