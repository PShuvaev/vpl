using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace VisualPracticalLanguage
{
	public class ElementPanel : FlowLayoutPanel
	{
		private static Dictionary<string, Func<DraggableControl>> elements = new Dictionary<string, Func<DraggableControl>>(){
			{"+", () => MakeBinaryOp("+")},
			{"-", () => MakeBinaryOp("-")},
			{"/", () => MakeBinaryOp("/")},
			{"*", () => MakeBinaryOp("*")},
			{"=", () => MakeBinaryOp("==")},
			{">", () => MakeBinaryOp(">")},
			{"<", () => MakeBinaryOp("<")},
			{"!=", () => MakeBinaryOp("!=")},
			{"если", () => new VIfStatement()},
			{"пока", () => new VWhileStatement()},
			{"вернуть ", () => new VReturnStatement()},
			{"константа", () => {
					var val = DiverseUtilExtensions.ShowDialog("Новая константа", "Введите значение");
					if(val.StartsWith("\"")) return new VStringConst(val);

					decimal x;
					if(decimal.TryParse(val, out x)) return new VNumberConst(x);

					return null;
				}},
			{"функция", () => {
					// TODO: если пользователь ничего не вводит, возвращать null
					var name = DiverseUtilExtensions.ShowDialog("Новая функция", "Введите имя");
					return new VFunction(name);
				}}
		};

		public ElementPanel (Action<DraggableControl> addControl)
		{
			Width = 180;
			Dock = DockStyle.Right;
			BackColor = Color.MintCream;
			BorderStyle = BorderStyle.FixedSingle;


			foreach (var control in elements) {
				var btn = new Button { Text = control.Key };
				btn.Click += delegate {
					var c = control.Value();
					if(c != null){ 
						addControl(c);
						c.BringToFront();
					}
				};
				Controls.Add (btn);
			}
		}

		private static VBinaryOp MakeBinaryOp(string symbol){
			return new VBinaryOp (new FunctionDeclaration { name = symbol, isBinOperation = true });
		}
	}
}

