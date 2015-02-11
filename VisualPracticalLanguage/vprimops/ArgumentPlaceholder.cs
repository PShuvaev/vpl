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
		private VBaseElement parent;

		public ArgumentPlaceholder (VBaseElement parent)
		{
			this.parent = parent;
			Size = new Size (15, 15);
			BackColor = Color.WhiteSmoke;
		}

		private void OnDrop(VBaseElement el){
			if (parent.TryPutElement (this, el)) {
				//?
			}
		}
	}
}

