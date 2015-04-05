using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class VIfStatement : VCondStatement, IIfStatement
	{
		public VIfStatement (IIfStatement ifStatement) : base("если", ifStatement)
		{
			BackColor = Color.LightGray;
		}

		public VIfStatement () : base("если")
		{
			BackColor = Color.LightGray;
		}
	}
}

