using System.Drawing;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    public class VStringConst : DraggableControl, IConstExpression, IResizable
    {
        private string str;
        private readonly CustomLabel lbl;

        public VStringConst(string str)
        {
            this.str = str;

            BackColor = ColorSettings.Get("StringConst");
            lbl = new CustomLabel(str, BackColor);
            Controls.Add(lbl);
            UpdateSize();
            lbl.Location = new Point(Const.TAB_SIZE/2, Const.TAB_SIZE/2);

            lbl.MouseDoubleClick += (sender, e) =>
            {
                this.str = DiverseUtilExtensions.ShowDialog("Введите новое значение", "Новое значение");
                lbl.Text = this.str;
                this.UpdateRecSize();
            };
        }

        public object constValue
        {
            get { return str; }
        }

        public void UpdateSize()
        {
            Size = new Size(lbl.Width + Const.TAB_SIZE, lbl.Height + Const.TAB_SIZE);
        }

        public IResizable ResizableParent
        {
            get { return EParent; }
        }
    }
}