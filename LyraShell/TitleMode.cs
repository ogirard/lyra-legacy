namespace Lyra2.LyraShell
{
  public enum TitleMode
  {
    NumberAndTitle = 0,
    TitleOnly = 1,
    None = 2
  }

  public static class TitleModeExtensions
  {
    public static string AsString(this TitleMode mode)
    {
      return mode.ToString("g");
    }

    public static TitleMode AsTitleMode(string modeStr)
    {
      if (string.IsNullOrEmpty(modeStr))
      {
        return TitleMode.NumberAndTitle;
      }

      switch (modeStr)
      {
        case "NumberAndTitle":
        default:
          return TitleMode.NumberAndTitle;
        case "TitleOnly":
          return TitleMode.TitleOnly;
        case "None":
          return TitleMode.None;
      }
    }
  }
}