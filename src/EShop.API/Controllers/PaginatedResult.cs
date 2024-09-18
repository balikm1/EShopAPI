using System.Collections.Generic;
using System;

namespace EShop.API.Controllers
{
    /// <summary>Represents partial result of item search suitable for pagination.</summary>
    /// <typeparam name="T">Type of the items</typeparam>
    public class PaginatedResult<T>
    {
        /// <summary>Gets or sets items of current page.</summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>Gets or sets total count of all items.</summary>
        public int TotalCount { get; set; }

        /// <summary>Gets or sets count of pages to fit all items.</summary>
        public int TotalPages { get; set; }

        /// <summary>Gets or sets number of current page.</summary>
        public int CurrentPage { get; set; }

        /// <summary>Initializes new instance of <see cref="PaginatedResult"/>.</summary>
        /// <param name="items">Items of current page.</param>
        /// <param name="totalCount">Total count of all items.</param>
        /// <param name="pageSize">Size of one page.</param>
        /// <param name="currentPage">Number of current page.</param>
        public PaginatedResult(IEnumerable<T> items, int totalCount, int pageSize, int currentPage)
        {
            Items = items;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            CurrentPage = currentPage;
        }
    }
}
