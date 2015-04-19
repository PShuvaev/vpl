using System.Collections.Generic;

namespace VisualPracticalLanguage.Interface
{
    public interface INamespace
    {
        string namespaceName { get; set; }
        IList<string> importedDlls { get; set; }
        IList<IFunctionDefinition> functions { get; set; }
    }
}