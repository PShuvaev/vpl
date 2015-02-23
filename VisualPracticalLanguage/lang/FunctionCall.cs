using System;
using System.Collections.Generic;

namespace VisualPracticalLanguage
{
	public class FunctionCall : IExpression
	{
		public FunctionDeclaration function { get; set; }
		public IList<IExpression> arguments { get; set; }

		public FunctionCall ()
		{
		}
	}
}