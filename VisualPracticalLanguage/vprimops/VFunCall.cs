using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace VisualPracticalLanguage
{
	public class VFunCall : DraggableControl, IPlaceholderContainer
	{
		
		private string name;
		private IList<ArgumentPlaceholder> placeholders;
		private IList<DraggableControl> arguments;
		private CustomLabel funName;


		// отступ от границ компонента
		private const int BorderPadding = 10;

		// промежуток между операцией и аргументом
		private const int OpArgPadding = 5;

		public VFunCall (string name, int argsCount)
		{
			this.name = name;

			funName = new CustomLabel (name, BackColor);
			funName.Parent = this;

			Size = new Size (200, 200);
			arguments = new List<DraggableControl> ();
			placeholders = new List<ArgumentPlaceholder> ();

			for (int i = 0; i < argsCount; i++) {
				arguments.Add (null);
				placeholders.Add (
					new ArgumentPlaceholder (this).With (_ => {
					_.Parent = this;
				}));
			}

			BackColor = Color.Green;

			UpdateSize ();
		}

		public void UpdateSize(){
			var width = BorderPadding + funName.Size.Width + OpArgPadding;

			var controls = arguments.Zip<Control,Control,Control> (placeholders, (arg, pl) => (Control)arg ?? (Control)pl).ToList();

			var height = controls.Max (c => c.Height) + 2*BorderPadding;

			foreach (var control in controls) {
				var cHeight = (height - control.Height) / 2;
				control.Location = new Point(width, cHeight);
				width += control.Width + OpArgPadding;
			}

			funName.Location = new Point (BorderPadding, (height - funName.Height) / 2);

			width += BorderPadding;

			Size = new Size (width, height);
		}


		public bool CanPutElement (ArgumentPlaceholder p, DraggableControl el)
		{
			return el is DraggableControl;
		}

		public bool PutElement (ArgumentPlaceholder p, DraggableControl el)
		{
			var pos = placeholders.IndexOf (p);

			arguments[pos] = el;

			el.Parent = this;
			el.EParent = this;

			Hide (p);

			return true;
		}


		public void OnChildDisconnect (DraggableControl c){
			var pos = arguments.IndexOf (c);
			arguments[pos] = null;
		}

	}
}

