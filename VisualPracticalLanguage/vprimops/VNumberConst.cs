using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPracticalLanguage
{
	public class VNumberConst : VExpression
	{
		private decimal number;

		public VNumberConst (decimal number)
		{
			this.number = number;
			
			color = Color.Blue;
			Size = new Size (100, Const.HEADER_SIZE);
			var lbl = new CustomLabel (number.ToString(), color);

			Controls.Add (lbl);
		}
		
		protected override void OnPaint (PaintEventArgs e)
		{
			{
				var rectangle = new RectangleF (new PointF (0, 0), new SizeF (Size.Width, Const.EXPR_HIGHT));
				e.Graphics.FillRectangle (new SolidBrush (color), rectangle);
			}
		}
		
		public override bool PutElement (ArgumentPlaceholder p, VBaseElement el)
		{
			return false;
		}

		public override bool CanPutElement (ArgumentPlaceholder p, VBaseElement el)
		{
			return false;
		}

		
		public override void OnChildDisconnect (DraggableControl c){
		}

		public override void UpdateSize (){
		}
	}
}

