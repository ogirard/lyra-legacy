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
      get { return headerBackground.Appearance.BackColor; }
      set { headerBackground.Appearance.BackColor = value; }
    }

    private Color _titleForegroundColor;

    public Color TitleForegroundColor
    {
      get { return _titleForegroundColor; }
      set
      {
        _titleForegroundColor = value;
        number.Appearance.ForeColor = value;
        title.Appearance.ForeColor = value;
      }
    }

    private Font _titleFont;

    public Font TitleFont
    {
      get { return _titleFont; }
      set
      {
        _titleFont = value;
        if (value == null)
        {
          number.Appearance.ResetFontData();
          title.Appearance.ResetFontData();
        }
        else
        {
          number.Appearance.FontData.Name = value.Name;
          title.Appearance.FontData.Name = value.Name;

          number.Appearance.FontData.SizeInPoints = value.SizeInPoints;
          title.Appearance.FontData.SizeInPoints = value.SizeInPoints;
        }
        
        UpdateSize();
      }
    }

    private TitleMode _mode;

    public TitleMode Mode
    {
      get { return _mode; }
      set
      {
        _mode = value;
        switch (Mode)
        {
          case TitleMode.NumberAndTitle:
          default:
            number.Visible = true;
            title.Visible = true;
            this.Visible = true;
            break;
          case TitleMode.TitleOnly:
            number.Visible = false;
            title.Visible = true;
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
      get { return title.Text; }
      set
      {
        title.Text = value;
        UpdateSize();
      }
    }

    public string Number
    {
      get { return number.Text; }
      set
      {
        number.Text = value;
        UpdateSize();
      }
    }

    private void UpdateSize()
    {
      using (var graphics = CreateGraphics())
      {
        var height = 8;
        if (!string.IsNullOrEmpty(Number) && _titleFont != null)
        {
          height = Math.Max((int)graphics.MeasureString(Number, _titleFont).Height, height);  
        }

        if (!string.IsNullOrEmpty(Title) && _titleFont != null)
        {
          height = Math.Max((int)graphics.MeasureString(Title, _titleFont).Height, height);
        }

        this.Height = height + 8;
      }
    }

    public ViewTitle()
    {
      InitializeComponent();
      headerBackground.Padding = new Padding(5);
    }
  }
}
