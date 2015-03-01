using System;
using System.Drawing;
using System.Windows.Forms;
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
			Size = new Size (100, Const.HEADER_SIZE);
			var lbl = new CustomLabel (str, BackColor);

			Controls.Add (lbl);
		}

		public string constValue {
			get { return "@\"" + str + "\""; }
		}
	}
}

