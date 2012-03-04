using System;
using System.Windows.Forms;
using Lyra2.UtilShared;


namespace Lyra2.LyraShell
{
	/// <summary>
	/// Zusammendfassende Beschreibung für pswd.
	/// </summary>
	public class Pswd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private LyraButtonControl button1;

		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private LyraButtonControl button2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.Label label3;
		private LyraButtonControl button3;
		private static Pswd password = null;


		private Pswd()
		{
			InitializeComponent();
			this.Height = 104;
			this.AcceptButton = this.button1;
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
			Pswd.password = null;
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button1 = new LyraButtonControl();
			this.button2 = new LyraButtonControl();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.button3 = new LyraButtonControl();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.Color.SaddleBrown;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(320, 32);
			this.label1.TabIndex = 0;
			this.label1.Text = "Zum Ändern dieser Option müssen Sie Berechtigung haben!  Geben Sie bitte das Admi" +
				"nistratorpasswort ein.";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(16, 48);
			this.textBox1.Name = "textBox1";
			this.textBox1.PasswordChar = '*';
			this.textBox1.Size = new System.Drawing.Size(80, 20);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(280, 48);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(56, 24);
			this.button1.TabIndex = 1;
			this.button1.Text = "Ok";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(200, 48);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(72, 24);
			this.button2.TabIndex = 4;
			this.button2.Text = "Abbrechen";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 83);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(160, 16);
			this.label2.TabIndex = 6;
			this.label2.Text = "neues Passwort:";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(104, 80);
			this.textBox2.Name = "textBox2";
			this.textBox2.PasswordChar = '*';
			this.textBox2.Size = new System.Drawing.Size(80, 20);
			this.textBox2.TabIndex = 1;
			this.textBox2.Text = "";
			// 
			// linkLabel1
			// 
			this.linkLabel1.ActiveLinkColor = System.Drawing.Color.SaddleBrown;
			this.linkLabel1.LinkColor = System.Drawing.Color.Navy;
			this.linkLabel1.Location = new System.Drawing.Point(104, 56);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(64, 16);
			this.linkLabel1.TabIndex = 8;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "ändern";
			this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Navy;
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(104, 107);
			this.textBox3.Name = "textBox3";
			this.textBox3.PasswordChar = '*';
			this.textBox3.Size = new System.Drawing.Size(80, 20);
			this.textBox3.TabIndex = 2;
			this.textBox3.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 112);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 16);
			this.label3.TabIndex = 9;
			this.label3.Text = "bestätigen:";
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(200, 107);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(56, 20);
			this.button3.TabIndex = 3;
			this.button3.Text = "setzen";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// Pswd
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.button2;
			this.ClientSize = new System.Drawing.Size(354, 131);
			this.ControlBox = false;
			this.Controls.Add(this.button3);
			this.Controls.Add(this.textBox3);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "Pswd";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Passworteingabe";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Pswd_KeyDown);
			this.ResumeLayout(false);

		}

		#endregion

		private Options options;

		public static void showAdminPassword(Options options)
		{
			if (Pswd.password == null)
			{
				Pswd.password = new Pswd();
			}
			Pswd.password.options = options;
			Pswd.password.Show();
		}

		private static bool ok = false;

		public static bool OK
		{
			get { return Pswd.ok; }
		}

		private bool checkPsw()
		{
			try
			{
				ConfigFile configFile = new ConfigFile(Util.CONFIGPATH);
				string pw = configFile["pw"];
				return pw.Equals(this.textBox1.Text);
			}
			catch (Exception e)
			{
				Util.MBoxError(e.Message, e);
			}
			return false;

		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			if (this.checkPsw())
			{
				this.options.enableChanges();
				Pswd.ok = true;
				this.options.Focus();
				this.Close();
			}
			else
			{
				Util.MBoxError("Falsches Passwort!");
				Pswd.ok = false;
				this.textBox1.Text = "";
				this.textBox1.Select();
			}
		}

		// Abbrechen
		private void button2_Click(object sender, System.EventArgs e)
		{
			this.options.Focus();
			this.Close();
		}

		// ändern
		private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			this.button1.Enabled = false;
			this.Height = 160;
		}

		// psw setzen
		private void button3_Click(object sender, System.EventArgs e)
		{
			if (this.checkPsw() &&
				this.textBox2.Text == this.textBox3.Text &&
				this.textBox2.Text != "")
			{
				ConfigFile configFile = new ConfigFile(Util.CONFIGPATH);
				configFile["pw"] = this.textBox2.Text;
				configFile.Save(Util.CONFIGPATH);

				this.Height = 104;
				this.button1.Enabled = true;
				this.textBox1.Text = "";
				this.textBox1.Select();
			}
			else
			{
				Util.MBoxError("Angaben nicht korrekt!\n" +
					"Geben Sie bitte das alte und das neue Passwort\nan und bestätigen Sie das neue!");
			}
		}

		private void Pswd_KeyDown(object sender, KeyEventArgs ke)
		{
			if (ke.KeyCode == Keys.Enter)
			{
				this.button1_Click(sender, ke);
			}
		}
	}
}