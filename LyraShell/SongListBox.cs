using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
  /// <summary>
  /// Summary description for SongListBox
  /// </summary>
  public class SongListBox : ListBox
  {
    [Category("Action")]
    public event ScrollEventHandler Scrolled = null;
    private int oldTopIndex = 0;

    public SongListBox()
    {
      base.DrawMode = DrawMode.OwnerDrawFixed;
      base.ItemHeight = 15;
      this.DrawItem += SongListBox_DrawItem;
      this.Sorted = false;
      this.bgBrush = new SolidBrush(base.BackColor);
      this.foreColBrush = new SolidBrush(base.ForeColor);
    }

    protected override void OnBackColorChanged(EventArgs e)
    {
      base.OnBackColorChanged(e);
      this.bgBrush = new SolidBrush(base.BackColor);
    }

    protected override void OnForeColorChanged(EventArgs e)
    {
      base.OnForeColorChanged(e);
      this.foreColBrush = new SolidBrush(base.ForeColor);
    }

    private readonly Brush highlight = new SolidBrush(Color.FromArgb(251, 225, 98));
    private readonly Brush highlightLight = new SolidBrush(Color.FromArgb(253, 253, 176));
    private readonly Brush normalTextColor = Brushes.Navy;
    private readonly Brush selectedBackColorOverlay = new SolidBrush(Color.FromArgb(80, 0, 85, 170));
    private Brush bgBrush;
    private Brush foreColBrush;
    private readonly Pen highlightBorder = new Pen(Color.FromArgb(251, 225, 98));
    private readonly Pen highlightBorderLight = new Pen(Color.FromArgb(253, 253, 176));
    private readonly Pen separatorLine = new Pen(Color.FromArgb(230, 220, 197));

    private readonly Font titleFont = new Font("Verdana", 9f, GraphicsUnit.Point);

    private const int RatingWidth = 50;
    private const int RatingOffset = 0;

    private void SongListBox_DrawItem(object sender, DrawItemEventArgs e)
    {
      if (e.Index < this.Items.Count && e.Index >= 0)
      {
        bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

        // background
        if (e.Index < this.nrOfNumberMatches)
        {
          e.Graphics.FillRectangle(highlightLight, e.Bounds);
        }
        else
        {
          e.Graphics.FillRectangle(bgBrush, e.Bounds);
        }
        if (selected)
        {
          e.Graphics.FillRectangle(selectedBackColorOverlay, e.Bounds);
        }

        object item = this.Items[e.Index];
        if (item is Song)
        {
          Song song = (Song)item;

          this.DrawString(e.Graphics, Util.toFour(song.Number), e.Font, Brushes.DimGray,
                              new RectangleF(e.Bounds.X, e.Bounds.Y, 50, e.Bounds.Height), highlightLight,
                              highlightBorderLight);

          // it's a number match...
          RectangleF titleFrame = new RectangleF(e.Bounds.X + 50, e.Bounds.Y, e.Bounds.Width - 50,
                                                e.Bounds.Height);
          this.DrawString(e.Graphics, song.Title, titleFont, normalTextColor, titleFrame, highlight, highlightBorder);

          if (this.nrOfNumberMatches <= e.Index)
          {
            if (this.method == SortMethod.RatingDescending || this.method == SortMethod.RatingAscending)
            {
              float rating = this.Ratings[song];
              float space = RatingWidth * rating;

              Brush b = new LinearGradientBrush(new Point(e.Bounds.Left, e.Bounds.Top),
                                                new Point(e.Bounds.Right, e.Bounds.Top), Color.FromArgb(100, 0, 85, 170), Color.FromArgb(148, 0, 85, 170));
              e.Graphics.FillRectangle(b, new RectangleF(e.Bounds.Right - space - RatingOffset, e.Bounds.Top, space, e.Bounds.Height));
            }
          }
        }
        else
        {
          e.Graphics.DrawString(item.ToString(), e.Font, foreColBrush, new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
        }
        if (this.nrOfNumberMatches > 0 && e.Index == this.nrOfNumberMatches - 1 && this.Items.Count > this.nrOfNumberMatches)
        {
          e.Graphics.DrawLine(separatorLine, e.Bounds.X, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
        }
        e.DrawFocusRectangle();
      }
      // scrolled?
      if (this.oldTopIndex != this.TopIndex)
      {
        ScrollEventArgs scrollArgs = new ScrollEventArgs(this.oldTopIndex > this.TopIndex ?
            ScrollEventType.SmallDecrement : ScrollEventType.SmallIncrement, this.TopIndex);
        this.OnScroll(scrollArgs);
      }
      this.oldTopIndex = this.TopIndex;
    }

    private IList<string> searchTags;

    public void SetSearchTags(IList<string> tags)
    {
      this.searchTags = new List<string>();
      foreach (string tag in tags)
      {
        this.searchTags.Add(tag.Replace("*", "").Replace("?", "").ToLowerInvariant());
      }
      this.Refresh();
    }

    private int nrOfNumberMatches = 0;

    public int NrOfNumberMatches
    {
      get { return this.nrOfNumberMatches; }
      set { this.nrOfNumberMatches = value; }
    }

    public void ResetSearchTags()
    {
      this.searchTags = null;
      this.Refresh();
    }

    private void DrawString(Graphics g, string text, Font font, Brush brush, RectangleF bounds, Brush highlightBrush, Pen highlightBorder)
    {
      if (!this.HasSearchTagMatch(text))
      {
        g.DrawString(text, font, brush, bounds);
      }
      else
      {
        string lowerText = QueryHelper.PrepareForComparison(text);
        List<CharacterRange> intervals = new List<CharacterRange>();
        foreach (string searchTag in searchTags)
        {
          int pos = lowerText.IndexOf(QueryHelper.PrepareForComparison(searchTag), StringComparison.InvariantCulture);
          while (pos >= 0 && pos < lowerText.Length)
          {
            intervals.Add(new CharacterRange(pos, searchTag.Length));
            pos = lowerText.IndexOf(searchTag, pos + 1, StringComparison.InvariantCulture);
          }
        }

        intervals.Sort((a, b) => a.First - b.First);
        for (int i = 0; i < intervals.Count - 1; i++)
        {
          CharacterRange interval = intervals[i];
          CharacterRange nextInterval = intervals[i + 1];
          if (interval.First + interval.Length >= nextInterval.First)
          {
            interval.Length = nextInterval.First + nextInterval.Length - interval.First;
            intervals.Remove(nextInterval);
            --i; // remain at this position
          }
        }
        if (intervals.Count == 0)
        {
          g.DrawString(text, font, brush, bounds);
          return; // should not occur
        }

        StringFormat stringFormat = new StringFormat();
        stringFormat.SetMeasurableCharacterRanges(intervals.ToArray());

        Region[] regions = g.MeasureCharacterRanges(text, font, bounds, stringFormat);
        foreach (Region region in regions)
        {
          RectangleF regBounds = region.GetBounds(g);
          regBounds.X--;
          regBounds.Width += 2;
          g.FillRectangle(highlightBrush, regBounds);
          g.DrawRectangle(highlightBorder, Rectangle.Round(regBounds));
        }

        g.DrawString(text, font, brush, bounds, stringFormat);
      }
    }

    private readonly IDictionary<ISong, float> ratings = new Dictionary<ISong, float>();

    public IDictionary<ISong, float> Ratings
    {
      get { return this.ratings; }
    }

    private int CompareRating(ISong a, ISong b)
    {
      float ar = 0f;
      float br = 0f;
      if (this.ratings.ContainsKey(a)) ar = this.ratings[a];
      if (this.ratings.ContainsKey(b)) br = this.ratings[b];

      if (ar == br)
      {
        return 0;
      }
      return ar > br ? -1 : 1;
    }

    private List<ISong> sortedSongs;
    private List<ISong> numberSongs;

    private SortMethod method;

    public void Sort(SortMethod method)
    {
      #region    Precondition

      if (sortedSongs == null ||
          numberSongs == null) return;

      #endregion Precondition

      this.method = method;

      this.BeginUpdate();

      switch (method)
      {
        case SortMethod.NumberAscending:
          sortedSongs.Sort((a, b) => a.Number.CompareTo(b.Number));
          break;
        case SortMethod.NumberDescending:
          sortedSongs.Sort((a, b) => -a.Number.CompareTo(b.Number));
          break;
        case SortMethod.RatingAscending:
          sortedSongs.Sort((a, b) => -CompareRating(a, b));
          break;
        case SortMethod.RatingDescending:
          sortedSongs.Sort(CompareRating);
          break;
        case SortMethod.TitleAscending:
          sortedSongs.Sort((a, b) => a.Title.CompareTo(b.Title));
          break;
        case SortMethod.TitleDescending:
          sortedSongs.Sort((a, b) => -a.Title.CompareTo(b.Title));
          break;
      }

      this.Items.Clear();
      this.Items.AddRange(numberSongs.ToArray());
      this.Items.AddRange(sortedSongs.ToArray());

      this.EndUpdate();
    }

    private bool HasSearchTagMatch(string text)
    {
      #region    Precondition

      if (this.searchTags == null) return false;

      #endregion Precondition

      return QueryHelper.Contains(text, searchTags);
    }

    protected virtual void OnScroll(ScrollEventArgs e)
    {
      if (this.Scrolled != null)
      {
        this.Scrolled(this, e);
      }
    }

    public void ShowSongs(List<ISong> numberSongs, List<ISong> songs, SortMethod method)
    {
      this.numberSongs = numberSongs;
      this.nrOfNumberMatches = numberSongs.Count;
      this.sortedSongs = songs;
      this.Sort(method);
    }

    public void ClearSongs()
    {
      this.BeginUpdate();
      this.numberSongs = null;
      this.nrOfNumberMatches = 0;
      this.sortedSongs = null;
      this.Items.Clear();
      this.ResetSearchTags();
      this.EndUpdate();
    }
  }
}
