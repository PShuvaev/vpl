using System;
using System.Windows.Forms;
using System.Drawing;

namespace VisualPracticalLanguage
{
	public class CustomLabel : Label
	{
		public CustomLabel (string text, Color color)
		{
			this.Text = text;
			this.BackColor = color;
			this.AutoSize = true;
			this.ForeColor = InvertColour (color);
		}

		private Color InvertColour(Color ColourToInvert)
		{
			return Color.FromArgb((byte)~ColourToInvert.R, (byte)~ColourToInvert.G, (byte)~ColourToInvert.B);
		}
	}
}

