using System;
using System.Windows.Forms;
using System.Drawing;

namespace VisualPracticalLanguage
{
	public class Trasher : PictureBox, IPlaceholderContainer, IPlaceholder
	{
		private PictureBox box;
		private Image openTrash, closedTrash;
		Action<DraggableControl> onElementRemove;

		public Trasher (Panel workPanel, Action<DraggableControl> onElementRemove)
		{
			this.onElementRemove = onElementRemove;
			openTrash = Image.FromFile ("./Resources/open_trash_icon.png");
			closedTrash = Image.FromFile ("./Resources/trash_icon.png");
			SetImage (closedTrash);

			Parent = workPanel;
			BringToFront();
			SetLocation(workPanel);
			workPanel.Resize += (object sender, EventArgs e) => {
				SetLocation(workPanel);
			};
		}

		void SetLocation(Panel workPanel){
			Location = new Point(workPanel.Width - Width - 10, workPanel.Height - Height - 10);
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

		public void OnChildDisconnect (DraggableControl c){}
		
		public IResizable ResizableParent { get{ return EParent; } }

		public void UpdateSize (){}

		public IPlaceholderContainer EParent { get; set; }

		public bool OnDrop (DraggableControl el)
		{
			onElementRemove (el);
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

