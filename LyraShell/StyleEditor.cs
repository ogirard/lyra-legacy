using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Infragistics.Win;
using Resources = Lyra2.LyraShell.Properties.Resources;

namespace Lyra2.LyraShell
{
  public partial class StyleEditor : Form
  {
    private readonly IStorage _storage;
    private readonly IList<Style> _styles;

    private Style _selectedStyle;

    public Style SelectedStyle
    {
      get { return this._selectedStyle; }
      private set
      {
        if (this._selectedStyle != value)
        {
          this._selectedStyle = value;
          this.stylesList.SelectedItem = value;
          this.UpdateStyleInfo();
          this.UpdatePreviewPanel();
          this.delStyleBtn.Enabled = !value.IsDefault;
          this.setDefaultStyleBtn.Enabled = !value.IsDefault;
          this.newFromSelectedBtn.Enabled = value != null;
        }
      }
    }

    private bool isUpdating = false;

    private void UpdateStyleInfo()
    {

      if (this.SelectedStyle == null)
      {
        return;
      }

      if (isUpdating) return;
      isUpdating = true;

      this.styleName.Text = this.SelectedStyle.Name;
      this.fontName.Value = this.SelectedStyle.Font;
      this.fontColor.Color = this.SelectedStyle.ForegroundColor ?? Util.COLOR;
      this.fontSize.Value = this.SelectedStyle.FontSize;
      this.bgColor.Color = this.SelectedStyle.BackgroundColor ?? Util.BGCOLOR;
      this.backgroundImage.Text = this.SelectedStyle.BackgroundImageUri;
      this.transparency.Value = this.SelectedStyle.Transparency;
      this.transPreviewImage.Image = Util.handlePic(false, Resources.TransparencyPreview, new Size(32, 32), true, this.transparency.Value);
      this.transLabel.Text = (this.transparency.MaxValue - this.transparency.Value) + @"%";
      this.scale.Checked = this.SelectedStyle.Scale;
      this.titleMode.Value = (int)SelectedStyle.TitleMode;
      this.titleForegroundColor.Color = this.SelectedStyle.TitleForegroundColor ?? Util.TITLECOLOR;
      this.titleBackgroundColor.Color = this.SelectedStyle.TitleBackgroundColor ?? Util.TITLEBGCOLOR;
      this.titleFont.Value = this.SelectedStyle.TitleFont;
      this.titleFontSize.Value = this.SelectedStyle.TitleFontSize;

      this.UpdatePreviewPanel();
      isUpdating = false;
    }

    public StyleEditor()
    {
      InitializeComponent();
    }

    public StyleEditor(IStorage storage)
      : this()
    {
      _storage = storage;
      this._styles = _storage.Styles;
      this.stylesList.BeginUpdate();
      foreach (Style style in this._styles)
      {
        this.stylesList.Items.Add(style);
      }
      if (this._styles.Count > 0)
      {
        this.stylesList.SelectedIndex = 0;
      }
      this.stylesList.EndUpdate();
    }

    private void OnSelectedIndexChangedHandler(object sender, EventArgs e)
    {
      if (this.stylesList.Items.Count == 0)
      {
        return;
      }

      if (this.stylesList.SelectedIndex < 0)
      {
        this.stylesList.SelectedIndex = 0;
      }
      else
      {
        this.SelectedStyle = (Style)this.stylesList.SelectedItem;
      }

      this.stylesListPanel.Enabled = true;
      this.saveCancelPanel.Visible = false;
      this.saveCancelPanel.Enabled = false;
    }

    private void OnStyleValueChanged(object sender, EventArgs e)
    {
      if (isUpdating) return;

      this.stylesListPanel.Enabled = false;
      this.saveCancelPanel.Visible = true;
      this.saveCancelPanel.Enabled = true;

      if (sender == this.transparency)
      {
        this.transPreviewImage.Image = Util.handlePic(false, Resources.TransparencyPreview, new Size(32, 32), true, this.transparency.Value);
        this.transLabel.Text = (this.transparency.MaxValue - this.transparency.Value) + @"%";
      }

      this.UpdatePreviewPanel();
    }

