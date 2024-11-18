namespace BookStoreApp.Model.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string? ISBN { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public DateTime? PublishedDate { get; set; }
        public decimal Price { get; set; } 
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        

    }
}
