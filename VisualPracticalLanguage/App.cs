using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace VisualPracticalLanguage
{
	public class MForm : Form {
		public Panel workPanel;
		public MForm() {
			Size = new Size (600, 400);

			new GeneratorTest ().Test ();

			var expPanel = new FlowLayoutPanel ();
			expPanel.Parent = this;
			expPanel.Width = 100;
			expPanel.Dock = DockStyle.Right;
			expPanel.BackColor = Color.MintCream;
			expPanel.BorderStyle = BorderStyle.FixedSingle;

			workPanel = new Panel ();
			workPanel.Parent = this;
			workPanel.Dock = DockStyle.Fill;
			workPanel.BackColor = Color.White;

			var f = new VFunction ("fibbs");
			f.AddExpression (new VNumberConst(100009));
			f.AddExpression (new VSetVariable("a"));
			f.AddExpression (new VFunCall(new FunctionDeclaration{ name = "Substring", isBinOperation = false, argumentsCount = 2 }));
			//f.AddExpression (new VBinaryOp(new FunctionDeclaration{ name = "+", isBinOperation = true, argumentsCount = 2 }));
			f.AddExpression (new VStringConst("somes"));
			//f.AddExpression (new VIfStatement());
			//f.AddExpression (new VWhileStatement());
			f.Parent = workPanel;
			
			f.AddArgument ("key");
			f.AddArgument ("state");

			
			var btn = new Button {
				Text = "add fun"
			};

			int fin = 0;
			btn.Click += (object sender, EventArgs e) => {
				Control control = null;
				workPanel.Controls.Add(control = new VFunction ("fun " + fin++));
				control.BringToFront();
			};
			expPanel.Controls.Add (btn);

			var btn2 = new Button {
				Text = "generate code"
			};
			btn2.Click += (object sender, EventArgs e) => {
				var writer = new StringWriter ();
				var generator = new Generator (writer);
				generator.Generate(f);
				Logger.Log(writer.ToString());
			};
			expPanel.Controls.Add (btn2);

			CenterToScreen();
		}
	}


	public static class App
	{
		public static MForm Form;

		static public void Main()
		{
			//new GeneratorTest ().Test ();
			//new BinGenerator ().Run ();
			Application.Run(Form = new MForm());
		}
	}
}

