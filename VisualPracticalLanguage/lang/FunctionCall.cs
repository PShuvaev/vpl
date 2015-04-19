using System.Collections.Generic;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    public class FunctionCall : IFunctionCall
    {
        public IFunctionDeclaration function { get; set; }
        public IList<IExpression> arguments { get; set; }
    }
}