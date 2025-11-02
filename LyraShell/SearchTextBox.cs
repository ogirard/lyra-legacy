using System;
using System.Drawing;

namespace Lyra2.LyraShell
{
	/// <summary>
	/// Summary description for SearchTextBox.
	/// </summary>
	public class SearchTextBox : System.Windows.Forms.TextBox
	{
		private string defaultText = "";
		
		public string DefaultText
		{
			get { return defaultText; }
			set
			{
				defaultText = value;
				if(Text == "")
				{
					Text = value;
					ForeColor = Color.DimGray;
				}
			}
		}
		
		public SearchTextBox()
		{
			Enter += new EventHandler(SearchTextBox_Enter);
			Leave += new EventHandler(SearchTextBox_Leave);
			Text = defaultText;
			ForeColor = Color.DimGray;
		}

		private void SearchTextBox_Enter(object sender, EventArgs e)
		{
			if(Text == defaultText)
			{
				ForeColor = Color.Black;
				Text = "";
			}
		}

		private void SearchTextBox_Leave(object sender, EventArgs e)
		{
			if(Text == "")
			{
				ForeColor = Color.DimGray;
				Text = defaultText;
			}

		}
	}
}
