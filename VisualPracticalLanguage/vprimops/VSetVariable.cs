using System;
using System.Windows.Forms;
using System.Drawing;

namespace VisualPracticalLanguage
{
	public class VSetVariable : DraggableControl, IPlaceholderContainer
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

		public bool CanPutElement (ArgumentPlaceholder p, DraggableControl el)
		{
			if (!(el is DraggableControl))
				return false;

			if (p == argPlaceHolder && arg == null) {
				return true;
			}

			return false;
		}

		public bool PutElement (ArgumentPlaceholder p, DraggableControl el)
		{
			if (p == argPlaceHolder && arg == null) {
				arg = el;
				arg.Parent = this;
				((DraggableControl)arg).EParent = this;

				Hide (argPlaceHolder);
				return true;
			}

			return false;
		}
		
		public void OnChildDisconnect (DraggableControl c){
			if (arg == c) {
				arg = null;
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

