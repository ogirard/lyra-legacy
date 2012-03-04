using System.Collections.Generic;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
  /// <summary>
  /// Storage Interface
  /// </summary>
  public interface IStorage
  {
    bool Commit();
    void ResetToLast();
    ISong getSongById(object id);
    ISong GetSong(int nr);
    IList<Style> Styles { get; }
    void Clear();
    void AddSong(ISong song);
    void RemoveSong(string id);

    bool Import(string url, bool append);
    bool Export(string url);
    bool ExportPPC(string url);

    void DisplaySongs(ListBox box);

    void Search(string query, SongListBox resultBox, bool text, bool matchCase, bool whole, bool trans, SortMethod sortMethod);

    bool ToBeCommited { get; set; }

    bool CleanSearchIndex();

    IPhysicalStorage PhysicalStorage { get; }

    bool IsStyleInUse(Style style);
    void SetStyleAsDefault(Style style);
  }
}