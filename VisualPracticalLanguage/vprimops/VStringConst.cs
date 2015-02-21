using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPracticalLanguage
{
	public class VStringConst : VExpression
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
		
		public override bool PutElement (ArgumentPlaceholder p, VBaseElement el)
		{
			return false;
		}

		public override bool CanPutElement (ArgumentPlaceholder p, VBaseElement el)
		{
			return false;
		}
		
		public override void OnChildDisconnect (DraggableControl c){
		}
	}
}

