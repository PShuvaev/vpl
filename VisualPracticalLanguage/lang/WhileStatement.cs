using System;
using System.Collections.Generic;

namespace VisualPracticalLanguage
{
	public class WhileStatement : IStatement
	{
		public FunctionCall condition { get; set; }
		public IList<IStatement> statements { get; set; }

		public WhileStatement ()
		{
		}
	}
}

