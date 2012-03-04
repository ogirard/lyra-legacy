using System;
using System.Drawing;
using System.Xml;

namespace Lyra2.UtilShared
{
  public static class XmlExtensions
  {
    public static bool HasNode(this XmlNode parent, params string[] elements)
    {
      if (parent == null || elements == null || elements.Length == 0)
      {
        return false;
      }

      var current = parent;

      foreach (var element in elements)
      {
        current = current[element];

        if (current == null)
        {
          return false;
        }
      }

      return true;
    }

    public static string GetNodeValue(this XmlNode parent, params string[] elements)
    {
      if (parent == null || elements == null || elements.Length == 0)
      {
        return null;
      }

      var current = parent;

      foreach (var element in elements)
      {
        current = current[element];

        if (current == null)
        {
          return null;
        }
      }

      return current.InnerText;
    }

    public static int GetNodeIntValue(this XmlNode parent, params string[] elements)
    {
      var value = GetNodeValue(parent, elements);

      return CheckInt(value);
    }

    private static int CheckInt(string value)
    {
      int result;
      if (!string.IsNullOrEmpty(value) && int.TryParse(value, out result))
      {
        return result;
      }

      return 0;
    }

    public static Color? GetNodeColorValue(this XmlNode parent, params string[] elements)
    {
      var value = GetNodeValue(parent, elements);
      return CheckColor(value);
    }

    private static Color? CheckColor(string value)
    {
      return !string.IsNullOrEmpty(value) ? Serialization.ColorFromHexString(value) : null;
    }

    public static bool GetNodeBoolValue(this XmlNode parent, params string[] elements)
    {
      var value = GetNodeValue(parent, elements);
      return CheckBool(value);
    }

    private static bool CheckBool(string value)
    {
      return !string.IsNullOrEmpty(value) && (value.ToLowerInvariant() == "yes" || value.ToLowerInvariant() == "true");
    }

    public static string GetAttributeValue(this XmlNode node, string attributeName)
    {
      if (node == null)
      {
        return null;
      }

      if (node.Attributes != null && node.Attributes[attributeName] != null)
      {
        return node.Attributes[attributeName].InnerText;
      }

      return null;
    }

    public static int GetAttributeIntValue(this XmlNode node, string attributeName)
    {
      var value = GetAttributeValue(node, attributeName);
      return CheckInt(value);
    }

    public static Color? GetAttributeColorValue(this XmlNode node, string attributeName)
    {
      var value = GetAttributeValue(node, attributeName);
      return CheckColor(value);
    }

    public static bool GetAttributeBoolValue(this XmlNode node, string attributeName)
    {
      var value = GetAttributeValue(node, attributeName);
      return CheckBool(value);
    }

    public static Guid GetId(this XmlNode node)
    {
      var value = GetAttributeValue(node, "id");
      return !string.IsNullOrEmpty(value) ? Guid.Parse(value) : Guid.Empty;
    }
  }
}