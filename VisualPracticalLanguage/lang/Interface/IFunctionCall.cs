using System;
using System.Collections.Generic;

namespace VisualPracticalLanguage.Interface
{
	public interface IFunctionCall : IExpression
	{
		IFunctionDeclaration function { get; set; }
		IList<IExpression> arguments { get; set; }
	}
}