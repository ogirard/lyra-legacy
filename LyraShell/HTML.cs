using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Lyra2.UtilShared;

namespace Lyra2.LyraShell
{
    /// <summary>
    /// Zusammendfassende Beschreibung für HTML.
    /// </summary>
    public class HTML : Form
    {
        private LyraButtonControl button1;
        private Label label1;
        private LyraButtonControl button4;
        private LyraButtonControl button2;
        private Label label6;
        private Label label2;
        private Label label3;

        private static HTML myHTML = null;

        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private Container components = null;

        private Label label4;
        private RadioButton radioButton1;
        private Label label5;
        private Label label7;
        private RadioButton radioButton2;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private Label label8;

        private string project = "LyraHTML";
        private ListBox box;
        private string idtext = "";
        private CheckBox checkBox1;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;

        private GUI owner;

        public static void showHTML(GUI owner, ListBox box, string idtext)
        {
            if (myHTML == null)
            {
                myHTML = new HTML(owner, box, idtext);
            }
            myHTML.Enabled = true;
            myHTML.Show();
            myHTML.Focus();
        }

        private HTML(GUI owner, ListBox box, string idtext)
        {
            //
            // Erforderlich für die Windows Form-Designerunterstützung
            //
            InitializeComponent();

            this.owner = owner;
            this.box = box;
            this.idtext = idtext;
            this.updateFields(Util.BASEURL);
            this.AcceptButton = this.button1;
        }

