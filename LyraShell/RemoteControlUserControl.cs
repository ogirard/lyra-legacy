using Infragistics.Win.Misc;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Humanizer;
using Humanizer.Localisation;
using Infragistics.Win;
using Resources = Lyra2.LyraShell.Properties.Resources;
using Timer = System.Windows.Forms.Timer;

namespace Lyra2.LyraShell
{
    /// <summary>
    ///   Summary description for RemoteControl.
    /// </summary>
    public class RemoteControlUserControl : UserControl
    {
        /// <summary>
        ///   Required designer variable.
        /// </summary>
#pragma warning disable 649
        private IContainer components;
#pragma warning restore 649

        private readonly GUI owner;
        private UltraPanel remotePanel;
        private ListBox jumpMarksListBox;
        private LyraButtonControl lastBtn;
        private UltraPanel bottomPanel;
        private Panel lineContainer;
        private UltraPanel line;
        private LyraButtonControl nextBtn;
        private Label sourceLabel;
        private Label prevLabel;
        private Label nextLabel;
        private Panel mainPane;
        private Panel rightPane;
        private Label jumperLabel;
        private Panel scrollPane;
        private LyraButtonControl scrollUpwardsBtn;
        private LyraButtonControl jumpTopBtn;
        private LyraButtonControl jumpEndBtn;
        private LyraButtonControl scrollDownwardsBtn;
        private UltraPanel scrollBox;
        private UltraLabel titleLabel;
        private UltraLabel infoLabel;
        private PictureBox scrollImage;
        private ScrollVisualPanel scrollVisual;

        private DateTime? songDisplayStarted;

        private readonly BackgroundWorker bgw = new BackgroundWorker();
        private Timer timer;

        public RemoteControlUserControl(GUI owner)
        {
            //
            // Required for Windows Form Designer support
            //
            this.InitializeComponent();
            this.owner = owner;
            View.SongDisplayed += this.ViewSongDisplayed;
            View.ScrollDataChanged += this.ViewScrollDataChangedHandler;
            this.Update(View.CurrentSongInfo);
            this.bgw.DoWork += this.UnblinkWork;
            this.bgw.WorkerSupportsCancellation = true;
            foreach (Control control in this.Controls)
            {
                control.MouseWheel += this.MouseWheelHandler;
            }

            this.scrollBox.MouseWheel += this.MouseWheelHandler;
            this.MouseWheel += this.MouseWheelHandler;
            this.jumpMarksListBox.MouseWheel += this.MouseWheelHandler;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (songDisplayStarted == null)
            {
                this.infoLabel.Text = $"{DateTime.Now:D}";
            }
            else
            {
                var runningSince = (DateTime.Now - songDisplayStarted.Value).Humanize(culture: new CultureInfo("de-CH"), minUnit: TimeUnit.Minute);
                this.infoLabel.Text = $"{DateTime.Now:D} - Lied wird angezeigt seit {runningSince}";
            }
        }

        private void MouseWheelHandler(object sender, MouseEventArgs e)
        {
            #region    Precondition

            var mousePos = this.scrollBox.PointToClient(MousePosition);
            if (!this.scrollBox.ClientRectangle.Contains(mousePos)) return;

            #endregion Precondition

            if (e.Delta < 0)
            {
                // down
                this.scrollDownBtn_Click(this, e);
                this.ScrollBoxBlink(true);
            }
            else
            {
                this.scrollUpBtn_Click(this, e);
                this.ScrollBoxBlink(false);
            }
        }

        private void UnblinkWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(100);
            if (!e.Cancel)
            {
                this.Invoke(new MethodInvoker(() => this.scrollImage.Visible = false));
            }
        }

        private void ScrollBoxBlink(bool down)
        {
            this.scrollImage.Image = down ? Resources.scroll_down_bg : Resources.scroll_up_bg;
            this.scrollImage.Visible = true;
            if (!this.bgw.IsBusy)
            {
                this.bgw.RunWorkerAsync(Color.DimGray);
            }
        }

        private void ViewSongDisplayed(object sender, SongDisplayedEventArgs args)
        {
            this.Update(args);
        }

        private void ViewScrollDataChangedHandler(object sender, ScrollDataEventArgs e)
        {
            this.scrollVisual.UpdateScrollData(e);
        }

