using System;
using System.Security.Principal;
using System.Windows.Forms;


namespace Lyra2.LyraShell
{
	/// <summary>
	/// Zusammendfassende Beschreibung für NewList.
	/// </summary>
	public class NewList : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox nameTextBox;
		private System.Windows.Forms.TextBox authorTextBox;
		private LyraButtonControl button3;
		private LyraButtonControl button1;

		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private GUI owner;
		private static NewList newList = null;

		public static void ShowNewList(GUI owner)
		{
			ShowNewList(owner, "Neue Liste", null);
		}

        public static void ShowNewList(GUI owner, string name, string[] songs)
        {
            if (NewList.newList == null)
            {
                NewList.newList = new NewList(owner);
            }
            newList.songs = songs;
            newList.nameTextBox.Text = name;
            WindowsIdentity wi = WindowsIdentity.GetCurrent();
            if (wi != null)
            {
                string[] parts = wi.Name.Split('\\');
                if (parts.Length > 0) newList.authorTextBox.Text = parts[parts.Length - 1];
            }
            NewList.newList.Show();
        }

		private NewList(GUI owner)
		{
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();
			this.owner = owner;
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
			NewList.newList = null;
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
			this.label3 = new System.Windows.Forms.Label();
			this.nameTextBox = new System.Windows.Forms.TextBox();
			this.authorTextBox = new System.Windows.Forms.TextBox();
			this.button3 = new LyraButtonControl();
			this.button1 = new LyraButtonControl();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
			this.label1.ForeColor = System.Drawing.Color.SlateGray;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(192, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "Neue Liste erstellen";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 16);
			this.label2.TabIndex = 7;
			this.label2.Text = "Titel:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 16);
			this.label3.TabIndex = 8;
			this.label3.Text = "Autor:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// nameTextBox
			// 
			this.nameTextBox.Location = new System.Drawing.Point(58, 29);
			this.nameTextBox.Name = "nameTextBox";
			this.nameTextBox.Size = new System.Drawing.Size(296, 20);
			this.nameTextBox.TabIndex = 10;
			this.nameTextBox.Text = "";
			// 
			// authorTextBox
			// 
			this.authorTextBox.Location = new System.Drawing.Point(58, 56);
			this.authorTextBox.Name = "authorTextBox";
			this.authorTextBox.Size = new System.Drawing.Size(296, 20);
			this.authorTextBox.TabIndex = 11;
			this.authorTextBox.Text = "";
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(200, 88);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(80, 24);
			this.button3.TabIndex = 13;
			this.button3.Text = "Abbrechen";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(288, 88);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(64, 24);
			this.button1.TabIndex = 12;
			this.button1.Text = "Ok";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// NewList
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(362, 115);
			this.ControlBox = false;
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.authorTextBox);
			this.Controls.Add(this.nameTextBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewList";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Neue Liste";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NewList_KeyDown);
			this.ResumeLayout(false);

		}

		#endregion

		private void button3_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

	    private string[] songs;
		private void button1_Click(object sender, System.EventArgs e)
		{
			this.owner.CreateNewList(this.nameTextBox.Text, this.authorTextBox.Text, songs);
			this.Close();
		}

		private void NewList_KeyDown(object sender, KeyEventArgs ke)
		{
			if (ke.KeyCode == Keys.Enter)
			{
				this.button1_Click(sender, ke);
			}
		}
	}
}