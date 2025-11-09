using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Infragistics.Win.Misc;

namespace Lyra2.LyraShell
{
    /// <summary>
    ///   Zusammendfassende Beschreibung für View.
    /// </summary>
    public class View : Form
    {
        public enum ViewActions
        {
            ScrollUp,
            ScrollDown,
            ScrollPageUp,
            ScrollPageDown,
            ScrollToTop,
            ScrollToEnd,
            NextSong,
            PreviewsSong,
        }

        /// <summary>
        ///   Erforderliche Designervariable.
        /// </summary>
        private readonly Container _components = new Container();

        private ContextMenu _contextMenu1;
        private MenuItem _menuItem1;
        private MenuItem _menuItem2;
        private MenuItem _menuItem3;
        private MenuItem _menuItem4;
        private ExtendedRichTextBox _richTextBox1;
        private ExtendedRichTextBox _richTextBox2;
        private Panel _viewPanel;
        private MenuItem _menuItem5;
        private MenuItem _menuItem7;
        private MenuItem _menuItem6;
        private PictureBox _pictureBox1;
        private Label _label5;
        private Panel _panel2;
        private Label _label8;
        private UltraPanel _lyraBtn;
        private Label _label9;
        private Panel _panel4;

        public static Screen Display = Screen.PrimaryScreen;
        private GUI _owner;
        public static bool Black;
        private static int _countViews;
        private ISong _song;
        private string _source;
        private ITranslation _trans;
        private ViewTitle _title;

        private static readonly ArrayList _songHistory = new ArrayList();
        private ScrollVisualPanel _scrollVisual;

        private const short LyraBtnMinimumAlpha = 64;

        public static event EventHandler<ScrollDataEventArgs> ScrollDataChanged;

        private static void OnScrollDataChanged(ScrollDataEventArgs args)
        {
            if (ScrollDataChanged != null)
            {
                ScrollDataChanged(_this, args);
            }
        }

        #region    Styling

        private Color ResolvedForegroundColor
        {
            get { return _song != null ? _song.Style.ForegroundColor ?? Util.COLOR : Util.COLOR; }
        }

        private Color ResolvedRefrainForegroundColor
        {
            get
            {
                return _song != null ? _song.Style.ForegroundColor ?? Util.REFCOLOR : Util.REFCOLOR;
            }
        }

        private Color ResolveBackgroundColor
        {
            get
            {
                return _song != null ? _song.Style.BackgroundColor ?? Util.BGCOLOR : Util.BGCOLOR;
            }
        }

        private Font ResolvedFont
        {
            get { return _song != null ? _song.Style.GetFont() ?? Util.FONT : Util.FONT; }
        }

        private Image GetResolvedBackgroundImage()
        {
            if (_song != null && _song.Style.HasBackgroundImage)
            {
                return _song.Style.GetBackgroundImage(new Size(Width, Height), Util.KEEPRATIO);
            }

            return null;
        }

        private Color ResolveTitleForegroundColor
        {
            get
            {
                return _song != null
                    ? _song.Style.TitleForegroundColor ?? Util.TITLECOLOR
                    : Util.TITLECOLOR;
            }
        }

        private Font ResolvedTitleFont
        {
            get
            {
                return _song != null
                    ? _song.Style.GetTitleFont() ?? Util.TITLEFONT
                    : Util.TITLEFONT;
            }
        }

        private Color ResolveTitleBackgroundColor
        {
            get
            {
                return _song != null
                    ? _song.Style.TitleBackgroundColor ?? Util.TITLEBGCOLOR
                    : Util.TITLEBGCOLOR;
            }
        }

        #endregion Styling

        private static void addSongToHistory(ISong song)
        {
            if (_songHistory.Contains(song))
            {
                _songHistory.Remove(song);
            }
            _songHistory.Add(song);
            if (HistoryChanged != null)
            {
                HistoryChanged(null, EventArgs.Empty);
            }
        }

        public static ArrayList SongHistory
        {
            get { return _songHistory; }
        }

        public static event EventHandler HistoryChanged;

        public static event SongDisplayedEventHandler SongDisplayed;

        private static void OnSongDisplayed(SongDisplayedEventArgs args)
        {
            if (SongDisplayed != null && _this != null)
            {
                SongDisplayed(_this, args);
            }
        }

        private static SongDisplayedEventArgs currentSongInfo;

        public static SongDisplayedEventArgs CurrentSongInfo
        {
            get { return currentSongInfo; }
        }

        private static View _this;

        ///<summary>
        ///  Executes action on left left rich-textbox
        ///</summary>
        ///<param name="action"> <see cref="ViewActions" /> </param>
        ///<returns> <code>true</code> if view active, <code>false</code> otherwise </returns>
        public static bool ExecuteActionOnView(ViewActions action)
        {
            if (_this != null)
            {
                switch (action)
                {
                    case ViewActions.NextSong:
                        _this.MoveNext_Handler(_this, EventArgs.Empty);
                        break;
                    case ViewActions.PreviewsSong:
                        _this.MovePrevious_Handler(_this, EventArgs.Empty);
                        break;
                    case ViewActions.ScrollUp:
                        _this._richTextBox1.ScrollUp();
                        break;
                    case ViewActions.ScrollDown:
                        _this._richTextBox1.ScrollDown();
                        break;
                    case ViewActions.ScrollPageUp:
                        _this._richTextBox1.ScrollPageUp();
                        break;
                    case ViewActions.ScrollPageDown:
                        _this._richTextBox1.ScrollPageDown();
                        break;
                    case ViewActions.ScrollToTop:
                        _this._richTextBox1.ScrollToTop();
                        break;
                    case ViewActions.ScrollToEnd:
                        _this._richTextBox1.ScrollToBottom();
                        break;
                    default:
                        break;
                }
                return true;
            }
            return false;
        }

