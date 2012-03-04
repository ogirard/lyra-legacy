using System;
using System.Collections.Generic;

namespace Lyra2.LyraShell.Search
{
    /// <summary>
    /// IIndexObject providing key and searchable text-content
    /// </summary>
    public interface IIndexObject
    {
        /// <summary>
        /// Searchable text relating to the object's content
        /// </summary>
        IDictionary<string, string> SearchableText { get; }

        /// <summary>
        /// Unique key identifying the object
        /// </summary>
        Guid Key { get; }
    }
}