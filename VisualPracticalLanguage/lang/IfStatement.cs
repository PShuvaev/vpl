using System;
using System.Collections.Generic;

namespace VisualPracticalLanguage
{
	public class IfStatement : IStatement
	{
		public IExpression condition { get; set; }
		public IList<IStatement> statements { get; set; }

		public IfStatement ()
		{
		}
	}
}