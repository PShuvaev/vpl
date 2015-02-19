using System;
using System.Windows.Forms;
using System.Drawing;

namespace VisualPracticalLanguage
{
	public static class DiverseUtilExtensions
	{
		public static Control RootPanel(this Control el){
			if (el is Panel)
				return el;
			return el.Parent.RootPanel ();
		}

		/// <summary>
		/// Получает самый вложенный элемент в точке p относительно контрола parentControl
		/// </summary>
		public static Control GetDeepChild(this Control parentControl, Point p){
			Control child = null;

			do {
				child = parentControl.GetChildAtPoint (p);
				if(child == null) return parentControl;
				p.X -= child.Location.X;
				p.Y -= child.Location.Y;
				parentControl = child;
			} while(true);
		}

		/// <summary>
		/// Получает координаты левого верхнего угла контрола относительно формы
		/// </summary>
		public static Point AbsolutePoint(this Control control){
			Point p = new Point ();
			while (!(control is MForm)) {
				p.X += control.Location.X;
				p.Y += control.Location.Y;
				control = control.Parent;
			}
			return p;
		}
	}
}

