using System.ComponentModel;
using System.Windows.Forms;
using Infragistics.Win.Misc;

namespace Lyra2.LyraShell
{
	/// <summary>
	/// Summary description for SongPreview.
	/// </summary>
	public class SongPreview : UserControl
	{
		private Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label titlePreview;
		private System.Windows.Forms.Label nrPreview;
		private System.Windows.Forms.Label textPreview;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private Container components;

		public SongPreview()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			this.Reset();
		}
		
		public void Reset()
		{
			this.nrPreview.Text = "?";
			this.titlePreview.Text = "Kein Lied ausgewählt!";
			this.textPreview.Text = "";
		}
		
		public void ShowSong(ISong song)
		{
			this.nrPreview.Text = song.Number.ToString();
			this.titlePreview.Text = song.Title;
			this.textPreview.Text = Util.CleanText(song.Text);
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
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
            this.textPreview = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.titlePreview = new System.Windows.Forms.Label();
            this.nrPreview = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textPreview);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1024, 400);
            this.panel2.TabIndex = 8;
            // 
            // textPreview
            // 
            this.textPreview.BackColor = System.Drawing.Color.White;
            this.textPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textPreview.Location = new System.Drawing.Point(0, 32);
            this.textPreview.Name = "textPreview";
            this.textPreview.Size = new System.Drawing.Size(1024, 368);
            this.textPreview.TabIndex = 10;
            this.textPreview.Text = "label1";
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
