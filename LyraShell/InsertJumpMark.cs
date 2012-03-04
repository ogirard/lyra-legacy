using System;
using System.Security.Principal;
using System.Windows.Forms;


namespace Lyra2.LyraShell
{
	/// <summary>
	/// Zusammendfassende Beschreibung für NewList.
	/// </summary>
	public class InsertJumpMark : System.Windows.Forms.Form
	{
		private Label label1;
        private Label label2;
        private TextBox nameTextBox;
		private LyraButtonControl cancelBtn;
		private LyraButtonControl okBtn;

		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public InsertJumpMark()
		{
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();
			this.AcceptButton = this.okBtn;
		}

        public string JumpMarkName
        {
            get { return this.nameTextBox.Text; }
            set { this.nameTextBox.Text = value; }
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
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.cancelBtn = new Lyra2.LyraShell.LyraButtonControl();
            this.okBtn = new Lyra2.LyraShell.LyraButtonControl();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SlateGray;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Sprungmarke einfügen";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Name:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(58, 29);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(296, 20);
            this.nameTextBox.TabIndex = 10;
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(200, 61);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(80, 24);
            this.cancelBtn.TabIndex = 13;
            this.cancelBtn.Text = "Abbrechen";
            this.cancelBtn.Click += new System.EventHandler(this.button3_Click);
            // 
            // okBtn
            // 
            this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBtn.Location = new System.Drawing.Point(288, 61);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(64, 24);
            this.okBtn.TabIndex = 12;
            this.okBtn.Text = "Ok";
            // 
            // InsertJumpMark
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(362, 95);
            this.ControlBox = false;
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InsertJumpMark";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sprungmarke einfügen";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private void button3_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}
}