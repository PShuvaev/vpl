using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    public class VIfStatement : VCondStatement, IIfStatement
    {
        public VIfStatement(IIfStatement ifStatement) : base("если", ifStatement)
        {
            BackColor = ColorSettings.Get("IfStatement");
        }

        public VIfStatement() : base("если")
        {
            BackColor = ColorSettings.Get("IfStatement");
        }
    }
}