    private void CancelBtnClickHandler(object sender, EventArgs e)
    {
      // undo "new"
      if (this.SelectedStyle.IsNew)
      {
        this.stylesList.Items.Remove(this.SelectedStyle);
        this.SelectedStyle = this.stylesList.Items.Count > 0 ? this.stylesList.Items[0] as Style : null;
      }

      // reset
      this.UpdateStyleInfo();

      this.stylesListPanel.Enabled = true;
      this.saveCancelPanel.Visible = false;
      this.saveCancelPanel.Enabled = false;
    }

    private void SaveBtnClickHandler(object sender, EventArgs e)
    {
      // save current style
      this.SelectedStyle.Name = this.styleName.Text;
      this.SelectedStyle.Font = this.fontName.Value as string ?? "";
      this.SelectedStyle.ForegroundColor = this.fontColor.Color;
      this.SelectedStyle.FontSize = (int)this.fontSize.Value;
      this.SelectedStyle.BackgroundColor = this.bgColor.Color;
      this.SelectedStyle.BackgroundImageUri = this.backgroundImage.Text;
      this.SelectedStyle.Transparency = this.transparency.Value;
      this.SelectedStyle.Scale = this.scale.Checked;
      this.SelectedStyle.TitleMode = (TitleMode)this.titleMode.Value;
      this.SelectedStyle.TitleForegroundColor = this.titleForegroundColor.Color;
      this.SelectedStyle.TitleBackgroundColor = this.titleBackgroundColor.Color;
      this.SelectedStyle.TitleFont = this.titleFont.Value as string ?? "";
      this.SelectedStyle.TitleFontSize = (int)this.titleFontSize.Value;

      if (this.SelectedStyle.Save())
      {
        this.stylesListPanel.Enabled = true;
        this.saveCancelPanel.Visible = false;
        this.saveCancelPanel.Enabled = false;

        this.stylesList.BeginUpdate();
        var style = this.SelectedStyle;
        var idx = stylesList.SelectedIndex;
        this.stylesList.Items.Remove(style);
        this.stylesList.Items.Insert(idx, style);
        this.stylesList.SelectedIndex = idx;
        this.stylesList.EndUpdate();
      }
    }

    private void DeleteClickHandler(object sender, EventArgs e)
    {
      if (!this._storage.IsStyleInUse(this.SelectedStyle) ||
            MessageBox.Show(this, @"Dieser Style wird von mindestens einem Lied verwendet." + Environment.NewLine +
                                                   @"Soll er trotzdem gelöscht werden?", @"Löschen bestätigen",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Exclamation) == DialogResult.Yes)
      {
        var styleToDelete = this.SelectedStyle;
        this.stylesList.Items.Remove(styleToDelete);
        this.SelectedStyle = this.stylesList.Items.Count > 0 ? this.stylesList.Items[0] as Style : null;
        styleToDelete.Delete();
      }
    }

    private void NewStyleClickHandler(object sender, EventArgs e)
    {
      var name = sender == this.newFromSelectedBtn && this.SelectedStyle != null ? this.SelectedStyle.Name + " (Kopie)"
          : "Neuer Style " + DateTime.Now.ToString("dd.MM.yyyy");
      var newStyle = Style.CreateNewStyle(this._storage.PhysicalStorage, name);
      this.stylesList.Items.Add(newStyle);

      if (sender == this.newFromSelectedBtn && this.SelectedStyle != null)
      {
        newStyle.BackgroundColor = this.SelectedStyle.BackgroundColor;
        newStyle.BackgroundImageUri = this.SelectedStyle.BackgroundImageUri;
        newStyle.Font = this.SelectedStyle.Font;
        newStyle.FontSize = this.SelectedStyle.FontSize;
        newStyle.ForegroundColor = this.SelectedStyle.ForegroundColor;
        newStyle.Scale = this.SelectedStyle.Scale;
        newStyle.Transparency = this.SelectedStyle.Transparency;
        newStyle.TitleMode = SelectedStyle.TitleMode;
        newStyle.TitleForegroundColor = this.SelectedStyle.TitleForegroundColor;
        newStyle.TitleBackgroundColor = this.SelectedStyle.TitleBackgroundColor;
        newStyle.TitleFont = this.SelectedStyle.TitleFont;
        newStyle.TitleFontSize = this.SelectedStyle.TitleFontSize;
      }
      else
      {
        newStyle.Font = Util.FONT.Name;
        newStyle.FontSize = (int)Util.FONT.SizeInPoints;
        newStyle.ForegroundColor = Util.COLOR;
        newStyle.BackgroundColor = Util.BGCOLOR;
      }
      this.SelectedStyle = newStyle;

      this.stylesListPanel.Enabled = false;
      this.saveCancelPanel.Visible = true;
      this.saveCancelPanel.Enabled = true;
    }

