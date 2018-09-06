using System;
using System.Drawing;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
  public partial class ViewTitle : UserControl
  {
    public Color TitleBackgroundColor
    {
      get { return this.headerBackground.Appearance.BackColor; }
      set { this.headerBackground.Appearance.BackColor = value; }
    }

    private Color _titleForegroundColor;

    public Color TitleForegroundColor
    {
      get { return this._titleForegroundColor; }
      set
      {
          this._titleForegroundColor = value;
          this.number.Appearance.ForeColor = value;
          this.title.Appearance.ForeColor = value;
      }
    }

    private Font _titleFont;

    public Font TitleFont
    {
      get { return this._titleFont; }
      set
      {
          this._titleFont = value;
        if (value == null)
        {
            this.number.Appearance.ResetFontData();
            this.title.Appearance.ResetFontData();
        }
        else
        {
            this.number.Appearance.FontData.Name = value.Name;
            this.title.Appearance.FontData.Name = value.Name;

            this.number.Appearance.FontData.SizeInPoints = value.SizeInPoints;
            this.title.Appearance.FontData.SizeInPoints = value.SizeInPoints;
        }

          this.UpdateSize();
      }
    }

    private TitleMode _mode;

    public TitleMode Mode
    {
      get { return this._mode; }
      set
      {
          this._mode = value;
        switch (this.Mode)
        {
          case TitleMode.NumberAndTitle:
          default:
              this.number.Visible = true;
              this.title.Visible = true;
            this.Visible = true;
            break;
          case TitleMode.TitleOnly:
              this.number.Visible = false;
              this.title.Visible = true;
            this.Visible = true;
            break;
          case TitleMode.None:
            this.Visible = false;
            break;
        }
      }
    }

    public string Title
    {
      get { return this.title.Text; }
      set
      {
          this.title.Text = value;
          this.UpdateSize();
      }
    }

    public string Number
    {
      get { return this.number.Text; }
      set
      {
          this.number.Text = value;
          this.UpdateSize();
      }
    }

    private void UpdateSize()
    {
      using (var graphics = this.CreateGraphics())
      {
        var height = 8;
        if (!string.IsNullOrEmpty(this.Number) && this._titleFont != null)
        {
          height = Math.Max((int)graphics.MeasureString(this.Number, this._titleFont).Height, height);  
        }

        if (!string.IsNullOrEmpty(this.Title) && this._titleFont != null)
        {
          height = Math.Max((int)graphics.MeasureString(this.Title, this._titleFont).Height, height);
        }

        this.Height = height + 8;
      }
    }

    public ViewTitle()
    {
        this.InitializeComponent();
        this.headerBackground.Padding = new Padding(5);
    }
  }
}
