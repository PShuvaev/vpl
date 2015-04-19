using System.Collections.Generic;
using System.Drawing;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    public class VVariableRef : DraggableControl, IVariableRef, IVariableRefsHolder, IResizable
    {
        private CustomLabel nameLabel;

        public VVariableRef(IVariableRef varRef) : this((VVariable) null)
        {
            markInitVarName = varRef.varName;
        }

        public VVariableRef(VVariable variable)
        {
            SetInitVariable(variable);
        }

        private VVariable variable { get; set; }
        public string markInitVarName { get; private set; }

        public IResizable ResizableParent
        {
            get { return EParent; }
        }

        public void UpdateSize()
        {
            Size = new Size(nameLabel.Size.Width + 10, nameLabel.Size.Height + 10);
        }

        public string varName
        {
            get { return variable.OrDef(_ => _.varName) ?? markInitVarName; }
        }

        public IList<VVariableRef> refs
        {
            get { return new List<VVariableRef> {this}; }
        }

        public void SetInitVariable(VVariable variable)
        {
            this.variable = variable;
            BackColor = Color.PapayaWhip;
            nameLabel = new CustomLabel(varName, Color.Azure);
            nameLabel.Parent = this;
            nameLabel.Location = new Point(5, 5);
            markInitVarName = null;
            UpdateName();
        }

        public void UpdateName()
        {
            nameLabel.Text = varName;
            this.UpdateRecSize();
        }

        public void Destroy()
        {
            variable.Disconnect(this);
        }
    }
}