using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPracticalLanguage
{
	public class VBinaryOp : VBaseElement
	{
		public static readonly VBinaryOp PLUS = new VBinaryOp ("+");
		public static readonly VBinaryOp MINUS = new VBinaryOp ("-");
		public static readonly VBinaryOp MULTIPLE = new VBinaryOp ("*");
		public static readonly VBinaryOp DIVIDE = new VBinaryOp ("/");

		private string symbol;
		private Func<decimal, decimal, decimal> op;

		CustomLabel OpSymbol;

		// отступ от границ компонента
		private const int BorderPadding = 15;

		// промежуток между операцией и аргументом
		private const int OpArgPadding = 5;


		private VExpression firstArg;
		private VExpression secondArg;

		private ArgumentPlaceholder firstArgPlaceHolder;
		private ArgumentPlaceholder secondArgPlaceHolder;

		public VBinaryOp (string symbol) : base()
		{
			this.symbol = symbol;
			
			color = Color.GreenYellow;
			
			firstArgPlaceHolder = new ArgumentPlaceholder (this);
			secondArgPlaceHolder = new ArgumentPlaceholder (this);

			Controls.Add (firstArgPlaceHolder);
			Controls.Add (secondArgPlaceHolder);

			OpSymbol = new CustomLabel (symbol, color);
			Controls.Add (OpSymbol);

			UpdateSize ();

			BackColor = Color.Green;
		}

		private void UpdateSize(){
			Control fArg = (Control)firstArg ?? firstArgPlaceHolder;
			Control sArg = (Control)secondArg ?? secondArgPlaceHolder;

			var argHeight = Math.Max (fArg.With(_ => _.Height, 0), sArg.With(_ => _.Height, 0));
			var height = 2 * BorderPadding + argHeight;

			var argsWidth = fArg.With(_ => _.Width, 0) + sArg.With(_ => _.Width, 0);
			var width = 2 * BorderPadding + 2 * OpArgPadding + argsWidth + OpSymbol.Width;

			Size = new Size (width, height);
			
			fArg.Location = new Point (BorderPadding, BorderPadding);
			OpSymbol.Location = new Point (fArg.Location.X + fArg.Width + OpArgPadding, (Size.Height-OpSymbol.Height) / 2);
			sArg.Location = new Point (Size.Width - BorderPadding - sArg.Width, BorderPadding);
		}

		/// <summary>
		/// Вызывается плейсхолдером p при попытке заменить плейсходер выражением el
		/// </summary>
		public override bool TryPutElement (ArgumentPlaceholder p, VBaseElement el)
		{
			if (!(el is VExpression))
				return false;
			
			if (p == firstArgPlaceHolder && firstArg == null) {
				firstArg = (VExpression)el;
				firstArgPlaceHolder.Dispose ();
				firstArgPlaceHolder = null;
				UpdateSize ();
				return true;
			}

			if (p == firstArgPlaceHolder && firstArg == null) {
				secondArg = (VExpression)el;
				secondArgPlaceHolder.Dispose ();
				secondArgPlaceHolder = null;
				UpdateSize ();
				return true;
			}

			return false;
		}
	}
}

