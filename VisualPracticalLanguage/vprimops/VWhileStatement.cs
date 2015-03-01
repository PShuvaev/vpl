using System;
using System.Drawing;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class VWhileStatement : VCondStatement, IWhileStatement
	{
		public VWhileStatement () : base("while")
		{
			BackColor = Color.Bisque;
		}
	}
}