        private void Update(SongDisplayedEventArgs songInfo)
        {
            if (songInfo == null)
            {
                this.titleLabel.Text = @"Es wird kein Lied präsentiert.";
                this.titleLabel.Enabled = false;
                this.infoLabel.Text = $"{DateTime.Now:D}";
                this.sourceLabel.Text = "*";
                this.nextBtn.Enabled = false;
                this.lastBtn.Enabled = false;
                this.nextLabel.Visible = false;
                this.prevLabel.Visible = false;
                this.nextBtn.Visible = false;
                this.lastBtn.Visible = false;
                this.rightPane.Visible = false;
                this.sourceLabel.Visible = false;
                this.jumpMarksListBox.Items.Clear();
                this.scrollVisual.UpdateScrollData(null);
                this.songDisplayStarted = null;
                return;
            }

            this.songDisplayStarted = DateTime.Now;
            this.sourceLabel.Visible = true;
            this.rightPane.Visible = true;
            this.titleLabel.Text = songInfo.DisplayedSong != null ? $"{songInfo.DisplayedSong.Number.ToString().PadLeft(4, '0')} {songInfo.DisplayedSong.Title}" : "";
            this.titleLabel.Enabled = true;
            this.infoLabel.Text = $"{DateTime.Now:D}";
            this.sourceLabel.Text = songInfo.DisplayedSong != null ? $"Anzeigequelle [{songInfo.Source}]" : "";
            this.nextLabel.Visible = true;
            this.prevLabel.Visible = true;
            this.nextBtn.Visible = true;
            this.lastBtn.Visible = true;
            this.nextBtn.Enabled = songInfo.NextSong != null;
            this.lastBtn.Enabled = songInfo.PreviousSong != null;
            this.nextLabel.Text = songInfo.NextSong != null ? $"{songInfo.NextSong.Number.ToString().PadLeft(4, '0')} {songInfo.NextSong.Title}" : "";
            this.prevLabel.Text = songInfo.PreviousSong != null ? $"{songInfo.PreviousSong.Number.ToString().PadLeft(4, '0')} {songInfo.PreviousSong.Title}" : "";
            this.jumpMarksListBox.BeginUpdate();
            this.jumpMarksListBox.Items.Clear();
            foreach (var jumpMark in songInfo.Jumpmarks)
            {
                this.jumpMarksListBox.Items.Add(jumpMark);
            }

            this.jumpMarksListBox.EndUpdate();
        }

