using Infragistics.Win.Misc;
using log4net;
using Lyra2.UtilShared;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
    /// <summary>
    /// Zusammendfassende Beschreibung für GUI.
    /// </summary>
    public class GUI : Form
    {
        #region    Log4Net Logger

        protected static readonly ILog Logger = LogManager.GetLogger(typeof(GUI));

        #endregion Log4Net Logger

        private IContainer components;

        #region Designervariablen

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Label label3;
        private LyraButtonControl button1;
        private SongListBox allSongsListBox;
        private LyraButtonControl button2;
        private StatusBar statusBar1;
        private StatusBarPanel statusBarPanel1;
        private LyraButtonControl button3;
        private StatusBarPanel statusBarPanel2;
        private ContextMenu contextMenu1;
        private MenuItem menuItem6;
        private MenuItem menuItem7;
        private TabPage tabPage3;
        private ContextMenu contextMenu2;
        private SongListBox personalListsListBox;
        private LyraButtonControl button4;
        private LyraButtonControl button5;
        private LyraButtonControl button6;
        private ComboBox persListCombo;
        private SearchTextBox textBox1;
        private LyraButtonControl button7;
        private RemoteControlUserControl remoteControl;
        private SearchTextBox mainSearchBox;
        private SongListBox searchListBox;
        private CheckBox checkBox1;
        private LyraButtonControl button9;
        private SearchTextBox textBox3;
        private UltraButton moveListItemDownBtn;
        private UltraButton moveListItemUpBtn;
        private MainMenu mainMenu1;
        private MenuItem menuItem1;
        private MenuItem menuItem2;
        private MenuItem menuItem3;
        private MenuItem menuItem4;
        private MenuItem menuItem5;
        private MenuItem menuItem8;
        private MenuItem menuItem13;
        private MenuItem menuItem14;
        private MenuItem menuItem15;
        private MenuItem menuItem16;
        private MenuItem menuItem17;
        private MenuItem menuItem18;
        private MenuItem menuItem19;
        private MenuItem menuItem20;
        private MenuItem menuItem21;
        private MenuItem menuItem24;
        private MenuItem menuItem30;
        private MenuItem menuItem31;
        private MenuItem menuItem9;
        private MenuItem menuItem10;
        private MenuItem menuItem11;
        private MenuItem menuItem12;
        private MenuItem menuItem22;
        private MenuItem menuItem23;
        private MenuItem menuItem26;
        private MenuItem menuItem27;
        private MenuItem menuItem28;
        private MenuItem menuItem29;
        private MenuItem menuItem32;
        private MenuItem menuItem33;
        private MenuItem menuItem34;
        private MenuItem menuItem35;
        private MenuItem menuItem36;
        private MenuItem menuItem37;
        private MenuItem menuItem25;
        private MenuItem menuItem39;
        private MenuItem menuItem38;
        private MenuItem menuItem40;
        private MenuItem menuItem41;
        private MenuItem menuItem42;
        private MenuItem menuItem43;
        private MenuItem menuItem44;
        private MenuItem menuItem45;
        private MenuItem menuItem46;
        private MenuItem menuItem47;
        private MenuItem menuItem48;
        private MenuItem menuItem49;
        private MenuItem menuItem50;
        private MenuItem menuItem51;
        private MenuItem menuItem52;
        private MenuItem menuItem54;
        private MenuItem menuItem55;
        private MenuItem menuItem56;
        private MenuItem menuItem57;
        private MenuItem menuItem58;
        private MenuItem menuItem59;
        private MenuItem menuItem61;
        private PictureBox pictureBox4;
        private Label resultsLabel;
        private LinkLabel linkLabel1;

        #endregion

        private IStorage storage;
        public int curItem = -1;
        private PLists persLists;
        private MenuItem menuItem62;
        private MenuItem menuItem63;
        private MenuItem menuItem64;
        private Label label4;
        private MenuItem menuItem65;
        private MenuItem menuItem70;
        private SongPreview songPreview1;
        private UltraPanel controlPaneRight;
        private UltraPanel panel1;
        private Panel panel4;
        private SongPreview songPreview3;
        private MenuItem menuItem60;
        private MenuItem menuItem66;
        private MenuItem menuItem67;
        private UltraPanel panel7;
        private Panel panel8;
        private Panel moveUpDownPanel;
        private SongPreview songPreview2;
        private PictureBox pictureBox1;
        private Label label2;
        private SplitContainer searchSplitter;
        private Panel searchPaneTop;
        private DelayedTaskRunner<EventArgs> searchTask;
        private SplitContainer allSongsSplitter;
        private SplitContainer persListSplitter;
        private MenuItem menuItem69;
        private MenuItem menuItem68;
        private MenuItem menuItem53;
        private readonly Personalizer personalizeStore;
        private ComboBox sortCombo;
        private Label sortLabel;
        private LyraButtonControl showSongBtn;
        private UltraLabel songManagmentLabel;
        private UltraLabel ultraLabel1;
        private UltraLabel listManagementLabel;

        public Personalizer Personalizer
        {
            get { return personalizeStore; }
        }

        public ListBox StandardNavigate
        {
            get { return allSongsListBox; }
        }

        private readonly Start start;

        public GUI()
        {
            Logger.Debug("Initialize Lyra");
            InitializeComponent();
            Enabled = false;
            personalizeStore = new Personalizer(Application.StartupPath + @"\laststate.dat");
            Logger.DebugFormat("Personalizer loaded from '{0}'", Application.StartupPath + @"\laststate.dat");

            InitializeLyraGUI();
            InitializeSearch();

            var bgWorker = new BackgroundWorker();
            bgWorker.DoWork += delegate { InitializeLyraData(); };
            start = new Start();
            start.StartPosition = FormStartPosition.CenterScreen;

            start.Show(this);
            bgWorker.RunWorkerCompleted += bgWorker_RunWorkerCompleted;
            bgWorker.RunWorkerAsync();

            if (Screen.AllScreens.Length == 1)
            {
                menuItem54.Enabled = false;
            }
            else
            {
                if (Util.SCREEN_ID == 0)
                {
                    menuItem55.Checked = true;
                }
                else
                {
                    menuItem56.Checked = true;
                }
            }
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Enabled = true;
            storage.DisplaySongs(allSongsListBox);
            Song.owner = this;
            persLists = new PLists(persListCombo, storage);
            if (persListCombo.Items.Count > 0)
            {
                persListCombo.SelectedIndex = 0;
            }
            else
            {
                menuItem7.Visible = false;
            }
            menuItem10.Visible = false;
            Deactivate += DeactivateMe;
            start.Hide();
        }

        private void InitializeLyraGUI()
        {
            Logger.Debug("Initialize GUI");
            Text = Util.GUINAME;
            menuItem41.Enabled = File.Exists(Util.BASEURL + "\\" + Util.URL + ".bac");
            WindowState = FormWindowState.Normal;

            allSongsListBox.MouseDown += listBox1_click;
            searchListBox.MouseDown += listBox3_click;
            allSongsListBox.KeyDown += listBox1_KeyDown;
            personalListsListBox.KeyDown += listBox2_KeyDown;
            searchListBox.KeyDown += listBox3_KeyDown;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            statusBar1.Visible = DEBUG;
            statusBarPanel1.Text = "ok";
            statusBarPanel2.Text = Util.URL;
            Resize += delegate { SizeSearchPane(); };
            personalizeStore.Load();
            sortCombo.SelectedIndex = 2;
            sortMethod = SortMethod.RatingDescending;
            searchListBox.BeginUpdate();
            searchListBox.Sort(sortMethod);
            searchListBox.EndUpdate();
            sortCombo.SelectedIndexChanged += SortMethodChanged;
            StartPosition = FormStartPosition.Manual;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (personalizeStore.GetIntValue(PersonalizationItemNames.HistoryIsShown) > 0)
            {
                History.ShowHistory(this);
            }
            if (personalizeStore.GetIntValue(PersonalizationItemNames.RemoteIsShown) > 0)
            {
                RemoteControl.ShowRemoteControl(this);
            }

            SizeSearchPane();

            // init splitters
            var searchSplit = personalizeStore.GetIntValue(PersonalizationItemNames.SearchSplitPosition);
            if (searchSplit >= 0) searchSplitter.SplitterDistance = searchSplit;
            var allSongsSplit = personalizeStore.GetIntValue(PersonalizationItemNames.AllSplitPosition);
            if (allSongsSplit >= 0) allSongsSplitter.SplitterDistance = allSongsSplit;
            var persListSplit = personalizeStore.GetIntValue(PersonalizationItemNames.ListSplitPosition);
            if (persListSplit >= 0) persListSplitter.SplitterDistance = persListSplit;

            // init GUI size
            var left = personalizeStore.GetIntValue(PersonalizationItemNames.GUILeft);
            if (left >= 0) Left = left;
            var top = personalizeStore.GetIntValue(PersonalizationItemNames.GUITop);
            if (top >= 0) Top = top;
            var width = personalizeStore.GetIntValue(PersonalizationItemNames.GUIWidth);
            if (width >= 0) Width = width;
            var height = personalizeStore.GetIntValue(PersonalizationItemNames.GUIHeight);
            if (height >= 0) Height = height;
        }

        private SortMethod sortMethod;

        private void SortMethodChanged(object sender, EventArgs e)
        {
            var newSortMethod = (SortMethod)sortCombo.SelectedIndex;
            if (sortMethod != newSortMethod)
            {
                sortMethod = newSortMethod;
                searchListBox.BeginUpdate();
                searchListBox.Sort(newSortMethod);
                searchListBox.EndUpdate();
            }
        }

        private void SizeSearchPane()
        {
            mainSearchBox.Width = panel4.Width - searchPaneTop.Width - 10;
            resultsLabel.Left = mainSearchBox.Right - resultsLabel.Width;
        }

        private void InitializeLyraData()
        {
            Logger.Debug("Initialize Data from " + Util.BASEURL + "\\" + Util.URL);
            storage = new Storage(Util.URL, this);
            Thread.Sleep(3000);
        }

        private delegate void InvokeSearch();

        private void InitializeSearch()
        {
            searchTask = new DelayedTaskRunner<EventArgs>(250);
            InvokeSearch searchInvoker = ExecuteSearch;
            searchTask.RunDelayedTask += (sender, args) => Invoke(searchInvoker);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            History.StorePersonalizationSettings(Personalizer, true);
            RemoteControl.StorePersonalizationSettings(Personalizer, true);
            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            personalizeStore[PersonalizationItemNames.GUILeft] = Left.ToString();
            personalizeStore[PersonalizationItemNames.GUITop] = Top.ToString();
            personalizeStore[PersonalizationItemNames.GUIWidth] = Width.ToString();
            personalizeStore[PersonalizationItemNames.GUIHeight] = Height.ToString();
            personalizeStore[PersonalizationItemNames.SearchSplitPosition] = searchSplitter.SplitterDistance.ToString();
            personalizeStore[PersonalizationItemNames.AllSplitPosition] = allSongsSplitter.SplitterDistance.ToString();
            personalizeStore[PersonalizationItemNames.ListSplitPosition] = persListSplitter.SplitterDistance.ToString();
            personalizeStore.Write();
            base.OnClosed(e);
        }

        /// <summary>
        /// Die verwendeten Ressourcen bereinigen.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            Util.storeStats();
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(GUI));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            this.tabControl1 = new TabControl();
            this.tabPage2 = new TabPage();
            this.searchSplitter = new SplitContainer();
            this.searchListBox = new SongListBox();
            this.label4 = new Label();
            this.songPreview3 = new SongPreview();
            this.panel4 = new Panel();
            this.sortCombo = new ComboBox();
            this.sortLabel = new Label();
            this.searchPaneTop = new Panel();
            this.pictureBox4 = new PictureBox();
            this.label2 = new Label();
            this.pictureBox1 = new PictureBox();
            this.mainSearchBox = new SearchTextBox();
            this.checkBox1 = new CheckBox();
            this.resultsLabel = new Label();
            this.panel1 = new UltraPanel();
            this.ultraLabel1 = new UltraLabel();
            this.textBox1 = new SearchTextBox();
            this.button7 = new LyraButtonControl();
            this.remoteControl = new RemoteControlUserControl(this);
            this.showSongBtn = new LyraButtonControl();
            this.tabPage1 = new TabPage();
            this.allSongsSplitter = new SplitContainer();
            this.allSongsListBox = new SongListBox();
            this.songPreview1 = new SongPreview();
            this.controlPaneRight = new UltraPanel();
            this.songManagmentLabel = new UltraLabel();
            this.button3 = new LyraButtonControl();
            this.button1 = new LyraButtonControl();
            this.button2 = new LyraButtonControl();
            this.button9 = new LyraButtonControl();
            this.tabPage3 = new TabPage();
            this.persListSplitter = new SplitContainer();
            this.personalListsListBox = new SongListBox();
            this.panel8 = new Panel();
            this.persListCombo = new ComboBox();
            this.button5 = new LyraButtonControl();
            this.moveUpDownPanel = new Panel();
            this.moveListItemDownBtn = new UltraButton();
            this.moveListItemUpBtn = new UltraButton();
            this.songPreview2 = new SongPreview();
            this.panel7 = new UltraPanel();
            this.listManagementLabel = new UltraLabel();
            this.linkLabel1 = new LinkLabel();
            this.button6 = new LyraButtonControl();
            this.button4 = new LyraButtonControl();
            this.label3 = new Label();
            this.textBox3 = new SearchTextBox();
            this.mainMenu1 = new MainMenu(this.components);
            this.menuItem1 = new MenuItem();
            this.menuItem13 = new MenuItem();
            this.menuItem14 = new MenuItem();
            this.menuItem15 = new MenuItem();
            this.menuItem17 = new MenuItem();
            this.menuItem30 = new MenuItem();
            this.menuItem31 = new MenuItem();
            this.menuItem18 = new MenuItem();
            this.menuItem16 = new MenuItem();
            this.menuItem2 = new MenuItem();
            this.menuItem19 = new MenuItem();
            this.menuItem35 = new MenuItem();
            this.menuItem36 = new MenuItem();
            this.menuItem20 = new MenuItem();
            this.menuItem21 = new MenuItem();
            this.menuItem51 = new MenuItem();
            this.menuItem52 = new MenuItem();
            this.menuItem69 = new MenuItem();
            this.menuItem68 = new MenuItem();
            this.menuItem53 = new MenuItem();
            this.menuItem40 = new MenuItem();
            this.menuItem63 = new MenuItem();
            this.menuItem64 = new MenuItem();
            this.menuItem38 = new MenuItem();
            this.menuItem41 = new MenuItem();
            this.menuItem45 = new MenuItem();
            this.menuItem44 = new MenuItem();
            this.menuItem49 = new MenuItem();
            this.menuItem50 = new MenuItem();
            this.menuItem43 = new MenuItem();
            this.menuItem34 = new MenuItem();
            this.menuItem46 = new MenuItem();
            this.menuItem42 = new MenuItem();
            this.menuItem47 = new MenuItem();
            this.menuItem48 = new MenuItem();
            this.menuItem24 = new MenuItem();
            this.menuItem3 = new MenuItem();
            this.menuItem57 = new MenuItem();
            this.menuItem59 = new MenuItem();
            this.menuItem65 = new MenuItem();
            this.menuItem70 = new MenuItem();
            this.menuItem58 = new MenuItem();
            this.menuItem54 = new MenuItem();
            this.menuItem55 = new MenuItem();
            this.menuItem56 = new MenuItem();
            this.menuItem61 = new MenuItem();
            this.menuItem62 = new MenuItem();
            this.menuItem10 = new MenuItem();
            this.menuItem26 = new MenuItem();
            this.menuItem23 = new MenuItem();
            this.menuItem28 = new MenuItem();
            this.menuItem27 = new MenuItem();
            this.menuItem11 = new MenuItem();
            this.menuItem22 = new MenuItem();
            this.menuItem12 = new MenuItem();
            this.menuItem4 = new MenuItem();
            this.menuItem5 = new MenuItem();
            this.menuItem33 = new MenuItem();
            this.menuItem29 = new MenuItem();
            this.menuItem32 = new MenuItem();
            this.menuItem37 = new MenuItem();
            this.statusBar1 = new StatusBar();
            this.statusBarPanel1 = new StatusBarPanel();
            this.statusBarPanel2 = new StatusBarPanel();
            this.contextMenu1 = new ContextMenu();
            this.menuItem6 = new MenuItem();
            this.menuItem7 = new MenuItem();
            this.menuItem39 = new MenuItem();
            this.menuItem8 = new MenuItem();
            this.menuItem25 = new MenuItem();
            this.contextMenu2 = new ContextMenu();
            this.menuItem60 = new MenuItem();
            this.menuItem66 = new MenuItem();
            this.menuItem67 = new MenuItem();
            this.menuItem9 = new MenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.searchSplitter.Panel1.SuspendLayout();
            this.searchSplitter.Panel2.SuspendLayout();
            this.searchSplitter.SuspendLayout();
            this.panel4.SuspendLayout();
            this.searchPaneTop.SuspendLayout();
            ((ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.ClientArea.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.allSongsSplitter.Panel1.SuspendLayout();
            this.allSongsSplitter.Panel2.SuspendLayout();
            this.allSongsSplitter.SuspendLayout();
            this.controlPaneRight.ClientArea.SuspendLayout();
            this.controlPaneRight.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.persListSplitter.Panel1.SuspendLayout();
            this.persListSplitter.Panel2.SuspendLayout();
            this.persListSplitter.SuspendLayout();
            this.panel8.SuspendLayout();
            this.moveUpDownPanel.SuspendLayout();
            this.panel7.ClientArea.SuspendLayout();
            this.panel7.SuspendLayout();
            ((ISupportInitialize)(this.statusBarPanel1)).BeginInit();
            ((ISupportInitialize)(this.statusBarPanel2)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(950, 449);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += this.tabControl1_SelectedIndexChanged;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = Color.WhiteSmoke;
            this.tabPage2.Controls.Add(this.searchSplitter);
            this.tabPage2.Controls.Add(this.panel4);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new Size(942, 423);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Suche";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // searchSplitter
            // 
            this.searchSplitter.Dock = DockStyle.Fill;
            this.searchSplitter.Location = new Point(0, 60);
            this.searchSplitter.Name = "searchSplitter";
            this.searchSplitter.Orientation = Orientation.Horizontal;
            // 
            // searchSplitter.Panel1
            // 
            this.searchSplitter.Panel1.Controls.Add(this.searchListBox);
            this.searchSplitter.Panel1.Controls.Add(this.label4);
            this.searchSplitter.Panel1MinSize = 150;
            // 
            // searchSplitter.Panel2
            // 
            this.searchSplitter.Panel2.Controls.Add(this.songPreview3);
            this.searchSplitter.Panel2MinSize = 50;
            this.searchSplitter.Size = new Size(830, 363);
            this.searchSplitter.SplitterDistance = 266;
            this.searchSplitter.TabIndex = 18;
            // 
            // searchListBox
            // 
            this.searchListBox.Dock = DockStyle.Fill;
            this.searchListBox.DrawMode = DrawMode.OwnerDrawFixed;
            this.searchListBox.ItemHeight = 15;
            this.searchListBox.Location = new Point(0, 0);
            this.searchListBox.Name = "searchListBox";
            this.searchListBox.NrOfNumberMatches = 0;
            this.searchListBox.Size = new Size(830, 244);
            this.searchListBox.TabIndex = 5;
            this.searchListBox.SelectedIndexChanged += this.listBox3_SelectedIndexChanged;
            this.searchListBox.DoubleClick += this.listBox3_dblClick;
            this.searchListBox.SelectedValueChanged += this.listBox3_SelectedValueChanged;
            // 
            // label4
            // 
            this.label4.Dock = DockStyle.Bottom;
            this.label4.ForeColor = Color.SaddleBrown;
            this.label4.Location = new Point(0, 251);
            this.label4.Name = "label4";
            this.label4.Size = new Size(830, 15);
            this.label4.TabIndex = 14;
            this.label4.Text = "Suchergebnisse könnten fehlerhaft sein. Bitte Änderungen übernehmen!";
            this.label4.Visible = false;
            // 
            // songPreview3
            // 
            this.songPreview3.AutoScroll = true;
            this.songPreview3.Dock = DockStyle.Fill;
            this.songPreview3.Location = new Point(0, 0);
            this.songPreview3.Name = "songPreview3";
            this.songPreview3.Size = new Size(830, 93);
            this.songPreview3.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.sortCombo);
            this.panel4.Controls.Add(this.sortLabel);
            this.panel4.Controls.Add(this.searchPaneTop);
            this.panel4.Controls.Add(this.mainSearchBox);
            this.panel4.Controls.Add(this.checkBox1);
            this.panel4.Controls.Add(this.resultsLabel);
            this.panel4.Dock = DockStyle.Top;
            this.panel4.Location = new Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(830, 60);
            this.panel4.TabIndex = 17;
            // 
            // sortCombo
            // 
            this.sortCombo.BackColor = Color.WhiteSmoke;
            this.sortCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            this.sortCombo.FlatStyle = FlatStyle.Flat;
            this.sortCombo.FormattingEnabled = true;
            this.sortCombo.Items.AddRange(new object[]
                                                    {
                                                  "Nummer (aufsteigend)",
                                                  "Nummer (absteigend)",
                                                  "Relevanz (absteigend)",
                                                  "Relevanz (aufsteigend)",
                                                  "Titel (aufsteigend)",
                                                  "Titel (absteiged)"
                                                    });
            this.sortCombo.Location = new Point(193, 32);
            this.sortCombo.Name = "sortCombo";
            this.sortCombo.Size = new Size(164, 21);
            this.sortCombo.TabIndex = 16;
            // 
            // sortLabel
            // 
            this.sortLabel.AutoSize = true;
            this.sortLabel.ForeColor = Color.DimGray;
            this.sortLabel.Location = new Point(111, 35);
            this.sortLabel.Name = "sortLabel";
            this.sortLabel.Size = new Size(76, 13);
            this.sortLabel.TabIndex = 15;
            this.sortLabel.Text = "Sortierung :";
            // 
            // searchPaneTop
            // 
            this.searchPaneTop.BackColor = Color.WhiteSmoke;
            this.searchPaneTop.Controls.Add(this.pictureBox4);
            this.searchPaneTop.Controls.Add(this.label2);
            this.searchPaneTop.Controls.Add(this.pictureBox1);
            this.searchPaneTop.Dock = DockStyle.Right;
            this.searchPaneTop.Location = new Point(630, 0);
            this.searchPaneTop.Name = "searchPaneTop";
            this.searchPaneTop.Size = new Size(200, 60);
            this.searchPaneTop.TabIndex = 2;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Cursor = Cursors.Hand;
            this.pictureBox4.Image = ((Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new Point(3, 8);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new Size(22, 22);
            this.pictureBox4.TabIndex = 12;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.Click += this.button8_Click;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = Color.DimGray;
            this.label2.Location = new Point(67, 12);
            this.label2.Name = "label2";
            this.label2.Size = new Size(121, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Suche zurücksetzen";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = Cursors.Hand;
            this.pictureBox1.Image = ((Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new Point(43, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(22, 22);
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += this.pictureBox1_Click;
            // 
            // mainSearchBox
            // 
            this.mainSearchBox.Anchor = ((((AnchorStyles.Top | AnchorStyles.Left)
                                           | AnchorStyles.Right)));
            this.mainSearchBox.BackColor = Color.FromArgb(((((253)))), ((((253)))), ((((176)))));
            this.mainSearchBox.DefaultText = "Suchbegriffe";
            this.mainSearchBox.Font = new Font("Verdana", 9F, FontStyle.Regular, GraphicsUnit.Point,
                                                              ((0)));
            this.mainSearchBox.ForeColor = Color.DimGray;
            this.mainSearchBox.Location = new Point(8, 8);
            this.mainSearchBox.Name = "mainSearchBox";
            this.mainSearchBox.Size = new Size(616, 22);
            this.mainSearchBox.TabIndex = 0;
            this.mainSearchBox.Text = "Suchbegriffe";
            this.mainSearchBox.TextChanged += this.textBox2_TextChanged;
            this.mainSearchBox.KeyDown += this.textBox2_KeyDown;
            // 
            // checkBox1
            // 
            this.checkBox1.CheckAlign = ContentAlignment.MiddleRight;
            this.checkBox1.Font = new Font("Verdana", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((0)));
            this.checkBox1.ForeColor = Color.DimGray;
            this.checkBox1.Location = new Point(8, 34);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(88, 18);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Nur Titel :";
            this.checkBox1.CheckedChanged += this.checkBox3_CheckedChanged;
            // 
            // resultsLabel
            // 
            this.resultsLabel.BackColor = Color.Transparent;
            this.resultsLabel.ForeColor = Color.DimGray;
            this.resultsLabel.Location = new Point(504, 32);
            this.resultsLabel.Name = "resultsLabel";
            this.resultsLabel.Size = new Size(100, 23);
            this.resultsLabel.TabIndex = 13;
            this.resultsLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // panel1
            // 
            appearance1.ImageBackground = Properties.Resources.right_pane_bg2025;
            appearance1.ImageBackgroundStretchMargins = new Infragistics.Win.ImageBackgroundStretchMargins(110, 100, 1, 350);
            appearance1.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
            this.panel1.Appearance = appearance1;
            // 
            // panel1.ClientArea
            // 
            this.panel1.ClientArea.Controls.Add(this.ultraLabel1);
            this.panel1.ClientArea.Controls.Add(this.textBox1);
            this.panel1.ClientArea.Controls.Add(this.button7);
            this.panel1.ClientArea.Controls.Add(this.showSongBtn);
            this.panel1.Dock = DockStyle.Right;
            this.panel1.Location = new Point(830, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(112, 423);
            this.panel1.TabIndex = 15;
            // 
            // ultraLabel1
            // 
            appearance15.BackColor = Color.Transparent;
            appearance15.BackColor2 = Color.FromArgb(((((255)))), ((((255)))), ((((192)))));
            appearance15.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            appearance15.FontData.ItalicAsString = "True";
            appearance15.ForeColor = Color.DimGray;
            appearance15.TextHAlignAsString = "Left";
            this.ultraLabel1.Appearance = appearance15;
            this.ultraLabel1.Location = new Point(12, 87);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Padding = new Size(10, 0);
            this.ultraLabel1.Size = new Size(96, 26);
            this.ultraLabel1.TabIndex = 7;
            this.ultraLabel1.Text = "Quick\r\n   View";
            // 
            // textBox1
            // 
            this.textBox1.DefaultText = "Nummer";
            this.textBox1.ForeColor = Color.DimGray;
            this.textBox1.Location = new Point(12, 119);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(96, 21);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "Nummer";
            this.textBox1.Click += this.textBox1_Click;
            this.textBox1.KeyDown += this.textBox1_KeyDown;
            // 
            // button7
            // 
            this.button7.Location = new Point(28, 144);
            this.button7.Name = "button7";
            this.button7.Size = new Size(80, 22);
            this.button7.TabIndex = 1;
            this.button7.Text = "Anzeigen";
            this.button7.Click += this.button7_Click;
            // 
            // remoteControl
            // 
            this.remoteControl.Location = new Point(0, 0);
            this.remoteControl.Name = "remoteControl";
            //            this.remoteControl.Size = new Size(0, 100);
            this.remoteControl.TabIndex = 1;
            this.remoteControl.Dock = DockStyle.Bottom;
            // 
            // showSongBtn
            // 
            appearance10.FontData.BoldAsString = "False";
            appearance10.FontData.SizeInPoints = 11F;
            appearance10.ForeColor = Color.FromArgb(((((0)))), ((((85)))), ((((170)))));
            this.showSongBtn.Appearance = appearance10;
            this.showSongBtn.ForeColor = Color.Brown;
            this.showSongBtn.Location = new Point(12, 8);
            this.showSongBtn.Name = "showSongBtn";
            this.showSongBtn.Size = new Size(96, 46);
            this.showSongBtn.TabIndex = 4;
            this.showSongBtn.Text = "Anzeigen!";
            this.showSongBtn.Click += this.listBox3_dblClick;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = Color.WhiteSmoke;
            this.tabPage1.Controls.Add(this.allSongsSplitter);
            this.tabPage1.Controls.Add(this.controlPaneRight);
            this.tabPage1.Location = new Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(942, 423);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Alle Lieder";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // allSongsSplitter
            // 
            this.allSongsSplitter.Dock = DockStyle.Fill;
            this.allSongsSplitter.Location = new Point(0, 0);
            this.allSongsSplitter.Name = "allSongsSplitter";
            this.allSongsSplitter.Orientation = Orientation.Horizontal;
            // 
            // allSongsSplitter.Panel1
            // 
            this.allSongsSplitter.Panel1.Controls.Add(this.allSongsListBox);
            this.allSongsSplitter.Panel1MinSize = 150;
            // 
            // allSongsSplitter.Panel2
            // 
            this.allSongsSplitter.Panel2.Controls.Add(this.songPreview1);
            this.allSongsSplitter.Panel2MinSize = 50;
            this.allSongsSplitter.Size = new Size(830, 423);
            this.allSongsSplitter.SplitterDistance = 292;
            this.allSongsSplitter.TabIndex = 8;
            // 
            // allSongsListBox
            // 
            this.allSongsListBox.Dock = DockStyle.Fill;
            this.allSongsListBox.DrawMode = DrawMode.OwnerDrawFixed;
            this.allSongsListBox.ItemHeight = 15;
            this.allSongsListBox.Location = new Point(0, 0);
            this.allSongsListBox.Name = "allSongsListBox";
            this.allSongsListBox.NrOfNumberMatches = 0;
            this.allSongsListBox.RightToLeft = RightToLeft.No;
            this.allSongsListBox.Size = new Size(830, 289);
            this.allSongsListBox.Sorted = true;
            this.allSongsListBox.TabIndex = 0;
            this.allSongsListBox.SelectedIndexChanged += this.listBox1_SelectedIndexChanged;
            this.allSongsListBox.DoubleClick += this.listBox1_dblClick;
            this.allSongsListBox.SelectedValueChanged += this.listBox1_SelectedValueChanged;
            // 
            // songPreview1
            // 
            this.songPreview1.AutoScroll = true;
            this.songPreview1.Dock = DockStyle.Fill;
            this.songPreview1.Location = new Point(0, 0);
            this.songPreview1.Name = "songPreview1";
            this.songPreview1.Size = new Size(830, 127);
            this.songPreview1.TabIndex = 0;
            // 
            // controlPaneRight
            // 
            appearance2.ImageBackground = Properties.Resources.right_pane_bg2025;
            appearance2.ImageBackgroundStretchMargins = new Infragistics.Win.ImageBackgroundStretchMargins(110, 100, 1, 350);
            appearance2.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
            this.controlPaneRight.Appearance = appearance2;
            // 
            // controlPaneRight.ClientArea
            // 
            this.controlPaneRight.ClientArea.Controls.Add(this.songManagmentLabel);
            this.controlPaneRight.ClientArea.Controls.Add(this.button3);
            this.controlPaneRight.ClientArea.Controls.Add(this.button1);
            this.controlPaneRight.ClientArea.Controls.Add(this.button2);
            this.controlPaneRight.ClientArea.Controls.Add(this.button9);
            this.controlPaneRight.Dock = DockStyle.Right;
            this.controlPaneRight.Location = new Point(830, 0);
            this.controlPaneRight.Name = "controlPaneRight";
            this.controlPaneRight.Size = new Size(112, 423);
            this.controlPaneRight.TabIndex = 7;
            // 
            // songManagmentLabel
            // 
            appearance13.BackColor = Color.Transparent;
            appearance13.BackColor2 = Color.FromArgb(((((255)))), ((((255)))), ((((192)))));
            appearance13.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            appearance13.FontData.ItalicAsString = "True";
            appearance13.ForeColor = Color.DimGray;
            appearance13.TextHAlignAsString = "Left";
            this.songManagmentLabel.Appearance = appearance13;
            this.songManagmentLabel.Location = new Point(12, 87);
            this.songManagmentLabel.Name = "songManagmentLabel";
            this.songManagmentLabel.Padding = new Size(10, 0);
            this.songManagmentLabel.Size = new Size(96, 26);
            this.songManagmentLabel.TabIndex = 6;
            this.songManagmentLabel.Text = "Lieder\r\n   verwalten";
            // 
            // button3
            // 
            appearance12.FontData.BoldAsString = "False";
            appearance12.FontData.SizeInPoints = 11F;
            appearance12.ForeColor = Color.FromArgb(((((0)))), ((((85)))), ((((170)))));
            this.button3.Appearance = appearance12;
            this.button3.ForeColor = Color.Brown;
            this.button3.Location = new Point(12, 8);
            this.button3.Name = "button3";
            this.button3.Size = new Size(96, 46);
            this.button3.TabIndex = 3;
            this.button3.Text = "Anzeigen!";
            this.button3.Click += this.button3_Click;
            // 
            // button1
            // 
            this.button1.Location = new Point(12, 119);
            this.button1.Name = "button1";
            this.button1.Size = new Size(96, 28);
            this.button1.TabIndex = 1;
            this.button1.Text = "Neues Lied…";
            this.button1.Click += this.button1_Click;
            // 
            // button2
            // 
            this.button2.Location = new Point(12, 153);
            this.button2.Name = "button2";
            this.button2.Size = new Size(96, 28);
            this.button2.TabIndex = 2;
            this.button2.Text = "Editieren…";
            this.button2.Click += this.button2_Click;
            // 
            // button9
            // 
            this.button9.Location = new Point(12, 187);
            this.button9.Name = "button9";
            this.button9.Size = new Size(96, 28);
            this.button9.TabIndex = 5;
            this.button9.Text = "Löschen";
            this.button9.Click += this.button9_Click;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = Color.WhiteSmoke;
            this.tabPage3.Controls.Add(this.persListSplitter);
            this.tabPage3.Controls.Add(this.panel7);
            this.tabPage3.Location = new Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new Size(942, 423);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Listen";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // persListSplitter
            // 
            this.persListSplitter.Dock = DockStyle.Fill;
            this.persListSplitter.Location = new Point(0, 0);
            this.persListSplitter.Name = "persListSplitter";
            this.persListSplitter.Orientation = Orientation.Horizontal;
            // 
            // persListSplitter.Panel1
            // 
            this.persListSplitter.Panel1.Controls.Add(this.personalListsListBox);
            this.persListSplitter.Panel1.Controls.Add(this.panel8);
            this.persListSplitter.Panel1.Controls.Add(this.moveUpDownPanel);
            this.persListSplitter.Panel1MinSize = 150;
            // 
            // persListSplitter.Panel2
            // 
            this.persListSplitter.Panel2.Controls.Add(this.songPreview2);
            this.persListSplitter.Panel2MinSize = 50;
            this.persListSplitter.Size = new Size(830, 423);
            this.persListSplitter.SplitterDistance = 292;
            this.persListSplitter.TabIndex = 17;
            // 
            // personalListsListBox
            // 
            this.personalListsListBox.Dock = DockStyle.Fill;
            this.personalListsListBox.DrawMode = DrawMode.OwnerDrawFixed;
            this.personalListsListBox.ItemHeight = 15;
            this.personalListsListBox.Location = new Point(24, 30);
            this.personalListsListBox.Name = "personalListsListBox";
            this.personalListsListBox.NrOfNumberMatches = 0;
            this.personalListsListBox.Size = new Size(806, 259);
            this.personalListsListBox.TabIndex = 0;
            this.personalListsListBox.SelectedIndexChanged += this.listBox2_SelectedIndexChanged;
            this.personalListsListBox.DoubleClick += this.listBox2_DoubleClick;
            this.personalListsListBox.SelectedValueChanged += this.listBox2_SelectedValueChanged;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.persListCombo);
            this.panel8.Controls.Add(this.button5);
            this.panel8.Dock = DockStyle.Top;
            this.panel8.Location = new Point(24, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new Size(806, 30);
            this.panel8.TabIndex = 15;
            // 
            // persListCombo
            // 
            this.persListCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            this.persListCombo.Location = new Point(1, 4);
            this.persListCombo.Name = "persListCombo";
            this.persListCombo.Size = new Size(448, 21);
            this.persListCombo.TabIndex = 4;
            this.persListCombo.SelectedIndexChanged += this.comboBox1_SelectedIndexChanged;
            // 
            // button5
            // 
            this.button5.Location = new Point(455, 4);
            this.button5.Name = "button5";
            this.button5.Size = new Size(96, 21);
            this.button5.TabIndex = 2;
            this.button5.Text = "Neue Liste…";
            this.button5.Click += this.button5_Click;
            // 
            // moveUpDownPanel
            // 
            this.moveUpDownPanel.Controls.Add(this.moveListItemDownBtn);
            this.moveUpDownPanel.Controls.Add(this.moveListItemUpBtn);
            this.moveUpDownPanel.Dock = DockStyle.Left;
            this.moveUpDownPanel.Location = new Point(0, 0);
            this.moveUpDownPanel.Name = "moveUpDownPanel";
            this.moveUpDownPanel.Size = new Size(24, 292);
            this.moveUpDownPanel.TabIndex = 16;
            this.moveUpDownPanel.Resize += this.MoveUpDownPanelResizeHandler;
            // 
            // moveListItemDownBtn
            // 
            appearance7.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance7.ForegroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance7.ImageAlpha = Infragistics.Win.Alpha.Transparent;
            appearance7.ImageBackground = Properties.Resources.arrow_down;
            this.moveListItemDownBtn.Appearance = appearance7;
            this.moveListItemDownBtn.BackgroundImage = Properties.Resources.pfeilDown;
            this.moveListItemDownBtn.BackgroundImageLayout = ImageLayout.Center;
            this.moveListItemDownBtn.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            appearance6.ImageBackground = Properties.Resources.arrow_down_over;
            this.moveListItemDownBtn.HotTrackAppearance = appearance6;
            this.moveListItemDownBtn.Location = new Point(5, 136);
            this.moveListItemDownBtn.Name = "moveListItemDownBtn";
            appearance9.ImageBackground = Properties.Resources.arrow_down_down;
            this.moveListItemDownBtn.PressedAppearance = appearance9;
            this.moveListItemDownBtn.ShowFocusRect = false;
            this.moveListItemDownBtn.ShowOutline = false;
            this.moveListItemDownBtn.Size = new Size(13, 37);
            this.moveListItemDownBtn.TabIndex = 7;
            this.moveListItemDownBtn.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.moveListItemDownBtn.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.moveListItemDownBtn.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.moveListItemDownBtn.Click += this.button12_Click;
            // 
            // moveListItemUpBtn
            // 
            appearance4.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance4.ForegroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance4.ImageAlpha = Infragistics.Win.Alpha.Transparent;
            appearance4.ImageBackground = Properties.Resources.arrow_up;
            appearance4.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
            this.moveListItemUpBtn.Appearance = appearance4;
            this.moveListItemUpBtn.BackgroundImage = Properties.Resources.pfeilUp;
            this.moveListItemUpBtn.BackgroundImageLayout = ImageLayout.Center;
            this.moveListItemUpBtn.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            appearance5.ImageBackground = Properties.Resources.arrow_up_over;
            this.moveListItemUpBtn.HotTrackAppearance = appearance5;
            this.moveListItemUpBtn.Location = new Point(5, 88);
            this.moveListItemUpBtn.Name = "moveListItemUpBtn";
            appearance8.ImageBackground = Properties.Resources.arrow_up_down;
            this.moveListItemUpBtn.PressedAppearance = appearance8;
            this.moveListItemUpBtn.ShowFocusRect = false;
            this.moveListItemUpBtn.ShowOutline = false;
            this.moveListItemUpBtn.Size = new Size(13, 37);
            this.moveListItemUpBtn.TabIndex = 8;
            this.moveListItemUpBtn.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.moveListItemUpBtn.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.moveListItemUpBtn.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.moveListItemUpBtn.Click += this.button13_Click;
            // 
            // songPreview2
            // 
            this.songPreview2.AutoScroll = true;
            this.songPreview2.Dock = DockStyle.Fill;
            this.songPreview2.Location = new Point(0, 0);
            this.songPreview2.Name = "songPreview2";
            this.songPreview2.Size = new Size(830, 127);
            this.songPreview2.TabIndex = 0;
            // 
            // panel7
            // 
            appearance3.ImageBackground = Properties.Resources.right_pane_bg2025;
            appearance3.ImageBackgroundStretchMargins = new Infragistics.Win.ImageBackgroundStretchMargins(110, 100, 1, 350);
            appearance3.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
            this.panel7.Appearance = appearance3;
            // 
            // panel7.ClientArea
            // 
            this.panel7.ClientArea.Controls.Add(this.listManagementLabel);
            this.panel7.ClientArea.Controls.Add(this.linkLabel1);
            this.panel7.ClientArea.Controls.Add(this.button6);
            this.panel7.ClientArea.Controls.Add(this.button4);
            this.panel7.ClientArea.Controls.Add(this.label3);
            this.panel7.ClientArea.Controls.Add(this.textBox3);
            this.panel7.Dock = DockStyle.Right;
            this.panel7.Location = new Point(830, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new Size(112, 423);
            this.panel7.TabIndex = 14;
            // 
            // listManagementLabel
            // 
            appearance14.BackColor = Color.Transparent;
            appearance14.BackColor2 = Color.FromArgb(((((255)))), ((((255)))), ((((192)))));
            appearance14.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            appearance14.FontData.ItalicAsString = "True";
            appearance14.ForeColor = Color.DimGray;
            appearance14.TextHAlignAsString = "Left";
            this.listManagementLabel.Appearance = appearance14;
            this.listManagementLabel.Location = new Point(12, 87);
            this.listManagementLabel.Name = "listManagementLabel";
            this.listManagementLabel.Padding = new Size(10, 0);
            this.listManagementLabel.Size = new Size(96, 26);
            this.listManagementLabel.TabIndex = 15;
            this.listManagementLabel.Text = "Listen\r\n   verwalten";
            // 
            // linkLabel1
            // 
            this.linkLabel1.ActiveLinkColor = Color.Orange;
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = Color.Transparent;
            this.linkLabel1.Font = new Font("Consolas", 11.25F, FontStyle.Bold, GraphicsUnit.Point, ((0)));
            this.linkLabel1.LinkColor = Color.FromArgb(((((64)))), ((((64)))), ((((64)))));
            this.linkLabel1.Location = new Point(12, 169);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new Size(24, 18);
            this.linkLabel1.TabIndex = 13;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "<<";
            this.linkLabel1.VisitedLinkColor = Color.FromArgb(((((64)))), ((((64)))), ((((64)))));
            this.linkLabel1.Click += this.button10_Click;
            // 
            // button6
            // 
            this.button6.Location = new Point(12, 119);
            this.button6.Name = "button6";
            this.button6.Size = new Size(96, 27);
            this.button6.TabIndex = 3;
            this.button6.Text = "Lied entfernen";
            this.button6.Click += this.button6_Click;
            // 
            // button4
            // 
            appearance11.FontData.SizeInPoints = 11F;
            appearance11.ForeColor = Color.FromArgb(((((0)))), ((((85)))), ((((170)))));
            this.button4.Appearance = appearance11;
            this.button4.Location = new Point(12, 8);
            this.button4.Name = "button4";
            this.button4.Size = new Size(96, 46);
            this.button4.TabIndex = 1;
            this.button4.Text = "Anzeigen!";
            this.button4.Click += this.button4_Click;
            // 
            // label3
            // 
            this.label3.BackColor = Color.Transparent;
            this.label3.ForeColor = Color.DimGray;
            this.label3.Location = new Point(12, 153);
            this.label3.Name = "label3";
            this.label3.Size = new Size(96, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Lied hinzufügen";
            // 
            // textBox3
            // 
            this.textBox3.DefaultText = "Nummer";
            this.textBox3.ForeColor = Color.DimGray;
            this.textBox3.Location = new Point(36, 169);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Size(72, 21);
            this.textBox3.TabIndex = 9;
            this.textBox3.Text = "Nummer";
            this.textBox3.Click += this.textBox3_Click;
            this.textBox3.KeyDown += this.textBox3_KeyDown;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new[]
                                                        {
                                                      this.menuItem1,
                                                      this.menuItem19,
                                                      this.menuItem57,
                                                      this.menuItem10,
                                                      this.menuItem4,
                                                      this.menuItem37
                                                  });
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new[]
                                                        {
                                                      this.menuItem13,
                                                      this.menuItem14,
                                                      this.menuItem17,
                                                      this.menuItem18,
                                                      this.menuItem16,
                                                      this.menuItem2
                                                  });
            this.menuItem1.Text = "&Datei";
            // 
            // menuItem13
            // 
            this.menuItem13.Enabled = false;
            this.menuItem13.Index = 0;
            this.menuItem13.Shortcut = Shortcut.CtrlS;
            this.menuItem13.Text = "Ü&bernehmen!";
            this.menuItem13.Click += this.CommitMenuItemClickHandler;
            // 
            // menuItem14
            // 
            this.menuItem14.Enabled = false;
            this.menuItem14.Index = 1;
            this.menuItem14.MenuItems.AddRange(new[]
                                                         {
                                                       this.menuItem15
                                                   });
            this.menuItem14.Text = "Änderungen &verwerfen!";
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 0;
            this.menuItem15.Text = "&Ok";
            this.menuItem15.Click += this.menuItem15_Click;
            // 
            // menuItem17
            // 
            this.menuItem17.Index = 2;
            this.menuItem17.MenuItems.AddRange(new[]
                                                         {
                                                       this.menuItem30,
                                                       this.menuItem31
                                                   });
            this.menuItem17.Text = "I&mportieren";
            // 
            // menuItem30
            // 
            this.menuItem30.Index = 0;
            this.menuItem30.Shortcut = Shortcut.CtrlShiftI;
            this.menuItem30.Text = "&XML";
            this.menuItem30.Click += this.menuItem30_Click;
            // 
            // menuItem31
            // 
            this.menuItem31.Index = 1;
            this.menuItem31.Text = "&LTX (Liedtext-Format)";
            this.menuItem31.Click += this.menuItem31_Click;
            // 
            // menuItem18
            // 
            this.menuItem18.Index = 3;
            this.menuItem18.Shortcut = Shortcut.CtrlShiftE;
            this.menuItem18.Text = "Ex&portieren…";
            this.menuItem18.Click += this.menuItem18_Click;
            // 
            // menuItem16
            // 
            this.menuItem16.Index = 4;
            this.menuItem16.Text = "-";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 5;
            this.menuItem2.Text = "Be&enden";
            this.menuItem2.Click += this.menuItem2_Click;
            // 
            // menuItem19
            // 
            this.menuItem19.Index = 1;
            this.menuItem19.MenuItems.AddRange(new[]
                                                         {
                                                       this.menuItem35,
                                                       this.menuItem36,
                                                       this.menuItem20,
                                                       this.menuItem21,
                                                       this.menuItem51,
                                                       this.menuItem40,
                                                       this.menuItem24,
                                                       this.menuItem3
                                                   });
            this.menuItem19.Text = "E&xtras";
            // 
            // menuItem35
            // 
            this.menuItem35.Index = 0;
            this.menuItem35.Shortcut = Shortcut.F12;
            this.menuItem35.Text = "&Präsentationsmodus";
            this.menuItem35.Click += this.menuItem35_Click;
            // 
            // menuItem36
            // 
            this.menuItem36.Index = 1;
            this.menuItem36.Text = "-";
            // 
            // menuItem20
            // 
            this.menuItem20.Enabled = false;
            this.menuItem20.Index = 2;
            this.menuItem20.Visible = false;
            this.menuItem20.Text = "Songbook drucken...";
            // 
            // menuItem21
            // 
            this.menuItem21.Index = 3;
            this.menuItem21.Text = "HTML Seite generieren…";
            this.menuItem21.Click += this.menuItem21_Click;
            // 
            // menuItem51
            // 
            this.menuItem51.Index = 4;
            this.menuItem51.MenuItems.AddRange(new[]
                                                         {
                                                       this.menuItem52,
                                                       this.menuItem69
                                                   });
            this.menuItem51.Text = "Unformatierter Text…";
            // 
            // menuItem52
            // 
            this.menuItem52.Index = 0;
            this.menuItem52.Text = "Ausgewählter Song";
            this.menuItem52.Click += this.menuItem52_Click;
            // 
            // menuItem69
            // 
            this.menuItem69.Index = 1;
            this.menuItem69.MenuItems.AddRange(new[]
                                                         {
                                                       this.menuItem68,
                                                       this.menuItem53
                                                   });
            this.menuItem69.Text = "Alle Songs";
            // 
            // menuItem68
            // 
            this.menuItem68.Index = 0;
            this.menuItem68.Text = "Titel-Index";
            this.menuItem68.Click += this.menuItem68_Click;
            // 
            // menuItem53
            // 
            this.menuItem53.Index = 1;
            this.menuItem53.Text = "Komplett";
            this.menuItem53.Click += this.menuItem53_Click;
            // 
            // menuItem40
            // 
            this.menuItem40.Index = 5;
            this.menuItem40.MenuItems.AddRange(new[]
                                                         {
                                                       this.menuItem63,
                                                       this.menuItem64,
                                                       this.menuItem38,
                                                       this.menuItem41,
                                                       this.menuItem45,
                                                       this.menuItem44,
                                                       this.menuItem43,
                                                       this.menuItem34,
                                                       this.menuItem46,
                                                       this.menuItem42,
                                                       this.menuItem47,
                                                       this.menuItem48
                                                   });
            this.menuItem40.Text = "E&xpertentools";
            // 
            // menuItem63
            // 
            this.menuItem63.Index = 0;
            this.menuItem63.Text = "Suchindex ern&euern";
            this.menuItem63.Click += this.menuItem63_Click;
            // 
            // menuItem64
            // 
            this.menuItem64.Index = 1;
            this.menuItem64.Text = "-";
            this.menuItem64.Visible = false;
            // 
            // menuItem38
            // 
            this.menuItem38.Index = 2;
            this.menuItem38.Text = "&Update lyra Songtexte...";
            this.menuItem38.Click += this.menuItem38_Click;
            this.menuItem38.Visible = false;
            // 
            // menuItem41
            // 
            this.menuItem41.Index = 3;
            this.menuItem41.Text = "Letztes Update &rückgängig machen";
            this.menuItem41.Click += this.menuItem41_Click;
            this.menuItem41.Visible = false;
            // 
            // menuItem45
            // 
            this.menuItem45.Index = 4;
            this.menuItem45.Text = "-";
            this.menuItem45.Visible = false;
            // 
            // menuItem44
            // 
            this.menuItem44.Enabled = false;
            this.menuItem44.Visible = false;
            this.menuItem44.Index = 5;
            this.menuItem44.MenuItems.AddRange(new[]
                                                         {
                                                       this.menuItem49,
                                                       this.menuItem50
                                                   });
            this.menuItem44.Text = "Dateien für Update-&Server erstellen";
            // 
            // menuItem49
            // 
            this.menuItem49.Index = 0;
            this.menuItem49.Text = "neuer Server...";
            this.menuItem49.Click += this.menuItem49_Click;
            // 
            // menuItem50
            // 
            this.menuItem50.Index = 1;
            this.menuItem50.Text = "für bestehenden Server...";
            this.menuItem50.Click += this.menuItem50_Click;
            // 
            // menuItem43
            // 
            this.menuItem43.Index = 6;
            this.menuItem43.Visible = false;
            this.menuItem43.Text = "-";
            // 
            // menuItem34
            // 
            this.menuItem34.Index = 7;
            this.menuItem34.Text = "Liste für &Pocket PC...";
            this.menuItem34.Click += this.menuItem34_Click;
            this.menuItem34.Visible = false;
            // 
            // menuItem46
            // 
            this.menuItem46.Index = 8;
            this.menuItem46.Text = "-";
            this.menuItem46.Visible = false;
            // 
            // menuItem42
            // 
            this.menuItem42.Visible = false;
            this.menuItem42.Index = 9;
            this.menuItem42.Text = "&Vorbereiten für Lyra 2.0";
            // 
            // menuItem47
            // 
            this.menuItem47.Index = 10;
            this.menuItem47.Text = "-";
            // 
            // menuItem48
            // 
            this.menuItem48.Index = 11;
            this.menuItem48.Text = "Lyra neu starten! (ohne Übernehmen)";
            this.menuItem48.Click += this.menuItem48_Click;
            // 
            // menuItem24
            // 
            this.menuItem24.Index = 6;
            this.menuItem24.Text = "-";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 7;
            this.menuItem3.Text = "&Optionen…";
            this.menuItem3.Click += this.menuItem3_Click;
            // 
            // menuItem57
            // 
            this.menuItem57.Index = 2;
            this.menuItem57.MenuItems.AddRange(new[]
                                                         {
                                                       this.menuItem59,
                                                       this.menuItem65,
                                                       this.menuItem70,
                                                       this.menuItem58,
                                                       this.menuItem54,
                                                       this.menuItem61,
                                                       this.menuItem62
                                                   });
            this.menuItem57.Text = "&Anzeige";
            this.menuItem57.Popup += this.menuItem57_Popup;
            // 
            // menuItem59
            // 
            this.menuItem59.Index = 0;
            this.menuItem59.Shortcut = Shortcut.CtrlH;
            this.menuItem59.Text = "History";
            this.menuItem59.Click += this.menuItem59_Click;
            // 
            // menuItem65
            // 
            this.menuItem65.Index = 1;
            this.menuItem65.Shortcut = Shortcut.CtrlR;
            this.menuItem65.Text = "Fernsteuerung";
            this.menuItem65.Click += this.menuItem65_Click;
            // 
            // menuItem70
            // 
            this.menuItem70.Index = 2;
            this.menuItem70.Shortcut = Shortcut.CtrlShiftS;
            this.menuItem70.Text = "Style Editor";
            this.menuItem70.Click += this.menuItem70_Click;
            // 
            // menuItem58
            // 
            this.menuItem58.Index = 3;
            this.menuItem58.Text = "-";
            // 
            // menuItem54
            // 
            this.menuItem54.Index = 4;
            this.menuItem54.MenuItems.AddRange(new[]
                                                         {
                                                       this.menuItem55,
                                                       this.menuItem56
                                                   });
            this.menuItem54.Text = "Anzeige&schirm";
            // 
            // menuItem55
            // 
            this.menuItem55.Index = 0;
            this.menuItem55.Text = "Primär";
            this.menuItem55.Click += this.menuItem55_Click;
            // 
            // menuItem56
            // 
            this.menuItem56.Index = 1;
            this.menuItem56.Text = "Sekundär";
            this.menuItem56.Click += this.menuItem56_Click;
            // 
            // menuItem61
            // 
            this.menuItem61.Index = 5;
            this.menuItem61.Shortcut = Shortcut.CtrlB;
            this.menuItem61.Text = "Anzeige aus&blenden";
            this.menuItem61.Click += this.menuItem61_Click;
            // 
            // menuItem62
            // 
            this.menuItem62.Index = 6;
            this.menuItem62.Shortcut = Shortcut.CtrlShiftB;
            this.menuItem62.Text = "Anzeige &schliessen";
            this.menuItem62.Click += this.menuItem62_Click;
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 3;
            this.menuItem10.MenuItems.AddRange(new[]
                                                         {
                                                       this.menuItem26,
                                                       this.menuItem23,
                                                       this.menuItem27,
                                                       this.menuItem11,
                                                       this.menuItem22,
                                                       this.menuItem12
                                                   });
            this.menuItem10.Text = "aktuelle &Liste";
            this.menuItem10.Visible = false;
            // 
            // menuItem26
            // 
            this.menuItem26.Index = 0;
            this.menuItem26.Text = "&Anzeigen!";
            this.menuItem26.Click += this.menuItem26_Click;
            // 
            // menuItem23
            // 
            this.menuItem23.Index = 1;
            this.menuItem23.MenuItems.AddRange(new[]
                                                         {
                                                       this.menuItem28
                                                   });
            this.menuItem23.Text = "Liste &löschen";
            // 
            // menuItem28
            // 
            this.menuItem28.Index = 0;
            this.menuItem28.Text = "&Ok";
            this.menuItem28.Click += this.menuItem28_Click;
            // 
            // menuItem27
            // 
            this.menuItem27.Index = 2;
            this.menuItem27.Text = "-";
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 3;
            this.menuItem11.Text = "Neue Liste…";
            this.menuItem11.Click += this.menuItem11_Click;
            // 
            // menuItem22
            // 
            this.menuItem22.Index = 4;
            this.menuItem22.Text = "Liste &importieren…";
            this.menuItem22.Click += this.menuItem22_Click;
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 5;
            this.menuItem12.Text = "Liste &exportieren…";
            this.menuItem12.Click += this.menuItem12_Click;
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 4;
            this.menuItem4.Text = "In&fo";
            this.menuItem4.Click += this.InfoMenuItemClickedHandler;

            // 
            // menuItem37
            // 
            this.menuItem37.Index = 5;
            this.menuItem37.Shortcut = Shortcut.F12;
            this.menuItem37.Text = "Präsentation b&eenden";
            this.menuItem37.Visible = false;
            this.menuItem37.Click += this.menuItem35_Click;
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new Point(0, 449);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Panels.AddRange(new[]
                                                      {
                                                    this.statusBarPanel1,
                                                    this.statusBarPanel2
                                                });
            this.statusBar1.ShowPanels = true;
            this.statusBar1.Size = new Size(950, 18);
            this.statusBar1.TabIndex = 1;
            this.statusBar1.Text = "d";
            // 
            // statusBarPanel1
            // 
            this.statusBarPanel1.Name = "statusBarPanel1";
            this.statusBarPanel1.Text = "cur state/action";
            this.statusBarPanel1.Width = 250;
            // 
            // statusBarPanel2
            // 
            this.statusBarPanel2.Name = "statusBarPanel2";
            this.statusBarPanel2.Text = "cur xml-path / options";
            this.statusBarPanel2.Width = 330;
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new[]
                                                           {
                                                         this.menuItem6,
                                                         this.menuItem7,
                                                         this.menuItem39,
                                                         this.menuItem8,
                                                         this.menuItem25
                                                     });
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 0;
            this.menuItem6.Text = "&Anzeigen!";
            this.menuItem6.Click += this.menuItem6_Click;
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 1;
            this.menuItem7.Text = "&Editieren…";
            this.menuItem7.Click += this.menuItem7_Click;
            // 
            // menuItem39
            // 
            this.menuItem39.Index = 2;
            this.menuItem39.Text = "&Löschen";
            this.menuItem39.Click += this.menuItem39_Click;
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 3;
            this.menuItem8.Text = "Zu aktueller &Liste hinzufügen";
            this.menuItem8.Click += this.menuItem8_Click;
            // 
            // menuItem25
            // 
            this.menuItem25.Index = 4;
            this.menuItem25.Text = "Überse&tzungen";
            // 
            // contextMenu2
            // 
            this.contextMenu2.MenuItems.AddRange(new[]
                                                           {
                                                         this.menuItem60,
                                                         this.menuItem66,
                                                         this.menuItem67,
                                                         this.menuItem9
                                                     });
            // 
            // menuItem60
            // 
            this.menuItem60.Index = 0;
            this.menuItem60.Text = "&Anzeigen!";
            this.menuItem60.Click += this.menuItem60_Click;
            // 
            // menuItem66
            // 
            this.menuItem66.Index = 1;
            this.menuItem66.Text = "&Editieren…";
            this.menuItem66.Click += this.menuItem66_Click;
            // 
            // menuItem67
            // 
            this.menuItem67.Index = 2;
            this.menuItem67.Text = "&Löschen";
            this.menuItem67.Click += this.menuItem67_Click;
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 3;
            this.menuItem9.Text = "Zu aktueller &Liste hinzufügen";
            this.menuItem9.Click += this.menuItem9_Click;
            // 
            // GUI
            // 
            this.AutoScaleBaseSize = new Size(6, 14);
            this.BackColor = Color.WhiteSmoke;
            this.ClientSize = new Size(950, 467);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.remoteControl);
            this.Controls.Add(this.statusBar1);
            this.Font = new Font("Verdana", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((0)));
            this.Icon = ((Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.MinimumSize = new Size(500, 300);
            this.Name = "GUI";
            this.Text = "lyra";
            this.WindowState = FormWindowState.Minimized;
            this.Activated += this.MeGotFocus;
            this.Closing += this.Exit;
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.searchSplitter.Panel1.ResumeLayout(false);
            this.searchSplitter.Panel2.ResumeLayout(false);
            this.searchSplitter.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.searchPaneTop.ResumeLayout(false);
            this.searchPaneTop.PerformLayout();
            ((ISupportInitialize)(this.pictureBox4)).EndInit();
            ((ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ClientArea.ResumeLayout(false);
            this.panel1.ClientArea.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.allSongsSplitter.Panel1.ResumeLayout(false);
            this.allSongsSplitter.Panel2.ResumeLayout(false);
            this.allSongsSplitter.ResumeLayout(false);
            this.controlPaneRight.ClientArea.ResumeLayout(false);
            this.controlPaneRight.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.persListSplitter.Panel1.ResumeLayout(false);
            this.persListSplitter.Panel2.ResumeLayout(false);
            this.persListSplitter.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.moveUpDownPanel.ResumeLayout(false);
            this.panel7.ClientArea.ResumeLayout(false);
            this.panel7.ClientArea.PerformLayout();
            this.panel7.ResumeLayout(false);
            ((ISupportInitialize)(this.statusBarPanel1)).EndInit();
            ((ISupportInitialize)(this.statusBarPanel2)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        public static bool DEBUG;

        // Activated -> assure, that the open Editor gets focus back!
        private void MeGotFocus(object sender, EventArgs e)
        {
            if (Editor.open)
            {
                Editor.editor.Focus();
            }
            if (curItem != -1)
                allSongsListBox.SelectedIndex = curItem;
            if (tabControl1.SelectedIndex == 0)
            {
                mainSearchBox.Focus();
                mainSearchBox.SelectAll();
            }
        }

        //deavtivated
        private void DeactivateMe(object sender, EventArgs e)
        {
            curItem = allSongsListBox.SelectedIndex;
        }

        // onDoubleClick on listElement
        private void listBox1_dblClick(object sender, EventArgs e)
        {
            try
            {
                if (allSongsListBox.SelectedItems.Count == 1)
                {
                    Util.CTRLSHOWNR = Util.SHOWNR;
                    View.ShowSong((ISong)allSongsListBox.SelectedItem, this, allSongsListBox, "Alle Lieder");
                }
            }
            catch (ToManyViews ex)
            {
                Util.MBoxError(ex.Message, ex);
            }
        }

        // Suche::onDoubleClick on listElement
        private void listBox3_dblClick(object sender, EventArgs e)
        {
            if (searchListBox.SelectedItem is ISong)
            {
                try
                {
                    if (searchListBox.SelectedItems.Count == 1)
                    {
                        Util.CTRLSHOWNR = Util.SHOWNR;
                        View.ShowSong((ISong)searchListBox.SelectedItem, this, searchListBox, $"Suche nach '{mainSearchBox.Text}'");
                    }
                }
                catch (ToManyViews ex)
                {
                    Util.MBoxError(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// functionality
        /// </summary>
        // neu...
        private void button1_Click(object sender, EventArgs e)
        {
            if (!Editor.open)
            {
                (new Editor(null, this, storage)).Show();
            }
            else
            {
                Editor.editor.Focus();
            }
            songPreview1.Reset();
        }

        // edit
        private void button2_Click(object sender, EventArgs e)
        {
            if (allSongsListBox.SelectedItems.Count == 1 && !Editor.open)
            {
                (new Editor((ISong)allSongsListBox.SelectedItem, this, storage)).Show();
            }
            else if (Editor.open)
            {
                Editor.editor.Focus();
            }
        }

        // del
        private void button9_Click(object sender, EventArgs e)
        {
            if (allSongsListBox.SelectedItems.Count == 1)
            {
                ((ISong)allSongsListBox.SelectedItem).Delete();
                UpdateListBox();
                ToUpdate(true);
                songPreview1.Reset();
            }
        }

        // anzeigen
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (allSongsListBox.SelectedItems.Count == 1)
                {
                    Util.CTRLSHOWNR = Util.SHOWNR;
                    View.ShowSong((ISong)allSongsListBox.SelectedItem, this, allSongsListBox, "Alle Lieder");
                }
            }
            catch (ToManyViews ex)
            {
                Util.MBoxError(ex.Message, ex);
            }
        }

        public ISong quickload(string nrstr)
        {
            try
            {
                var nr = Int32.Parse(nrstr);
                return storage.GetSong(nr);
            }
            catch
            {
            }
            return null;
        }

        // Quickload
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                var nr = Int32.Parse(textBox1.Text);
                var toShow = storage.GetSong(nr);
                if (toShow != null)
                {
                    try
                    {
                        Util.CTRLSHOWNR = !((KeyEventArgs)e).Control;
                    }
                    catch (Exception)
                    {
                        Util.CTRLSHOWNR = Util.SHOWNR;
                    }
                    View.ShowSong(toShow, this, allSongsListBox, $"Alle Lieder (Quickload für Nr. {nr.ToString().PadLeft(4, '0')})");
                }
                else
                {
                    Util.MBoxError("Lied konnte nicht gefunden werden!");
                }
            }
            catch (FormatException fe)
            {
                Util.MBoxError("Geben Sie bitte nur ganze, positive Zahlen ein!\n\n" +
                               fe.Message, fe);
            }
            catch (ToManyViews mv)
            {
                Util.MBoxError(mv.Message, mv);
            }
            catch (Exception ex)
            {
                Util.MBoxError(ex.Message, ex);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs ke)
        {
            if (ke.KeyCode == Keys.Enter)
            {
                button7_Click(sender, ke);
            }
        }

        /// <summary>
        /// MainMenu
        /// </summary>
        // Help
        private void menuItem5_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Util.BASEURL + "\\" + Util.HLPURL);
        }

        //sicheres Beenden
        private bool CheckOnExit()
        {
            var cancel = true;
            if (storage.ToBeCommited)
            {
                var res = MessageBox.Show(
                    "Vor dem Beenden Änderungen übernehmen?",
                    "Beenden von lyra",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (res == DialogResult.Yes)
                {
                    if (storage.Commit())
                    {
                        cancel = false;
                    }
                }
                else if (res == DialogResult.No)
                {
                    cancel = false;
                }
            }
            else
            {
                cancel = false;
            }
            return cancel;
        }

        // x-Exit
        private void Exit(object sender, CancelEventArgs args)
        {
            args.Cancel = CheckOnExit();
        }

        // beenden aus Menü
        private void menuItem2_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }


        // Übernehmen
        private void CommitMenuItemClickHandler(object sender, EventArgs e)
        {
            if (!Util.NOCOMMIT)
            {
                Cursor.Current = Cursors.WaitCursor;
                Enabled = false;
                if (storage.Commit())
                {
                    ToUpdate(false);
                    ResetSearch();
                    Status = "Änderungen erfolgreich übernommen und Index neu kreiert...";
                    Util.DELALL = false;
                }
                else
                {
                    Util.MBoxError("Fehler beim Übernehmen!");
                    Status = "Fehler beim Übernehmen!";
                }
                Enabled = true;
                Cursor.Current = Cursors.Default;
            }
            else
            {
                Util.MBoxError("Sie haben keine Berechtigung, Änderungen vorzunehmen!\n" +
                               "Entfernen Sie unter Extras->Optionen..., Allgemein... den Schreibschutz\n" +
                               "oder wenden Sie sich an den Administrator.");
                Status = "Fehler beim Übernehmen!";
            }
        }

        private void ResetSearch()
        {
            searchListBox.Items.Clear();
            searchListBox.ResetSearchTags();
            resultsLabel.Text = "";
            mainSearchBox.Text = "Suchbegriffe";
            mainSearchBox.ForeColor = Color.DimGray;
            label4.Visible = false;
        }

        // Änderungen verwerfen -> ok
        private void menuItem15_Click(object sender, EventArgs e)
        {
            storage.ResetToLast();
            ResetSearch();
            Util.DELALL = false;
            Status = "Änderungen verworfen!";
        }

        // exportieren
        private void menuItem18_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "XML Dateien (*.xml)|*.xml|Alle Dateien (*.*)|*.*";
            sfd.OverwritePrompt = true;
            sfd.Title = "In XML-Datei exportieren";
            var dr = sfd.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                Status = storage.Export(sfd.FileName) ? "Export erfolgreich! :-)" : "Beim Exportieren ging leider etwas schief. :-(";
            }
        }

        // import XML
        private void menuItem30_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "XML Dateien (*.xml)|*.xml|Alle Dateien (*.*)|*.*";
            ofd.CheckFileExists = true;
            ofd.Title = "XML-Datei importieren";
            var dr = ofd.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                var msg = "Sollen die Lieder der aktuellen Sammlung hinzugefügt werden?\n";
                msg += "(drücken Sie \"Nein\", falls die aktuellen Liedtexte gelöscht werden sollen)";
                var app = MessageBox.Show(this, msg,
                                                   "lyra", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                bool res;
                if (app == DialogResult.Yes)
                {
                    res = storage.Import(ofd.FileName, true);
                }
                else
                {
                    Util.DELALL = true;
                    res = storage.Import(ofd.FileName, false);
                }
                storage.DisplaySongs(allSongsListBox);
                ToUpdate(true);
                if (res)
                {
                    Status = "Importieren erfolgreich! :-)";
                }
                else
                {
                    Status = "Beim Importieren ging leider etwas schief. :-(";
                }
            }
        }

        // import LTX
        private void menuItem31_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Beachten Sie bitte, dass die LTX-Datei UTF-8-codiert sein muss." + Util.NL +
                                  "Sie können mit dem Converter-Tool ({lyra}\\Converter\\) selber LTX-Dateien generieren!" +
                                  Util.NL +
                                  "Mehr dazu finden Sie in der Hilfe (F1).",
                            "lyra", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            var ofd = new OpenFileDialog();
            ofd.Filter = "LTX Dateien (*.ltx)|*.ltx|Alle Dateien (*.*)|*.*";
            ofd.CheckFileExists = true;
            ofd.Title = "LTX-Datei importieren";
            var dr = ofd.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                var msg = "Sollen die Lieder der aktuellen Sammlung hinzugefügt werden?\n";
                msg += "(drücken Sie \"Nein\", falls die aktuellen Liedtexte gelöscht werden sollen)";
                var app = MessageBox.Show(this, msg,
                                                   "lyra", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (app == DialogResult.Yes)
                {
                    ImportLTX(ofd.FileName, true);
                }
                else
                {
                    ImportLTX(ofd.FileName, false);
                }
            }
        }

        private void ImportLTX(string url, bool append)
        {
            if (!append)
            {
                storage.Clear();
                Util.DELALL = true;
            }

            var reader = new StreamReader(url);
            try
            {
                string line;
                var nr = 0;
                var intext = false;
                while ((line = reader.ReadLine()) != null)
                {
                    var title = "";
                    if (intext)
                    {
                        var text = "";
                        if (line.EndsWith("#END"))
                        {
                            text += line.Substring(0, line.Length - 4);
                            ISong song = new Song(nr, title, text, "s" + Util.toFour(nr), "", true);
                            text = title = "";
                            intext = false;
                            try
                            {
                                storage.AddSong(song);
                            }
                            catch (ArgumentException)
                            {
                                var msg = "Wollen Sie Lied Nr." + nr + " ersetzen?\n";
                                msg += "(drücken Sie \"abbrechen\", wenn Sie das Lied überspringen wollen.)";
                                var dr =
                                    MessageBox.Show(this, msg,
                                                    "lyra import", MessageBoxButtons.YesNoCancel,
                                                    MessageBoxIcon.Question);
                                if (dr == DialogResult.Yes)
                                {
                                    storage.RemoveSong(song.ID);
                                    storage.AddSong(song);
                                }
                                else if (dr == DialogResult.No)
                                {
                                    song.nextID();
                                    storage.AddSong(song);
                                }
                                else
                                {
                                    // cancel <-> ignore song
                                }
                                continue;
                            }
                        }
                        else
                        {
                            text += line + "\n";
                        }
                    }
                    // first line of song
                    else if (line != "")
                    {
                        var words = line.Split(' ');
                        title = line.Substring(words[0].Length + 1);
                        nr = Int32.Parse(words[0]);
                        intext = true;
                    }
                }
                storage.DisplaySongs(allSongsListBox);
                ToUpdate(true);
                Status = "LTX-Import erfolgreich! :-)";
            }
            catch (Exception ie)
            {
                Util.MBoxError("Fehler beim Importieren.\n" + ie.Message, ie);
                Status = "Beim LTX-Import ging leider etwas schief. :-(";
            }
        }

        // reload the curtext.xml-file
        public void ReloadCurText()
        {
            storage.ResetToLast();
        }


        /// <summary>
        /// contextmenu
        /// </summary>
        // Kontextmenü
        private void listBox1_click(object sender, MouseEventArgs e)
        {
            if (prback == null)
            {
                var i = allSongsListBox.IndexFromPoint(e.X, e.Y);
                menuItem8.Visible = (persListCombo.Items.Count > 0);

                if (i != -1)
                {
                    allSongsListBox.SetSelected(i, true);
                    try
                    {
                        if (contextMenu1.MenuItems.Count > 4)
                        {
                            contextMenu1.MenuItems.RemoveAt(4);
                        }
                    }
                    catch (Exception)
                    {
                    }

                    if (((ISong)allSongsListBox.SelectedItem).TransMenu != null)
                        contextMenu1.MenuItems.Add(((ISong)allSongsListBox.SelectedItem).TransMenu);
                }
                if (e.Button == MouseButtons.Right && allSongsListBox.SelectedItems.Count == 1)
                {
                    contextMenu1.Show(allSongsListBox, new Point(e.X, e.Y));
                }
            }
        }

        // Kontextmenü Suche
        private void listBox3_click(object sender, MouseEventArgs e)
        {
            if (persListCombo.Items.Count > 0 && prback == null)
            {
                var i = searchListBox.IndexFromPoint(e.X, e.Y);
                if (e.Button == MouseButtons.Right && i != -1 &&
                    searchListBox.Items[i].ToString().Substring(0, 4) != "Leid")
                {
                    searchListBox.SelectedIndex = i;
                    contextMenu2.Show(searchListBox, new Point(e.X, e.Y));
                }
            }
        }

        // context-menu search list

        /// <summary>
        /// anzeigen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem60_Click(object sender, EventArgs e)
        {
            try
            {
                if (searchListBox.SelectedItems.Count == 1)
                {
                    Util.CTRLSHOWNR = Util.SHOWNR;
                    View.ShowSong((ISong)searchListBox.SelectedItem, this, searchListBox, $"Suche nach '{mainSearchBox.Text}'");
                }
            }
            catch (ToManyViews ex)
            {
                Util.MBoxError(ex.Message, ex);
            }
        }

        /// <summary>
        /// editieren
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem66_Click(object sender, EventArgs e)
        {
            if (!Editor.open)
                (new Editor((ISong)searchListBox.SelectedItem, this, storage)).Show();
            else
                Editor.editor.Focus();
        }

        /// <summary>
        /// löschen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem67_Click(object sender, EventArgs e)
        {
            ((ISong)searchListBox.SelectedItem).Delete();
            searchListBox.BeginUpdate();
            searchListBox.Items.RemoveAt(searchListBox.SelectedIndex);
            searchListBox.EndUpdate();
            ToUpdate(true);
            songPreview3.Reset();
        }

        // anzeigen
        private void menuItem6_Click(object sender, EventArgs e)
        {
            Util.CTRLSHOWNR = Util.SHOWNR;
            button3_Click(sender, e);
        }

        // edit
        private void menuItem7_Click(object sender, EventArgs e)
        {
            if (!Editor.open)
                (new Editor((ISong)allSongsListBox.SelectedItem, this, storage)).Show();
            else
                Editor.editor.Focus();
        }

        // del
        private void menuItem39_Click(object sender, EventArgs e)
        {
            ((ISong)allSongsListBox.SelectedItem).Delete();
            UpdateListBox();
            ToUpdate(true);
            songPreview1.Reset();
        }


        /// <summary>
        /// Util
        /// </summary>
        public void AddSong(ISong song)
        {
            storage.AddSong(song);
        }

        public void UpdateListBox()
        {
            storage.DisplaySongs(allSongsListBox);
            persLists.Refresh(personalListsListBox);
            songPreview1.Reset();
            songPreview2.Reset();
        }

        public void ToUpdate(bool update)
        {
            menuItem13.Enabled = update;
            menuItem14.Enabled = update;
            storage.ToBeCommited = update;
        }

        // Search
        private void button8_Click(object sender, EventArgs e)
        {
            searchTask.CancelTask();
            ExecuteSearch();
        }

        private void ExecuteSearch()
        {
            if (mainSearchBox.Text != "" && mainSearchBox.Text != "Suchbegriffe")
            {
                try
                {
                    Logger.DebugFormat("Search for '{0}'", mainSearchBox.Text);
                    storage.Search(mainSearchBox.Text, searchListBox, !checkBox1.Checked,
                                        false, false, false, sortMethod);
                    label4.Visible = storage.ToBeCommited;
                }
                catch (Exception ex)
                {
                    Util.MBoxError(
                        "Ihre Suchanfrage hat einen Fehler verursacht.\nIn der Hilfe (F1) finden Sie eine Anleitung.",
                        ex);
                    Logger.Debug("Search for '" + mainSearchBox.Text + "' FAILED!", ex);
                }
            }
            var res = searchListBox.Items.Count;
            switch (res)
            {
                case 0:
                    resultsLabel.Text = "Keine Resultate!";
                    break;
                case 1:
                    resultsLabel.Text = "Ein Resultat";
                    break;
                default:
                    resultsLabel.Text = res + " Resultate";
                    break;
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs ke)
        {
            if (ke.KeyCode == Keys.Enter)
            {
                searchTask.CancelTask();
                ExecuteSearch();
            }
            else if (ke.KeyCode == Keys.Delete)
            {
                if (mainSearchBox.SelectionStart + mainSearchBox.SelectionLength ==
                    mainSearchBox.Text.Length)
                {
                    ResetQuery();
                    ke.Handled = true;
                }
            }
            else if (ke.KeyCode == Keys.Down)
            {
                if (searchListBox.Items.Count > 0)
                {
                    searchListBox.Focus();
                    searchListBox.SelectedIndex = 0;
                }
            }
        }

        private void ResetQuery()
        {
            songPreview3.Reset();
            searchListBox.ClearSongs();
            mainSearchBox.Text = "";
            searchListBox.Focus();
            resultsLabel.Text = "";
        }

        public string Status
        {
            set { statusBarPanel1.Text = value; }
        }


        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }

        // Optionen
        private void menuItem3_Click(object sender, EventArgs e)
        {
            Options.ShowOptions(storage);
        }

        /**
         * MyLists
         */
        // MyLists-Change
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            persLists.setCurrent((MyList)persListCombo.SelectedItem, personalListsListBox);
            songPreview2.Reset();
            Status = "";
        }

        // anzeigen
        private void button4_Click(object sender, EventArgs e)
        {
            if (personalListsListBox.Items.Count > 0)
            {
                try
                {
                    Util.CTRLSHOWNR = Util.SHOWNR;
                    var ind = (personalListsListBox.SelectedIndex > 0) ? personalListsListBox.SelectedIndex : 0;
                    var currentList = (MyList)persListCombo.SelectedItem;
                    View.ShowSong((ISong)personalListsListBox.Items[ind], this, personalListsListBox, $"Liste '{currentList}'");
                }
                catch (ToManyViews ex)
                {
                    Util.MBoxError(ex.Message, ex);
                }
            }
        }

        // up
        private void button13_Click(object sender, EventArgs e)
        {
            if (personalListsListBox.SelectedItems.Count == 1)
            {
                var i = persLists.MoveSongUp(personalListsListBox.SelectedIndex);
                persLists.Refresh(personalListsListBox);
                personalListsListBox.SelectedIndex = i;
            }
        }

        // down
        private void button12_Click(object sender, EventArgs e)
        {
            if (personalListsListBox.SelectedItems.Count == 1)
            {
                var i = persLists.MoveSongDown(personalListsListBox.SelectedIndex);
                persLists.Refresh(personalListsListBox);
                personalListsListBox.SelectedIndex = i;
            }
        }

        // hinzufügen (aus Liste)
        private void menuItem8_Click(object sender, EventArgs e)
        {
            persLists.AddSongToCurrent((ISong)allSongsListBox.SelectedItem);
            persLists.Refresh(personalListsListBox);
            Status = "Song hinzugefügt.";
            songPreview2.Reset();
        }

        // hinzufügen (aus Suche)
        private void menuItem9_Click(object sender, EventArgs e)
        {
            persLists.AddSongToCurrent((ISong)searchListBox.SelectedItem);
            persLists.Refresh(personalListsListBox);
            Status = "Song hinzugefügt.";
            songPreview2.Reset();
        }

        // entfernen
        private void button6_Click(object sender, EventArgs e)
        {
            if (personalListsListBox.SelectedItems.Count == 1)
            {
                persLists.RemoveSongFromCurrent(personalListsListBox.SelectedIndex);
                persLists.Refresh(personalListsListBox);
                Status = "Song entfernt.";
                songPreview2.Reset();
            }
        }

        // Listen-Menü
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            menuItem10.Visible = (tabControl1.SelectedIndex == 2) && (prback == null);
            if (tabControl1.SelectedIndex == 0)
            {
                mainSearchBox.Focus();
                mainSearchBox.SelectAll();
            }
            label4.Visible = storage.ToBeCommited;
        }

        // anzeigen! (Menü)
        private void menuItem26_Click(object sender, EventArgs e)
        {
            button4_Click(sender, e);
        }

        // lösche Liste (Menü)
        private void menuItem28_Click(object sender, EventArgs e)
        {
            if (persListCombo.Items.Count == 1)
            {
                MessageBox.Show(this,
                                "Liste kann nicht gelöscht werden!" + Util.NL +
                                "(mindestens eine Liste muss vorhanden sein)",
                                "lyra Warnung",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (persListCombo.SelectedItem != null)
            {
                persLists.DeleteCurrent();
                persListCombo.Items.RemoveAt(persListCombo.SelectedIndex);
                if (persListCombo.Items.Count > 0)
                {
                    persListCombo.SelectedIndex = 0;
                }
                else
                {
                    personalListsListBox.Items.Clear();
                    persListCombo.Text = "";
                }
            }
        }

        // neue Liste
        public void CreateNewList(string name, string author)
        {
            CreateNewList(name, author, null);
        }

        public void CreateNewList(string name, string author, string[] songs)
        {
            var date = Util.GetDate();
            persLists.AddNewList(name, author, date, songs);
            persListCombo.SelectedIndex = persListCombo.Items.Count - 1;
            Status = "Neue Liste erstellt.";
            tabControl1.SelectedIndex = 2;
        }

        private void menuItem11_Click(object sender, EventArgs e)
        {
            NewList.ShowNewList(this);
            songPreview2.Reset();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            menuItem11_Click(sender, e);
        }

        // import Liste
        private void menuItem22_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "LLS Dateien (*.lls)|*.lls|Alle Dateien (*.*)|*.*";
            ofd.CheckFileExists = true;
            ofd.Title = "LyraListen-Datei importieren";
            var dr = ofd.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                importList(ofd.FileName);
            }
            Status = "Liste erfolgreich importiert. :-)";
        }

        private void importList(string url)
        {
            try
            {
                var reader = new StreamReader(url);
                var lls = reader.ReadLine();
                if (lls == "<LYRA LISTFILE>")
                {
                    var name = reader.ReadLine();
                    var author = reader.ReadLine();
                    var date = reader.ReadLine();
                    var songs = reader.ReadLine().Split(',');
                    persLists.AddNewList(name, author, date, songs);
                    persListCombo.SelectedIndex = persListCombo.Items.Count - 1;
                }
                else
                {
                    Util.MBoxError("Falsches File-Format.");
                    Status = "Fehler beim Importieren der Liste. :-(";
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Status = "Fehler beim Importieren der Liste. :-(";
                Util.MBoxError("Lyra-Listen-Datei nicht gefunden oder Format nicht korrekt!", ex);
            }
        }

        // export Liste
        private void menuItem12_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "LLS Dateien (*.lls)|*.lls|Alle Dateien (*.*)|*.*";
            sfd.OverwritePrompt = true;
            sfd.Title = "In LyraListen-Datei exportieren";
            var dr = sfd.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                try
                {
                    var writer = new StreamWriter(sfd.FileName);
                    writer.WriteLine("<LYRA LISTFILE>");
                    ((MyList)persListCombo.SelectedItem).exportMe(writer);
                }
                catch (Exception ex)
                {
                    Util.MBoxError("File kann nicht erstellt werden.", ex);
                    Status = "Fehler beim Exportieren!";
                }
            }
            Status = "Liste erfolgreich exportiert. :-)";
        }

        // HTML-Seite generieren...
        private void menuItem21_Click(object sender, EventArgs e)
        {
            ListBox box = null;
            var idtext = "";
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    box = searchListBox;
                    idtext = "Suchabfrage </b>[";
                    if (mainSearchBox.Text.Equals("Suchbegriffe..."))
                    {
                        idtext += "leer]";
                    }
                    else
                    {
                        if (checkBox1.Checked) idtext += "nur Titel,";
                        if (idtext[idtext.Length - 1] == ',') idtext = idtext.Substring(0, idtext.Length - 1);
                        idtext += "]:" + Util.HTMLNL + "<b>\"" + mainSearchBox.Text + "\"";
                    }
                    break;
                case 1:
                    box = allSongsListBox;
                    idtext = "ganze Liste";
                    break;
                case 2:
                    box = personalListsListBox;
                    idtext = "pers&ouml;nliche Liste:" + Util.HTMLNL + persListCombo.SelectedItem;
                    break;
            }
            HTML.showHTML(this, box, idtext);
        }

        // Debugging
        private void menuItem32_Click(object sender, EventArgs e)
        {
            DebugConsole.ShowDebugConsole(this);
        }


        // add song directly to list by nr
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                var curpos = personalListsListBox.SelectedIndex;
                var nr = Int32.Parse(textBox3.Text);
                var addSong = storage.GetSong(nr);
                if (addSong != null)
                {
                    persLists.AddSongToCurrent(addSong);
                    persLists.MoveLast(curpos);
                    persLists.Refresh(personalListsListBox);
                    Status = "Song hinzugefügt.";
                }
                else
                {
                    Status = "Song nicht gefunden.";
                    Util.MBoxError("Lied konnte nicht gefunden werden!");
                }
            }
            catch (FormatException fe)
            {
                Util.MBoxError("Geben Sie bitte nur ganze, positive Zahlen ein!\n\n" +
                               fe.Message, fe);
                textBox3.Text = "Liednr";
                textBox3.SelectAll();
                textBox3.Focus();
            }
            catch (Exception ex)
            {
                Util.MBoxError(ex.Message, ex);
            }
        }


        // Lists
        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.SelectAll();
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            button4_Click(sender, e);
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs ke)
        {
            if (ke.KeyCode == Keys.Enter)
            {
                button10_Click(sender, ke);
                textBox3.SelectAll();
                textBox3.Focus();
            }
        }

        // show info
        private void InfoMenuItemClickedHandler(object sender, EventArgs e)
        {
            Info.showInfo(this);
        }

        // Pocket PC
        private void menuItem34_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "LPPC Datei (data.lppc)|*.lppc";
            sfd.FileName = "data.lppc";
            sfd.OverwritePrompt = true;
            sfd.Title = "Liste für Pocket PC generieren.";
            var dr = sfd.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                if (storage.ExportPPC(sfd.FileName))
                {
                    Status = "Export für Pocket PC erfolgreich! :-)";
                }
                else
                {
                    Status = "Beim Exportieren für Pocket PC ging leider etwas schief. :-(";
                }
            }
        }

        #region Präsentationsmodus

        private PrBackground prback;

        private void menuItem35_Click(object sender, EventArgs e)
        {
            if (prback != null)
            {
                prback.Close();
                prback = null;
                hideForPresentation(false);
            }
            else
            {
                prback = new PrBackground(this);
                hideForPresentation(true);
                prback.Show();
                Focus();
            }
        }

        private void hideForPresentation(bool hide)
        {
            var visible = !hide;
            menuItem37.Visible = hide;
            menuItem1.Visible = visible;
            menuItem19.Visible = visible;
            menuItem4.Visible = visible;
            menuItem10.Visible = visible & (tabControl1.SelectedIndex == 2);
            listManagementLabel.Visible = visible;
            songManagmentLabel.Visible = visible;
            button1.Visible = visible;
            button2.Visible = visible;
            button9.Visible = visible;
            button6.Visible = visible;
            linkLabel1.Visible = visible;
            button5.Visible = visible;
            textBox3.Visible = visible;
            label3.Visible = visible;
        }

        #endregion

        // update lyra
        private void menuItem38_Click(object sender, EventArgs e)
        {
            if (LyraUpdate.ShowUpdate(this) == DialogResult.OK)
            {
                MessageBox.Show(this, "Update wurde durchgeführt!" + Util.NL + "lyra wird jetzt neu gestartet.",
                                "lyra Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                restart();
            }
            // else cancelled
        }

        // rollback last update
        private void menuItem41_Click(object sender, EventArgs e)
        {
            File.Copy(LyraUpdateView.BACKUP, LyraUpdateView.CURTEXT, true);
            File.Delete(LyraUpdateView.BACKUP);
            if (MessageBox.Show(this,
                                "Listen ebenfalls wieder auf den Stand" + Util.NL +
                                "vor dem letzten Update zurücksetzen?" +
                                Util.NL + Util.NL + "Vorsicht! Alle Änderungen seit dem letzten Update" + Util.NL +
                                "gehen verloren.",
                                "lyra Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                File.Copy(LyraUpdateView.BACKUPLIST, LyraUpdateView.LISTFILE, true);
                File.Delete(LyraUpdateView.BACKUPLIST);
            }
            MessageBox.Show(this, "Update wurde rückgängig gemacht!" + Util.NL + "lyra wird jetzt neu gestartet.",
                            "lyra Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            restart();
        }

        // restart lyra
        private void restart()
        {
            // Shut down the current app instance
            Application.Exit();
            // Restart the app
            Process.Start(Application.ExecutablePath);
        }

        // restart lyra client
        private void menuItem48_Click(object sender, EventArgs e)
        {
            restart();
        }

        // create new Server
        private void menuItem49_Click(object sender, EventArgs e)
        {
        }

        // from existing Server
        private void menuItem50_Click(object sender, EventArgs e)
        {
        }

        private readonly string[] formats =
            new[] { "refrain", "special", "p8", "p16", "p24", "p32", "p40", "b", "i", "pagebreak", "jumpmark" };

        private bool isFormat(string s)
        {
            s = s.TrimStart('/', ' ');
            foreach (var f in formats)
            {
                if (s.StartsWith(f))
                {
                    return true;
                }
            }
            return false;
        }

        private void showUnformated(ISong s)
        {
            var sfd = new SaveFileDialog();
            sfd.Title = "Bitte geben Sie den Pfad für die Textdatei an!";
            sfd.Filter = "Textdatei|*.txt";
            sfd.FileName = "lyra_" + s.Number + ".txt";
            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                var sw = new StreamWriter(sfd.FileName, false);
                var title = s.Number.ToString().PadRight(5, ' ') + ":  " + s.Title;
                var line = "".PadRight(title.Length, '-');
                sw.WriteLine(title);
                sw.WriteLine(line);
                sw.WriteLine();
                var text = "";
                for (var i = 0; i < s.Text.Length; i++)
                {
                    if (s.Text[i] == '<')
                    {
                        if (isFormat(s.Text.Substring(i + 1)))
                        {
                            if (s.Text.Substring(i + 1).StartsWith("refrain"))
                            {
                                text += "REFRAIN:" + Util.NL;
                            }
                            i = s.Text.IndexOf('>', i) + 1;
                        }
                    }
                    text += s.Text[i];
                }
                text = text.Replace("&gt;", ">").Replace("&lt;", "<").Replace("\n", "\r\n");
                sw.WriteLine(text);
                sw.Close();

                Process.Start("file://" + sfd.FileName);
            }
        }

        private void menuItem52_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                // search
                if (searchListBox.SelectedItem is ISong)
                {
                    var s = (ISong)searchListBox.SelectedItem;
                    showUnformated(s);
                    return;
                }
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                // selection
                if (allSongsListBox.SelectedItem is ISong)
                {
                    var s = (ISong)allSongsListBox.SelectedItem;
                    showUnformated(s);
                    return;
                }
            }
            else
            {
                // list selection
                if (personalListsListBox.SelectedItem is ISong)
                {
                    var s = (ISong)personalListsListBox.SelectedItem;
                    showUnformated(s);
                    return;
                }
            }
            MessageBox.Show(this, "Es ist kein Lied ausgewählt!", "Fehler!", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
        }

        private void menuItem53_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Title = "Bitte geben Sie den Pfad für die Textdatei an!";
            sfd.Filter = "Textdatei|*.txt";
            sfd.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_lyra" + ".txt";
            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                var sw = new StreamWriter(sfd.FileName, false);

                foreach (ISong s in allSongsListBox.Items)
                {
                    var title = s.Number.ToString().PadRight(5, ' ') + ":  " + s.Title;
                    var line = "".PadRight(title.Length, '-');
                    sw.WriteLine(title);
                    sw.WriteLine(line);
                    var text = "";
                    for (var i = 0; i < s.Text.Length; i++)
                    {
                        if (s.Text[i] == '<')
                        {
                            if (isFormat(s.Text.Substring(i + 1)))
                            {
                                if (s.Text.Substring(i + 1).StartsWith("refrain"))
                                {
                                    text += "$$_RefrainStart_$$";
                                }

                                if (s.Text.Substring(i + 2).StartsWith("refrain"))
                                {
                                    text += "$$_RefrainEnd_$$";
                                }

                                i = s.Text.IndexOf('>', i) + 1;
                            }
                        }

                        if (i < s.Text.Length)
                        {
                            text += s.Text[i];
                        }
                    }

                    text = text.Replace("&gt;", ">").Replace("&lt;", "<").Replace(Environment.NewLine, "\r").Replace('\n', '\r');

                    // clean blank lines
                    var cleanText = string.Empty;
                    var lastWasBlankLine = false;
                    foreach (var c in text)
                    {
                        if (c == '\r')
                        {
                            if (lastWasBlankLine)
                            {
                                continue;
                            }
                            cleanText += Environment.NewLine;
                            lastWasBlankLine = true;
                            continue;
                        }

                        cleanText += c;
                        lastWasBlankLine = false;
                    }


                    cleanText = cleanText.Replace("$$_RefrainEnd_$$", Environment.NewLine).Replace("$$_RefrainStart_$$", Environment.NewLine + "Refrain:" + Environment.NewLine);
                    while (cleanText.EndsWith("\r\n")) cleanText = cleanText.Substring(0, cleanText.Length - 2);
                    if (cleanText.StartsWith(Environment.NewLine))
                    {
                        cleanText = cleanText.Substring(2);
                    }

                    sw.WriteLine(cleanText);
                    sw.WriteLine();
                }
                sw.Close();

                Process.Start("file://" + sfd.FileName);
            }
        }


        private void menuItem55_Click(object sender, EventArgs e)
        {
            Util.SCREEN_ID = 0;
            View.Display = Util.GetScreen(0);
            Util.updateRegSettings();
            menuItem55.Checked = true;
            menuItem56.Checked = false;
        }

        private void menuItem56_Click(object sender, EventArgs e)
        {
            Util.SCREEN_ID = 1;
            View.Display = Util.GetScreen(1);
            Util.updateRegSettings();
            menuItem55.Checked = false;
            menuItem56.Checked = true;
        }

        private void menuItem61_Click(object sender, EventArgs e)
        {
            BlackScreen();
        }

        public void BlackScreen()
        {
            menuItem61.Checked = !menuItem61.Checked;
            View.BlackScreen(menuItem61.Checked);
        }

        private void menuItem57_Popup(object sender, EventArgs e)
        {
            menuItem61.Checked = View.Black;
        }

        private void menuItem59_Click(object sender, EventArgs e)
        {
            History.ShowHistory(this);
        }

        private void listBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            var s = searchListBox.SelectedItem as ISong;
            if (s != null)
            {
                songPreview3.ShowSong(s);
                searchListBox.Focus();
            }
            else
            {
                songPreview3.Reset();
            }
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            var s = allSongsListBox.SelectedItem as ISong;
            if (s != null)
            {
                songPreview1.ShowSong(s);
                allSongsListBox.Focus();
            }
            else
            {
                songPreview1.Reset();
            }
        }

        private void listBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            var s = personalListsListBox.SelectedItem as ISong;
            if (s != null)
            {
                songPreview2.ShowSong(s);
                personalListsListBox.Focus();
            }
            else
            {
                songPreview2.Reset();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            searchTask.RunTask(EventArgs.Empty);
            songPreview3.Reset();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            searchTask.CancelTask();
            ExecuteSearch();
        }

        private void menuItem62_Click(object sender, EventArgs e)
        {
            View.CloseView();
        }


        private void menuItem63_Click(object sender, EventArgs e)
        {
            if (storage.ToBeCommited)
            {
                MessageBox.Show(this,
                                "Index konnte nicht neu gebildet werden!" + Util.NL +
                                "Bitte zuerst alle Änderungen übernehmen oder verwerfen.",
                                "Suchindex erneuern...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (storage.CleanSearchIndex())
            {
                MessageBox.Show(this, "Index wurde erfolgreich neu gebildet.", "Suchindex erneuern...",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this,
                                "Index konnte nicht neu gebildet werden!" + Util.NL +
                                "Bitte lyra beenden und vor dem Neustart das Verzeichnis '<lyra>\\index' manuell löschen.",
                                "Suchindex erneuern...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void menuItem65_Click(object sender, EventArgs e)
        {
            RemoteControl.ShowRemoteControl(this);
        }

        private void menuItem70_Click(object sender, EventArgs e)
        {
            using (var se = new StyleEditor(storage))
            {
                se.StartPosition = FormStartPosition.CenterParent;
                se.ShowDialog(this);
            }
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                listBox1_dblClick(sender, e);
            }
        }

        private void listBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                listBox2_DoubleClick(sender, e);
            }
        }

        private void listBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                listBox3_dblClick(sender, e);
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (personalListsListBox.SelectedIndex < 0)
            {
                songPreview2.Reset();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (allSongsListBox.SelectedIndex < 0)
            {
                songPreview1.Reset();
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (searchListBox.SelectedIndex < 0)
            {
                songPreview3.Reset();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            songPreview3.Reset();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ResetQuery();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F3)
            {
                Util.OpenFile(0);
            }
            else if (keyData == Keys.F4)
            {
                Util.OpenFile(1);
            }
            else if (keyData == Keys.F5)
            {
                Util.OpenFile(2);
            }
            else if (keyData == Keys.F6)
            {
                Util.OpenFile(3);
            }
            else if (keyData == Keys.F7)
            {
                Util.OpenFile(4);
            }
            else if (keyData == Keys.F8)
            {
                Util.OpenFile(5);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void menuItem68_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Title = "Bitte geben Sie den Pfad für die Titel-Index Datei an!";
            sfd.Filter = "Textdatei|*.txt";
            sfd.FileName = "lyra_titel_index_" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                var sw = new StreamWriter(sfd.FileName, false);
                var fileTitle = "Lyra Titel-Index " + DateTime.Now.ToString("ddMMyyyy") + "  [" + allSongsListBox.Items.Count + " Songs]";
                sw.WriteLine(fileTitle);
                sw.WriteLine("".PadRight(fileTitle.Length, '-'));
                sw.WriteLine();
                foreach (ISong s in allSongsListBox.Items)
                {
                    var title = s.Number.ToString().PadRight(5, ' ') + ":  " + s.Title;
                    sw.WriteLine(title);
                }
                sw.Close();

                Process.Start("file://" + sfd.FileName);
            }
        }

        private void MoveUpDownPanelResizeHandler(object sender, EventArgs e)
        {
            var middle = (moveUpDownPanel.Height - 30) / 2 + 30;

            moveListItemUpBtn.Top = middle - 5 - moveListItemUpBtn.Height;
            moveListItemDownBtn.Top = middle + 5;
        }
    }
}