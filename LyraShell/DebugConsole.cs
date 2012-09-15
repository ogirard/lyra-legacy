using System;


namespace Lyra2.LyraShell
{
	/// <summary>
	/// Zusammendfassende Beschreibung für DebugConsole.
	/// </summary>
	public class DebugConsole : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox textBox1;

		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private LyraButtonControl button1;

		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private DebugConsole()
		{
			InitializeComponent();
			this.Clear();
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
			DebugConsole.cons = null;
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DebugConsole));
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new LyraButtonControl();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.AcceptsReturn = true;
			this.textBox1.AutoSize = false;
			this.textBox1.BackColor = System.Drawing.Color.White;
			this.textBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.textBox1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBox1.ForeColor = System.Drawing.Color.Black;
			this.textBox1.Location = new System.Drawing.Point(0, 56);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(456, 120);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "textBox1";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.White;
			this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(64, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(51, 19);
			this.label2.TabIndex = 7;
			this.label2.Text = "version: ";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.SlateGray;
			this.label1.Location = new System.Drawing.Point(0, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(160, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "lyra debug console";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(1, 29);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(56, 24);
			this.button1.TabIndex = 8;
			this.button1.Text = "clear";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// DebugConsole
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(452, 177);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(460, 100);
			this.Name = "DebugConsole";
			this.Text = "lyra Debug Console";
			this.SizeChanged += new System.EventHandler(this.adaptSize);
			this.ResumeLayout(false);

		}

		#endregion

		private static DebugConsole cons = null;

		public static void ShowDebugConsole(GUI owner)
		{
			if (DebugConsole.cons == null)
			{
				DebugConsole.cons = new DebugConsole();
			}
			DebugConsole.cons.adaptSize(owner, null);
			DebugConsole.cons.label2.Text = Util.NAME + " v" + Util.VER + "   " + Util.BUILD;
			DebugConsole.cons.Show();
			DebugConsole.cons.Focus();
		}

		private void adaptSize(object sender, System.EventArgs args)
		{
			this.textBox1.Left = 0;
			this.textBox1.Top = 50;
			this.textBox1.Width = this.ClientRectangle.Width;
			this.textBox1.Height = this.ClientRectangle.Height - 50;
			this.button1.Left = this.textBox1.Right - this.button1.Width;
		}

		private void Clear()
		{
			this.textBox1.Text = "";
		}

		public static void Append(string msg, Exception ex)
		{
			if (msg != "")
			{
				DebugConsole.cons.textBox1.AppendText(DebugConsole.ReplaceESC(msg) + Util.NL + Util.NL);
			}
			if (ex != null)
			{
				DebugConsole.cons.textBox1.AppendText(ex + Util.NL + ">> source: " +
					ex.Source + Util.NL + "Stack:" + Util.NL +
					ex.StackTrace + Util.NL + Util.NL);
			}
		}

		private static string ReplaceESC(string msg)
		{
			string res = "";
			bool last = false;
			foreach (char c in msg)
			{
				if (c == '\r')
				{
					last = true;
				}
				else if (c == '\n' && !last)
				{
					res += '\r';
				}
				else
				{
					last = false;
				}
				res += c;
			}
			return res;
		}

		// clear console
		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Clear();
		}
	}
}