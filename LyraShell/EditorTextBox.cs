using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
    public class EditorTextBox : RichTextBox
    {
        private Size _contentSize;
        private bool _mouseOver = false;

        public EditorTextBox()
        {
            _contentSize = Size;
        }

        public bool IsMouseOver
        {
            get { return _mouseOver; }
        }

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
            SendMessage(new HandleRef(this, Handle), WM_VSCROLL, SB_LINEDOWN, IntPtr.Zero);
        }

        public void ScrollUp()
        {
            SendMessage(new HandleRef(this, Handle), WM_VSCROLL, SB_LINEUP, IntPtr.Zero);
        }

        public void ScrollPageDown()
        {
            SendMessage(new HandleRef(this, Handle), WM_VSCROLL, SB_PAGEDOWN, IntPtr.Zero);
        }

        public void ScrollPageUp()
        {
            SendMessage(new HandleRef(this, Handle), WM_VSCROLL, SB_PAGEUP, IntPtr.Zero);
        }

        public void ScrollToTop()
        {
            SendMessage(new HandleRef(this, Handle), WM_VSCROLL, SB_TOP, IntPtr.Zero);
        }

        public void ScrollToBottom()
        {
            SendMessage(new HandleRef(this, Handle), WM_VSCROLL, SB_BOTTOM, IntPtr.Zero);
        }

        public Point ScrollPosition
        {
            get
            {
                var scrollPoint = new Point();
                SendMessage(new HandleRef(this, Handle), EM_GETSCROLLPOS, 0, ref scrollPoint);
                return scrollPoint;
            }

            set
            {
                var scrollPoint = value;
                SendMessage(new HandleRef(this, Handle), EM_SETSCROLLPOS, 0, ref scrollPoint);
            }
        }

        public void ModifyWithStableScroll(Action<EditorTextBox> modify)
        {
            var temp = ScrollPosition;
            modify(this);
            ScrollPosition = temp;
        }

        public event EventHandler<ScrollDataEventArgs> ScrollDataChanged;

        private void OnScrollDataChanged()
        {
            ScrollDataChanged?.Invoke(this, new ScrollDataEventArgs
            {
                DesiredHeight = Math.Max(_contentSize.Height, Height),
                DisplayHeight = Height,
                ScrollPosition = ScrollPosition.Y
            });
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
}