using Infragistics.Win.Misc;
using Lyra2.UtilShared;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Resources = Lyra2.LyraShell.Properties.Resources;

namespace Lyra2.LyraShell
{
    /// <summary>
    ///   Summary description for RemoteControl.
    /// </summary>
    public class RemoteControl : Form
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
        private UltraPanel line;
        private LyraButtonControl nextBtn;
        private Label nrLabel;
        private Label prevLabel;
        private Label nextLabel;
        private Panel mainPane;
        private Label jumperLabel;
        private Panel scrollPane;
        private LyraButtonControl scrollUpwardsBtn;
        private LyraButtonControl jumpTopBtn;
        private LyraButtonControl jumpEndBtn;
        private LyraButtonControl scrollDownwardsBtn;
        private UltraPanel scrollBox;
        private UltraLabel titleLabel;
        private PictureBox scrollImage;
        private ScrollVisualPanel scrollVisual;

        private static RemoteControl _this;

        public static void ShowRemoteControl(GUI owner)
        {
            if (_this == null)
            {
                _this = new RemoteControl(owner);
                LoadPersonalizationSettings(owner.Personalizer);
            }
            _this.Show();
            _this.Focus();
        }

        public static void ForceFocus()
        {
            if (_this != null)
            {
                _this.Focus();
            }
        }

        private readonly BackgroundWorker bgw = new BackgroundWorker();

        private RemoteControl(GUI owner)
        {
            //
            // Required for Windows Form Designer support
            //
            this.InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.owner = owner;
            this.Closing += this.RemoteControl_Closing;
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
                this.titleLabel.Text = "";
                this.nrLabel.Text = "n/a";
                this.nextBtn.Enabled = false;
                this.lastBtn.Enabled = false;
                this.nextLabel.Text = "";
                this.prevLabel.Text = "";
                this.jumpMarksListBox.Items.Clear();
                this.scrollVisual.UpdateScrollData(null);
                return;
            }

