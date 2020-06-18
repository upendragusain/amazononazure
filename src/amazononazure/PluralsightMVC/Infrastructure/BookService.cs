using PluralsightMVC.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PluralsightMVC.Infrastructure
{
    public class BookService : IBookService
    {
        public Task<Book> GetItem(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Book>> GetItems(int pageSize, int pageIndex, string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
