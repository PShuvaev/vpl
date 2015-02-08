using System;
using System.Collections.Generic;

namespace VisualPracticalLanguage
{
	public class FunctionDefinition : FunctionDeclaration
	{
		public IList<Variable> arguments { get; private set;}
		public IList<Variable> variables { get; private set;}
		public IList<IStatement> statements { get; private set;}

		public FunctionDefinition ()
		{
			arguments = new List<Variable> ();
			variables = new List<Variable> ();
			statements = new List<IStatement> ();
		}
		
		public Variable AddArgument(string name)
		{
			var v = new Variable { name = name };
			arguments.Add (v);
			return v;
		}

		public void RemoveArgument(string name)
		{
		}

		public Variable AddVariable(string name)
		{
			var v = new Variable { name = name };
			variables.Add (v);
			return v;
		}

		public void RemoveVariable(string name)
		{
		}

		public void AddStatement(IStatement statement)
		{
			statements.Add (statement);
		}

		public void InsertStatement(int position, IStatement statement)
		{
			statements.Insert (position, statement);
		}

		public void RemoveStatement(IStatement statement)
		{
		}
	}
}

