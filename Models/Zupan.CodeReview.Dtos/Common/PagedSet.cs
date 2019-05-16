namespace Zupan.CodeReview.Dtos.Common
{
    using System.Collections.Generic;

    /// <summary>
    /// A collection of items of type T with pagination activated
    /// </summary>
    /// <typeparam name="T">The generic type we're querying</typeparam>
    public class PagedSet<T>
    {
        /// <summary>
        /// The results of the query
        /// </summary>
        public IEnumerable<T> Result { get; set; }

        /// <summary>
        /// The total number of items available
        /// </summary>
        public int Total { get; set; }
    }
}
