using System;
using System.Windows.Forms;
using System.Drawing;

namespace VisualPracticalLanguage
{
	public class Trasher : PictureBox, IPlaceholderContainer, IPlaceholder
	{
		private PictureBox box;
		private Image openTrash, closedTrash;

		public Trasher ()
		{
			openTrash = Image.FromFile ("./Resources/open_trash_icon.png");
			closedTrash = Image.FromFile ("./Resources/trash_icon.png");
			SetImage (closedTrash);
		}

		private void SetImage(Image img){
			Image = img;
			Width = img.Width;
			Height = img.Height;
		}

		public bool CanPutElement (ArgumentPlaceholder p, DraggableControl el)
		{
			return true;
		}

		public bool PutElement (ArgumentPlaceholder p, DraggableControl el)
		{
			return true;
		}

		public void OnChildDisconnect (DraggableControl c){
		}

		public void UpdateSize (){
		}

		public IPlaceholderContainer EParent { get; set; }

		public bool OnDrop (DraggableControl el)
		{
			el.Parent = null;
			ResetColor ();
			return true;
		}

		public void OnOver (DraggableControl c)
		{
			SetImage (openTrash);
		}

		public void OnLeave (DraggableControl c)
		{
			SetImage (closedTrash);
		}

		public void ResetColor ()
		{
			SetImage (closedTrash);
		}

		public IPlaceholderContainer parent { get { return this; } }
	}
}

