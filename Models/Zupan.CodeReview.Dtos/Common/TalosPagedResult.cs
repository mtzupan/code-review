using System;
using System.Collections.Generic;
using System.Text;

namespace Zupan.CodeReview.Dtos.Common
{
    public class TalosPagedResult<T>
    {
        /// <summary>
        /// If the search was successful
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// The log message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The list of results
        /// </summary>
        public IEnumerable<T> Result { get; set; }

        /// <summary>
        /// The total amount of results
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Date at which the data was generated
        /// </summary>
        public DateTime GeneratedAt { get; set; } = DateTime.Now;
    }
}
