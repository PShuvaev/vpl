using System;
using VisualPracticalLanguage.Interface;
using System.Collections.Generic;

namespace VisualPracticalLanguage
{
	public class Namespace : INamespace
	{
		public string namespaceName { get; set; }
		public IList<string> importedDlls { get; set; }
		public IList<IFunctionDefinition> functions { get; set; }
	}
}

