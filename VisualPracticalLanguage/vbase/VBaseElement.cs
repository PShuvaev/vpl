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
			ResizeRedraw = true;
			color = Color.Green;
		}
		
		protected virtual void OnResize()
		{
		}

		public abstract bool TryPutElement (ArgumentPlaceholder p, VBaseElement el);
	}
}