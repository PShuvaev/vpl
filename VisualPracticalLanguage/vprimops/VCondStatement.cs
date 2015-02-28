using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace VisualPracticalLanguage
{
	public class VCondStatement : DraggableControl, IPlaceholderContainer
	{
		private DraggableControl condArg;
		private ArgumentPlaceholder condPlaceholder;
		private IList<DraggableControl> expressions;
		private IList<ArgumentPlaceholder> placeholders;

		private CustomLabel condTypeLabel;

		// отступ от границ компонента
		private const int BorderPadding = 10;

		// промежуток между операцией и аргументом
		private const int OpArgPadding = 5;



		public VCondStatement (string condType)
		{
			condTypeLabel = new CustomLabel (condType, Color.Black){
				Parent = this,
				Location = new Point(5, 5)
			};


			expressions = new List<DraggableControl> ();
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

		public void UpdateSize(){
			var condControl = (Control)condArg ?? condPlaceholder;;
			var declwidth = condTypeLabel.Size.Width + 2*OpArgPadding +condControl.Width;
			var bodyexprWidth = expressions.Aggregate (0, (acc, e) => Math.Max (acc, e.Size.Width));
			var width = 2 * BorderPadding + declwidth + bodyexprWidth;

			condControl.Location = new Point (condTypeLabel.Size.Width + 2*OpArgPadding, 5);

			var height = BorderPadding + Math.Max(condTypeLabel.Size.Height, condControl.Height);

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
			this.UpdateRecSize ();
		}

		public bool CanPutElement (ArgumentPlaceholder p, DraggableControl el)
		{
			return el is DraggableControl;
		}

		public bool PutElement (ArgumentPlaceholder p, DraggableControl el)
		{
			if (p == condPlaceholder) {
				condArg = el;
				condArg.Parent = this;
				condArg.EParent = this;

				Hide (condPlaceholder);
				return true;
			}

			var pos = placeholders.IndexOf (p);
			expressions.Insert (pos, el);

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
				var pos = expressions.IndexOf (c);
				Controls.Remove (placeholders [pos]);
				expressions.RemoveAt (pos);
				placeholders.RemoveAt (pos);
			}

			Controls.Remove (c);
		}
	}
}

