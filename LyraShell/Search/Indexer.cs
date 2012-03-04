using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using log4net;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;

namespace Lyra2.LyraShell.Search
{
  /// <summary>
  ///   Provides a Full-Text search index mapping to <see cref = "IIndexObject" />s
  /// </summary>
  public sealed class Indexer<TElement> : ISearchProvider<TElement>, IDisposable
      where TElement : class, IIndexObject
  {
    #region    Logging

    protected static readonly ILog Logger = LogManager.GetLogger(typeof(Indexer<TElement>));

    #endregion Logging

    private const string KeyFieldName = "key";
    private IDictionary<string, int> _searchFields;
    private string _defaultField;
    private readonly Directory _indexDirectory;
    private readonly string _name;

    private readonly Dictionary<Guid, TElement> _items;
    private readonly List<Guid> _indexedItems;

    private BackgroundWorker _indexUpdater;
    private readonly StandardAnalyzer _stdAnalyzer;

    private bool _isIndexReady = true;
    private readonly object _safe = new object();

    /// <summary>
    ///   Creates an indexed dictionary
    /// </summary>
    /// <param name = "searchFields">fields that can be searched</param>
    /// <param name="name">Name of this provider</param>
    public Indexer(IDictionary<string, int> searchFields, string name)
    {
      this._name = name;
      this._items = new Dictionary<Guid, TElement>();
      this._indexedItems = new List<Guid>();
      this._stdAnalyzer = new StandardAnalyzer(Version.LUCENE_29);
      this._indexDirectory = new RAMDirectory();
      this.ClearIndex();

      // init
      this.InitializeIndexUpdateWorker();
      this.InitializeIndexSearch(searchFields);
    }

    #region    Init

    private void InitializeIndexUpdateWorker()
    {
      this._indexUpdater = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };

      // events
      this._indexUpdater.DoWork += this.DoIndexUpdate;
      this._indexUpdater.ProgressChanged += this.ProgressChangedHandler;
      this._indexUpdater.RunWorkerCompleted += this.UpdateComplete;
    }

    private void InitializeIndexSearch(IDictionary<string, int> searchFields)
    {
      // set search fields
      this._searchFields = new Dictionary<string, int>(searchFields);

      // get default (most weighted) field
      int maxWeight = 0;
      foreach (KeyValuePair<string, int> field in this._searchFields)
      {
        if (field.Value > maxWeight)
        {
          this._defaultField = field.Key;
          maxWeight = field.Value;
        }
      }
    }

    #endregion Init

    #region    Settings

    private bool _isAndQuery;

    public bool IsAndQuery
    {
      get { return _isAndQuery; }
      set { _isAndQuery = value; }
    }

    #endregion Settings

    #region    Properties

    /// <summary>
    /// Gets the name of this search provider
    /// </summary>
    public string SearchProviderName
    {
      get { return this._name; }
    }

    /// <summary>
    ///   Number of entries
    /// </summary>
    public int NumberOfIndexedItems
    {
      get { return this._items.Count; }
    }

    /// <summary>
    ///   Key dictionary lookup
    /// </summary>
    /// <param name = "key">key of object to be retrieved</param>
    /// <returns>object or <code>null</code> if key doesn't exist</returns>
    public TElement this[Guid key]
    {
      get
      {
        lock (this._safe)
        {
          return this._items.ContainsKey(key) ? this._items[key] : default(TElement);
        }
      }
    }

    /// <summary>
    ///   <code>true</code> if index ready, <code>false</code> otherwise
    /// </summary>
    public bool IsIndexReady
    {
      get { return this._isIndexReady; }
    }

    private int _progress = 100;

    /// <summary>
    ///   Gets the current indexing progress
    /// </summary>
    public int Progress
    {
      get { return this._progress; }
      protected set
      {
        if (this._progress != value)
        {
          this._progress = value;
          this.OnProgressChanged(new IndexerProgressChangedEventArgs(value));
        }
      }
    }

    #endregion Properties

    #region    Update Index

    /// <summary>
    ///   Gets the index writer
    /// </summary>
    /// <returns></returns>
    private IndexWriter GetIndexWriter(bool create = false)
    {
      return new IndexWriter(this._indexDirectory, _stdAnalyzer, create, IndexWriter.MaxFieldLength.UNLIMITED);
    }

    public void ClearIndex()
    {
      IndexWriter writer = GetIndexWriter(true);
      writer.Close();
      this._indexedItems.Clear();
      this._items.Clear();
    }

