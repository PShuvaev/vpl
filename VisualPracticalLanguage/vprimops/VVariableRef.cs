using System;
using System.Drawing;
using VisualPracticalLanguage.Interface;
using System.Collections.Generic;

namespace VisualPracticalLanguage
{
	public class VVariableRef : DraggableControl, IVariableRef, IVariableRefsHolder, IResizable
	{
		private VVariable variable { get; set; }
		private CustomLabel nameLabel;

		public string markInitVarName { get; private set;}

		public VVariableRef (IVariableRef varRef) : this((VVariable)null)
		{
			markInitVarName = varRef.varName;
		}

		public VVariableRef (VVariable variable)
		{
			SetInitVariable (variable);
		}

		public void SetInitVariable(VVariable variable){
			this.variable = variable;
			BackColor = Color.PapayaWhip;
			nameLabel = new CustomLabel (varName, Color.Azure);
			nameLabel.Parent = this;
			nameLabel.Location = new Point (5, 5);
			markInitVarName = null;
			UpdateName ();
		}

		public string varName {
			get { return variable.OrDef(_ => _.varName) ?? markInitVarName; }
		}

		public void UpdateName(){
			nameLabel.Text = varName;
			this.UpdateRecSize ();
		}

		public IList<VVariableRef> refs {
			get {
				return new List<VVariableRef>{ this };
			}
		}
		
		public IResizable ResizableParent { get{ return EParent; } }

		public void UpdateSize(){
			Size = new Size (nameLabel.Size.Width + 10, nameLabel.Size.Height + 10);
		}

		public void Destroy(){
			variable.Disconnect (this);
		}
	}
}

