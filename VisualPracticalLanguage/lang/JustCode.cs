using System;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	// TODO: hack, hack, hack!
	public class JustCode : IExpression, IStatement, IFunctionDefinition
	{
		public string code { get; set; }
		public JustCode (string code)
		{
			this.code = code;
		}

		public string fnamespace {
			get { throw new NotImplementedException (); }
		}

		public string fclass {
			get { throw new NotImplementedException (); }
		}

		public string name {
			get { throw new NotImplementedException (); }
		}

		public int argumentsCount {
			get { throw new NotImplementedException (); }
		}

		public bool isBinOperation {
			get { throw new NotImplementedException (); }
		}

		public bool isReturnVoid {
			get { throw new NotImplementedException (); }
		}

		public System.Collections.Generic.IList<IVariable> arguments {
			get { throw new NotImplementedException (); }
		}

		public System.Collections.Generic.IList<IVariable> variables {
			get { throw new NotImplementedException (); }
		}

		public System.Collections.Generic.IList<IStatement> statements {
			get { throw new NotImplementedException (); }
		}
	}
}

