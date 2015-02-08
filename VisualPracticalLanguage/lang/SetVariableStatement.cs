using System;

namespace VisualPracticalLanguage
{
	public class SetVariableStatement : IStatement
	{
		public Variable variable { get; set; }
		public IExpression expression { get; set; }

		public SetVariableStatement ()
		{
		}
	}
}

