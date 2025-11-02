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
      indexer = new Indexer<Song>(Song.SongIndexFields, "Fast Search");
      nrIndex = new Dictionary<int, IList<Song>>();
    }

    #region Implementation of ISearch

    public void AddValues(IEnumerable<Song> values, bool append = false)
    {
      if (!append)
      {
        indexer.ClearIndex();
        nrIndex.Clear();
      }
      indexer.AddObjectsToSearch(values);
      foreach (var song in values)
      {
        if (!nrIndex.ContainsKey(song.Number))
        {
          nrIndex.Add(song.Number, new List<Song>());
        }
        if (!nrIndex[song.Number].Contains(song))
        {
          nrIndex[song.Number].Add(song);
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
          var nrMatches = nrIndex.ContainsKey(nr) ? nrIndex[nr] : null;
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
        foreach (var songResult in indexer.SearchObjects(query, !text))
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
      if (indexer != null)
      {
        indexer.Dispose();
      }
    }
  }
}
