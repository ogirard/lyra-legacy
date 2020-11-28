using Infragistics.Win;
using Infragistics.Win.UltraWinToolbars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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
        private EditorTextBox editorTextBox;
        private GUI owner;
        private readonly IStorage _storage;
        private readonly IList<Style> styles;
        private Stack<(string, int, int, Point)> undo = new Stack<(string, int, int, Point)>();

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
        private Panel topPanel;
        private Panel middlePanel;
        private Panel bottomPanel;
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

        public bool UndoEnabled
        {
            get => this.toolbars.Toolbars[0].Tools["undo"].SharedProps.Enabled;
            set { this.toolbars.Toolbars[0].Tools["undo"].SharedProps.Enabled = value; }
        }

        public Editor(ISong song, GUI owner, IStorage storage)
        {
            this.song = song;
            this.owner = owner;
            this._storage = storage;
            this.styles = this._storage.Styles;

            open = true;
            editor = this;
            this.AcceptButton = this.button1;
            this.InitializeComponent();
            this.comboStyle.BeginUpdate();
            this.bottomPanel.Height = 93;
            foreach (var style in storage.Styles)
            {
                this.comboStyle.Items.Add(style);
            }
            this.comboStyle.EndUpdate();

            if (song != null)
            {
                this.textBox1.Text = song.Title;
                this.textBox2.Text = song.Number.ToString();
                this.editorTextBox.Text = song.Text;
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
                this.editorTextBox.Text = "";
                this.textBox2.Enabled = true;
                this.textBox3.Text = "---";
                this.checkBox1.Checked = false;
                this.textBox3.Enabled = false;
                this.useDefaultStyleCheckBox.Checked = true;
                this.comboStyle.SelectedItem = this.styles.FirstOrDefault(s => s.IsDefault);
            }

            this.toolbars.ToolClick += this.ToolbarsClickHandler;
            var combo = (ComboBoxTool)this.toolbars.Toolbars[0].Tools["indentCombo"];
            this.UndoEnabled = false;
            combo.SelectedIndex = 0;
        }

        private void OpenStyleEditorClickHandler(object sender, EventArgs e)
        {
            using (var se = new StyleEditor(this._storage))
            {
                se.ShowDialog(this);
                this.comboStyle.BeginUpdate();
                this.comboStyle.Items.Clear();
                foreach (var style in this.styles)
                {
                    this.comboStyle.Items.Add(style);
                }

                if (this.song != null && !this.song.UseDefaultStyle)
                {
                    this.comboStyle.SelectedItem = this.song.Style;
                }
                else
                {
                    this.comboStyle.SelectedItem = this.styles.FirstOrDefault(s => s.IsDefault);
                }

                this.comboStyle.EndUpdate();
            }
        }

        void ToolbarsClickHandler(object sender, ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "bold":    // ButtonTool
                    this.FormatText(Util.BOLD, false);
                    break;

                case "italic":    // ButtonTool
                    this.FormatText(Util.ITALIC, false);
                    break;

                case "refrain":    // ButtonTool
                    this.FormatText(Util.REF, true);
                    break;

                case "special":    // ButtonTool
                    this.FormatText(Util.SPEC, false);
                    break;

                case "tab":    // ButtonTool
                    this.undo.Push((this.editorTextBox.Rtf, this.editorTextBox.SelectionStart, this.editorTextBox.SelectionLength, this.editorTextBox.ScrollPosition));
                    this.editorTextBox.ModifyWithStableScroll(editor =>
                    {
                        var tabpos = editor.SelectionStart;
                        editor.Text = editor.Text.Insert(tabpos, '\t'.ToString());
                        editor.Focus();
                        editor.Select(tabpos + 1, 0);
                    });
                    break;

                case "indent":    // ButtonTool
                    var combo = (ComboBoxTool)this.toolbars.Toolbars[0].Tools["indentCombo"];
                    this.FormatText(Util.BLOCK + ((ValueListItem)combo.SelectedItem).DisplayText, true);
                    break;

                case "Seitenumbruch":    // ButtonTool
                    this.undo.Push((this.editorTextBox.Rtf, this.editorTextBox.SelectionStart, this.editorTextBox.SelectionLength, this.editorTextBox.ScrollPosition));
                    this.editorTextBox.ModifyWithStableScroll(editor =>
                    {
                        int findpos;
                        if ((findpos = editor.Find("<" + Util.PGBR + " />", 0, RichTextBoxFinds.MatchCase)) == -1)
                        {
                            var pos = editor.SelectionStart;
                            if ((pos > 0) && (editor.Text[pos - 1] != '\n'))
                            {
                                editor.Text = editor.Text.Insert(pos, "\n");
                                pos++;
                            }
                            editor.Text = editor.Text.Insert(pos, "\n<" + Util.PGBR + " />\n");
                            pos += Util.PGBR.Length + 6;
                            if ((pos < editor.Text.Length) && (editor.Text[pos] != '\n'))
                            {
                                editor.Text = editor.Text.Insert(pos, "\n");
                            }
                            editor.Focus();
                            editor.Select(pos + 1, 0);
                        }
                        else
                        {
                            Util.MBoxError("Nur ein Seitenumbruch möglich!");
                            editor.Focus();
                            editor.Select(findpos, 4 + Util.PGBR.Length);
                        }
                    });

                    break;

                case "jumpmark":    // ButtonTool
                    this.editorTextBox.ModifyWithStableScroll(editor =>
                    {
                        var ijm = new InsertJumpMark();
                        var line = editor.GetLineFromCharIndex(editor.SelectionStart) + 1;
                        ijm.JumpMarkName = "Sprungmarke Zeile " + line;
                        if (ijm.ShowDialog(this) == DialogResult.OK)
                        {
                            this.undo.Push((this.editorTextBox.Rtf, this.editorTextBox.SelectionStart, this.editorTextBox.SelectionLength, this.editorTextBox.ScrollPosition));
                            var jmpMarkPos = editor.SelectionStart;
                            var jumpMark = string.IsNullOrEmpty(ijm.JumpMarkName)
                                ? "Sprungmarke Zeile " + line
                                : ijm.JumpMarkName.Replace('\"', '\'');
                            jumpMark = "<" + Util.JMP + " name=\"" + jumpMark + "\" />";
                            editor.Text = editor.Text.Insert(jmpMarkPos, jumpMark);
                            editor.Focus();
                            editor.Select(jmpMarkPos + jumpMark.Length, 0);
                        }
                    });
                    break;
                case "undo":    // ButtonTool
                    if (this.undo.Count > 0)
                    {
                        var (rtf, start, length, scroll) = this.undo.Pop();
                        this.editorTextBox.Rtf = rtf;
                        this.editorTextBox.SelectionStart = start;
                        this.editorTextBox.SelectionLength = length;
                        this.editorTextBox.ScrollPosition = scroll;
                    }
                    else if (this.editorTextBox.CanUndo)
                    {
                        this.editorTextBox.Undo();
                    }
                    break;
            }

            this.UndoEnabled = this.editorTextBox.CanUndo || this.undo.Count > 0;
        }



        /// <summary>
        /// Die verwendeten Ressourcen bereinigen.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            open = false;
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
            this.components = new Container();
            UltraToolbar ultraToolbar1 = new UltraToolbar("editorToolBar");
            ButtonTool buttonTool1 = new ButtonTool("bold");
            ButtonTool buttonTool2 = new ButtonTool("italic");
            ButtonTool buttonTool3 = new ButtonTool("refrain");
            ButtonTool buttonTool4 = new ButtonTool("special");
            ButtonTool buttonTool5 = new ButtonTool("tab");
            ButtonTool buttonTool6 = new ButtonTool("indent");
            ComboBoxTool comboBoxTool1 = new ComboBoxTool("indentCombo");
            ButtonTool buttonTool8 = new ButtonTool("jumpmark");
            ButtonTool buttonTool7 = new ButtonTool("Seitenumbruch");
            ButtonTool buttonTool17 = new ButtonTool("undo");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            ButtonTool buttonTool9 = new ButtonTool("bold");
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            ButtonTool buttonTool10 = new ButtonTool("italic");
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            ButtonTool buttonTool11 = new ButtonTool("refrain");
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            ButtonTool buttonTool12 = new ButtonTool("special");
            ButtonTool buttonTool13 = new ButtonTool("tab");
            ButtonTool buttonTool14 = new ButtonTool("indent");
            ButtonTool buttonTool15 = new ButtonTool("Seitenumbruch");
            ButtonTool buttonTool16 = new ButtonTool("jumpmark");
            ComboBoxTool comboBoxTool2 = new ComboBoxTool("indentCombo");
            ValueList valueList1 = new ValueList(0);
            ValueListItem valueListItem1 = new ValueListItem();
            ValueListItem valueListItem2 = new ValueListItem();
            ValueListItem valueListItem3 = new ValueListItem();
            ValueListItem valueListItem4 = new ValueListItem();
            ValueListItem valueListItem5 = new ValueListItem();
            ButtonTool buttonTool18 = new ButtonTool("undo");
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Editor));
            this.textBox1 = new TextBox();
            this.textBox2 = new TextBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.editorTextBox = new EditorTextBox();
            this.listBox1 = new ListBox();
            this.label7 = new Label();
            this.textBox3 = new TextBox();
            this.checkBox1 = new CheckBox();
            this.lblStyle = new Label();
            this.label11 = new Label();
            this.toolbars = new UltraToolbarsManager(this.components);
            this.toolbarPanel = new Infragistics.Win.Misc.UltraPanel();
            this.ClientArea_Fill_Panel = new Panel();
            this._ClientArea_Toolbars_Dock_Area_Left = new UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Right = new UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Top = new UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Bottom = new UltraToolbarsDockArea();
            this.Editor_Fill_Panel = new Panel();
            this.useDefaultStyleCheckBox = new CheckBox();
            this.comboStyle = new ComboBox();
            this.previewBtn = new PictureBox();
            this.button12 = new LyraButtonControl();
            this.button10 = new LyraButtonControl();
            this.btnOpenStyleEditor = new LyraButtonControl();
            this.button2 = new LyraButtonControl();
            this.button1 = new LyraButtonControl();
            this.button14 = new LyraButtonControl();
            this.button16 = new LyraButtonControl();
            this.button17 = new LyraButtonControl();
            ((ISupportInitialize)(this.toolbars)).BeginInit();
            this.toolbarPanel.ClientArea.SuspendLayout();
            this.toolbarPanel.SuspendLayout();
            this.ClientArea_Fill_Panel.SuspendLayout();
            this.Editor_Fill_Panel.SuspendLayout();
            ((ISupportInitialize)(this.previewBtn)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new Point(240, 38);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(384, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "textBox1";
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new Point(102, 38);
            this.textBox2.MaxLength = 4;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Size(40, 20);
            this.textBox2.TabIndex = 0;
            this.textBox2.Text = "textBox2";
            // 
            // label1
            // 
            this.label1.Font = new Font("Verdana", 9.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = Color.SlateGray;
            this.label1.Location = new Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(160, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Lyra Songtext Editor";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new Point(24, 40);
            this.label2.Name = "label2";
            this.label2.Size = new Size(78, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Liednummer:";
            // 
            // label3
            // 
            this.label3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new Point(206, 40);
            this.label3.Name = "label3";
            this.label3.Size = new Size(32, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Titel:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new Point(24, 80);
            this.label4.Name = "label4";
            this.label4.Size = new Size(36, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Text:";
            // 
            // editorTextBox
            // 
            this.editorTextBox.Dock = DockStyle.Fill;
            this.editorTextBox.Margin = new Padding(24, 8, 24, 8);
            this.editorTextBox.Name = "editorTextBox";
            this.editorTextBox.TabIndex = 2;
            this.editorTextBox.Text = "editorTextBox";
            this.editorTextBox.BackColor = Color.White;
            // 
            // listBox1
            // 
            this.listBox1.Location = new Point(24, 100);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new Size(600, 69);
            this.listBox1.TabIndex = 25;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = Color.Black;
            this.label7.Location = new Point(24, 100);
            this.label7.Name = "label7";
            this.label7.Size = new Size(92, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Übersetzungen";
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Location = new Point(200, 64);
            this.textBox3.MaxLength = 3;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Size(40, 20);
            this.textBox3.TabIndex = 28;
            this.textBox3.Text = "---";
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new Point(104, 65);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(104, 16);
            this.checkBox1.TabIndex = 29;
            this.checkBox1.Text = "Bemerkungen:";
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            // 
            // lblStyle
            // 
            this.lblStyle.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.lblStyle.Location = new Point(24, 8);
            this.lblStyle.Name = "lblStyle";
            this.lblStyle.Size = new Size(68, 16);
            this.lblStyle.TabIndex = 6;
            this.lblStyle.Text = "Style :";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = Color.Transparent;
            this.label11.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = Color.DimGray;
            this.label11.Location = new Point(653, 68);
            this.label11.Name = "label11";
            this.label11.Size = new Size(52, 13);
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
            appearance1.Image = Properties.Resources.undo_icon;
            buttonTool17.InstanceProps.AppearancesSmall.Appearance = appearance1;
            buttonTool17.InstanceProps.IsFirstInGroup = true;
            ultraToolbar1.NonInheritedTools.AddRange(new ToolBase[] {
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
            ultraToolbar1.Settings.AllowCustomize = DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockBottom = DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockLeft = DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockRight = DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowFloating = DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowHiding = DefaultableBoolean.False;
            ultraToolbar1.Settings.FillEntireRow = DefaultableBoolean.True;
            ultraToolbar1.Settings.GrabHandleStyle = GrabHandleStyle.NoGlyph;
            ultraToolbar1.Settings.ToolDisplayStyle = ToolDisplayStyle.TextOnlyAlways;
            ultraToolbar1.Text = "editorToolBar";
            this.toolbars.Toolbars.AddRange(new UltraToolbar[] {
            ultraToolbar1});
            this.toolbars.ToolbarSettings.BorderStyleDocked = UIElementBorderStyle.Etched;
            appearance14.BackColor = Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance14.BackColor2 = Color.Silver;
            appearance14.BackGradientStyle = GradientStyle.GlassTop37;
            appearance14.BorderColor = Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.toolbars.ToolbarSettings.DockedAppearance = appearance14;
            appearance16.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(178)))));
            appearance16.BackColor2 = Color.Silver;
            appearance16.BackGradientStyle = GradientStyle.VerticalWithGlassLeft50;
            appearance16.BorderColor = Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
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
            valueList1.DisplayStyle = ValueListDisplayStyle.DisplayText;
            valueList1.PreferredDropDownSize = new Size(0, 0);
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
            valueList1.ValueListItems.AddRange(new ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3,
            valueListItem4,
            valueListItem5});
            comboBoxTool2.ValueList = valueList1;
            buttonTool18.SharedPropsInternal.Caption = "Rückgängig";
            buttonTool18.SharedPropsInternal.DisplayStyle = ToolDisplayStyle.DefaultForToolType;
            this.toolbars.Tools.AddRange(new ToolBase[] {
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
            this.toolbarPanel.Location = new Point(27, 88);
            this.toolbarPanel.Name = "toolbarPanel";
            this.toolbarPanel.Dock = DockStyle.Fill;
            this.toolbarPanel.Size = new Size(685, 261);
            this.toolbarPanel.TabIndex = 34;
            // 
            // ClientArea_Fill_Panel
            // 
            this.ClientArea_Fill_Panel.Controls.Add(this.editorTextBox);
            this.ClientArea_Fill_Panel.Cursor = Cursors.Default;
            this.ClientArea_Fill_Panel.Dock = DockStyle.Fill;
            this.ClientArea_Fill_Panel.Location = new Point(0, 31);
            this.ClientArea_Fill_Panel.Name = "ClientArea_Fill_Panel";
            this.ClientArea_Fill_Panel.Size = new Size(685, 230);
            this.ClientArea_Fill_Panel.TabIndex = 0;
            // 
            // _ClientArea_Toolbars_Dock_Area_Left
            // 
            this._ClientArea_Toolbars_Dock_Area_Left.AccessibleRole = AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Left.BackColor = SystemColors.Control;
            this._ClientArea_Toolbars_Dock_Area_Left.DockedPosition = DockedPosition.Left;
            this._ClientArea_Toolbars_Dock_Area_Left.ForeColor = SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Left.Location = new Point(0, 31);
            this._ClientArea_Toolbars_Dock_Area_Left.Name = "_ClientArea_Toolbars_Dock_Area_Left";
            this._ClientArea_Toolbars_Dock_Area_Left.Size = new Size(0, 230);
            this._ClientArea_Toolbars_Dock_Area_Left.ToolbarsManager = this.toolbars;
            // 
            // _ClientArea_Toolbars_Dock_Area_Right
            // 
            this._ClientArea_Toolbars_Dock_Area_Right.AccessibleRole = AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Right.BackColor = SystemColors.Control;
            this._ClientArea_Toolbars_Dock_Area_Right.DockedPosition = DockedPosition.Right;
            this._ClientArea_Toolbars_Dock_Area_Right.ForeColor = SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Right.Location = new Point(685, 31);
            this._ClientArea_Toolbars_Dock_Area_Right.Name = "_ClientArea_Toolbars_Dock_Area_Right";
            this._ClientArea_Toolbars_Dock_Area_Right.Size = new Size(0, 230);
            this._ClientArea_Toolbars_Dock_Area_Right.ToolbarsManager = this.toolbars;
            // 
            // _ClientArea_Toolbars_Dock_Area_Top
            // 
            this._ClientArea_Toolbars_Dock_Area_Top.AccessibleRole = AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Top.BackColor = SystemColors.Control;
            this._ClientArea_Toolbars_Dock_Area_Top.DockedPosition = DockedPosition.Top;
            this._ClientArea_Toolbars_Dock_Area_Top.ForeColor = SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Top.Location = new Point(0, 0);
            this._ClientArea_Toolbars_Dock_Area_Top.Name = "_ClientArea_Toolbars_Dock_Area_Top";
            this._ClientArea_Toolbars_Dock_Area_Top.Size = new Size(685, 31);
            this._ClientArea_Toolbars_Dock_Area_Top.ToolbarsManager = this.toolbars;
            // 
            // _ClientArea_Toolbars_Dock_Area_Bottom
            // 
            this._ClientArea_Toolbars_Dock_Area_Bottom.AccessibleRole = AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Bottom.BackColor = SystemColors.Control;
            this._ClientArea_Toolbars_Dock_Area_Bottom.DockedPosition = DockedPosition.Bottom;
            this._ClientArea_Toolbars_Dock_Area_Bottom.ForeColor = SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Bottom.Location = new Point(0, 261);
            this._ClientArea_Toolbars_Dock_Area_Bottom.Name = "_ClientArea_Toolbars_Dock_Area_Bottom";
            this._ClientArea_Toolbars_Dock_Area_Bottom.Size = new Size(685, 0);
            this._ClientArea_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.toolbars;
            // 
            // Editor_Fill_Panel
            // 
            this.topPanel = new Panel { Name = "topPanel", Dock = DockStyle.Top, Height = 93 };
            this.middlePanel = new Panel
            {
                Name = "middlePanel",
                Dock = DockStyle.Fill,
                Height = 93,
                DockPadding = { Left = 24, Right = 24, Top = 8, Bottom = 8 }
            };

            this.bottomPanel = new Panel { Name = "bottomPanel", Dock = DockStyle.Bottom };
            var okCancelPanel = new Panel { Name = "okCancelPanel", Dock = DockStyle.Right, Width = 155 };
            okCancelPanel.Controls.Add(this.button2);
            okCancelPanel.Controls.Add(this.button1);
            bottomPanel.Controls.Add(okCancelPanel);


            this.Editor_Fill_Panel.Controls.Add(this.middlePanel);
            this.middlePanel.Controls.Add(this.toolbarPanel);
            this.toolbarPanel.Margin = new Padding(24, 8, 24, 8);
            this.Editor_Fill_Panel.Controls.Add(topPanel);
            this.Editor_Fill_Panel.Controls.Add(bottomPanel);
            topPanel.Controls.Add(this.label11);
            topPanel.Controls.Add(this.previewBtn);
            topPanel.Controls.Add(this.textBox3);
            topPanel.Controls.Add(this.label4);
            topPanel.Controls.Add(this.label2);
            topPanel.Controls.Add(this.textBox2);
            topPanel.Controls.Add(this.textBox1);
            topPanel.Controls.Add(this.checkBox1);
            topPanel.Controls.Add(this.button12);
            topPanel.Controls.Add(this.label1);
            topPanel.Controls.Add(this.label3);

            bottomPanel.Controls.Add(this.listBox1);
            bottomPanel.Controls.Add(this.label7);
            bottomPanel.Controls.Add(this.useDefaultStyleCheckBox);
            bottomPanel.Controls.Add(this.button10);
            bottomPanel.Controls.Add(this.comboStyle);
            bottomPanel.Controls.Add(this.btnOpenStyleEditor);

            bottomPanel.Controls.Add(this.button14);
            bottomPanel.Controls.Add(this.button16);
            bottomPanel.Controls.Add(this.button17);
            bottomPanel.Controls.Add(this.lblStyle);
            this.Editor_Fill_Panel.Cursor = Cursors.Default;
            this.Editor_Fill_Panel.Dock = DockStyle.Fill;
            this.Editor_Fill_Panel.Location = new Point(0, 0);
            this.Editor_Fill_Panel.Name = "Editor_Fill_Panel";
            this.Editor_Fill_Panel.Size = new Size(736, 544);
            this.Editor_Fill_Panel.TabIndex = 0;
            // 
            // useDefaultStyleCheckBox
            // 
            this.useDefaultStyleCheckBox.AutoSize = true;
            this.useDefaultStyleCheckBox.Location = new Point(75, 36);
            this.useDefaultStyleCheckBox.Name = "useDefaultStyleCheckBox";
            this.useDefaultStyleCheckBox.Size = new Size(182, 17);
            this.useDefaultStyleCheckBox.TabIndex = 36;
            this.useDefaultStyleCheckBox.Text = "Immer Standard Style verwenden";
            this.useDefaultStyleCheckBox.UseVisualStyleBackColor = true;
            this.useDefaultStyleCheckBox.CheckedChanged += new EventHandler(this.useDefaultStyleCheckBox_CheckedChanged);
            // 
            // comboStyle
            // 
            this.comboStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboStyle.FormattingEnabled = true;
            this.comboStyle.Location = new Point(73, 8);
            this.comboStyle.Name = "comboStyle";
            this.comboStyle.Size = new Size(257, 21);
            this.comboStyle.TabIndex = 35;
            // 
            // previewBtn
            // 
            this.previewBtn.Cursor = Cursors.Hand;
            this.previewBtn.Image = ((Image)(resources.GetObject("previewBtn.Image")));
            this.previewBtn.Location = new Point(648, 5);
            this.previewBtn.Name = "previewBtn";
            this.previewBtn.Size = new Size(64, 64);
            this.previewBtn.TabIndex = 33;
            this.previewBtn.TabStop = false;
            this.previewBtn.Click += new EventHandler(this.previewButton_Click);
            // 
            // button12
            // 
            this.button12.Enabled = false;
            this.button12.Location = new Point(149, 38);
            this.button12.Name = "button12";
            this.button12.Size = new Size(53, 20);
            this.button12.TabIndex = 27;
            this.button12.Text = "ändern";
            this.button12.Click += new EventHandler(this.button12_Click);
            // 
            // button10
            // 
            this.button10.BackColorInternal = SystemColors.Control;
            this.button10.Location = new Point(24, 62);
            this.button10.Name = "button10";
            this.button10.Size = new Size(160, 24);
            this.button10.TabIndex = 21;
            this.button10.Text = "Übersetzungen >>";
            this.button10.Click += new EventHandler(this.button10_Click);
            // 
            // btnOpenStyleEditor
            // 
            this.btnOpenStyleEditor.BackColorInternal = SystemColors.Control;
            this.btnOpenStyleEditor.Location = new Point(339, 8);
            this.btnOpenStyleEditor.Name = "btnOpenStyleEditor";
            this.btnOpenStyleEditor.Size = new Size(94, 23);
            this.btnOpenStyleEditor.TabIndex = 20;
            this.btnOpenStyleEditor.Text = "Style Editor ...";
            this.btnOpenStyleEditor.Click += new EventHandler(this.OpenStyleEditorClickHandler);
            // 
            // button2
            // 
            this.button2.BackColorInternal = SystemColors.Control;
            this.button2.Location = new Point(8, 8);
            this.button2.Name = "button2";
            this.button2.Size = new Size(72, 24);
            this.button2.TabIndex = 20;
            this.button2.Text = "Abbrechen";
            this.button2.Click += new EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColorInternal = SystemColors.Control;
            this.button1.Location = new Point(88, 8);
            this.button1.Name = "button1";
            this.button1.Size = new Size(40, 24);
            this.button1.TabIndex = 19;
            this.button1.Text = "Ok";
            this.button1.Click += new EventHandler(this.button1_Click);
            // 
            // button14
            // 
            this.button14.Location = new Point(688, 128);
            this.button14.Name = "button14";
            this.button14.Size = new Size(40, 24);
            this.button14.TabIndex = 24;
            this.button14.Text = "Del";
            this.button14.Click += new EventHandler(this.button14_Click);
            // 
            // button16
            // 
            this.button16.Location = new Point(632, 100);
            this.button16.Name = "button16";
            this.button16.Size = new Size(96, 24);
            this.button16.TabIndex = 22;
            this.button16.Text = "Neu…";
            this.button16.Click += new EventHandler(this.button16_Click);
            // 
            // button17
            // 
            this.button17.Location = new Point(632, 128);
            this.button17.Name = "button17";
            this.button17.Size = new Size(40, 24);
            this.button17.TabIndex = 23;
            this.button17.Text = "Edit";
            this.button17.Click += new EventHandler(this.button17_Click);
            // 
            // Editor
            // 
            this.AutoScaleBaseSize = new Size(5, 13);
            this.ControlBox = true;
            this.MinimizeBox = false;
            this.ShowIcon = false;
            this.Controls.Add(this.Editor_Fill_Panel);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(750, 550);
            this.ClientSize = new Size(950, 800);
            this.Name = "Editor";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Editor";
            this.Activated += new EventHandler(this.MeGotFocus);
            ((ISupportInitialize)(this.toolbars)).EndInit();
            this.toolbarPanel.ClientArea.ResumeLayout(false);
            this.toolbarPanel.ResumeLayout(false);
            this.ClientArea_Fill_Panel.ResumeLayout(false);
            this.Editor_Fill_Panel.ResumeLayout(false);
            this.Editor_Fill_Panel.PerformLayout();
            ((ISupportInitialize)(this.previewBtn)).EndInit();
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
            var title = this.textBox1.Text;
            var text = this.editorTextBox.Text;
            var desc = this.textBox3.Text == @"---" ? "" : this.textBox3.Text;
            if (this.toDel != null && nr == this.toDel.Number) // number hasn't changed!
            {
                this.song = this.toDel;
            }
            if (this.song != null)
            {
                this.song.Title = title;
                this.song.Text = text;
                this.song.Desc = desc;
                this.song.Style = this.comboStyle.SelectedItem as Style;
                this.song.UseDefaultStyle = this.useDefaultStyleCheckBox.Checked;
                this.owner.Status = "Liedtext editiert...";
            }
            else
            {
                var id = "s" + Util.toFour(nr);
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
                    var msg = "Diese Liednummer wurde bereits einmal verwendet!\n";
                    msg += "Soll das Lied trotzdem hinzugefügt werden?\n";
                    msg += "(evtl. haben Sie das Lied bereits einer anderen Nummer zugewiesen,\n";
                    msg += "in diesem Fall einfach \"Ja\" klicken!)";

                    var dr = MessageBox.Show(this, msg, "lyra - neues Lied hinzufügen",
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

        private void FormatText(string tag, bool nl)
        {
            this.undo.Push((this.editorTextBox.Rtf, this.editorTextBox.SelectionStart, this.editorTextBox.SelectionLength, this.editorTextBox.ScrollPosition));
            this.editorTextBox.ModifyWithStableScroll(editor =>
            {
                var left = editor.SelectionStart;
                var right = editor.SelectionLength + left + tag.Length + 2;
                editor.Text = editor.Text.Insert(left, "<" + tag + ">");
                editor.Text = editor.Text.Insert(right, "</" + tag + ">");
                if (nl)
                {
                    if (((right + tag.Length + 3) < editor.Text.Length) &&
                        (editor.Text[right + tag.Length + 3] != '\n'))
                    {
                        editor.Text = editor.Text.Insert(right + tag.Length + 3, "\n");
                    }
                    if ((left > 0) && (editor.Text[left - 1] != '\n'))
                    {
                        editor.Text = editor.Text.Insert(left, "\n");
                        right++;
                    }
                }
                editor.Focus();
                editor.Select(right, 0);
            });

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
                var add = MessageBox.Show(this, @"Das neue Lied muss zuerst gespeichert werden." + Environment.NewLine +
                                                         @"Soll das Lied jetzt hinzugefügt werden?", @"lyra Hinweis",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Warning);
                if (add == DialogResult.No)
                {
                    return;
                }
                var title = this.textBox1.Text;
                var text = this.editorTextBox.Text;
                var id = "s" + Util.toFour(nr);
                var desc = this.checkBox1.Checked ? this.textBox3.Text : "";
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
                    var msg = "Diese Liednummer wurde bereits einmal verwendet!\n";
                    msg += "Soll das Lied trotzdem hinzugefügt werden?";

                    var dr = MessageBox.Show(this, msg, "lyra - neues Lied hinzufügen",
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
                this.bottomPanel.Height = 180;
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
                this.bottomPanel.Height = 93;
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
            var id = Util.PREVIEW_SONG_ID;
            var title = this.textBox1.Text;
            var text = this.editorTextBox.Text;
            var nr = 0;
            try
            {
                nr = int.Parse(this.textBox2.Text);
            }
            catch (Exception)
            { }
            var desc = this.textBox3.Text == @"---" ? "" : this.textBox3.Text;
            ISong previewSong = new Song(nr, title, text, id, desc, true);
            previewSong.Style = this.comboStyle.SelectedItem as Style;
            var previewListBox = new ListBox();
            previewListBox.Items.Add(previewSong);
            View.ShowSong(previewSong, this.owner, previewListBox, "Vorschau aus Editor");
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