using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;


namespace Lyra2.LyraShell
{
	/// <summary>
	/// Summary description for LyraUpdateView.
	/// </summary>
	public class LyraUpdateView : System.Windows.Forms.Form
	{
		private LyraButtonControl button2;
		private LyraButtonControl button1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckedListBox checkedListBox1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;

		public static string CURTEXT = Util.BASEURL + "\\" + Util.URL;
		public static string TMPFILE = LyraUpdateView.CURTEXT + ".tmp";
		public static string BACKUP = LyraUpdateView.CURTEXT + ".bac";

		public static string LISTFILE = Util.BASEURL + "\\" + Util.LISTURL;
		public static string TMPLIST = LyraUpdateView.LISTFILE + ".tmp";
		public static string BACKUPLIST = LyraUpdateView.LISTFILE + ".bac";

		private bool backup;


		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private System.Windows.Forms.Panel panel1;

		private static LyraUpdateView update = null;

		private LyraUpdateView(string url, string desc, UpdateFileURL[] versions, bool backup)
		{
			InitializeComponent();

			this.label4.Text = url;
			this.label5.Text = desc;
			foreach (UpdateFileURL v in versions)
			{
				this.comboBox1.Items.Add(v);
			}
			this.comboBox1.SelectedIndex = 0;
			this.backup = backup;
		}

		public static DialogResult ShowUpdateView(GUI owner, string url, string desc,
		                                          UpdateFileURL[] versions, bool backup)
		{
			if (LyraUpdateView.update == null)
			{
				LyraUpdateView.update = new LyraUpdateView(url, desc, versions, backup);
			}
			return LyraUpdateView.update.ShowDialog(owner);
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
			LyraUpdateView.update = null;
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button2 = new LyraButtonControl();
			this.button1 = new LyraButtonControl();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
			this.button2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.button2.Location = new System.Drawing.Point(208, 176);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(72, 24);
			this.button2.TabIndex = 1;
			this.button2.Text = "Abbrechen";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
			this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.button1.Location = new System.Drawing.Point(296, 176);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(72, 24);
			this.button1.TabIndex = 1;
			this.button1.Text = "Ok";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
			this.label2.ForeColor = System.Drawing.Color.SlateGray;
			this.label2.Location = new System.Drawing.Point(8, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(280, 32);
			this.label2.TabIndex = 4;
			this.label2.Text = "Server:";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
			this.label1.ForeColor = System.Drawing.Color.SlateGray;
			this.label1.Location = new System.Drawing.Point(8, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(280, 32);
			this.label1.TabIndex = 4;
			this.label1.Text = "Update:";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
			this.label3.ForeColor = System.Drawing.Color.SlateGray;
			this.label3.Location = new System.Drawing.Point(8, 88);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(280, 32);
			this.label3.TabIndex = 4;
			this.label3.Text = "Listen:";
			// 
			// checkedListBox1
			// 
			this.checkedListBox1.Location = new System.Drawing.Point(88, 88);
			this.checkedListBox1.Name = "checkedListBox1";
			this.checkedListBox1.Size = new System.Drawing.Size(280, 79);
			this.checkedListBox1.TabIndex = 5;
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.Location = new System.Drawing.Point(88, 56);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(280, 21);
			this.comboBox1.TabIndex = 6;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
			this.label4.Location = new System.Drawing.Point(4, 5);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(272, 16);
			this.label4.TabIndex = 7;
			this.label4.Text = "url";
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.Transparent;
			this.label5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
			this.label5.Location = new System.Drawing.Point(4, 21);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(272, 16);
			this.label5.TabIndex = 7;
			this.label5.Text = "name [owner]";
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.label5);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Location = new System.Drawing.Point(88, 8);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(280, 40);
			this.panel1.TabIndex = 8;
			// 
			// LyraUpdateView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(378, 208);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.checkedListBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label3);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LyraUpdateView";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Lyra Update konfigurieren";
			this.TopMost = true;
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		// make a backup of the curtext.xml-file
		private void backupCurtext()
		{
			File.Copy(LyraUpdateView.CURTEXT, LyraUpdateView.BACKUP, true);
			File.Copy(LyraUpdateView.LISTFILE, LyraUpdateView.BACKUPLIST, true);
		}

		// download the last version of the songs
		private void downloadCurtext(string url)
		{
			try
			{
				WebClient client = new WebClient();
				client.DownloadFile(url, LyraUpdateView.TMPFILE);
			}
			catch (Exception exp)
			{
				this.DialogResult = DialogResult.Abort;
				Util.MBoxError("Download fehlgeschlagen!", exp);
			}
		}

		// do update!
		private void button1_Click(object sender, System.EventArgs e)
		{
			this.downloadCurtext(((UpdateFileURL) this.comboBox1.SelectedItem).URL);
			if (this.backup)
			{
				this.backupCurtext();
			}

			XmlDocument doc = new XmlDocument();
			doc.Load(LyraUpdateView.LISTFILE);
			XmlNode listroot = doc.GetElementsByTagName("lists")[0];
			foreach (XMLWrapperObject xmlobj in this.checkedListBox1.CheckedItems)
			{
				try
				{
					listroot.AppendChild(doc.ImportNode(xmlobj.Node, true));
				}
				catch (Exception ex)
				{
					Util.MBoxError(ex.ToString() + Util.NL + ex.Message);
				}
			}
			doc.Save(LyraUpdateView.TMPLIST);

			File.Copy(LyraUpdateView.TMPFILE, LyraUpdateView.CURTEXT, true);
			File.Delete(LyraUpdateView.TMPFILE);
			File.Copy(LyraUpdateView.TMPLIST, LyraUpdateView.LISTFILE, true);
			File.Delete(LyraUpdateView.TMPLIST);

			LyraUpdateView.update.Dispose();
		}

		// cancel
		private void button2_Click(object sender, System.EventArgs e)
		{
			LyraUpdateView.update.Dispose();
		}

		// change list-content
		private void selectVersion(UpdateFileURL ver)
		{
			this.checkedListBox1.Items.Clear();
			if (ver.List != "")
			{
				XmlDocument doc = new XmlDocument();
				try
				{
					WebRequest request = WebRequest.Create(ver.List);
					HttpWebResponse response = (HttpWebResponse) request.GetResponse();
					Stream stream = response.GetResponseStream();
					doc.Load(stream);

					XmlNodeList lists = doc.GetElementsByTagName("List");
					foreach (XmlNode curList in lists)
					{
						string name = curList.ChildNodes[0].InnerText + "  [" +
							curList.ChildNodes[2].InnerText + "]";
						this.checkedListBox1.Items.Add(new XMLWrapperObject(curList, name));
					}
					stream.Close();
					response.Close();
				}
				catch (Exception exp)
				{
					Util.MBoxError("Update fehlgeschlagen!", exp);
				}
			}
		}

		// selected Index changed
		private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.selectVersion((UpdateFileURL) this.comboBox1.SelectedItem);
		}

		private class XMLWrapperObject
		{
			private string name;
			private XmlNode node;

			public XMLWrapperObject(XmlNode node, string name)
			{
				this.node = node;
				this.name = name;
			}

			public XmlNode Node
			{
				get { return this.node; }
			}

			public override string ToString()
			{
				return this.name;
			}

		}
	}
}