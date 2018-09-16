using System;
using System.Drawing;
using System.Windows.Forms;


namespace Lyra2.LyraShell
{
	/// <summary>
	/// Zusammendfassende Beschreibung für Options.
	/// </summary>
	public class Options : Form
	{
	    private readonly IStorage storage;
	    private Label label1;
		private TabControl tabControl1;
		private TabPage tabPage1;
		private TabPage tabPage2;
		private LyraButtonControl button1;
		private LyraButtonControl button2;
		private LyraButtonControl button3;

		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private static Options options = null;
		private RichTextBox richTextBox1;
		private RichTextBox richTextBox2;
		private LyraButtonControl button4;
		private LyraButtonControl button5;
		private Label label2;
		private Label label3;
		private LyraButtonControl button6;
		private RadioButton radioButton1;
		private RadioButton radioButton2;
		private Panel panel1;
		private CheckBox checkBox1;
		private CheckBox checkBox2;
		private Label label4;
		private CheckBox checkBox3;
		private LyraButtonControl button7;
		private CheckBox checkBox4;
		private Label label5;
		private Label label6;
		private Label label7;
		private LinkLabel linkLabel1;
		private LyraButtonControl button8;
		private RichTextBox richTextBox3;
		private CheckBox checkBox5;
		private NumericUpDown numericUpDown1;
		private Label label8;
		private TabPage tabPage3;
		private RadioButton radioButton3;
		private Button button9;
		private Panel panel2;
		private LyraButtonControl button10;
		private Panel panel3;
		private LyraButtonControl button11;
		private Panel panel4;
		private RadioButton radioButton4;
		private RadioButton radioButton5;
		private LyraButtonControl button12;
		private Panel panel5;
		private RadioButton radioButton6;
		private LyraButtonControl button13;
		private PictureBox pictureBox1;
		private Label label9;
		private TabPage tabPage4;
		private CheckBox checkBox6;
		private bool changed = false;
		private Button button14;
		private Button button15;
		private Button button16;
		private Button button17;
		private Button button18;
		private Button button19;
		private Label label10;
		private Label label11;
		private Label label12;
		private Label label13;
		private Label label14;
		private Label label15;
		private Label label16;
		private Label label17;
		private PictureBox pictureBox2;
		private PictureBox pictureBox3;
		private PictureBox pictureBox4;
		private PictureBox pictureBox5;
		private PictureBox pictureBox6;
		private PictureBox pictureBox7;
		private Panel panel6;
		private Panel panel7;
		private Panel panel8;
		private Panel panel9;
		private Panel panel10;
		private Panel panel11;
		private PictureBox pictureBox8;
        private Label label18;
        private ComboBox comboStyle;
		private static int REMOPENTAB = 0;

		public static void ShowOptions(IStorage storage)
		{
			if (options == null)
			{
                options = new Options(storage);
				options.button2.Enabled = false;
				options.checkBox1.Enabled = false;
				options.Show();
			}
			else
			{
				options.Focus();
			}
		}

        private Options(IStorage storage)
		{
            this.storage = storage;
		    this.InitializeComponent();
			this.richTextBox1.Font = Util.FONT;
			this.richTextBox2.Font = Util.SPECFONT;
			this.richTextBox3.Font = Util.TRANSFONT;
			this.checkBox1.Checked = Util.NOCOMMIT;
			this.checkBox2.Checked = Util.SHOWBUILDNEWS;
			this.checkBox3.Checked = Util.SHOWRIGHT;
			this.checkBox4.Checked = Util.SHOWGER;
			this.checkBox5.Checked = Util.SHOWNR;
			this.checkBox1.Enabled = false;
			this.numericUpDown1.Value = Util.TIMER;
			this.refresh();
			this.fd = new FontDialog();
			this.fd.FontMustExist = true;
			this.fd.ShowEffects = false;
			this.fd.ShowApply = false;
			
			this.AcceptButton = this.button1;

			this.cd = new ColorDialog();
			this.panel1.BackColor = Util.REFCOLOR;
			this.checkBox1.Enabled = Pswd.OK;

            this.comboStyle.BeginUpdate();
            foreach (var style in this.storage.Styles)
            {
                this.comboStyle.Items.Add(style);
                if (style.IsDefault)
                {
                    this.comboStyle.SelectedItem = style;
                }
            }
            this.comboStyle.EndUpdate();

			if (Util.refmode)
				this.radioButton2.Checked = true;
			else
				this.radioButton1.Checked = true;

			this.panel3.BackColor = Util.UNICOLOR;
			this.panel4.BackColor = Util.GRADCOL2;
			this.panel5.BackColor = Util.GRADCOL1;
			if (Util.BGIMG != null)
			{
				this.pictureBox1.Image = Util.stretchProportional(Util.BGIMG, this.pictureBox1.Size);
			}
			this.radioButton4.Checked = false;
			this.radioButton5.Checked = false;
			this.radioButton6.Checked = false;
			switch (Util.PREMODE)
			{
				case 0:
					this.radioButton4.Checked = true;
					break;
				case 1:
					this.radioButton5.Checked = true;
					break;
				case 2:
					this.radioButton6.Checked = true;
					break;
			}
			this.checkBox6.Checked = Util.CASCADEPIC;
			this.tabControl1.SelectedIndex = REMOPENTAB;
			// FX
			this.setFilename(Util.FX[0], this.label12, this.pictureBox2);
			this.setFilename(Util.FX[1], this.label13, this.pictureBox3);
			this.setFilename(Util.FX[2], this.label14, this.pictureBox4);
			this.setFilename(Util.FX[3], this.label15, this.pictureBox5);
			this.setFilename(Util.FX[4], this.label16, this.pictureBox6);
			this.setFilename(Util.FX[5], this.label17, this.pictureBox7);

		}

		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			REMOPENTAB = this.tabControl1.SelectedIndex;
			options = null;

