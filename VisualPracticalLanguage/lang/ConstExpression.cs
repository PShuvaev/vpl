using System;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class ConstExpression : IConstExpression
	{
		public object constValue { get; set; }

		public ConstExpression ()
		{
		}
	}
}

