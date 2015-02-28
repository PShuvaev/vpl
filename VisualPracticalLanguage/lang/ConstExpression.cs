using System;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class ConstExpression : IConstExpression
	{
		public string constValue { get; set; }

		public ConstExpression ()
		{
		}
	}
}

