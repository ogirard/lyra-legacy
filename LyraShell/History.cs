using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Lyra2.UtilShared;

namespace Lyra2.LyraShell
{
    /// <summary>
    /// Summary description for History.
    /// </summary>
    public class History : Form
    {
        private SongListBox historyListBox;
        private IContainer components;
        private readonly GUI owner;
        private SplitContainer historySplitter;
        private SongPreview historySongPreview;
        private Infragistics.Win.Misc.UltraPanel bottomPanel;
        private LyraButtonControl ultraButton1;
        private LyraButtonControl historyAsListBtn;
        private Infragistics.Win.Misc.UltraPanel line;

        private static History _this;

        public static void ShowHistory(GUI owner)
        {
            if (_this == null)
            {
                _this = new History(owner);
                LoadPersonalizationSettings(owner.Personalizer);
            }
            _this.Show();
            _this.Focus();
        }

        public static bool IsShown
        {
            get { return _this != null; }
        }

        public static void ForceFocus()
        {
            if (_this != null)
            {
                _this.Focus();
            }
        }

        private readonly EventHandler changedHandler;

        private History(GUI owner)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.changedHandler = View_HistoryChanged;
            View.HistoryChanged += this.changedHandler;
            this.Closing += History_Closing;
            this.owner = owner;
            this.loadHistory();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.historySplitter = new System.Windows.Forms.SplitContainer();
            this.historyListBox = new Lyra2.LyraShell.SongListBox();
            this.historySongPreview = new Lyra2.LyraShell.SongPreview();
            this.bottomPanel = new Infragistics.Win.Misc.UltraPanel();
            this.line = new Infragistics.Win.Misc.UltraPanel();
            this.ultraButton1 = new Lyra2.LyraShell.LyraButtonControl();
            this.historyAsListBtn = new Lyra2.LyraShell.LyraButtonControl();
            this.historySplitter.Panel1.SuspendLayout();
            this.historySplitter.Panel2.SuspendLayout();
            this.historySplitter.SuspendLayout();
            this.bottomPanel.ClientArea.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.line.SuspendLayout();
            this.SuspendLayout();
            // 
            // historySplitter
            // 
            this.historySplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.historySplitter.Location = new System.Drawing.Point(0, 0);
            this.historySplitter.Name = "historySplitter";
            this.historySplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // historySplitter.Panel1
            // 
            this.historySplitter.Panel1.Controls.Add(this.historyListBox);
            // 
            // historySplitter.Panel2
            // 
            this.historySplitter.Panel2.Controls.Add(this.historySongPreview);
            this.historySplitter.Size = new System.Drawing.Size(593, 644);
            this.historySplitter.SplitterDistance = 405;
            this.historySplitter.TabIndex = 7;
            // 
            // historyListBox
            // 
            this.historyListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.historyListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.historyListBox.ItemHeight = 15;
            this.historyListBox.Location = new System.Drawing.Point(0, 0);
            this.historyListBox.Name = "historyListBox";
            this.historyListBox.NrOfNumberMatches = 0;
            this.historyListBox.Size = new System.Drawing.Size(593, 394);
            this.historyListBox.TabIndex = 6;
            this.historyListBox.SelectedIndexChanged += new System.EventHandler(this.historyListBox_SelectedIndexChanged);
            this.historyListBox.DoubleClick += new System.EventHandler(this.listBox3_DoubleClick);
            this.historyListBox.SelectedValueChanged += new System.EventHandler(this.listBox3_SelectedValueChanged);
            // 
            // historySongPreview
            // 
            this.historySongPreview.AutoScroll = true;
            this.historySongPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.historySongPreview.Location = new System.Drawing.Point(0, 0);
            this.historySongPreview.Name = "historySongPreview";
            this.historySongPreview.Size = new System.Drawing.Size(593, 235);
            this.historySongPreview.TabIndex = 1;
            // 
            // bottomPanel
            // 
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.GlassTop50;
            this.bottomPanel.Appearance = appearance1;
            // 
            // bottomPanel.ClientArea
            // 
            this.bottomPanel.ClientArea.Controls.Add(this.line);
            this.bottomPanel.ClientArea.Controls.Add(this.ultraButton1);
            this.bottomPanel.ClientArea.Controls.Add(this.historyAsListBtn);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 644);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(593, 29);
            this.bottomPanel.TabIndex = 8;
            // 
            // line
            // 
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            appearance2.BackColor = System.Drawing.Color.DimGray;
            this.line.Appearance = appearance2;
            this.line.Dock = System.Windows.Forms.DockStyle.Top;
            this.line.Location = new System.Drawing.Point(0, 0);
            this.line.Name = "line";
            this.line.Size = new System.Drawing.Size(593, 1);
            this.line.TabIndex = 1;
            // 
            // ultraButton1
            // 
            this.ultraButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButton1.Location = new System.Drawing.Point(217, 3);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(179, 23);
            this.ultraButton1.TabIndex = 0;
            this.ultraButton1.Text = "History in Text Datei speichern...";
            this.ultraButton1.Click += new System.EventHandler(this.SaveHistoryAsTextFile);
            // 
            // historyAsListBtn
            // 
            this.historyAsListBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.historyAsListBtn.Location = new System.Drawing.Point(404, 3);
            this.historyAsListBtn.Name = "historyAsListBtn";
            this.historyAsListBtn.Size = new System.Drawing.Size(179, 23);
            this.historyAsListBtn.TabIndex = 0;
            this.historyAsListBtn.Text = "History als Liste speichern...";
            this.historyAsListBtn.Click += new System.EventHandler(this.SaveAsListButtonClickHandler);
            // 
            // History
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(593, 673);
            this.Controls.Add(this.historySplitter);
            this.Controls.Add(this.bottomPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "History";
            this.ShowInTaskbar = false;
            this.Text = "History";
            this.historySplitter.Panel1.ResumeLayout(false);
            this.historySplitter.Panel2.ResumeLayout(false);
            this.historySplitter.ResumeLayout(false);
            this.bottomPanel.ClientArea.ResumeLayout(false);
            this.bottomPanel.ResumeLayout(false);
            this.line.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void View_HistoryChanged(object sender, EventArgs e)
        {
            this.loadHistory();
        }

        private void History_Closing(object sender, CancelEventArgs e)
        {
            View.HistoryChanged -= this.changedHandler;
            StorePersonalizationSettings(owner.Personalizer, false);
            _this = null;
        }

        private void loadHistory()
        {
            this.historyListBox.BeginUpdate();
            this.historyListBox.Items.Clear();
            foreach (Song s in View.SongHistory)
            {
                if (s.ID != Util.PREVIEW_SONG_ID)
                {
                    this.historyListBox.Items.Insert(0, s);
                }
            }
            
            if (this.historyListBox.Items.Count == 0)
            {
                this.historyListBox.Items.Add("Es sind noch keine Lieder geöffnet worden!");
            }
            else
            {
                // select first item in history
                this.historyListBox.SelectedIndex = 0;
            }
            this.historyListBox.EndUpdate();
        }

        private void listBox3_DoubleClick(object sender, EventArgs e)
        {
            if (this.historyListBox.SelectedItem is Song)
            {
                View.ShowSong((Song)this.historyListBox.SelectedItem, this.owner, this.historyListBox);
            }
        }

        private void listBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            ISong s = this.historyListBox.SelectedItem as ISong;
            if (s != null)
            {
                this.historySongPreview.ShowSong(s);
                this.historyListBox.Focus();
            }
            else
            {
                this.historySongPreview.Reset();
            }
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

        private void historyListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.historyListBox.SelectedIndex < 0)
            {
                this.historySongPreview.Reset();
            }
        }

        public static void StorePersonalizationSettings(Personalizer personalizer, bool shown)
        {
            if (_this != null)
            {
                personalizer[PersonalizationItemNames.HistoryTop] = _this.Top.ToString();
                personalizer[PersonalizationItemNames.HistoryLeft] = _this.Left.ToString();
                personalizer[PersonalizationItemNames.HistoryWidth] = _this.Width.ToString();
                personalizer[PersonalizationItemNames.HistoryHeight] = _this.Height.ToString();
                personalizer[PersonalizationItemNames.HistorySplit] = _this.historySplitter.SplitterDistance.ToString();
                personalizer[PersonalizationItemNames.HistoryIsShown] = shown ? "1" : "0";
                personalizer.Write();
            }
        }

        private static void LoadPersonalizationSettings(Personalizer personalizer)
        {
            if (_this != null)
            {
                personalizer.Load();
                int top = personalizer.GetIntValue(PersonalizationItemNames.HistoryTop);
                if (top > 0) _this.Top = top;
                int left = personalizer.GetIntValue(PersonalizationItemNames.HistoryLeft);
                if (left > 0) _this.Left = left;
                int width = personalizer.GetIntValue(PersonalizationItemNames.HistoryWidth);
                if (width > 0) _this.Width = width;
                int height = personalizer.GetIntValue(PersonalizationItemNames.HistoryHeight);
                if (height > 0) _this.Height = height;
                int split = personalizer.GetIntValue(PersonalizationItemNames.HistorySplit);
                if (split > 0) _this.historySplitter.SplitterDistance = split;

                personalizer[PersonalizationItemNames.HistoryIsShown] = "1";
                personalizer.Write();
            }
        }

        private void SaveAsListButtonClickHandler(object sender, EventArgs e)
        {

            List<string> songs = new List<string>();
            foreach (object song in this.historyListBox.Items)
            {
                if(song is ISong)
                {
                    songs.Add(((ISong)song).ID);
                }
                
            }
            NewList.ShowNewList(this.owner, "History vom " + DateTime.Now.ToString("dddd, dd.MM.yyyy"), songs.ToArray());
        }

        private void SaveHistoryAsTextFile(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Title = "Bitte Text Datei angeben";
            fd.AddExtension = true;
            fd.DefaultExt = ".txt";
            fd.Filter = "Text Dateien|*.txt";
            fd.RestoreDirectory = true;
            fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            fd.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_history.txt";
            if(fd.ShowDialog(this.owner) == DialogResult.OK)
            {
                using(StreamWriter sw = new StreamWriter(fd.FileName, false, Encoding.UTF8))
                {
                    sw.WriteLine("History vom " + DateTime.Now.ToString("dddd, dd.MM.yyyy"));
                    sw.WriteLine();
                    foreach (object item in this.historyListBox.Items)
                    {
                        if(item is ISong)
                        {
                            ISong s = (ISong) item;
                            sw.WriteLine(s.Number.ToString().PadLeft(5, ' ') + " : " + s.Title);
                        }
                    }   
                }
            }
        }
    }
}
