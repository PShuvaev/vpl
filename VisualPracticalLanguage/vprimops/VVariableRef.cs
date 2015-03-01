using System;
using System.Drawing;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class VVariableRef : DraggableControl, IVariable
	{
		private VVariable variable { get; set; }
		private CustomLabel nameLabel;

		public VVariableRef (VVariable variable)
		{
			this.variable = variable;
			BackColor = Color.PapayaWhip;
			nameLabel = new CustomLabel (variable.varName, Color.Azure);
			nameLabel.Parent = this;
			nameLabel.Location = new Point (5, 5);
			UpdateName ();
		}

		public string varName {
			get { return variable.varName; }
		}

		public void UpdateName(){
			nameLabel.Text = variable.varName;
			UpdateSize ();
		}

		private void UpdateSize(){
			Size = new Size (nameLabel.Size.Width + 10, nameLabel.Size.Height + 10);
		}
	}
}

