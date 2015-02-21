using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace VisualPracticalLanguage
{
	public abstract class DraggableControl : Control
	{
		
		private bool isDragging = false;
		public DraggableControl EParent{ get; set; }

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


			if(EParent != null) EParent.OnChildDisconnect (this);
			EParent = null;

			var absPos = this.AbsolutePoint ();

			this.Parent = App.Form.workPanel;

			this.Location = absPos;

			BringToFront ();


			Controls.Add (markLabel);
			markLabel.BringToFront ();


			oldPos = Cursor.Position;
		}

		private Point oldPos;

		ArgumentPlaceholder lastControl;

		private void OnMouseMove(object sender, MouseEventArgs e) 
		{
			if (isDragging)
			{
				var control = GetTargetControl () as ArgumentPlaceholder;

				if (lastControl != control && lastControl != null) {
					lastControl.OnLeave (this);
				}
				lastControl = control.With(_ => _.OnOver(this));


				var pos = Cursor.Position;
				Top = Top + (pos.Y - oldPos.Y);
				Left = Left + (pos.X - oldPos.X);
				oldPos = Cursor.Position;
			}
		}

		private void OnMouseUp(object sender, MouseEventArgs e) 
		{
			isDragging = false;
			Controls.Remove (markLabel);

			var targetControl = GetTargetControl ();

			var placeholder = targetControl as ArgumentPlaceholder;
			if (placeholder != null) {
				placeholder.OnDrop ((VBaseElement)this);
			}
		}

		private Control GetTargetControl(){
			var movedContolPos = this.AbsolutePoint ();
			movedContolPos.X--;
			movedContolPos.Y--;

			return App.Form.GetDeepChild (movedContolPos);
		}

		public abstract void OnChildDisconnect (DraggableControl c);
	}
}