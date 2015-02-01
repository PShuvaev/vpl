using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPracticalLanguage
{
	public class BinaryOp : VBaseElement
	{
		public static readonly BinaryOp PLUS = new BinaryOp ("+", (x,y) => x+y);
		public static readonly BinaryOp MINUS = new BinaryOp ("-", (x,y) => x-y);
		public static readonly BinaryOp MULTIPLE = new BinaryOp ("*", (x,y) => x*y);
		public static readonly BinaryOp DIVIDE = new BinaryOp ("/", (x,y) => x/y);

		private string symbol;
		private Func<decimal, decimal, decimal> op;

		public BinaryOp (string symbol, Func<decimal, decimal, decimal> op)
		{
			this.op = op;
			this.symbol = symbol;
			
			color = Color.GreenYellow;
			Size = new Size (100, 50);

			var symbolLabel = new CustomLabel (symbol, color);
			symbolLabel.Location = new Point (15, 0);
			Controls.Add (symbolLabel);
		}

		protected override void OnPaint (PaintEventArgs e)
		{
			{
				var rectangle = new RectangleF (new PointF (0, 0), new SizeF (Size.Width, Const.EXPR_HIGHT));
				e.Graphics.FillRectangle (new SolidBrush (color), rectangle);
			}
			{
				var rectangle = new RectangleF (new PointF (5, 5), new SizeF (5, 5));
				e.Graphics.FillRectangle (new SolidBrush (Color.White), rectangle);
			}
			{
				var rectangle = new RectangleF (new PointF (35, 5), new SizeF (5, 5));
				e.Graphics.FillRectangle (new SolidBrush (Color.White), rectangle);
			}
		}
	}
}

