using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
	/// <summary>
	/// Summary description for TestForm.
	/// </summary>
	public class TestForm : System.Windows.Forms.Form
	{
		private ExtendedListBoxControl listBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TestForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.listBox1.BeginUpdate();
			for(int i = 0; i < 50; i++)
			{
				ExtendedListBoxItem item = new ExtendedListBoxItem();
				item.MaxString = "Das ist ein Liedtext, resp. eine tolle Vorschau...";
				item.MinString = (i+1) + " Liedtitel";
				this.listBox1.AddItem(item);
			}
			this.listBox1.EndUpdate();
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.listBox1 = new ExtendedListBoxControl();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.listBox1.Location = new System.Drawing.Point(0, 0);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(824, 430);
			this.listBox1.TabIndex = 0;
			// 
			// TestForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(824, 430);
			this.Controls.Add(this.listBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Name = "TestForm";
			this.Text = "TestForm";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