        public static void ScrollToPosition(int charPos)
        {
            #region    Precondition

            if (_this == null)
                return;

            #endregion Precondition

            _this._richTextBox1.Select(charPos, 0);
            _this._richTextBox1.ScrollToCaret();
        }

        public static void ShowSong(
            ISong song,
            ITranslation trans,
            GUI owner,
            ListBox navigate,
            string source
        )
        {
            if (_this == null)
            {
                _this = new View();
                _this._richTextBox1.ScrollDataChanged += OnRichTextBox1OnScrollDataChanged;
            }

            _this._song = song;
            _this._source = source;
            _this._menuItem1.Visible = false;
            _this._owner = owner;
            _this.navigate = navigate;
            _this._menuItem6.Checked = Util.SHOWRIGHT;

            if (trans != null)
            {
                _this.RefreshSong(song, trans);
            }
            else
            {
                _this.RefreshSong(song);
            }
            _this._richTextBox1.Focus();
            _this.pos = navigate.Items.IndexOf(_this._song);
            _this.Show();
        }

        private static void OnRichTextBox1OnScrollDataChanged(
            object sender,
            ScrollDataEventArgs args
        )
        {
            OnScrollDataChanged(args);
        }

        public static void BlackScreen(bool on)
        {
            Black = on;
            if (_this != null)
            {
                _this._panel4.Bounds = _this.Bounds; // assert the correct bounds
                _this._panel4.Visible = on;
            }
        }

        public static void ShowSong(ISong song, GUI owner, ListBox navigate, string source)
        {
            ShowSong(song, null, owner, navigate, source);
            _this._menuItem1.Visible = true;
        }

        public static void CloseView()
        {
            if (_this != null)
            {
                _this.Close();
            }
        }

        public View()
        {
            InitializeComponent();
            InitializeContextMenu();
            InitializeSongPresenters();
            _scrollVisual.Visible = false;

            Closed += ViewClosedHandler;
            Closing += ViewClosingHandler;
            ScrollDataChanged += OnScrollDataChanged;
        }

        private void OnScrollDataChanged(object sender, ScrollDataEventArgs scrollDataEventArgs)
        {
            _scrollVisual.UpdateScrollData(scrollDataEventArgs);
        }

        private ListBox navigate;
        private int pos;

        private int NextPos
        {
            get
            {
                pos = (pos + navigate.Items.Count + 1) % navigate.Items.Count;
                return pos;
            }
        }

        private int LastPos
        {
            get
            {
                pos = (pos + navigate.Items.Count - 1) % navigate.Items.Count;
                return pos;
            }
        }

        public void RefreshSong(ISong song, ITranslation trans)
        {
            _song = song;
            _trans = trans;
            RefreshSong();
        }

        public void RefreshSong(ISong song)
        {
            if (_song != null)
                _song.uncheck();
            RefreshSong(song, null);
        }

        private bool haspgbr;

