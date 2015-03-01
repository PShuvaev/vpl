using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace VisualPracticalLanguage
{
	public class ElementPanel : FlowLayoutPanel
	{
		private static Dictionary<string, Func<Control>> elements = new Dictionary<string, Func<Control>>(){
			{"+", () => new VBinaryOp(new FunctionDeclaration{name = "+", isBinOperation = true})},
			{"-", () => new VBinaryOp(new FunctionDeclaration{name = "-", isBinOperation = true})},
			{"/", () => new VBinaryOp(new FunctionDeclaration{name = "-", isBinOperation = true})},
			{"*", () => new VBinaryOp(new FunctionDeclaration{name = "-", isBinOperation = true})},
			{"if", () => new VIfStatement()},
			{"while", () => new VWhileStatement()},
		};

		public ElementPanel (Control workPanel)
		{
			Width = 100;
			Dock = DockStyle.Right;
			BackColor = Color.MintCream;
			BorderStyle = BorderStyle.FixedSingle;


			foreach (var control in elements) {
				var btn = new Button { Text = control.Key };
				btn.Click += delegate {
					var c = control.Value();
					workPanel.Controls.Add(c);
					c.BringToFront();
				};
				Controls.Add (btn);
			}
		}
	}
}

