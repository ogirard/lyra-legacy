using System.Drawing;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
  class ScrollVisualPanel : Panel
  {
    public ScrollVisualPanel()
    {
        this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

        this.BorderColor = Color.FromArgb(237, 237, 237);
        this.BackgroundColor = Color.White;
        this.ScrollBarColor = Color.FromArgb(222, 222, 222);
    }

    private ScrollDataEventArgs _scrollData = null;

    public void UpdateScrollData(ScrollDataEventArgs scrollData)
    {
        this._scrollData = scrollData;
        this.Refresh();
    }

    private Color _borderColor;
    private Pen _borderPen;

    public Color BorderColor
    {
      get { return this._borderColor; }
      set
      {
          this._borderColor = value;
          this._borderPen = new Pen(this._borderColor);
      }
    }

    private Color _scrollBarColor;
    private Brush _scrollBarBrush;

    public Color ScrollBarColor
    {
      get { return this._scrollBarColor; }
      set
      {
          this._scrollBarColor = value;
          this._scrollBarBrush = new SolidBrush(this._scrollBarColor);
      }
    }

    private Color _backgroundColor;
    private Brush _backgroundBrush;

    public Color BackgroundColor
    {
      get { return this._backgroundColor; }
      set
      {
          this._backgroundColor = value;
          this._backgroundBrush = new SolidBrush(this._backgroundColor);
      }
    }

    protected override void OnPaint(PaintEventArgs paintEventArgs)
    {
      base.OnPaint(paintEventArgs);

      var g = paintEventArgs.Graphics;
      var rect = paintEventArgs.ClipRectangle;

      g.FillRectangle(this._backgroundBrush, rect);

      if (this._scrollData != null)
      {
        if (this._scrollData.DesiredHeight == this._scrollData.DisplayHeight || this._scrollData.DesiredHeight == 0)
        {
          g.FillRectangle(this._scrollBarBrush, rect);
        }
        else
        {
          var fraction = (float)rect.Height / this._scrollData.DesiredHeight;
          var position = (int)(fraction * this._scrollData.ScrollPosition);
          var height = (int)(fraction * this._scrollData.DisplayHeight);
          var barRect = new Rectangle(rect.X, position, rect.Width, height);
          g.FillRectangle(this._scrollBarBrush, barRect);
          g.DrawRectangle(this._borderPen, barRect);
        }
      }

      g.DrawRectangle(this._borderPen, new Rectangle(rect.Location, new Size(rect.Width - 1, rect.Height - 1)));
    }
  }
}
