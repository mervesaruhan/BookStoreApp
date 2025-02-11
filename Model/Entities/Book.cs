namespace BookStoreApp.Model.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public DateTime? PublishedDate { get; set; }
        public string Publisher { get; set; } = null!;
        public decimal Price { get; set; } 
        public int Stock { get; set; }
        public string Genre { get; set; } = null!;

        //çoka-çok ilişki: bir kitap birden çok kategoriye ait olabilir
        public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();

        // bire - çok ilişki:bir kitap birden cok sipariş kaleminde olabilir
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();




    }
}
