using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace VisualPracticalLanguage
{
	public class VFunction : VBaseElement
	{
		private string name;
		private IList<VBaseElement> expressions;
		private IList<ArgumentPlaceholder> placeholders;
		private IList<CustomLabel> arguments;
		private CustomLabel funName;

		

		// отступ от границ компонента
		private const int BorderPadding = 10;

		// промежуток между операцией и аргументом
		private const int OpArgPadding = 5;

		public VFunction (string name)
		{
			this.name = name;

			funName = new CustomLabel (name, color);
			funName.Parent = this;

			Size = new Size (200, 200);
			expressions = new List<VBaseElement> ();
			arguments = new List<CustomLabel> ();
			placeholders = new List<ArgumentPlaceholder> { 
				new ArgumentPlaceholder(this).With(_ => {
					_.Parent = this;
				})
			};

			UpdateSize ();
		}

		public void AddArgument(string arg)
		{
			var label = new CustomLabel (arg, color);

			if (!arguments.Any ()) {
				label.Location = new Point (funName.Location.X + funName.Size.Width + 10, 0);
			} else {
				var lastArg = arguments.Last ();
				label.Location = new Point (lastArg.Location.X + lastArg.Size.Width + 10, 0);
			}

			arguments.Add (label);

			Controls.Add (label);
		}


		public override void UpdateSize(){
			var declwidth = funName.Size.Width + arguments.Sum (x => x.Size.Width) + OpArgPadding * (arguments.Count - 1);
			var bodyexprWidth = expressions.Aggregate (0, (acc, e) => Math.Max (acc, e.Size.Width));
			var width = 2 * BorderPadding + declwidth + bodyexprWidth;

			var declHeight = funName.Size.Height;
			var bodyexprHeight = expressions.Sum (x => x.Size.Height);

			var height = 2 * BorderPadding + funName.Size.Height;

			foreach (var el in placeholders.Intercalate<Control>(expressions)) {
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
				e.Graphics.FillRectangle (new SolidBrush (color), rectangle);
				
			}
			{
				var rectangle = new RectangleF (new PointF (0, Const.HEADER_SIZE), new SizeF (Const.TAB_SIZE, Size.Height-Const.PALLET_HEIGHT));
				e.Graphics.FillRectangle (new SolidBrush (color), rectangle);
			}

			{
				var rectangle = new RectangleF (new PointF (0, Size.Height-Const.PALLET_HEIGHT), new SizeF (Size.Width/2, Const.PALLET_HEIGHT));
				e.Graphics.FillRectangle (new SolidBrush (color), rectangle);
			}
		}

		public void AddExpression(VBaseElement expr){
			expr.Parent = this;
			expr.EParent = this;

			if (!expressions.Any ()) {
				expr.Location = new Point (Const.TAB_SIZE, Const.HEADER_SIZE);
			} else {
				var lastExpr = expressions.Last ();
				expr.Location = new Point (Const.TAB_SIZE, lastExpr.Location.Y + lastExpr.Size.Height + 2);
			}
			
			expressions.Add (expr);
			placeholders.Add (
				new ArgumentPlaceholder(this).With(_ => {
				_.Parent = this;
			}));
			UpdateSize ();
		}
		
		public override bool CanPutElement (ArgumentPlaceholder p, VBaseElement el)
		{
			return el is VExpression;
		}

		public override bool PutElement (ArgumentPlaceholder p, VBaseElement el)
		{
			var pos = placeholders.IndexOf (p);
			
			Logger.Log ("put  " + el + " at " + pos);

			expressions.Remove (el);

			expressions.Insert (pos, el);

			el.Parent = this;
			el.EParent = this;

			placeholders.Insert (pos,
				new ArgumentPlaceholder(this).With(_ => {
				_.Parent = this;
			}));
			UpdateSize ();

			return true;
		}

		
		public override void OnChildDisconnect (DraggableControl c){
			Logger.Log ("disconnect " + c);
			var pos = expressions.IndexOf ((VBaseElement)c);
			expressions.RemoveAt (pos);
			placeholders.RemoveAt (pos);
		}

	}
}

