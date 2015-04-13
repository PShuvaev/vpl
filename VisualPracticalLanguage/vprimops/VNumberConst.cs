using System;
using System.Drawing;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class VNumberConst : DraggableControl, IConstExpression
	{
		decimal number;

		public VNumberConst (decimal number)
		{
			this.number = number;
			
			BackColor = Color.Blue;
			Size = new Size (20, Const.HEADER_SIZE);
			var lbl = new CustomLabel (number.ToString (), BackColor);

			Controls.Add (lbl);
		}

		public object constValue {
			get { return number; }
		}
	}
}

