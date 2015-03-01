using System;
using System.Windows.Forms;
using System.Drawing;

namespace VisualPracticalLanguage
{
	public static class DiverseUtilExtensions
	{
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

		
		public static string ShowDialog(string text, string caption)
		{
			Form prompt = new Form {
				Width = 500,
				Height = 150,
				FormBorderStyle = FormBorderStyle.FixedDialog,
				Text = caption,
				StartPosition = FormStartPosition.CenterScreen
			};
			prompt.Controls.Add(new Label { Left = 50, Top = 20, Text = text, AutoSize = true });

			TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
			prompt.Controls.Add(textBox);

			Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 80};
			confirmation.Click += (sender, e) => { prompt.Close(); };
			prompt.Controls.Add(confirmation);
			prompt.AcceptButton = confirmation;
			prompt.ShowDialog();
			return textBox.Text;
		}
	}
}