        public void RefreshSong()
        {
            addSongToHistory(_song);
            SetCurrentSongInfo();
            _richTextBox1.Font = _this.ResolvedFont;
            _richTextBox2.Font = _this.ResolvedFont;
            BackColor = _this.ResolveBackgroundColor;
            if (_trans == null)
                transCount = 0;

            // update title and number
            _title.Number = _song.Number.ToString();
            _title.Title = _song.Title;
            _title.Mode =
                _song != null && _song.Style != null
                    ? _song.Style.TitleMode
                    : TitleMode.NumberAndTitle;
            _title.TitleForegroundColor = ResolveTitleForegroundColor;
            _title.TitleBackgroundColor = ResolveTitleBackgroundColor;
            _lyraBtn.Appearance.BackColor =
                _title.Mode == TitleMode.None ? Color.Transparent : ResolveTitleBackgroundColor;
            _title.TitleFont = ResolvedTitleFont;

            UpdatePresenterSizeAndPosition();
            _richTextBox2.Text = "";
            _richTextBox2.Visible = false;

            RichTextBox myrtb = _richTextBox1;
            if (_trans != null)
            {
                _menuItem7.Visible = true;
                if (Util.SHOWRIGHT && !haspgbr)
                {
                    myrtb = _richTextBox2;
                    _richTextBox2.Top = _richTextBox1.Top;
                    _richTextBox2.Height = _richTextBox1.Height;
                    _richTextBox2.Width = (_richTextBox2.Parent.Width - 24) / 2 - 6;
                    _richTextBox1.Width = _richTextBox2.Width;
                    _richTextBox2.Left = _richTextBox1.Right + 12;
                    _richTextBox2.Visible = true;
                }

                myrtb.Text = _trans.Text;
                if (_trans.Unformatted)
                {
                    myrtb.Text += "\n\n<special>(wird nicht gesungen)</special>";
                }
            }
            else
            {
                _menuItem7.Visible = false;
                _richTextBox1.Text = _song.Text;
                var findpos = 0;
                haspgbr = false;
                if (
                    (
                        findpos = _richTextBox1.Find(
                            "<" + Util.PGBR + " />",
                            0,
                            RichTextBoxFinds.MatchCase
                        )
                    ) != -1
                )
                {
                    var text = _richTextBox1.Text.Substring(0, findpos);
                    _richTextBox2.Text = _richTextBox1.Text.Substring(
                        findpos + Util.PGBR.Length + 5
                    );
                    if (Util.SHOWRIGHT)
                    {
                        _richTextBox1.Text = text;
                        _richTextBox2.Visible = true;
                        formatall(myrtb);
                        myrtb = _richTextBox2;
                        haspgbr = true;
                        _richTextBox2.Top = _richTextBox1.Top;
                        _richTextBox2.Height = _richTextBox1.Height;
                        _richTextBox2.Width = (_richTextBox2.Parent.Width - 24) / 2 - 6;
                        _richTextBox1.Width = _richTextBox2.Width;
                        _richTextBox2.Left = _richTextBox1.Right + 12;
                    }
                    else
                    {
                        _richTextBox1.Text = text + "\n" + _richTextBox2.Text;
                    }
                }
            }

            if (_contextMenu1.MenuItems.Count == 5)
                _contextMenu1.MenuItems.RemoveAt(3);

            if (_song.GetTransMenu(this) != null)
            {
                _contextMenu1.MenuItems.Add(3, _song.GetTransMenu(this));
            }

            if (_trans != null && _trans.Unformatted)
            {
                formatUnform(myrtb);
            }
            else
            {
                formatall(myrtb);
            }

            if (Util.SHOWNR && Util.CTRLSHOWNR)
            {
                (new Thread(ShowNrAtStart)).Start();
            }
            else
            {
                _panel2.Hide();
            }

            BackgroundImage = GetResolvedBackgroundImage();

            _richTextBox1.ScrollToTop();
            _richTextBox2.ScrollToTop();
            OnSongDisplayed(currentSongInfo);
            Focus();
        }

        private void UpdatePresenterSizeAndPosition()
        {
            if (_richTextBox1 == null)
            {
                return;
            }

            _richTextBox1.Left = 12;
            _richTextBox1.Width = _richTextBox1.Parent.Width - 24;
            _richTextBox1.Top = _title.Visible ? _title.Bottom + 12 : 12;
            _richTextBox1.Height = _richTextBox1.Parent.Height - 12 - _richTextBox1.Top;
            _scrollVisual.Top = _richTextBox1.Top;
            _scrollVisual.Height = _richTextBox1.Height;
        }

        private void SetCurrentSongInfo()
        {
            var currentSongPosition = navigate.Items.IndexOf(_song);
            currentSongInfo = new SongDisplayedEventArgs(
                _song,
                (ISong)
                    navigate.Items[
                        (currentSongPosition + 1 + navigate.Items.Count) % navigate.Items.Count
                    ],
                (ISong)
                    navigate.Items[
                        (currentSongPosition - 1 + navigate.Items.Count) % navigate.Items.Count
                    ],
                _source
            );
        }

        private void formatall(RichTextBox rtb)
        {
            rtb.SelectAll();
            rtb.SelectionFont = ResolvedFont;
            rtb.SelectionColor = ResolvedForegroundColor;
            // Format Refrain
            if (Util.refmode)
            {
                format(
                    rtb,
                    Util.REF,
                    ResolvedFont,
                    ResolvedRefrainForegroundColor,
                    16,
                    "Refrain:" + Util.RTNL,
                    ""
                );
            }
            else
            {
                format(
                    rtb,
                    Util.REF,
                    new Font(ResolvedFont, FontStyle.Bold),
                    ResolvedRefrainForegroundColor,
                    0,
                    "",
                    ""
                );
            }

            format(rtb, Util.SPEC, Util.SPECFONT, ResolvedForegroundColor, 0, "", "");
            formatBlock(rtb);
            format(rtb, Util.BOLD, null, ResolvedForegroundColor, 0, "", "");
            format(rtb, Util.ITALIC, null, ResolvedForegroundColor, 0, "", "");
            GetJumpMarks(rtb);
        }

        private void GetJumpMarks(RichTextBox rtb)
        {
            var start = 0;
            var startText = 0;
            currentSongInfo.Jumpmarks.Clear();
            while ((start = rtb.Find("<" + Util.JMP, start, RichTextBoxFinds.MatchCase)) >= 0)
            {
                var end = 0;
                var endText = 0;
                if ((end = rtb.Find("/>", start, RichTextBoxFinds.MatchCase)) >= 0)
                {
                    startText = rtb.Text.IndexOf("<" + Util.JMP, startText);
                    endText = rtb.Text.IndexOf("/>", startText) + 2;
                    var jumpmark = rtb.Text.Substring(startText, endText - startText);
                    currentSongInfo.Jumpmarks.Add(new JumpMark(jumpmark, start));
                    rtb.Rtf = rtb.Rtf.Replace(jumpmark, "");
                    start = 0;
                }
            }
        }

