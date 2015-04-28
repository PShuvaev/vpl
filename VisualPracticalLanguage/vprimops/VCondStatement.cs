using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    public class VCondStatement : DraggableControl, ICondStatement, IPlaceholderContainer, IVariableRefsHolder
    {
        // промежуток между операцией и аргументом
        private const int OpArgPadding = 5;
        private DraggableControl condArg;
        private readonly ArgumentPlaceholder condPlaceholder;
        private readonly CustomLabel condTypeLabel;
        private readonly IList<DraggableControl> controlStatements;
        private readonly IList<ArgumentPlaceholder> placeholders;

        public VCondStatement(string condType, ICondStatement condStatement) : this(condType)
        {
            foreach (var statement in condStatement.statements)
            {
                AddExpression(VElementBuilder.Create(statement));
            }

            SetCondElement(VElementBuilder.Create(condStatement.condition));
            UpdateSize();
        }

        public VCondStatement(string condType)
        {
            condTypeLabel = new CustomLabel(condType, ColorSettings.Get("CondLabel"))
            {
                Parent = this,
                Location = new Point(5, 5)
            };

            controlStatements = new List<DraggableControl>();
            placeholders = new List<ArgumentPlaceholder>
            {
                new ArgumentPlaceholder(this).With(_ => { _.Parent = this; })
            };

            condPlaceholder = new ArgumentPlaceholder(this)
            {
                Parent = this,
                Location = new Point(25, 5)
            };

            UpdateSize();
        }

        public IList<IStatement> statements
        {
            get { return controlStatements.Cast<IStatement>().ToList(); }
        }

        public IExpression condition
        {
            get { return condArg as IExpression; }
        }

        public IResizable ResizableParent
        {
            get { return EParent; }
        }

        public void UpdateSize()
        {
            var condControl = (Control) condArg ?? condPlaceholder;
            ;
            var declwidth = condTypeLabel.Size.Width + 2*OpArgPadding + condControl.Width;
            var bodyexprWidth = Const.TAB_SIZE + controlStatements.Aggregate(0, (acc, e) => Math.Max(acc, e.Size.Width));
            var width = 2*BorderPadding + Math.Max(declwidth, bodyexprWidth);

            condControl.Location = new Point(condTypeLabel.Size.Width + 2*OpArgPadding, 5);

            var height = BorderPadding + Math.Max(condTypeLabel.Size.Height, condControl.Height);

            var isPlaceholder = true;
            foreach (var el in placeholders.Intercalate<Control>(controlStatements))
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
            if (p == condPlaceholder)
                return el is IExpression;

            return el is IStatement && placeholders.IndexOf(p) >= 0;
        }

        public bool PutElement(ArgumentPlaceholder p, DraggableControl el)
        {
            if (!CanPutElement(p, el))
                return false;

            if (p == condPlaceholder)
            {
                SetCondElement(el);
                return true;
            }

            var pos = placeholders.IndexOf(p);
            controlStatements.Insert(pos, el);

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
            if (condArg == c)
            {
                condArg = null;
            }
            else
            {
                var pos = controlStatements.IndexOf(c);
                Controls.Remove(placeholders[pos]);
                controlStatements.RemoveAt(pos);
                placeholders.RemoveAt(pos);
            }

            Controls.Remove(c);
        }

        public IList<VVariableRef> refs
        {
            get
            {
                var condVars = (condArg as IVariableRefsHolder).OrDef(_ => _.refs).EmptyIfNull();
                var statementsVars = controlStatements.Select(x => x as IVariableRefsHolder)
                    .Where(x => x != null).SelectMany(x => x.refs);
                return condVars.Concat(statementsVars).ToList();
            }
        }

        public void AddExpression(DraggableControl expr)
        {
            expr.Parent = this;
            expr.EParent = this;

            if (!controlStatements.Any())
            {
                expr.Location = new Point(Const.TAB_SIZE, Const.HEADER_SIZE);
            }
            else
            {
                var lastExpr = controlStatements.Last();
                expr.Location = new Point(Const.TAB_SIZE, lastExpr.Location.Y + lastExpr.Size.Height + 2);
            }

            controlStatements.Add(expr);
            placeholders.Add(
                new ArgumentPlaceholder(this).With(_ => { _.Parent = this; }));
            this.UpdateRecSize();
        }

        private void SetCondElement(DraggableControl el)
        {
            condArg = el;
            if (el == null)
                return;
            condArg.Parent = this;
            condArg.EParent = this;
            Hide(condPlaceholder);
        }
    }
}