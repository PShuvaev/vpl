using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class VFunCall : DraggableControl, IFunctionCall, IFunCallStatement, IPlaceholderContainer, IVariableRefsHolder
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

		public VFunCall (IFunctionCall functionCall)
		{
			this.functionDeclaration = functionCall.function;
			this.name = functionCall.function.name;

			funName = new CustomLabel (name, BackColor);
			funName.Parent = this;

			int argCount = functionCall.arguments.Count;

			controlArguments = new List<DraggableControl> (new DraggableControl[argCount]);
			placeholders = new List<ArgumentPlaceholder> (new ArgumentPlaceholder[argCount]);

			for (int i = 0; i < argCount; i++) {
				placeholders[i]= new ArgumentPlaceholder (this){ Parent = this };
				var elArg = VElementBuilder.Create (functionCall.arguments[i]);
				SetArgument (i, elArg);
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

		public IFunctionCall functionCall {
			get {return new FunctionCall { function = function, arguments = arguments }; }
		}

		public IResizable ResizableParent { get{ return EParent; } }

		public void UpdateSize(){
			var width = BorderPadding + funName.Size.Width + OpArgPadding;

			var controls = controlArguments.Zip<Control,Control,Control> (placeholders, (arg, pl) => (Control)arg ?? (Control)pl).ToList();

			var height = 2*BorderPadding + (controls.Empty() ? 0 : controls.Max (c => c.Height));

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

		private void SetArgument(int pos, DraggableControl el){
			controlArguments[pos] = el;
			if (el == null)
				return;
			el.Parent = this;
			el.EParent = this;
			Hide (placeholders[pos]);
		}

		public bool PutElement (ArgumentPlaceholder p, DraggableControl el)
		{
			if (!CanPutElement (p, el))
				return false;
			var pos = placeholders.IndexOf (p);
			SetArgument (pos, el);
			return true;
		}


		public void OnChildDisconnect (DraggableControl c){
			var pos = controlArguments.IndexOf (c);
			controlArguments[pos] = null;
		}
		
		public IList<VVariableRef> refs {
			get {
				return controlArguments.Select (x => x as IVariableRefsHolder)
					.Where (x => x != null).SelectMany (x => x.refs).ToList();
			}
		}
	}
}

