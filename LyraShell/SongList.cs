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
			base.Items.Clear();
		}
		
		public void AddSong(Song song)
		{
			this.BeginUpdate();
			ListViewItem lvi = new ListViewItem();
			lvi.Tag = song;
			lvi.Text = song.Number.ToString();
			lvi.SubItems.Add(song.Title);
			base.Items.Add(lvi);
			this.EndUpdate();
		}
		
		public void AddSongs(ICollection songs)
		{
			this.BeginUpdate();
			foreach(Song song in songs)
			{
				ListViewItem lvi = new ListViewItem();
				lvi.Tag = song;
				lvi.Text = song.Number.ToString();
				lvi.SubItems.Add(song.Title);
				base.Items.Add(lvi);
			}
			this.EndUpdate();
		}
		
		public Song SelectedSong
		{
			get
			{
				if(base.SelectedItems.Count == 1)
				{
					return (Song)base.SelectedItems[0].Tag;
				}
				else
				{
					return null;
				}
			}
		}
		
		public ICollection GetSongs()
		{
			ArrayList songs = new ArrayList();
			foreach(ListViewItem lvi in base.Items)
			{
				songs.Add(lvi.Tag);
			}
			return songs;
		}
		
		public SongList()
		{
			base.FullRowSelect = true;
			base.GridLines = true;
			base.HeaderStyle = ColumnHeaderStyle.Clickable;
			base.LabelWrap = true;
			base.MultiSelect = false;
			base.AllowColumnReorder = false;
			ColumnHeader nrCol = new ColumnHeader();
			nrCol.Text = "Nummer";
			nrCol.Width = 36;
			base.Columns.Add(nrCol);
			ColumnHeader titleCol = new ColumnHeader();
			titleCol.Text = "Titel";
			titleCol.Width = base.Width - 48;
			base.Columns.Add(titleCol);
		}
	}
}
