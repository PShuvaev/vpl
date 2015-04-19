using System.Drawing;

namespace VisualPracticalLanguage
{
    public static class CloningExtension
    {
        public static Size Clone(this Size o)
        {
            return new Size(o.Width, o.Height);
        }
    }
}