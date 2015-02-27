using System;
using System.Drawing;

namespace VisualPracticalLanguage
{
	public class VVariableRef : DraggableControl
	{
		private VVariable Variable { get; set; }
		private CustomLabel nameLabel;

		public VVariableRef (VVariable variable)
		{
			Variable = variable;
			BackColor = Color.PapayaWhip;
			nameLabel = new CustomLabel (Variable.VarName, Color.Azure);
			nameLabel.Parent = this;
			nameLabel.Location = new Point (5, 5);
			UpdateName ();
		}

		public void UpdateName(){
			nameLabel.Text = Variable.VarName;
			UpdateSize ();
		}

		private void UpdateSize(){
			Size = new Size (nameLabel.Size.Width + 10, nameLabel.Size.Height + 10);
		}
	}
}

