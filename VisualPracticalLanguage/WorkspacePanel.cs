using System;
using System.Windows.Forms;
using System.Drawing;

namespace VisualPracticalLanguage
{
	public class WorkspacePanel : Panel
	{
		public WorkspacePanel ()
		{
			Dock = DockStyle.Fill;
			BackColor = Color.White;

			new Trasher ().With (_ => {
				Action setLocation = () => _.Location = 
					new Point(this.Width - _.Width - 10, this.Height - _.Height - 10);
				_.Parent = this;
				_.BringToFront();
				setLocation();
				this.Resize += (object sender, EventArgs e) => {
					setLocation();
					_.Refresh();
				};
			});
		}
	}
}

