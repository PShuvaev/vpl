using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPracticalLanguage
{
	public class VStringConst : VBaseElement
	{
		private string str;

		public VStringConst (string str)
		{
			this.str = str;
			
			color = Color.Orange;
			Size = new Size (100, 50);
			var lbl = new CustomLabel (str, color);

			Controls.Add (lbl);
		}
		
		protected override void OnPaint (PaintEventArgs e)
		{
			{
				var rectangle = new RectangleF (new PointF (0, 0), new SizeF (Size.Width, Const.HEADER_SIZE));
				e.Graphics.FillRectangle (new SolidBrush (color), rectangle);
			}
		}
		
		public override bool TryPutElement (ArgumentPlaceholder p, VBaseElement el)
		{
			return false;
		}
	}
}

