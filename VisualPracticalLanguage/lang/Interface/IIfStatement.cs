using System;
using System.Collections.Generic;

namespace VisualPracticalLanguage.Interface
{
	public interface IIfStatement : IStatement
	{
		IExpression condition { get; set; }
		IList<IStatement> statements { get; set; }
	}
}