            this.titleLabel.Text = songInfo.DisplayedSong != null ? songInfo.DisplayedSong.Title : "";
            this.nrLabel.Text = songInfo.DisplayedSong != null ? songInfo.DisplayedSong.Number.ToString().PadLeft(4, '0') : "n/a";
            this.nextBtn.Enabled = songInfo.NextSong != null;
            this.lastBtn.Enabled = songInfo.PreviousSong != null;
            this.nextLabel.Text = songInfo.NextSong != null ? songInfo.NextSong.Number.ToString().PadLeft(4, '0') : "";
            this.prevLabel.Text = songInfo.PreviousSong != null ? songInfo.PreviousSong.Number.ToString().PadLeft(4, '0') : "";
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
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.remotePanel = new Infragistics.Win.Misc.UltraPanel();
            this.mainPane = new System.Windows.Forms.Panel();
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
            this.nrLabel = new System.Windows.Forms.Label();
            this.nextLabel = new System.Windows.Forms.Label();
            this.prevLabel = new System.Windows.Forms.Label();
            this.line = new Infragistics.Win.Misc.UltraPanel();
            this.nextBtn = new Lyra2.LyraShell.LyraButtonControl();
            this.lastBtn = new Lyra2.LyraShell.LyraButtonControl();
            this.remotePanel.ClientArea.SuspendLayout();
            this.remotePanel.SuspendLayout();
            this.mainPane.SuspendLayout();
            this.scrollPane.SuspendLayout();
            this.scrollBox.ClientArea.SuspendLayout();
            this.scrollBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scrollImage)).BeginInit();
            this.bottomPanel.ClientArea.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.line.SuspendLayout();
            this.SuspendLayout();
            // 
            // remotePanel
            // 
            // 
            // remotePanel.ClientArea
            // 
            this.remotePanel.ClientArea.Controls.Add(this.mainPane);
            this.remotePanel.ClientArea.Controls.Add(this.scrollPane);
            this.remotePanel.ClientArea.Controls.Add(this.bottomPanel);
            this.remotePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.remotePanel.Location = new System.Drawing.Point(0, 0);
            this.remotePanel.Name = "remotePanel";
            this.remotePanel.Size = new System.Drawing.Size(357, 174);
            this.remotePanel.TabIndex = 11;
            // 
            // mainPane
            // 
            this.mainPane.Controls.Add(this.jumpMarksListBox);
            this.mainPane.Controls.Add(this.jumperLabel);
            this.mainPane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPane.Location = new System.Drawing.Point(129, 43);
            this.mainPane.Name = "mainPane";
            this.mainPane.Size = new System.Drawing.Size(228, 131);
            this.mainPane.TabIndex = 12;
            // 
            // jumpMarksListBox
            // 
            this.jumpMarksListBox.Anchor =
              ((System.Windows.Forms.AnchorStyles)
               ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                 | System.Windows.Forms.AnchorStyles.Right)));
            this.jumpMarksListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))),
                                                                            ((int)(((byte)(176)))));
            this.jumpMarksListBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular,
                                                                 System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jumpMarksListBox.FormattingEnabled = true;
            this.jumpMarksListBox.ItemHeight = 16;
            this.jumpMarksListBox.Items.AddRange(new object[]
                                                   {
                                               "Test 1"
                                                   });
            this.jumpMarksListBox.Location = new System.Drawing.Point(9, 25);
            this.jumpMarksListBox.Name = "jumpMarksListBox";
            this.jumpMarksListBox.Size = new System.Drawing.Size(216, 100);
            this.jumpMarksListBox.TabIndex = 0;
            this.jumpMarksListBox.Click += new System.EventHandler(this.JumpMarksDoubleClickHandler);
            this.jumpMarksListBox.DoubleClick += new System.EventHandler(this.JumpMarksDoubleClickHandler);
            // 
            // jumperLabel
            // 
            this.jumperLabel.AutoSize = true;
            this.jumperLabel.BackColor = System.Drawing.Color.Transparent;
            this.jumperLabel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold,
                                                            System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jumperLabel.ForeColor = System.Drawing.Color.DimGray;
            this.jumperLabel.Location = new System.Drawing.Point(6, 3);
            this.jumperLabel.Name = "jumperLabel";
            this.jumperLabel.Size = new System.Drawing.Size(103, 13);
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
            this.scrollPane.Location = new System.Drawing.Point(0, 43);
            this.scrollPane.Name = "scrollPane";
            this.scrollPane.Size = new System.Drawing.Size(129, 131);
            this.scrollPane.TabIndex = 11;
            // 
            // scrollVisual
            // 
            this.scrollVisual.Location = new System.Drawing.Point(113, 5);
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
            this.scrollBox.Location = new System.Drawing.Point(43, 33);
            this.scrollBox.Name = "scrollBox";
            this.scrollBox.Size = new System.Drawing.Size(64, 64);
            this.scrollBox.TabIndex = 13;
            // 
            // scrollImage
            // 
            this.scrollImage.Location = new System.Drawing.Point(6, 6);
            this.scrollImage.Name = "scrollImage";
            this.scrollImage.Size = new System.Drawing.Size(48, 48);
            this.scrollImage.TabIndex = 12;
            this.scrollImage.TabStop = false;
            // 
            // jumpEndBtn
            // 
            appearance5.Image = global::Lyra2.LyraShell.Properties.Resources.scroll_bottom;
            appearance5.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance5.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.jumpEndBtn.Appearance = appearance5;
            this.jumpEndBtn.Location = new System.Drawing.Point(6, 97);
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
            this.scrollDownwardsBtn.Location = new System.Drawing.Point(6, 68);
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
            this.scrollUpwardsBtn.Location = new System.Drawing.Point(6, 34);
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
            this.jumpTopBtn.Location = new System.Drawing.Point(6, 5);
            this.jumpTopBtn.Name = "jumpTopBtn";
            this.jumpTopBtn.Size = new System.Drawing.Size(32, 27);
            this.jumpTopBtn.TabIndex = 12;
            this.jumpTopBtn.Click += new System.EventHandler(this.scrollToTopBtn_Click);
            // 
            // bottomPanel
            // 
            appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))),
                                                                   ((int)(((byte)(224)))));
            appearance21.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))),
                                                                    ((int)(((byte)(241)))));
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.GlassTop50;
            this.bottomPanel.Appearance = appearance21;
            // 
            // bottomPanel.ClientArea
            // 
            this.bottomPanel.ClientArea.Controls.Add(this.titleLabel);
            this.bottomPanel.ClientArea.Controls.Add(this.nrLabel);
            this.bottomPanel.ClientArea.Controls.Add(this.nextLabel);
            this.bottomPanel.ClientArea.Controls.Add(this.prevLabel);
            this.bottomPanel.ClientArea.Controls.Add(this.line);
            this.bottomPanel.ClientArea.Controls.Add(this.nextBtn);
            this.bottomPanel.ClientArea.Controls.Add(this.lastBtn);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.bottomPanel.Location = new System.Drawing.Point(0, 0);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(357, 43);
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
            appearance23.FontData.SizeInPoints = 9F;
            appearance23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(85)))),
                                                                   ((int)(((byte)(170)))));
            appearance23.TextHAlignAsString = "Left";
            appearance23.TextTrimming = Infragistics.Win.TextTrimming.EllipsisWord;
            appearance23.TextVAlignAsString = "Middle";
            this.titleLabel.Appearance = appearance23;
            this.titleLabel.Location = new System.Drawing.Point(5, 18);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(202, 23);
            this.titleLabel.TabIndex = 13;
            this.titleLabel.Text = "Titel";
            this.titleLabel.WrapText = false;
            // 
            // nrLabel
            // 
            this.nrLabel.BackColor = System.Drawing.Color.Transparent;
            this.nrLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular,
                                                        System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nrLabel.ForeColor = System.Drawing.Color.Black;
            this.nrLabel.Location = new System.Drawing.Point(5, 0);
            this.nrLabel.Name = "nrLabel";
            this.nrLabel.Size = new System.Drawing.Size(39, 23);
            this.nrLabel.TabIndex = 11;
            this.nrLabel.Text = "0009";
            this.nrLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nextLabel
            // 
            this.nextLabel.Anchor =
              ((System.Windows.Forms.AnchorStyles)
               ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nextLabel.BackColor = System.Drawing.Color.Transparent;
            this.nextLabel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular,
                                                          System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextLabel.ForeColor = System.Drawing.Color.DimGray;
            this.nextLabel.Location = new System.Drawing.Point(307, 1);
            this.nextLabel.Name = "nextLabel";
            this.nextLabel.Size = new System.Drawing.Size(47, 14);
            this.nextLabel.TabIndex = 11;
            this.nextLabel.Text = "next";
            this.nextLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // prevLabel
            // 
            this.prevLabel.Anchor =
              ((System.Windows.Forms.AnchorStyles)
               ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.prevLabel.BackColor = System.Drawing.Color.Transparent;
            this.prevLabel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular,
                                                          System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevLabel.ForeColor = System.Drawing.Color.DimGray;
            this.prevLabel.Location = new System.Drawing.Point(258, 2);
            this.prevLabel.Name = "prevLabel";
            this.prevLabel.Size = new System.Drawing.Size(47, 13);
            this.prevLabel.TabIndex = 11;
            this.prevLabel.Text = "prev";
            this.prevLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // line
            // 
            appearance22.BackColor = System.Drawing.Color.DimGray;
            this.line.Appearance = appearance22;
            this.line.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.line.Location = new System.Drawing.Point(0, 42);
            this.line.Name = "line";
            this.line.Size = new System.Drawing.Size(357, 1);
            this.line.TabIndex = 1;
            // 
            // nextBtn
            // 
            this.nextBtn.Anchor =
              ((System.Windows.Forms.AnchorStyles)
               ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance7.Image = global::Lyra2.LyraShell.Properties.Resources.next;
            appearance7.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.nextBtn.Appearance = appearance7;
            this.nextBtn.Location = new System.Drawing.Point(307, 17);
            this.nextBtn.Name = "nextBtn";
            this.nextBtn.Size = new System.Drawing.Size(47, 24);
            this.nextBtn.TabIndex = 12;
            this.nextBtn.Click += new System.EventHandler(this.nextSongBtn_Click);
            // 
            // lastBtn
            // 
            this.lastBtn.Anchor =
              ((System.Windows.Forms.AnchorStyles)
               ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance6.Image = global::Lyra2.LyraShell.Properties.Resources.prev;
            appearance6.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.lastBtn.Appearance = appearance6;
            this.lastBtn.Location = new System.Drawing.Point(258, 17);
            this.lastBtn.Name = "lastBtn";
            this.lastBtn.Size = new System.Drawing.Size(47, 24);
            this.lastBtn.TabIndex = 12;
            this.lastBtn.Click += new System.EventHandler(this.previousSongBtn_Click);
            // 
            // RemoteControl
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(357, 174);
            this.Controls.Add(this.remotePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(128, 208);
            this.Name = "RemoteControl";
            this.ShowInTaskbar = false;
            this.Text = "Fernsteuerung";
            this.TopMost = true;
            this.remotePanel.ClientArea.ResumeLayout(false);
            this.remotePanel.ResumeLayout(false);
            this.mainPane.ResumeLayout(false);
            this.mainPane.PerformLayout();
            this.scrollPane.ResumeLayout(false);
            this.scrollBox.ClientArea.ResumeLayout(false);
            this.scrollBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scrollImage)).EndInit();
            this.bottomPanel.ClientArea.ResumeLayout(false);
            this.bottomPanel.ResumeLayout(false);
            this.line.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private void RemoteControl_Closing(object sender, CancelEventArgs e)
        {
            StorePersonalizationSettings(this.owner.Personalizer, false);
            _this = null;
        }

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

        //		private void scrollPageUpBtn_Click(object sender, System.EventArgs e)
        //		{
        //			View.ExecuteActionOnView(View.ViewActions.ScrollPageUp);
        //		}
        //
        //		private void scrollPageDownBtn_Click(object sender, System.EventArgs e)
        //		{
        //			View.ExecuteActionOnView(View.ViewActions.ScrollPageDown);
        //		}

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


        public static void StorePersonalizationSettings(Personalizer personalizer, bool shown)
        {
            if (_this != null)
            {
                personalizer[PersonalizationItemNames.RemoteTop] = _this.Top.ToString();
                personalizer[PersonalizationItemNames.RemoteLeft] = _this.Left.ToString();
                personalizer[PersonalizationItemNames.RemoteWidth] = _this.Width.ToString();
                personalizer[PersonalizationItemNames.RemoteHeight] = _this.Height.ToString();
                personalizer[PersonalizationItemNames.RemoteIsShown] = shown ? "1" : "0";
                personalizer.Write();
            }
        }

        private static void LoadPersonalizationSettings(Personalizer personalizer)
        {
            if (_this != null)
            {
                personalizer.Load();
                var top = personalizer.GetIntValue(PersonalizationItemNames.RemoteTop);
                if (top > 0) _this.Top = top;
                var left = personalizer.GetIntValue(PersonalizationItemNames.RemoteLeft);
                if (left > 0) _this.Left = left;
                var width = personalizer.GetIntValue(PersonalizationItemNames.RemoteWidth);
                if (width > 0) _this.Width = width;
                var height = personalizer.GetIntValue(PersonalizationItemNames.RemoteHeight);
                if (height > 0) _this.Height = height;

                personalizer[PersonalizationItemNames.RemoteIsShown] = "1";
                personalizer.Write();
            }
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