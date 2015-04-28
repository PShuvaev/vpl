using System.Drawing;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    public class VWhileStatement : VCondStatement, IWhileStatement
    {
        public VWhileStatement(IWhileStatement whileStatement) : base("выполнять пока", whileStatement)
        {
            BackColor = ColorSettings.Get("WhileStatement");
        }

        public VWhileStatement() : base("выполнять пока")
        {
            BackColor = ColorSettings.Get("WhileStatement");
        }
    }
}