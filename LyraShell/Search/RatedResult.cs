namespace Lyra2.LyraShell.Search
{
    public class RatedResult<E>
        where E : class, IIndexObject
    {
        private readonly E result;

        public E Result
        {
            get { return this.result; }
        }

        private readonly double rating;

        public double Rating
        {
            get { return this.rating; }
        }
        
        /// <summary>
        /// </summary>
        /// <param name = "result"></param>
        /// <param name = "rating"></param>
        public RatedResult(E result, double rating)
        {
            this.result = result;
            this.rating = rating;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Result == null ? "<n/a>" : ("<" + this.Result.Key + ", " + this.Rating + ">");
        }
    }
}