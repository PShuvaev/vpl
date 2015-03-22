using System;
using System.Collections.Generic;

namespace VisualPracticalLanguage.Interface
{
	public interface INamespace
	{
		string namespaceName { get; }
		IList<string> importedDlls { get; }
		IList<IFunctionDefinition> functions { get; }
	}
}