        private void formatUnform(RichTextBox rtb)
        {
            format(
                rtb,
                Util.REF,
                Util.TRANSFONT,
                ResolvedRefrainForegroundColor,
                0,
                "Refrain :" + Util.RTNL,
                ""
            );
            format(
                rtb,
                Util.SPEC,
                new Font(Util.SPECFONT.FontFamily, Util.TRANSFONT.Size, Util.SPECFONT.Style),
                ResolvedForegroundColor,
                0,
                "",
                ""
            );
            formatBlock(rtb);
            format(rtb, Util.BOLD, Util.TRANSFONT, ResolvedForegroundColor, 0, "", "");
            format(rtb, Util.ITALIC, Util.TRANSFONT, ResolvedForegroundColor, 0, "", "");
            rtb.SelectAll();
            rtb.SelectionFont = Util.TRANSFONT;
        }

        private void format(
            RichTextBox rtb,
            string tag,
            Font font,
            Color c,
            int offset,
            string l,
            string r
        )
        {
            var start = 0;
            int len;

            // Collect all tag positions first before modifying RTF
            var tagPositions =
                new List<(int start, int end, Font selectedFont, Color selectedColor)>();

            while ((start = rtb.Find("<" + tag + ">", start, RichTextBoxFinds.MatchCase)) >= 0)
            {
                start += tag.Length + 2;
                len = rtb.Find("</" + tag + ">", start, RichTextBoxFinds.MatchCase) - start;
                rtb.Select(start, len);

                // Store current formatting before we modify anything
                var currentFont = rtb.SelectionFont;
                var currentColor = rtb.SelectionColor;

                tagPositions.Add((start, start + len, currentFont, currentColor));
            }

            // Now remove all tag markers from RTF (this happens before formatting)
            rtb.Rtf = rtb.Rtf.Replace("<" + tag + ">", l);
            rtb.Rtf = rtb.Rtf.Replace("</" + tag + ">", r);

            // Reapply formatting based on stored positions
            // Adjust positions for the removed opening tags
            var adjustment = 0;
            foreach (var (tagStart, tagEnd, selFont, selColor) in tagPositions)
            {
                // Account for removed opening tag markers
                var openingTagLength = tag.Length + 2; // "<tag>"
                var adjustedStart = tagStart - openingTagLength - adjustment;
                var adjustedLen = tagEnd - tagStart;

                rtb.Select(adjustedStart, adjustedLen);
                var stylefont = font ?? selFont;

                if (tag == Util.BOLD)
                {
                    if (stylefont != null)
                    {
                        var currentStyle = stylefont.Style;
                        var newStyle = currentStyle | FontStyle.Bold;
                        if (newStyle != currentStyle)
                        {
                            rtb.SelectionFont = new Font(stylefont, newStyle);
                        }
                    }
                    else
                    {
                        rtb.SelectionFont = new Font(ResolvedFont, FontStyle.Bold);
                    }
                }
                else if (tag == Util.ITALIC)
                {
                    if (stylefont != null)
                    {
                        var currentStyle = stylefont.Style;
                        var newStyle = currentStyle | FontStyle.Italic;
                        if (newStyle != currentStyle)
                        {
                            rtb.SelectionFont = new Font(stylefont, newStyle);
                        }
                    }
                    else
                    {
                        rtb.SelectionFont = new Font(ResolvedFont, FontStyle.Italic);
                    }
                }
                else if (stylefont != null)
                {
                    rtb.SelectionFont = stylefont;
                }
                rtb.SelectionColor = c;
                rtb.SelectionIndent += offset;

                adjustment += openingTagLength + (tag.Length + 3); // closing tag "</" + tag + ">"
            }

            if (tag == Util.REF)
            {
                var start2 = 0;
                while ((start2 = rtb.Find("Refrain:", ++start2, RichTextBoxFinds.MatchCase)) >= 0)
                {
                    if (start2 == 1) //remove first \par\r
                    {
                        rtb.Rtf = rtb.Rtf.Remove(rtb.Rtf.IndexOf("Refrain:") - 6, 6);
                        start2 = 0;
                    }
                    rtb.Select(start2, 8);
                    rtb.SelectionFont = new Font(rtb.SelectionFont, FontStyle.Bold);
                    rtb.SelectionColor = ResolvedRefrainForegroundColor;
                    rtb.SelectionIndent = 0;
                }
            }
        }

        private void formatBlock(RichTextBox rtb)
        {
            var start = 0;
            while ((start = rtb.Find("<" + Util.BLOCK, start, RichTextBoxFinds.MatchCase)) >= 0)
            {
                var tag = rtb.Text.Substring(++start, 3);
                if (tag[2] == '>')
                {
                    tag = tag.Substring(0, 2);
                }
                var offset = Int32.Parse(tag.Substring(1, tag.Length - 1));

                // Store the current formatting of the first character within the <p> block
                var blockStart = rtb.Find(
                    "<" + Util.BLOCK + tag.Substring(1) + ">",
                    start - 1,
                    RichTextBoxFinds.MatchCase
                );
                var blockTextStart = blockStart + Util.BLOCK.Length + tag.Length + 2;
                rtb.Select(blockTextStart, 1);

                var preservedFont = rtb.SelectionFont ?? ResolvedFont;
                var preservedColor = rtb.SelectionColor;

                // Now format the block with preserved font and color
                format(rtb, tag, preservedFont, preservedColor, offset, "", "");
            }
        }

