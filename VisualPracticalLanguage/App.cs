using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace VisualPracticalLanguage
{
	class MForm : Form {
		private MouseMoveMessageFilter mouseMessageFilter;
		protected override void OnLoad( EventArgs e ) {
			base.OnLoad( e );

			this.mouseMessageFilter = new MouseMoveMessageFilter();
			this.mouseMessageFilter.TargetForm = this;
			Application.AddMessageFilter( this.mouseMessageFilter );
		}

		protected override void OnClosed( EventArgs e ) {
			base.OnClosed( e );
			Application.RemoveMessageFilter( this.mouseMessageFilter );
		}

		class MouseMoveMessageFilter : IMessageFilter {
			public Form TargetForm { get; set; }

			public bool PreFilterMessage( ref Message m ) {
				int numMsg = m.Msg;
				if ( numMsg == 0x0200 /*WM_MOUSEMOVE*/) {
					var cursorPos = TargetForm.PointToClient (Cursor.Position);
					var movedContol = TargetForm.GetDeepChild (cursorPos) as VBaseElement;

					if (movedContol == null) {
						return false;
					}

					var movedContolPos = movedContol.AbsolutePoint ();
					movedContolPos.X--;
					movedContolPos.Y--;

					var targetControl = TargetForm.GetDeepChild (movedContolPos);

					var placeholder = targetControl as ArgumentPlaceholder;
					if (placeholder != null) {
						placeholder.OnDrop (movedContol);
					}
					this.TargetForm.Text = "!" + targetControl;
				}

				return false;
			}
		}

		public MForm() {
			var expPanel = new Panel ();
			expPanel.Parent = this;
			expPanel.Width = 100;
			expPanel.Dock = DockStyle.Right;
			expPanel.BackColor = Color.MintCream;
			expPanel.BorderStyle = BorderStyle.FixedSingle;
			
			var workPanel = new Panel ();
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


	public class App
	{
		public App ()
		{
		}

		static public void Main()
		{
			new GeneratorTest ().Test ();
			new BinGenerator ().Run ();
			Application.Run(new MForm());
		}
	}
}

