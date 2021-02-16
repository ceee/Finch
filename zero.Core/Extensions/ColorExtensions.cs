using System;
using System.Drawing;
using System.Linq;

namespace zero.Core.Extensions
{
  public static class ColorExtensions
  {
    /// <summary>
    /// Get color object from a hex string
    /// </summary>
    public static Color FromHex(this string color)
    {
      if (color.StartsWith("#"))
      {
        color = color[1..];
      }

      if (color.Length != 6)
      {
        throw new Exception("Hex color has to be in format #RRGGBB");
      }

      return Color.FromArgb(
        int.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
        int.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
        int.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber)
      );
    }


    /// <summary>
    /// Get contrast ratio between two color values
    /// <see cref="https://github.com/LeaVerou/contrast-ratio/blob/gh-pages/color.js#L95" />
    /// </summary>
    public static double GetContrastRatio(this Color color1, Color color2)
    {
      double lum1 = color1.GetLuminance();
      double lum2 = color2.GetLuminance();
      double brightest = Math.Max(lum1, lum2);
      double darkest = Math.Min(lum1, lum2);
      return (brightest + 0.05) / (darkest + 0.05);
    }


    /// <summary>
    /// Get luminance value of a color based on WCAG-conform JS implementation
    /// <see cref="https://github.com/LeaVerou/contrast-ratio/blob/gh-pages/color.js#L42" />
    /// </summary>
    public static double GetLuminance(this Color color)
    {
      double[] a = new double[3] { color.R, color.G, color.B }.Select(v =>
      {
        v /= 255;
        return v <= 0.03928 ? v / 12.92 : Math.Pow((v + 0.055) / 1.055, 2.4);
      }).ToArray();

      return a[0] * 0.2126 + a[1] * 0.7152 + a[2] * 0.0722;
    }
  }
}
