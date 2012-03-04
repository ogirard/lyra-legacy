using System;
using Lyra2.UtilShared;


namespace Lyra2.LyraShell
{
	/// <summary>
	/// Zusammendfassende Beschreibung für first.
	/// </summary>
	public class Frst : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private LyraButtonControl button1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RichTextBox richTextBox1;

		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Frst()
		{
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();
			this.AcceptButton = this.button1;
			try
			{
				// StreamReader reader = new StreamReader(Util.NEWSURL);
				// this.textBox1.Text = reader.ReadToEnd();
				this.richTextBox1.LoadFile(Util.BASEURL + "\\" + Util.INFORTF);
			}
			catch (System.IO.IOException ioe)
			{
				Util.MBoxError(ioe.Message, ioe);
			}
		}

		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
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
			Frst.info = null;
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Frst));
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new LyraButtonControl();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 120);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 88);
			this.label1.TabIndex = 1;
			this.label1.Text = "label1";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(581, 189);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(72, 24);
			this.button1.TabIndex = 2;
			this.button1.Text = "schliessen";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(104, 104);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// checkBox1
			// 
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.checkBox1.Location = new System.Drawing.Point(432, 192);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(120, 16);
			this.checkBox1.TabIndex = 4;
			this.checkBox1.Text = "weiterhin anzeigen";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.SlateGray;
			this.label2.Location = new System.Drawing.Point(128, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(200, 24);
			this.label2.TabIndex = 5;
			this.label2.Text = "buildnews";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(136, 192);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(192, 16);
			this.label3.TabIndex = 7;
			this.label3.Text = "Hilfedatei zeigen Sie mit F1 an!";
			// 
			// richTextBox1
			// 
			this.richTextBox1.BackColor = System.Drawing.SystemColors.Info;
			this.richTextBox1.HideSelection = false;
			this.richTextBox1.Location = new System.Drawing.Point(136, 32);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(512, 152);
			this.richTextBox1.TabIndex = 8;
			this.richTextBox1.Text = "";
			// 
			// Frst
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(656, 216);
			this.ControlBox = false;
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Frst";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.TopMost = true;
			this.Load += new System.EventHandler(this.Frst_Load);
			this.LostFocus += new System.EventHandler(this.Frst_LostFocus);
			this.ResumeLayout(false);

		}

		#endregion

		private void Frst_Load(object sender, System.EventArgs e)
		{
			this.label1.Text = "lyra v." + Util.VER + Util.NL;
			this.label1.Text += "underlies GNU Public Licence";
			this.checkBox1.Select();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			if (!this.checkBox1.Checked && change)
			{
				try
				{
					Util.SHOWBUILDNEWS = false;
					ConfigFile configFile = new ConfigFile(Util.CONFIGPATH);
					configFile["1"] = "no";
					configFile.Save(Util.CONFIGPATH);
				}
				catch (Exception ex)
				{
					Util.MBoxError(ex.Message, ex);
				}
			}
			Frst.shown = false;
			this.Close();
		}

		private void Frst_LostFocus(object sender, System.EventArgs e)
		{
			this.Focus();
		}

		private static Frst info = null;
		private bool change = true;
		private static bool shown = false;

		public static void ShowInfo()
		{
			if (Frst.shown)
			{
				Frst.info.Focus();
			}
			else
			{
				if (Frst.info == null)
				{
					Frst.info = new Frst();
				}
				Frst.info.checkBox1.Visible = false;
				Frst.info.change = false;
				Frst.info.Show();
				Frst.shown = true;
			}
		}
	}
}