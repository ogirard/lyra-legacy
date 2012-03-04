using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
	/// <summary>
	/// Summary description for PrBackground.
	/// </summary>
	public class PrBackground : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private GUI owner;

		public PrBackground(GUI owner)
		{
			this.owner = owner;
			InitializeComponent();

			// init Screen
			this.Width = Screen.PrimaryScreen.Bounds.Width;
			this.Height = Screen.PrimaryScreen.Bounds.Height;
			this.Top = 0;
			this.Left = 0;

			this.label1.Text = "lyra v" + Util.VER + " " + Util.BUILD;
			this.label1.Top = this.Height - this.label1.Height - 15;
			this.label1.Left = this.Width - this.label1.Width - 15;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (this.bgbitmap != null) this.bgbitmap.Dispose();
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		// on paint
		private Bitmap bgbitmap = null;

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics g = e.Graphics;
			switch (Util.PREMODE)
			{
				case 0:
					g.FillRectangle(new SolidBrush(Util.UNICOLOR), 0, 0, this.Width, this.Height);
					break;
				case 1:
					g.FillRectangle(new LinearGradientBrush(new Point(0, 0), new Point(0, this.Height), Util.GRADCOL1, Util.GRADCOL2), 0, 0, this.Width, this.Height);
					break;
				case 2:
					{
						if (Util.BGIMG != null)
						{
							if (Util.CASCADEPIC)
							{
								this.BackgroundImage = Util.BGIMG;
							}
							else
							{
								if (this.bgbitmap == null)
								{
									this.bgbitmap = new Bitmap(Util.BGIMG, new Size(this.Width, this.Height));
								}
								g.DrawImage(this.bgbitmap, 0, 0, this.Width, this.Height);
							}
						}
						else
						{
							g.FillRectangle(new SolidBrush(Util.UNICOLOR), 0, 0, this.Width, this.Height);
						}
						break;
					}
			}
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.ForeColor = System.Drawing.Color.MidnightBlue;
			this.label1.Location = new System.Drawing.Point(72, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(22, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "lyra";
			// 
			// PrBackground
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(264, 128);
			this.ControlBox = false;
			this.Controls.Add(this.label1);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PrBackground";
			this.ShowInTaskbar = false;
			this.GotFocus += new System.EventHandler(this.PrBackground_GotFocus);
			this.ResumeLayout(false);

		}

		#endregion

		// pass Focus to owner!
		private void PrBackground_GotFocus(object sender, EventArgs e)
		{
			History.ForceFocus();
			this.owner.Focus();
		}
	}
}