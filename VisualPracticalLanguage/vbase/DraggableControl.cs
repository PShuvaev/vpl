using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace VisualPracticalLanguage
{
	public class DraggableControl : Control
	{
		
		private bool isDragging = false;
		private int oldX, oldY;
		private Label markLabel = new Label ()
		{
			Text = "*",
			AutoSize = true,
			Location = new Point (1, 1),
			ForeColor = Color.Red
		};

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

			Controls.Add (markLabel);
			markLabel.BringToFront ();
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
			Controls.Remove (markLabel);
		}
	}
}