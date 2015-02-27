using System;
using System.Drawing;

namespace VisualPracticalLanguage
{
	public class VVariableRef : DraggableControl
	{
		public VVariable Variable { get; set; }

		public VVariableRef ()
		{
			Size = new Size (20, 20);
			BackColor = Color.Black;
		}

		public void UpdateName(string name){

		}
	}
}

