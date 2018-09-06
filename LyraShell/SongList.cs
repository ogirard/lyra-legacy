using System.Collections;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
	/// <summary>
	/// Summary description for SongList.
	/// </summary>
	public class SongList : ListView
	{
		public void ClearSongs()
		{
			this.Items.Clear();
		}
		
		public void AddSong(Song song)
		{
			this.BeginUpdate();
			var lvi = new ListViewItem();
			lvi.Tag = song;
			lvi.Text = song.Number.ToString();
			lvi.SubItems.Add(song.Title);
			this.Items.Add(lvi);
			this.EndUpdate();
		}
		
		public void AddSongs(ICollection songs)
		{
			this.BeginUpdate();
			foreach(Song song in songs)
			{
				var lvi = new ListViewItem();
				lvi.Tag = song;
				lvi.Text = song.Number.ToString();
				lvi.SubItems.Add(song.Title);
				this.Items.Add(lvi);
			}
			this.EndUpdate();
		}
		
		public Song SelectedSong
		{
			get
			{
				if(this.SelectedItems.Count == 1)
				{
					return (Song)this.SelectedItems[0].Tag;
				}
				else
				{
					return null;
				}
			}
		}
		
		public ICollection GetSongs()
		{
			var songs = new ArrayList();
			foreach(ListViewItem lvi in this.Items)
			{
				songs.Add(lvi.Tag);
			}
			return songs;
		}
		
		public SongList()
		{
			this.FullRowSelect = true;
			this.GridLines = true;
			this.HeaderStyle = ColumnHeaderStyle.Clickable;
			this.LabelWrap = true;
			this.MultiSelect = false;
			this.AllowColumnReorder = false;
			var nrCol = new ColumnHeader();
			nrCol.Text = "Nummer";
			nrCol.Width = 36;
			this.Columns.Add(nrCol);
			var titleCol = new ColumnHeader();
			titleCol.Text = "Titel";
			titleCol.Width = this.Width - 48;
			this.Columns.Add(titleCol);
		}
	}
}
