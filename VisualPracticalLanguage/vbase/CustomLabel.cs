using System.Drawing;
using System.Windows.Forms;

namespace VisualPracticalLanguage
{
    public class CustomLabel : Label
    {
        public CustomLabel(string text, Color color)
        {
            Text = text;
            BackColor = color;
            AutoSize = true;
            ForeColor = InvertColour(color);
        }

        private Color InvertColour(Color ColourToInvert)
        {
            return Color.FromArgb((byte) ~ColourToInvert.R, (byte) ~ColourToInvert.G, (byte) ~ColourToInvert.B);
        }
    }
}