        /// <summary>
        ///   Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                {
                    this.components.Dispose();
                }
                View.SongDisplayed -= this.ViewSongDisplayed;
                View.ScrollDataChanged -= this.ViewScrollDataChangedHandler;
                this.timer.Tick -= OnTimerTick;
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///   Required method for Designer support - do not modify the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance infoAppearance = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.remotePanel = new Infragistics.Win.Misc.UltraPanel();
            this.mainPane = new System.Windows.Forms.Panel();
            this.lineContainer = new System.Windows.Forms.Panel();
            this.rightPane = new System.Windows.Forms.Panel();
            this.jumpMarksListBox = new System.Windows.Forms.ListBox();
            this.jumperLabel = new System.Windows.Forms.Label();
            this.scrollPane = new System.Windows.Forms.Panel();
            this.scrollVisual = new ScrollVisualPanel();
            this.scrollBox = new Infragistics.Win.Misc.UltraPanel();
            this.scrollImage = new System.Windows.Forms.PictureBox();
            this.jumpEndBtn = new Lyra2.LyraShell.LyraButtonControl();
            this.scrollDownwardsBtn = new Lyra2.LyraShell.LyraButtonControl();
            this.scrollUpwardsBtn = new Lyra2.LyraShell.LyraButtonControl();
            this.jumpTopBtn = new Lyra2.LyraShell.LyraButtonControl();
            this.bottomPanel = new Infragistics.Win.Misc.UltraPanel();
            this.titleLabel = new Infragistics.Win.Misc.UltraLabel();
            this.infoLabel = new Infragistics.Win.Misc.UltraLabel();
            this.sourceLabel = new System.Windows.Forms.Label();
            this.nextLabel = new System.Windows.Forms.Label();
            this.prevLabel = new System.Windows.Forms.Label();
            this.line = new Infragistics.Win.Misc.UltraPanel();
            this.nextBtn = new Lyra2.LyraShell.LyraButtonControl();
            this.lastBtn = new Lyra2.LyraShell.LyraButtonControl();
            this.remotePanel.ClientArea.SuspendLayout();
            this.remotePanel.SuspendLayout();
            this.mainPane.SuspendLayout();
            this.lineContainer.SuspendLayout();
            this.rightPane.SuspendLayout();
            this.scrollPane.SuspendLayout();
            this.scrollBox.ClientArea.SuspendLayout();
            this.scrollBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scrollImage)).BeginInit();
            this.bottomPanel.ClientArea.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.line.SuspendLayout();
            this.SuspendLayout();


            this.timer = new Timer()
            {
                Interval = 1_000,
                Enabled = true
            };

            this.timer.Tick += OnTimerTick;
            // 
            // remotePanel
            // 
            // 
            // remotePanel.ClientArea
            // 
            this.Controls.Add(this.remotePanel);
            this.remotePanel.ClientArea.Controls.Add(this.rightPane);
            this.rightPane.Controls.Add(this.mainPane);
            this.rightPane.Controls.Add(this.scrollPane);
            this.rightPane.Controls.Add(this.lineContainer);
            this.lineContainer.Controls.Add(this.line);
            this.remotePanel.ClientArea.Controls.Add(this.bottomPanel);
            this.remotePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.remotePanel.Location = new System.Drawing.Point(0, 0);
            this.remotePanel.Name = "remotePanel";
            this.remotePanel.TabIndex = 11;
            //appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))),
            //    ((int)(((byte)(224)))));
            appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))),
                ((int)(((byte)(213)))));
            appearance21.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))),
                ((int)(((byte)(241)))));
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.remotePanel.Appearance = appearance21;
            // 
            // mainPane
            // 
            this.mainPane.Controls.Add(this.jumpMarksListBox);
            this.mainPane.Controls.Add(this.jumperLabel);
            this.mainPane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPane.Name = "mainPane";
            this.mainPane.TabIndex = 12;
            // 
            // lineContainer
            // 
            this.lineContainer.Dock = System.Windows.Forms.DockStyle.Left;
            this.lineContainer.Name = "lineContainer";
            this.lineContainer.Size = new Size(5, 10);
            this.lineContainer.BackColor = Color.Transparent;

            // rightPane
            this.rightPane.Dock = DockStyle.Right;
            this.rightPane.Name = "rightPane";
            this.rightPane.Size = new Size(500, 10);
            this.rightPane.DockPadding.Top = 5;
            this.rightPane.DockPadding.Bottom = 5;
            this.rightPane.DockPadding.Right = 8;
            // 
            // jumpMarksListBox
            // 
            this.jumpMarksListBox.Anchor =
              ((System.Windows.Forms.AnchorStyles)
               ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                 | System.Windows.Forms.AnchorStyles.Right)));
            this.jumpMarksListBox.BackColor = Color.White;
            this.jumpMarksListBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular,
                                                                 System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jumpMarksListBox.FormattingEnabled = true;
            this.jumpMarksListBox.ItemHeight = 16;
            this.jumpMarksListBox.Items.AddRange(new object[]
                                                   {
                                               "Test 1"
                                                   });
            this.jumpMarksListBox.Location = new System.Drawing.Point(9, 25);
            this.jumpMarksListBox.Dock = DockStyle.Fill;
            this.jumpMarksListBox.Name = "jumpMarksListBox";
            this.jumpMarksListBox.Size = new System.Drawing.Size(300, 50);
            this.jumpMarksListBox.TabIndex = 0;
            this.jumpMarksListBox.Click += new System.EventHandler(this.JumpMarksDoubleClickHandler);
            this.jumpMarksListBox.DoubleClick += new System.EventHandler(this.JumpMarksDoubleClickHandler);
            // 
            // jumperLabel
            // 
            this.jumperLabel.AutoSize = true;
            this.jumperLabel.BackColor = System.Drawing.Color.Transparent;
            this.jumperLabel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular,
                                                            System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jumperLabel.ForeColor = System.Drawing.Color.White;
            this.jumperLabel.Dock = DockStyle.Top;
            this.jumperLabel.Name = "jumperLabel";
            this.jumperLabel.Size = new System.Drawing.Size(103, 24);
            this.jumperLabel.TabIndex = 11;
            this.jumperLabel.Text = "Sprungmarken";
            // 
            // scrollPane
            // 
            this.scrollPane.Controls.Add(this.scrollVisual);
            this.scrollPane.Controls.Add(this.scrollBox);
            this.scrollPane.Controls.Add(this.jumpEndBtn);
            this.scrollPane.Controls.Add(this.scrollDownwardsBtn);
            this.scrollPane.Controls.Add(this.scrollUpwardsBtn);
            this.scrollPane.Controls.Add(this.jumpTopBtn);
            this.scrollPane.Dock = System.Windows.Forms.DockStyle.Left;
            this.scrollPane.Name = "scrollPane";
            this.scrollPane.Size = new System.Drawing.Size(140, 131);
            this.scrollPane.Margin = new Padding(15, 0, 15, 0);
            this.scrollPane.Padding = new Padding(15, 0, 15, 0);
            this.scrollPane.TabIndex = 11;
            // 
            // scrollVisual
            // 
            this.scrollVisual.Location = new System.Drawing.Point(113, 22);
            this.scrollVisual.Name = "scrollVisual";
            this.scrollVisual.Size = new System.Drawing.Size(9, 119);
            this.scrollVisual.TabIndex = 14;
            // 
            // scrollBox
            // 
            appearance1.BackColor = System.Drawing.Color.DimGray;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Rectangular;
            appearance1.BorderColor = System.Drawing.Color.Silver;
            this.scrollBox.Appearance = appearance1;
            this.scrollBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Rounded1;
            // 
            // scrollBox.ClientArea
            // 
            this.scrollBox.ClientArea.Controls.Add(this.scrollImage);
            this.scrollBox.Location = new System.Drawing.Point(53, 57);
            this.scrollBox.Name = "scrollBox";
            this.scrollBox.Size = new System.Drawing.Size(50, 50);
            this.scrollBox.TabIndex = 13;
            // 
            // scrollImage
            // 
            this.scrollImage.Location = new System.Drawing.Point(0, 0);
            this.scrollImage.Name = "scrollImage";
            this.scrollImage.Size = new System.Drawing.Size(40, 40);
            this.scrollImage.TabIndex = 12;
            this.scrollImage.TabStop = false;
            // 
            // jumpEndBtn
            // 
            appearance5.Image = global::Lyra2.LyraShell.Properties.Resources.scroll_bottom;
            appearance5.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance5.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.jumpEndBtn.Appearance = appearance5;
            this.jumpEndBtn.Location = new System.Drawing.Point(12, 114);
            this.jumpEndBtn.Name = "jumpEndBtn";
            this.jumpEndBtn.Size = new System.Drawing.Size(32, 27);
            this.jumpEndBtn.TabIndex = 12;
            this.jumpEndBtn.Click += new System.EventHandler(this.scrollToEndBtn_Click);
            // 
            // scrollDownwardsBtn
            // 
            appearance4.Image = global::Lyra2.LyraShell.Properties.Resources.scroll_down;
            appearance4.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance4.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.scrollDownwardsBtn.Appearance = appearance4;
            this.scrollDownwardsBtn.AutoRepeat = true;
            this.scrollDownwardsBtn.Location = new System.Drawing.Point(12, 85);
            this.scrollDownwardsBtn.Name = "scrollDownwardsBtn";
            this.scrollDownwardsBtn.Size = new System.Drawing.Size(32, 27);
            this.scrollDownwardsBtn.TabIndex = 12;
            this.scrollDownwardsBtn.Click += new System.EventHandler(this.scrollDownBtn_Click);
            // 
            // scrollUpwardsBtn
            // 
            appearance3.Image = global::Lyra2.LyraShell.Properties.Resources.scroll_up;
            appearance3.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance3.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.scrollUpwardsBtn.Appearance = appearance3;
            this.scrollUpwardsBtn.AutoRepeat = true;
            this.scrollUpwardsBtn.Location = new System.Drawing.Point(12, 51);
            this.scrollUpwardsBtn.Name = "scrollUpwardsBtn";
            this.scrollUpwardsBtn.Size = new System.Drawing.Size(32, 27);
            this.scrollUpwardsBtn.TabIndex = 12;
            this.scrollUpwardsBtn.Click += new System.EventHandler(this.scrollUpBtn_Click);
            // 
            // jumpTopBtn
            // 
            appearance2.Image = global::Lyra2.LyraShell.Properties.Resources.scroll_top;
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance2.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.jumpTopBtn.Appearance = appearance2;
            this.jumpTopBtn.Location = new System.Drawing.Point(12, 22);
            this.jumpTopBtn.Name = "jumpTopBtn";
            this.jumpTopBtn.Size = new System.Drawing.Size(32, 27);
            this.jumpTopBtn.TabIndex = 12;
            this.jumpTopBtn.Click += new System.EventHandler(this.scrollToTopBtn_Click);
            // 
            // bottomPanel
            // 
            this.bottomPanel.Appearance.BackColor = Color.Transparent;
            this.scrollPane.BackColor = Color.Transparent;
            this.mainPane.BackColor = Color.Transparent;
            this.rightPane.BackColor = Color.Transparent;

            // 
            // bottomPanel.ClientArea
            // 
            this.bottomPanel.ClientArea.Controls.Add(this.titleLabel);
            this.bottomPanel.ClientArea.Controls.Add(this.infoLabel);
            this.bottomPanel.ClientArea.Controls.Add(this.sourceLabel);
            this.bottomPanel.ClientArea.Controls.Add(this.nextBtn);
            this.bottomPanel.ClientArea.Controls.Add(this.lastBtn);
            this.bottomPanel.ClientArea.Controls.Add(this.nextLabel);
            this.bottomPanel.ClientArea.Controls.Add(this.prevLabel);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.bottomPanel.Location = new System.Drawing.Point(0, 0);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(800, 43);
            this.bottomPanel.TabIndex = 9;
            // 
            // titleLabel
            // 
            this.titleLabel.Anchor =
              ((System.Windows.Forms.AnchorStyles)
               (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                 | System.Windows.Forms.AnchorStyles.Right)));
            appearance23.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance23.FontData.Name = "Verdana";
            appearance23.FontData.SizeInPoints = 14F;
            appearance23.FontData.Bold = DefaultableBoolean.True;
            appearance23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(85)))),
                                                                   ((int)(((byte)(170)))));
            appearance23.TextHAlignAsString = "Left";
            appearance23.TextTrimming = Infragistics.Win.TextTrimming.EllipsisWord;
            appearance23.TextVAlignAsString = "Middle";
            this.titleLabel.Appearance = appearance23;
            this.titleLabel.Location = new System.Drawing.Point(5, 45);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.AutoSize = true;
            this.titleLabel.TabIndex = 13;
            this.titleLabel.Text = "Titel";
            this.titleLabel.WrapText = false;
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                    (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
            infoAppearance.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            infoAppearance.FontData.Name = "Verdana";
            infoAppearance.FontData.SizeInPoints = 9;
            infoAppearance.ForeColor = Color.Gray;
            infoAppearance.TextHAlignAsString = "Left";
            infoAppearance.TextTrimming = Infragistics.Win.TextTrimming.EllipsisWord;
            infoAppearance.TextVAlignAsString = "Middle";
            this.infoLabel.Appearance = infoAppearance;
            this.infoLabel.Location = new System.Drawing.Point(7, 150);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.AutoSize = true;
            this.infoLabel.TabIndex = 13;
            this.infoLabel.Text = "Info";
            this.infoLabel.WrapText = false;
            // 
            // sourceLabel
            // 
            this.sourceLabel.BackColor = System.Drawing.Color.Transparent;
            this.sourceLabel.Font = new System.Drawing.Font("Verdana", 12, System.Drawing.FontStyle.Regular,
                                                        System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceLabel.ForeColor = System.Drawing.Color.White;
            this.sourceLabel.Location = new System.Drawing.Point(5, 15);
            this.sourceLabel.Name = "sourceLabel";
            this.sourceLabel.AutoSize = true;
            this.sourceLabel.TabIndex = 11;
            this.sourceLabel.Text = "0009";
            this.sourceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nextLabel
            // 
            this.nextLabel.BackColor = System.Drawing.Color.Transparent;
            this.nextLabel.Font = new System.Drawing.Font("Verdana", 9, System.Drawing.FontStyle.Regular,
                                                          System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextLabel.ForeColor = System.Drawing.Color.Black;
            this.nextLabel.Location = new System.Drawing.Point(75, 119);
            this.nextLabel.Name = "nextLabel";
            this.nextLabel.Size = new System.Drawing.Size(47, 24);
            this.nextLabel.AutoSize = true;
            this.nextLabel.TabIndex = 11;
            this.nextLabel.Text = "next";
            this.nextLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // prevLabel
            // 
            this.prevLabel.BackColor = System.Drawing.Color.Transparent;
            this.prevLabel.Font = new System.Drawing.Font("Verdana", 9, System.Drawing.FontStyle.Regular,
                                                          System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevLabel.ForeColor = System.Drawing.Color.Black;
            this.prevLabel.Location = new System.Drawing.Point(75, 84);
            this.prevLabel.Name = "prevLabel";
            this.prevLabel.Size = new System.Drawing.Size(47, 24);
            this.prevLabel.AutoSize = true;
            this.prevLabel.TabIndex = 11;
            this.prevLabel.Text = "prev";
            this.prevLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // line
            // 
            appearance22.BackColor = System.Drawing.Color.White;
            appearance22.AlphaLevel = 150;
            this.line.Appearance = appearance22;
            this.line.Dock = System.Windows.Forms.DockStyle.Left;
            this.line.Location = new System.Drawing.Point(0, 15);
            this.line.Name = "line";
            this.line.Size = new System.Drawing.Size(1, 80);
            this.line.TabIndex = 1;
            // 
            // nextBtn
            // 
            appearance7.Image = global::Lyra2.LyraShell.Properties.Resources.next;
            appearance7.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.nextBtn.Appearance = appearance7;
            this.nextBtn.Location = new System.Drawing.Point(22, 115);
            this.nextBtn.Name = "nextBtn";
            this.nextBtn.Size = new System.Drawing.Size(47, 24);
            this.nextBtn.TabIndex = 12;
            this.nextBtn.Click += new System.EventHandler(this.nextSongBtn_Click);
            // 
            // lastBtn
            // 
            appearance6.Image = global::Lyra2.LyraShell.Properties.Resources.prev;
            appearance6.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.lastBtn.Appearance = appearance6;
            this.lastBtn.Location = new System.Drawing.Point(22, 80);
            this.lastBtn.Name = "lastBtn";
            this.lastBtn.Size = new System.Drawing.Size(47, 24);
            this.lastBtn.TabIndex = 12;
            this.lastBtn.Click += new System.EventHandler(this.previousSongBtn_Click);
            // 
            // RemoteControl
            // 
            this.ClientSize = new System.Drawing.Size(357, 174);
            // this.Controls.Add(this.remotePanel);
            // this.MinimumSize = new System.Drawing.Size(128, 208);
            this.Name = "RemoteControl";
            this.Text = "Fernsteuerung";
            this.remotePanel.ClientArea.ResumeLayout(false);
            this.remotePanel.ResumeLayout(false);
            this.mainPane.ResumeLayout(false);
            this.mainPane.PerformLayout();
            this.rightPane.ResumeLayout(false);
            this.rightPane.PerformLayout();
            this.lineContainer.ResumeLayout(false);
            this.lineContainer.PerformLayout();
            this.scrollPane.ResumeLayout(false);
            this.scrollBox.ClientArea.ResumeLayout(false);
            this.scrollBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scrollImage)).EndInit();
            this.bottomPanel.ClientArea.ResumeLayout(false);
            this.bottomPanel.ResumeLayout(false);
            this.line.ResumeLayout(false);
            this.ResumeLayout(false);

            this.timer.Start();
        }

        #endregion

        private void scrollUpBtn_Click(object sender, EventArgs e)
        {
            View.ExecuteActionOnView(View.ViewActions.ScrollUp);
        }

        private void nextSongBtn_Click(object sender, EventArgs e)
        {
            View.ExecuteActionOnView(View.ViewActions.NextSong);
        }

        private void previousSongBtn_Click(object sender, EventArgs e)
        {
            View.ExecuteActionOnView(View.ViewActions.PreviewsSong);
        }

        private void scrollDownBtn_Click(object sender, EventArgs e)
        {
            View.ExecuteActionOnView(View.ViewActions.ScrollDown);
        }

        private void scrollToTopBtn_Click(object sender, EventArgs e)
        {
            View.ExecuteActionOnView(View.ViewActions.ScrollToTop);
        }

        private void scrollToEndBtn_Click(object sender, EventArgs e)
        {
            View.ExecuteActionOnView(View.ViewActions.ScrollToEnd);
        }

        private void JumpMarksDoubleClickHandler(object sender, EventArgs e)
        {
            #region    Precondition

            if (this.jumpMarksListBox.SelectedItem == null) return;

            #endregion Precondition

            var jumpMark = (JumpMark)this.jumpMarksListBox.SelectedItem;
            View.ScrollToPosition(jumpMark.Position);
        }
    }
}