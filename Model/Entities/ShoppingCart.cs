using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;

namespace BookStoreApp.Model.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new();
        public decimal TotalPrice => Items.Sum(item => item.TotalPrice);
    }

    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Quantity * Price;
    }
}
