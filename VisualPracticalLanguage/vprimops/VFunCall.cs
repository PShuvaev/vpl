using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class VFunCall : DraggableControl, IPlaceholderContainer, IFunctionCall
	{
		
		private string name;
		private IList<ArgumentPlaceholder> placeholders;
		private IList<DraggableControl> controlArguments;
		private CustomLabel funName;

		private IFunctionDeclaration functionDeclaration;

		// отступ от границ компонента
		private const int BorderPadding = 10;

		// промежуток между операцией и аргументом
		private const int OpArgPadding = 5;

		public VFunCall (IFunctionDeclaration decl)
		{
			this.functionDeclaration = decl;
			this.name = decl.name;

			funName = new CustomLabel (name, BackColor);
			funName.Parent = this;

			Size = new Size (200, 200);
			controlArguments = new List<DraggableControl> ();
			placeholders = new List<ArgumentPlaceholder> ();

			for (int i = 0; i < decl.argumentsCount; i++) {
				controlArguments.Add (null);
				placeholders.Add (
					new ArgumentPlaceholder (this){
					Parent = this
				});
			}

			BackColor = Color.Green;

			UpdateSize ();
		}

		public IFunctionDeclaration function {
			get { return functionDeclaration; }
		}

		public IList<IExpression> arguments {
			get { return controlArguments.Cast<IExpression> ().ToList (); }
		}

		public void UpdateSize(){
			var width = BorderPadding + funName.Size.Width + OpArgPadding;

			var controls = controlArguments.Zip<Control,Control,Control> (placeholders, (arg, pl) => (Control)arg ?? (Control)pl).ToList();

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
			return el is IExpression;
		}

		public bool PutElement (ArgumentPlaceholder p, DraggableControl el)
		{
			if (!CanPutElement (p, el))
				return false;
			
			var pos = placeholders.IndexOf (p);

			controlArguments[pos] = el;

			el.Parent = this;
			el.EParent = this;

			Hide (p);

			return true;
		}


		public void OnChildDisconnect (DraggableControl c){
			var pos = controlArguments.IndexOf (c);
			controlArguments[pos] = null;
		}

	}
}

