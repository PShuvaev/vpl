using System.Collections.Generic;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    public class FunctionDefinition : FunctionDeclaration, IFunctionDefinition
    {
        public FunctionDefinition()
        {
            arguments = new List<IVariable>();
            variables = new List<IVariable>();
            statements = new List<IStatement>();
        }

        public IList<IVariable> arguments { get; set; }
        public IList<IVariable> variables { get; set; }
        public IList<IStatement> statements { get; set; }

        public IVariableRef AddArgument(string name)
        {
            arguments.Add(new Variable {varName = name});
            return new VariableRef {varName = name};
        }

        public void RemoveArgument(string name)
        {
        }

        public IVariableRef AddVariable(string name)
        {
            variables.Add(new Variable {varName = name});
            return new VariableRef {varName = name};
        }

        public void RemoveVariable(string name)
        {
        }

        public void AddStatement(IStatement statement)
        {
            statements.Add(statement);
        }

        public void InsertStatement(int position, IStatement statement)
        {
            statements.Insert(position, statement);
        }

        public void RemoveStatement(IStatement statement)
        {
        }
    }
}