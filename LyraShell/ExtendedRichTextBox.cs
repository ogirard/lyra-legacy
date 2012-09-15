using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
  /// <summary>
  /// This is a hack to get a nice transparent RichTextBox
  /// </summary>
  public class ExtendedRichTextBox : RichTextBox
  {
    private Size _contentSize;
    private bool _mouseOver = false;

    public ExtendedRichTextBox()
    {
      ScrollBars = RichTextBoxScrollBars.None;
      _contentSize = this.Size;
      Enabled = false;
    }

    public bool IsMouseOver
    {
      get { return _mouseOver; }
    }

    #region Transparency

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    static extern IntPtr LoadLibrary(string lpFileName);

    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams prams = base.CreateParams;
        if (LoadLibrary("msftedit.dll") != IntPtr.Zero)
        {
          prams.ExStyle |= 0x020; // transparent
          prams.ClassName = "RICHEDIT50W";
        }
        return prams;
      }
    }

    #endregion Transparency

    #region Scrolling

    // import user32 SendMessage function
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
    private static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern int SendMessage(HandleRef hWnd, uint wMsg, Int32 wParam, ref Point pt);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int GetScrollPos(IntPtr hWnd, Orientation nBar);

    [DllImport("user32.dll")]
    public static extern int SetScrollPos(IntPtr hWnd, Orientation nBar, int nPos, bool bRedraw);

    // msg HSCROLL | VSCROLL
    private const int WM_VSCROLL = 0x115;
    private const int WM_HSCROLL = 0x114;

    // params VSCROLL
    private readonly IntPtr SB_LINEUP = new IntPtr(0);
    private readonly IntPtr SB_LINEDOWN = new IntPtr(1);
    private readonly IntPtr SB_PAGEUP = new IntPtr(2);
    private readonly IntPtr SB_PAGEDOWN = new IntPtr(3);
    private readonly IntPtr SB_TOP = new IntPtr(6);
    private readonly IntPtr SB_BOTTOM = new IntPtr(7);

    private const int WM_USER = 0x0400;
    private const int EM_GETSCROLLPOS = WM_USER + 221;
    private const int EM_SETSCROLLPOS = 0x0400 + 222;

    public void ScrollDown()
    {
      SendMessage(new HandleRef(this, this.Handle), WM_VSCROLL, SB_LINEDOWN, IntPtr.Zero);
    }

    public void ScrollUp()
    {
      SendMessage(new HandleRef(this, this.Handle), WM_VSCROLL, SB_LINEUP, IntPtr.Zero);
    }

    public void ScrollPageDown()
    {
      SendMessage(new HandleRef(this, this.Handle), WM_VSCROLL, SB_PAGEDOWN, IntPtr.Zero);
    }

    public void ScrollPageUp()
    {
      SendMessage(new HandleRef(this, this.Handle), WM_VSCROLL, SB_PAGEUP, IntPtr.Zero);
    }

    public void ScrollToTop()
    {
      SendMessage(new HandleRef(this, this.Handle), WM_VSCROLL, SB_TOP, IntPtr.Zero);
    }

    public void ScrollToBottom()
    {
      SendMessage(new HandleRef(this, this.Handle), WM_VSCROLL, SB_BOTTOM, IntPtr.Zero);
    }

    private double _yFactor = 1.0d;

    public Point ScrollPosition
    {
      get
      {
        Point scrollPoint = new Point();

        SendMessage(new HandleRef(this, this.Handle), EM_GETSCROLLPOS, 0, ref scrollPoint);
        return scrollPoint;
      }
    }

    public event EventHandler<ScrollDataEventArgs> ScrollDataChanged;

    private void OnScrollDataChanged()
    {
      if (ScrollDataChanged != null)
      {
        ScrollDataChanged(this, new ScrollDataEventArgs
        {
          DesiredHeight = Math.Max(_contentSize.Height, this.Height),
          DisplayHeight = Height,
          ScrollPosition = ScrollPosition.Y
        });
      }
    }

    protected override void OnVScroll(EventArgs e)
    {
      base.OnVScroll(e);
      OnScrollDataChanged();
    }

    protected override void OnContentsResized(ContentsResizedEventArgs e)
    {
      base.OnContentsResized(e);
      _contentSize = e.NewRectangle.Size;
      OnScrollDataChanged();
    }

    protected override void OnMouseEnter(EventArgs e)
    {
      base.OnMouseEnter(e);
      _mouseOver = true;
    }

    protected override void OnMouseLeave(EventArgs e)
    {
      base.OnMouseLeave(e);
      _mouseOver = false;
    }

    #endregion Scrolling
  }

  public class ScrollDataEventArgs : EventArgs
  {
    public int DesiredHeight { get; set; }

    public int DisplayHeight { get; set; }

    public int ScrollPosition { get; set; }
  }

}