using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lyra2.LyraShell.Search;

namespace Lyra2.LyraShell
{
  public class FastSearch : ISearch, IDisposable
  {
    private readonly Indexer<Song> indexer;
    private readonly IDictionary<int, IList<Song>> nrIndex;

    public FastSearch()
    {
      this.indexer = new Indexer<Song>(Song.SongIndexFields, "Fast Search");
      this.nrIndex = new Dictionary<int, IList<Song>>();
    }

    #region Implementation of ISearch

    public void AddValues(IEnumerable<Song> values, bool append = false)
    {
      if (!append)
      {
        this.indexer.ClearIndex();
        this.nrIndex.Clear();
      }
      this.indexer.AddObjectsToSearch(values);
      foreach (var song in values)
      {
        if (!this.nrIndex.ContainsKey(song.Number))
        {
          this.nrIndex.Add(song.Number, new List<Song>());
        }
        if (!this.nrIndex[song.Number].Contains(song))
        {
          this.nrIndex[song.Number].Add(song);
        }
      }
    }

    public bool SearchCollection(string query, SortedList list, SongListBox resultBox, bool text, bool matchCase, bool whole, bool trans, SortMethod sortMethod)
    {
      var lQuery = SearchUtil.CreateLyraQuery(query, whole, !text);
      var numberSongs = new List<ISong>();

      // search for nr
      if (lQuery.Numbers != null)
      {
        foreach (var nr in lQuery.Numbers)
        {
          var nrMatches = this.nrIndex.ContainsKey(nr) ? this.nrIndex[nr] : null;
          if (nrMatches != null)
          {
            numberSongs.AddRange(nrMatches);
          }
        }
      }

      var songs = new List<ISong>();
      resultBox.Ratings.Clear();
      if (!string.IsNullOrEmpty(lQuery.Query))
      {
        foreach (var songResult in this.indexer.SearchObjects(query, !text))
        {
          if (numberSongs.Contains(songResult.Result)) continue;
          resultBox.Ratings.Add(songResult.Result, (float)songResult.Rating);
          songs.Add(songResult.Result);
        }
      }

      lock (resultBox)
      {
        resultBox.BeginUpdate();
        resultBox.Items.Clear();
        resultBox.SetSearchTags(GetTags(query));
        resultBox.ShowSongs(numberSongs, songs, sortMethod);
        resultBox.EndUpdate();
      }
      return true;
    }

    private static IList<string> GetTags(string query)
    {
      return QueryHelper.ExtractWordsOrPhrases(query, false).Where(tag => !IsKeyWord(tag)).Select(tag => QueryHelper.FormatTag(tag, 4)).ToList();
    }

    private static bool IsKeyWord(string tag)
    {
      return tag == "OR" || tag == "AND" || tag == "(" || tag == ")";
    }

    #endregion

    public void Dispose()
    {
      if (this.indexer != null)
      {
        this.indexer.Dispose();
      }
    }
  }
}
