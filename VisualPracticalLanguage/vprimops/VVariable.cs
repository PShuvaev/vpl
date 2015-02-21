using System;
using System.Windows.Forms;
using System.Drawing;

namespace VisualPracticalLanguage
{
	public class VVariable : VBaseElement
	{
		private CustomLabel name;

		public VVariable (string vname)
		{
			color = Color.Yellow;
			Size = new Size (100, 50);
			name = new CustomLabel (vname, color);

			Controls.Add (name);
			var eq = new CustomLabel (" = ", color);
			Controls.Add (eq);
			name.TextChanged += delegate {
				var nameLoc = name.Location;
				eq.Location = new Point(nameLoc.X + name.Width + 10, 0);
			};
		}
		
		protected override void OnPaint (PaintEventArgs e)
		{
			{
				var rectangle = new RectangleF (new PointF (0, 0), new SizeF (Size.Width, Const.EXPR_HIGHT));
				e.Graphics.FillRectangle (new SolidBrush (color), rectangle);
			}
			
			{
				var rectangle = new RectangleF (new PointF (35, 5), new SizeF (5, 5));
				e.Graphics.FillRectangle (new SolidBrush (Color.White), rectangle);
			}
		}
		
		public override bool CanPutElement (ArgumentPlaceholder p, VBaseElement el)
		{
			return el is VExpression;// && location;
		}

		public override bool PutElement (ArgumentPlaceholder p, VBaseElement el)
		{
			return el is VExpression;// && location;
		}

		
		public override void OnChildDisconnect (DraggableControl c){
		}
	}
}

