using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
  /// <summary>
  /// Zusammendfassende Beschreibung für View.
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
    /// Erforderliche Designervariable.
    /// </summary>
    private readonly Container components;

    private ContextMenu contextMenu1;
    private MenuItem menuItem1;
    private MenuItem menuItem2;
    private MenuItem menuItem3;
    private MenuItem menuItem4;
    private ExtendedRichTextBox richTextBox1;
    private ExtendedRichTextBox richTextBox2;
    private Label label1;
    private Label label2;
    private Panel panel1;
    private MenuItem menuItem5;
    private Label label3;
    private MenuItem menuItem7;
    private MenuItem menuItem6;
    private Label label4;
    private PictureBox pictureBox1;
    private Label label5;
    private Panel panel2;
    private Label label7;
    private Label label8;
    private Panel lyraBtn;
    private Panel panel3;
    private Label label9;
    private Panel panel4;

    public static Screen display = Screen.PrimaryScreen;
    private GUI owner;
    public static bool black;
    private static int countViews;
    private ISong song;
    private ITranslation trans;

    private static readonly ArrayList songHistory = new ArrayList();

    #region    Styling

    private Color ResolvedForegroundColor
    {
      get { return this.song != null ? this.song.Style.ForegroundColor ?? Util.COLOR : Util.COLOR; }
    }

    private Color ResolvedRefrainForegroundColor
    {
      get { return this.song != null ? this.song.Style.ForegroundColor ?? Util.REFCOLOR : Util.REFCOLOR; }
    }

    private Color ResolveBackgroundColor
    {
      get { return this.song != null ? this.song.Style.BackgroundColor ?? Util.BGCOLOR : Util.BGCOLOR; }
    }

    private Font ResolvedFont
    {
      get { return this.song != null ? this.song.Style.GetFont() ?? Util.FONT : Util.FONT; }
    }

    private Image GetResolvedBackgroundImage()
    {
      if (this.song != null && this.song.Style.HasBackgroundImage)
      {
        return this.song.Style.GetBackgroundImage(new Size(this.Width, this.Height), Util.KEEPRATIO);
      }

      return null;
    }

    private Color ResolveTitleForegroundColor
    {
      get { return this.song != null ? this.song.Style.TitleForegroundColor ?? Util.TITLECOLOR : Util.TITLECOLOR; }
    }

    private Font ResolvedTitleFont
    {
      get { return this.song != null ? this.song.Style.GetTitleFont() ?? Util.TITLEFONT : Util.TITLEFONT; }
    }

    private Color ResolveTitleBackgroundColor
    {
      get { return this.song != null ? this.song.Style.TitleBackgroundColor ?? Util.TITLEBGCOLOR : Util.TITLEBGCOLOR; }
    }

    #endregion Styling

    private static void addSongToHistory(ISong song)
    {
      if (songHistory.Contains(song))
      {
        songHistory.Remove(song);
      }
      songHistory.Add(song);
      if (HistoryChanged != null)
      {
        HistoryChanged(null, EventArgs.Empty);
      }
    }

    public static ArrayList SongHistory
    {
      get { return songHistory; }
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

    /// <summary>
    /// Executes action on left left rich-textbox
    /// </summary>
    /// <param name="action"><see cref="ViewActions"/></param>
    /// <returns><code>true</code> if view active, <code>false</code> otherwise
    ///	</returns>
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
            _this.richTextBox1.ScrollUp();
            break;
          case ViewActions.ScrollDown:
            _this.richTextBox1.ScrollDown();
            break;
          case ViewActions.ScrollPageUp:
            _this.richTextBox1.ScrollPageUp();
            break;
          case ViewActions.ScrollPageDown:
            _this.richTextBox1.ScrollPageDown();
            break;
          case ViewActions.ScrollToTop:
            _this.richTextBox1.ScrollToTop();
            break;
          case ViewActions.ScrollToEnd:
            _this.richTextBox1.ScrollToBottom();
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

      _this.richTextBox1.Select(charPos, 0);
      _this.richTextBox1.ScrollToCaret();
    }

    public static void ShowSong(ISong song, ITranslation trans, GUI owner, ListBox navigate)
    {
      if (_this == null)
      {
        _this = new View();
      }
      _this.song = song;
      _this.menuItem1.Visible = false;
      _this.owner = owner;
      _this.navigate = navigate;
      _this.menuItem6.Checked = Util.SHOWRIGHT;

      if (trans != null)
      {
        _this.refresh(song, trans);
      }
      else
      {
        _this.refresh(song);
      }
      _this.richTextBox1.Focus();
      _this.pos = navigate.Items.IndexOf(_this.song);
      _this.Show();
    }

    public static void BlackScreen(bool on)
    {
      black = on;
      if (_this != null)
      {
        _this.panel4.Bounds = _this.Bounds; // assert the correct bounds
        _this.panel4.Visible = on;
      }
    }

    public static void ShowSong(ISong song, GUI owner, ListBox navigate)
    {
      ShowSong(song, null, owner, navigate);
      _this.menuItem1.Visible = true;
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
      this.initTransparentBoxes();
      this.Closed += this.View_Closed;
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


    public void refresh(ISong song, ITranslation trans)
    {
      this.song = song;
      this.trans = trans;
      this.refresh();
    }

    public void refresh(ISong song)
    {
      if (this.song != null) this.song.uncheck();
      this.refresh(song, null);
    }


    private bool haspgbr;

    public void refresh()
    {
      addSongToHistory(this.song);
      this.SetCurrentSongInfo();
      this.richTextBox1.Font = _this.ResolvedFont;
      this.richTextBox2.Font = _this.ResolvedFont;
      this.BackColor = _this.ResolveBackgroundColor;
      if (this.trans == null) this.transCount = 0;

      this.label1.Text = this.song.Number.ToString();
      this.label7.Text = this.song.Desc;
      this.label3.Text = "";
      this.label4.Text = "";
      this.label4.Visible = false;
      this.label2.Width = this.richTextBox1.Width - 2 * this.label2.Left;
      this.richTextBox1.Width = this.Width - 24;
      this.richTextBox2.Text = "";
      this.richTextBox2.Visible = false;

      RichTextBox myrtb = this.richTextBox1;
      if (this.trans != null)
      {
        this.menuItem7.Visible = true;
        if (Util.SHOWRIGHT && !this.haspgbr)
        {
          myrtb = this.richTextBox2;
          this.richTextBox1.Width = this.richTextBox2.Width;
          this.richTextBox2.Visible = true;
          this.label4.Visible = true;
          this.label4.Text = this.trans.ToString();
          this.label4.Width = this.richTextBox2.Width;
          this.label2.Width = this.richTextBox1.Width - 2 * this.label2.Left;
        }
        else
        {
          if (Util.SHOWGER)
          {
            this.label3.Text = "\t(deutscher Titel: \"" + this.song.Title + "\")";
            this.label3.Top = 13;
            this.label3.Left = this.panel1.Width - this.label3.Width - 25;
          }
          this.label2.Text = this.trans.ToString();
        }
        myrtb.Text = this.trans.Text;
        if (this.trans.Unformatted)
        {
          myrtb.Text += "\n\n<special>(wird nicht gesungen)</special>";
        }
      }
      else
      {
        this.menuItem7.Visible = false;
        this.label2.Text = this.song.Title;
        this.richTextBox1.Text = this.song.Text;
        int findpos = 0;
        this.haspgbr = false;
        if ((findpos = this.richTextBox1.Find("<" + Util.PGBR + " />", 0, RichTextBoxFinds.MatchCase)) != -1)
        {
          string text = this.richTextBox1.Text.Substring(0, findpos);
          this.richTextBox2.Text = this.richTextBox1.Text.Substring(findpos + Util.PGBR.Length + 5);
          if (Util.SHOWRIGHT)
          {
            this.richTextBox1.Text = text;
            this.richTextBox2.Visible = true;
            this.formatall(myrtb);
            myrtb = this.richTextBox2;
            this.haspgbr = true;
            this.richTextBox1.Width = this.richTextBox2.Width;
          }
          else
          {
            this.richTextBox1.Text = text + "\n" + this.richTextBox2.Text;
          }
        }
      }

      this.label1.Width = this.label1.Text.Length * 20 + 10;
      this.label7.Width = this.label5.Text.Length == 0 ? 0 : this.label7.Text.Length * 20 + 6;
      this.label7.Left = 0;
      this.label1.Left = this.label7.Width - 1;
      this.label2.Left = this.label1.Left + this.label1.Width + 10;
      this.label4.Left = this.richTextBox2.Left - this.panel1.Left - 2;

      if (this.contextMenu1.MenuItems.Count == 5)
        this.contextMenu1.MenuItems.RemoveAt(3);


      if (this.song.GetTransMenu(this) != null)
      {
        this.contextMenu1.MenuItems.Add(3, this.song.GetTransMenu(this));
      }

      if (this.trans != null && this.trans.Unformatted)
      {
        this.formatUnform(myrtb);
      }
      else
      {
        this.formatall(myrtb);
      }

      if (Util.SHOWNR && Util.CTRLSHOWNR)
      {
        (new Thread(this.showNrAtStart)).Start();
      }
      else
      {
        this.panel2.Hide();
      }

      this.BackgroundImage = this.GetResolvedBackgroundImage();

      this.richTextBox1.ScrollToTop();
      this.richTextBox2.ScrollToTop();
      OnSongDisplayed(currentSongInfo);
      this.Focus();
    }

    private void SetCurrentSongInfo()
    {
      currentSongInfo = new SongDisplayedEventArgs(this.song,
                                                   (ISong)
                                                   this.navigate.Items[
                                                       (this.pos + this.navigate.Items.Count + 1) %
                                                       this.navigate.Items.Count],
                                                   (ISong)
                                                   this.navigate.Items[
                                                       (this.pos + this.navigate.Items.Count - 1) %
                                                       this.navigate.Items.Count]);
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
      int start = 0;
      int startText = 0;
      currentSongInfo.Jumpmarks.Clear();
      while ((start = rtb.Find("<" + Util.JMP, start, RichTextBoxFinds.MatchCase)) >= 0)
      {
        int end = 0;
        int endText = 0;
        if ((end = rtb.Find("/>", start, RichTextBoxFinds.MatchCase)) >= 0)
        {
          startText = rtb.Text.IndexOf("<" + Util.JMP, startText);
          endText = rtb.Text.IndexOf("/>", startText) + 2;
          string jumpmark = rtb.Text.Substring(startText, endText - startText);
          currentSongInfo.Jumpmarks.Add(new JumpMark(jumpmark, start));
          rtb.Rtf = rtb.Rtf.Replace(jumpmark, "");
          start = 0;
        }
      }
    }

    private void formatUnform(RichTextBox rtb)
    {
      this.format(rtb, Util.REF, Util.TRANSFONT, this.ResolvedRefrainForegroundColor, 0, "Refrain :" + Util.RTNL, "");
      this.format(rtb, Util.SPEC, new Font(Util.SPECFONT.FontFamily, Util.TRANSFONT.Size, Util.SPECFONT.Style),
                  this.ResolvedForegroundColor, 0, "", "");
      this.formatBlock(rtb);
      this.format(rtb, Util.BOLD, Util.TRANSFONT, this.ResolvedForegroundColor, 0, "", "");
      this.format(rtb, Util.ITALIC, Util.TRANSFONT, this.ResolvedForegroundColor, 0, "", "");
      rtb.SelectAll();
      rtb.SelectionFont = Util.TRANSFONT;
    }

    private void format(RichTextBox rtb, string tag, Font font, Color c, int offset, string l, string r)
    {
      int start = 0;
      int len;
      while ((start = rtb.Find("<" + tag + ">", start, RichTextBoxFinds.MatchCase)) >= 0)
      {
        start += tag.Length + 2;
        len = rtb.Find("</" + tag + ">", start, RichTextBoxFinds.MatchCase) - start;
        rtb.Select(start, len);
        Font stylefont = font == null ? rtb.SelectionFont : font;
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
      int start = 0;
      while ((start = rtb.Find("<" + Util.BLOCK, start, RichTextBoxFinds.MatchCase)) >= 0)
      {
        string tag = rtb.Text.Substring(++start, 3);
        if (tag[2] == '>')
        {
          tag = tag.Substring(0, 2);
        }
        int offset = Int32.Parse(tag.Substring(1, tag.Length - 1));
        this.format(rtb, tag, this.ResolvedFont, this.ResolvedForegroundColor, offset, "", "");
      }
    }

    /// <summary>
    /// Die verwendeten Ressourcen bereinigen.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (this.components != null)
        {
          this.components.Dispose();
        }
      }
      base.Dispose(disposing);
    }

    private void initTransparentBoxes()
    {
      this.richTextBox1 = new ExtendedRichTextBox();
      this.richTextBox2 = new ExtendedRichTextBox();
      // 
      // richTextBox1
      // 
      this.richTextBox1.BackColor = Color.White;
      this.richTextBox1.BorderStyle = BorderStyle.None;
      this.richTextBox1.Cursor = Cursors.Arrow;
      this.richTextBox1.Font =
          new Font("Microsoft Sans Serif", 12F, FontStyle.Regular,
                   GraphicsUnit.Point, 0);
      // this.richTextBox1.Location = new Point(30, 72);
      this.richTextBox1.Name = "richTextBox1";
      this.richTextBox1.ReadOnly = true;
      // this.richTextBox1.Size = new Size(360, 176);
      this.richTextBox1.TabIndex = 1;
      this.richTextBox1.TabStop = false;
      this.richTextBox1.Text = "Text";
      this.richTextBox1.KeyDown += this.View_KeyDown;
      this.richTextBox1.GotFocus += this.richTextBox1_GotFocus;
      this.richTextBox1.Left = 12;
      this.richTextBox1.Top = this.panel1.Bottom + 8;
      this.richTextBox1.Width = this.Width - 24;
      this.richTextBox1.Height = this.Height - this.panel1.Bottom - 16;
      this.richTextBox1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top;

      // 
      // richTextBox2
      // 
      this.richTextBox2.BackColor = Color.White;
      this.richTextBox2.BorderStyle = BorderStyle.None;
      this.richTextBox2.Cursor = Cursors.Arrow;
      this.richTextBox2.Font =
          new Font("Microsoft Sans Serif", 12F, FontStyle.Regular,
                   GraphicsUnit.Point, 0);
      this.richTextBox2.Location = new Point(152, 72);
      this.richTextBox2.Name = "richTextBox2";
      this.richTextBox2.ReadOnly = true;
      this.richTextBox2.Size = new Size(360, 176);
      this.richTextBox2.TabIndex = 7;
      this.richTextBox2.TabStop = false;
      this.richTextBox2.Text = "TextRight";
      this.richTextBox2.Visible = false;
      this.richTextBox2.KeyDown += this.View_KeyDown;
      this.richTextBox2.GotFocus += this.richTextBox1_GotFocus;


      this.Controls.Add(this.richTextBox1);
      this.Controls.Add(this.richTextBox2);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung. 
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(View));
      this.contextMenu1 = new System.Windows.Forms.ContextMenu();
      this.menuItem1 = new System.Windows.Forms.MenuItem();
      this.menuItem3 = new System.Windows.Forms.MenuItem();
      this.menuItem4 = new System.Windows.Forms.MenuItem();
      this.menuItem6 = new System.Windows.Forms.MenuItem();
      this.menuItem7 = new System.Windows.Forms.MenuItem();
      this.menuItem5 = new System.Windows.Forms.MenuItem();
      this.menuItem2 = new System.Windows.Forms.MenuItem();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.panel1 = new System.Windows.Forms.Panel();
      this.panel3 = new System.Windows.Forms.Panel();
      this.label7 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.label5 = new System.Windows.Forms.Label();
      this.panel2 = new System.Windows.Forms.Panel();
      this.label9 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.lyraBtn = new System.Windows.Forms.Panel();
      this.panel4 = new System.Windows.Forms.Panel();
      this.panel1.SuspendLayout();
      this.panel3.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      // 
      // contextMenu1
      // 
      //this.contextMenu1.MenuItems.AddRange(new[]
      //                                               {
      //                                                   this.menuItem1,
      //                                                   this.menuItem6,
      //                                                   this.menuItem7,
      //                                                   this.menuItem5,
      //                                                   this.menuItem2
      //                                               });
       
      //// menuItem1
      //// 
      //this.menuItem1.Index = 0;
      //this.menuItem1.MenuItems.AddRange(new[]
      //                                            {
      //                                                this.menuItem3,
      //                                                this.menuItem4
      //                                            });
      //this.menuItem1.Text = "&Navigation";
      //// 
      //// menuItem3
      //// 
      //this.menuItem3.Index = 0;
      //this.menuItem3.Text = "&>>";
      //this.menuItem3.Click += this.MoveNext_Handler;
      //// 
      //// menuItem4
      //// 
      //this.menuItem4.Index = 1;
      //this.menuItem4.Text = "&<<";
      //this.menuItem4.Click += this.MovePrevious_Handler;
      //// 
      //// menuItem6
      //// 
      //this.menuItem6.Index = 1;
      //this.menuItem6.Text = "&rechtes Fenster benützen";
      //this.menuItem6.Click += this.menuItem6_Click;
      //// 
      //// menuItem7
      //// 
      //this.menuItem7.Index = 2;
      //this.menuItem7.Text = "&Originaltext anzeigen";
      //this.menuItem7.Visible = false;
      //this.menuItem7.Click += this.menuItem7_Click;
      //// 
      //// menuItem5
      //// 
      //this.menuItem5.Index = 3;
      //this.menuItem5.Text = "Über&setzungen";
      //// 
      //// menuItem2
      //// 
      //this.menuItem2.Index = 4;
      //this.menuItem2.Text = "schlie&ssen";
      //this.menuItem2.Click += this.menuItem2_Click;
      // 
      // label1
      // 
      this.label1.BackColor = System.Drawing.Color.Transparent;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold,
                                                 System.Drawing.GraphicsUnit.Point, ((0)));
      this.label1.ForeColor = System.Drawing.Color.Gainsboro;
      this.label1.Location = new System.Drawing.Point(64, -4);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(82, 38);
      this.label1.TabIndex = 2;
      this.label1.Text = "9999";
      this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
      // 
      // label2
      // 
      this.label2.BackColor = System.Drawing.Color.Transparent;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point,
                                                 ((0)));
      this.label2.ForeColor = System.Drawing.Color.Navy;
      this.label2.Location = new System.Drawing.Point(144, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(472, 30);
      this.label2.TabIndex = 3;
      this.label2.Text = "Title";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // panel1
      // 
      this.panel1.BackColor = System.Drawing.Color.Gainsboro;
      this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
      this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel1.Controls.Add(this.panel3);
      this.panel1.Controls.Add(this.label4);
      this.panel1.Controls.Add(this.label3);
      this.panel1.Controls.Add(this.label2);
      this.panel1.Location = new System.Drawing.Point(12, 8);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(552, 32);
      this.panel1.TabIndex = 4;
      this.panel1.KeyDown += this.View_KeyDown;
      // 
      // panel3
      // 
      this.panel3.BackColor = System.Drawing.Color.Transparent;
      this.panel3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel3.BackgroundImage")));
      this.panel3.Controls.Add(this.label1);
      this.panel3.Controls.Add(this.label7);
      this.panel3.Location = new System.Drawing.Point(0, 0);
      this.panel3.Name = "panel3";
      this.panel3.Size = new System.Drawing.Size(180, 32);
      this.panel3.TabIndex = 7;
      // 
      // label7
      // 
      this.label7.BackColor = System.Drawing.Color.Transparent;
      this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point,
                                                 ((0)));
      this.label7.ForeColor = System.Drawing.Color.Gainsboro;
      this.label7.Location = new System.Drawing.Point(8, -4);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(64, 34);
      this.label7.TabIndex = 6;
      this.label7.Text = "XXX";
      this.label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
      // 
      // label4
      // 
      this.label4.BackColor = System.Drawing.Color.Transparent;
      this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point,
                                                 ((0)));
      this.label4.ForeColor = System.Drawing.Color.Navy;
      this.label4.Location = new System.Drawing.Point(229, 0);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(472, 30);
      this.label4.TabIndex = 5;
      this.label4.Text = "Title";
      this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(520, 9);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(16, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "dt";
      // 
      // pictureBox1
      // 
      this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
      this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
      this.pictureBox1.Location = new System.Drawing.Point(24, 16);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(216, 136);
      this.pictureBox1.TabIndex = 8;
      this.pictureBox1.TabStop = false;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.BackColor = System.Drawing.Color.Transparent;
      this.label5.Font = new System.Drawing.Font("Verdana", 65.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((0)));
      this.label5.ForeColor = System.Drawing.Color.DimGray;
      this.label5.Location = new System.Drawing.Point(432, 40);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(293, 106);
      this.label5.TabIndex = 9;
      this.label5.Text = "1000";
      this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.label9);
      this.panel2.Controls.Add(this.label5);
      this.panel2.Controls.Add(this.pictureBox1);
      this.panel2.Location = new System.Drawing.Point(304, 360);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(752, 176);
      this.panel2.TabIndex = 11;
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.BackColor = System.Drawing.Color.Transparent;
      this.label9.Font = new System.Drawing.Font("Verdana", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((0)));
      this.label9.ForeColor = System.Drawing.Color.DarkGray;
      this.label9.Location = new System.Drawing.Point(272, 56);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(136, 78);
      this.label9.TabIndex = 10;
      this.label9.Text = "NB";
      this.label9.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // label8
      // 
      this.label8.BackColor = System.Drawing.Color.Transparent;
      this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular,
                                                 System.Drawing.GraphicsUnit.Point, ((0)));
      this.label8.ForeColor = System.Drawing.Color.DarkGray;
      this.label8.Location = new System.Drawing.Point(576, 280);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(56, 32);
      this.label8.TabIndex = 12;
      this.label8.Text = "next: 9999";
      this.label8.Visible = false;
      // 
      // lyraBtn
      // 
      this.lyraBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("lyraBtn.BackgroundImage")));
      this.lyraBtn.Location = new System.Drawing.Point(592, 8);
      this.lyraBtn.Name = "lyraBtn";
      this.lyraBtn.Size = new System.Drawing.Size(32, 32);
      this.lyraBtn.TabIndex = 13;
      this.lyraBtn.Click += this.LyraButton_Click;
      // 
      // panel4
      // 
      this.panel4.BackColor = System.Drawing.Color.Black;
      this.panel4.Location = new System.Drawing.Point(48, 272);
      this.panel4.Name = "panel4";
      this.panel4.Size = new System.Drawing.Size(64, 40);
      this.panel4.TabIndex = 14;
      this.panel4.Visible = false;
      // 
      // View
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.BackColor = System.Drawing.Color.White;
      this.ClientSize = new System.Drawing.Size(1096, 576);
      this.ControlBox = false;
      this.Controls.Add(this.panel4);
      this.Controls.Add(this.lyraBtn);
      this.Controls.Add(this.label8);
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.panel1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "View";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.TopMost = true;
      this.Load += this.View_Load;
      this.KeyDown += this.View_KeyDown;
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel3.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
    }

    #endregion

    private void View_Load(object sender, EventArgs e)
    {
      // init Screen
      this.Width = display.Bounds.Width;
      this.Height = display.Bounds.Height;
      this.Top = display.Bounds.Top;
      this.Left = display.Bounds.Left;

      this.lyraBtn.Top = 10;
      this.lyraBtn.Left = this.Width - 44;

      this.label8.Top = this.Height - this.label8.Height;
      this.label8.Left = this.Width - this.label8.Width;

      // init panel1
      this.panel1.Width = this.Width - 60;
      this.panel1.Top = 10;
      this.panel1.Left = 12;

      // init TextBoxes
      // this.richTextBox1.Left = 24;
      // this.richTextBox1.Width = this.Width - 80;
      // this.richTextBox1.Top = this.panel1.Top + this.panel1.Height + 20;
      // this.richTextBox1.Height = this.Height - this.richTextBox1.Top - 10;
      this.richTextBox2.Top = this.richTextBox1.Top;
      this.richTextBox2.Height = this.richTextBox1.Height;
      this.richTextBox2.Width = this.richTextBox1.Width / 2 - 5;
      this.richTextBox2.Left = this.richTextBox1.Left + this.richTextBox2.Width + 10;
      this.richTextBox2.Height = this.richTextBox1.Height;

      // init Titlebar
      this.label2.Width = this.richTextBox1.Width - this.label2.Left;
      this.label4.Visible = false;
      this.label4.Left = this.richTextBox2.Left - this.panel1.Left - 10;
      this.label3.Left = this.panel1.Width - this.label3.Width - 25;
      this.menuItem6.Checked = true;

      // show nr
      this.panel2.Width = display.Bounds.Width;
      this.panel2.Height = display.Bounds.Height;
      this.panel2.Top = 0;
      this.panel2.Left = 0;

      this.label5.Left = display.Bounds.Width / 2;
      this.label5.Top = display.Bounds.Height / 2 - this.label5.Height / 2;
      this.label9.Top = this.label5.Bottom + 2;
      this.label9.Left = this.label5.Left;
      this.pictureBox1.Left = this.Width / 2 - this.pictureBox1.Width;
      this.pictureBox1.Top = this.label5.Top;
      this.label5.Text = this.song.Number.ToString();
      this.label9.Text = this.song.Desc;
      this.panel1.Focus();

      if (this.trans != null)
      {
        this.refresh(this.song, this.trans);
      }
      else
      {
        this.refresh(this.song);
      }

      this.panel4.Bounds = this.Bounds;
      this.panel4.Visible = black;
    }

    private bool DISABLEACTIONS;


    private delegate void CrossThreadInvoke();

    private void showNrAtStart()
    {
      CrossThreadInvoke showNr = delegate
                                     {
                                       this.panel2.Show();
                                       this.label5.Text = this.song.Number.ToString();
                                       this.label9.Text = this.song.Desc;
                                       this.DISABLEACTIONS = true;
                                     };
      this.Invoke(showNr);
      Thread.Sleep(Util.TIMER * 1000);
      CrossThreadInvoke hideNr = delegate
                                     {
                                       this.DISABLEACTIONS = false;
                                       this.panel2.Hide();
                                       this.panel1.Focus();
                                       Util.CTRLSHOWNR = true;
                                     };
      this.Invoke(hideNr);
    }

    private void LyraButton_Click(object sender, EventArgs e)
    {
      this.contextMenu1.Show(this.lyraBtn, new Point(16, 24));
    }

    private void menuItem2_Click(object sender, EventArgs e)
    {
      this.song.uncheck();
      countViews--;
      this.owner.Status = "ok";
      this.Close();
    }

    // show song
    private void menuItem7_Click(object sender, EventArgs e)
    {
      this.refresh(this.song);
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
      this.refresh((ISong)this.navigate.Items[this.NextPos]);
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
      this.refresh((ISong)this.navigate.Items[this.LastPos]);
    }

    private int transCount;

    private void View_KeyDown(object sender, KeyEventArgs ke)
    {
      if (this.DISABLEACTIONS) return;
      if ((ke.KeyCode == Keys.F4 && ke.Alt) || (ke.KeyCode == Keys.Escape))
      {
        this.menuItem2_Click(sender, ke);
      }
      else if (ke.KeyCode == Keys.B && ke.Control)
      {
        BlackScreen(!black);
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
        this.refresh(this.song, this.song.GetTranslation(this.transCount++));
      }
      else if (ke.KeyCode == Keys.U)
      {
        this.refresh(this.song);
      }
    }

    private void menuItem6_Click(object sender, EventArgs e)
    {
      this.menuItem6.Checked = !this.menuItem6.Checked;
      Util.SHOWRIGHT = this.menuItem6.Checked;
      this.richTextBox2.Text = "";
      this.richTextBox2.Visible = false;
      this.label4.Text = "";
      this.label4.Visible = false;
      if (this.menuItem7.Visible)
      {
        this.menuItem7_Click(this, new EventArgs());
      }
      else
      {
        this.refresh(this.song);
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
      this.label8.Visible = this.isactivated;
    }

    private void updatePreview()
    {
      string next =
          ((ISong)this.navigate.Items[(this.pos + this.navigate.Items.Count + 1) % this.navigate.Items.Count]).
              Number.ToString();
      string last =
          ((ISong)this.navigate.Items[(this.pos + this.navigate.Items.Count - 1) % this.navigate.Items.Count]).
              Number.ToString();
      this.label8.Text = "PgUp:" + next + Util.NL + "PgDn:" + last;
    }

    private void richTextBox1_GotFocus(object sender, EventArgs e)
    {
      this.panel1.Focus();
    }

    private void View_Closed(object sender, EventArgs e)
    {
      _this = null; // delete link to closed View
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
        int pos = xml.IndexOf("name");
        if (pos >= 0)
        {
          int left = xml.IndexOf("\"", pos) + 1;
          int right = 0;
          if (left >= 0) right = xml.IndexOf("\"", left);
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