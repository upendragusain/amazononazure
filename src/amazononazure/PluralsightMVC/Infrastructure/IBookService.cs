using PluralsightMVC.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PluralsightMVC.Infrastructure
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetItems(
            int pageSize, int pageIndex, string searchTerm = null);

        Task<Book> GetItem(string id);
    }
}
