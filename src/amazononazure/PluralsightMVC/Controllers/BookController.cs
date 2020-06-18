using Microsoft.AspNetCore.Mvc;
using PluralsightMVC.Infrastructure;
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

        public async Task<IActionResult> List()
        {
            return View(await _bookService.GetItems(10,1));
        }
    }
}