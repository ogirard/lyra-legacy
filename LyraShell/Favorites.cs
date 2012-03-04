using System;
using System.ComponentModel;
using System.Windows.Forms;
using Lyra2.UtilShared;

namespace Lyra2.LyraShell
{
    /// <summary>
    /// Summary description for Favorites.
    /// </summary>
    public class Favorites : Form
    {
        private SongListBox favoritesListBox;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components;
        private readonly GUI owner;
        private SplitContainer favoritesSplitter;
        private SongPreview favoritesSongPreview;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor favoritesFilterTB;
        private Label label1;

        private static Favorites _this;

        public static void ShowFavorites(GUI owner)
        {
            if (_this == null)
            {
                _this = new Favorites(owner);
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

        private Favorites(GUI owner)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Closing += Favorites_Closing;
            this.owner = owner;
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
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.favoritesListBox = new Lyra2.LyraShell.SongListBox();
            this.favoritesSplitter = new System.Windows.Forms.SplitContainer();
            this.favoritesSongPreview = new Lyra2.LyraShell.SongPreview();
            this.favoritesFilterTB = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.label1 = new System.Windows.Forms.Label();
            this.favoritesSplitter.Panel1.SuspendLayout();
            this.favoritesSplitter.Panel2.SuspendLayout();
            this.favoritesSplitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.favoritesFilterTB)).BeginInit();
            this.SuspendLayout();
            // 
            // favoritesListBox
            // 
            this.favoritesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.favoritesListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.favoritesListBox.ItemHeight = 15;
            this.favoritesListBox.Location = new System.Drawing.Point(0, 26);
            this.favoritesListBox.Name = "favoritesListBox";
            this.favoritesListBox.Size = new System.Drawing.Size(593, 394);
            this.favoritesListBox.TabIndex = 6;
            this.favoritesListBox.SelectedIndexChanged += new System.EventHandler(this.favoritesListBox_SelectedIndexChanged);
            this.favoritesListBox.DoubleClick += new System.EventHandler(this.listBox3_DoubleClick);
            this.favoritesListBox.SelectedValueChanged += new System.EventHandler(this.listBox3_SelectedValueChanged);
            // 
            // favoritesSplitter
            // 
            this.favoritesSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.favoritesSplitter.Location = new System.Drawing.Point(0, 0);
            this.favoritesSplitter.Name = "favoritesSplitter";
            this.favoritesSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // favoritesSplitter.Panel1
            // 
            this.favoritesSplitter.Panel1.Controls.Add(this.label1);
            this.favoritesSplitter.Panel1.Controls.Add(this.favoritesFilterTB);
            this.favoritesSplitter.Panel1.Controls.Add(this.favoritesListBox);
            // 
            // favoritesSplitter.Panel2
            // 
            this.favoritesSplitter.Panel2.Controls.Add(this.favoritesSongPreview);
            this.favoritesSplitter.Size = new System.Drawing.Size(593, 673);
            this.favoritesSplitter.SplitterDistance = 424;
            this.favoritesSplitter.TabIndex = 7;
            // 
            // favoritesSongPreview
            // 
            this.favoritesSongPreview.AutoScroll = true;
            this.favoritesSongPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.favoritesSongPreview.Location = new System.Drawing.Point(0, 0);
            this.favoritesSongPreview.Name = "favoritesSongPreview";
            this.favoritesSongPreview.Size = new System.Drawing.Size(593, 245);
            this.favoritesSongPreview.TabIndex = 1;
            // 
            // favoritesFilterTB
            // 
            this.favoritesFilterTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(250)))), ((int)(((byte)(215)))));
            this.favoritesFilterTB.Appearance = appearance4;
            this.favoritesFilterTB.AutoSize = false;
            this.favoritesFilterTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(250)))), ((int)(((byte)(215)))));
            this.favoritesFilterTB.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance2.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance2.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance2.ForegroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance2.ImageAlpha = Infragistics.Win.Alpha.Transparent;
            appearance2.ImageBackground = global::Lyra2.LyraShell.Properties.Resources.clear_normal_16;
            appearance2.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            editorButton1.Appearance = appearance2;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            appearance3.ImageBackground = global::Lyra2.LyraShell.Properties.Resources.clear_pressed_16;
            editorButton1.PressedAppearance = appearance3;
            editorButton1.Text = "";
            editorButton1.Width = 16;
            this.favoritesFilterTB.ButtonsRight.Add(editorButton1);
            this.favoritesFilterTB.Location = new System.Drawing.Point(380, 4);
            this.favoritesFilterTB.Name = "favoritesFilterTB";
            this.favoritesFilterTB.NullText = "Favoriten Filter";
            appearance1.FontData.ItalicAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.Gray;
            this.favoritesFilterTB.NullTextAppearance = appearance1;
            this.favoritesFilterTB.Size = new System.Drawing.Size(209, 19);
            this.favoritesFilterTB.TabIndex = 7;
            this.favoritesFilterTB.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SlateGray;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Lyra Favoriten";
            // 
            // Favorites
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(593, 673);
            this.Controls.Add(this.favoritesSplitter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Favorites";
            this.ShowInTaskbar = false;
            this.Text = "Favorites";
            this.favoritesSplitter.Panel1.ResumeLayout(false);
            this.favoritesSplitter.Panel2.ResumeLayout(false);
            this.favoritesSplitter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.favoritesFilterTB)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void Favorites_Closing(object sender, CancelEventArgs e)
        {
            StorePersonalizationSettings(owner.Personalizer, false);
            _this = null;
        }

        
        private void listBox3_DoubleClick(object sender, EventArgs e)
        {
            if (this.favoritesListBox.SelectedItem is Song)
            {
                View.ShowSong((Song)this.favoritesListBox.SelectedItem, this.owner, this.favoritesListBox);
            }
        }

        private void listBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            ISong s = this.favoritesListBox.SelectedItem as ISong;
            if (s != null)
            {
                this.favoritesSongPreview.ShowSong(s);
                this.favoritesListBox.Focus();
            }
            else
            {
                this.favoritesSongPreview.Reset();
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

        private void favoritesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.favoritesListBox.SelectedIndex < 0)
            {
                this.favoritesSongPreview.Reset();
            }
        }

        public static void StorePersonalizationSettings(Personalizer personalizer, bool shown)
        {
            if (_this != null)
            {
                personalizer[PersonalizationItemNames.FavoritesTop] = _this.Top.ToString();
                personalizer[PersonalizationItemNames.FavoritesLeft] = _this.Left.ToString();
                personalizer[PersonalizationItemNames.FavoritesWidth] = _this.Width.ToString();
                personalizer[PersonalizationItemNames.FavoritesHeight] = _this.Height.ToString();
                personalizer[PersonalizationItemNames.FavoritesSplit] = _this.favoritesSplitter.SplitterDistance.ToString();
                personalizer[PersonalizationItemNames.FavoritesIsShown] = shown ? "1" : "0";
                personalizer.Write();
            }
        }

        private static void LoadPersonalizationSettings(Personalizer personalizer)
        {
            if (_this != null)
            {
                personalizer.Load();
                int top = personalizer.GetIntValue(PersonalizationItemNames.FavoritesTop);
                if (top > 0) _this.Top = top;
                int left = personalizer.GetIntValue(PersonalizationItemNames.FavoritesLeft);
                if (left > 0) _this.Left = left;
                int width = personalizer.GetIntValue(PersonalizationItemNames.FavoritesWidth);
                if (width > 0) _this.Width = width;
                int height = personalizer.GetIntValue(PersonalizationItemNames.FavoritesHeight);
                if (height > 0) _this.Height = height;
                int split = personalizer.GetIntValue(PersonalizationItemNames.FavoritesSplit);
                if (split > 0) _this.favoritesSplitter.SplitterDistance = split;

                personalizer[PersonalizationItemNames.FavoritesIsShown] = "1";
                personalizer.Write();
            }
        }
    }
}