    /// <summary>
    ///   Optimizes index
    /// </summary>
    public void Optimize()
    {
      IndexWriter writer = this.GetIndexWriter();
      writer.Optimize();
      writer.Close();
    }

    public void ReInitialize()
    {
      List<TElement> itemsCopy = new List<TElement>(this._items.Values);
      this.ClearIndex();
      this.AddObjectsToSearch(itemsCopy);
    }

    /// <summary>
    ///   Adds a single object to the index
    /// </summary>
    /// <param name = "obj">object to be added to the index</param>
    /// <returns><code>true</code> if object added, <code>false</code> otherwise </returns>
    /// <remarks>
    ///   Use the method <see cref = "AddObjectsToSearch" /> for more efficient bulk
    ///   insertions!
    /// </remarks>
    public bool AddObjectToSearch(TElement obj)
    {
      #region    Precondition

      if (obj == null) return false;

      #endregion Precondition

      return this.AddObjectsToSearch(new List<TElement>(new[] { obj }));
    }

    /// <summary>
    ///   Adds objects to the search index
    /// </summary>
    /// <param name = "objs">list of objects to be added to the index</param>
    /// <returns>if task started</returns>
    public bool AddObjectsToSearch(IEnumerable<TElement> objs)
    {
      #region    Precondition

      lock (this._safe)
      {
        if (!this._isIndexReady || this._indexUpdater.IsBusy || objs == null || objs.Count() == 0) return false;
        this._isIndexReady = false;
      }

      #endregion Precondition

      this._indexUpdater.RunWorkerAsync(objs);

      return true;
    }

    private void DoIndexUpdate(object sender, DoWorkEventArgs e)
    {
      lock (this._safe)
      {
        IList<TElement> objs = (IList<TElement>)e.Argument;
        this.Progress = 0;
        try
        {
          IndexWriter writer = this.GetIndexWriter();
          int count = 0;
          foreach (TElement obj in objs)
          {
            if (!this._items.ContainsKey(obj.Key))
            {
              this._items.Add(obj.Key, obj);
              this.Index(writer, obj);
            }
            this._indexUpdater.ReportProgress((int)(++count * 100.0f / objs.Count));
            // check if cancellation pending
            if (e.Cancel) break;
          }
          writer.Optimize();
          writer.Close();
        }
        catch (Exception ex)
        {
          Logger.Error("Could not create index!", ex);
        }
      }
    }

    private void ProgressChangedHandler(object sender, ProgressChangedEventArgs e)
    {
      this.Progress = e.ProgressPercentage;
    }


