using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    public class ReturnStatement : IReturnStatement
    {
        public IExpression expression { get; set; }
    }
}