using System.Drawing;
using System.Windows.Forms;

namespace VisualPracticalLanguage
{
    /// <summary>
    ///     Компонент-заместитель аргумента в неком выражении.
    ///     Припопытке поместить элемент поверх плейсхолдера, последний вызывает у родителя метод TryPutElement и, если тот
    ///     возвращает true,
    ///     замещается выражением.
    /// </summary>
    public class ArgumentPlaceholder : Control, IPlaceholder
    {
        public ArgumentPlaceholder(IPlaceholderContainer parent)
        {
            this.parent = parent;
            Size = new Size(15, 15);
            BackColor = Color.WhiteSmoke;
        }

        public IPlaceholderContainer parent { get; set; }

        public bool OnDrop(DraggableControl el)
        {
            return parent.PutElement(this, el);
        }

        public void OnOver(DraggableControl c)
        {
            if (parent.CanPutElement(this, c))
            {
                BackColor = Color.Red;
            }
        }

        public void OnLeave(DraggableControl c)
        {
            ResetColor();
        }

        public void ResetColor()
        {
            BackColor = Color.WhiteSmoke;
        }
    }
}