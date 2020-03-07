using Lyra2.UtilShared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
    /// <summary>
    /// Summary description for SongPreview.
    /// </summary>
    public class SongPreview : UserControl
    {
        private Panel panel2;
        private Panel panel3;
        private Panel panel1;
        private Label titlePreview;
        private Label nrPreview;
        private Panel flowPanel;
        private ISong song;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
#pragma warning disable 649
        private Container components;
#pragma warning restore 649

        public SongPreview()
        {
            // This call is required by the Windows.Forms Form Designer.
            this.InitializeComponent();
            this.Reset();
        }

        public void Reset()
        {
            this.nrPreview.Text = "?";
            this.titlePreview.Text = "Kein Lied ausgewählt!";
            this.flowPanel.Controls.Clear();
        }

        public void ShowSong(ISong song)
        {
            if (song == null)
            {
                return;
            }

            this.song = song;
            this.nrPreview.Text = song.Number.ToString();
            this.titlePreview.Text = song.Title;

            var text = Util.CleanText(song.Text).TrimEnd('\n');
            this.flowPanel.Controls.Clear();
            var height = this.flowPanel.Height - this.flowPanel.Padding.Vertical;
            var width = this.flowPanel.Width - this.flowPanel.Padding.Horizontal;

            var box = GraphicUtils.MeasureString(text, this.flowPanel.Font);
            var columnWidth = box.Width + 16;
            var columnCount = Math.Min((int)Math.Ceiling(box.Height / (decimal)height), (int)Math.Floor((decimal)width / columnWidth));
            var lines = text.Split(new[] { '\n' }, StringSplitOptions.None);
            var batchSize = (int)Math.Floor(lines.Length / (decimal)columnCount);

            var controls = new List<Control>();
            var columnHeight = 0;
            for (var i = 0; i < columnCount; i++)
            {
                var label = new Label
                {
                    Name = $"flowlabel{i}",
                    Text = string.Join(Environment.NewLine, lines.Skip(batchSize * i).Take(batchSize)),
                    Dock = DockStyle.Left,
                    Width = columnWidth
                };

                var labelHeight = GraphicUtils.MeasureString(label.Text, this.flowPanel.Font).Height;
                label.Height = labelHeight + 8;
                columnHeight = Math.Max(columnHeight, label.Height);

                if (i != 0)
                {
                    controls.Insert(0, new Panel { Dock = DockStyle.Left, Width = 1, BackColor = Color.FromArgb(255, 238, 238, 238) });
                    label.Padding = new Padding(4);
                }
                else
                {
                    label.Padding = new Padding(0, 4, 4, 4);
                }

                controls.Insert(0, label);
            }

            this.flowPanel.Controls.AddRange(controls.ToArray());
            this.flowPanel.AutoScrollMinSize = new Size(0, columnHeight);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ShowSong(song);
        }

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SongPreview));
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.titlePreview = new System.Windows.Forms.Label();
            this.nrPreview = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.flowPanel = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.flowPanel);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1024, 400);
            this.panel2.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.titlePreview);
            this.panel1.Controls.Add(this.nrPreview);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1024, 24);
            this.panel1.TabIndex = 9;
            // 
            // titlePreview
            // 
            this.titlePreview.BackColor = System.Drawing.Color.White;
            this.titlePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titlePreview.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.titlePreview.Location = new System.Drawing.Point(59, 0);
            this.titlePreview.Name = "titlePreview";
            this.titlePreview.Size = new System.Drawing.Size(965, 24);
            this.titlePreview.TabIndex = 5;
            this.titlePreview.Text = "Testtitel";
            // 
            // nrPreview
            // 
            this.nrPreview.BackColor = System.Drawing.Color.White;
            this.nrPreview.Dock = System.Windows.Forms.DockStyle.Left;
            this.nrPreview.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nrPreview.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.nrPreview.Location = new System.Drawing.Point(0, 0);
            this.nrPreview.Name = "nrPreview";
            this.nrPreview.Size = new System.Drawing.Size(59, 24);
            this.nrPreview.TabIndex = 3;
            this.nrPreview.Text = "9999";
            // 
            // panel3
            // 
            this.panel3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel3.BackgroundImage")));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1024, 8);
            this.panel3.TabIndex = 8;
            // 
            // flowPanel
            // 
            this.flowPanel.AutoScroll = true;
            this.flowPanel.BackColor = System.Drawing.Color.White;
            this.flowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPanel.Location = new System.Drawing.Point(0, 32);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Padding = new System.Windows.Forms.Padding(4);
            this.flowPanel.Size = new System.Drawing.Size(1024, 368);
            this.flowPanel.TabIndex = 10;
            // 
            // SongPreview
            // 
            this.AutoScroll = true;
            this.Controls.Add(this.panel2);
            this.Name = "SongPreview";
            this.Size = new System.Drawing.Size(1024, 400);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
    }
}
