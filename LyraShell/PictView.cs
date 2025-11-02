using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
	/// <summary>
	/// Summary description for PictView.
	/// </summary>
	public class PictView : Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private string[] files;
		private PictureBox pictureBox1;
		private Label label1;
		private static PictView pview = null;

		public static void ShowPictView(string directory)
		{
			if (pview == null)
			{
				pview = new PictView(directory);
			}
			pview.Show();
		}

		private PictView(string directory)
		{
		    InitializeComponent();
			Width = View.Display.Bounds.Width;
			Height = View.Display.Bounds.Height;
			Top = View.Display.Bounds.Top;
			Left = View.Display.Bounds.Left;
			pictureBox1.Height = Height;
			pictureBox1.Width = Width;
			pictureBox1.Top = 0;
			pictureBox1.Left = 0;

			KeyDown += new KeyEventHandler(PictView_KeyDown);
			pictureBox1.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);
			label1.Visible = false;
			files = Directory.GetFiles(directory);
			nextImage();
		}

		private int current = -1;

		/// <summary>
		/// get next image
		/// </summary>
		private void nextImage()
		{
			setImage(1);
		}

		/// <summary>
		/// get previous image
		/// </summary>
		private void prevImage()
		{
			setImage(-1);
		}

		// set the image
		private void setImage(int diff)
		{
			if (!label1.Visible)
			{
				diff += files.Length;
				current = (current + diff)%files.Length;
				var count = 0;
				while (!(files[current].ToLower().EndsWith(".jpg")
					|| files[current].ToLower().EndsWith(".gif")
					|| files[current].ToLower().EndsWith(".png")
					|| files[current].ToLower().EndsWith(".bmp")))
				{
					current = (current + diff)%files.Length;
					if (++count > files.Length)
					{
						label1.Visible = true;
						return;
					}
				}
				var img = Image.FromFile(files[current]);
				pictureBox1.Image = Util.stretchProportional(img, new Size(Width, Height));
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			pview = null;
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
				Close();
				Dispose();
			}
			else if (e.KeyCode == Keys.PageDown)
			{
				prevImage();
			}
			else if (e.KeyCode == Keys.PageUp)
			{
				nextImage();
			}
		}

		// MOUSE DOWN
		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				nextImage();
			}
			else if (e.Button == MouseButtons.Right)
			{
				prevImage();
			}
		}
	}
}