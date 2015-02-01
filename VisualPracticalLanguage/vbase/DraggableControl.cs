using System;
using System.Windows.Forms;

namespace VisualPracticalLanguage
{
	public class DraggableControl : Control
	{
		
		private bool isDragging = false;
		private int oldX, oldY;

		public DraggableControl(){
			MouseDown += new MouseEventHandler(OnMouseDown);
			MouseUp += new MouseEventHandler(OnMouseUp);
			MouseMove += new MouseEventHandler(OnMouseMove);
		}

		private void OnMouseDown(object sender, MouseEventArgs e) 
		{
			isDragging = true;
			oldX = e.X;
			oldY = e.Y;
			BringToFront ();
		}

		private void OnMouseMove(object sender, MouseEventArgs e) 
		{
			if (isDragging)
			{
				Top = Top + (e.Y - oldY); 
				Left = Left + (e.X - oldX); 
			}
		}

		private void OnMouseUp(object sender, MouseEventArgs e) 
		{
			isDragging = false;
		}
	}
}

