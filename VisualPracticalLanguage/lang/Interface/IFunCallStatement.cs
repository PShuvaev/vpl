namespace VisualPracticalLanguage.Interface
{
    /// <summary>
    ///     Выражение - вызов функции.
    ///     e.g. "print(42);"
    /// </summary>
    public interface IFunCallStatement : IStatement
    {
        IFunctionCall functionCall { get; }
    }
}