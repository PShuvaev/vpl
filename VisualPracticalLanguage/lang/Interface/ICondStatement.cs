using System.Collections.Generic;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    public interface ICondStatement : IStatement
    {
        IExpression condition { get; }
        IList<IStatement> statements { get; }
    }
}