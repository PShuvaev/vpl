using System;
using System.Windows.Forms;
using System.Drawing;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class VSetVariable : DraggableControl, IPlaceholderContainer, ISetVariableStatement
	{
		private CustomLabel name;
		private CustomLabel eqLabel;

		
		private DraggableControl arg;

		private ArgumentPlaceholder argPlaceHolder;

		
		// отступ от границ компонента
		private const int BorderPadding = 10;

		// промежуток между операцией и аргументом
		private const int OpArgPadding = 5;

		public VSetVariable (string vname)
		{
			BackColor = Color.Yellow;
			BackColor = Color.Yellow;
			Size = new Size (100, Const.EXPR_HEIGHT);
			name = new CustomLabel (vname, BackColor);
			
			argPlaceHolder = new ArgumentPlaceholder (this);
			argPlaceHolder.Parent = this;
			Controls.Add (name);
			eqLabel = new CustomLabel (" = ", BackColor);
			Controls.Add (eqLabel);
			name.TextChanged += delegate {
				var nameLoc = name.Location;
				eqLabel.Location = new Point(nameLoc.X + name.Width + 10, 0);
			};

			UpdateSize ();
		}

		public IVariable variable {
			get { return new Variable { varName = name.Text }; }
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
			var width = 2 * BorderPadding + 2*OpArgPadding + argsWidth + name.Width+eqLabel.Size.Width;

			Size = new Size (width, height);


			name.Location = new Point (BorderPadding, BorderPadding);
			eqLabel.Location = new Point (name.Location.X + name.Size.Width + OpArgPadding, BorderPadding);

			fArg.Location = new Point (eqLabel.Location.X + eqLabel.Size.Width + OpArgPadding, BorderPadding);
		}
	}
}

