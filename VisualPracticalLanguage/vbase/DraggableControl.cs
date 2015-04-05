using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace VisualPracticalLanguage
{
	public abstract class DraggableControl : Control
	{
		
		private bool isDragging = false;

		//TODO: уточнить семантику поля
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

		private IPlaceholder lastControl;

		private void OnMouseMove(object sender, MouseEventArgs e) 
		{
			if (isDragging)
			{
				var control = GetTargetControl () as IPlaceholder;

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
			// сохраняем абсолютную позицию, ибо она изменится при отсоединении от родителя
			var absPos = this.AbsolutePoint ();

			if (EParent != null) {
				EParent.OnChildDisconnect (this);
				EParent.UpdateSize ();
			}
			EParent = null;

			this.Parent = App.Form.workPanel;

			// уродливое восстановление позиции. todo отрефакторить!
			var workspaceLocation = App.Form.workPanel.Parent.Location;
			this.Location = new Point(absPos.X - workspaceLocation.X, absPos.Y - workspaceLocation.Y);
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

			var placeholder = targetControl as IPlaceholder;
			if (placeholder != null) {
				var result = placeholder.OnDrop (this);
				if (result) {
					placeholder.parent.UpdateRecSize ();
				}
			}
		}
	}
}