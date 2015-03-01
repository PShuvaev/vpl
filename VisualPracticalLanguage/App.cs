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


			workPanel = new Panel ();
			workPanel.Parent = this;
			workPanel.Dock = DockStyle.Fill;
			workPanel.BackColor = Color.White;

			
			var expPanel = new ElementPanel (workPanel);
			expPanel.Parent = this;

			var f = new VFunction ("fibbs");
			f.AddExpression (new VSetVariable("a"));
			f.Parent = workPanel;
			
			f.AddArgument ("key");
			f.AddArgument ("state");


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

