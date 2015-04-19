using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    public class VFunction : DraggableControl, IFunctionDefinition, IPlaceholderContainer, IVariableRefsHolder
    {
        // промежуток между операцией и аргументом
        private const int OpArgPadding = 5;
        private readonly Button addArgBtn;
        private readonly Button addVarBtn;
        private readonly CustomLabel funName;
        private readonly IList<ArgumentPlaceholder> placeholders;
        private readonly IList<DraggableControl> vstatements;

        public VFunction(IFunctionDefinition funDef) : this(funDef.name)
        {
            foreach (var arg in funDef.arguments)
            {
                AddArgument(arg.varName);
            }
            foreach (var v in funDef.variables)
            {
                AddVariable(v.varName);
            }
            foreach (var statement in funDef.statements)
            {
                var velement = VElementBuilder.Create(statement);
                AddExpression(velement);
            }

            var sourceVariables = vvariables.Concat(varguments).ToList();
            foreach (var @ref in refs)
            {
                var variable = sourceVariables.FirstOrDefault(x => x.varName == @ref.markInitVarName);
                variable.AttachVarRef(@ref);
            }
        }

        public VFunction(string name)
        {
            this.name = name;

            BackColor = Color.LightBlue;

            funName = new CustomLabel(name, Color.Black)
            {
                Parent = this,
                Location = new Point(BorderPadding, BorderPadding)
            };

            vstatements = new List<DraggableControl>();
            varguments = new List<VVariable>();
            vvariables = new List<VVariable>();
            placeholders = new List<ArgumentPlaceholder>
            {
                new ArgumentPlaceholder(this)
                {
                    Parent = this
                }
            };

            addArgBtn = new Button
            {
                Text = "+",
                Parent = this,
                Size = new Size(20, 20)
            };
            addArgBtn.Click += (object sender, EventArgs e) =>
            {
                if (name == Const.MainFunName)
                {
                    MessageBox.Show("Главная подпрограмма не принимает аргументов.");
                    return;
                }
                var argName = "arg" + varguments.Count;
                AddArgument(argName);
            };

            addVarBtn = new Button
            {
                Text = "+",
                Parent = this,
                Size = new Size(20, 20)
            };
            addVarBtn.Click += (object sender, EventArgs e) =>
            {
                var varName = "var" + vvariables.Count;
                AddVariable(varName);
            };

            UpdateSize();
        }

        public IList<VVariable> varguments { get; set; }
        public IList<VVariable> vvariables { get; set; }
        public string name { get; set; }
        public bool isBinOperation { get; set; }

        public IList<IVariable> variables
        {
            get { return vvariables.Cast<IVariable>().ToList(); }
        }

        public IList<IStatement> statements
        {
            get { return vstatements.Cast<IStatement>().ToList(); }
        }

        public IList<IVariable> arguments
        {
            get { return varguments.Cast<IVariable>().ToList(); }
        }

        public int argumentsCount
        {
            get { return varguments.Count; }
        }

        public IResizable ResizableParent
        {
            get { return EParent; }
        }

        public void UpdateSize()
        {
            var argumentsWidth = varguments.Sum(x => x.Size.Width) + OpArgPadding*(varguments.Count - 1);
            var variablesWidth = vvariables.Sum(x => x.Size.Width) + OpArgPadding*(vvariables.Count - 1);

            {
                // расположение аргументов + кнопки добавления аргумента
                var startArgsX = BorderPadding + funName.Size.Width + OpArgPadding;
                foreach (var arg in varguments)
                {
                    arg.Location = new Point(startArgsX, BorderPadding);
                    startArgsX += arg.Size.Width + OpArgPadding;
                }

                addArgBtn.Location = new Point(startArgsX, BorderPadding);
            }

            var funDeclHeight = 30;

            {
                // расположение переменных + кнопки добавления переменной
                var startVarX = BorderPadding;
                foreach (var v in vvariables)
                {
                    v.Location = new Point(startVarX, BorderPadding + funDeclHeight);
                    startVarX += v.Size.Width + OpArgPadding;
                }

                addVarBtn.Location = new Point(startVarX, BorderPadding + funDeclHeight);
                funDeclHeight += 30;
            }

            var funDeclWidth = 2*BorderPadding + funName.Size.Width + OpArgPadding + argumentsWidth + OpArgPadding +
                               addArgBtn.Width;
            funDeclWidth = Math.Max(funDeclWidth, BorderPadding + variablesWidth + addVarBtn.Width);

            var bodyExprsWidth = Const.TAB_SIZE + vstatements.Aggregate(0, (acc, e) => Math.Max(acc, e.Size.Width));
            var width = 2*BorderPadding + Math.Max(funDeclWidth, bodyExprsWidth);

            var height = 2*BorderPadding + funDeclHeight;

            var isPlaceholder = true;
            foreach (var el in placeholders.Intercalate<Control>(vstatements))
            {
                var xlocation = isPlaceholder ? Const.TAB_SIZE : 2*Const.TAB_SIZE;
                isPlaceholder = !isPlaceholder;
                el.Location = new Point(xlocation, height);
                height += el.Size.Height;
            }

            height += BorderPadding;

            Size = new Size(width, height);
        }

        public bool CanPutElement(ArgumentPlaceholder p, DraggableControl el)
        {
            return el is IStatement && placeholders.Contains(p);
        }

        public bool PutElement(ArgumentPlaceholder p, DraggableControl el)
        {
            if (!CanPutElement(p, el))
                return false;

            var pos = placeholders.IndexOf(p);

            vstatements.Insert(pos, el);

            el.Parent = this;
            el.EParent = this;

            placeholders.Insert(pos, new ArgumentPlaceholder(this)
            {
                Parent = this
            });

            return true;
        }

        public void OnChildDisconnect(DraggableControl c)
        {
            var pos = vstatements.IndexOf(c);
            Controls.Remove(vstatements[pos]);
            Controls.Remove(placeholders[pos]);
            vstatements.RemoveAt(pos);
            placeholders.RemoveAt(pos);
        }

        public IList<VVariableRef> refs
        {
            get
            {
                return vstatements.Select(x => x as IVariableRefsHolder)
                    .Where(x => x != null).SelectMany(x => x.refs).ToList();
            }
        }

        public void AddArgument(string arg)
        {
            var label = new VVariable(arg, this);
            varguments.Add(label);

            Controls.Add(label);
            UpdateSize();
        }

        public void RemoveArgument(VVariable arg)
        {
            if (arg.VariableRefs.Count == 0)
            {
                varguments.Remove(arg);
                Controls.Remove(arg);
                UpdateSize();
            }
        }

        public void AddVariable(string v)
        {
            var label = new VVariable(v, this);
            vvariables.Add(label);

            Controls.Add(label);
            UpdateSize();
        }

        public void RemoveVariable(VVariable v)
        {
            if (v.VariableRefs.Count == 0)
            {
                vvariables.Remove(v);
                Controls.Remove(v);
                UpdateSize();
            }
        }

        public void AddExpression(DraggableControl expr)
        {
            expr.Parent = this;
            expr.EParent = this;

            if (!vstatements.Any())
            {
                expr.Location = new Point(Const.TAB_SIZE, Const.HEADER_SIZE);
            }
            else
            {
                var lastExpr = vstatements.Last();
                expr.Location = new Point(Const.TAB_SIZE, lastExpr.Location.Y + lastExpr.Size.Height + 2);
            }

            vstatements.Add(expr);
            placeholders.Add(
                new ArgumentPlaceholder(this).With(_ => { _.Parent = this; }));
            this.UpdateRecSize();
        }
    }
}