using System;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class SetVariableStatement : ISetVariableStatement
	{
		public IVariable variable { get; set; }
		public IExpression expression { get; set; }

		public SetVariableStatement ()
		{
		}
	}
}

