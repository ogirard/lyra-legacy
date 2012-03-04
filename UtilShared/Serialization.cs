using System;
using System.Drawing;

namespace Lyra2.UtilShared
{
  public class Serialization
  {
    public static Color? ColorFromHexString(string color)
    {
      try
      {
        string colStr = color.Trim(' ', '#');
        int r = Convert.ToInt32(colStr.Substring(0, 2), 16);
        int g = Convert.ToInt32(colStr.Substring(2, 2), 16);
        int b = Convert.ToInt32(colStr.Substring(4, 2), 16);
        return Color.FromArgb(r, g, b);
      }
      catch (Exception)
      {
        return null;
      }
    }

    public static string ColorToHexString(Color? color)
    {
      if (color == null) return "";
      return "#" + GetColorHex(((Color)color).R) + GetColorHex(((Color)color).G) + GetColorHex(((Color)color).B);
    }

    private static string GetColorHex(int colorValue)
    {
      return Convert.ToString(colorValue, 16).PadLeft(2, '0');
    }
  }
}