        /// <summary>
        ///   Die verwendeten Ressourcen bereinigen.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_components != null)
                {
                    _components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void UpdateViewPanel()
        {
            _viewPanel.BackColor = Color.Transparent;
            _viewPanel.Top = 0;
            _viewPanel.Height = _viewPanel.Parent.Height;
            _viewPanel.Width = Math.Min(_viewPanel.Parent.Height * 3 / 2, Width);
            _viewPanel.Left = Math.Max((_viewPanel.Parent.Width - _viewPanel.Width) / 2, Left);
        }

        private void InitializeSongPresenters()
        {
            _viewPanel = new Panel();
            _richTextBox1 = new ExtendedRichTextBox();
            _richTextBox2 = new ExtendedRichTextBox();

            //
            // viewPanel
            //
            Controls.Add(_viewPanel);
            UpdateViewPanel();

            //
            // richTextBox1
            //

            _viewPanel.Controls.Add(_richTextBox1);
            _richTextBox1.BackColor = Color.White;
            _richTextBox1.BorderStyle = BorderStyle.None;
            _richTextBox1.Cursor = Cursors.Arrow;
            _richTextBox1.Font = new Font(
                "Microsoft Sans Serif",
                12F,
                FontStyle.Regular,
                GraphicsUnit.Point,
                0
            );
            _richTextBox1.Name = "richTextBox1";
            _richTextBox1.ReadOnly = true;
            _richTextBox1.TabIndex = 1;
            _richTextBox1.TabStop = false;
            _richTextBox1.Text = "Text";
            _richTextBox1.KeyDown += OnKeyDown;
            _richTextBox1.GotFocus += HandlePresenterFocus;
            _richTextBox1.Anchor =
                AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top;
            _richTextBox1.MouseEnter += PresenterMouseEnterHandler;
            _richTextBox1.MouseLeave += PresenterMouseLeaveHandler;

            //
            // richTextBox2
            //
            _viewPanel.Controls.Add(_richTextBox2);
            _richTextBox2.BackColor = Color.White;
            _richTextBox2.BorderStyle = BorderStyle.None;
            _richTextBox2.Cursor = Cursors.Arrow;
            _richTextBox2.Font = new Font(
                "Microsoft Sans Serif",
                12F,
                FontStyle.Regular,
                GraphicsUnit.Point,
                0
            );
            _richTextBox2.Name = "richTextBox2";
            _richTextBox2.ReadOnly = true;
            _richTextBox2.TabIndex = 7;
            _richTextBox2.TabStop = false;
            _richTextBox2.Text = "TextRight";
            _richTextBox2.Visible = false;
            _richTextBox2.KeyDown += OnKeyDown;
            _richTextBox2.GotFocus += HandlePresenterFocus;
            _richTextBox2.MouseEnter += PresenterMouseEnterHandler;
            _richTextBox2.MouseLeave += PresenterMouseLeaveHandler;
        }

        private void PresenterMouseLeaveHandler(object sender, EventArgs e)
        {
            _scrollVisual.Visible = false;
        }

        private void PresenterMouseEnterHandler(object sender, EventArgs e)
        {
            _scrollVisual.Visible = true;
        }

        private void HandlePresenterFocus(object sender, EventArgs e)
        {
            _title.Focus();
        }

        private void InitializeContextMenu()
        {
            _contextMenu1 = new ContextMenu();
            _menuItem1 = new MenuItem();
            _menuItem3 = new MenuItem();
            _menuItem4 = new MenuItem();
            _menuItem6 = new MenuItem();
            _menuItem7 = new MenuItem();
            _menuItem5 = new MenuItem();
            _menuItem2 = new MenuItem();

            // menuItem1
            //
            _menuItem1.Index = 0;
            _menuItem1.MenuItems.AddRange(new[] { _menuItem3, _menuItem4 });
            _menuItem1.Text = "&Navigation";
            //
            // menuItem3
            //
            _menuItem3.Index = 0;
            _menuItem3.Text = "&>>";
            _menuItem3.Click += MoveNext_Handler;
            //
            // menuItem4
            //
            _menuItem4.Index = 1;
            _menuItem4.Text = "&<<";
            _menuItem4.Click += MovePrevious_Handler;
            //
            // menuItem6
            //
            _menuItem6.Index = 1;
            _menuItem6.Text = "&rechtes Fenster benützen";
            _menuItem6.Click += menuItem6_Click;
            //
            // menuItem7
            //
            _menuItem7.Index = 2;
            _menuItem7.Text = "&Originaltext anzeigen";
            _menuItem7.Visible = false;
            _menuItem7.Click += menuItem7_Click;
            //
            // menuItem5
            //
            _menuItem5.Index = 3;
            _menuItem5.Text = "Über&setzungen";
            //
            // menuItem2
            //
            _menuItem2.Index = 4;
            _menuItem2.Text = "schlie&ssen";
            _menuItem2.Click += menuItem2_Click;

            //
            // contextMenu1
            //
            _contextMenu1.MenuItems.AddRange(
                new[] { _menuItem1, _menuItem6, _menuItem7, _menuItem5, _menuItem2 }
            );

            _lyraBtn.ContextMenu = _contextMenu1;
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///   Erforderliche Methode für die Designerunterstützung. Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(View));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this._label5 = new System.Windows.Forms.Label();
            this._panel2 = new System.Windows.Forms.Panel();
            this._label9 = new System.Windows.Forms.Label();
            this._pictureBox1 = new System.Windows.Forms.PictureBox();
            this._label8 = new System.Windows.Forms.Label();
            this._lyraBtn = new Infragistics.Win.Misc.UltraPanel();
            this._panel4 = new System.Windows.Forms.Panel();
            this._title = new Lyra2.LyraShell.ViewTitle();
            this._scrollVisual = new Lyra2.LyraShell.ScrollVisualPanel();
            this._panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox1)).BeginInit();
            this._lyraBtn.SuspendLayout();
            this.SuspendLayout();
            //
            // label5
            //
            this._label5.AutoSize = true;
            this._label5.BackColor = System.Drawing.Color.Transparent;
            this._label5.Font = new System.Drawing.Font(
                "Verdana",
                65.25F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0))
            );
            this._label5.ForeColor = System.Drawing.Color.DimGray;
            this._label5.Location = new System.Drawing.Point(432, 40);
            this._label5.Name = "_label5";
            this._label5.Size = new System.Drawing.Size(293, 106);
            this._label5.TabIndex = 9;
            this._label5.Text = "1000";
            this._label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            //
            // panel2
            //
            this._panel2.Controls.Add(this._label9);
            this._panel2.Controls.Add(this._label5);
            this._panel2.Controls.Add(this._pictureBox1);
            this._panel2.Location = new System.Drawing.Point(21, 414);
            this._panel2.Name = "_panel2";
            this._panel2.Size = new System.Drawing.Size(752, 176);
            this._panel2.TabIndex = 11;
            //
            // label9
            //
            this._label9.AutoSize = true;
            this._label9.BackColor = System.Drawing.Color.Transparent;
            this._label9.Font = new System.Drawing.Font(
                "Verdana",
                48F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0))
            );
            this._label9.ForeColor = System.Drawing.Color.DarkGray;
            this._label9.Location = new System.Drawing.Point(272, 56);
            this._label9.Name = "_label9";
            this._label9.Size = new System.Drawing.Size(136, 78);
            this._label9.TabIndex = 10;
            this._label9.Text = "NB";
            this._label9.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            //
            // pictureBox1
            //
            this._pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this._pictureBox1.Image = (
                (System.Drawing.Image)(resources.GetObject("pictureBox1.Image"))
            );
            this._pictureBox1.Location = new System.Drawing.Point(24, 16);
            this._pictureBox1.Name = "_pictureBox1";
            this._pictureBox1.Size = new System.Drawing.Size(216, 136);
            this._pictureBox1.TabIndex = 8;
            this._pictureBox1.TabStop = false;
            //
            // label8
            //
            this._label8.BackColor = System.Drawing.Color.Transparent;
            this._label8.Font = new System.Drawing.Font(
                "Microsoft Sans Serif",
                6.75F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0))
            );
            this._label8.ForeColor = System.Drawing.Color.DarkGray;
            this._label8.Location = new System.Drawing.Point(576, 280);
            this._label8.Name = "_label8";
            this._label8.Size = new System.Drawing.Size(56, 32);
            this._label8.TabIndex = 12;
            this._label8.Text = "next: 9999";
            this._label8.Visible = false;
            //
            // lyraBtn
            //
            this._lyraBtn.Anchor = (
                (System.Windows.Forms.AnchorStyles)(
                    (
                        System.Windows.Forms.AnchorStyles.Top
                        | System.Windows.Forms.AnchorStyles.Right
                    )
                )
            );
            appearance1.AlphaLevel = ((short)(64));
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            appearance1.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance1.ImageAlpha = Infragistics.Win.Alpha.Transparent;
            appearance1.ImageBackground = (
                (System.Drawing.Image)(resources.GetObject("appearance1.ImageBackground"))
            );
            appearance1.ImageBackgroundAlpha = Infragistics.Win.Alpha.UseAlphaLevel;
            this._lyraBtn.Appearance = appearance1;
            this._lyraBtn.Location = new System.Drawing.Point(1054, 9);
            this._lyraBtn.Name = "_lyraBtn";
            this._lyraBtn.Size = new System.Drawing.Size(32, 32);
            this._lyraBtn.TabIndex = 13;
            this._lyraBtn.UseAppStyling = false;
            this._lyraBtn.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this._lyraBtn.MouseClickClient += this.LyraButtonClickHandler;
            this._lyraBtn.MouseEnterClient += new System.EventHandler(
                this.LyraButtonMouseEnterHandler
            );
            this._lyraBtn.MouseLeaveClient += new System.EventHandler(
                this.LyraButtonMouseLeaveHandler
            );
            //
            // panel4
            //
            this._panel4.BackColor = System.Drawing.Color.Black;
            this._panel4.Location = new System.Drawing.Point(48, 272);
            this._panel4.Name = "_panel4";
            this._panel4.Size = new System.Drawing.Size(64, 40);
            this._panel4.TabIndex = 14;
            this._panel4.Visible = false;
            //
            // title
            //
            this._title.Anchor = (
                (System.Windows.Forms.AnchorStyles)(
                    (
                        (
                            System.Windows.Forms.AnchorStyles.Top
                            | System.Windows.Forms.AnchorStyles.Left
                        ) | System.Windows.Forms.AnchorStyles.Right
                    )
                )
            );
            this._title.BackColor = System.Drawing.Color.Transparent;
            this._title.Location = new System.Drawing.Point(0, 0);
            this._title.Mode = Lyra2.LyraShell.TitleMode.NumberAndTitle;
            this._title.Name = "_title";
            this._title.Number = "1050";
            this._title.Size = new System.Drawing.Size(1096, 16);
            this._title.TabIndex = 15;
            this._title.Title = "A test title";
            this._title.TitleBackgroundColor = System.Drawing.Color.Empty;
            this._title.TitleFont = null;
            this._title.TitleForegroundColor = System.Drawing.Color.Empty;
            this._title.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            //
            // _scrollVisual
            //
            this._scrollVisual.Anchor = (
                (System.Windows.Forms.AnchorStyles)(
                    (
                        (
                            System.Windows.Forms.AnchorStyles.Top
                            | System.Windows.Forms.AnchorStyles.Bottom
                        ) | System.Windows.Forms.AnchorStyles.Right
                    )
                )
            );
            this._scrollVisual.Location = new System.Drawing.Point(1085, 51);
            this._scrollVisual.Name = "_scrollVisual";
            this._scrollVisual.Size = new System.Drawing.Size(6, 519);
            this._scrollVisual.TabIndex = 16;
            //
            // View
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1096, 576);
            this.ControlBox = false;
            this.Controls.Add(this._scrollVisual);
            this.Controls.Add(this._lyraBtn);
            this.Controls.Add(this._title);
            this.Controls.Add(this._panel4);
            this.Controls.Add(this._label8);
            this.Controls.Add(this._panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "View";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this._panel2.ResumeLayout(false);
            this._panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox1)).EndInit();
            this._lyraBtn.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            // init Screen
            Width = Display.Bounds.Width;
            Height = Display.Bounds.Height;
            Top = Display.Bounds.Top;
            Left = Display.Bounds.Left;

            _label8.Top = _label8.Parent.Height - _label8.Height;
            _label8.Left = _label8.Parent.Width - _label8.Width;

            UpdateViewPanel();

            _richTextBox2.Top = _richTextBox1.Top;
            _richTextBox2.Height = _richTextBox1.Height;
            _richTextBox2.Width = _richTextBox1.Width / 2 - 5;
            _richTextBox2.Left = _richTextBox1.Left + _richTextBox2.Width + 10;
            _richTextBox2.Height = _richTextBox1.Height;

            // init Titlebar
            _menuItem6.Checked = true;

            // show nr
            _panel2.Width = Display.Bounds.Width;
            _panel2.Height = Display.Bounds.Height;
            _panel2.Top = 0;
            _panel2.Left = 0;

            _label5.Left = Display.Bounds.Width / 2;
            _label5.Top = Display.Bounds.Height / 2 - _label5.Height / 2;
            _label9.Top = _label5.Bottom + 2;
            _label9.Left = _label5.Left;
            _pictureBox1.Left = _pictureBox1.Parent.Width / 2 - _pictureBox1.Width;
            _pictureBox1.Top = _label5.Top;
            _label5.Text = _song.Number.ToString();
            _label9.Text = _song.Desc;

            if (_trans != null)
            {
                RefreshSong(_song, _trans);
            }
            else
            {
                RefreshSong(_song);
            }

            _panel4.Bounds = Bounds;
            _panel4.Visible = Black;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            UpdatePresenterSizeAndPosition();
        }

        private bool DISABLEACTIONS;

        private delegate void CrossThreadInvoke();

        private void ShowNrAtStart()
        {
            CrossThreadInvoke showNr = delegate
            {
                _panel2.Show();
                _label5.Text = _song.Number.ToString(CultureInfo.InvariantCulture);
                _label9.Text = _song.Desc;
                DISABLEACTIONS = true;
            };
            Invoke(showNr);
            Thread.Sleep(Util.TIMER * 1000);
            CrossThreadInvoke hideNr = delegate
            {
                DISABLEACTIONS = false;
                _panel2.Hide();
                Util.CTRLSHOWNR = true;
            };
            Invoke(hideNr);
        }

        private void LyraButtonClickHandler(object sender, MouseEventArgs e)
        {
            _contextMenu1.Show(this, PointToClient(_lyraBtn.PointToScreen(new Point(0, 0))));
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            _song.uncheck();
            _countViews--;
            _owner.Status = "ok";
            Close();
        }

        // show song
        private void menuItem7_Click(object sender, EventArgs e)
        {
            RefreshSong(_song);
        }

        // back
        private void MoveNext_Handler(object sender, EventArgs e)
        {
            try
            {
                Util.CTRLSHOWNR = !((KeyEventArgs)e).Control;
            }
            catch (Exception)
            {
                Util.CTRLSHOWNR = Util.SHOWNR;
            }

            RefreshSong((ISong)navigate.Items[NextPos]);
        }

        // forward
        private void MovePrevious_Handler(object sender, EventArgs e)
        {
            try
            {
                Util.CTRLSHOWNR = !((KeyEventArgs)e).Control;
            }
            catch (Exception)
            {
                Util.CTRLSHOWNR = Util.SHOWNR;
            }

            RefreshSong((ISong)navigate.Items[LastPos]);
        }

        private int transCount;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            OnKeyDown(this, e);
        }

        private void OnKeyDown(object sender, KeyEventArgs ke)
        {
            if (DISABLEACTIONS)
                return;
            if ((ke.KeyCode == Keys.F4 && ke.Alt) || (ke.KeyCode == Keys.Escape))
            {
                menuItem2_Click(sender, ke);
            }
            else if (ke.KeyCode == Keys.B && ke.Control)
            {
                BlackScreen(!Black);
            }
            else if (ke.KeyCode == Keys.PageDown)
            {
                MovePrevious_Handler(sender, ke);
                updatePreview();
            }
            else if (ke.KeyCode == Keys.PageUp)
            {
                MoveNext_Handler(sender, ke);
                updatePreview();
            }
            else if (ke.KeyCode == Keys.Up)
            {
                ExecuteActionOnView(ViewActions.ScrollUp);
            }
            else if (ke.KeyCode == Keys.Down)
            {
                ExecuteActionOnView(ViewActions.ScrollDown);
            }
            else if (ke.KeyCode == Keys.Left || ke.KeyCode == Keys.Home)
            {
                ExecuteActionOnView(ViewActions.ScrollToTop);
            }
            else if (ke.KeyCode == Keys.Right || ke.KeyCode == Keys.End)
            {
                ExecuteActionOnView(ViewActions.ScrollToEnd);
            }
            else if (ke.KeyCode == Keys.F3)
            {
                Util.OpenFile(0);
            }
            else if (ke.KeyCode == Keys.F4)
            {
                Util.OpenFile(1);
            }
            else if (ke.KeyCode == Keys.F5)
            {
                Util.OpenFile(2);
            }
            else if (ke.KeyCode == Keys.F6)
            {
                Util.OpenFile(3);
            }
            else if (ke.KeyCode == Keys.F7)
            {
                Util.OpenFile(4);
            }
            else if (ke.KeyCode == Keys.F8)
            {
                Util.OpenFile(5);
            }
            else if (ke.KeyCode == Keys.F9)
            {
                activatePreview();
            }
            else if (ke.KeyCode == Keys.T)
            {
                RefreshSong(_song, _song.GetTranslation(transCount++));
            }
            else if (ke.KeyCode == Keys.U)
            {
                RefreshSong(_song);
            }
        }

        private void menuItem6_Click(object sender, EventArgs e)
        {
            _menuItem6.Checked = !_menuItem6.Checked;
            Util.SHOWRIGHT = _menuItem6.Checked;
            _richTextBox2.Text = "";
            _richTextBox2.Visible = false;
            if (_menuItem7.Visible)
            {
                menuItem7_Click(this, new EventArgs());
            }
            else
            {
                RefreshSong(_song);
            }
        }

        private bool isactivated;

        private void activatePreview()
        {
            isactivated = !isactivated;
            if (isactivated)
            {
                updatePreview();
            }

            _label8.Visible = isactivated;
        }

        private void updatePreview()
        {
            var next = (
                (ISong)navigate.Items[(pos + navigate.Items.Count + 1) % navigate.Items.Count]
            ).Number.ToString(CultureInfo.InvariantCulture);
            var last = (
                (ISong)navigate.Items[(pos + navigate.Items.Count - 1) % navigate.Items.Count]
            ).Number.ToString(CultureInfo.InvariantCulture);
            _label8.Text = "PgUp:" + next + Util.NL + "PgDn:" + last;
        }

        private static void ViewClosingHandler(object sender, CancelEventArgs e)
        {
            if (_this != null)
            {
                ScrollDataChanged -= _this.OnScrollDataChanged;
                _this._richTextBox1.ScrollDataChanged -= OnRichTextBox1OnScrollDataChanged;
            }
        }

        private static void ViewClosedHandler(object sender, EventArgs e)
        {
            OnSongDisplayed(null);
            _this = null; // delete link to closed View
        }

        private void LyraButtonMouseEnterHandler(object sender, EventArgs e)
        {
            _lyraBtn.Appearance.AlphaLevel = 255;
        }

        private void LyraButtonMouseLeaveHandler(object sender, EventArgs e)
        {
            _lyraBtn.Appearance.AlphaLevel = LyraBtnMinimumAlpha;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (!_richTextBox1.IsMouseOver)
            {
                return;
            }

            if (e.Delta > 0)
            {
                _richTextBox1.ScrollUp();
            }
            else
            {
                _richTextBox1.ScrollDown();
            }

            base.OnMouseWheel(e);
        }
    }

    public class ToManyViews : Exception
    {
        public ToManyViews()
            : base("Zu viele Songtexte geöffnet.") { }
    }

    public class JumpMark
    {
        private readonly string name;
        private readonly int position;

        public JumpMark(string xml, int position)
        {
            try
            {
                var pos = xml.IndexOf("name", StringComparison.InvariantCulture);
                if (pos >= 0)
                {
                    var left = xml.IndexOf("\"", pos, StringComparison.InvariantCulture) + 1;
                    var right = 0;
                    if (left >= 0)
                        right = xml.IndexOf("\"", left, StringComparison.InvariantCulture);
                    name = xml.Substring(left, right - left);
                }
            }
            catch (Exception)
            {
                name = "n/a";
            }

            this.position = position;
        }

        public string Name
        {
            get { return name; }
        }

        public int Position
        {
            get { return position; }
        }

        public override string ToString()
        {
            return name;
        }
    }
}
