using System;
using VisualPracticalLanguage.Interface;
using System.Collections.Generic;

namespace VisualPracticalLanguage
{
	public interface ICondStatement : IStatement
	{
		IExpression condition { get; }
		IList<IStatement> statements { get; }
	}
}

