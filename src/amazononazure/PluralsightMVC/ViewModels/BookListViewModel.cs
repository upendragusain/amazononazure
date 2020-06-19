using PluralsightMVC.Models;
using System.Collections.Generic;

namespace PluralsightMVC.ViewModels
{
    public class BookListViewModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IEnumerable<Book> Data { get; set; }

        public PaginationInfo PaginationInfo { get; set; }
    }
}
