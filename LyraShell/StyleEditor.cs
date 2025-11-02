using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
      get { return _selectedStyle; }
      private set
      {
        if (_selectedStyle != value)
        {
          _selectedStyle = value;
          stylesList.SelectedItem = value;
          UpdateStyleInfo();
          UpdatePreviewPanel();
          delStyleBtn.Enabled = !value.IsDefault;
          setDefaultStyleBtn.Enabled = !value.IsDefault;
          newFromSelectedBtn.Enabled = value != null;
        }
      }
    }

    private bool isUpdating = false;

    private void UpdateStyleInfo()
    {

      if (SelectedStyle == null)
      {
        return;
      }

      if (isUpdating) return;
        isUpdating = true;

      styleName.Text = SelectedStyle.Name;
      fontName.Value = SelectedStyle.Font;
      fontColor.Color = SelectedStyle.ForegroundColor ?? Util.COLOR;
      fontSize.Value = SelectedStyle.FontSize;
      bgColor.Color = SelectedStyle.BackgroundColor ?? Util.BGCOLOR;
      backgroundImage.Text = SelectedStyle.BackgroundImageUri;
      transparency.Value = SelectedStyle.Transparency;
      transPreviewImage.Image = Util.handlePic(false, Resources.TransparencyPreview, new Size(32, 32), true, transparency.Value);
      transLabel.Text = (transparency.MaxValue - transparency.Value) + @"%";
      scale.Checked = SelectedStyle.Scale;
      titleMode.Value = (int) SelectedStyle.TitleMode;
      titleForegroundColor.Color = SelectedStyle.TitleForegroundColor ?? Util.TITLECOLOR;
      titleBackgroundColor.Color = SelectedStyle.TitleBackgroundColor ?? Util.TITLEBGCOLOR;
      titleFont.Value = SelectedStyle.TitleFont;
      titleFontSize.Value = SelectedStyle.TitleFontSize;

      UpdatePreviewPanel();
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
      _styles = _storage.Styles;
      stylesList.BeginUpdate();
      foreach (var style in _styles)
      {
        stylesList.Items.Add(style);
      }
      if (_styles.Count > 0)
      {
        stylesList.SelectedIndex = _styles.IndexOf(_styles.First(s => s.IsDefault));
      }
      stylesList.EndUpdate();
    }

    private void OnSelectedIndexChangedHandler(object sender, EventArgs e)
    {
      if (stylesList.Items.Count == 0)
      {
        return;
      }

      if (stylesList.SelectedIndex < 0)
      {
        stylesList.SelectedIndex = 0;
      }
      else
      {
        SelectedStyle = (Style)stylesList.SelectedItem;
      }

      stylesListPanel.Enabled = true;
      saveCancelPanel.Visible = false;
      saveCancelPanel.Enabled = false;
    }

    private void OnStyleValueChanged(object sender, EventArgs e)
    {
      if (isUpdating) return;

      stylesListPanel.Enabled = false;
      saveCancelPanel.Visible = true;
      saveCancelPanel.Enabled = true;

      if (sender == transparency)
      {
        transPreviewImage.Image = Util.handlePic(false, Resources.TransparencyPreview, new Size(32, 32), true, transparency.Value);
        transLabel.Text = (transparency.MaxValue - transparency.Value) + @"%";
      }

      UpdatePreviewPanel();
    }

    private void CancelBtnClickHandler(object sender, EventArgs e)
    {
      // undo "new"
      if (SelectedStyle.IsNew)
      {
        stylesList.Items.Remove(SelectedStyle);
        SelectedStyle = stylesList.Items.Count > 0 ? stylesList.Items[0] as Style : null;
      }

      // reset
      UpdateStyleInfo();

      stylesListPanel.Enabled = true;
      saveCancelPanel.Visible = false;
      saveCancelPanel.Enabled = false;
    }

    private void SaveBtnClickHandler(object sender, EventArgs e)
    {
      // save current style
      SelectedStyle.Name = styleName.Text;
      SelectedStyle.Font = fontName.Value as string ?? "";
      SelectedStyle.ForegroundColor = fontColor.Color;
      SelectedStyle.FontSize = (int)fontSize.Value;
      SelectedStyle.BackgroundColor = bgColor.Color;
      SelectedStyle.BackgroundImageUri = backgroundImage.Text;
      SelectedStyle.Transparency = transparency.Value;
      SelectedStyle.Scale = scale.Checked;
      SelectedStyle.TitleMode = (TitleMode)titleMode.Value;
      SelectedStyle.TitleForegroundColor = titleForegroundColor.Color;
      SelectedStyle.TitleBackgroundColor = titleBackgroundColor.Color;
      SelectedStyle.TitleFont = titleFont.Value as string ?? "";
      SelectedStyle.TitleFontSize = (int)titleFontSize.Value;

      if (SelectedStyle.Save())
      {
        stylesListPanel.Enabled = true;
        saveCancelPanel.Visible = false;
        saveCancelPanel.Enabled = false;

        stylesList.BeginUpdate();
        var style = SelectedStyle;
        var idx = stylesList.SelectedIndex;
        stylesList.Items.Remove(style);
        stylesList.Items.Insert(idx, style);
        stylesList.SelectedIndex = idx;
        stylesList.EndUpdate();
      }
    }

    private void DeleteClickHandler(object sender, EventArgs e)
    {
      if (!_storage.IsStyleInUse(SelectedStyle) ||
            MessageBox.Show(this, @"Dieser Style wird von mindestens einem Lied verwendet." + Environment.NewLine +
                                                   @"Soll er trotzdem gelöscht werden?", @"Löschen bestätigen",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Exclamation) == DialogResult.Yes)
      {
        var styleToDelete = SelectedStyle;
        stylesList.Items.Remove(styleToDelete);
        SelectedStyle = stylesList.Items.Count > 0 ? stylesList.Items[0] as Style : null;
        styleToDelete.Delete();
      }
    }

    private void NewStyleClickHandler(object sender, EventArgs e)
    {
      var name = sender == newFromSelectedBtn && SelectedStyle != null ? SelectedStyle.Name + " (Kopie)"
          : "Neuer Style " + DateTime.Now.ToString("dd.MM.yyyy");
      var newStyle = Style.CreateNewStyle(_storage.PhysicalStorage, name);
      stylesList.Items.Add(newStyle);

      if (sender == newFromSelectedBtn && SelectedStyle != null)
      {
        newStyle.BackgroundColor = SelectedStyle.BackgroundColor;
        newStyle.BackgroundImageUri = SelectedStyle.BackgroundImageUri;
        newStyle.Font = SelectedStyle.Font;
        newStyle.FontSize = SelectedStyle.FontSize;
        newStyle.ForegroundColor = SelectedStyle.ForegroundColor;
        newStyle.Scale = SelectedStyle.Scale;
        newStyle.Transparency = SelectedStyle.Transparency;
        newStyle.TitleMode = SelectedStyle.TitleMode;
        newStyle.TitleForegroundColor = SelectedStyle.TitleForegroundColor;
        newStyle.TitleBackgroundColor = SelectedStyle.TitleBackgroundColor;
        newStyle.TitleFont = SelectedStyle.TitleFont;
        newStyle.TitleFontSize = SelectedStyle.TitleFontSize;
      }
      else
      {
        newStyle.Font = Util.FONT.Name;
        newStyle.FontSize = (int)Util.FONT.SizeInPoints;
        newStyle.ForegroundColor = Util.COLOR;
        newStyle.BackgroundColor = Util.BGCOLOR;
      }
      SelectedStyle = newStyle;

      stylesListPanel.Enabled = false;
      saveCancelPanel.Visible = true;
      saveCancelPanel.Enabled = true;
    }

    private void BrowseBackgroundBtnClickHandler(object sender, EventArgs e)
    {
      using (var openFileDialog = new OpenFileDialog())
      {
        openFileDialog.Title = @"Hintergrundbild auswählen...";

        if (!string.IsNullOrEmpty(backgroundImage.Text) && File.Exists(backgroundImage.Text))
        {
          openFileDialog.InitialDirectory = Path.GetDirectoryName(backgroundImage.Text);
        }
        else
        {
          openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        }
        openFileDialog.Filter = @"Bild Dateien|*.png; *.gif; *.jpg; *.bmp";
        if (openFileDialog.ShowDialog(this) == DialogResult.OK)
        {
          backgroundImage.Text = openFileDialog.FileName;
        }
      }
    }

    private void UpdatePreviewPanel()
    {
      var tempStyle = new Style
                        {
                          Name = styleName.Text,
                          Font = fontName.Value as string ?? "",
                          ForegroundColor = fontColor.Color,
                          FontSize = (int)fontSize.Value,
                          BackgroundColor = bgColor.Color,
                          BackgroundImageUri = backgroundImage.Text,
                          Transparency = transparency.Value,
                          Scale = scale.Checked,
                          TitleMode = (TitleMode)titleMode.Value,
                          TitleForegroundColor = titleForegroundColor.Color,
                          TitleBackgroundColor = titleBackgroundColor.Color,
                          TitleFont = titleFont.Value as string ?? "",
                          TitleFontSize = (int)titleFontSize.Value
                        };


      var previewBitmap = new Bitmap(400, previewPanel.ClientArea.Height);
      var g = Graphics.FromImage(previewBitmap);
      var clip = new Rectangle(0, 0, previewBitmap.Width, previewBitmap.Height);
      if (tempStyle.HasBackgroundImage)
      {
        g.DrawImage(tempStyle.GetBackgroundImage(new Size(clip.Width, clip.Height)), 0, 0);
      }
      else
      {
        g.FillRectangle(new SolidBrush(tempStyle.BackgroundColor ?? Util.BGCOLOR), clip);
      }

      exampleSongText.Font = tempStyle.GetFont() ?? Util.FONT;
      exampleSongText.ForeColor = tempStyle.ForegroundColor ?? Util.COLOR;
      previewPanel.Appearance.ImageBackground = previewBitmap;
      previewPanel.Appearance.ImageBackgroundStyle = ImageBackgroundStyle.Tiled;
      viewTitle.TitleFont = tempStyle.GetTitleFont() ?? Util.TITLEFONT;
      viewTitle.TitleBackgroundColor = tempStyle.TitleBackgroundColor ?? Util.TITLEBGCOLOR;
      viewTitle.TitleForegroundColor = tempStyle.TitleForegroundColor ?? Util.TITLECOLOR;
      viewTitle.Mode = tempStyle.TitleMode;

        exampleSongText.Top = viewTitle.Mode == TitleMode.None ? 10 : (viewTitle.Bottom + 10);
    }

    private void SetAsDefaultBtnClickHandler(object sender, EventArgs e)
    {
      _storage.SetStyleAsDefault(SelectedStyle);
      stylesList.BeginUpdate();
      var style = SelectedStyle;
      stylesList.Items.Clear();
      foreach (var s in _storage.Styles)
      {
        stylesList.Items.Add(s);
      }
      stylesList.SelectedItem = style;
      stylesList.EndUpdate();
      delStyleBtn.Enabled = false;
      setDefaultStyleBtn.Enabled = false;
    }
  }
}
