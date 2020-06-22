using PluralsightMVC.Models;

namespace oinkmvc.Infrastructure
{
    public interface IShoppingCartService
    {
        void AddToCart(Book book);
        void ClearCart();
        ShoppingCart GetCart();
        void RemoveFromCart(string shoppingCartId);
    }
}