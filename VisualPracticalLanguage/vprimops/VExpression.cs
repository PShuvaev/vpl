using System;
using System.Drawing;

namespace VisualPracticalLanguage
{
	public class VExpression : VBaseElement
	{
		public VExpression ()
		{
		}

		public override bool TryPutElement (ArgumentPlaceholder p, VBaseElement el){
			return false;
		}
	}
}

