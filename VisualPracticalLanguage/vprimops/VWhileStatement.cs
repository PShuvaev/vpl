using System;
using System.Drawing;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class VWhileStatement : VCondStatement, IWhileStatement
	{
		public VWhileStatement () : base("выполнять пока")
		{
			BackColor = Color.Bisque;
		}
	}
}

