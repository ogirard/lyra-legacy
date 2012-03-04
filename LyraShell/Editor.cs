using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinToolbars;

namespace Lyra2.LyraShell
{
    /// <summary>
    /// Zusammendfassende Beschreibung für Editor.
    /// </summary>
    public class Editor : Form
    {
        private LyraButtonControl button1;
        private LyraButtonControl button2;
        private TextBox textBox1;
        private TextBox textBox2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private IContainer components;

        private ISong song = null;
        private RichTextBox richTextBox1;
        private GUI owner;
        private readonly IStorage _storage;
        private readonly IList<Style> styles;
        private string undo = "";
        private Bitmap transImage = null;

        public static bool open = false;
        private LyraButtonControl button10;
        private ListBox listBox1;
        private Label label7;
        private LyraButtonControl button14;
        private LyraButtonControl button16;
        private LyraButtonControl button17;
        private LyraButtonControl button12;
        private TextBox textBox3;
        private CheckBox checkBox1;
        private Label lblStyle;
        private PictureBox previewBtn;
        private Label label11;
        private UltraToolbarsManager toolbars;
        private Panel Editor_Fill_Panel;
        private Infragistics.Win.Misc.UltraPanel toolbarPanel;
        private Panel ClientArea_Fill_Panel;
        private UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Left;
        private UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Right;
        private UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Top;
        private UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Bottom;
        private ComboBox comboStyle;
        private LyraButtonControl btnOpenStyleEditor;
        private CheckBox useDefaultStyleCheckBox;
        public static Editor editor = null;


        public Editor(ISong song, GUI owner, IStorage storage)
        {
            this.song = song;
            this.owner = owner;
            this._storage = storage;
            this.styles = _storage.Styles;

            open = true;
            editor = this;
            this.AcceptButton = this.button1;
            InitializeComponent();
            this.Height = 460;
            this.comboStyle.BeginUpdate();
            foreach (Style style in storage.Styles)
            {
                this.comboStyle.Items.Add(style);
            }
            this.comboStyle.EndUpdate();
            
            if (song != null)
            {
                this.textBox1.Text = song.Title;
                this.textBox2.Text = song.Number.ToString();
                this.richTextBox1.Text = song.Text;
                this.button12.Enabled = true;
                if (song.Desc == "")
                {
                    this.checkBox1.Checked = false;
                    this.textBox3.Text = "---";
                    this.textBox3.Enabled = false;
                }
                else
                {
                    this.checkBox1.Checked = true;
                    this.textBox3.Text = song.Desc;
                    this.textBox3.Enabled = true;
                }
                this.useDefaultStyleCheckBox.Checked = this.song.UseDefaultStyle;
                if (!this.song.UseDefaultStyle)
                {
                    this.comboStyle.SelectedItem = this.song.Style;
                }
            }
            else
            {
                this.textBox1.Text = "";
                this.textBox2.Text = "";
                this.richTextBox1.Text = "";
                this.textBox2.Enabled = true;
                this.textBox3.Text = "---";
                this.checkBox1.Checked = false;
                this.textBox3.Enabled = false;
                this.useDefaultStyleCheckBox.Checked = true;
                this.comboStyle.SelectedItem = styles.FirstOrDefault(s => s.IsDefault);
            }
            
            this.toolbars.ToolClick += ToolbarsClickHandler;
            ComboBoxTool combo = (ComboBoxTool)this.toolbars.Toolbars[0].Tools["indentCombo"];
            this.toolbars.Toolbars[0].Tools["undo"].SharedProps.Enabled = false;
            combo.SelectedIndex = 0;
        }

        private void OpenStyleEditorClickHandler(object sender, EventArgs e)
        {
            using (StyleEditor se = new StyleEditor(this._storage))
            {
                se.ShowDialog(this);
                this.comboStyle.BeginUpdate();
                this.comboStyle.Items.Clear();
                foreach (Style style in styles)
                {
                    this.comboStyle.Items.Add(style);
                }

                if (this.song != null && !this.song.UseDefaultStyle)
                {
                    this.comboStyle.SelectedItem = this.song.Style;
                }
                else
                {
                    this.comboStyle.SelectedItem = styles.FirstOrDefault(s => s.IsDefault);
                }

                this.comboStyle.EndUpdate();
            }
        }

