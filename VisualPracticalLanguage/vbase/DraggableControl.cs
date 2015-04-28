using System.Drawing;
using System.Windows.Forms;

namespace VisualPracticalLanguage
{
    public abstract class DraggableControl : Control
    {
        // отступ от границ компонента
        protected const int BorderPadding = 10;
        private bool isDragging;
        private IPlaceholder lastControl;
        private Point oldPos;

        private readonly Label markLabel = new Label
        {
            Text = "*",
            AutoSize = true,
            Location = new Point(1, 1),
            ForeColor = Color.Red
        };

        public DraggableControl()
        {
            MouseDown += OnMouseDown;
            MouseUp += OnMouseUp;
            MouseMove += OnMouseMove;
        }

        //TODO: уточнить семантику поля
        public IPlaceholderContainer EParent { get; set; }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            SetDragged();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                var control = GetTargetControl() as IPlaceholder;

                if (lastControl != control && lastControl != null)
                {
                    lastControl.OnLeave(this);
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
            Release();
        }

        private Control GetTargetControl()
        {
            var movedContolPos = this.AbsolutePoint();
            movedContolPos.X--;
            movedContolPos.Y--;

            return App.Form.GetDeepChild(movedContolPos);
        }

        protected void Hide(ArgumentPlaceholder h)
        {
            h.Location = new Point(-100, -100);
        }

        private void DisconnectFromParent()
        {
            // сохраняем абсолютную позицию, ибо она изменится при отсоединении от родителя
            var absPos = this.AbsolutePoint();

            if (EParent != null)
            {
                EParent.OnChildDisconnect(this);
                EParent.UpdateRecSize();
            }
            EParent = null;

            Parent = App.Form.workPanel;

            // уродливое восстановление позиции. todo отрефакторить!
            var workspaceLocation = App.Form.workPanel.Parent.Location;
            Location = new Point(absPos.X - workspaceLocation.X, absPos.Y - workspaceLocation.Y);
        }

        private void SetDragged()
        {
            isDragging = true;

            DisconnectFromParent();

            BringToFront();

            Controls.Add(markLabel);
            markLabel.BringToFront();

            oldPos = Cursor.Position;
        }

        private void Release()
        {
            isDragging = false;
            Controls.Remove(markLabel);

            var targetControl = GetTargetControl();

            var placeholder = targetControl as IPlaceholder;
            if (placeholder != null)
            {
                var result = placeholder.OnDrop(this);
                if (result)
                {
                    placeholder.parent.UpdateRecSize();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var pen = new Pen(ColorSettings.Get("ElementBorder"), 3);
            e.Graphics.DrawRectangle(pen, new Rectangle(1, 1, Width - 3, Height - 3));
        }
    }
}