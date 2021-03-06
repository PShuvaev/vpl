using System.Drawing;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    public class VNumberConst : DraggableControl, IResizable, IConstExpression
    {
        private decimal number;
        private readonly CustomLabel lbl;

        public VNumberConst(decimal number)
        {
            this.number = number;

            BackColor = ColorSettings.Get("NumberConst");
            lbl = new CustomLabel(number.ToString(), BackColor);
            lbl.Location = new Point(Const.TAB_SIZE/2, Const.TAB_SIZE/2);

            lbl.MouseDoubleClick += (sender, e) =>
            {
                var newName = DiverseUtilExtensions.ShowDialog("Введите новое значение", "Новое значение");
                newName = newName.Trim();
                decimal result;
                if (newName.Length == 0 || !decimal.TryParse(newName, out result)) return;

                this.number = result;
                lbl.Text = this.number.ToString();
                this.UpdateRecSize();
            };

            Controls.Add(lbl);
            UpdateSize();
        }

        public object constValue
        {
            get { return number; }
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