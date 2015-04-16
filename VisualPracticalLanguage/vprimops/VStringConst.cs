using System.Drawing;
using VisualPracticalLanguage.Interface;
using System.Windows.Forms;

namespace VisualPracticalLanguage
{
	public class VStringConst : DraggableControl, IConstExpression
	{
		private string str;
		CustomLabel lbl;

		public VStringConst (string str)
		{
			this.str = str;
			
			BackColor = Color.Orange;
			lbl = new CustomLabel (str, BackColor);
			Controls.Add (lbl);
			UpdateSize ();
			lbl.Location = new Point (Const.TAB_SIZE / 2, Const.TAB_SIZE / 2);

			lbl.MouseDoubleClick += (object sender, MouseEventArgs e) => {
				str = DiverseUtilExtensions.ShowDialog("Введите новое значение", "Новое значение");
				lbl.Text = str;
				UpdateSize();
			};
		}

		public object constValue {
			get { return str; }
		}

		public void UpdateSize(){
			Size = new Size (lbl.Width + Const.TAB_SIZE, lbl.Height + Const.TAB_SIZE);
		}

        public IResizable ResizableParent
        {
            get { return EParent; }
        }
    }
}

