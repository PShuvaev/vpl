using System;
using System.Windows.Forms;

namespace VisualPracticalLanguage
{
	public interface IPlaceholderContainer : IResizable
	{
		IPlaceholderContainer EParent{ get; set; }
		bool CanPutElement (ArgumentPlaceholder p, DraggableControl el);
		bool PutElement (ArgumentPlaceholder p, DraggableControl el);
		void OnChildDisconnect (DraggableControl c);
	}

	public static class IPlaceholderContainerExtensions{
		public static void UpdateRecSize (this IResizable container){
			container.UpdateSize ();
			if (container.ResizableParent != null) {
				container.ResizableParent.UpdateRecSize ();
			}
		}
	}
}

