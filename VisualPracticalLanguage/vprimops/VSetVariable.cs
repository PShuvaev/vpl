using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    public class VSetVariable : DraggableControl, ISetVariableStatement, IPlaceholderContainer, IVariableRefsHolder
    {
        // промежуток между операцией и аргументом
        private const int OpArgPadding = 5;
        private DraggableControl arg;
        private VVariableRef varRef;
        private readonly ArgumentPlaceholder argPlaceHolder;
        private readonly CustomLabel eqLabel;
        private readonly ArgumentPlaceholder varPlaceHolder;

        public VSetVariable(ISetVariableStatement setVarStatement) : this()
        {
            SetVariableRef(VElementBuilder.Create(setVarStatement.variableRef));
            SetExpression(VElementBuilder.Create(setVarStatement.expression));
            UpdateSize();
        }

        public VSetVariable()
        {
            BackColor = Color.Yellow;
            BackColor = Color.Yellow;
            Size = new Size(100, Const.EXPR_HEIGHT);

            argPlaceHolder = new ArgumentPlaceholder(this);
            argPlaceHolder.Parent = this;
            varPlaceHolder = new ArgumentPlaceholder(this);
            varPlaceHolder.Parent = this;

            eqLabel = new CustomLabel(" = ", BackColor);
            Controls.Add(eqLabel);

            UpdateSize();
        }

        public bool CanPutElement(ArgumentPlaceholder p, DraggableControl el)
        {
            return p == argPlaceHolder && el is IExpression
                   || p == varPlaceHolder && el is VVariableRef;
        }

        public bool PutElement(ArgumentPlaceholder p, DraggableControl el)
        {
            if (!CanPutElement(p, el))
                return false;

            if (p == argPlaceHolder)
            {
                SetExpression(el);
            }
            else
            {
                SetVariableRef(el);
            }
            return true;
        }

        public void OnChildDisconnect(DraggableControl c)
        {
            if (arg == c)
            {
                arg = null;
                Controls.Remove(c);
            }
            if (varRef == c)
            {
                varRef = null;
                Controls.Remove(c);
            }
        }

        public IResizable ResizableParent
        {
            get { return EParent; }
        }

        public void UpdateSize()
        {
            var vArg = (Control) varRef ?? varPlaceHolder;
            var fArg = (Control) arg ?? argPlaceHolder;

            var argHeight = fArg.OrDef(_ => _.Height, 0);
            var height = 2*BorderPadding + argHeight;

            var argsWidth = fArg.OrDef(_ => _.Width, 0);
            var width = 2*BorderPadding + 2*OpArgPadding + argsWidth + vArg.Width + eqLabel.Size.Width;

            Size = new Size(width, height);

            vArg.Location = new Point(BorderPadding, (height - vArg.Height)/2);
            eqLabel.Location = new Point(vArg.Location.X + vArg.Size.Width + OpArgPadding, (height - eqLabel.Height)/2);

            fArg.Location = new Point(eqLabel.Location.X + eqLabel.Size.Width + OpArgPadding, BorderPadding);
        }

        public IVariableRef variableRef
        {
            get { return varRef.OrDef(_ => new VariableRef {varName = _.varName}); }
        }

        public IExpression expression
        {
            get { return arg as IExpression; }
        }

        public IList<VVariableRef> refs
        {
            get
            {
                return varRef.OrDef(x => x.refs).EmptyIfNull().Concat
                    ((arg as IVariableRefsHolder).OrDef(x => x.refs).EmptyIfNull()).ToList();
            }
        }

        private void SetVariableRef(DraggableControl el)
        {
            varRef = (VVariableRef) el;
            if (el == null)
                return;
            varRef.Parent = this;
            varRef.EParent = this;
            Hide(varPlaceHolder);
        }

        private void SetExpression(DraggableControl el)
        {
            arg = el;
            if (el == null)
                return;
            arg.Parent = this;
            arg.EParent = this;
            Hide(argPlaceHolder);
        }
    }
}