			if (disposing)
			{
				if (this.components != null)
				{
				    this.components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.panel10 = new System.Windows.Forms.Panel();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.label16 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.label14 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.panel11 = new System.Windows.Forms.Panel();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.label17 = new System.Windows.Forms.Label();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.button9 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label18 = new System.Windows.Forms.Label();
            this.comboStyle = new System.Windows.Forms.ComboBox();
            this.button3 = new Lyra2.LyraShell.LyraButtonControl();
            this.button2 = new Lyra2.LyraShell.LyraButtonControl();
            this.button1 = new Lyra2.LyraShell.LyraButtonControl();
            this.button8 = new Lyra2.LyraShell.LyraButtonControl();
            this.button6 = new Lyra2.LyraShell.LyraButtonControl();
            this.button5 = new Lyra2.LyraShell.LyraButtonControl();
            this.button4 = new Lyra2.LyraShell.LyraButtonControl();
            this.button13 = new Lyra2.LyraShell.LyraButtonControl();
            this.button12 = new Lyra2.LyraShell.LyraButtonControl();
            this.button11 = new Lyra2.LyraShell.LyraButtonControl();
            this.button10 = new Lyra2.LyraShell.LyraButtonControl();
            this.button7 = new Lyra2.LyraShell.LyraButtonControl();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.panel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SlateGray;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Lyra Optionen";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(0, 32);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(388, 310);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.comboStyle);
            this.tabPage2.Controls.Add(this.button8);
            this.tabPage2.Controls.Add(this.richTextBox3);
            this.tabPage2.Controls.Add(this.radioButton2);
            this.tabPage2.Controls.Add(this.radioButton1);
            this.tabPage2.Controls.Add(this.button6);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.button5);
            this.tabPage2.Controls.Add(this.button4);
            this.tabPage2.Controls.Add(this.richTextBox2);
            this.tabPage2.Controls.Add(this.richTextBox1);
            this.tabPage2.Controls.Add(this.label18);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(380, 284);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Anzeige";
            // 
            // richTextBox3
            // 
            this.richTextBox3.AutoSize = true;
            this.richTextBox3.BackColor = System.Drawing.Color.White;
            this.richTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.richTextBox3.Location = new System.Drawing.Point(84, 119);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.ReadOnly = true;
            this.richTextBox3.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox3.Size = new System.Drawing.Size(288, 15);
            this.richTextBox3.TabIndex = 11;
            this.richTextBox3.Text = "nicht gesungene Texte";
            // 
            // radioButton2
            // 
            this.radioButton2.Checked = true;
            this.radioButton2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.radioButton2.ForeColor = System.Drawing.Color.SaddleBrown;
            this.radioButton2.Location = new System.Drawing.Point(169, 179);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(191, 16);
            this.radioButton2.TabIndex = 10;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "standard, mit \"Refrain\" markiert";
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.radioButton1.Location = new System.Drawing.Point(169, 195);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(40, 16);
            this.radioButton1.TabIndex = 9;
            this.radioButton1.Text = "fett";
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Brown;
            this.panel1.Location = new System.Drawing.Point(112, 179);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(40, 24);
            this.panel1.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(8, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(360, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Refrain: (basiert auf Standardschriftart)";
            // 
            // richTextBox2
            // 
            this.richTextBox2.AutoSize = true;
            this.richTextBox2.BackColor = System.Drawing.Color.White;
            this.richTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox2.Cursor = System.Windows.Forms.Cursors.Default;
            this.richTextBox2.Location = new System.Drawing.Point(84, 95);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox2.Size = new System.Drawing.Size(288, 12);
            this.richTextBox2.TabIndex = 2;
            this.richTextBox2.Text = "Spezial-Schriftart";
            // 
            // richTextBox1
            // 
            this.richTextBox1.AutoSize = true;
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.richTextBox1.Location = new System.Drawing.Point(84, 71);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1.Size = new System.Drawing.Size(288, 12);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "Standardschriftart";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(8, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(259, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "Standardeinstellungen für neue Styles :";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.checkBox6);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.pictureBox1);
            this.tabPage3.Controls.Add(this.radioButton6);
            this.tabPage3.Controls.Add(this.radioButton5);
            this.tabPage3.Controls.Add(this.panel5);
            this.tabPage3.Controls.Add(this.radioButton4);
            this.tabPage3.Controls.Add(this.panel4);
            this.tabPage3.Controls.Add(this.panel3);
            this.tabPage3.Controls.Add(this.pictureBox8);
            this.tabPage3.Controls.Add(this.button13);
            this.tabPage3.Controls.Add(this.button12);
            this.tabPage3.Controls.Add(this.button11);
            this.tabPage3.Controls.Add(this.button10);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(380, 284);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Präsentationsmodus";
            // 
            // checkBox6
            // 
            this.checkBox6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.checkBox6.ForeColor = System.Drawing.Color.DimGray;
            this.checkBox6.Location = new System.Drawing.Point(40, 192);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(80, 16);
            this.checkBox6.TabIndex = 24;
            this.checkBox6.Text = "kacheln";
            this.checkBox6.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.DimGray;
            this.label9.Location = new System.Drawing.Point(187, 100);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(16, 16);
            this.label9.TabIndex = 23;
            this.label9.Text = ">";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(155, 164);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(112, 81);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 22;
            this.pictureBox1.TabStop = false;
            // 
            // radioButton6
            // 
            this.radioButton6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.radioButton6.ForeColor = System.Drawing.Color.DimGray;
            this.radioButton6.Location = new System.Drawing.Point(16, 136);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(328, 16);
            this.radioButton6.TabIndex = 21;
            this.radioButton6.Text = "Bild als Hintergrund verwenden";
            this.radioButton6.CheckedChanged += new System.EventHandler(this.presMode_CheckedChanged);
            // 
            // radioButton5
            // 
            this.radioButton5.Checked = true;
            this.radioButton5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.radioButton5.ForeColor = System.Drawing.Color.DimGray;
            this.radioButton5.Location = new System.Drawing.Point(16, 72);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(328, 16);
            this.radioButton5.TabIndex = 19;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "Vertikaler Verlauf als Hintergrund verwenden";
            this.radioButton5.CheckedChanged += new System.EventHandler(this.presMode_CheckedChanged);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.LightBlue;
            this.panel5.Location = new System.Drawing.Point(136, 96);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(40, 24);
            this.panel5.TabIndex = 17;
            // 
            // radioButton4
            // 
            this.radioButton4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.radioButton4.ForeColor = System.Drawing.Color.DimGray;
            this.radioButton4.Location = new System.Drawing.Point(16, 8);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(328, 16);
            this.radioButton4.TabIndex = 16;
            this.radioButton4.Text = "Hintergrundfarbe verwenden";
            this.radioButton4.CheckedChanged += new System.EventHandler(this.presMode_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Navy;
            this.panel4.Location = new System.Drawing.Point(304, 96);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(40, 24);
            this.panel4.TabIndex = 14;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Gainsboro;
            this.panel3.Location = new System.Drawing.Point(136, 32);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(40, 24);
            this.panel3.TabIndex = 12;
            // 
            // pictureBox8
            // 
            this.pictureBox8.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox8.Image")));
            this.pictureBox8.Location = new System.Drawing.Point(144, 152);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(144, 120);
            this.pictureBox8.TabIndex = 25;
            this.pictureBox8.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.numericUpDown1);
            this.tabPage1.Controls.Add(this.checkBox5);
            this.tabPage1.Controls.Add(this.linkLabel1);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.checkBox4);
            this.tabPage1.Controls.Add(this.checkBox3);
            this.tabPage1.Controls.Add(this.checkBox2);
            this.tabPage1.Controls.Add(this.checkBox1);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.button7);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(380, 284);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Allgemein";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(304, 162);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 24);
            this.label8.TabIndex = 20;
            this.label8.Text = "Sekunde(n).";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(266, 160);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(40, 20);
            this.numericUpDown1.TabIndex = 19;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // checkBox5
            // 
            this.checkBox5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.checkBox5.Location = new System.Drawing.Point(16, 151);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(296, 37);
            this.checkBox5.TabIndex = 18;
            this.checkBox5.Text = "Nummer bei SongView zuerst gross anzeigen.";
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
            // 
            // linkLabel1
            // 
            this.linkLabel1.ActiveLinkColor = System.Drawing.Color.SaddleBrown;
            this.linkLabel1.LinkColor = System.Drawing.Color.Navy;
            this.linkLabel1.Location = new System.Drawing.Point(61, 41);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(24, 16);
            this.linkLabel1.TabIndex = 17;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Info";
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Navy;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.Color.SaddleBrown;
            this.label7.Location = new System.Drawing.Point(16, 248);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(272, 32);
            this.label7.TabIndex = 16;
            this.label7.Text = "Diese Pfade werden vorerst nicht für manuelle Einstellungen freigegeben.";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 216);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(296, 32);
            this.label6.TabIndex = 15;
            this.label6.Text = "Basis XML-Datei (curtext.xml) finden Sie im Verzeichnis {lyra}\\data\\, die Dokumen" +
    "tation in {lyra}\\doc.";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.DimGray;
            this.label5.Location = new System.Drawing.Point(8, 192);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 24);
            this.label5.TabIndex = 14;
            this.label5.Text = "Pfade:";
            // 
            // checkBox4
            // 
            this.checkBox4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.checkBox4.Location = new System.Drawing.Point(16, 118);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(240, 37);
            this.checkBox4.TabIndex = 13;
            this.checkBox4.Text = "bei Vollbildanzeige von Übersetzungen deutschen Titel auch einblenden";
            this.checkBox4.Click += new System.EventHandler(this.checkBox4_Click);
            // 
            // checkBox3
            // 
            this.checkBox3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.checkBox3.Location = new System.Drawing.Point(17, 96);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(320, 16);
            this.checkBox3.TabIndex = 12;
            this.checkBox3.Text = "rechte Ansicht standardmässig verwenden";
            this.checkBox3.Click += new System.EventHandler(this.checkBox3_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.checkBox2.Location = new System.Drawing.Point(17, 40);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(320, 16);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "zeige         beim Start an";
            this.checkBox2.Click += new System.EventHandler(this.checkBox2_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Enabled = false;
            this.checkBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.ForeColor = System.Drawing.Color.SaddleBrown;
            this.checkBox1.Location = new System.Drawing.Point(17, 16);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(241, 18);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "keine Änderungen zulassen!";
            this.checkBox1.Click += new System.EventHandler(this.checkBox1_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DimGray;
            this.label4.Location = new System.Drawing.Point(8, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 24);
            this.label4.TabIndex = 11;
            this.label4.Text = "View:";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.panel10);
            this.tabPage4.Controls.Add(this.panel7);
            this.tabPage4.Controls.Add(this.panel6);
            this.tabPage4.Controls.Add(this.button14);
            this.tabPage4.Controls.Add(this.button15);
            this.tabPage4.Controls.Add(this.button16);
            this.tabPage4.Controls.Add(this.button17);
            this.tabPage4.Controls.Add(this.button18);
            this.tabPage4.Controls.Add(this.button19);
            this.tabPage4.Controls.Add(this.label11);
            this.tabPage4.Controls.Add(this.label10);
            this.tabPage4.Controls.Add(this.panel8);
            this.tabPage4.Controls.Add(this.panel9);
            this.tabPage4.Controls.Add(this.panel11);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(380, 284);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Spezialfunktionen";
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel10.Controls.Add(this.pictureBox6);
            this.panel10.Controls.Add(this.label16);
            this.panel10.Location = new System.Drawing.Point(56, 202);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(312, 32);
            this.panel10.TabIndex = 23;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox6.Image")));
            this.pictureBox6.Location = new System.Drawing.Point(2, 3);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(24, 24);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox6.TabIndex = 20;
            this.pictureBox6.TabStop = false;
            // 
            // label16
            // 
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.DimGray;
            this.label16.Location = new System.Drawing.Point(32, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(280, 30);
            this.label16.TabIndex = 19;
            this.label16.Text = "-";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.label13);
            this.panel7.Controls.Add(this.pictureBox3);
            this.panel7.Location = new System.Drawing.Point(56, 94);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(312, 32);
            this.panel7.TabIndex = 22;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.DimGray;
            this.label13.Location = new System.Drawing.Point(32, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(280, 30);
            this.label13.TabIndex = 19;
            this.label13.Text = "-";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(2, 3);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(24, 24);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox3.TabIndex = 20;
            this.pictureBox3.TabStop = false;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.label12);
            this.panel6.Controls.Add(this.pictureBox2);
            this.panel6.Location = new System.Drawing.Point(56, 58);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(312, 32);
            this.panel6.TabIndex = 21;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.DimGray;
            this.label12.Location = new System.Drawing.Point(32, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(280, 30);
            this.label12.TabIndex = 19;
            this.label12.Text = "powerpoint.ppt";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(2, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(24, 24);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 20;
            this.pictureBox2.TabStop = false;
            // 
            // button14
            // 
            this.button14.BackColor = System.Drawing.Color.DarkGray;
            this.button14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button14.Location = new System.Drawing.Point(16, 58);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(32, 32);
            this.button14.TabIndex = 0;
            this.button14.Text = " F3";
            this.button14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button14.UseVisualStyleBackColor = false;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button15
            // 
            this.button15.BackColor = System.Drawing.Color.DarkGray;
            this.button15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button15.Location = new System.Drawing.Point(16, 94);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(32, 32);
            this.button15.TabIndex = 0;
            this.button15.Text = " F4";
            this.button15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button15.UseVisualStyleBackColor = false;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button16
            // 
            this.button16.BackColor = System.Drawing.Color.DarkGray;
            this.button16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button16.Location = new System.Drawing.Point(16, 130);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(32, 32);
            this.button16.TabIndex = 0;
            this.button16.Text = " F5";
            this.button16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button16.UseVisualStyleBackColor = false;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // button17
            // 
            this.button17.BackColor = System.Drawing.Color.DarkGray;
            this.button17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button17.Location = new System.Drawing.Point(16, 166);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(32, 32);
            this.button17.TabIndex = 0;
            this.button17.Text = " F6";
            this.button17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button17.UseVisualStyleBackColor = false;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // button18
            // 
            this.button18.BackColor = System.Drawing.Color.DarkGray;
            this.button18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button18.Location = new System.Drawing.Point(16, 202);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(32, 32);
            this.button18.TabIndex = 0;
            this.button18.Text = " F7";
            this.button18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button18.UseVisualStyleBackColor = false;
            this.button18.Click += new System.EventHandler(this.button18_Click);
            // 
            // button19
            // 
            this.button19.BackColor = System.Drawing.Color.DarkGray;
            this.button19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button19.Location = new System.Drawing.Point(16, 238);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(32, 32);
            this.button19.TabIndex = 0;
            this.button19.Text = " F8";
            this.button19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button19.UseVisualStyleBackColor = false;
            this.button19.Click += new System.EventHandler(this.button19_Click);
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.DimGray;
            this.label11.Location = new System.Drawing.Point(8, 33);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(360, 20);
            this.label11.TabIndex = 18;
            this.label11.Text = "Klicken Sie auf die Tasten, um die Einstellungen zu ändern:";
            // 
            // label10
            // 
            this.label10.ForeColor = System.Drawing.Color.SaddleBrown;
            this.label10.Location = new System.Drawing.Point(8, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(360, 16);
            this.label10.TabIndex = 17;
            this.label10.Text = "Die folgenden Dateien können Sie im Anzeigemodus mit F1-F6 öffnen!";
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel8.Controls.Add(this.pictureBox4);
            this.panel8.Controls.Add(this.label14);
            this.panel8.Location = new System.Drawing.Point(56, 130);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(312, 32);
            this.panel8.TabIndex = 22;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(2, 3);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(24, 24);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox4.TabIndex = 20;
            this.pictureBox4.TabStop = false;
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.DimGray;
            this.label14.Location = new System.Drawing.Point(32, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(280, 30);
            this.label14.TabIndex = 19;
            this.label14.Text = "-";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel9.Controls.Add(this.label15);
            this.panel9.Controls.Add(this.pictureBox5);
            this.panel9.Location = new System.Drawing.Point(56, 166);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(312, 32);
            this.panel9.TabIndex = 22;
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.DimGray;
            this.label15.Location = new System.Drawing.Point(32, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(280, 30);
            this.label15.TabIndex = 19;
            this.label15.Text = "-";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(2, 3);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(24, 24);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox5.TabIndex = 20;
            this.pictureBox5.TabStop = false;
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel11.Controls.Add(this.pictureBox7);
            this.panel11.Controls.Add(this.label17);
            this.panel11.Location = new System.Drawing.Point(56, 238);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(312, 32);
            this.panel11.TabIndex = 23;
            // 
            // pictureBox7
            // 
            this.pictureBox7.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox7.Image")));
            this.pictureBox7.Location = new System.Drawing.Point(2, 3);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(24, 24);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox7.TabIndex = 20;
            this.pictureBox7.TabStop = false;
            // 
            // label17
            // 
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.DimGray;
            this.label17.Location = new System.Drawing.Point(32, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(280, 30);
            this.label17.TabIndex = 19;
            this.label17.Text = "-";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // radioButton3
            // 
            this.radioButton3.Checked = true;
            this.radioButton3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.radioButton3.ForeColor = System.Drawing.Color.SaddleBrown;
            this.radioButton3.Location = new System.Drawing.Point(169, 136);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(191, 16);
            this.radioButton3.TabIndex = 10;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "standard, mit \"Refrain\" markiert";
            // 
            // button9
            // 
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button9.Location = new System.Drawing.Point(11, 136);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(85, 24);
            this.button9.TabIndex = 8;
            this.button9.Text = "Farbe wählen";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Brown;
            this.panel2.Location = new System.Drawing.Point(112, 136);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(40, 24);
            this.panel2.TabIndex = 7;
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.DimGray;
            this.label18.Location = new System.Drawing.Point(8, 6);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(259, 24);
            this.label18.TabIndex = 5;
            this.label18.Text = "Standard Style :";
            // 
            // comboStyle
            // 
            this.comboStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStyle.FormattingEnabled = true;
            this.comboStyle.Location = new System.Drawing.Point(12, 23);
            this.comboStyle.Name = "comboStyle";
            this.comboStyle.Size = new System.Drawing.Size(257, 21);
            this.comboStyle.TabIndex = 36;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(8, -12);
            this.button3.Name = "button3";
		    this.button3.Margin = new Padding(0, 0, 0, 16);
            this.button3.Size = new System.Drawing.Size(80, 24);
            this.button3.TabIndex = 9;
            this.button3.Text = "Abbrechen";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(216, -12);
            this.button2.Name = "button2";
		    this.button2.Margin = new Padding(0, 0, 0, 16);
            this.button2.Size = new System.Drawing.Size(88, 24);
            this.button2.TabIndex = 8;
            this.button2.Text = "Übernehmen";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(312, -12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 24);
		    this.button1.Margin = new Padding(0, 0, 0, 16);
            this.button1.TabIndex = 7;
            this.button1.Text = "Ok";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(12, 116);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(64, 21);
            this.button8.TabIndex = 12;
            this.button8.Text = "ändern…";
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(11, 179);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(85, 24);
            this.button6.TabIndex = 8;
            this.button6.Text = "Farbe wählen";
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 92);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(64, 21);
            this.button5.TabIndex = 4;
            this.button5.Text = "ändern…";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 68);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(64, 21);
            this.button4.TabIndex = 3;
            this.button4.Text = "ändern…";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(40, 160);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(85, 24);
            this.button13.TabIndex = 20;
            this.button13.Text = "Bild wählen";
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(40, 96);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(85, 24);
            this.button12.TabIndex = 18;
            this.button12.Text = "Farbe wählen";
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(208, 96);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(85, 24);
            this.button11.TabIndex = 15;
            this.button11.Text = "Farbe wählen";
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(40, 32);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(85, 24);
            this.button10.TabIndex = 13;
            this.button10.Text = "Farbe wählen";
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(264, 11);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(104, 24);
            this.button7.TabIndex = 10;
            this.button7.Text = "Option freigeben";
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // Options
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(394, 424);
            this.ControlBox = false;
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Options";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lyra Optionen";
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.panel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.panel11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		// Refresh
		private void refresh()
		{
			this.richTextBox1.Height = this.richTextBox1.Font.Height + 2;
			this.richTextBox2.Height = this.richTextBox2.Font.Height + 2;
			this.richTextBox3.Height = this.richTextBox3.Font.Height + 2;

			this.button4.Top = this.richTextBox1.Top;
			this.richTextBox2.Top = this.richTextBox1.Top + this.richTextBox1.Height + 15;
			this.button5.Top = this.richTextBox2.Top;
			this.richTextBox3.Top = this.richTextBox2.Top + this.richTextBox2.Height + 15;
			this.button8.Top = this.richTextBox3.Top;
			this.label3.Top = this.richTextBox3.Top + this.richTextBox3.Height + 15;
			this.button6.Top = this.label3.Top + this.label3.Height + 15;
			this.panel1.Top = this.button6.Top;
			this.radioButton2.Top = this.panel1.Top - 5;
			this.radioButton1.Top = this.radioButton2.Top + this.radioButton2.Height + 5;
			var h = this.radioButton1.Top + this.radioButton1.Height + 35;
			this.tabControl1.Height = h > 310 ? h : 310;
			this.button1.Top = this.tabControl1.Top + this.tabControl1.Height + 8;
			this.button2.Top = this.button1.Top;
			this.button3.Top = this.button1.Top;
			this.Height = this.button3.Top + this.button3.Height + 44;
		}

		// Commit!
		private void commit()
		{
			if (this.changed)
			{
				Util.FONT = this.richTextBox1.Font;
				Util.SPECFONT = this.richTextBox2.Font;
				Util.TRANSFONT = this.richTextBox3.Font;
				Util.REFCOLOR = this.panel1.BackColor;
				Util.NOCOMMIT = this.checkBox1.Checked;
				Util.SHOWBUILDNEWS = this.checkBox2.Checked;
				Util.SHOWRIGHT = this.checkBox3.Checked;
				Util.SHOWGER = this.checkBox4.Checked;
				Util.SHOWNR = this.checkBox5.Checked;
				Util.TIMER = Int32.Parse(this.numericUpDown1.Value.ToString());
				Util.UNICOLOR = this.panel3.BackColor;
				Util.GRADCOL1 = this.panel5.BackColor;
				Util.GRADCOL2 = this.panel4.BackColor;
				Util.PREMODE = this.radioButton4.Checked ? 0 : this.radioButton5.Checked ? 1 : 2;
				Util.PICTURI = this.picturi;
				Util.CASCADEPIC = this.checkBox6.Checked;
				Util.FX = (string[]) this.FX.Clone();
				Util.updateRegSettings();
				this.changed = false;

			    var selectedStyle = this.comboStyle.SelectedItem as Style;
                if (selectedStyle != null && !selectedStyle.IsDefault)
                {
                    this.storage.SetStyleAsDefault(selectedStyle);
                }
			}
		}


		// abbrechen
		private void button3_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		// ok
		private void button1_Click(object sender, EventArgs e)
		{
			this.commit();
			this.Close();
		}

		// übernehmen
		private void button2_Click(object sender, EventArgs e)
		{
			this.commit();
			this.button2.Enabled = false;
		}

		// Standardschriftart ändern
		private void button4_Click(object sender, EventArgs e)
		{
			this.richTextBox1.Font = this.showFontDialog(this.richTextBox1.Font);
			this.refresh();
		}

		// Spezial-Schriftart ändern
		private void button5_Click(object sender, EventArgs e)
		{
			this.richTextBox2.Font = this.showFontDialog(this.richTextBox2.Font);
			this.refresh();
		}

		// Translation-Schriftart ändern
		private void button8_Click(object sender, EventArgs e)
		{
			this.richTextBox3.Font = this.showFontDialog(this.richTextBox3.Font);
			this.refresh();
		}

		// Farbe wählen
		private void button6_Click(object sender, EventArgs e)
		{
			this.panel1.BackColor = this.showColorDialog(this.panel1.BackColor);
		}

		// FontDialog
		private FontDialog fd;

		private Font showFontDialog(Font curFont)
		{
			this.fd.Font = curFont;
			var dr = this.fd.ShowDialog(this);
			if (dr == DialogResult.OK)
			{
				this.changed = true;
				this.button2.Enabled = true;
				return this.fd.Font;
			}
			return curFont;
		}

		// ColorDialog
		private ColorDialog cd;

		private Color showColorDialog(Color curColor)
		{
			this.cd.Color = curColor;
			var dr = this.cd.ShowDialog(this);
			if (dr == DialogResult.OK)
			{
				this.changed = true;
				this.button2.Enabled = true;
				return this.cd.Color;
			}
			return curColor;
		}

		// refrain normal
		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			this.changed = true;
			this.button2.Enabled = true;
			Util.refmode = true;
		}

		// refrain fett
		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			this.changed = true;
			this.button2.Enabled = true;
			Util.refmode = false;
		}

		// buildnews
		private void checkBox2_Click(object sender, EventArgs e)
		{
			this.changed = true;
			this.button2.Enabled = true;
		}

		// time to show the nr at startup
		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			this.changed = true;
			this.button2.Enabled = true;
		}

		// AllowCommit
		private void checkBox1_Click(object sender, EventArgs e)
		{
			this.changed = true;
			this.button2.Enabled = true;
		}

		// show right window
		private void checkBox3_Click(object sender, EventArgs e)
		{
			this.changed = true;
			this.button2.Enabled = true;
		}

		// show german title
		private void checkBox4_Click(object sender, EventArgs e)
		{
			this.changed = true;
			this.button2.Enabled = true;
		}

		// show nr at view-startup
		private void checkBox5_CheckedChanged(object sender, EventArgs e)
		{
			this.changed = true;
			this.button2.Enabled = true;
		}

		// cascade bg pic
		private void checkBox6_CheckedChanged(object sender, EventArgs e)
		{
			this.changed = true;
			this.button2.Enabled = true;
		}

		// enable admin settings
		public void enableChanges()
		{
			this.checkBox1.Enabled = true;
			this.button7.Enabled = false;
		}

		private void button7_Click(object sender, EventArgs e)
		{
			Pswd.showAdminPassword(this);
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Frst.ShowInfo();
		}

		// unicolor
		private void button10_Click(object sender, EventArgs e)
		{
			this.radioButton4.Checked = true;
			this.radioButton5.Checked = false;
			this.radioButton6.Checked = false;
			this.panel3.BackColor = this.showColorDialog(this.panel3.BackColor);
		}

		// grad1
		private void button12_Click(object sender, EventArgs e)
		{
			this.radioButton4.Checked = false;
			this.radioButton5.Checked = true;
			this.radioButton6.Checked = false;
			this.panel5.BackColor = this.showColorDialog(this.panel5.BackColor);
		}

		// grad2
		private void button11_Click(object sender, EventArgs e)
		{
			this.radioButton4.Checked = false;
			this.radioButton5.Checked = true;
			this.radioButton6.Checked = false;
			this.panel4.BackColor = this.showColorDialog(this.panel4.BackColor);
		}

		// picture chooser
		private string picturi = Util.PICTURI;

		private void button13_Click(object sender, EventArgs e)
		{
			this.radioButton4.Checked = false;
			this.radioButton5.Checked = false;
			this.radioButton6.Checked = true;
			this.changed = true;
			this.button2.Enabled = true;
			var fd = new OpenFileDialog();
			fd.CheckPathExists = true;
			fd.CheckFileExists = true;
			fd.Filter = "Bilddateien (*.jpg;*.gif;*.png;*.bmp)|*.jpg;*.gif;*.png;*.bmp|Alle Dateien (*.*)|*.*";
			if (fd.ShowDialog(this) == DialogResult.OK)
			{
				this.picturi = fd.FileName;
			}
			try
			{
				var img = Image.FromFile(this.picturi);
				this.pictureBox1.Image = Util.stretchProportional(img, this.pictureBox1.Size);
			}
			catch (Exception)
			{
				this.picturi = "";
				this.pictureBox1.Image = null;
			}
		}


		// presenation mode changed
		private void presMode_CheckedChanged(object sender, EventArgs e)
		{
			this.button2.Enabled = true;
			this.changed = true;
		}

		/*FX*/
		// FX
		private string[] FX = (string[]) Util.FX.Clone();

		private void getFile(Label label, PictureBox pictBox, int fnr)
		{
			var fxr = LyraShell.FX.ShowFX(this);
			if (fxr == LyraShell.FX.FXResult.NewFile)
			{
				var filename = "";
				var fd = new OpenFileDialog();
				fd.CheckPathExists = true;
				fd.CheckFileExists = true;
				fd.Filter = "Alle Dateien (*.*)|*.*";
				fd.Title = "lyra - wählen Sie die auszuführende Datei";
				if (fd.ShowDialog(this) == DialogResult.OK)
				{
					filename = fd.FileName;
					this.FX[fnr] = filename;
					this.setFilename(filename, label, pictBox);
					this.changed = true;
					this.button2.Enabled = true;
				}
			}
			else if (fxr == LyraShell.FX.FXResult.Delete)
			{
				this.setFilename("-", label, pictBox);
				this.FX[fnr] = "-";
				this.changed = true;
				this.button2.Enabled = true;
			}
			else if (fxr == LyraShell.FX.FXResult.PictChoice)
			{
				var filename = "";
				var fbd = new FolderBrowserDialog();
				fbd.Description = "Wählen Sie den Ordner mit Ihren Bildern aus!" + Util.NL;
				fbd.Description += "(Bilder aus Unterverzeichnissen werden NICHT angezeigt)";
				fbd.ShowNewFolderButton = false;
				if (fbd.ShowDialog(this) == DialogResult.OK)
				{
					filename = fbd.SelectedPath;
					filename = "pict://" + filename;
					this.FX[fnr] = filename;
					this.setFilename(filename, label, pictBox);
					this.changed = true;
					this.button2.Enabled = true;
				}
			}
		}

		// format label
		private void setFilename(string filename, Label label, PictureBox pictBox)
		{
			var fsplit = filename.Split('\\');
			var filelabel = fsplit[fsplit.Length - 1];
			if (filelabel.Length > 80) filelabel = filelabel.Substring(0, 80) + "...";
			if (filename.StartsWith("pict://")) filelabel += " (Bilder)";
			label.Text = filelabel;
			fsplit = filename.Split('.');
			var ext = fsplit[fsplit.Length - 1];
			var icon = FileLauncher.IconFromExtension(ext, FileLauncher.IconSize.Large);
			Bitmap iconBmp;
			if (icon != null)
			{
				iconBmp = Util.stretchProportional(icon.ToBitmap(), new Size(24, 24));
			}
			else
			{
				var resources = new System.Resources.ResourceManager(typeof (Options));
				iconBmp = new Bitmap((Image) resources.GetObject("pictureBox2.Image"));
			}
			pictBox.Image = iconBmp;
		}

		// F1
		private void button14_Click(object sender, EventArgs e)
		{
			this.getFile(this.label12, this.pictureBox2, 0);
		}

		// F2
		private void button15_Click(object sender, EventArgs e)
		{
			this.getFile(this.label13, this.pictureBox3, 1);
		}

		// F3
		private void button16_Click(object sender, EventArgs e)
		{
			this.getFile(this.label14, this.pictureBox4, 2);
		}

		// F4
		private void button17_Click(object sender, EventArgs e)
		{
			this.getFile(this.label15, this.pictureBox5, 3);
		}

		// F5
		private void button18_Click(object sender, EventArgs e)
		{
			this.getFile(this.label16, this.pictureBox6, 4);
		}

		// F6
		private void button19_Click(object sender, EventArgs e)
		{
			this.getFile(this.label17, this.pictureBox7, 5);
		}
	}
}