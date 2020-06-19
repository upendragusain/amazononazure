using Microsoft.AspNetCore.Mvc;
using PluralsightMVC.Infrastructure;
using PluralsightMVC.Models;
using PluralsightMVC.ViewModels;
using System;
using System.Threading.Tasks;

namespace PluralsightMVC.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IActionResult> List(
           int? page, string searchTerm = null)
        {
            if (page <= 0)
                page = 1;

            var itemsPage = 10;

            var pageBooks = await _bookService.GetItems(
                itemsPage, page ?? 1, searchTerm);

            var vm = new BookListViewModel()
            {
                Data = pageBooks.Data,
                PaginationInfo = new PaginationInfo()
                {
                    ActualPage = page ?? 1,
                    ItemsPerPage = itemsPage,
                    TotalItems = pageBooks.Count,
                    TotalPages = (int)Math.Ceiling((decimal)pageBooks.Count/itemsPage),
                    SearchTerm = searchTerm
                }
            };

            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) 
                ? "is-disabled" : "";
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) 
                ? "is-disabled" : "";

            //ViewBag.BasketInoperativeMsg = errorMsg;

            return View(vm);
        }

        public async Task<IActionResult> Details(string id)
        {
            var book = await _bookService.GetItem(id);
            if (book == null)
                return NotFound();

            return View(book);
        }
    }
}