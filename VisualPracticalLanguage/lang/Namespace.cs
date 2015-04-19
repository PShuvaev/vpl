using System.Collections.Generic;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    public class Namespace : INamespace
    {
        public string namespaceName { get; set; }
        public IList<string> importedDlls { get; set; }
        public IList<IFunctionDefinition> functions { get; set; }
    }
}