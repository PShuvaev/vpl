namespace VisualPracticalLanguage.Interface
{
    public interface ISetVariableStatement : IStatement
    {
        IVariableRef variableRef { get; }
        IExpression expression { get; }
    }
}