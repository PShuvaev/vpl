using System;
using System.Collections.Generic;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class WhileStatement : IWhileStatement
	{
		public IExpression condition { get; set; }
		public IList<IStatement> statements { get; set; }

		public WhileStatement ()
		{
		}
	}
}

