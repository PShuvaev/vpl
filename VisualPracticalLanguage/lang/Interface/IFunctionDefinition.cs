using System;
using System.Collections.Generic;

namespace VisualPracticalLanguage.Interface
{
	public interface IFunctionDefinition : IFunctionDeclaration
	{
		IList<IVariable> arguments { get; set;}
		IList<IVariable> variables { get; set;}
		IList<IStatement> statements { get; set;}

		IVariable AddArgument (string name);

		void RemoveArgument (string name);

		IVariable AddVariable (string name);

		void RemoveVariable (string name);

		void AddStatement (IStatement statement);

		void InsertStatement (int position, IStatement statement);

		void RemoveStatement (IStatement statement);
	}
}

