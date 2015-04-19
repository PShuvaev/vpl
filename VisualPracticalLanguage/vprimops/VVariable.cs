using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    public class VVariable : CustomLabel, IVariable
    {
        public VVariable(string name, VFunction parentFunc) : base(name, Color.Red)
        {
            varName = name;
            this.parentFunc = parentFunc;

            BackColor = Color.Orange;
            Size = new Size(20, 20);

            VariableRefs = new List<VVariableRef>();

            MouseDoubleClick += (sender, e) =>
            {
                var newName = DiverseUtilExtensions.ShowDialog("Введите имя переменной", "Переименование");
                if (newName.Trim().Length == 0) return;

                varName = newName;
                Text = newName;
                foreach (var varRef in VariableRefs)
                {
                    varRef.UpdateName();
                }
            };

            MouseClick += (sender, e) =>
            {
                if (ModifierKeys == Keys.Control)
                {
                    CreateVVariableRef();
                }
                if (ModifierKeys == Keys.Shift)
                {
                    parentFunc.RemoveArgument(this);
                }
            };
        }

        public VFunction parentFunc { get; set; }
        public IList<VVariableRef> VariableRefs { get; set; }
        public string varName { get; set; }

        public void CreateVVariableRef()
        {
            var v = new VVariableRef(this);
            VariableRefs.Add(v);

            v.Parent = App.Form.workPanel;
            v.BringToFront();
        }

        public void AttachVarRef(VVariableRef vRef)
        {
            VariableRefs.Add(vRef);
            vRef.SetInitVariable(this);
        }

        public void Remove()
        {
            if (VariableRefs.Count == 0)
            {
                parentFunc.Controls.Remove(this);
            }
        }

        public void Disconnect(VVariableRef vRef)
        {
            VariableRefs.Remove(vRef);
        }
    }
}