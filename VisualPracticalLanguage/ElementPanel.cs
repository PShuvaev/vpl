using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace VisualPracticalLanguage
{
	public class ElementPanel : FlowLayoutPanel
	{
		public ElementPanel (Action<DraggableControl> addControl, Dictionary<string, Func<DraggableControl>> elements)
		{
			Width = 180;
			Dock = DockStyle.Right;
			BackColor = Color.MintCream;
			BorderStyle = BorderStyle.FixedSingle;

			foreach (var control in elements) {
				var btn = new Button { Text = control.Key };
				btn.AutoSize = true;
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
	}
}

