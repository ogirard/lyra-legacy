using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;


namespace Lyra2.LyraShell
{
	/// <summary>
	/// Summary description for GetLyraXML.
	/// </summary>
	public class LyraUpdate : System.Windows.Forms.Form
	{
		private LyraButtonControl button1;
		private System.Windows.Forms.Label label1;
		private LyraButtonControl button2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;

		private static LyraUpdate lyraUpdate = null;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox checkBox1;


		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private LyraUpdate()
		{
			InitializeComponent();
			this.textBox1.Text = Util.UPDATESERVER;
			this.label5.Focus();
		}

		public static DialogResult ShowUpdate(GUI owner)
		{
			if (LyraUpdate.lyraUpdate == null)
			{
				LyraUpdate.lyraUpdate = new LyraUpdate();
			}
			return LyraUpdate.lyraUpdate.ShowDialog(owner);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			LyraUpdate.lyraUpdate = null;
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
			this.button1 = new LyraButtonControl();
			this.label1 = new System.Windows.Forms.Label();
			this.button2 = new LyraButtonControl();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
			this.button1.ForeColor = System.Drawing.Color.LightSlateGray;
			this.button1.Location = new System.Drawing.Point(220, 175);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(104, 24);
			this.button1.TabIndex = 0;
			this.button1.Text = "Starte Download";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.Color.DimGray;
			this.label1.Location = new System.Drawing.Point(12, 38);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(296, 32);
			this.label1.TabIndex = 2;
			this.label1.Text = "Wenn Sie auf \"update\" klicken, werden die Liedtexte automatisch vom Server gelade" +
				"n.";
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
			this.button2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.button2.Location = new System.Drawing.Point(140, 175);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(72, 24);
			this.button2.TabIndex = 0;
			this.button2.Text = "Abbrechen";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
			this.label2.ForeColor = System.Drawing.Color.SlateGray;
			this.label2.Location = new System.Drawing.Point(8, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(280, 32);
			this.label2.TabIndex = 3;
			this.label2.Text = "Update der Liedtexte";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
			this.label3.ForeColor = System.Drawing.Color.SaddleBrown;
			this.label3.Location = new System.Drawing.Point(29, 69);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(296, 38);
			this.label3.TabIndex = 2;
			this.label3.Text = "Vorerst wird kein Merging unterstützt, d.h. Ihre eigenen Liedtexte werden beim Up" +
				"date einfach überschrieben und direkt übernommen!";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
			this.label4.ForeColor = System.Drawing.Color.DarkRed;
			this.label4.Location = new System.Drawing.Point(8, 70);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(16, 32);
			this.label4.TabIndex = 4;
			this.label4.Text = "!";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
			this.label5.ForeColor = System.Drawing.Color.DimGray;
			this.label5.Location = new System.Drawing.Point(272, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(56, 23);
			this.label5.TabIndex = 5;
			this.label5.Text = "BETA";
			// 
			// textBox1
			// 
			this.textBox1.Enabled = false;
			this.textBox1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
			this.textBox1.Location = new System.Drawing.Point(12, 139);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(312, 20);
			this.textBox1.TabIndex = 6;
			this.textBox1.Text = "textBox1";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(12, 120);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(152, 16);
			this.label6.TabIndex = 7;
			this.label6.Text = "Update Server URL:";
			// 
			// checkBox1
			// 
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.checkBox1.Location = new System.Drawing.Point(12, 168);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(108, 16);
			this.checkBox1.TabIndex = 8;
			this.checkBox1.Text = "erstelle Backup ";
			// 
			// LyraUpdate
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(330, 205);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label4);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LyraUpdate";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "lyra - Update    BETA";
			this.TopMost = true;
			this.ResumeLayout(false);

		}

		#endregion

		// cancel
		private void button2_Click(object sender, System.EventArgs e)
		{
			this.Dispose();
		}

		// execute update!
		private void button1_Click(object sender, System.EventArgs e)
		{
			this.doUpdate();
			this.Dispose();
		}

		// get download-url
		private void doUpdate()
		{
			XmlDocument doc = new XmlDocument();
			try
			{
				WebRequest request = WebRequest.Create(this.textBox1.Text + "/updatedesc.xml");
				HttpWebResponse response = (HttpWebResponse) request.GetResponse();
				Stream stream = response.GetResponseStream();
				doc.Load(stream);

				XmlNodeList versions = doc.GetElementsByTagName("lyraXML");
				UpdateFileURL[] ver = new UpdateFileURL[versions.Count];
				int i = 0;
				foreach (XmlNode curVers in versions)
				{
					string listurl = "";
					if (curVers.ChildNodes.Count != 0)
					{
						listurl = curVers.ChildNodes[0].Attributes["url"].InnerText;
					}
					ver[i++] = new UpdateFileURL(curVers.Attributes["url"].InnerText, curVers.Attributes["name"].InnerText,
					                             curVers.Attributes["ver"].InnerText, listurl);
				}

				XmlNode server = doc.GetElementsByTagName("serverdetails")[0];
				string desc = server.ChildNodes[0].InnerText + " [" +
					server.ChildNodes[1].InnerText + "]";
				string serverurl = server.ChildNodes[2].InnerText;

				this.DialogResult = LyraUpdateView.ShowUpdateView((GUI) this.Owner,
				                                                  serverurl, desc, ver, this.checkBox1.Checked);

				stream.Close();
				response.Close();
			}
			catch (Exception exp)
			{
				Util.MBoxError("Update fehlgeschlagen!", exp);
			}
		}
	}
}