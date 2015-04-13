using System.Drawing;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class VStringConst : DraggableControl, IConstExpression
	{
		private string str;

		public VStringConst (string str)
		{
			this.str = str;
			
			BackColor = Color.Orange;
			var lbl = new CustomLabel (str, BackColor);
			Controls.Add (lbl);
			//TODO: TAB_SIZE -> PaddingSize
			Size = new Size (lbl.Size.Width + BorderPadding, lbl.Size.Height + BorderPadding);
			lbl.Location = new Point (BorderPadding/2, BorderPadding/2);
		}

		public object constValue {
			get { return str; }
		}
	}
}

