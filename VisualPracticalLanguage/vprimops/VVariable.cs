using System;
using System.Drawing;

namespace VisualPracticalLanguage
{
	public class VVariable : VExpression
	{
		public VVariable ()
		{
			this.str = str;

			color = Color.Orange;
			Size = new Size (100, Const.HEADER_SIZE);
			var lbl = new CustomLabel (str, color);

			Controls.Add (lbl);
		}

		
		private string str;


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

		public override void UpdateSize ()
		{
		}
	}
}

