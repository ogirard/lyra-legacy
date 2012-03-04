using System;
using System.Collections;
using System.Collections.Generic;

namespace Lyra2.LyraShell
{
    /// <summary>
    /// Search Interface
    /// </summary>
    public interface ISearch : IDisposable
    {
        void AddValues(IEnumerable<Song> values, bool append = false);

        bool SearchCollection(string query,
                              SortedList list,
                              SongListBox resultBox,
                              bool text,
                              bool matchCase,
                              bool whole,
                              bool trans,
                              SortMethod sortMethod);
    }

    public enum SortMethod
    {
        NumberAscending = 0,
        NumberDescending = 1,
        RatingDescending = 2,
        RatingAscending = 3,
        TitleAscending = 4,
        TitleDescending = 5
    }
}