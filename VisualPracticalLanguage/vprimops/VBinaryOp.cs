using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPracticalLanguage
{
	public class VBinaryOp : DraggableControl, IPlaceholderContainer
	{
		public static readonly VBinaryOp PLUS = new VBinaryOp ("+");
		public static readonly VBinaryOp MINUS = new VBinaryOp ("-");
		public static readonly VBinaryOp MULTIPLE = new VBinaryOp ("*");
		public static readonly VBinaryOp DIVIDE = new VBinaryOp ("/");

		private string symbol;

		CustomLabel OpSymbol;

		// отступ от границ компонента
		private const int BorderPadding = 10;

		// промежуток между операцией и аргументом
		private const int OpArgPadding = 5;

		private DraggableControl firstArg;
		private DraggableControl secondArg;

		private ArgumentPlaceholder firstArgPlaceHolder;
		private ArgumentPlaceholder secondArgPlaceHolder;

		public VBinaryOp (string symbol) : base()
		{
			this.symbol = symbol;
			
			BackColor = Color.GreenYellow;
			
			firstArgPlaceHolder = new ArgumentPlaceholder (this);
			secondArgPlaceHolder = new ArgumentPlaceholder (this);

			firstArgPlaceHolder.Parent = this;
			secondArgPlaceHolder.Parent = this;

			OpSymbol = new CustomLabel (symbol, BackColor);
			OpSymbol.Parent = this;

			UpdateSize ();

			BackColor = Color.Green;
		}

		public void UpdateSize(){
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

		
		public bool CanPutElement (ArgumentPlaceholder p, DraggableControl el)
		{
			if (!(el is DraggableControl))
				return false;

			if (p == firstArgPlaceHolder && firstArg == null) {
				return true;
			}

			if (p == secondArgPlaceHolder && secondArg == null) {
				return true;
			}

			return false;
		}

		/// <summary>
		/// Вызывается плейсхолдером p при попытке заменить плейсходер выражением el
		/// </summary>
		public bool PutElement (ArgumentPlaceholder p, DraggableControl el)
		{
			if (!(el is DraggableControl))
				return false;
			
			if (p == firstArgPlaceHolder && firstArg == null) {
				firstArg = el;
				firstArg.Parent = this;
				((DraggableControl)firstArg).EParent = this;

				Hide (firstArgPlaceHolder);
				return true;
			}
			
			if (p == secondArgPlaceHolder && secondArg == null) {
				secondArg = el;
				secondArg.Parent = this;
				((DraggableControl)secondArg).EParent = this;

				Hide (secondArgPlaceHolder);
				return true;
			}

			return false;
		}

		
		public void OnChildDisconnect (DraggableControl c){
			if (firstArg == c) {
				firstArg = null;
			}
			if (secondArg == c) {
				secondArg = null;
			}
			UpdateSize ();
		}

	}
}

