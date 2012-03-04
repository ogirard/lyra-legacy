using System;
using System.Collections.Generic;

namespace Lyra2.LyraShell.Search
{
    public interface ISearchProvider<T> : IEnumerable<T>
        where T : class, IIndexObject
    {
        /// <summary>
        /// Gets the name of this search provider
        /// </summary>
        string SearchProviderName { get; }

        /// <summary>
        /// Number of entries
        /// </summary>
        int NumberOfIndexedItems { get; }

        /// <summary>
        /// Re-initializes the search provider
        /// </summary>
        void ReInitialize();

        /// <summary>
        /// Executes the given query and returns the matching objects
        /// </summary>
        /// <param name = "query">query string</param>
        /// <param name="defaultFieldOnly"></param>
        /// <returns>result list, or empty list if nothing found</returns>
        IList<RatedResult<T>> SearchObjects(string query, bool defaultFieldOnly);

        /// <summary>
        /// Adds a single object to the search
        /// </summary>
        /// <param name = "obj">object to be added to the search</param>
        /// <returns><code>true</code> if object added, <code>false</code> otherwise </returns>
        /// <remarks>
        ///   Use the method <see cref = "AddObjectsToSearch" /> for more efficient bulk
        ///   insertions!
        /// </remarks>
        bool AddObjectToSearch(T obj);

        /// <summary>
        ///   Adds objects to the search index
        /// </summary>
        /// <param name = "objs">list of objects to be added to the index</param>
        /// <returns>if task started</returns>
        bool AddObjectsToSearch(IEnumerable<T> objs);
    }
}