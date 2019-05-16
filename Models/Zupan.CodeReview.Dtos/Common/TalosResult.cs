using System;
using System.Collections.Generic;
using System.Text;

namespace Zupan.CodeReview.Dtos.Common
{
    public class TalosResult<T>
    {
        /// <summary>
        /// The result of the search
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// At what time the results were generated
        /// </summary>
        public DateTime GeneratedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// If the search was successful
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// The http log message
        /// </summary>
        public string Message { get; set; }
    }
}
