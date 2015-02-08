using System;
using System.Windows.Forms;
using System.Drawing;

namespace VisualPracticalLanguage
{
	public abstract class VBaseElement : DraggableControl
	{
		public Color color { get; set; }

		public VBaseElement ()
		{
			color = Color.Green;
		}
		
		protected virtual void OnResize()
		{
		}


		/// <summary>
		/// Принимает ли выражение, дабы вложить его в себя
		/// </summary>
		protected abstract bool TakeElement (Point p, VBaseElement el);
	}
}