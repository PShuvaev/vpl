using System.Collections.Generic;

namespace VisualPracticalLanguage.Interface
{
    public interface IFunctionCall : IExpression
    {
        IFunctionDeclaration function { get; }
        IList<IExpression> arguments { get; }
    }
}