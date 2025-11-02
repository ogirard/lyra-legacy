using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
    /// <summary>
    /// Zusammendfassende Beschreibung für TEditor.
    /// </summary>
    public class TEditor : Form
    {
        private Label label4;
        private Label label2;
        private Label label1;
        private Panel panel1;
        private LyraButtonControl button9;
        private Label label6;
        private ComboBox comboBox1;
        private LyraButtonControl button8;
        private LyraButtonControl button7;
        private LyraButtonControl button6;
        private LyraButtonControl button5;
        private LyraButtonControl button4;
        private Label label5;
        private LyraButtonControl button3;
        private RichTextBox richTextBox1;
        private Label label3;
        private TextBox textBox2;
        private TextBox textBox1;
        private LyraButtonControl button2;
        private LyraButtonControl button1;
        private ComboBox comboBox2;
        private Label label7;

        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private Container components = null;

        private ISong song = null;
        private ITranslation trans = null;
        private ListBox lb = null;
        private GUI owner;

        public static bool TEditorOpen = false;
        private CheckBox checkBox1;
        public static TEditor tEditor = null;

        public TEditor(ListBox lb, ISong song, GUI owner) : this(lb, song, owner, null)
        {
        }

        public TEditor(ListBox lb, ISong song, GUI owner, ITranslation trans)
        {
            //
            // Erforderlich für die Windows Form-Designerunterstützung
            //
            InitializeComponent();
            AcceptButton = button1;
            this.song = song;
            this.trans = trans;
            this.lb = lb;
            this.owner = owner;
            textBox2.Text = this.song.Number.ToString();
            textBox2.Enabled = false;
            tEditor = this;
            TEditorOpen = true;
            if (this.trans != null)
            {
                textBox1.Text = this.trans.Title;
                richTextBox1.Text = this.trans.Text;
                checkBox1.Checked = this.trans.Unformatted;
                panel1.Enabled = !this.trans.Unformatted;
            }
            else
            {
                textBox1.Text = "";
                richTextBox1.Text = "";
            }
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
            TEditorOpen = false;
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button9 = new LyraButtonControl();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button8 = new LyraButtonControl();
            this.button7 = new LyraButtonControl();
            this.button6 = new LyraButtonControl();
            this.button5 = new LyraButtonControl();
            this.button4 = new LyraButtonControl();
            this.label5 = new System.Windows.Forms.Label();
            this.button3 = new LyraButtonControl();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new LyraButtonControl();
            this.button1 = new LyraButtonControl();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font =
                new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold,
                                        System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
            this.label4.Location = new System.Drawing.Point(24, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 16);
            this.label4.TabIndex = 19;
            this.label4.Text = "Text:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font =
                new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold,
                                        System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
            this.label2.Location = new System.Drawing.Point(24, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 16);
            this.label2.TabIndex = 17;
            this.label2.Text = "Liednummer:";
            // 
            // label1
            // 
            this.label1.Font =
                new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold,
                                        System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
            this.label1.ForeColor = System.Drawing.Color.SlateGray;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(216, 24);
            this.label1.TabIndex = 15;
            this.label1.Text = "Lyra Übersetzung Editor";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button9);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.button8);
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Location = new System.Drawing.Point(640, 96);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(80, 232);
            this.panel1.TabIndex = 20;
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.LightSteelBlue;
            this.button9.Location = new System.Drawing.Point(7, 141);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(64, 24);
            this.button9.TabIndex = 9;
            this.button9.Text = "Tab";
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 115);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 16);
            this.label6.TabIndex = 8;
            this.label6.Text = "um";
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.LightGray;
            this.comboBox1.Font =
                new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular,
                                        System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
            this.comboBox1.Items.AddRange(new object[]
                                              {
                                                  "8",
                                                  "16",
                                                  "24",
                                                  "32",
                                                  "40"
                                              });
            this.comboBox1.Location = new System.Drawing.Point(32, 112);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(40, 21);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.Text = "8";
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.LightSteelBlue;
            this.button8.Location = new System.Drawing.Point(7, 86);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(64, 24);
            this.button8.TabIndex = 6;
            this.button8.Text = "Einrücken";
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.LightSteelBlue;
            this.button7.Location = new System.Drawing.Point(7, 54);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(64, 24);
            this.button7.TabIndex = 5;
            this.button7.Text = "Spezial";
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.LightSteelBlue;
            this.button6.Font =
                new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic,
                                        System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
            this.button6.Location = new System.Drawing.Point(47, 173);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(24, 24);
            this.button6.TabIndex = 4;
            this.button6.Text = "K";
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.LightSteelBlue;
            this.button5.Font =
                new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold,
                                        System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
            this.button5.Location = new System.Drawing.Point(7, 173);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(24, 24);
            this.button5.TabIndex = 3;
            this.button5.Text = "F";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.LightSteelBlue;
            this.button4.Font =
                new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular,
                                        System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
            this.button4.Location = new System.Drawing.Point(56, 208);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(22, 22);
            this.button4.TabIndex = 2;
            this.button4.Text = "«";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font =
                new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold,
                                        System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
            this.label5.ForeColor = System.Drawing.Color.SlateGray;
            this.label5.Location = new System.Drawing.Point(2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 17);
            this.label5.TabIndex = 1;
            this.label5.Text = "Format";
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.LightSteelBlue;
            this.button3.Location = new System.Drawing.Point(7, 22);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(64, 24);
            this.button3.TabIndex = 0;
            this.button3.Text = "Refrain";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(24, 96);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(600, 232);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "richTextBox1";
            // 
            // label3
            // 
            this.label3.Font =
                new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold,
                                        System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
            this.label3.Location = new System.Drawing.Point(160, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 16);
            this.label3.TabIndex = 18;
            this.label3.Text = "Titel:";
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(104, 40);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(40, 20);
            this.textBox2.TabIndex = 0;
            this.textBox2.Text = "textBox2";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(200, 40);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(424, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "textBox1";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.Control;
            this.button2.Location = new System.Drawing.Point(608, 344);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(72, 24);
            this.button2.TabIndex = 16;
            this.button2.Text = "Abbrechen";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(688, 344);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 24);
            this.button1.TabIndex = 14;
            this.button1.Text = "Ok";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.Items.AddRange(new object[]
                                              {
                                                  "english",
                                                  "français",
                                                  "italiano",
                                                  "andere"
                                              });
            this.comboBox2.Location = new System.Drawing.Point(536, 64);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(88, 21);
            this.comboBox2.TabIndex = 21;
            this.comboBox2.Text = "english";
            // 
            // label7
            // 
            this.label7.Font =
                new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold,
                                        System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
            this.label7.Location = new System.Drawing.Point(480, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 16);
            this.label7.TabIndex = 22;
            this.label7.Text = "Sprache:";
            // 
            // checkBox1
            // 
            this.checkBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.checkBox1.Location = new System.Drawing.Point(200, 68);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(125, 16);
            this.checkBox1.TabIndex = 23;
            this.checkBox1.Text = "wird nicht gesungen";
            this.checkBox1.Click += new System.EventHandler(this.checkBox1_Click);
            // 
            // TEditor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(734, 375);
            this.ControlBox = false;
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TEditor";
            this.ShowInTaskbar = false;
            this.Text = "Übersetzung";
            this.Load += new System.EventHandler(this.TEditor_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        // abbrechen
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        // ok
        private void button1_Click(object sender, EventArgs e)
        {
            if (trans != null)
            {
                trans.Title = textBox1.Text;
                trans.Text = richTextBox1.Text;
                trans.Language = comboBox2.SelectedIndex;
                trans.Unformatted = checkBox1.Checked;
            }
            else
            {
                trans =
                    new Translation(textBox1.Text, richTextBox1.Text, comboBox2.SelectedIndex,
                                    checkBox1.Checked, PhysicalXml.HighestTrID, true);
                song.AddTranslation(trans);
            }
            song.ShowTranslations(lb);
            song.RefreshTransMenu();
            song.Update();
            owner.ToUpdate(true);
            Close();
        }

        private void TEditor_Load(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            for (var i = 0; i < Util.LangNR; i++)
            {
                comboBox2.Items.Add(Util.getLanguageString(i, false));
            }
            comboBox2.SelectedIndex = 0;
            if (trans != null)
            {
                comboBox2.SelectedIndex = trans.Language;
            }
        }


        // clean Formats
        private string cleanFormat(string toclean)
        {
            var clean = "";
            for (var i = 0; i < toclean.Length; i++)
            {
                if (toclean[i] == '<')
                {
                    while (i++ < toclean.Length && toclean[i] != '>')
                    {
                    }
                }
                else
                {
                    clean += toclean[i];
                }
            }
            return clean;
        }

        // Format

        // Refrain
        private void button3_Click(object sender, EventArgs e)
        {
            format(Util.REF, true);
        }

        // bold
        private void button5_Click(object sender, EventArgs e)
        {
            format(Util.BOLD, false);
        }

        // italic
        private void button6_Click(object sender, EventArgs e)
        {
            format(Util.ITALIC, false);
        }

        // special
        private void button7_Click(object sender, EventArgs e)
        {
            format(Util.SPEC, false);
        }

        // einrücken
        private void button8_Click(object sender, EventArgs e)
        {
            format(Util.BLOCK + comboBox1.SelectedItem.ToString(), true);
        }

        // tab
        private void button9_Click(object sender, EventArgs e)
        {
            var pos = richTextBox1.SelectionStart;
            richTextBox1.Text = richTextBox1.Text.Insert(pos, '\t'.ToString());
            richTextBox1.Focus();
            richTextBox1.Select(pos + 1, 0);
        }

        // undo
        private void button4_Click(object sender, EventArgs e)
        {
            if (undo != "")
            {
                richTextBox1.Rtf = undo;
                undo = "";
            }
            else if (richTextBox1.CanUndo)
            {
                richTextBox1.Undo();
            }
        }

        private string undo = "";

        private void format(string tag, bool nl)
        {
            button4.Enabled = true;
            button4.BackColor = Color.LightSteelBlue;
            undo = richTextBox1.Rtf;
            var left = richTextBox1.SelectionStart;
            var right = richTextBox1.SelectionLength + left + tag.Length + 2;
            richTextBox1.Text = richTextBox1.Text.Insert(left, "<" + tag + ">");
            richTextBox1.Text = richTextBox1.Text.Insert(right, "</" + tag + ">");
            if (nl)
            {
                if (((right + tag.Length + 3) < richTextBox1.Text.Length) &&
                    (richTextBox1.Text[right + tag.Length + 3] != '\n'))
                {
                    richTextBox1.Text = richTextBox1.Text.Insert(right + tag.Length + 3, "\n");
                }
                if ((left > 0) && (richTextBox1.Text[left - 1] != '\n'))
                {
                    richTextBox1.Text = richTextBox1.Text.Insert(left, "\n");
                    right++;
                }
            }
            richTextBox1.Focus();
            richTextBox1.Select(right, 0);
        }

        // unformatted
        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (MessageBox.Show(this, "Achtung!" + Util.NL + "Alle Formatierungen gehen verloren.",
                                    "lyra", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    richTextBox1.Text = cleanFormat(richTextBox1.Text);
                    panel1.Enabled = false;
                }
                else
                {
                    checkBox1.Checked = false;
                }
            }
            else
            {
                panel1.Enabled = true;
            }
        }
    }
}