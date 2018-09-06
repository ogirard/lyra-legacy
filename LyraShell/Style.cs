using System;
using System.Drawing;
using System.IO;
using System.Xml;
using Lyra2.UtilShared;

namespace Lyra2.LyraShell
{
  public class Style
  {
    private bool _isNew;
    private readonly IPhysicalStorage _storage;
    private readonly Guid _id;

    private Color? _foregroundColor;
    private Color? _backgroundColor;
    private string _font;
    private int _fontSize;
    private string _backgroundImageUri;
    private Image _backgroundImage;
    private int _transparency;
    private bool _scale;
    private bool _isDefault;

    private Color? _titleForegroundColor;
    private Color? _titleBackgroundColor;
    private string _titleFont;
    private int _titleFontSize;
    private TitleMode _titleMode;

    private string _name;

    internal Style()
    {
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="Style" /> class.
    /// </summary>
    /// <param name="styleNode"> The style node. </param>
    /// <param name="storage"> The storage. </param>
    public Style(XmlNode styleNode, IPhysicalStorage storage)
    {
        this._storage = storage;
        this._id = styleNode.GetId();
        this._isDefault = styleNode.GetAttributeBoolValue("isdefault");
        this._name = styleNode.GetAttributeValue("name");

        this._foregroundColor = styleNode.GetNodeColorValue("Foreground", "color");
        this._font = styleNode.GetNodeValue("Foreground", "font");
        this._fontSize = styleNode.GetNodeIntValue("Foreground", "fontsize");

      if (styleNode.HasNode("Background"))
      {
          this._backgroundColor = styleNode.GetNodeColorValue("Background", "color");

        if (styleNode.HasNode("Background", "image"))
        {
            this._backgroundImageUri = styleNode.GetNodeValue("Background", "image", "uri");

          var cache = styleNode.GetNodeValue("Background", "image", "cache");
          if (!string.IsNullOrEmpty(cache))
          {
              this._backgroundImage = ImageSerializer.DeserializeBase64(cache);
          }
          else if (File.Exists(this._backgroundImageUri))
          {
              this._backgroundImage = Image.FromFile(this._backgroundImageUri);
          }

            this._transparency = styleNode.GetNodeIntValue("Background", "image", "transparency");
            this._scale = styleNode.GetNodeBoolValue("Background", "image", "scale");
        }

        if (styleNode.HasNode("Title"))
        {
            this._titleMode = TitleModeExtensions.AsTitleMode(styleNode["Title"].GetAttributeValue("mode"));

          if (styleNode.HasNode("Title", "Foreground"))
          {
              this._titleForegroundColor = styleNode.GetNodeColorValue("Title", "Foreground", "color");
              this._titleFont = styleNode.GetNodeValue("Title", "Foreground", "font");
              this._titleFontSize = styleNode.GetNodeIntValue("Title", "Foreground", "fontsize");
          }
          if (styleNode.HasNode("Title", "Background"))
          {
              this._titleBackgroundColor = styleNode.GetNodeColorValue("Title", "Background", "color");
          }
        }
      }
    }

    public XmlNode Serialize(XmlDocument doc)
    {
      var styleNode = doc.CreateElement("Style");
      var idAttr = doc.CreateAttribute("id");
      idAttr.InnerText = this._id.ToString().Trim('{', '}');
      styleNode.Attributes.Append(idAttr);
      var nameAttr = doc.CreateAttribute("name");
      nameAttr.InnerText = this._name;
      styleNode.Attributes.Append(nameAttr);
      var isDefAttr = doc.CreateAttribute("isdefault");
      isDefAttr.InnerText = this.IsDefault ? "yes" : "no";
      styleNode.Attributes.Append(isDefAttr);
      var foregroundNode = doc.CreateElement("Foreground");
      var foregroundColorNode = doc.CreateElement("color");
      foregroundColorNode.InnerText = Serialization.ColorToHexString(this._foregroundColor);
      foregroundNode.AppendChild(foregroundColorNode);
      var fontNode = doc.CreateElement("font");
      fontNode.InnerText = this._font;
      foregroundNode.AppendChild(fontNode);
      var fontSizeNode = doc.CreateElement("fontsize");
      fontSizeNode.InnerText = this._fontSize.ToString();
      foregroundNode.AppendChild(fontSizeNode);
      styleNode.AppendChild(foregroundNode);
      var backgroundNode = doc.CreateElement("Background");
      var backgroundColorNode = doc.CreateElement("color");
      backgroundColorNode.InnerText = Serialization.ColorToHexString(this._backgroundColor);
      backgroundNode.AppendChild(backgroundColorNode);
      var imageNode = doc.CreateElement("image");
      var uriNode = doc.CreateElement("uri");
      uriNode.InnerText = this._backgroundImageUri;
      imageNode.AppendChild(uriNode);
      var cacheNode = doc.CreateElement("cache");
      cacheNode.InnerText = ""; // ImageSerializer.SerializeBase64(this.backgroundImage);
      imageNode.AppendChild(cacheNode);
      var transNode = doc.CreateElement("transparency");
      transNode.InnerText = this._transparency.ToString();
      imageNode.AppendChild(transNode);
      var scaleNode = doc.CreateElement("scale");
      scaleNode.InnerText = this._scale ? "yes" : "no";
      imageNode.AppendChild(scaleNode);
      backgroundNode.AppendChild(imageNode);
      styleNode.AppendChild(foregroundNode);
      styleNode.AppendChild(backgroundNode);

      // Title
      var titleNode = doc.CreateElement("Title");
      var titleMode = doc.CreateAttribute("mode");
      titleMode.InnerText = this._titleMode.AsString();
      titleNode.Attributes.Append(titleMode);
      var titleForegroundNode = doc.CreateElement("Foreground");
      var titleForegroundColorNode = doc.CreateElement("color");
      titleForegroundColorNode.InnerText = Serialization.ColorToHexString(this._titleForegroundColor);
      titleForegroundNode.AppendChild(titleForegroundColorNode);
      var titleForegroundFontNode = doc.CreateElement("font");
      titleForegroundFontNode.InnerText = this._titleFont;
      titleForegroundNode.AppendChild(titleForegroundFontNode);
      var titleForegroundFontSizeNode = doc.CreateElement("fontsize");
      titleForegroundFontSizeNode.InnerText = this._titleFontSize.ToString();
      titleForegroundNode.AppendChild(titleForegroundFontSizeNode);
      titleNode.AppendChild(titleForegroundNode);
      var titleBackgroundNode = doc.CreateElement("Background");
      var titleBackgroundColorNode = doc.CreateElement("color");
      titleBackgroundColorNode.InnerText = Serialization.ColorToHexString(this._titleBackgroundColor);
      titleBackgroundNode.AppendChild(titleBackgroundColorNode);
      titleNode.AppendChild(titleBackgroundNode);
      styleNode.AppendChild(titleNode);
      return styleNode;
    }

    public bool Save()
    {
      try
      {
          this._storage.SaveStyle(this);
      }
      catch (Exception)
      {
        return false;
      }

        this._isNew = false;
      return true;
    }

    public bool Delete()
    {
      try
      {
          this._storage.DeleteStyle(this);
      }
      catch (Exception)
      {
        return false;
      }

      return true;
    }

    public bool IsNew
    {
      get { return this._isNew; }
    }

    public Guid ID
    {
      get { return this._id; }
    }

    public Color? ForegroundColor
    {
      get { return this._foregroundColor; }
      set { this._foregroundColor = value; }
    }

    public Color? BackgroundColor
    {
      get { return this._backgroundColor; }
      set { this._backgroundColor = value; }
    }

    public string BackgroundImageUri
    {
      get { return this._backgroundImageUri; }
      set
      {
        if (this._backgroundImageUri == value)
        {
          return;
        }

        try
        {
            this._backgroundImage = Image.FromFile(value);
            this._backgroundImageUri = value;
        }
        catch (Exception)
        {
            this._backgroundImage = null;
            this._backgroundImageUri = string.Empty;
        }
      }
    }

    public bool HasBackgroundImage
    {
      get { return this._backgroundImage != null; }
    }

    public Image GetBackgroundImage(Size bounds, bool keepRatio = true)
    {
      if (!this.HasBackgroundImage) return null;

      try
      {
        return Util.handlePic(this.Scale, this._backgroundImage, bounds, keepRatio, this.Transparency);
      }
      catch (Exception)
      {
        return null;
      }
    }

    public int Transparency
    {
      get { return this._transparency; }
      set { this._transparency = value; }
    }

    public bool Scale
    {
      get { return this._scale; }
      set { this._scale = value; }
    }

    public bool IsDefault
    {
      get { return this._isDefault; }
      set { this._isDefault = value; }
    }

    public string Name
    {
      get { return this._name; }
      set { this._name = value; }
    }

    public string Font
    {
      get { return this._font; }
      set { this._font = value; }
    }

    public int FontSize
    {
      get { return this._fontSize; }
      set { this._fontSize = value; }
    }

    public Font GetFont()
    {
      try
      {
        return new Font(this._font, this._fontSize, FontStyle.Regular, GraphicsUnit.Point);
      }
      catch (Exception)
      {
        return null;
      }
    }

    public TitleMode TitleMode
    {
      get { return this._titleMode; }
      set { this._titleMode = value; }
    }

    public Color? TitleBackgroundColor
    {
      get { return this._titleBackgroundColor; }
      set { this._titleBackgroundColor = value; }
    }

    public Color? TitleForegroundColor
    {
      get { return this._titleForegroundColor; }
      set { this._titleForegroundColor = value; }
    }

    public string TitleFont
    {
      get { return this._titleFont; }
      set { this._titleFont = value; }
    }

    public int TitleFontSize
    {
      get { return this._titleFontSize; }
      set { this._titleFontSize = value; }
    }

    public Font GetTitleFont()
    {
      try
      {
        return new Font(this._titleFont, this._titleFontSize, FontStyle.Regular, GraphicsUnit.Point);
      }
      catch (Exception)
      {
        return null;
      }
    }

    public override string ToString()
    {
      return this._name + (this.IsDefault ? " [Standard]" : "");
    }

    #region    Factory

    private Style(IPhysicalStorage storage)
    {
        this._id = Guid.NewGuid();
        this._storage = storage;
    }

    public static Style CreateNewStyle(IPhysicalStorage storage, string name)
    {
      var style = new Style(storage) { Name = name };
      style._isNew = true;
      return style;
    }

    #endregion Factory
  }
}