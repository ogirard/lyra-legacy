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
      _storage = storage;
      _id = styleNode.GetId();
      _isDefault = styleNode.GetAttributeBoolValue("isdefault");
      _name = styleNode.GetAttributeValue("name");

      _foregroundColor = styleNode.GetNodeColorValue("Foreground", "color");
      _font = styleNode.GetNodeValue("Foreground", "font");
      _fontSize = styleNode.GetNodeIntValue("Foreground", "fontsize");

      if (styleNode.HasNode("Background"))
      {
        _backgroundColor = styleNode.GetNodeColorValue("Background", "color");

        if (styleNode.HasNode("Background", "image"))
        {
          _backgroundImageUri = styleNode.GetNodeValue("Background", "image", "uri");

          var cache = styleNode.GetNodeValue("Background", "image", "cache");
          if (!string.IsNullOrEmpty(cache))
          {
            _backgroundImage = ImageSerializer.DeserializeBase64(cache);
          }
          else if (File.Exists(_backgroundImageUri))
          {
            _backgroundImage = Image.FromFile(_backgroundImageUri);
          }

          _transparency = styleNode.GetNodeIntValue("Background", "image", "transparency");
          _scale = styleNode.GetNodeBoolValue("Background", "image", "scale");
        }

        if (styleNode.HasNode("Title"))
        {
          _titleMode = TitleModeExtensions.AsTitleMode(styleNode["Title"].GetAttributeValue("mode"));

          if (styleNode.HasNode("Title", "Foreground"))
          {
            _titleForegroundColor = styleNode.GetNodeColorValue("Title", "Foreground", "color");
            _titleFont = styleNode.GetNodeValue("Title", "Foreground", "font");
            _titleFontSize = styleNode.GetNodeIntValue("Title", "Foreground", "fontsize");
          }
          if (styleNode.HasNode("Title", "Background"))
          {
            _titleBackgroundColor = styleNode.GetNodeColorValue("Title", "Background", "color");
          }
        }
      }
    }

    public XmlNode Serialize(XmlDocument doc)
    {
      XmlElement styleNode = doc.CreateElement("Style");
      XmlAttribute idAttr = doc.CreateAttribute("id");
      idAttr.InnerText = _id.ToString().Trim('{', '}');
      styleNode.Attributes.Append(idAttr);
      XmlAttribute nameAttr = doc.CreateAttribute("name");
      nameAttr.InnerText = _name;
      styleNode.Attributes.Append(nameAttr);
      XmlAttribute isDefAttr = doc.CreateAttribute("isdefault");
      isDefAttr.InnerText = IsDefault ? "yes" : "no";
      styleNode.Attributes.Append(isDefAttr);
      XmlElement foregroundNode = doc.CreateElement("Foreground");
      XmlElement foregroundColorNode = doc.CreateElement("color");
      foregroundColorNode.InnerText = Serialization.ColorToHexString(_foregroundColor);
      foregroundNode.AppendChild(foregroundColorNode);
      XmlElement fontNode = doc.CreateElement("font");
      fontNode.InnerText = _font;
      foregroundNode.AppendChild(fontNode);
      XmlElement fontSizeNode = doc.CreateElement("fontsize");
      fontSizeNode.InnerText = _fontSize.ToString();
      foregroundNode.AppendChild(fontSizeNode);
      styleNode.AppendChild(foregroundNode);
      XmlElement backgroundNode = doc.CreateElement("Background");
      XmlElement backgroundColorNode = doc.CreateElement("color");
      backgroundColorNode.InnerText = Serialization.ColorToHexString(_backgroundColor);
      backgroundNode.AppendChild(backgroundColorNode);
      XmlElement imageNode = doc.CreateElement("image");
      XmlElement uriNode = doc.CreateElement("uri");
      uriNode.InnerText = _backgroundImageUri;
      imageNode.AppendChild(uriNode);
      XmlElement cacheNode = doc.CreateElement("cache");
      cacheNode.InnerText = ""; // ImageSerializer.SerializeBase64(this.backgroundImage);
      imageNode.AppendChild(cacheNode);
      XmlElement transNode = doc.CreateElement("transparency");
      transNode.InnerText = _transparency.ToString();
      imageNode.AppendChild(transNode);
      XmlElement scaleNode = doc.CreateElement("scale");
      scaleNode.InnerText = _scale ? "yes" : "no";
      imageNode.AppendChild(scaleNode);
      backgroundNode.AppendChild(imageNode);
      styleNode.AppendChild(foregroundNode);
      styleNode.AppendChild(backgroundNode);

      // Title
      XmlElement titleNode = doc.CreateElement("Title");
      var titleMode = doc.CreateAttribute("mode");
      titleMode.InnerText = _titleMode.AsString();
      titleNode.Attributes.Append(titleMode);
      XmlElement titleForegroundNode = doc.CreateElement("Foreground");
      XmlElement titleForegroundColorNode = doc.CreateElement("color");
      titleForegroundColorNode.InnerText = Serialization.ColorToHexString(_titleForegroundColor);
      titleForegroundNode.AppendChild(titleForegroundColorNode);
      XmlElement titleForegroundFontNode = doc.CreateElement("font");
      titleForegroundFontNode.InnerText = _titleFont;
      titleForegroundNode.AppendChild(titleForegroundFontNode);
      XmlElement titleForegroundFontSizeNode = doc.CreateElement("fontsize");
      titleForegroundFontSizeNode.InnerText = _titleFontSize.ToString();
      titleForegroundNode.AppendChild(titleForegroundFontSizeNode);
      titleNode.AppendChild(titleForegroundNode);
      XmlElement titleBackgroundNode = doc.CreateElement("Background");
      XmlElement titleBackgroundColorNode = doc.CreateElement("color");
      titleBackgroundColorNode.InnerText = Serialization.ColorToHexString(_titleBackgroundColor);
      titleBackgroundNode.AppendChild(titleBackgroundColorNode);
      titleNode.AppendChild(titleBackgroundNode);
      styleNode.AppendChild(titleNode);
      return styleNode;
    }

    public bool Save()
    {
      try
      {
        _storage.SaveStyle(this);
      }
      catch (Exception)
      {
        return false;
      }

      _isNew = false;
      return true;
    }

    public bool Delete()
    {
      try
      {
        _storage.DeleteStyle(this);
      }
      catch (Exception)
      {
        return false;
      }

      return true;
    }

    public bool IsNew
    {
      get { return _isNew; }
    }

    public Guid ID
    {
      get { return _id; }
    }

    public Color? ForegroundColor
    {
      get { return _foregroundColor; }
      set { _foregroundColor = value; }
    }

    public Color? BackgroundColor
    {
      get { return _backgroundColor; }
      set { _backgroundColor = value; }
    }

    public string BackgroundImageUri
    {
      get { return _backgroundImageUri; }
      set
      {
        if (_backgroundImageUri == value)
        {
          return;
        }

        try
        {
          _backgroundImage = Image.FromFile(value);
          _backgroundImageUri = value;
        }
        catch (Exception)
        {
          _backgroundImage = null;
          _backgroundImageUri = string.Empty;
        }
      }
    }

    public bool HasBackgroundImage
    {
      get { return _backgroundImage != null; }
    }

    public Image GetBackgroundImage(Size bounds, bool keepRatio = true)
    {
      if (!HasBackgroundImage) return null;

      try
      {
        return Util.handlePic(Scale, _backgroundImage, bounds, keepRatio, Transparency);
      }
      catch (Exception)
      {
        return null;
      }
    }

    public int Transparency
    {
      get { return _transparency; }
      set { _transparency = value; }
    }

    public bool Scale
    {
      get { return _scale; }
      set { _scale = value; }
    }

    public bool IsDefault
    {
      get { return _isDefault; }
      set { _isDefault = value; }
    }

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public string Font
    {
      get { return _font; }
      set { _font = value; }
    }

    public int FontSize
    {
      get { return _fontSize; }
      set { _fontSize = value; }
    }

    public Font GetFont()
    {
      try
      {
        return new Font(_font, _fontSize, FontStyle.Regular, GraphicsUnit.Point);
      }
      catch (Exception)
      {
        return null;
      }
    }

    public TitleMode TitleMode
    {
      get { return _titleMode; }
      set { _titleMode = value; }
    }

    public Color? TitleBackgroundColor
    {
      get { return _titleBackgroundColor; }
      set { _titleBackgroundColor = value; }
    }

    public Color? TitleForegroundColor
    {
      get { return _titleForegroundColor; }
      set { _titleForegroundColor = value; }
    }

    public string TitleFont
    {
      get { return _titleFont; }
      set { _titleFont = value; }
    }

    public int TitleFontSize
    {
      get { return _titleFontSize; }
      set { _titleFontSize = value; }
    }

    public Font GetTitleFont()
    {
      try
      {
        return new Font(_titleFont, _titleFontSize, FontStyle.Regular, GraphicsUnit.Point);
      }
      catch (Exception)
      {
        return null;
      }
    }

    public override string ToString()
    {
      return _name + (IsDefault ? " [Standard]" : "");
    }

    #region    Factory

    private Style(IPhysicalStorage storage)
    {
      _id = Guid.NewGuid();
      _storage = storage;
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