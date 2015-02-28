using System;
using System.Collections.Generic;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class FunctionDefinition : FunctionDeclaration, IFunctionDefinition
	{
		public IList<IVariable> arguments { get; set;}
		public IList<IVariable> variables { get; set;}
		public IList<IStatement> statements { get; set;}

		public FunctionDefinition ()
		{
			arguments = new List<IVariable> ();
			variables = new List<IVariable> ();
			statements = new List<IStatement> ();
		}
		
		public IVariable AddArgument(string name)
		{
			var v = new Variable { varName = name };
			arguments.Add (v);
			return v;
		}

		public void RemoveArgument(string name)
		{
		}

		public IVariable AddVariable(string name)
		{
			var v = new Variable { varName = name };
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

