using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPracticalLanguage
{
	public class VStringConst : DraggableControl
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
		
		protected override void OnPaint (PaintEventArgs e)
		{
			{
				var rectangle = new RectangleF (new PointF (0, 0), new SizeF (Size.Width, Const.EXPR_HEIGHT));
				e.Graphics.FillRectangle (new SolidBrush (BackColor), rectangle);
			}
		}
	}
}