        private void updateFields(string url)
        {
            this.textBox4.Text = this.project;
            this.textBox1.Text = url;
            string sep = url.EndsWith("\\") ? "" : "\\";
            this.textBox2.Text = url + sep + this.project + "\\lyra.html";
            this.textBox3.Text = url + sep + this.project + "\\data";
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
            myHTML = null;
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof (HTML));
            this.button1 = new LyraButtonControl();
            this.label1 = new System.Windows.Forms.Label();
            this.button4 = new LyraButtonControl();
            this.button2 = new LyraButtonControl();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(440, 280);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 24);
            this.button1.TabIndex = 0;
            this.button1.Text = "Ok";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Font =
                new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold,
                                        System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
            this.label1.ForeColor = System.Drawing.Color.SlateGray;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "HTML-Seite generieren...";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(408, 58);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(104, 16);
            this.button4.TabIndex = 8;
            this.button4.Text = "durchsuchen...";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(360, 280);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(72, 24);
            this.button2.TabIndex = 9;
            this.button2.Text = "Abbrechen";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.SaddleBrown;
            this.label6.Location = new System.Drawing.Point(16, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 12);
            this.label6.TabIndex = 16;
            this.label6.Text = "Basispfad:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "HTML-Datei:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "Ressourcen:";
            // 
            // label4
            // 
            this.label4.Font =
                new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold,
                                        System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
            this.label4.ForeColor = System.Drawing.Color.SaddleBrown;
            this.label4.Location = new System.Drawing.Point(16, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(144, 32);
            this.label4.TabIndex = 21;
            this.label4.Text = "Modus:";
            // 
            // radioButton1
            // 
            this.radioButton1.Checked = true;
            this.radioButton1.Font =
                new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold,
                                        System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
            this.radioButton1.Location = new System.Drawing.Point(24, 160);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(272, 16);
            this.radioButton1.TabIndex = 22;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "HTML-Dateien vollständig erstellen!";
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.DimGray;
            this.label5.Location = new System.Drawing.Point(40, 178);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(328, 40);
            this.label5.TabIndex = 23;
            this.label5.Text = "Daten werden ausschliesslich in HTML-Dateien geschrieben und in einem Frame mit I" +
                               "nhaltsverzeichnis dargestellt.";
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.Color.DimGray;
            this.label7.Location = new System.Drawing.Point(40, 246);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(328, 49);
            this.label7.TabIndex = 25;
            this.label7.Text = "Daten werden (bereinigt) in eine XML-Datei geschrieben, die im Browser mit einem " +
                               "XSLT-StyleSheet angezeigt werden kann (ohne Frames; Anpassung an eigene Formatie" +
                               "rung viel einfacher).";
            // 
            // radioButton2
            // 
            this.radioButton2.Font =
                new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold,
                                        System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
            this.radioButton2.Location = new System.Drawing.Point(24, 228);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(272, 16);
            this.radioButton2.TabIndex = 24;
            this.radioButton2.Text = "Als XML/XSLT-Dateien exportieren!";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(88, 56);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(302, 20);
            this.textBox1.TabIndex = 26;
            this.textBox1.Text = "textBox1";
            this.textBox1.LostFocus += new System.EventHandler(this.textBox4_LostFocus);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(88, 88);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(302, 20);
            this.textBox2.TabIndex = 27;
            this.textBox2.Text = "textBox2";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(88, 112);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(302, 20);
            this.textBox3.TabIndex = 28;
            this.textBox3.Text = "textBox3";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(88, 30);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(302, 20);
            this.textBox4.TabIndex = 30;
            this.textBox4.Text = "textBox4";
            this.textBox4.LostFocus += new System.EventHandler(this.textBox4_LostFocus);
            // 
            // label8
            // 
            this.label8.ForeColor = System.Drawing.Color.SaddleBrown;
            this.label8.Location = new System.Drawing.Point(17, 34);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 12);
            this.label8.TabIndex = 29;
            this.label8.Text = "Projekttitel:";
            // 
            // checkBox1
            // 
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.ForeColor = System.Drawing.Color.DimGray;
            this.checkBox1.Location = new System.Drawing.Point(42, 203);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(160, 16);
            this.checkBox1.TabIndex = 31;
            this.checkBox1.Text = "Formatierung übernehmen!";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image) (resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(416, 152);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(96, 40);
            this.pictureBox1.TabIndex = 32;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image) (resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(416, 192);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(88, 40);
            this.pictureBox2.TabIndex = 33;
            this.pictureBox2.TabStop = false;
            // 
            // HTML
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(522, 315);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HTML";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HTML-Seite generieren";
            this.ResumeLayout(false);
        }

        #endregion

        // Abbrechen
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //OK
        private void button1_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(this.textBox3.Text) || File.Exists(this.textBox2.Text))
            {
                DialogResult dr =
                    MessageBox.Show(this, "Achtung! Sie überschreiben möglicherweise eine bereits erstellte HTML-Datei!",
                                    "lyra Hinweis", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.Cancel)
                {
                    return;
                }
                else
                {
                    try
                    {
                        //remove all files
                        if (Directory.Exists(this.textBox1.Text + "\\" + this.textBox4.Text))
                        {
                            Directory.Delete(this.textBox1.Text + "\\" + this.textBox4.Text, true);
                        }
                        this.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        Util.MBoxError("Dateien können nicht überschrieben werden!\n" +
                                       "Stellen Sie bitte sicher, dass keine anderen Programme darauf zugreifen.", ex);
                        return;
                    }
                }
            }
            if (this.radioButton1.Checked)
            {
                if (this.SaveHTML(this.box))
                {
                    this.owner.Status = "HTML erfolgreich erstellt! :-)";
                }
                else
                {
                    this.owner.Status = "HTML-Generierung fehlgeschlagen! :-(";
                }
            }
            else
            {
                if (this.SaveXML())
                {
                    this.owner.Status = "HTML erfolgreich erstellt! :-)";
                }
                else
                {
                    this.owner.Status = "HTML-Generierung fehlgeschlagen! :-(";
                }
            }
            this.Close();
        }

        // durchsuchen...
        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowser fb = new FolderBrowser();
            fb.Flags = BrowseFlags.BIF_STATUSTEXT | BrowseFlags.BIF_EDITBOX | BrowseFlags.BIF_NEWDIALOGSTYLE;
            DialogResult dr = fb.ShowDialog();

            if (dr == DialogResult.OK)
            {
                this.updateFields(fb.DirectoryPath);
            }
        }

        private void textBox4_LostFocus(object sender, EventArgs args)
        {
            this.project = textBox4.Text;
            this.updateFields(this.textBox1.Text);
        }


        private string HTMLHEADER =
            "<html>\n\t<head>\n\t\t<meta HTTP-EQUIV=\"content-type\" CONTENT=\"text/html; charset=UTF-8\">\n\n\t\t<title>lyra Songliste</title>\n\n\t\t" +
            "<meta name=\"DC.Title\" content=<\"lyra Songliste\" />\n\t\t" +
            "<meta name=\"DC.Date\" content=\"" + Util.GetDate() + "\" />\n\t\t" +
            "<meta name=\"DC.Language\" content=\"de\" />\n\t\t" +
            "<meta name=\"DC.Rights\" content=\"Alle Rechte dem Autor vorbehalten.\" />\n\t\t" +
            "<meta name=\"generator\" content=\"lyra v1.1 html-generator\" />\n\n\n\t\t" +
            "<style type=\"text/css\">\n\n\t\t\t" +
            "td\n\t\t\t{\n\t\t\t\tfont-family:verdana;\n\t\t\t\tfont-size:10pt;\n\t\t\t\tcolor:#000000;\n\t\t\t\t" +
            "text-align:left;\n\t\t\t\tborder-width:1px;\n\t\t\t}\n\n\t\t\t" +
            "td.right\n\t\t\t{\n\t\t\t\ttext-align:right;\n\t\t\t\tcolor:#346098;\n\t\t\t\t" +
            "font-weight:bold;\n\t\t\t\t" +
            "padding-right:10px;\n\t\t\t}\n\n\t\t\t" +
            "h1\n\t\t\t{\n\t\t\t\tfont-family:verdana;\n\t\t\t\tfont-size:13pt;\n\t\t\t\t" +
            "color:#346098;\n\t\t\t}\n\n\t\t\t" +
            "h2\n\t\t\t{\n\t\t\t\tfont-family:verdana;\n\t\t\t\tfont-size:12pt;\n\t\t\t\tcolor:#666666;" +
            "\n\t\t\t}\n\n\t\t\t" +
            "body\n\t\t\t{\n\t\t\t\tfont-family:verdana;\n\t\t\t\tfont-size:10pt;\n\t\t\t\t" +
            "background-color:#ffffff;\n\t\t\t}\n\n\t\t\t" +
            "body.menue\n\t\t\t{\n\t\t\t\tbackground-color:#a9cde8;\n\t\t\t}\n\n\t\t\t" +
            "a\n\t\t\t{\n\t\t\t\tfont-family:verdana;\n\t\t\t\ttext-decoration:none;\n\t\t\t\t" +
            "background-color:transparent;\n\t\t\t\t" +
            "font-size:10pt;\n\t\t\t}\n\n\t\t\t" +
            "a:link\n\t\t\t{\n\t\t\t\tcolor:#000000;\n\t\t\t}\n\n\t\t\t" +
            "a:visited\n\t\t\t{\n\t\t\t\tcolor:#000000;\n\t\t\t}\n\n\t\t\t" +
            "a:active\n\t\t\t{\n\t\t\t\tcolor:#000000;\n\t\t\t}\n\n\t\t\t" +
            "a:hover\n\t\t\t{\n\t\t\t\tcolor:#cc0000;\n\t\t\t}\n\n\t\t\t" +
            "a.top\n\t\t\t{\n\t\t\t\tfont-size:7pt;\n\t\t\t}\n\n\t\t"; // add more styles here

        private string CLOSEHEADER = "</style>\n\t</head>\n\n\t";

        private string frame =
            "<frameset cols=\"390,*\" framespacing=\"0\">\n\t\t" +
            "<frame src=\"data\\inhalt.html\" />\n\t\t" +
            "<frame src=\"data\\front.html\" name=\"content\" />\n\t</frameset>\n\n\t" +
            "<noframes><body>\n\t\tIhr Browser unterst&uuml;tzt leider keine Frames!<br />\n\t</body>" +
            "</noframes>\n</html>";

        private string createSongTable(ISong song)
        {
            string table = "<ul><table cellspacing=\"1\" cellpadding=\"2\" bgcolor=\"#aaaaaa\" width=\"800\">\n\t\t" +
                           "<tr>\n\t\t\t<td width=\"50\" bgcolor=\"dddddd\">\n\t\t\t\t<b>" + song.Number +
                           "</b>\n\t\t\t</td>\n\t\t\t<td width=\"710\" bgcolor=\"#dddddd\">" +
                           this.cleanText(song.Title, false) +
                           "</td>\n\t\t\t<td width=\"20\" bgcolor=\"#dddddd\"><a class=\"top\" href=\"" +
                           this.getBack(this.box, song) +
                           "\" target=\"content\">back</a></td>\n\t\t\t" +
                           "<td width=\"20\" bgcolor=\"#dddddd\"><a class=\"top\" href=\"" +
                           this.getNext(this.box, song) +
                           "\" target=\"content\">next</a></td>\n\t\t</tr>\n\t\t<tr>" +
                           "\n\t\t\t<td width=\"800\" style=\"padding:10px;\" colspan=\"4\" bgcolor=\"#f4f4f4\">" +
                           this.cleanText(song.Text, this.checkBox1.Checked) + "</td>\n\t\t</tr>\n\t" +
                           this.createTransTable(song.GetTransEnum()) + "</table></ul><br />\n\n\t";

            return table;
        }

        private string createTransTable(IDictionaryEnumerator translist)
        {
            translist.Reset();
            string res = "";
            while (translist.MoveNext())
            {
                ITranslation trans = (ITranslation) translist.Value;
                res += "<tr>\n\t\t\t<td></td><td colspan=\"3\" bgcolor=\"dddddd\">" + trans + "</td></tr>\n\t\t";
                res += "<tr>\n\t\t\t<td colspan=\"4\" style=\"padding:10px;\" bgcolor=\"#f4f4f4\">" +
                       this.cleanText(trans.Text, this.checkBox1.Checked) + "</td></tr>\n\t\t";
            }
            return res;
        }

        private string songstyle =
            "\tspan.special\n\t\t\t{\n\t\t\t\tcolor:#000000;\n\t\t\t\tfont-family:times;\n\t\t\t\tfont-style:italic;\n\t\t\t}\n\n\t\t\t" +
            "span.p8\n\t\t\t{\n\t\t\t\tpadding-left:16px;\n\t\t\t}\n\n\t\t\t" +
            "span.p16\n\t\t\t{\n\t\t\t\tpadding-left:32px;\n\t\t\t}\n\n\t\t\t" +
            "span.p24\n\t\t\t{\n\t\t\t\tpadding-left:48px;\n\t\t\t}\n\n\t\t\t" +
            "span.p32\n\t\t\t{\n\t\t\t\tpadding-left:64px;\n\t\t\t}\n\n\t\t\t" +
            "span.p40\n\t\t\t{\n\t\t\t\tpadding-left:80px;\n\t\t\t}\n\n\t\t\t" +
            "span.b\n\t\t\t{\n\t\t\t\tfont-weight:bold;\n\t\t\t}\n\n\t\t\t" +
            "span.i\n\t\t\t{\n\t\t\t\tfont-style:italic;\n\t\t\t}\n\n\t\t\t";


        // Clean Text
        private string cleanText(string text, bool format)
        {
            string clean = "";
            bool intag = false;
            for (int i = 0; i < text.Length; i++)
            {
                if (!intag)
                {
                    if (text[i] == '<')
                    {
                        intag = true;
                    }
                    else
                    {
                        if (text[i] == '\n')
                        {
                            clean += "<br />\n";
                        }
                        else if (text[i] == '\t')
                        {
                            clean += " ";
                        }
                        else if (text[i] == 'ä')
                        {
                            clean += "&auml;";
                        }
                        else if (text[i] == 'ö')
                        {
                            clean += "&ouml;";
                        }
                        else if (text[i] == 'ü')
                        {
                            clean += "&uuml;";
                        }
                        else if (text[i] == 'Ä')
                        {
                            clean += "&Auml;";
                        }
                        else if (text[i] == 'Ö')
                        {
                            clean += "&Ouml;";
                        }
                        else if (text[i] == 'Ü')
                        {
                            clean += "&Uuml;";
                        }
                        else
                        {
                            clean += text[i];
                        }
                    }
                }
                else
                {
                    int j = i;
                    while (text[j] != '>') j++;
                    if (format)
                    {
                        int taglength = j - i;
                        bool closing = false;
                        string tag = text.Substring(i, taglength);
                        if (tag[0] == '/')
                        {
                            closing = true;
                            tag = tag.Substring(1);
                        }

                        if (tag.Equals("special") || tag.Equals("refrain") || tag.Equals("b") ||
                            tag.Equals("i") || tag.StartsWith("p"))
                        {
                            if (closing)
                            {
                                clean += "</span>";
                                if (tag.Equals("refrain")) clean += "</span><br />";
                            }
                            else
                            {
                                clean += "<span class=\"" + tag + "\">";
                                if (tag.Equals("refrain"))
                                {
                                    clean += "<br />\n";
                                    if (Util.refmode)
                                    {
                                        clean += "<b>Refrain</b><br />\n";
                                    }
                                    clean += "<span class=\"p8\">";
                                }
                            }
                        }
                        i = j;
                    }
                    else
                    {
                        i = j;
                        clean += " ";
                    }
                    intag = false;
                }
            }
            return clean;
        }

        private string cleanXML(string text)
        {
            string clean = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '<')
                {
                    while (text[++i] != '>')
                    {
                    }
                }
                else
                {
                    clean += text[i];
                }
            }
            return clean;
        }

        // XML/XSLT mode
        private string DTD = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                             "<?xml-stylesheet type=\"text/xsl\" href=\"lyraxsl.xsl\"?>\n" +
                             "<!DOCTYPE lyra[\n" +
                             "  <!ELEMENT lyra (Song*)>\n" +
                             "  <!ELEMENT Song (Number,Title,Text,Translations) >\n" +
                             "  <!ELEMENT Translations (Translation*)>\n" +
                             "  <!ELEMENT Translation (Title,Text)>\n" +
                             "  <!ELEMENT Number (#PCDATA)>\n" +
                             "  <!ELEMENT Title (#PCDATA)>\n" +
                             "  <!ELEMENT Text (#PCDATA)>\n" +
                             "  <!ATTLIST Song id ID #REQUIRED>]>\n\n" +
                             "<lyra>\n</lyra>";

        private bool SaveXML()
        {
            try
            {
                this.XMLFile(this.box);
                this.XSLFile();
                this.HTMLFile();
                // logo-image
                File.Copy(Util.BASEURL + "\\doc\\logo_kl.jpg", this.textBox3.Text + "\\logo_kl.jpg", true);

                return true;
            }
            catch (Exception e)
            {
                Util.MBoxError(e.Message, e);
                return false;
            }
        }

        private void XSLFile()
        {
            StreamWriter writer = File.CreateText(this.textBox3.Text + "\\lyraxsl.xsl");
            writer.Write(this.XSL_1 + this.cleanXML(this.idtext) + this.XSL_2);
            writer.Close();
        }

        private void HTMLFile()
        {
            StreamWriter writer = File.CreateText(this.textBox2.Text);
            string html = "<html>\n\t" +
                          "<body>\n\t<script type=\"text/javascript\">// Load XML\n\t\t" +
                          "var xml = new ActiveXObject(\"Microsoft.XMLDOM\")\n\t\t" +
                          "xml.async = false\n\t\txml.load(\"data/lyradata.xml\")\n\n\t\t" +
                          "// Load XSL\n\t\tvar xsl = new ActiveXObject(\"Microsoft.XMLDOM\")\n\t\t" +
                          "xsl.async = false\n\t\txsl.load(\"data/lyraxsl.xsl\")\n\t\t" +
                          "// Transform\n\t\tdocument.write(xml.transformNode(xsl))</script>\n\t" +
                          "</body>\n</html>";
            writer.Write(html);
            writer.Close();
        }

        private void XMLFile(ListBox box)
        {
            // Create Directories
            Directory.CreateDirectory(this.textBox1.Text);
            Directory.CreateDirectory(this.textBox3.Text);

            // Create XML-File
            StreamWriter writer = File.CreateText(this.textBox3.Text + "\\lyradata.xml");
            writer.Write(this.DTD);
            writer.Close();

            XmlDocument doc = new XmlDocument();
            doc.Load(this.textBox3.Text + "\\lyradata.xml");
            XmlNode mainNode, curNode, nr, title, text, trans;
            XmlAttribute idattr;
            // create main node
            mainNode = doc.GetElementsByTagName("lyra")[0];

            IEnumerator en = box.Items.GetEnumerator();
            en.Reset();
            while (en.MoveNext())
            {
                ISong song = (ISong) en.Current;
                curNode = doc.CreateNode(XmlNodeType.Element, "Song", doc.NamespaceURI);

                idattr = doc.CreateAttribute("id", doc.NamespaceURI);
                nr = doc.CreateNode(XmlNodeType.Element, "Number", doc.NamespaceURI);
                title = doc.CreateNode(XmlNodeType.Element, "Title", doc.NamespaceURI);
                text = doc.CreateNode(XmlNodeType.Element, "Text", doc.NamespaceURI);
                trans = doc.CreateNode(XmlNodeType.Element, "Translations", doc.NamespaceURI);

                idattr.Value = song.ID;
                nr.InnerText = song.Number.ToString();
                title.InnerText = song.Title;
                text.InnerText = this.cleanXML(song.Text);

                curNode.Attributes.Append(idattr);
                curNode.AppendChild(nr);
                curNode.AppendChild(title);
                curNode.AppendChild(text);

                IDictionaryEnumerator transen = song.GetTransEnum();
                transen.Reset();

                XmlNode transmain, transtitle, transtext;
                while (transen.MoveNext())
                {
                    ITranslation curtrans = (ITranslation) transen.Value;
                    transmain = doc.CreateNode(XmlNodeType.Element, "Translation", doc.NamespaceURI);
                    transtitle = doc.CreateNode(XmlNodeType.Element, "Title", doc.NamespaceURI);
                    transtext = doc.CreateNode(XmlNodeType.Element, "Text", doc.NamespaceURI);

                    transtitle.InnerText = curtrans.ToString();
                    transtext.InnerText = this.cleanXML(curtrans.Text);

                    transmain.AppendChild(transtitle);
                    transmain.AppendChild(transtext);
                    trans.AppendChild(transmain);
                }
                if (trans.ChildNodes.Count != 0)
                {
                    curNode.AppendChild(trans);
                }
                mainNode.AppendChild(curNode);
            }
            doc.AppendChild(mainNode);
            doc.Save(this.textBox3.Text + "\\lyradata.xml");
        }


        // next
        private string getNext(ListBox box, ISong song)
        {
            int i = (box.Items.IndexOf(song) + 1)%box.Items.Count;
            return ((ISong) box.Items[i]).ID + ".html";
        }

        // back
        private string getBack(ListBox box, ISong song)
        {
            int i = (box.Items.IndexOf(song) + box.Items.Count - 1)%box.Items.Count;
            return ((ISong) box.Items[i]).ID + ".html";
        }

        // HTML mode
        private bool SaveHTML(ListBox box)
        {
            try
            {
                // Create Directories
                Directory.CreateDirectory(this.textBox1.Text);
                Directory.CreateDirectory(this.textBox3.Text);

                // Create Frame -> lyra.html
                StreamWriter writer = File.CreateText(this.textBox2.Text);
                writer.Write(this.HTMLHEADER + this.CLOSEHEADER + this.frame);
                writer.Close();

                // logo-image
                File.Copy(Util.BASEURL + "\\doc\\logo.jpg", this.textBox3.Text + "\\logo.jpg", true);
                // Create Links
                StreamWriter links = File.CreateText(this.textBox3.Text + "\\inhalt.html");
                links.Write(this.HTMLHEADER);
                links.Write("\tspan.smallgr\n\t\t\t{\n\t\t\t\tfont-size:8pt;\n\t\t\t\tcolor:#666666;\n\t\t\t}\n\n\t\t");
                links.Write("\ttd.over\n\t\t\t{\n\t\t\t\tcursor:pointer;\n\t\t\t\tbackground-color:#c3dff5;\n\t\t\t\t" +
                            "border-style:solid;\n\t\t\t\tborder-color:#888888;\n\t\t\t\tcolor:#000066\n\t\t\t}\n\n\t\t\t" +
                            "td.out\n\t\t\t{\n\t\t\t\tborder-width:1px;\n\t\t\t\tborder-style:solid;\n\t\t\t\t" +
                            "border-color:#a9cde8;\n\t\t\t}\n\t\t");

                links.Write(this.CLOSEHEADER);
                links.Write("<body class=\"menue\" topmargin=\"0\" rightmargin=\"0\">\n\n\t");
                links.Write(
                    "<a href=\"front.html\" target=\"content\"><img src=\"logo.jpg\" border=\"0\" alt=\"lyra HTML Liedtexte\"/></a>" +
                    "<br />\n\t<br />\n\t" + "<h1>Inhaltsverzeichnis</h1>\n\n\t");
                links.Write("<span class=\"smallgr\"><b>" + this.idtext + "</b></span><br />\n\t");
                links.Write("<span class=\"smallgr\">generated by lyra " + Util.GetDate() +
                            "</span><br />\n\t<br />\n\t<br />\n\t");
                links.Write("<table>\n\t\t");

                // Create Inhalt
                StreamWriter front = File.CreateText(this.textBox3.Text + "\\front.html");
                front.Write(this.HTMLHEADER + this.CLOSEHEADER);
                front.Write("<body>\n\n\t<h2>lyra Liedtexte (HTML)</h2>\n\t<br />\n\t<br />\n\t");
                string fronttxt = "HTML Dateien, generiert von lyra " + Util.VER + Util.HTMLNL;
                fronttxt += "Datum: " + Util.GetDate() + Util.HTMLNL + Util.HTMLNL + Util.HTMLNL;
                fronttxt += "Generiert aus:" + Util.HTMLNL + "<b>" + this.idtext + "</b>" + Util.HTMLNL + Util.HTMLNL +
                            "Beinhaltet ";
                switch (this.box.Items.Count)
                {
                    case 0:
                        fronttxt += "<b>keinen</b> Liedtext.";
                        break;
                    case 1:
                        fronttxt += "<b>einen</b> Liedtext.";
                        break;
                    default:
                        fronttxt += "<b>" + this.box.Items.Count + "</b> Liedtexte.";
                        break;
                }

                front.Write(fronttxt);
                front.Write(Util.HTMLNL + Util.HTMLNL + Util.HTMLNL);
                front.Write("</body>\n</html>");

                // all Songs
                IEnumerator en = box.Items.GetEnumerator();
                en.Reset();
                while (en.MoveNext())
                {
                    ISong song = (ISong) en.Current;
                    if (song != null)
                    {
                        StreamWriter songhtml = File.CreateText(this.textBox3.Text + "\\" + song.ID + ".html");
                        songhtml.Write(this.HTMLHEADER);
                        songhtml.Write(songstyle);

                        string refcolor = Util.hexValue(Util.REFCOLOR.R) +
                                          Util.hexValue(Util.REFCOLOR.G) +
                                          Util.hexValue(Util.REFCOLOR.B);

                        string refstyle = "span.refrain\n\t\t\t{\n\t\t\t\tcolor:#" + refcolor + ";\n\t\t\t\t";
                        if (!Util.refmode) refstyle += "font-weight:bold;\n\t\t\t";
                        refstyle += "\n\t\t\t}\n\n\t\t\t";

                        songhtml.Write(refstyle);
                        songhtml.Write(this.CLOSEHEADER);
                        songhtml.Write("<body>\n\n\t" + Util.HTMLNL + Util.HTMLNL + Util.HTMLNL +
                                       this.createSongTable(song));
                        songhtml.Write("</body>\n</html>");
                        songhtml.Close();
                        links.Write("<tr><td class=\"out\" onMouseOver=\"this.className='over'\" " +
                                    "onMouseOut=\"this.className='out'\" onClick=\"parent.frames['content'].location.href='" +
                                    song.ID +
                                    ".html'\">");
                        links.Write(this.cleanText(song.Number.ToString(), false) + "&nbsp;&nbsp;&nbsp;" +
                                    this.cleanText(song.Title, false) + "</td></tr>\n\t\t");
                    }
                }

                links.Write("\n\t</table>\n\t</body>\n</html>");

                front.Close();
                links.Close();
                return true;
            }
            catch (Exception e)
            {
                Util.MBoxError(e.Message, e); //"HTML konnte nicht erstellt werden!");
                return false;
            }
        }


        // XSLT
        private string XSL_1 =
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
            "<xsl:stylesheet version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\">\n\n" +
            "<xsl:template match=\"/\">\n" +
            "  <html>\n" +
            "  <head>\n" +
            "    <title>lyra Songliste</title>\n" +
            "    <meta name=\"DC.Title\" content=\"lyra Songliste\" />\n" +
            "    <meta name=\"DC.Date\" content=\"" + Util.GetDate() + "\" />\n" +
            "    <meta name=\"DC.Format\" content=\"text/xsl\" />\n" +
            "    <meta name=\"DC.Language\" content=\"de\" />\n" +
            "    <meta name=\"DC.Rights\" content=\"Alle Rechte dem Autor vorbehalten.\" />\n" +
            "    <meta name=\"generator\" content=\"lyra v1.1 html-generator\" />\n\n" +
            "    <style type=\"text/css\">\n" +
            "      td {font-family:verdana;\n" +
            "          font-size:10pt;\n" +
            "          color:#000000;\n" +
            "          text-align:left;\n" +
            "          border-width:1px;}\n\n" +
            "      td.right {text-align:right;\n" +
            "                color:#346098;\n" +
            "                font-weight: bold;\n" +
            "                padding-right:10px; }\n\n" +
            "      h1 {font-family:verdana;font-size:13pt;color:#346098;}\n" +
            "      h2 {font-family:verdana;font-size:12pt;color:#666666;}\n" +
            "          \n" +
            "      body {font-family:verdana; font-size: 10pt; background-color:#ffffff;}\n\n\n" +
            "      a         {font-family:verdana;\n" +
            "                 text-decoration:none;\n" +
            "                 background-color:transparent;\n" +
            "                 font-size:10pt; }\n" +
            "      a:link    {color:#000000;}\n" +
            "      a:visited {color:#000000;}\n" +
            "      a:active  {color:#cc0000;}\n" +
            "      a:hover   {color:#cc0000;}\n" +
            "      span.info {color:#AAAAAA;}\n" +
            "      \n" +
            "      a.top         {font-size:7pt; }\n\n\n" +
            "    </style>\n\n" +
            "  </head>\n" +
            "  \n" +
            "  <body>\n" +
            "    <img src=\"logo_kl.jpg\" hspace=\"15\" align=\"left\" />\n" +
            "    <br />\n" +
            "    <h1>lyra Songliste</h1>\n" +
            "    <span class=\"info\">" + Util.GetDate() + "<br />\n<br />\n";

        private string XSL_2 =
            "\n    </span><br />\n<br />\n" +
            "    <br clear=\"all\" />\n" +
            "    <a name=\"top\" />\n" +
            "    <h2>Inhaltsverzeichnis</h2>\n" +
            "    <ul>\n" +
            "    <table cellspacing=\"0\" cellpadding=\"0\">\n" +
            "    <xsl:for-each select=\"/lyra/Song\">\n" +
            "    <xsl:sort select=\"Number\" data-type=\"number\" />\n" +
            "      <tr>\n" +
            "        <td class=\"right\"><xsl:value-of select=\"Number\"/></td>\n" +
            "        <td>\n" +
            "	  		<script type=\"text/javascript\">\n" +
            "          document.write(\"&lt;a href=\\\"#<xsl:value-of select=\"@id\"/>\\\"&gt;\");\n" +
            " 					document.write(\"<xsl:value-of select=\"Title\"/>\");\n" +
            "          document.write(\"&lt;/a&gt;\");\n" +
            "        </script>\n" +
            "        </td>\n" +
            "      </tr>\n" +
            "    </xsl:for-each>\n" +
            "    </table>\n" +
            "    </ul>\n" +
            "    <br />\n" +
            "    <h2>Liedtexte</h2>    \n" +
            "    <ul>\n" +
            "    <xsl:for-each select=\"/lyra/Song\">\n" +
            "    <xsl:sort select=\"Number\" data-type=\"number\" />\n" +
            "    <table cellspacing=\"1\" cellpadding=\"2\" bgcolor=\"#aaaaaa\" width=\"800\">\n" +
            "    <tr>\n" +
            "      <td width=\"50\" bgcolor=\"dddddd\">\n" +
            "        <script type=\"text/javascript\">\n" +
            "          document.write(\"&lt;a name=\\\"<xsl:value-of select=\"@id\"/>\\\"/&gt;\");\n" +
            "        </script>\n" +
            "        <b><xsl:value-of select=\"Number\"/></b>\n" +
            "      </td>\n" +
            "      <td width=\"734\" bgcolor=\"#dddddd\"><xsl:value-of select=\"Title\"/></td>\n" +
            "     <td width=\"16\" bgcolor=\"#dddddd\"><a class=\"top\" href=\"#top\">top</a></td>\n" +
            "    </tr>\n" +
            "    <tr>\n" +
            "      <td width=\"800\" colspan=\"3\" bgcolor=\"#f4f4f4\"><xsl:value-of select=\"Text\" /></td>\n" +
            "    </tr>\n" +
            "    <xsl:for-each select=\"Translations/Translation\">\n" +
            "    <tr>\n" +
            "    	<td width=\"50\" bgcolor=\"#f4f4f4\"></td>\n" +
            "    	<td width=\"750\" colspan=\"2\" bgcolor=\"#dddddd\">\n" +
            "    		<xsl:value-of select=\"Title\" />\n" +
            "    	</td>\n" +
            "    </tr>\n" +
            "    <tr>\n" +
            "    	<td colspan=\"3\" bgcolor=\"#f4f4f4\">\n" +
            "    		<xsl:value-of select=\"Text\" />\n" +
            "    	</td>\n" +
            "    </tr>\n" +
            "    </xsl:for-each>\n" +
            "    </table><br />\n" +
            "    </xsl:for-each>\n" +
            "    </ul>\n" +
            "    \n" +
            "  </body>\n" +
            "  </html>\n" +
            "</xsl:template>\n" +
            "\n" +
            "</xsl:stylesheet>\n";
    }
}