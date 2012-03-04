namespace Lyra2.LyraShell
{
	/// <summary>
	/// Summary description for UpdateFileURL.
	/// </summary>
	public class UpdateFileURL
	{
		private string url = "";
		private string name = "";
		private string version = "";
		private string listurl = "";

		public UpdateFileURL(string url, string name, string version, string listurl)
		{
			this.url = url;
			this.name = name;
			this.version = version;
			this.listurl = listurl;
		}

		public string URL
		{
			get { return this.url; }
		}

		public string Name
		{
			get { return this.name; }
		}

		public string Version
		{
			get { return this.version; }
		}

		public string List
		{
			get { return this.listurl; }
		}

		public override string ToString()
		{
			return this.name + "  [ver: " + this.version + "]";
		}
	}
}