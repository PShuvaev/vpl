using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace VisualPracticalLanguage
{
	public abstract class DraggableControl : Control
	{
		
		private bool isDragging = false;
		public IPlaceholderContainer EParent{ get; set; }

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
			SetDragged ();
		}

		private Point oldPos;

		private ArgumentPlaceholder lastControl;

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
			Release ();
		}

		private Control GetTargetControl(){
			var movedContolPos = this.AbsolutePoint ();
			movedContolPos.X--;
			movedContolPos.Y--;

			return App.Form.GetDeepChild (movedContolPos);
		}

		
		protected void Hide(ArgumentPlaceholder h){
			h.Location = new Point (-100, -100);
		}

		private void DisconnectFromParent(){
			if (EParent != null) {
				EParent.OnChildDisconnect (this);
				EParent.UpdateSize ();
			}
			EParent = null;
			
			// сохраняем абсолютную позицию, ибо она изменится при отсоединении от родителя
			var absPos = this.AbsolutePoint ();
			this.Parent = App.Form.workPanel;
			this.Location = absPos;
		}

		private void SetDragged(){
			isDragging = true;

			DisconnectFromParent ();

			BringToFront ();

			Controls.Add (markLabel);
			markLabel.BringToFront ();

			oldPos = Cursor.Position;
		}

		private void Release(){
			isDragging = false;
			Controls.Remove (markLabel);

			var targetControl = GetTargetControl ();

			var placeholder = targetControl as ArgumentPlaceholder;
			if (placeholder != null) {
				var result = placeholder.OnDrop (this);
				if (result) {
					placeholder.parent.UpdateRecSize ();
				}
			}
		}
	}
}