using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace VisualPracticalLanguage
{
    public static class ColorSettings
    {
        private const string ColorSettingsFileName = "colors.txt";
        private static Random random = new Random();

        private static IDictionary<string, Color> cache = Init();

        private static IDictionary<string, Color> Init()
        {
            try
            {
                return File.ReadAllLines(ColorSettingsFileName)
                .Select(x => x.Split('='))
                .Select(x => new { name = x[0].Trim(), color = x[1].Trim() })
                .Select(x => new { name = x.name, color = int.Parse(x.color, NumberStyles.HexNumber) })
                .ToDictionary(x => x.name, x => Color.FromArgb(x.color));
            }
            catch (Exception)
            {
                Logger.Log("ColorSettingsFile not found");
                return new Dictionary<string, Color>();
            }
        }


        public static Color Get(string name)
        {
            Color color;
            return cache.TryGetValue(name, out color) ? color : Color.FromArgb(random.Next());
        }
    }
}
