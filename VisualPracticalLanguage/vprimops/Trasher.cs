using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPracticalLanguage
{
    public class Trasher : PictureBox, IPlaceholderContainer, IPlaceholder
    {
        private PictureBox box;
        private readonly Image closedTrash;
        private readonly Action<DraggableControl> onElementRemove;
        private readonly Image openTrash;

        public Trasher(Panel workPanel, Action<DraggableControl> onElementRemove)
        {
            this.onElementRemove = onElementRemove;
            openTrash = Image.FromFile("./Resources/open_trash_icon.png");
            closedTrash = Image.FromFile("./Resources/trash_icon.png");
            SetImage(closedTrash);

            Parent = workPanel;
            BringToFront();
            SetLocation(workPanel);
            workPanel.Resize += (object sender, EventArgs e) => { SetLocation(workPanel); };
        }

        public bool OnDrop(DraggableControl el)
        {
            // не давем удалять главную функцию
            if (IsMainFun(el))
                return false;
            onElementRemove(el);
            el.Parent = null;
            ResetColor();
            return true;
        }

        public void OnOver(DraggableControl c)
        {
            // не давем удалять главную функцию
            if (IsMainFun(c))
                return;
            SetImage(openTrash);
        }

        public void OnLeave(DraggableControl c)
        {
            SetImage(closedTrash);
        }

        public void ResetColor()
        {
            SetImage(closedTrash);
        }

        public IPlaceholderContainer parent
        {
            get { return this; }
        }

        public bool CanPutElement(ArgumentPlaceholder p, DraggableControl el)
        {
            return !IsMainFun(el);
        }

        public bool PutElement(ArgumentPlaceholder p, DraggableControl el)
        {
            return true;
        }

        public void OnChildDisconnect(DraggableControl c)
        {
        }

        public IResizable ResizableParent
        {
            get { return EParent; }
        }

        public void UpdateSize()
        {
        }

        public IPlaceholderContainer EParent { get; set; }

        private void SetLocation(Panel workPanel)
        {
            Location = new Point(10, workPanel.Height - Height - 10);
        }

        private void SetImage(Image img)
        {
            Image = img;
            Width = img.Width;
            Height = img.Height;
        }

        private bool IsMainFun(DraggableControl el)
        {
            return (el is VFunction) && (el as VFunction).name == Const.MainFunName;
        }
    }
}