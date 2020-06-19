using PluralsightMVC.Models;
using PluralsightMVC.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PluralsightMVC.Infrastructure
{
    public interface IBookService
    {
        Task<BookListViewModel> GetItems(
            int pageSize, int pageIndex, string searchTerm = null);

        Task<Book> GetItem(string id);
    }
}
