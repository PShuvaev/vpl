using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class VFunction : DraggableControl, IFunctionDefinition, IPlaceholderContainer, IVariableRefsHolder
	{
		public string fnamespace { get; set; }
		public string fclass { get; set; }
		public string name { get; set; }

		public bool isBinOperation { get; set;}
		public bool isReturnVoid { get; set;}

		private IList<DraggableControl> controlStatements;
		private IList<ArgumentPlaceholder> placeholders;
		public IList<VVariable> controlArguments { get; set; }
		private CustomLabel funName;
		private Button addArgBtn;

		// отступ от границ компонента
		private const int BorderPadding = 10;

		// промежуток между операцией и аргументом
		private const int OpArgPadding = 5;

		public VFunction(IFunctionDefinition funDef) : this(funDef.name){
			foreach (var arg in funDef.arguments) {
				AddArgument (arg.varName);
			}
			foreach (var statement in funDef.statements) {
				var velement = VElementBuilder.Create (statement);
				AddExpression (velement);
			}

			foreach (var @ref in refs) {
				var variable = controlArguments.FirstOrDefault(x => x.varName == @ref.markInitVarName);
				variable.AttachVarRef (@ref);
			}
		}

		public VFunction (string name)
		{
			this.name = name;

			BackColor = Color.LightBlue;

			funName = new CustomLabel (name, Color.Black){
				Parent = this,
				Location = new Point (BorderPadding, BorderPadding)
			};

			controlStatements = new List<DraggableControl> ();
			controlArguments = new List<VVariable> ();
			placeholders = new List<ArgumentPlaceholder> { 
				new ArgumentPlaceholder(this){
					Parent = this
				}
			};

			addArgBtn = new Button {
				Text = "+",
				Parent = this,
				Size = new Size(20, 20)
			};
			addArgBtn.Click += (object sender, EventArgs e) => {
				var argName = "arg" + controlArguments.Count;
				AddArgument(argName);
			};

			UpdateSize ();
		}

		public IList<IVariable> variables { 
			get {
				return new List<IVariable> ();
			}
		}
		public IList<IStatement> statements { 
			get {return controlStatements.Cast<IStatement> ().ToList ();}
		}

		public IList<IVariable> arguments {
			get {
				return controlArguments.Cast<IVariable> ().ToList ();
			}
		}

		public int argumentsCount {
			get { return controlArguments.Count; }
		}

		public void AddArgument(string arg)
		{
			var label = new VVariable (arg, this);
			controlArguments.Add (label);

			Controls.Add (label);
			UpdateSize ();
		}

		public void RemoveArgument(VVariable arg)
		{
			if (arg.VariableRefs.Count == 0) {
				controlArguments.Remove (arg);
				Controls.Remove (arg);
				UpdateSize ();
			}
		}

		public IResizable ResizableParent { get{ return EParent; } }

		public void UpdateSize(){
			var argumentsWidth = controlArguments.Sum (x => x.Size.Width) + OpArgPadding * (arguments.Count-1);

			{ // расположение аргументов + кнопки добавления аргумента
				var startArgsX = BorderPadding + funName.Size.Width + OpArgPadding;
				foreach (var arg in controlArguments) {
					arg.Location = new Point (startArgsX, BorderPadding);
					startArgsX += arg.Size.Width + OpArgPadding;
				}

				addArgBtn.Location = new Point (startArgsX, BorderPadding);
			}


			var funDeclWidth = 2 * BorderPadding + funName.Size.Width + OpArgPadding + argumentsWidth + OpArgPadding + addArgBtn.Width;

			var bodyExprsWidth = controlStatements.Aggregate (0, (acc, e) => Math.Max (acc, e.Size.Width));
			var width = 2 * BorderPadding + funDeclWidth + bodyExprsWidth;

			var declHeight = funName.Size.Height;

			var height = 2 * BorderPadding + funName.Size.Height;

			foreach (var el in placeholders.Intercalate<Control>(controlStatements)) {
				el.Location = new Point(el.Location.X, height);
				height += el.Size.Height;
			}

			height += BorderPadding;

			Size = new Size (width, height);
		}

		protected override void OnPaint (PaintEventArgs e)
		{
			{
				var rectangle = new RectangleF (new PointF (0, 0), new SizeF (Size.Width, Const.HEADER_SIZE));
				e.Graphics.FillRectangle (new SolidBrush (BackColor), rectangle);
				
			}
			{
				var rectangle = new RectangleF (new PointF (0, Const.HEADER_SIZE), new SizeF (Const.TAB_SIZE, Size.Height-Const.PALLET_HEIGHT));
				e.Graphics.FillRectangle (new SolidBrush (BackColor), rectangle);
			}

			{
				var rectangle = new RectangleF (new PointF (0, Size.Height-Const.PALLET_HEIGHT), new SizeF (Size.Width/2, Const.PALLET_HEIGHT));
				e.Graphics.FillRectangle (new SolidBrush (BackColor), rectangle);
			}
		}

		public void AddExpression(DraggableControl expr){
			expr.Parent = this;
			expr.EParent = this;

			if (!controlStatements.Any ()) {
				expr.Location = new Point (Const.TAB_SIZE, Const.HEADER_SIZE);
			} else {
				var lastExpr = controlStatements.Last ();
				expr.Location = new Point (Const.TAB_SIZE, lastExpr.Location.Y + lastExpr.Size.Height + 2);
			}
			
			controlStatements.Add (expr);
			placeholders.Add (
				new ArgumentPlaceholder(this).With(_ => {
				_.Parent = this;
			}));
			this.UpdateRecSize ();
		}
		
		public bool CanPutElement (ArgumentPlaceholder p, DraggableControl el)
		{
			return el is IStatement && placeholders.Contains(p);
		}

		public bool PutElement (ArgumentPlaceholder p, DraggableControl el)
		{
			if (!CanPutElement (p, el))
				return false;

			var pos = placeholders.IndexOf (p);
			
			controlStatements.Insert (pos, el);

			el.Parent = this;
			el.EParent = this;

			placeholders.Insert (pos, new ArgumentPlaceholder(this){
				Parent = this
			});

			return true;
		}
		
		public void OnChildDisconnect (DraggableControl c){
			var pos = controlStatements.IndexOf (c);
			Controls.Remove (controlStatements [pos]);
			Controls.Remove (placeholders [pos]);
			controlStatements.RemoveAt (pos);
			placeholders.RemoveAt (pos);
		}
		
		public IList<VVariableRef> refs {
			get {
				return controlStatements.Select (x => x as IVariableRefsHolder)
					.Where (x => x != null).SelectMany (x => x.refs).ToList();
			}
		}
	}
}

