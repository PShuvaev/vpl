using System.Collections.Generic;

namespace VisualPracticalLanguage
{
	public interface IVariableRefsHolder
	{
		IList<VVariableRef> refs { get; }
	}
}

