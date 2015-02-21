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
			var expPanel = new Panel ();
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
			f.AddExpression (new VVariable("a"));
			f.AddExpression (VBinaryOp.PLUS);
			f.AddExpression (new VStringConst("somes"));
			f.AddExpression (new VNumberConst(100009));
			f.Parent = workPanel;
			
			f.AddArgument ("key");
			f.AddArgument ("state");

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