    private void BrowseBackgroundBtnClickHandler(object sender, EventArgs e)
    {
      using (var openFileDialog = new OpenFileDialog())
      {
        openFileDialog.Title = @"Hintergrundbild auswählen...";

        if (!string.IsNullOrEmpty(this.backgroundImage.Text) && File.Exists(this.backgroundImage.Text))
        {
          openFileDialog.InitialDirectory = Path.GetDirectoryName(this.backgroundImage.Text);
        }
        else
        {
          openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        }
        openFileDialog.Filter = @"Bild Dateien|*.png; *.gif; *.jpg; *.bmp";
        if (openFileDialog.ShowDialog(this) == DialogResult.OK)
        {
          this.backgroundImage.Text = openFileDialog.FileName;
        }
      }
    }

    private void UpdatePreviewPanel()
    {
      var tempStyle = new Style
                        {
                          Name = this.styleName.Text,
                          Font = this.fontName.Value as string ?? "",
                          ForegroundColor = this.fontColor.Color,
                          FontSize = (int)this.fontSize.Value,
                          BackgroundColor = this.bgColor.Color,
                          BackgroundImageUri = this.backgroundImage.Text,
                          Transparency = this.transparency.Value,
                          Scale = this.scale.Checked,
                          TitleMode = (TitleMode)this.titleMode.Value,
                          TitleForegroundColor = this.titleForegroundColor.Color,
                          TitleBackgroundColor = this.titleBackgroundColor.Color,
                          TitleFont = this.titleFont.Value as string ?? "",
                          TitleFontSize = (int)this.titleFontSize.Value
                        };


      Bitmap previewBitmap = new Bitmap(400, this.previewPanel.ClientArea.Height);
      Graphics g = Graphics.FromImage(previewBitmap);
      Rectangle clip = new Rectangle(0, 0, previewBitmap.Width, previewBitmap.Height);
      if (tempStyle.HasBackgroundImage)
      {
        g.DrawImage(tempStyle.GetBackgroundImage(new Size(clip.Width, clip.Height)), 0, 0);
      }
      else
      {
        g.FillRectangle(new SolidBrush(tempStyle.BackgroundColor ?? Util.BGCOLOR), clip);
      }

      this.exampleSongText.Font = tempStyle.GetFont() ?? Util.FONT;
      this.exampleSongText.ForeColor = tempStyle.ForegroundColor ?? Util.COLOR;
      this.previewPanel.Appearance.ImageBackground = previewBitmap;
      this.previewPanel.Appearance.ImageBackgroundStyle = ImageBackgroundStyle.Tiled;
      this.viewTitle.TitleFont = tempStyle.GetTitleFont() ?? Util.TITLEFONT;
      this.viewTitle.TitleBackgroundColor = tempStyle.TitleBackgroundColor ?? Util.TITLEBGCOLOR;
      this.viewTitle.TitleForegroundColor = tempStyle.TitleForegroundColor ?? Util.TITLECOLOR;
      this.viewTitle.Mode = tempStyle.TitleMode;

      exampleSongText.Top = viewTitle.Mode == TitleMode.None ? 10 : (viewTitle.Bottom + 10);
    }

    private void SetAsDefaultBtnClickHandler(object sender, EventArgs e)
    {
      this._storage.SetStyleAsDefault(this.SelectedStyle);
      this.stylesList.BeginUpdate();
      var style = this.SelectedStyle;
      this.stylesList.Items.Clear();
      foreach (Style s in this._storage.Styles)
      {
        this.stylesList.Items.Add(s);
      }
      this.stylesList.SelectedItem = style;
      this.stylesList.EndUpdate();
      this.delStyleBtn.Enabled = false;
      this.setDefaultStyleBtn.Enabled = false;
    }
  }
}
