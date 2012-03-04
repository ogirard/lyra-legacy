using System.Collections;
using System.Xml;

namespace Lyra2.UtilShared
{
	/// <summary>
	/// Summary description for ConfigFile.
	/// </summary>
	public class ConfigFile
	{
		private Hashtable properties = new Hashtable();
		private XmlDocument xdoc = new XmlDocument();

		public ConfigFile(string path)
		{
			
			xdoc.LoadXml(Crypto.DecryptFile(path));
			foreach(XmlNode p in xdoc.GetElementsByTagName("property"))
			{
				this.properties.Add(p.Attributes["name"].InnerText,p.InnerText);
			}
		}

		public ConfigFile() { /* empty */ }

		/// <summary>
		/// gets the property value for property 'name'
		/// </summary>
		public string this [string name]
		{
			get
			{
				if (this.properties.ContainsKey(name))
				{
					return (string)this.properties[name];
				}
				return "n/a";
			}
			set
			{
				if (this.properties.ContainsKey(name))
				{
					this.properties[name] = value;
				}
			}
		}

		/// <summary>
		/// remove a property
		/// </summary>
		/// <param name="name">name of the property to be removed</param>
		/// <returns><code>true</code> if successful, <code>false</code> otherwise</returns>
		public bool removeProperty(string name)
		{
			if (this.properties.ContainsKey(name))
			{
				this.properties.Remove(name);
				return true;
			}
			return false;
		}

		/// <summary>
		/// add a property (if it doesn't exist)
		/// </summary>
		/// <param name="name">name of the property to be created</param>
		/// <param name="val">value of the property</param>
		/// <returns><code>true</code> if successful, <code>false</code> otherwise</returns>
		public bool addProperty(string name, string val)
		{
			if (!this.properties.ContainsKey(name))
			{
				this.properties.Add(name,val);
				return true;
			}
			return false;
		}

		/// <summary>
		/// config-file will be stored to the indicated file
		/// the content will be encrypted!
		/// </summary>
		/// <param name="filename">full path of the file</param>
		public void Save(string filename)
		{
			XmlNode config = xdoc.GetElementsByTagName("config")[0];
			XmlAttribute desc = config.Attributes["desc"];
			config.RemoveAll();
			config.Attributes.Append(desc);
			foreach(string str in this.properties.Keys)
			{
				XmlNode n = xdoc.CreateElement("property",xdoc.NamespaceURI);
				n.InnerText = (string)this.properties[str];
				XmlAttribute a = xdoc.CreateAttribute("name",xdoc.NamespaceURI);
				a.InnerText = str;
				n.Attributes.Append(a);
				config.AppendChild(n);
			}
			Crypto.EncryptFile(this.xdoc.OuterXml,filename);
		}
	}
}
