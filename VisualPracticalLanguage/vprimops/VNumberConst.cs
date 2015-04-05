using System;
using System.Drawing;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class VNumberConst : DraggableControl, IConstExpression
	{
		private decimal number;

		public VNumberConst (IConstExpression constExpression) : 
			this(decimal.Parse(constExpression.constValue))
		{
		}

		public VNumberConst (decimal number)
		{
			this.number = number;
			
			BackColor = Color.Blue;
			Size = new Size (20, Const.HEADER_SIZE);
			var lbl = new CustomLabel (number.ToString (), BackColor);

			Controls.Add (lbl);
		}

		public string constValue {
			get { return number.ToString (); }
			set { throw new NotImplementedException (); }
		}
	}
}

