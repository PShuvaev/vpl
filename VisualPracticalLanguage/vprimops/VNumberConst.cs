using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPracticalLanguage
{
	public class VNumberConst : VBaseElement
	{
		private decimal number;

		public VNumberConst (decimal number)
		{
			this.number = number;
			
			color = Color.Blue;
			Size = new Size (100, 50);
			var lbl = new CustomLabel (number.ToString(), color);

			Controls.Add (lbl);
		}
		
		protected override void OnPaint (PaintEventArgs e)
		{
			{
				var rectangle = new RectangleF (new PointF (0, 0), new SizeF (Size.Width, Const.HEADER_SIZE));
				e.Graphics.FillRectangle (new SolidBrush (color), rectangle);
			}
		}

		protected override bool TakeElement (Point p, VBaseElement el)
		{
			return false;
		}
	}
}

