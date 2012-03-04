namespace Lyra2.LyraShell
{
  partial class ViewTitle
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
      Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
      Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
      this.headerBackground = new Infragistics.Win.Misc.UltraPanel();
      this.title = new Infragistics.Win.Misc.UltraLabel();
      this.number = new Infragistics.Win.Misc.UltraLabel();
      this.headerBackground.ClientArea.SuspendLayout();
      this.headerBackground.SuspendLayout();
      this.SuspendLayout();
      // 
      // headerBackground
      // 
      this.headerBackground.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      appearance2.TextVAlignAsString = "Middle";
      this.headerBackground.Appearance = appearance2;
      // 
      // headerBackground.ClientArea
      // 
      this.headerBackground.ClientArea.Controls.Add(this.title);
      this.headerBackground.ClientArea.Controls.Add(this.number);
      this.headerBackground.Location = new System.Drawing.Point(3, 3);
      this.headerBackground.Name = "headerBackground";
      this.headerBackground.Size = new System.Drawing.Size(462, 50);
      this.headerBackground.TabIndex = 0;
      // 
      // title
      // 
      appearance4.FontData.SizeInPoints = 20F;
      appearance4.TextVAlignAsString = "Middle";
      this.title.Appearance = appearance4;
      this.title.AutoSize = true;
      this.title.Dock = System.Windows.Forms.DockStyle.Fill;
      this.title.Location = new System.Drawing.Point(70, 0);
      this.title.Name = "title";
      this.title.Size = new System.Drawing.Size(392, 50);
      this.title.TabIndex = 1;
      this.title.Text = "1050";
      // 
      // number
      // 
      appearance3.FontData.SizeInPoints = 20F;
      appearance3.TextVAlignAsString = "Middle";
      this.number.Appearance = appearance3;
      this.number.AutoSize = true;
      this.number.Dock = System.Windows.Forms.DockStyle.Left;
      this.number.Location = new System.Drawing.Point(0, 0);
      this.number.Name = "number";
      this.number.Size = new System.Drawing.Size(70, 50);
      this.number.TabIndex = 0;
      this.number.Text = "1050";
      // 
      // ViewTitle
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.headerBackground);
      this.Name = "ViewTitle";
      this.Size = new System.Drawing.Size(468, 56);
      this.headerBackground.ClientArea.ResumeLayout(false);
      this.headerBackground.ClientArea.PerformLayout();
      this.headerBackground.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private Infragistics.Win.Misc.UltraPanel headerBackground;
    private Infragistics.Win.Misc.UltraLabel number;
    private Infragistics.Win.Misc.UltraLabel title;
  }
}
