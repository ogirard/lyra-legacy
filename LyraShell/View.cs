using System;
using System.Collections;
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
            PreviewsSong
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
            get { return this._song != null ? this._song.Style.ForegroundColor ?? Util.COLOR : Util.COLOR; }
        }

        private Color ResolvedRefrainForegroundColor
        {
            get { return this._song != null ? this._song.Style.ForegroundColor ?? Util.REFCOLOR : Util.REFCOLOR; }
        }

        private Color ResolveBackgroundColor
        {
            get { return this._song != null ? this._song.Style.BackgroundColor ?? Util.BGCOLOR : Util.BGCOLOR; }
        }

        private Font ResolvedFont
        {
            get { return this._song != null ? this._song.Style.GetFont() ?? Util.FONT : Util.FONT; }
        }

        private Image GetResolvedBackgroundImage()
        {
            if (this._song != null && this._song.Style.HasBackgroundImage)
            {
                return this._song.Style.GetBackgroundImage(new Size(this.Width, this.Height), Util.KEEPRATIO);
            }

            return null;
        }

        private Color ResolveTitleForegroundColor
        {
            get { return this._song != null ? this._song.Style.TitleForegroundColor ?? Util.TITLECOLOR : Util.TITLECOLOR; }
        }

        private Font ResolvedTitleFont
        {
            get { return this._song != null ? this._song.Style.GetTitleFont() ?? Util.TITLEFONT : Util.TITLEFONT; }
        }

        private Color ResolveTitleBackgroundColor
        {
            get { return this._song != null ? this._song.Style.TitleBackgroundColor ?? Util.TITLEBGCOLOR : Util.TITLEBGCOLOR; }
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

            if (_this == null) return;

            #endregion Precondition

            _this._richTextBox1.Select(charPos, 0);
            _this._richTextBox1.ScrollToCaret();
        }

        public static void ShowSong(ISong song, ITranslation trans, GUI owner, ListBox navigate, string source)
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

        private static void OnRichTextBox1OnScrollDataChanged(object sender, ScrollDataEventArgs args)
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
            this.InitializeComponent();
            this.InitializeContextMenu();
            this.InitializeSongPresenters();
            this._scrollVisual.Visible = false;

            this.Closed += ViewClosedHandler;
            this.Closing += ViewClosingHandler;
            ScrollDataChanged += this.OnScrollDataChanged;
        }

        private void OnScrollDataChanged(object sender, ScrollDataEventArgs scrollDataEventArgs)
        {
            this._scrollVisual.UpdateScrollData(scrollDataEventArgs);
        }

        private ListBox navigate;
        private int pos;

        private int NextPos
        {
            get
            {
                this.pos = (this.pos + this.navigate.Items.Count + 1) % this.navigate.Items.Count;
                return this.pos;
            }
        }

        private int LastPos
        {
            get
            {
                this.pos = (this.pos + this.navigate.Items.Count - 1) % this.navigate.Items.Count;
                return this.pos;
            }
        }


        public void RefreshSong(ISong song, ITranslation trans)
        {
            this._song = song;
            this._trans = trans;
            this.RefreshSong();
        }

        public void RefreshSong(ISong song)
        {
            if (this._song != null) this._song.uncheck();
            this.RefreshSong(song, null);
        }

        private bool haspgbr;

        public void RefreshSong()
        {
            addSongToHistory(this._song);
            this.SetCurrentSongInfo();
            this._richTextBox1.Font = _this.ResolvedFont;
            this._richTextBox2.Font = _this.ResolvedFont;
            this.BackColor = _this.ResolveBackgroundColor;
            if (this._trans == null) this.transCount = 0;

            // update title and number
            this._title.Number = this._song.Number.ToString();
            this._title.Title = this._song.Title;
            this._title.Mode = this._song != null && this._song.Style != null ? this._song.Style.TitleMode : TitleMode.NumberAndTitle;
            this._title.TitleForegroundColor = this.ResolveTitleForegroundColor;
            this._title.TitleBackgroundColor = this.ResolveTitleBackgroundColor;
            this._lyraBtn.Appearance.BackColor = this._title.Mode == TitleMode.None ? Color.Transparent : this.ResolveTitleBackgroundColor;
            this._title.TitleFont = this.ResolvedTitleFont;

            this.UpdatePresenterSizeAndPosition();
            this._richTextBox2.Text = "";
            this._richTextBox2.Visible = false;

            RichTextBox myrtb = this._richTextBox1;
            if (this._trans != null)
            {
                this._menuItem7.Visible = true;
                if (Util.SHOWRIGHT && !this.haspgbr)
                {
                    myrtb = this._richTextBox2;
                    this._richTextBox2.Top = this._richTextBox1.Top;
                    this._richTextBox2.Height = this._richTextBox1.Height;
                    this._richTextBox2.Width = (this.Width - 24) / 2 - 6;
                    this._richTextBox1.Width = this._richTextBox2.Width;
                    this._richTextBox2.Left = this._richTextBox1.Right + 12;
                    this._richTextBox2.Visible = true;
                }

                myrtb.Text = this._trans.Text;
                if (this._trans.Unformatted)
                {
                    myrtb.Text += "\n\n<special>(wird nicht gesungen)</special>";
                }
            }
            else
            {
                this._menuItem7.Visible = false;
                this._richTextBox1.Text = this._song.Text;
                var findpos = 0;
                this.haspgbr = false;
                if ((findpos = this._richTextBox1.Find("<" + Util.PGBR + " />", 0, RichTextBoxFinds.MatchCase)) != -1)
                {
                    var text = this._richTextBox1.Text.Substring(0, findpos);
                    this._richTextBox2.Text = this._richTextBox1.Text.Substring(findpos + Util.PGBR.Length + 5);
                    if (Util.SHOWRIGHT)
                    {
                        this._richTextBox1.Text = text;
                        this._richTextBox2.Visible = true;
                        this.formatall(myrtb);
                        myrtb = this._richTextBox2;
                        this.haspgbr = true;
                        this._richTextBox2.Top = this._richTextBox1.Top;
                        this._richTextBox2.Height = this._richTextBox1.Height;
                        this._richTextBox2.Width = (this.Width - 24) / 2 - 6;
                        this._richTextBox1.Width = this._richTextBox2.Width;
                        this._richTextBox2.Left = this._richTextBox1.Right + 12;
                    }
                    else
                    {
                        this._richTextBox1.Text = text + "\n" + this._richTextBox2.Text;
                    }
                }
            }

            if (this._contextMenu1.MenuItems.Count == 5) this._contextMenu1.MenuItems.RemoveAt(3);


            if (this._song.GetTransMenu(this) != null)
            {
                this._contextMenu1.MenuItems.Add(3, this._song.GetTransMenu(this));
            }

            if (this._trans != null && this._trans.Unformatted)
            {
                this.formatUnform(myrtb);
            }
            else
            {
                this.formatall(myrtb);
            }

            if (Util.SHOWNR && Util.CTRLSHOWNR)
            {
                (new Thread(this.ShowNrAtStart)).Start();
            }
            else
            {
                this._panel2.Hide();
            }

            this.BackgroundImage = this.GetResolvedBackgroundImage();

            this._richTextBox1.ScrollToTop();
            this._richTextBox2.ScrollToTop();
            OnSongDisplayed(currentSongInfo);
            this.Focus();
        }

        private void UpdatePresenterSizeAndPosition()
        {
            if (this._richTextBox1 == null)
            {
                return;
            }

            this._richTextBox1.Left = 12;
            this._richTextBox1.Width = this.Width - 24;
            this._richTextBox1.Top = this._title.Visible ? this._title.Bottom + 12 : 12;
            this._richTextBox1.Height = this.Height - 12 - this._richTextBox1.Top;
            this._scrollVisual.Top = this._richTextBox1.Top;
            this._scrollVisual.Height = this._richTextBox1.Height;
        }

        private void SetCurrentSongInfo()
        {
            var currentSongPosition = this.navigate.Items.IndexOf(this._song);
            currentSongInfo = new SongDisplayedEventArgs(this._song,
                                                         (ISong) this.navigate.Items[
                                                           (currentSongPosition + 1 + this.navigate.Items.Count) % this.navigate.Items.Count],
                                                         (ISong) this.navigate.Items[
                                                           (currentSongPosition - 1 + this.navigate.Items.Count) % this.navigate.Items.Count], 
                                                         this._source);
            
        }

        private void formatall(RichTextBox rtb)
        {
            rtb.SelectAll();
            rtb.SelectionFont = this.ResolvedFont;
            rtb.SelectionColor = this.ResolvedForegroundColor;
            // Format Refrain
            if (Util.refmode)
            {
                this.format(rtb, Util.REF, this.ResolvedFont, this.ResolvedRefrainForegroundColor, 16, "Refrain:" + Util.RTNL, "");
            }
            else
            {
                this.format(rtb, Util.REF, new Font(this.ResolvedFont, FontStyle.Bold), this.ResolvedRefrainForegroundColor, 0, "", "");
            }

            this.format(rtb, Util.SPEC, Util.SPECFONT, this.ResolvedForegroundColor, 0, "", "");
            this.formatBlock(rtb);
            this.format(rtb, Util.BOLD, null, this.ResolvedForegroundColor, 0, "", "");
            this.format(rtb, Util.ITALIC, null, this.ResolvedForegroundColor, 0, "", "");
            this.GetJumpMarks(rtb);
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
            this.format(rtb, Util.REF, Util.TRANSFONT, this.ResolvedRefrainForegroundColor, 0, "Refrain :" + Util.RTNL, "");
            this.format(rtb, Util.SPEC, new Font(Util.SPECFONT.FontFamily, Util.TRANSFONT.Size, Util.SPECFONT.Style), this.ResolvedForegroundColor, 0, "", "");
            this.formatBlock(rtb);
            this.format(rtb, Util.BOLD, Util.TRANSFONT, this.ResolvedForegroundColor, 0, "", "");
            this.format(rtb, Util.ITALIC, Util.TRANSFONT, this.ResolvedForegroundColor, 0, "", "");
            rtb.SelectAll();
            rtb.SelectionFont = Util.TRANSFONT;
        }

        private void format(RichTextBox rtb, string tag, Font font, Color c, int offset, string l, string r)
        {
            var start = 0;
            int len;
            while ((start = rtb.Find("<" + tag + ">", start, RichTextBoxFinds.MatchCase)) >= 0)
            {
                start += tag.Length + 2;
                len = rtb.Find("</" + tag + ">", start, RichTextBoxFinds.MatchCase) - start;
                rtb.Select(start, len);
                var stylefont = font == null ? rtb.SelectionFont : font;
                if (tag == Util.BOLD)
                {
                    rtb.SelectionFont = new Font(stylefont, FontStyle.Bold);
                }
                else if (tag == Util.ITALIC)
                {
                    rtb.SelectionFont = new Font(stylefont, FontStyle.Italic);
                }
                else
                {
                    rtb.SelectionFont = stylefont;
                }
                rtb.SelectionColor = c;
                rtb.SelectionIndent += offset;
            }
            rtb.Rtf = rtb.Rtf.Replace("<" + tag + ">", l);
            rtb.Rtf = rtb.Rtf.Replace("</" + tag + ">", r);

            if (tag == Util.REF)
            {
                while ((start = rtb.Find("Refrain:", ++start, RichTextBoxFinds.MatchCase)) >= 0)
                {
                    if (start == 1) //remove first \par\r
                    {
                        rtb.Rtf = rtb.Rtf.Remove(rtb.Rtf.IndexOf("Refrain:") - 6, 6);
                        start = 0;
                    }
                    rtb.Select(start, 8);
                    rtb.SelectionFont = new Font(rtb.SelectionFont, FontStyle.Bold);
                    rtb.SelectionColor = this.ResolvedRefrainForegroundColor;
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
                this.format(rtb, tag, this.ResolvedFont, this.ResolvedForegroundColor, offset, "", "");
            }
        }

        /// <summary>
        ///   Die verwendeten Ressourcen bereinigen.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._components != null)
                {
                    this._components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeSongPresenters()
        {
            this._richTextBox1 = new ExtendedRichTextBox();
            this._richTextBox2 = new ExtendedRichTextBox();
            // 
            // richTextBox1
            // 
            this._richTextBox1.BackColor = Color.White;
            this._richTextBox1.BorderStyle = BorderStyle.None;
            this._richTextBox1.Cursor = Cursors.Arrow;
            this._richTextBox1.Font =
              new Font("Microsoft Sans Serif", 12F, FontStyle.Regular,
                       GraphicsUnit.Point, 0);
            this._richTextBox1.Name = "richTextBox1";
            this._richTextBox1.ReadOnly = true;
            this._richTextBox1.TabIndex = 1;
            this._richTextBox1.TabStop = false;
            this._richTextBox1.Text = "Text";
            this._richTextBox1.KeyDown += this.OnKeyDown;
            this._richTextBox1.GotFocus += this.HandlePresenterFocus;
            this._richTextBox1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top;
            this._richTextBox1.MouseEnter += this.PresenterMouseEnterHandler;
            this._richTextBox1.MouseLeave += this.PresenterMouseLeaveHandler;

            // 
            // richTextBox2
            // 
            this._richTextBox2.BackColor = Color.White;
            this._richTextBox2.BorderStyle = BorderStyle.None;
            this._richTextBox2.Cursor = Cursors.Arrow;
            this._richTextBox2.Font =
              new Font("Microsoft Sans Serif", 12F, FontStyle.Regular,
                       GraphicsUnit.Point, 0);
            this._richTextBox2.Name = "richTextBox2";
            this._richTextBox2.ReadOnly = true;
            this._richTextBox2.TabIndex = 7;
            this._richTextBox2.TabStop = false;
            this._richTextBox2.Text = "TextRight";
            this._richTextBox2.Visible = false;
            this._richTextBox2.KeyDown += this.OnKeyDown;
            this._richTextBox2.GotFocus += this.HandlePresenterFocus;
            this._richTextBox2.MouseEnter += this.PresenterMouseEnterHandler;
            this._richTextBox2.MouseLeave += this.PresenterMouseLeaveHandler;

            this.Controls.Add(this._richTextBox1);
            this.Controls.Add(this._richTextBox2);
        }

        private void PresenterMouseLeaveHandler(object sender, EventArgs e)
        {
            this._scrollVisual.Visible = false;
        }

        private void PresenterMouseEnterHandler(object sender, EventArgs e)
        {
            this._scrollVisual.Visible = true;
        }

        private void HandlePresenterFocus(object sender, EventArgs e)
        {
            this._title.Focus();
        }

        private void InitializeContextMenu()
        {
            this._contextMenu1 = new ContextMenu();
            this._menuItem1 = new MenuItem();
            this._menuItem3 = new MenuItem();
            this._menuItem4 = new MenuItem();
            this._menuItem6 = new MenuItem();
            this._menuItem7 = new MenuItem();
            this._menuItem5 = new MenuItem();
            this._menuItem2 = new MenuItem();

            // menuItem1
            // 
            this._menuItem1.Index = 0;
            this._menuItem1.MenuItems.AddRange(new[]
                                      {
                                          this._menuItem3, this._menuItem4
                                      });
            this._menuItem1.Text = "&Navigation";
            // 
            // menuItem3
            // 
            this._menuItem3.Index = 0;
            this._menuItem3.Text = "&>>";
            this._menuItem3.Click += this.MoveNext_Handler;
            // 
            // menuItem4
            // 
            this._menuItem4.Index = 1;
            this._menuItem4.Text = "&<<";
            this._menuItem4.Click += this.MovePrevious_Handler;
            // 
            // menuItem6
            // 
            this._menuItem6.Index = 1;
            this._menuItem6.Text = "&rechtes Fenster benützen";
            this._menuItem6.Click += this.menuItem6_Click;
            // 
            // menuItem7
            // 
            this._menuItem7.Index = 2;
            this._menuItem7.Text = "&Originaltext anzeigen";
            this._menuItem7.Visible = false;
            this._menuItem7.Click += this.menuItem7_Click;
            // 
            // menuItem5
            // 
            this._menuItem5.Index = 3;
            this._menuItem5.Text = "Über&setzungen";
            // 
            // menuItem2
            // 
            this._menuItem2.Index = 4;
            this._menuItem2.Text = "schlie&ssen";
            this._menuItem2.Click += this.menuItem2_Click;

            // 
            // contextMenu1
            // 
            this._contextMenu1.MenuItems.AddRange(new[]
                                         {
                                             this._menuItem1, this._menuItem6, this._menuItem7, this._menuItem5, this._menuItem2
                                         });

            this._lyraBtn.ContextMenu = this._contextMenu1;
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
            this._label5.Font = new System.Drawing.Font("Verdana", 65.25F, System.Drawing.FontStyle.Bold,
                                                       System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this._label9.Font = new System.Drawing.Font("Verdana", 48F, System.Drawing.FontStyle.Bold,
                                                       System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this._pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this._pictureBox1.Location = new System.Drawing.Point(24, 16);
            this._pictureBox1.Name = "_pictureBox1";
            this._pictureBox1.Size = new System.Drawing.Size(216, 136);
            this._pictureBox1.TabIndex = 8;
            this._pictureBox1.TabStop = false;
            // 
            // label8
            // 
            this._label8.BackColor = System.Drawing.Color.Transparent;
            this._label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular,
                                                       System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this._lyraBtn.Anchor =
              ((System.Windows.Forms.AnchorStyles)
               ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.AlphaLevel = ((short)(64));
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            appearance1.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance1.ImageAlpha = Infragistics.Win.Alpha.Transparent;
            appearance1.ImageBackground = ((System.Drawing.Image)(resources.GetObject("appearance1.ImageBackground")));
            appearance1.ImageBackgroundAlpha = Infragistics.Win.Alpha.UseAlphaLevel;
            this._lyraBtn.Appearance = appearance1;
            this._lyraBtn.Location = new System.Drawing.Point(1054, 9);
            this._lyraBtn.Name = "_lyraBtn";
            this._lyraBtn.Size = new System.Drawing.Size(32, 32);
            this._lyraBtn.TabIndex = 13;
            this._lyraBtn.UseAppStyling = false;
            this._lyraBtn.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this._lyraBtn.MouseClickClient += this.LyraButtonClickHandler;
            this._lyraBtn.MouseEnterClient += new System.EventHandler(this.LyraButtonMouseEnterHandler);
            this._lyraBtn.MouseLeaveClient += new System.EventHandler(this.LyraButtonMouseLeaveHandler);
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
            this._title.Anchor =
              ((System.Windows.Forms.AnchorStyles)
               (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                 | System.Windows.Forms.AnchorStyles.Right)));
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
            this._scrollVisual.Anchor =
              ((System.Windows.Forms.AnchorStyles)
               (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                 | System.Windows.Forms.AnchorStyles.Right)));
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
            this.Width = Display.Bounds.Width;
            this.Height = Display.Bounds.Height;
            this.Top = Display.Bounds.Top;
            this.Left = Display.Bounds.Left;

            this._label8.Top = this.Height - this._label8.Height;
            this._label8.Left = this.Width - this._label8.Width;

            // init TextBoxes
            // this.richTextBox1.Left = 24;
            // this.richTextBox1.Width = this.Width - 80;
            // this.richTextBox1.Top = this.panel1.Top + this.panel1.Height + 20;
            // this.richTextBox1.Height = this.Height - this.richTextBox1.Top - 10;
            this._richTextBox2.Top = this._richTextBox1.Top;
            this._richTextBox2.Height = this._richTextBox1.Height;
            this._richTextBox2.Width = this._richTextBox1.Width / 2 - 5;
            this._richTextBox2.Left = this._richTextBox1.Left + this._richTextBox2.Width + 10;
            this._richTextBox2.Height = this._richTextBox1.Height;

            // init Titlebar
            this._menuItem6.Checked = true;

            // show nr
            this._panel2.Width = Display.Bounds.Width;
            this._panel2.Height = Display.Bounds.Height;
            this._panel2.Top = 0;
            this._panel2.Left = 0;

            this._label5.Left = Display.Bounds.Width / 2;
            this._label5.Top = Display.Bounds.Height / 2 - this._label5.Height / 2;
            this._label9.Top = this._label5.Bottom + 2;
            this._label9.Left = this._label5.Left;
            this._pictureBox1.Left = this.Width / 2 - this._pictureBox1.Width;
            this._pictureBox1.Top = this._label5.Top;
            this._label5.Text = this._song.Number.ToString();
            this._label9.Text = this._song.Desc;

            if (this._trans != null)
            {
                this.RefreshSong(this._song, this._trans);
            }
            else
            {
                this.RefreshSong(this._song);
            }

            this._panel4.Bounds = this.Bounds;
            this._panel4.Visible = Black;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            this.UpdatePresenterSizeAndPosition();
        }

        private bool DISABLEACTIONS;


        private delegate void CrossThreadInvoke();

        private void ShowNrAtStart()
        {
            CrossThreadInvoke showNr = delegate
                                         {
                                             this._panel2.Show();
                                             this._label5.Text = this._song.Number.ToString(CultureInfo.InvariantCulture);
                                             this._label9.Text = this._song.Desc;
                                             this.DISABLEACTIONS = true;
                                         };
            this.Invoke(showNr);
            Thread.Sleep(Util.TIMER * 1000);
            CrossThreadInvoke hideNr = delegate
                                         {
                                             this.DISABLEACTIONS = false;
                                             this._panel2.Hide();
                                             Util.CTRLSHOWNR = true;
                                         };
            this.Invoke(hideNr);
        }

        private void LyraButtonClickHandler(object sender, MouseEventArgs e)
        {
            this._contextMenu1.Show(this, this.PointToClient(this._lyraBtn.PointToScreen(new Point(0, 0))));
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            this._song.uncheck();
            _countViews--;
            this._owner.Status = "ok";
            this.Close();
        }

        // show song
        private void menuItem7_Click(object sender, EventArgs e)
        {
            this.RefreshSong(this._song);
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

            this.RefreshSong((ISong) this.navigate.Items[this.NextPos]);
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

            this.RefreshSong((ISong) this.navigate.Items[this.LastPos]);
        }

        private int transCount;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            this.OnKeyDown(this, e);
        }

        private void OnKeyDown(object sender, KeyEventArgs ke)
        {
            if (this.DISABLEACTIONS) return;
            if ((ke.KeyCode == Keys.F4 && ke.Alt) || (ke.KeyCode == Keys.Escape))
            {
                this.menuItem2_Click(sender, ke);
            }
            else if (ke.KeyCode == Keys.B && ke.Control)
            {
                BlackScreen(!Black);
            }
            else if (ke.KeyCode == Keys.PageDown)
            {
                this.MovePrevious_Handler(sender, ke);
                this.updatePreview();
            }
            else if (ke.KeyCode == Keys.PageUp)
            {
                this.MoveNext_Handler(sender, ke);
                this.updatePreview();
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
                this.activatePreview();
            }
            else if (ke.KeyCode == Keys.T)
            {
                this.RefreshSong(this._song, this._song.GetTranslation(this.transCount++));
            }
            else if (ke.KeyCode == Keys.U)
            {
                this.RefreshSong(this._song);
            }
        }

        private void menuItem6_Click(object sender, EventArgs e)
        {
            this._menuItem6.Checked = !this._menuItem6.Checked;
            Util.SHOWRIGHT = this._menuItem6.Checked;
            this._richTextBox2.Text = "";
            this._richTextBox2.Visible = false;
            if (this._menuItem7.Visible)
            {
                this.menuItem7_Click(this, new EventArgs());
            }
            else
            {
                this.RefreshSong(this._song);
            }
        }

        private bool isactivated;

        private void activatePreview()
        {
            this.isactivated = !this.isactivated;
            if (this.isactivated)
            {
                this.updatePreview();
            }

            this._label8.Visible = this.isactivated;
        }

        private void updatePreview()
        {
            var next =
              ((ISong) this.navigate.Items[(this.pos + this.navigate.Items.Count + 1) % this.navigate.Items.Count]).
                Number.ToString(CultureInfo.InvariantCulture);
            var last =
              ((ISong) this.navigate.Items[(this.pos + this.navigate.Items.Count - 1) % this.navigate.Items.Count]).
                Number.ToString(CultureInfo.InvariantCulture);
            this._label8.Text = "PgUp:" + next + Util.NL + "PgDn:" + last;
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
            this._lyraBtn.Appearance.AlphaLevel = 255;
        }

        private void LyraButtonMouseLeaveHandler(object sender, EventArgs e)
        {
            this._lyraBtn.Appearance.AlphaLevel = LyraBtnMinimumAlpha;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (!this._richTextBox1.IsMouseOver)
            {
                return;
            }

            if (e.Delta > 0)
            {
                this._richTextBox1.ScrollUp();
            }
            else
            {
                this._richTextBox1.ScrollDown();
            }

            base.OnMouseWheel(e);
        }
    }

    public class ToManyViews : Exception
    {
        public ToManyViews()
            : base("Zu viele Songtexte geöffnet.")
        {
        }
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
                    if (left >= 0) right = xml.IndexOf("\"", left, StringComparison.InvariantCulture);
                    this.name = xml.Substring(left, right - left);
                }
            }
            catch (Exception)
            {
                this.name = "n/a";
            }

            this.position = position;
        }

        public string Name
        {
            get { return this.name; }
        }

        public int Position
        {
            get { return this.position; }
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}