using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PluralsightMVC.Models;
using System;
using System.Linq;

namespace oinkmvc.Infrastructure
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IServiceProvider _services;
        private readonly ISession _session;
        private const string SESSION_KEY = "CARTID";

        public ShoppingCartService(IServiceProvider services)
        {
            _services = services;
            _session = _services.GetRequiredService<IHttpContextAccessor>()?
               .HttpContext.Session;
        }
        public ShoppingCart GetCart()
        {
            var shoppingCart = _session.Get<ShoppingCart>(SESSION_KEY);
            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart()
                {
                    ShoppingCartId = Guid.NewGuid().ToString()
                };
                SaveCart(shoppingCart);
            }

            return shoppingCart;
        }

        private void SaveCart(ShoppingCart cart)
        {
            _session.Set<ShoppingCart>(SESSION_KEY, cart);
        }

        public void AddToCart(Book book)
        {
            var shoppingCart = GetCart();

            var item = shoppingCart.ShoppingCartItems
                .FirstOrDefault(_ => _.Book.Id == book.Id);

            if (item == null)
            {
                shoppingCart.ShoppingCartItems.Add(new ShoppingCartItem()
                {
                    Book = book,
                    ShoppingCartId = shoppingCart.ShoppingCartId,
                    ShoppingCartItemId = Guid.NewGuid().ToString(),
                    Quantity = 1
                });
            }
            else
            {
                shoppingCart.ShoppingCartItems.Remove(item);
                item.Quantity++;
                shoppingCart.ShoppingCartItems.Add(item);
            }

            SaveCart(shoppingCart);
        }

        public void RemoveFromCart(string shoppingCartId)
        {
            var shoppingCart = GetCart();
            var item = shoppingCart.ShoppingCartItems
                .First(_ => _.ShoppingCartId == shoppingCartId);

            shoppingCart.ShoppingCartItems.Remove(item);
            SaveCart(shoppingCart);
        }

        public void ClearCart()
        {
            SaveCart(new ShoppingCart()
            {
                ShoppingCartId = Guid.NewGuid().ToString()
            });
        }
    }
}
