using System;
using System.Windows.Forms;
using System.Drawing;

namespace VisualPracticalLanguage
{
	public class VSetVariable : VExpression
	{
		private CustomLabel name;
		private CustomLabel eqLabel;

		
		private VExpression arg;

		private ArgumentPlaceholder argPlaceHolder;

		
		// отступ от границ компонента
		private const int BorderPadding = 10;

		// промежуток между операцией и аргументом
		private const int OpArgPadding = 5;

		public VSetVariable (string vname)
		{
			BackColor = Color.Yellow;
			color = Color.Yellow;
			Size = new Size (100, Const.EXPR_HEIGHT);
			name = new CustomLabel (vname, color);
			
			argPlaceHolder = new ArgumentPlaceholder (this);
			argPlaceHolder.Parent = this;
			Controls.Add (name);
			eqLabel = new CustomLabel (" = ", color);
			Controls.Add (eqLabel);
			name.TextChanged += delegate {
				var nameLoc = name.Location;
				eqLabel.Location = new Point(nameLoc.X + name.Width + 10, 0);
			};

			UpdateSize ();
		}

		public override bool CanPutElement (ArgumentPlaceholder p, VBaseElement el)
		{
			if (!(el is VExpression))
				return false;

			if (p == argPlaceHolder && arg == null) {
				return true;
			}

			return false;
		}

		public override bool PutElement (ArgumentPlaceholder p, VBaseElement el)
		{
			if (p == argPlaceHolder && arg == null) {
				arg = (VExpression)el;
				arg.Parent = this;
				((DraggableControl)arg).EParent = this;

				Hide (argPlaceHolder);

				UpdateSize ();
				return true;
			}

			return false;
		}
		
		public override void OnChildDisconnect (DraggableControl c){
			if (arg == c) {
				arg = null;
			}
			UpdateSize ();
		}
		
		public override void UpdateSize (){
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

