using System.Windows.Forms;


namespace Lyra2.LyraShell
{
	/// <summary>
	/// Summary description for FX.
	/// </summary>
	public class FX : System.Windows.Forms.Form
	{
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radioButton3;
		private LyraButtonControl button1;
		private LyraButtonControl button2;

		public enum FXResult
		{
			Cancel = -1,
			Delete = 0,
			NewFile = 1,
			PictChoice = 2
		}

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private static FX fx = null;
		private static FXResult result = FXResult.Cancel;
		private Options owner;

		public static FXResult ShowFX(Options owner)
		{
			if (FX.fx == null)
			{
				FX.fx = new FX(owner);
			}
			if (fx.ShowDialog(owner) == DialogResult.OK)
			{
				return FX.result;
			}
			else
			{
				return FXResult.Cancel;
			}

		}

		private FX(Options owner)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.owner = owner;
			this.AcceptButton = this.button1;
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
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.button1 = new LyraButtonControl();
			this.button2 = new LyraButtonControl();
			this.SuspendLayout();
			// 
			// radioButton1
			// 
			this.radioButton1.Checked = true;
			this.radioButton1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.radioButton1.ForeColor = System.Drawing.Color.DimGray;
			this.radioButton1.Location = new System.Drawing.Point(16, 16);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(352, 16);
			this.radioButton1.TabIndex = 0;
			this.radioButton1.TabStop = true;
			this.radioButton1.Text = "keine Funktion zuweisen (aktuelle löschen)";
			// 
			// radioButton2
			// 
			this.radioButton2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.radioButton2.ForeColor = System.Drawing.Color.DimGray;
			this.radioButton2.Location = new System.Drawing.Point(16, 40);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(360, 16);
			this.radioButton2.TabIndex = 0;
			this.radioButton2.Text = "eine Datei auswählen (z.B. Microsoft® PowerPoint® Präsentation)";
			// 
			// radioButton3
			// 
			this.radioButton3.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.radioButton3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.radioButton3.ForeColor = System.Drawing.Color.DimGray;
			this.radioButton3.Location = new System.Drawing.Point(16, 64);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.Size = new System.Drawing.Size(352, 32);
			this.radioButton3.TabIndex = 0;
			this.radioButton3.Text = "Bilderauswahl (wird in internem Viewer dargestellt)                         Es we" +
				"rden Bilder vom Typ JPG, GIF, PNG und BMP unterstützt.";
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Location = new System.Drawing.Point(304, 104);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(64, 24);
			this.button1.TabIndex = 1;
			this.button1.Text = "Ok";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(208, 104);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(80, 24);
			this.button2.TabIndex = 2;
			this.button2.Text = "Abbrechen";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// FX
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(378, 133);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.radioButton1);
			this.Controls.Add(this.radioButton2);
			this.Controls.Add(this.radioButton3);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FX";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Wählen Sie eine FX-Funktion!";
			this.TopMost = true;
			this.ResumeLayout(false);

		}

		#endregion

		// cancel
		private void button2_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		// ok
		private void button1_Click(object sender, System.EventArgs e)
		{
			FX.result = this.radioButton1.Checked ? FXResult.Delete : this.radioButton2.Checked ? FXResult.NewFile : FXResult.PictChoice;
			this.Close();
		}
	}
}