using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPracticalLanguage
{
	public class VBinaryOp : VBaseElement
	{
		public static readonly VBinaryOp PLUS = new VBinaryOp ("+", (x,y) => x+y);
		public static readonly VBinaryOp MINUS = new VBinaryOp ("-", (x,y) => x-y);
		public static readonly VBinaryOp MULTIPLE = new VBinaryOp ("*", (x,y) => x*y);
		public static readonly VBinaryOp DIVIDE = new VBinaryOp ("/", (x,y) => x/y);

		private string symbol;
		private Func<decimal, decimal, decimal> op;


		private RectangleF firstArgArea { 
			get { 
				return new RectangleF (new PointF (5, 2), Const.ArgSize);
			}
		}

		private RectangleF secondArgArea { 
			get { 
				return new RectangleF (new PointF (35, 2), Const.ArgSize);
			}
		}

		public VBinaryOp (string symbol, Func<decimal, decimal, decimal> op)
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
				e.Graphics.FillRectangle (new SolidBrush (Color.White), firstArgArea);
			}
			{
				e.Graphics.FillRectangle (new SolidBrush (Color.White), secondArgArea);
			}
		}
		
		protected override bool TakeElement (Point p, VBaseElement el)
		{
			return el is VExpression;//; && location;
		}
	}
}

