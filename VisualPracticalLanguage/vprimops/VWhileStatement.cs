using System.Drawing;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    public class VWhileStatement : VCondStatement, IWhileStatement
    {
        public VWhileStatement(IWhileStatement whileStatement) : base("выполнять пока", whileStatement)
        {
            BackColor = Color.Bisque;
        }

        public VWhileStatement() : base("выполнять пока")
        {
            BackColor = Color.Bisque;
        }
    }
}