        void ToolbarsClickHandler(object sender, ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "bold":    // ButtonTool
                    this.format(Util.BOLD, false);
                    break;

                case "italic":    // ButtonTool
                    this.format(Util.ITALIC, false);
                    break;

                case "refrain":    // ButtonTool
                    this.format(Util.REF, true);
                    break;

                case "special":    // ButtonTool
                    this.format(Util.SPEC, false);
                    break;

                case "tab":    // ButtonTool
                    int tabpos = this.richTextBox1.SelectionStart;
                    this.richTextBox1.Text = this.richTextBox1.Text.Insert(tabpos, '\t'.ToString());
                    this.richTextBox1.Focus();
                    this.richTextBox1.Select(tabpos + 1, 0);
                    break;

                case "indent":    // ButtonTool
                    ComboBoxTool combo = (ComboBoxTool)this.toolbars.Toolbars[0].Tools["indentCombo"];
                    this.format(Util.BLOCK + ((ValueListItem)combo.SelectedItem).DisplayText, true);
                    break;

                case "Seitenumbruch":    // ButtonTool
                    int findpos;
                    if ((findpos = this.richTextBox1.Find("<" + Util.PGBR + " />", 0, RichTextBoxFinds.MatchCase)) == -1)
                    {
                        this.toolbars.Toolbars[0].Tools["undo"].SharedProps.Enabled = true;
                        this.undo = this.richTextBox1.Rtf;
                        int pos = this.richTextBox1.SelectionStart;
                        if ((pos > 0) && (this.richTextBox1.Text[pos - 1] != '\n'))
                        {
                            this.richTextBox1.Text = this.richTextBox1.Text.Insert(pos, "\n");
                            pos++;
                        }
                        this.richTextBox1.Text = this.richTextBox1.Text.Insert(pos, "\n<" + Util.PGBR + " />\n");
                        pos += Util.PGBR.Length + 6;
                        if ((pos < this.richTextBox1.Text.Length) && (this.richTextBox1.Text[pos] != '\n'))
                        {
                            this.richTextBox1.Text = this.richTextBox1.Text.Insert(pos, "\n");
                        }
                        this.richTextBox1.Focus();
                        this.richTextBox1.Select(pos + 1, 0);
                    }
                    else
                    {
                        Util.MBoxError("Nur ein Seitenumbruch möglich!");
                        this.richTextBox1.Focus();
                        this.richTextBox1.Select(findpos, 4 + Util.PGBR.Length);
                    }
                    break;

                case "jumpmark":    // ButtonTool
                    InsertJumpMark ijm = new InsertJumpMark();
                    int line = this.richTextBox1.GetLineFromCharIndex(this.richTextBox1.SelectionStart) + 1;
                    ijm.JumpMarkName = "Sprungmarke Zeile " + line;
                    if (ijm.ShowDialog(this) == DialogResult.OK)
                    {
                        this.toolbars.Toolbars[0].Tools["undo"].SharedProps.Enabled = true;
                        this.undo = this.richTextBox1.Rtf;
                        int jmpMarkPos = this.richTextBox1.SelectionStart;
                        string jumpMark = string.IsNullOrEmpty(ijm.JumpMarkName) ? "Sprungmarke Zeile " + line : ijm.JumpMarkName.Replace('\"', '\'');
                        jumpMark = "<" + Util.JMP + " name=\"" + jumpMark + "\" />";
                        this.richTextBox1.Text = this.richTextBox1.Text.Insert(jmpMarkPos, jumpMark);
                        this.richTextBox1.Focus();
                        this.richTextBox1.Select(jmpMarkPos + jumpMark.Length, 0);
                    }
                    break;
                case "undo":    // ButtonTool
                    
                    if (this.undo != "")
                    {
                        this.richTextBox1.Rtf = this.undo;
                        this.undo = "";
                    }
                    else if (this.richTextBox1.CanUndo)
                    {
                        this.richTextBox1.Undo();
                    }
                    break;
            }

        }

        

