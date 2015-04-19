using System.Collections.Generic;

namespace VisualPracticalLanguage.Interface
{
    public interface IFunctionDefinition : IFunctionDeclaration
    {
        IList<IVariable> arguments { get; }
        IList<IVariable> variables { get; }
        IList<IStatement> statements { get; }
    }
}