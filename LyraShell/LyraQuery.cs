/*
 * Created by: 
 * Created: Montag, 18. Februar 2008
 */

using System.Collections;
using System.Collections.Generic;

namespace Lyra2.LyraShell
{
    public class LyraQuery
    {
        private readonly string query;
        private string luceneQuery;
        private List<int> numbers;
        private bool exact;

        public LyraQuery(string query, string luceneQuery, IList<int> numbers, bool exact)
        {
            this.query = query;
            this.luceneQuery = luceneQuery;
            this.numbers = new List<int>();
            foreach (var i in numbers)
            {
                if (!this.numbers.Contains(i))
                {
                    this.numbers.Add(i);
                }
            }
            
            this.exact = exact;
        }

        public string Query
        {
            get { return this.query; }
        }

        public string LuceneQuery
        {
            get { return this.luceneQuery; }
        }

        public IList<int> Numbers
        {
            get { return this.numbers; }
        }

        public bool Exact
        {
            get { return this.exact; }
        }
    }
}