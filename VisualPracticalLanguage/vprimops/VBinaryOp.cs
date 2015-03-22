using System;
using System.Drawing;
using System.Windows.Forms;
using VisualPracticalLanguage.Interface;
using System.Collections.Generic;

namespace VisualPracticalLanguage
{
	public class VBinaryOp : DraggableControl, IPlaceholderContainer, IFunctionCall
	{

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

		private IFunctionDeclaration functionDeclaration;

		public VBinaryOp (IFunctionDeclaration functionDeclaration) : base()
		{
			this.functionDeclaration = functionDeclaration;
			this.symbol = functionDeclaration.name;
			
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

		public IFunctionDeclaration function {
			get { return functionDeclaration; }
		}

		public IList<IExpression> arguments {
			get { return new List<IExpression> { firstArg as IExpression, secondArg as IExpression }; }
		}

		public void UpdateSize(){
			Control fArg = (Control)firstArg ?? firstArgPlaceHolder;
			Control sArg = (Control)secondArg ?? secondArgPlaceHolder;

			var argHeight = Math.Max (fArg.OrDef(_ => _.Height, 0), sArg.OrDef(_ => _.Height, 0));
			var height = 2 * BorderPadding + argHeight;

			var argsWidth = fArg.OrDef(_ => _.Width, 0) + sArg.OrDef(_ => _.Width, 0);
			var width = 2 * BorderPadding + 2 * OpArgPadding + argsWidth + OpSymbol.Width;

			Size = new Size (width, height);
			
			fArg.Location = new Point (BorderPadding, (height - fArg.Height) / 2);
			OpSymbol.Location = new Point (fArg.Location.X + fArg.Width + OpArgPadding, (height-OpSymbol.Height) / 2);
			sArg.Location = new Point (Size.Width - BorderPadding - sArg.Width, (height - sArg.Height) / 2);
		}

		
		public bool CanPutElement (ArgumentPlaceholder p, DraggableControl el)
		{
			if (!(el is IExpression))
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
			if (!CanPutElement(p, el))
				return false;
			
			if (p == firstArgPlaceHolder && firstArg == null) {
				firstArg = el;
				firstArg.Parent = this;
				firstArg.EParent = this;

				Hide (firstArgPlaceHolder);
				return true;
			}
			
			if (p == secondArgPlaceHolder && secondArg == null) {
				secondArg = el;
				secondArg.Parent = this;
				secondArg.EParent = this;

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
			
			Controls.Remove (c);
			UpdateSize ();
		}

	}
}

