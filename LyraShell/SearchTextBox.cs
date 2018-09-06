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
			get { return this.defaultText; }
			set
			{
				this.defaultText = value;
				if(this.Text == "")
				{
					this.Text = value;
					this.ForeColor = Color.DimGray;
				}
			}
		}
		
		public SearchTextBox()
		{
			this.Enter += new EventHandler(this.SearchTextBox_Enter);
			this.Leave += new EventHandler(this.SearchTextBox_Leave);
			this.Text = this.defaultText;
			this.ForeColor = Color.DimGray;
		}

		private void SearchTextBox_Enter(object sender, EventArgs e)
		{
			if(this.Text == this.defaultText)
			{
				this.ForeColor = Color.Black;
				this.Text = "";
			}
		}

		private void SearchTextBox_Leave(object sender, EventArgs e)
		{
			if(this.Text == "")
			{
				this.ForeColor = Color.DimGray;
				this.Text = this.defaultText;
			}

		}
	}
}
