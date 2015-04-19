using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    public class ElementPanel : FlowLayoutPanel
    {
        //функция, вызываемая при нажатии на кнопку, т.е. при создании элемента
        private readonly Action<DraggableControl> OnAddFunctionToWorkspace;

        public ElementPanel(Action<DraggableControl> onAddFunctionToWorkspace,
            Dictionary<string, Func<DraggableControl>> elements)
        {
            Width = Const.ELEMENT_PANEL_WIDTH;
            Dock = DockStyle.Right;
            BackColor = Color.MintCream;
            BorderStyle = BorderStyle.FixedSingle;
            OnAddFunctionToWorkspace = onAddFunctionToWorkspace;

            foreach (var control in elements)
            {
                AddBtn(control.Key, control.Value);
            }
        }

        private void AddBtn(string text, Func<DraggableControl> getControl)
        {
            var btn = new Button {Text = text};
            btn.AutoSize = true;
            btn.Click += delegate
            {
                var c = getControl();
                if (c != null)
                {
                    OnAddFunctionToWorkspace(c);
                    c.BringToFront();
                }
            };
            Controls.Add(btn);
        }

        public void AddFunBtn(IFunctionDefinition funDef)
        {
            AddBtn(funDef.name, () => new VFunCall(new FunctionCall
            {
                function = funDef,
                arguments = funDef.arguments.Select<IVariable, IExpression>(x => null).ToList()
            }));
        }
    }
}