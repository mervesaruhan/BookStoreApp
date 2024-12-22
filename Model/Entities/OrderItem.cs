namespace BookStoreApp.Model.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; } // Sipariş ile ilişki
        public int BookId { get; set; } // Kitap ile ilişki
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
        public Order Order { get; set; }

    }
}
