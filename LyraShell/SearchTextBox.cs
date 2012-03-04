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
				if(base.Text == "")
				{
					base.Text = value;
					base.ForeColor = Color.DimGray;
				}
			}
		}
		
		public SearchTextBox()
		{
			this.Enter += new EventHandler(SearchTextBox_Enter);
			this.Leave += new EventHandler(SearchTextBox_Leave);
			base.Text = this.defaultText;
			base.ForeColor = Color.DimGray;
		}

		private void SearchTextBox_Enter(object sender, EventArgs e)
		{
			if(base.Text == this.defaultText)
			{
				base.ForeColor = Color.Black;
				base.Text = "";
			}
		}

		private void SearchTextBox_Leave(object sender, EventArgs e)
		{
			if(base.Text == "")
			{
				base.ForeColor = Color.DimGray;
				base.Text = this.defaultText;
			}

		}
	}
}