    /// <summary>
    ///   Adds given object to index
    /// </summary>
    /// <param name = "writer"></param>
    /// <param name = "obj"></param>
    /// <returns></returns>
    private bool Index(IndexWriter writer, TElement obj)
    {
      #region    Precondition

      // already indexed
      if (this._indexedItems.Contains(obj.Key)) return false;

      #endregion Precondition

      try
      {
        Document doc = new Document();
        doc.Add(new Field(KeyFieldName, obj.Key.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
        foreach (KeyValuePair<string, string> field in obj.SearchableText)
        {
          Field fld = new Field(field.Key, QueryHelper.NormalizeText(field.Value), Field.Store.NO, Field.Index.ANALYZED);
          fld.SetBoost(this._searchFields[field.Key]);
          doc.Add(fld);
        }

        writer.AddDocument(doc);
        return true;
      }
      catch (Exception ex)
      {
        Logger.Error(string.Format("Could not index element '{0}'!", obj), ex);
        return false;
      }
    }

    private void UpdateComplete(object sender, RunWorkerCompletedEventArgs e)
    {
      lock (this._safe)
      {
        this.Progress = 100;
        this._isIndexReady = true;
      }
    }

    public event EventHandler<IndexerProgressChangedEventArgs> ProgressChanged;

    private void OnProgressChanged(IndexerProgressChangedEventArgs args)
    {
      if (this.ProgressChanged != null)
      {
        this.ProgressChanged(this, args);
      }
    }

    #endregion Update Index

    #region    Search Index

    /// <summary>
    ///   Queries the index and returns the list of matching objects
    /// </summary>
    /// <param name = "query">full-text query</param>
    /// <param name="defaultFieldOnly"></param>
    /// <returns>result list, or empty list if nothing found</returns>
    /// <exception cref = "Exception">thrown if index not ready (i.e. !<see cref = "IsIndexReady" />)</exception>
    public IList<RatedResult<TElement>> SearchObjects(string query, bool defaultFieldOnly)
    {
      List<RatedResult<TElement>> results = new List<RatedResult<TElement>>();

      lock (this._safe)
      {
        #region    Precondition

        if (this._searchFields.Count == 0 || !this._isIndexReady)
        {
          throw new Exception("Index not ready to be queried!");
        }
        if (string.IsNullOrEmpty(query))
        {
          return results;
        }

        #endregion Precondition

        try
        {
          IndexSearcher indexSearcher = new IndexSearcher(this._indexDirectory, true);
          List<string> fields = new List<string>();
          if (defaultFieldOnly)
          {
            fields.Add(this._defaultField);
          }
          else
          {
            fields.AddRange(this._searchFields.Keys);
          }

          var queryParser = new MultiFieldQueryParser(Version.LUCENE_29, fields.ToArray(), this._stdAnalyzer);
          TopScoreDocCollector hitCollector = TopScoreDocCollector.create(_items.Count, true);
          var preparedQuery = PrepareLuceneQuery(query);
          var luceneQuery = queryParser.Parse(preparedQuery);
          indexSearcher.Search(luceneQuery, hitCollector);

          foreach (ScoreDoc scoreDoc in hitCollector.TopDocs().ScoreDocs)
          {
            Document doc = indexSearcher.Doc(scoreDoc.doc);
            Guid key = new Guid(doc.GetField(KeyFieldName).StringValue());

            if (!this._items.ContainsKey(key)) continue;

            double rating = scoreDoc.score;
            RatedResult<TElement> result = new RatedResult<TElement>(this._items[key], rating);
            results.Add(result);
          }

          indexSearcher.Close();
        }
        catch (Exception ex)
        {
          Logger.Error("Indexer does not work!", ex);
        }
      }
      // sort always
      return results.OrderByDescending(r => r.Rating).ToList();
    }

    /// <summary>
    ///   Converts the given full-text query to a lucene query
    /// </summary>
    /// <param name = "query"></param>
    /// <returns></returns>
    private string PrepareLuceneQuery(string query)
    {
      string luceneQuery = string.Empty;
      query = query.TrimStart(' ', '*', '?', '~');

      foreach (var word in QueryHelper.ExtractWordsOrPhrases(query))
      {
        var keyWord = word.Trim();

        if (keyWord == "OR" || keyWord == "AND")
        {
          luceneQuery = RemoveLastOperator(luceneQuery) + " " + keyWord + " ";
        }
        else if (keyWord == "(" || keyWord == ")")
        {
          luceneQuery += " " + keyWord + " ";
        }
        else
        {
          luceneQuery += word + BoolOperator;
        }
      }

      var preparedQuery = RemoveLastOperator(luceneQuery.ToString());

      return preparedQuery.Trim();
    }

    private string BoolOperator
    {
      get { return IsAndQuery ? " AND " : " OR "; }
    }

    private string RemoveLastOperator(string preparedQuery)
    {
      if (preparedQuery.EndsWith(BoolOperator))
      {
        return preparedQuery.Substring(0, preparedQuery.Length - BoolOperator.Length);
      }

      return preparedQuery;
    }

    #endregion Search Index

    #region IEnumerable<E> Members

    ///<summary>
    ///  Returns an enumerator that iterates through the collection.
    ///</summary>
    ///<returns>
    ///  A <see cref = "T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
    ///</returns>
    ///<filterpriority>1</filterpriority>
    IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
    {
      return this._items.Values.GetEnumerator();
    }

    #endregion

    #region IEnumerable Members

    ///<summary>
    ///  Returns an enumerator that iterates through a collection.
    ///</summary>
    ///<returns>
    ///  An <see cref = "T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
    ///</returns>
    ///<filterpriority>2</filterpriority>
    public IEnumerator GetEnumerator()
    {
      return this._items.Values.GetEnumerator();
    }

    #endregion

    #region    Nested Type : IndexerProgressChangedEventArgs

    public class IndexerProgressChangedEventArgs : EventArgs
    {
      private readonly int progress;

      public IndexerProgressChangedEventArgs(int progress)
      {
        this.progress = progress;
      }

      public int Progress
      {
        get { return this.progress; }
      }
    }

    #endregion Nested Type : IndexerProgressChangedEventArgs

    #region    IDisposable

    public void Dispose()
    {
      if (_indexDirectory != null)
      {
        _indexDirectory.Dispose();
      }
    }

    #endregion IDisposable
  }
}