using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace VisualPracticalLanguage
{
	public class VFunction : VBaseElement
	{
		private string name;
		private IList<VBaseElement> expressions;
		private IList<CustomLabel> arguments;
		private CustomLabel funName;

		public VFunction (string name)
		{
			this.name = name;

			
			funName = new CustomLabel (name, color);
			Controls.Add (funName);

			Size = new Size (200, 200);
			expressions = new List<VBaseElement> ();
			arguments = new List<CustomLabel> ();
		}

		public void AddArgument(string arg)
		{
			var label = new CustomLabel (arg, color);

			if (!arguments.Any ()) {
				label.Location = new Point (funName.Location.X + funName.Size.Width + 10, 0);
			} else {
				var lastArg = arguments.Last ();
				label.Location = new Point (lastArg.Location.X + lastArg.Size.Width + 10, 0);
			}

			arguments.Add (label);

			Controls.Add (label);
		}
		
		protected override void OnPaint (PaintEventArgs e)
		{
			{
				var rectangle = new RectangleF (new PointF (0, 0), new SizeF (Size.Width, Const.HEADER_SIZE));
				e.Graphics.FillRectangle (new SolidBrush (color), rectangle);
				
			}
			{
				var rectangle = new RectangleF (new PointF (0, Const.HEADER_SIZE), new SizeF (Const.TAB_SIZE, Size.Height-Const.PALLET_HEIGHT));
				e.Graphics.FillRectangle (new SolidBrush (color), rectangle);
			}

			{
				var rectangle = new RectangleF (new PointF (0, Size.Height-Const.PALLET_HEIGHT), new SizeF (Size.Width/2, Const.PALLET_HEIGHT));
				e.Graphics.FillRectangle (new SolidBrush (color), rectangle);
			}
		}

		public void AddExpression(VBaseElement expr){
			expr.Parent = this;

			if (!expressions.Any ()) {
				expr.Location = new Point (Const.TAB_SIZE, Const.HEADER_SIZE);
			} else {
				var lastExpr = expressions.Last ();
				expr.Location = new Point (Const.TAB_SIZE, lastExpr.Location.Y + lastExpr.Size.Height + 2);
			}
			
			expressions.Add (expr);
		}

		protected override bool TakeElement (Point p, VBaseElement el)
		{
			return el is VExpression;// && location;
		}
	}
}

