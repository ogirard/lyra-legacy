using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
	/// <summary>
	/// Summary description for PictView.
	/// </summary>
	public class PictView : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private string[] files;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
		private static PictView pview = null;

		public static void ShowPictView(string directory)
		{
			if (PictView.pview == null)
			{
				PictView.pview = new PictView(directory);
			}
			PictView.pview.Show();
		}

		private PictView(string directory)
		{
			InitializeComponent();
			this.Width = View.Display.Bounds.Width;
			this.Height = View.Display.Bounds.Height;
			this.Top = View.Display.Bounds.Top;
			this.Left = View.Display.Bounds.Left;
			this.pictureBox1.Height = this.Height;
			this.pictureBox1.Width = this.Width;
			this.pictureBox1.Top = 0;
			this.pictureBox1.Left = 0;

			this.KeyDown += new KeyEventHandler(PictView_KeyDown);
			this.pictureBox1.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);
			this.label1.Visible = false;
			this.files = Directory.GetFiles(directory);
			this.nextImage();
		}

		private int current = -1;

		/// <summary>
		/// get next image
		/// </summary>
		private void nextImage()
		{
			this.setImage(1);
		}

		/// <summary>
		/// get previous image
		/// </summary>
		private void prevImage()
		{
			this.setImage(-1);
		}

		// set the image
		private void setImage(int diff)
		{
			if (!this.label1.Visible)
			{
				diff += this.files.Length;
				this.current = (this.current + diff)%this.files.Length;
				int count = 0;
				while (!(this.files[this.current].ToLower().EndsWith(".jpg")
					|| this.files[this.current].ToLower().EndsWith(".gif")
					|| this.files[this.current].ToLower().EndsWith(".png")
					|| this.files[this.current].ToLower().EndsWith(".bmp")))
				{
					this.current = (this.current + diff)%this.files.Length;
					if (++count > this.files.Length)
					{
						this.label1.Visible = true;
						return;
					}
				}
				Image img = Image.FromFile(this.files[this.current]);
				this.pictureBox1.Image = Util.stretchProportional(img, new Size(this.Width, this.Height));
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			PictView.pview = null;
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
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.Location = new System.Drawing.Point(72, 32);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(344, 216);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(344, 56);
			this.label1.TabIndex = 1;
			this.label1.Text = "Keine Bilder gefunden!";
			this.label1.Visible = false;
			// 
			// PictView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(488, 280);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pictureBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "PictView";
			this.Text = "PictView";
			this.TopMost = true;
			this.ResumeLayout(false);

		}

		#endregion

		// KEY DOWN
		private void PictView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.Close();
				this.Dispose();
			}
			else if (e.KeyCode == Keys.PageDown)
			{
				this.prevImage();
			}
			else if (e.KeyCode == Keys.PageUp)
			{
				this.nextImage();
			}
		}

		// MOUSE DOWN
		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.nextImage();
			}
			else if (e.Button == MouseButtons.Right)
			{
				this.prevImage();
			}
		}
	}
}