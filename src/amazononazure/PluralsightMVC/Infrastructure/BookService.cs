using Newtonsoft.Json;
using PluralsightMVC.Models;
using PluralsightMVC.ViewModels;
using System;
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

        public Task<Book> GetItem(string id)
        {
            throw new NotImplementedException();
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
