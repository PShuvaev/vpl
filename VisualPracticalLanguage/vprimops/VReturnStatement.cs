using System;
using System.Windows.Forms;
using System.Drawing;
using VisualPracticalLanguage.Interface;
using System.Collections.Generic;
using System.Linq;

namespace VisualPracticalLanguage
{
	public class VReturnStatement : DraggableControl, IReturnStatement, IPlaceholderContainer, IVariableRefsHolder
	{
		private CustomLabel retLabel;

		private DraggableControl arg;

		private ArgumentPlaceholder argPlaceHolder;


		// отступ от границ компонента
		private const int BorderPadding = 10;

		// промежуток между операцией и аргументом
		private const int OpArgPadding = 5;

		
		public VReturnStatement (IReturnStatement returnStatement) : this()
		{
			SetRetExpression (VElementBuilder.Create (returnStatement.expression));
			UpdateSize ();
		}

		public VReturnStatement ()
		{
			BackColor = Color.Yellow;
			BackColor = Color.Yellow;
			Size = new Size (100, Const.EXPR_HEIGHT);

			argPlaceHolder = new ArgumentPlaceholder (this);
			argPlaceHolder.Parent = this;
			retLabel = new CustomLabel ("вернуть ", BackColor);
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

		private void SetRetExpression(DraggableControl el){
			arg = el;
			if (el == null)
				return;
			arg.Parent = this;
			arg.EParent = this;
			Hide (argPlaceHolder);
		}

		public bool PutElement (ArgumentPlaceholder p, DraggableControl el)
		{
			if (!CanPutElement (p, el))
				return false;
			SetRetExpression (el);
			return true;
		}

		public void OnChildDisconnect (DraggableControl c){
			if (arg == c) {
				arg = null;
				Controls.Remove (c);
			}
		}
		
		public IResizable ResizableParent { get{ return EParent; } }

		public void UpdateSize (){
			Control fArg = (Control)arg ?? argPlaceHolder;

			var argHeight = fArg.OrDef(_ => _.Height, 0);
			var height = 2 * BorderPadding + argHeight;

			var argsWidth = fArg.OrDef(_ => _.Width, 0) ;
			var width = 2 * BorderPadding + 2*OpArgPadding + argsWidth +retLabel.Size.Width;

			Size = new Size (width, height);

			retLabel.Location = new Point (BorderPadding, BorderPadding);

			fArg.Location = new Point (retLabel.Location.X + retLabel.Size.Width + OpArgPadding, BorderPadding);
		}
		
		public IList<VVariableRef> refs {
			get {
				return (arg as IVariableRefsHolder).OrDef(x => x.refs).EmptyIfNull().ToList();
			}
		}
	}
}

