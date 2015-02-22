using System;
using System.Windows.Forms;
using System.Drawing;

namespace VisualPracticalLanguage
{
	/// <summary>
	/// Компонент-заместитель аргумента в неком выражении.
	/// Припопытке поместить элемент поверх плейсхолдера, последний вызывает у родителя метод TryPutElement и, если тот возвращает true,
	/// замещается выражением.
	/// </summary>
	public class ArgumentPlaceholder : Control
	{
		public VBaseElement parent;

		public ArgumentPlaceholder (VBaseElement parent)
		{
			this.parent = parent;
			Size = new Size (15, 15);
			BackColor = Color.WhiteSmoke;
		}

		public bool OnDrop(VBaseElement el){
			return parent.PutElement (this, el);
		}

		public void OnOver(DraggableControl c){
			if (parent.CanPutElement (this, c as VBaseElement)) {
				BackColor = Color.Red;
			}
		}

		public void OnLeave (DraggableControl c){
			BackColor = Color.WhiteSmoke;
		}
	}
}

