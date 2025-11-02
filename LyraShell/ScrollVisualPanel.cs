using System.Drawing;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
  class ScrollVisualPanel : Panel
  {
    public ScrollVisualPanel()
    {
        SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

        BorderColor = Color.FromArgb(237, 237, 237);
        BackgroundColor = Color.White;
        ScrollBarColor = Color.FromArgb(222, 222, 222);
    }

    private ScrollDataEventArgs _scrollData = null;

    public void UpdateScrollData(ScrollDataEventArgs scrollData)
    {
        _scrollData = scrollData;
        Refresh();
    }

    private Color _borderColor;
    private Pen _borderPen;

    public Color BorderColor
    {
      get { return _borderColor; }
      set
      {
          _borderColor = value;
          _borderPen = new Pen(_borderColor);
      }
    }

    private Color _scrollBarColor;
    private Brush _scrollBarBrush;

    public Color ScrollBarColor
    {
      get { return _scrollBarColor; }
      set
      {
          _scrollBarColor = value;
          _scrollBarBrush = new SolidBrush(_scrollBarColor);
      }
    }

    private Color _backgroundColor;
    private Brush _backgroundBrush;

    public Color BackgroundColor
    {
      get { return _backgroundColor; }
      set
      {
          _backgroundColor = value;
          _backgroundBrush = new SolidBrush(_backgroundColor);
      }
    }

    protected override void OnPaint(PaintEventArgs paintEventArgs)
    {
      base.OnPaint(paintEventArgs);

      var g = paintEventArgs.Graphics;
      var rect = paintEventArgs.ClipRectangle;

      g.FillRectangle(_backgroundBrush, rect);

      if (_scrollData != null)
      {
        if (_scrollData.DesiredHeight == _scrollData.DisplayHeight || _scrollData.DesiredHeight == 0)
        {
          g.FillRectangle(_scrollBarBrush, rect);
        }
        else
        {
          var fraction = (float)rect.Height / _scrollData.DesiredHeight;
          var position = (int)(fraction * _scrollData.ScrollPosition);
          var height = (int)(fraction * _scrollData.DisplayHeight);
          var barRect = new Rectangle(rect.X, position, rect.Width, height);
          g.FillRectangle(_scrollBarBrush, barRect);
          g.DrawRectangle(_borderPen, barRect);
        }
      }

      g.DrawRectangle(_borderPen, new Rectangle(rect.Location, new Size(rect.Width - 1, rect.Height - 1)));
    }
  }
}
