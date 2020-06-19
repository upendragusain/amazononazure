using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using PluralsightMVC.Models;
using PluralsightMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PluralsightMVC.Infrastructure
{
    public class MockBookService : IBookService
    {
        private readonly IEnumerable<Book> _books;

        public MockBookService(IWebHostEnvironment webHostEnvironment)
        {
            var path = Path.Combine(webHostEnvironment.ContentRootPath, 
                "Infrastructure", "staticbooksdata.json");
            _books = JsonConvert.DeserializeObject<IEnumerable<Book>>(File.ReadAllText(path));
        }

        public async Task<Book> GetItem(string id)
        {
            var book = _books.FirstOrDefault(_ => _.Id == id);
            return await Task.FromResult(book);
        }

        public async Task<BookListViewModel> GetItems(
            int pageSize, int pageIndex, string searchTerm)
        {
            IEnumerable<Book> result = _books; ;
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                result = result.Where(_ => _.Name.Contains(searchTerm));
            }

            result = result
                .Skip(pageIndex)
                .Take(pageSize);

            var bookvm = new BookListViewModel()
            {
                Count = result.Count(),
                Data = result,
                PageIndex = pageIndex,
                PageSize = pageSize,
                PaginationInfo = new PaginationInfo()
                {
                    ActualPage = pageIndex,
                    ItemsPerPage = pageSize,
                    SearchTerm = searchTerm,
                    TotalItems = result.Count(),
                    TotalPages = (int)Math.Ceiling((decimal)result.Count() / pageSize)
                }
            };

            return await Task.FromResult(bookvm);
        }
    }
}
