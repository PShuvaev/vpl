using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    // TODO: реализовать в IFunctionCall интерфейс IFunCallStatement и делов-то?
    /// <summary>
    ///     Выражение - вызов функции.
    ///     e.g. "print(42);"
    /// </summary>
    public class FunCallStatement : IFunCallStatement
    {
        public IFunctionCall functionCall { get; set; }
    }
}