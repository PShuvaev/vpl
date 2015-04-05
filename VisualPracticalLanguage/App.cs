using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using Sprache;
using System.Collections.Generic;

namespace VisualPracticalLanguage
{
	public class MForm : Form {
		public Panel workPanel;
		public MForm() {
			Size = new Size (600, 400);

			new MenuStrip ().With (_ => {
				_.Parent = this;
				_.Dock = DockStyle.Top;
				_.Items.Add(new ToolStripMenuItem("&File").With(__ => {
					__.DropDownItems.Add(new ToolStripMenuItem("&New"));
					__.DropDownItems.Add(new ToolStripMenuItem("&Save"));
					__.DropDownItems.Add(new ToolStripMenuItem("Save &as"));
					__.DropDownItems.Add(new ToolStripMenuItem("E&xit"));
				}));
				_.Items.Add(new ToolStripMenuItem("&Build").With(__ => {
					__.DropDownItems.Add(new ToolStripMenuItem("&Make"));
					__.DropDownItems.Add(new ToolStripMenuItem("&Execute"));
				}));
				_.Items.Add(new ToolStripMenuItem("&Libraries").With(__ => {
					__.DropDownItems.Add(new ToolStripMenuItem("&Add"));
				}));
			});

			var groupPanel = new Panel ();
			groupPanel.Parent = this;
			groupPanel.Dock = DockStyle.Fill;

			// http://stackoverflow.com/questions/154543/panel-dock-fill-ignoring-other-panel-dock-setting
			groupPanel.BringToFront ();

			workPanel = new WorkspacePanel ();
			workPanel.Parent = groupPanel;

			new Trasher ().With (_ => {
				_.Parent = workPanel;
				_.Location = new Point(300, 200);
				_.BringToFront();
			});
			
			var expPanel = new ElementPanel (workPanel);
			expPanel.Parent = groupPanel;

			var fun = VplSharpParser.FunDefP.Parse (@"
public dynamic fibb(dynamic n){
	if(n < 2){
		return 1;
	}
	if(n < 3){
		return 1;
	}
	return fibb(n-1) + fibb(n-2);
}
");

			var f = new VFunction (fun);
			f.Parent = workPanel;
			/*
			f.AddArgument ("key");
			f.AddArgument ("state");
			*/

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
			Application.Run(Form = new MForm());
		}

	}
}

