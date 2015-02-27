using System;
using System.Windows.Forms;

namespace VisualPracticalLanguage
{
	public interface IPlaceholderContainer
	{
		IPlaceholderContainer EParent{ get; set; }
		bool CanPutElement (ArgumentPlaceholder p, DraggableControl el);
		bool PutElement (ArgumentPlaceholder p, DraggableControl el);
		void OnChildDisconnect (DraggableControl c);
		void UpdateSize ();
	}

	public static class IPlaceholderContainerExtensions{
		public static void UpdateRecSize (this IPlaceholderContainer container){
			container.UpdateSize ();
			if (container.EParent != null) {
				container.EParent.UpdateRecSize ();
			}
		}
	}
}

