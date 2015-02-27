using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPracticalLanguage
{
	public class VNumberConst : DraggableControl
	{
		private decimal number;

		public VNumberConst (decimal number)
		{
			this.number = number;
			
			BackColor = Color.Blue;
			Size = new Size (100, Const.HEADER_SIZE);
			var lbl = new CustomLabel (number.ToString(), BackColor);

			Controls.Add (lbl);
		}
		
		protected override void OnPaint (PaintEventArgs e)
		{
			{
				var rectangle = new RectangleF (new PointF (0, 0), new SizeF (Size.Width, Const.EXPR_HEIGHT));
				e.Graphics.FillRectangle (new SolidBrush (BackColor), rectangle);
			}
		}
	}
}

