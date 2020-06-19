using Newtonsoft.Json;
using PluralsightMVC.Models;
using PluralsightMVC.ViewModels;
using System.Net.Http;
using System.Threading.Tasks;

namespace PluralsightMVC.Infrastructure
{
    public class BookService : IBookService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BookService(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Book> GetItem(string id)
        {
            var endpoint = $"api/catalog/items/{id}";

            var catalogClient = _httpClientFactory.CreateClient("CatalogAPI");

            var response = await catalogClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var book = JsonConvert.DeserializeObject<Book>(
                    await response.Content.ReadAsStringAsync());

                return book;
            }

            return null;
        }

        public async Task<BookListViewModel> GetItems(
            int pageSize, int pageIndex, string searchTerm = null)
        {
            var endpoint = $"api/catalog/items?pageSize={pageSize}&pageIndex={pageIndex}&searchterm={searchTerm}";

            var catalogClient = _httpClientFactory.CreateClient("CatalogAPI");

            var responseString = await catalogClient.GetStringAsync(endpoint);

            var pageBooks = JsonConvert.DeserializeObject<BookListViewModel>(responseString);

            return pageBooks;
        }
    }
}
