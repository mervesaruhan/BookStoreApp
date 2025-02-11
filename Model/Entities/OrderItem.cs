namespace BookStoreApp.Model.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        // Sipariş ile ilişki: bire - çok ilişki: bir sipariş birden çok sipariş kalemine sahip olabilir
        public int OrderId { get; set; }
        public Order Order { get; set; }//navigation property

        // Kitap ile ilişki: bire - çok ilişki: bir kitap birden cok sipariş kaleminde olabilir
        public int BookId { get; set; }
        public Book? Book { get; set; }//navigation property


        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        



    }
}
