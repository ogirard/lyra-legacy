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
			Items.Clear();
		}
		
		public void AddSong(Song song)
		{
			BeginUpdate();
			var lvi = new ListViewItem();
			lvi.Tag = song;
			lvi.Text = song.Number.ToString();
			lvi.SubItems.Add(song.Title);
			Items.Add(lvi);
			EndUpdate();
		}
		
		public void AddSongs(ICollection songs)
		{
			BeginUpdate();
			foreach(Song song in songs)
			{
				var lvi = new ListViewItem();
				lvi.Tag = song;
				lvi.Text = song.Number.ToString();
				lvi.SubItems.Add(song.Title);
				Items.Add(lvi);
			}
			EndUpdate();
		}
		
		public Song SelectedSong
		{
			get
			{
				if(SelectedItems.Count == 1)
				{
					return (Song)SelectedItems[0].Tag;
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
			foreach(ListViewItem lvi in Items)
			{
				songs.Add(lvi.Tag);
			}
			return songs;
		}
		
		public SongList()
		{
			FullRowSelect = true;
			GridLines = true;
			HeaderStyle = ColumnHeaderStyle.Clickable;
			LabelWrap = true;
			MultiSelect = false;
			AllowColumnReorder = false;
			var nrCol = new ColumnHeader();
			nrCol.Text = "Nummer";
			nrCol.Width = 36;
			Columns.Add(nrCol);
			var titleCol = new ColumnHeader();
			titleCol.Text = "Titel";
			titleCol.Width = Width - 48;
			Columns.Add(titleCol);
		}
	}
}
