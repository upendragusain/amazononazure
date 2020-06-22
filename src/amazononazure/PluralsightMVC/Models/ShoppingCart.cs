using System.Collections.Generic;
using System.Linq;

namespace PluralsightMVC.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            ShoppingCartItems = new List<ShoppingCartItem>();
        }

        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public decimal Total
        {
            get { return this.ShoppingCartItems.Sum(_ => _.Book.Price); }
            private set { }
        }
    }
}
