using System;
using System.Windows.Forms;
using System.Drawing;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class VReturnStatement : DraggableControl, IPlaceholderContainer, IReturnStatement
	{
		private CustomLabel retLabel;

		private DraggableControl arg;

		private ArgumentPlaceholder argPlaceHolder;


		// отступ от границ компонента
		private const int BorderPadding = 10;

		// промежуток между операцией и аргументом
		private const int OpArgPadding = 5;

		public VReturnStatement ()
		{
			BackColor = Color.Yellow;
			BackColor = Color.Yellow;
			Size = new Size (100, Const.EXPR_HEIGHT);

			argPlaceHolder = new ArgumentPlaceholder (this);
			argPlaceHolder.Parent = this;
			retLabel = new CustomLabel ("return ", BackColor);
			Controls.Add (retLabel);

			UpdateSize ();
		}

		public IExpression expression {
			get { return arg as IExpression; }
		}

		public bool CanPutElement (ArgumentPlaceholder p, DraggableControl el)
		{
			return p == argPlaceHolder && el is IExpression;
		}

		public bool PutElement (ArgumentPlaceholder p, DraggableControl el)
		{
			if (!CanPutElement (p, el))
				return false;

			arg = el;
			arg.Parent = this;
			arg.EParent = this;

			Hide (argPlaceHolder);
			return true;
		}

		public void OnChildDisconnect (DraggableControl c){
			if (arg == c) {
				arg = null;
				Controls.Remove (c);
			}
		}

		public void UpdateSize (){
			Control fArg = (Control)arg ?? argPlaceHolder;

			var argHeight = fArg.With(_ => _.Height, 0);
			var height = 2 * BorderPadding + argHeight;

			var argsWidth = fArg.With(_ => _.Width, 0) ;
			var width = 2 * BorderPadding + 2*OpArgPadding + argsWidth +retLabel.Size.Width;

			Size = new Size (width, height);

			retLabel.Location = new Point (BorderPadding, BorderPadding);

			fArg.Location = new Point (retLabel.Location.X + retLabel.Size.Width + OpArgPadding, BorderPadding);
		}
	}
}

