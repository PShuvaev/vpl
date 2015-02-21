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
		
		public abstract bool CanPutElement (ArgumentPlaceholder p, VBaseElement el);
		public abstract bool PutElement (ArgumentPlaceholder p, VBaseElement el);
	}
}