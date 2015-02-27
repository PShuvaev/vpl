using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace VisualPracticalLanguage
{
	public class VVariable : CustomLabel
	{
		public VFunction ParentFunc { get; set; }
		public string VarName { get; set; }

		private IList<VVariableRef> VariableRefs;

		public VVariable (string name, VFunction parentFunc):base(name, Color.Red)
		{
			VarName = name;
			ParentFunc = parentFunc;

			BackColor = Color.Orange;
			Size = new Size (20, 20);

			VariableRefs = new List<VVariableRef> ();

			MouseDoubleClick += (object sender, MouseEventArgs e) => {
				var newName = ShowDialog("Введите имя переменной", "Переименование");
				if(newName.Trim().Length == 0) return;

				VarName = newName;
				this.Text = newName;
				foreach(var varRef in VariableRefs){
					varRef.UpdateName();
				}
			};
			
			MouseClick += (object sender, MouseEventArgs e) => {
				if (Control.ModifierKeys != Keys.Control) return;
				CreateVVariableRef();
			};
		}

		public void CreateVVariableRef(){
			var v = new VVariableRef (this);
			VariableRefs.Add(v);

			v.Parent = App.Form.workPanel;
			v.BringToFront ();
		}

		private static string ShowDialog(string text, string caption)
		{
			Form prompt = new Form {
				Width = 500,
				Height = 150,
				FormBorderStyle = FormBorderStyle.FixedDialog,
				Text = caption,
				StartPosition = FormStartPosition.CenterScreen
			};

			Label textLabel = new Label() { Left = 50, Top=20, Text=text };
			TextBox textBox = new TextBox() { Left = 50, Top=50, Width=400 };
			Button confirmation = new Button() { Text = "Ok", Left=350, Width=100, Top=70 };
			confirmation.Click += (sender, e) => { prompt.Close(); };
			prompt.Controls.Add(textBox);
			prompt.Controls.Add(confirmation);
			prompt.Controls.Add(textLabel);
			prompt.AcceptButton = confirmation;
			prompt.ShowDialog();
			return textBox.Text;
		}
	}
}

