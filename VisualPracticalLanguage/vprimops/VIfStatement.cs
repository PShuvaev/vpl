using System.Drawing;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    public class VIfStatement : VCondStatement, IIfStatement
    {
        public VIfStatement(IIfStatement ifStatement) : base("если", ifStatement)
        {
            BackColor = Color.LightGray;
        }

        public VIfStatement() : base("если")
        {
            BackColor = Color.LightGray;
        }
    }
}