using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace VisualPracticalLanguage
{
	public class VIfStatement : VCondStatement
	{

		public VIfStatement () : base("если")
		{
			BackColor = Color.LightGray;
		}
	}
}