        /// <summary>
        /// Die verwendeten Ressourcen bereinigen.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            open = false;
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
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("editorToolBar");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("bold");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("italic");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("refrain");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("special");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("tab");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("indent");
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool1 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("indentCombo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("jumpmark");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Seitenumbruch");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("undo");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("bold");
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("italic");
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("refrain");
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("special");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("tab");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("indent");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Seitenumbruch");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("jumpmark");
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool2 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("indentCombo");
            Infragistics.Win.ValueList valueList1 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("undo");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.lblStyle = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.toolbars = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this.toolbarPanel = new Infragistics.Win.Misc.UltraPanel();
            this.ClientArea_Fill_Panel = new System.Windows.Forms.Panel();
            this._ClientArea_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.Editor_Fill_Panel = new System.Windows.Forms.Panel();
            this.useDefaultStyleCheckBox = new System.Windows.Forms.CheckBox();
            this.comboStyle = new System.Windows.Forms.ComboBox();
            this.previewBtn = new System.Windows.Forms.PictureBox();
            this.button12 = new Lyra2.LyraShell.LyraButtonControl();
            this.button10 = new Lyra2.LyraShell.LyraButtonControl();
            this.btnOpenStyleEditor = new Lyra2.LyraShell.LyraButtonControl();
            this.button2 = new Lyra2.LyraShell.LyraButtonControl();
            this.button1 = new Lyra2.LyraShell.LyraButtonControl();
            this.button14 = new Lyra2.LyraShell.LyraButtonControl();
            this.button16 = new Lyra2.LyraShell.LyraButtonControl();
            this.button17 = new Lyra2.LyraShell.LyraButtonControl();
            ((System.ComponentModel.ISupportInitialize)(this.toolbars)).BeginInit();
            this.toolbarPanel.ClientArea.SuspendLayout();
            this.toolbarPanel.SuspendLayout();
            this.ClientArea_Fill_Panel.SuspendLayout();
            this.Editor_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewBtn)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(240, 38);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(384, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "textBox1";
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(102, 38);
            this.textBox2.MaxLength = 4;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(40, 20);
            this.textBox2.TabIndex = 0;
            this.textBox2.Text = "textBox2";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SlateGray;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Lyra Songtext Editor";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(24, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Liednummer:";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(206, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Titel:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(24, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Text:";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(685, 230);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "richTextBox1";
            // 
            // listBox1
            // 
            this.listBox1.Location = new System.Drawing.Point(24, 465);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(600, 69);
            this.listBox1.TabIndex = 25;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(24, 441);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Übersetzungen";
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(200, 64);
            this.textBox3.MaxLength = 3;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(40, 20);
            this.textBox3.TabIndex = 28;
            this.textBox3.Text = "---";
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new System.Drawing.Point(104, 65);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(104, 16);
            this.checkBox1.TabIndex = 29;
            this.checkBox1.Text = "Bemerkungen:";
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // lblStyle
            // 
            this.lblStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStyle.Location = new System.Drawing.Point(24, 358);
            this.lblStyle.Name = "lblStyle";
            this.lblStyle.Size = new System.Drawing.Size(68, 16);
            this.lblStyle.TabIndex = 6;
            this.lblStyle.Text = "Style :";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.DimGray;
            this.label11.Location = new System.Drawing.Point(653, 68);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(52, 13);
            this.label11.TabIndex = 5;
            this.label11.Text = "Vorschau";
            // 
            // toolbars
            // 
            this.toolbars.DesignerFlags = 1;
            this.toolbars.DockWithinContainer = this.toolbarPanel.ClientArea;
            this.toolbars.ShowFullMenusDelay = 500;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            buttonTool3.InstanceProps.IsFirstInGroup = true;
            buttonTool5.InstanceProps.IsFirstInGroup = true;
            buttonTool8.InstanceProps.IsFirstInGroup = true;
            appearance1.Image = global::Lyra2.LyraShell.Properties.Resources.undo_icon;
            buttonTool17.InstanceProps.AppearancesSmall.Appearance = appearance1;
            buttonTool17.InstanceProps.IsFirstInGroup = true;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2,
            buttonTool3,
            buttonTool4,
            buttonTool5,
            buttonTool6,
            comboBoxTool1,
            buttonTool8,
            buttonTool7,
            buttonTool17});
            ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
            ultraToolbar1.Settings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.NoGlyph;
            ultraToolbar1.Settings.ToolDisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            ultraToolbar1.Text = "editorToolBar";
            this.toolbars.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            this.toolbars.ToolbarSettings.BorderStyleDocked = Infragistics.Win.UIElementBorderStyle.Etched;
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance14.BackColor2 = System.Drawing.Color.Silver;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.GlassTop37;
            appearance14.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.toolbars.ToolbarSettings.DockedAppearance = appearance14;
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(178)))));
            appearance16.BackColor2 = System.Drawing.Color.Silver;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalWithGlassLeft50;
            appearance16.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.toolbars.ToolbarSettings.HotTrackAppearance = appearance16;
            appearance11.FontData.BoldAsString = "True";
            buttonTool9.SharedPropsInternal.AppearancesSmall.Appearance = appearance11;
            buttonTool9.SharedPropsInternal.Caption = "F";
            appearance12.FontData.ItalicAsString = "True";
            buttonTool10.SharedPropsInternal.AppearancesSmall.Appearance = appearance12;
            buttonTool10.SharedPropsInternal.Caption = "K";
            appearance13.FontData.BoldAsString = "True";
            appearance13.FontData.ItalicAsString = "True";
            appearance13.FontData.Name = "Times New Roman";
            buttonTool11.SharedPropsInternal.AppearancesSmall.Appearance = appearance13;
            buttonTool11.SharedPropsInternal.Caption = "Refrain";
            buttonTool12.SharedPropsInternal.Caption = "Spezial";
            buttonTool13.SharedPropsInternal.Caption = "Tab";
            buttonTool14.SharedPropsInternal.Caption = "Einrücken um";
            buttonTool15.SharedPropsInternal.Caption = "Seitenumbruch";
            buttonTool16.SharedPropsInternal.Caption = "Sprungmarke";
            comboBoxTool2.SharedPropsInternal.Width = 50;
            valueList1.DisplayStyle = Infragistics.Win.ValueListDisplayStyle.DisplayText;
            valueList1.PreferredDropDownSize = new System.Drawing.Size(0, 0);
            valueListItem1.DataValue = "ValueListItem0";
            valueListItem1.DisplayText = "8";
            valueListItem2.DataValue = "ValueListItem1";
            valueListItem2.DisplayText = "16";
            valueListItem3.DataValue = "ValueListItem2";
            valueListItem3.DisplayText = "24";
            valueListItem4.DataValue = "ValueListItem3";
            valueListItem4.DisplayText = "32";
            valueListItem5.DataValue = "ValueListItem4";
            valueListItem5.DisplayText = "40";
            valueList1.ValueListItems.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3,
            valueListItem4,
            valueListItem5});
            comboBoxTool2.ValueList = valueList1;
            buttonTool18.SharedPropsInternal.Caption = "Undo";
            buttonTool18.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.DefaultForToolType;
            this.toolbars.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool9,
            buttonTool10,
            buttonTool11,
            buttonTool12,
            buttonTool13,
            buttonTool14,
            buttonTool15,
            buttonTool16,
            comboBoxTool2,
            buttonTool18});
            // 
            // toolbarPanel
            // 
            // 
            // toolbarPanel.ClientArea
            // 
            this.toolbarPanel.ClientArea.Controls.Add(this.ClientArea_Fill_Panel);
            this.toolbarPanel.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Left);
            this.toolbarPanel.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Right);
            this.toolbarPanel.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Top);
            this.toolbarPanel.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Bottom);
            this.toolbarPanel.Location = new System.Drawing.Point(27, 88);
            this.toolbarPanel.Name = "toolbarPanel";
            this.toolbarPanel.Size = new System.Drawing.Size(685, 261);
            this.toolbarPanel.TabIndex = 34;
            // 
            // ClientArea_Fill_Panel
            // 
            this.ClientArea_Fill_Panel.Controls.Add(this.richTextBox1);
            this.ClientArea_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.ClientArea_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ClientArea_Fill_Panel.Location = new System.Drawing.Point(0, 31);
            this.ClientArea_Fill_Panel.Name = "ClientArea_Fill_Panel";
            this.ClientArea_Fill_Panel.Size = new System.Drawing.Size(685, 230);
            this.ClientArea_Fill_Panel.TabIndex = 0;
            // 
            // _ClientArea_Toolbars_Dock_Area_Left
            // 
            this._ClientArea_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._ClientArea_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._ClientArea_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._ClientArea_Toolbars_Dock_Area_Left.Name = "_ClientArea_Toolbars_Dock_Area_Left";
            this._ClientArea_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 230);
            this._ClientArea_Toolbars_Dock_Area_Left.ToolbarsManager = this.toolbars;
            // 
            // _ClientArea_Toolbars_Dock_Area_Right
            // 
            this._ClientArea_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._ClientArea_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._ClientArea_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(685, 31);
            this._ClientArea_Toolbars_Dock_Area_Right.Name = "_ClientArea_Toolbars_Dock_Area_Right";
            this._ClientArea_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 230);
            this._ClientArea_Toolbars_Dock_Area_Right.ToolbarsManager = this.toolbars;
            // 
            // _ClientArea_Toolbars_Dock_Area_Top
            // 
            this._ClientArea_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._ClientArea_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._ClientArea_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ClientArea_Toolbars_Dock_Area_Top.Name = "_ClientArea_Toolbars_Dock_Area_Top";
            this._ClientArea_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(685, 31);
            this._ClientArea_Toolbars_Dock_Area_Top.ToolbarsManager = this.toolbars;
            // 
            // _ClientArea_Toolbars_Dock_Area_Bottom
            // 
            this._ClientArea_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._ClientArea_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._ClientArea_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 261);
            this._ClientArea_Toolbars_Dock_Area_Bottom.Name = "_ClientArea_Toolbars_Dock_Area_Bottom";
            this._ClientArea_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(685, 0);
            this._ClientArea_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.toolbars;
            // 
            // Editor_Fill_Panel
            // 
            this.Editor_Fill_Panel.Controls.Add(this.useDefaultStyleCheckBox);
            this.Editor_Fill_Panel.Controls.Add(this.comboStyle);
            this.Editor_Fill_Panel.Controls.Add(this.toolbarPanel);
            this.Editor_Fill_Panel.Controls.Add(this.label11);
            this.Editor_Fill_Panel.Controls.Add(this.previewBtn);
            this.Editor_Fill_Panel.Controls.Add(this.textBox3);
            this.Editor_Fill_Panel.Controls.Add(this.label7);
            this.Editor_Fill_Panel.Controls.Add(this.label4);
            this.Editor_Fill_Panel.Controls.Add(this.label2);
            this.Editor_Fill_Panel.Controls.Add(this.textBox2);
            this.Editor_Fill_Panel.Controls.Add(this.textBox1);
            this.Editor_Fill_Panel.Controls.Add(this.checkBox1);
            this.Editor_Fill_Panel.Controls.Add(this.button12);
            this.Editor_Fill_Panel.Controls.Add(this.listBox1);
            this.Editor_Fill_Panel.Controls.Add(this.button10);
            this.Editor_Fill_Panel.Controls.Add(this.label1);
            this.Editor_Fill_Panel.Controls.Add(this.label3);
            this.Editor_Fill_Panel.Controls.Add(this.btnOpenStyleEditor);
            this.Editor_Fill_Panel.Controls.Add(this.button2);
            this.Editor_Fill_Panel.Controls.Add(this.button1);
            this.Editor_Fill_Panel.Controls.Add(this.button14);
            this.Editor_Fill_Panel.Controls.Add(this.button16);
            this.Editor_Fill_Panel.Controls.Add(this.button17);
            this.Editor_Fill_Panel.Controls.Add(this.lblStyle);
            this.Editor_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.Editor_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Editor_Fill_Panel.Location = new System.Drawing.Point(0, 0);
            this.Editor_Fill_Panel.Name = "Editor_Fill_Panel";
            this.Editor_Fill_Panel.Size = new System.Drawing.Size(736, 544);
            this.Editor_Fill_Panel.TabIndex = 0;
            // 
            // useDefaultStyleCheckBox
            // 
            this.useDefaultStyleCheckBox.AutoSize = true;
            this.useDefaultStyleCheckBox.Location = new System.Drawing.Point(75, 378);
            this.useDefaultStyleCheckBox.Name = "useDefaultStyleCheckBox";
            this.useDefaultStyleCheckBox.Size = new System.Drawing.Size(182, 17);
            this.useDefaultStyleCheckBox.TabIndex = 36;
            this.useDefaultStyleCheckBox.Text = "Immer Standard Style verwenden";
            this.useDefaultStyleCheckBox.UseVisualStyleBackColor = true;
            this.useDefaultStyleCheckBox.CheckedChanged += new System.EventHandler(this.useDefaultStyleCheckBox_CheckedChanged);
            // 
            // comboStyle
            // 
            this.comboStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStyle.FormattingEnabled = true;
            this.comboStyle.Location = new System.Drawing.Point(73, 355);
            this.comboStyle.Name = "comboStyle";
            this.comboStyle.Size = new System.Drawing.Size(257, 21);
            this.comboStyle.TabIndex = 35;
            // 
            // previewBtn
            // 
            this.previewBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.previewBtn.Image = ((System.Drawing.Image)(resources.GetObject("previewBtn.Image")));
            this.previewBtn.Location = new System.Drawing.Point(648, 5);
            this.previewBtn.Name = "previewBtn";
            this.previewBtn.Size = new System.Drawing.Size(64, 64);
            this.previewBtn.TabIndex = 33;
            this.previewBtn.TabStop = false;
            this.previewBtn.Click += new System.EventHandler(this.previewButton_Click);
            // 
            // button12
            // 
            this.button12.Enabled = false;
            this.button12.Location = new System.Drawing.Point(149, 38);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(53, 20);
            this.button12.TabIndex = 27;
            this.button12.Text = "ändern";
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button10
            // 
            this.button10.BackColorInternal = System.Drawing.SystemColors.Control;
            this.button10.Location = new System.Drawing.Point(24, 401);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(160, 24);
            this.button10.TabIndex = 21;
            this.button10.Text = "Übersetzungen >>";
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // btnOpenStyleEditor
            // 
            this.btnOpenStyleEditor.BackColorInternal = System.Drawing.SystemColors.Control;
            this.btnOpenStyleEditor.Location = new System.Drawing.Point(339, 354);
            this.btnOpenStyleEditor.Name = "btnOpenStyleEditor";
            this.btnOpenStyleEditor.Size = new System.Drawing.Size(94, 23);
            this.btnOpenStyleEditor.TabIndex = 20;
            this.btnOpenStyleEditor.Text = "Style Editor ...";
            this.btnOpenStyleEditor.Click += new System.EventHandler(this.OpenStyleEditorClickHandler);
            // 
            // button2
            // 
            this.button2.BackColorInternal = System.Drawing.SystemColors.Control;
            this.button2.Location = new System.Drawing.Point(608, 401);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(72, 24);
            this.button2.TabIndex = 20;
            this.button2.Text = "Abbrechen";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColorInternal = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(688, 401);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 24);
            this.button1.TabIndex = 19;
            this.button1.Text = "Ok";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(688, 505);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(40, 24);
            this.button14.TabIndex = 24;
            this.button14.Text = "Del";
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(632, 473);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(96, 24);
            this.button16.TabIndex = 22;
            this.button16.Text = "Neu…";
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(632, 505);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(40, 24);
            this.button17.TabIndex = 23;
            this.button17.Text = "Edit";
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // Editor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(736, 544);
            this.ControlBox = false;
            this.Controls.Add(this.Editor_Fill_Panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Editor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Editor";
            this.Activated += new System.EventHandler(this.MeGotFocus);
            ((System.ComponentModel.ISupportInitialize)(this.toolbars)).EndInit();
            this.toolbarPanel.ClientArea.ResumeLayout(false);
            this.toolbarPanel.ResumeLayout(false);
            this.ClientArea_Fill_Panel.ResumeLayout(false);
            this.Editor_Fill_Panel.ResumeLayout(false);
            this.Editor_Fill_Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewBtn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        // abbrechen
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ok
        private void button1_Click(object sender, EventArgs e)
        {
            int nr;
            try
            {
                nr = Int32.Parse(this.textBox2.Text);
            }
            catch (Exception ex)
            {
                Util.MBoxError("Überprüfen Sie die Liednummer!", ex);
                this.textBox2.Focus();
                this.textBox2.SelectAll();
                return;
            }
            string title = this.textBox1.Text;
            string text = this.richTextBox1.Text;
            string desc = this.textBox3.Text == @"---" ? "" : this.textBox3.Text;
            if (this.toDel != null && nr == this.toDel.Number) // number hasn't changed!
            {
                this.song = this.toDel;
            }
            if (this.song != null)
            {
                this.song.Title = title;
                this.song.Text = text;
                this.song.Desc = desc;
                song.Style = this.comboStyle.SelectedItem as Style;
                this.song.UseDefaultStyle = this.useDefaultStyleCheckBox.Checked;
                this.owner.Status = "Liedtext editiert...";
            }
            else
            {
                string id = "s" + Util.toFour(nr);
                ISong newSong = new Song(nr, title, text, id, desc, true);
                newSong.Style = this.comboStyle.SelectedItem as Style;
                newSong.UseDefaultStyle = this.useDefaultStyleCheckBox.Checked;
                if (this.toDel != null)
                {
                    CopyTrans(this.toDel, newSong);
                    this.toDel.Delete();
                }
                try
                {
                    this.owner.AddSong(newSong);
                }
                catch
                {
                    string msg = "Diese Liednummer wurde bereits einmal verwendet!\n";
                    msg += "Soll das Lied trotzdem hinzugefügt werden?\n";
                    msg += "(evtl. haben Sie das Lied bereits einer anderen Nummer zugewiesen,\n";
                    msg += "in diesem Fall einfach \"Ja\" klicken!)";

                    DialogResult dr = MessageBox.Show(this, msg, "lyra - neues Lied hinzufügen",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (dr == DialogResult.Yes)
                    {
                        newSong.nextID();
                        this.owner.AddSong(newSong);
                    }
                    else
                    {
                        return;
                    }
                }
                this.owner.Status = "Neues Lied erfolgreich hinzugefügt...";
            }
            this.owner.UpdateListBox();
            this.owner.ToUpdate(true);
            this.Close();
        }

        private void format(string tag, bool nl)
        {
            this.toolbars.Toolbars[0].Tools["undo"].SharedProps.Enabled = true;
            this.undo = this.richTextBox1.Rtf;
            int left = this.richTextBox1.SelectionStart;
            int right = this.richTextBox1.SelectionLength + left + tag.Length + 2;
            this.richTextBox1.Text = this.richTextBox1.Text.Insert(left, "<" + tag + ">");
            this.richTextBox1.Text = this.richTextBox1.Text.Insert(right, "</" + tag + ">");
            if (nl)
            {
                if (((right + tag.Length + 3) < this.richTextBox1.Text.Length) &&
                    (this.richTextBox1.Text[right + tag.Length + 3] != '\n'))
                {
                    this.richTextBox1.Text = this.richTextBox1.Text.Insert(right + tag.Length + 3, "\n");
                }
                if ((left > 0) && (this.richTextBox1.Text[left - 1] != '\n'))
                {
                    this.richTextBox1.Text = this.richTextBox1.Text.Insert(left, "\n");
                    right++;
                }
            }
            this.richTextBox1.Focus();
            this.richTextBox1.Select(right, 0);
        }

        // show translations
        private bool transshown = false;

        private void button10_Click(object sender, EventArgs e)
        {
            int nr;
            try
            {
                nr = Int32.Parse(this.textBox2.Text);
            }
            catch (Exception ex)
            {
                Util.MBoxError("Überprüfen Sie die Liednummer!", ex);
                this.textBox2.Focus();
                this.textBox2.SelectAll();
                return;
            }
            if (this.toDel != null && nr == this.toDel.Number)
            {
                this.song = this.toDel;
                this.toDel = null;
                this.textBox2.Enabled = false;
                this.button12.Enabled = true;
            }
            if (this.song == null)
            {
                DialogResult add = MessageBox.Show(this, @"Das neue Lied muss zuerst gespeichert werden." + Environment.NewLine +
                                                         @"Soll das Lied jetzt hinzugefügt werden?", @"lyra Hinweis",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Warning);
                if (add == DialogResult.No)
                {
                    return;
                }
                string title = this.textBox1.Text;
                string text = this.richTextBox1.Text;
                string id = "s" + Util.toFour(nr);
                string desc = this.checkBox1.Checked ? this.textBox3.Text : "";
                this.song = new Song(nr, title, text, id, desc, true);
                this.song.UseDefaultStyle = this.useDefaultStyleCheckBox.Checked;
                if (this.toDel != null)
                {
                    CopyTrans(this.toDel, this.song);
                    this.toDel.Delete();
                }
                try
                {
                    this.owner.AddSong(this.song);
                }
                catch
                {
                    string msg = "Diese Liednummer wurde bereits einmal verwendet!\n";
                    msg += "Soll das Lied trotzdem hinzugefügt werden?";

                    DialogResult dr = MessageBox.Show(this, msg, "lyra - neues Lied hinzufügen",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (dr == DialogResult.Yes)
                    {
                        this.song.nextID();
                        this.owner.AddSong(this.song);
                    }
                    else
                    {
                        return;
                    }
                }
                this.owner.Status = "Neues Lied erfolgreich hinzugefügt...";
                this.owner.UpdateListBox();
                this.owner.ToUpdate(true);
                this.textBox2.Enabled = false;
                this.button12.Enabled = true;
            }

            if (!this.transshown)
            {
                this.Height = 568;
                this.transshown = true;
                this.button10.Text = "<< Übersetzungen";
                this.song.ShowTranslations(this.listBox1);
                if (this.listBox1.Items.Count > 0)
                {
                    this.listBox1.SelectedIndex = 0;
                }
            }
            else
            {
                this.Height = 460;
                this.transshown = false;
                this.button10.Text = "Übersetzungen >>";
            }
        }

        // Translations
        // neu...
        private void button16_Click(object sender, EventArgs e)
        {
            if (!TEditor.TEditorOpen)
            {
                (new TEditor(this.listBox1, this.song, this.owner)).Show();
            }
            else
            {
                TEditor.tEditor.Focus();
            }
        }

        // edit
        private void button17_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItems.Count == 1)
            {
                if (!TEditor.TEditorOpen)
                {
                    (new TEditor(this.listBox1, this.song, this.owner, (ITranslation)this.listBox1.SelectedItem)).Show();
                }
                else
                {
                    TEditor.tEditor.Focus();
                }
            }
        }

        // del
        private void button14_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItems.Count == 1)
            {
                this.song.RemoveTranslation((ITranslation)this.listBox1.SelectedItem);
                this.song.ShowTranslations(this.listBox1);
                this.owner.ToUpdate(true);
            }
        }

        private void MeGotFocus(object sender, EventArgs e)
        {
            if (TEditor.TEditorOpen)
            {
                TEditor.tEditor.Focus();
            }
        }

        // change Song-Nr.
        private ISong toDel = null;

        private void button12_Click(object sender, EventArgs e)
        {
            if (this.transshown)
            {
                this.button10_Click(sender, e);
            }
            this.button12.Enabled = false;
            this.textBox2.Enabled = true;
            this.toDel = this.song;
            this.song = null;
        }

        private static void CopyTrans(ISong from, ISong to)
        {
            from.copyTranslation(to);
        }

        // checkBox
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.textBox3.Enabled = true;
                this.textBox3.Text = "";
                this.textBox3.SelectAll();
            }
            else
            {
                this.textBox3.Enabled = false;
                this.textBox3.Text = "---";
            }
        }

        
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (((keyData & Keys.B) == Keys.B) && (keyData & Keys.Control) == Keys.Control)
            {
                if (this.owner != null)
                {
                    this.owner.BlackScreen();
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void previewButton_Click(object sender, EventArgs e)
        {
            string id = Util.PREVIEW_SONG_ID;
            string title = this.textBox1.Text;
            string text = this.richTextBox1.Text;
            int nr = 0;
            try
            {
                nr = int.Parse(this.textBox2.Text);
            }
            catch (Exception)
            { }
            string desc = this.textBox3.Text == @"---" ? "" : this.textBox3.Text;
            ISong previewSong = new Song(nr, title, text, id, desc, true);
            previewSong.Style = this.comboStyle.SelectedItem as Style;
            ListBox previewListBox = new ListBox();
            previewListBox.Items.Add(previewSong);
            View.ShowSong(previewSong, this.owner, previewListBox);
        }

        private void useDefaultStyleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.useDefaultStyleCheckBox.Checked)
            {
                this.comboStyle.SelectedItem = this._storage.Styles.FirstOrDefault(style => style.IsDefault);
                this.comboStyle.Enabled = false;
            }
            else
            {
                this.comboStyle.SelectedItem = this.song != null
                                                   ? this.song.Style
                                                   : this._storage.Styles.FirstOrDefault(style => style.IsDefault);
                this.comboStyle.Enabled = true;
            }
            
        }
    }
}