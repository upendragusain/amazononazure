
using System;

namespace PluralsightMVC.Models
{
    public class ShoppingCartItem
    {
        public string ShoppingCartItemId { get; set; }
        public Book Book { get; set; }
        public int Quantity { get; set; }

        public decimal Amount
        {
            get { return Book.Price * Quantity; }
            private set { }
        }
        public string ShoppingCartId { get; set; }
    }
}
