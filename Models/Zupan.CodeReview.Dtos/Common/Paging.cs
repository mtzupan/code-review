using System;
using System.Collections.Generic;
using System.Text;

namespace Zupan.CodeReview.Dtos.Common
{
    public class Paging
    {
        /// <summary>
        /// The current index of the page. Default is set to 0
        /// </summary>
        public int CurrentIndex { get; set; } = 0;

        /// <summary>
        /// The number of items per page. Default is set to 10
        /// </summary>
        public int HowManyPerPage { get; set; } = 10;

        /// <summary>
        /// The property we want to order the results by. Default is set to Id
        /// </summary>
        public string PropertyToOrderBy { get; set; } = "Id";

        /// <summary>
        /// If the results will come ordered. Default is set to false
        /// </summary>
        public bool? Ordered { get; set; } = false;

        /// <summary>
        /// If the paging order is descending or not. Default is false
        /// </summary>
        public bool? IsDescending { get; set; } = false;
    }
}
