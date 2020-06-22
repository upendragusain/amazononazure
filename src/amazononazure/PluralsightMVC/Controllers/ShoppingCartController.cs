using Microsoft.AspNetCore.Mvc;
using oinkmvc.Infrastructure;
using oinkmvc.ViewModels;
using PluralsightMVC.Infrastructure;
using PluralsightMVC.Models;
using System.Threading.Tasks;

namespace oinkmvc.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IBookService _bookService;

        public ShoppingCartController(
            IShoppingCartService shoppingCartService,
            IBookService bookService)
        {
            _shoppingCartService = shoppingCartService;
            _bookService = bookService;
        }

        public ViewResult Index()
        {
            var cart = _shoppingCartService.GetCart();

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = cart,
            };

            return View(shoppingCartViewModel);
        }

        public async Task<RedirectToActionResult> AddToShoppingCart(string bookId)
        {
            var book = await _bookService.GetItem(bookId);
            _shoppingCartService.AddToCart(book);
            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromShoppingCart(string shoppingCartId)
        {
            _shoppingCartService.RemoveFromCart(shoppingCartId);
            return RedirectToAction("Index");
        }
    }
}