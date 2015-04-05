using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class VCondStatement : DraggableControl, ICondStatement, IPlaceholderContainer, IVariableRefsHolder
	{
		private DraggableControl condArg;
		private ArgumentPlaceholder condPlaceholder;
		private IList<DraggableControl> controlStatements;
		private IList<ArgumentPlaceholder> placeholders;

		private CustomLabel condTypeLabel;

		// отступ от границ компонента
		private const int BorderPadding = 10;

		// промежуток между операцией и аргументом
		private const int OpArgPadding = 5;

		
		public VCondStatement (string condType, ICondStatement condStatement) : this(condType)
		{
			foreach (var statement in condStatement.statements) {
				AddExpression (VElementBuilder.Create (statement));
			}

			SetCondElement (VElementBuilder.Create (condStatement.condition));
			UpdateSize ();
		}

		public VCondStatement (string condType)
		{
			condTypeLabel = new CustomLabel (condType, Color.Black){
				Parent = this,
				Location = new Point(5, 5)
			};

			controlStatements = new List<DraggableControl> ();
			placeholders = new List<ArgumentPlaceholder> { 
				new ArgumentPlaceholder(this).With(_ => {
					_.Parent = this;
				})
			};

			condPlaceholder = new ArgumentPlaceholder (this) {
				Parent = this,
				Location = new Point(25, 5)
			};

			UpdateSize ();
		}

		public IList<IStatement> statements {
			get { return controlStatements.Cast<IStatement> ().ToList (); }
		}

		public IExpression condition {
			get { return condArg as IExpression; }
		}
		
		public IResizable ResizableParent { get{ return EParent; } }

		public void UpdateSize(){
			var condControl = (Control)condArg ?? condPlaceholder;;
			var declwidth = condTypeLabel.Size.Width + 2*OpArgPadding +condControl.Width;
			var bodyexprWidth = controlStatements.Aggregate (0, (acc, e) => Math.Max (acc, e.Size.Width));
			var width = 2 * BorderPadding + declwidth + bodyexprWidth;

			condControl.Location = new Point (condTypeLabel.Size.Width + 2*OpArgPadding, 5);

			var height = BorderPadding + Math.Max(condTypeLabel.Size.Height, condControl.Height);

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
			if (p == condPlaceholder)
				return el is IExpression;

			return el is IStatement && placeholders.IndexOf (p) >= 0;
		}

		private void SetCondElement(DraggableControl el){
			condArg = el;
			if (el == null)
				return;
			condArg.Parent = this;
			condArg.EParent = this;
			Hide (condPlaceholder);
		}

		public bool PutElement (ArgumentPlaceholder p, DraggableControl el)
		{
			if (!CanPutElement (p, el))
				return false;

			if (p == condPlaceholder) {
				SetCondElement (el);
				return true;
			}

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
			if (condArg == c) {
				condArg = null;
			} else {
				var pos = controlStatements.IndexOf (c);
				Controls.Remove (placeholders [pos]);
				controlStatements.RemoveAt (pos);
				placeholders.RemoveAt (pos);
			}

			Controls.Remove (c);
		}

		public IList<VVariableRef> refs {
			get {
				var condVars = (condArg as IVariableRefsHolder).OrDef(_ => _.refs).EmptyIfNull();
				var statementsVars = controlStatements.Select (x => x as IVariableRefsHolder)
					.Where (x => x != null).SelectMany (x => x.refs);
				return condVars.Concat (statementsVars).ToList ();
			}
		}
	}
}

