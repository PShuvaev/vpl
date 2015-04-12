using System;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class VElementBuilder
	{
		public static DraggableControl Create(IStatement statement){
			return
				(statement as IIfStatement).OrDef (_ => (DraggableControl)new VIfStatement (_)) ??
					(statement as IWhileStatement).OrDef (_ => (DraggableControl)new VWhileStatement (_)) ??
					(statement as IReturnStatement).OrDef (_ => (DraggableControl)new VReturnStatement (_)) ??
					(statement as ISetVariableStatement).OrDef (_ => (DraggableControl)new VSetVariable (_)) ??
					(statement as IFunCallStatement).OrDef (_ => (DraggableControl)new VFunCall (_.functionCall));
		}

		public static DraggableControl Create(IExpression expression){
			return
			// TODO VStringConst & VNumConst
				(expression as IConstExpression).OrDef (_ => _.constValue is string 
				                                        ? (DraggableControl)new VStringConst (_.constValue.ToString()) 
				                                        : (DraggableControl)new VNumberConst (_)) ??
					(expression as IFunctionCall).OrDef (_ => _.function.isBinOperation 
					                                     ? (DraggableControl)new VBinaryOp (_) 
					                                     : (DraggableControl)new VFunCall (_)) ??
				(expression as IVariableRef).OrDef (_ => (DraggableControl)new VVariableRef (_));
		}
	}
}

