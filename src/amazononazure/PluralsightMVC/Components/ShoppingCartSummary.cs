using Microsoft.AspNetCore.Mvc;
using oinkmvc.Infrastructure;
using oinkmvc.ViewModels;

namespace oinkmvc.Components
{
    public class ShoppingCartSummary : ViewComponent
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartSummary(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        public IViewComponentResult Invoke()
        {
            var cart = _shoppingCartService.GetCart();

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = cart,
            };

            return View(shoppingCartViewModel);
        }
    }
}
