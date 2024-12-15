namespace BookStoreApp.Model.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string? ISBN { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string Publisher { get; set; } = null!;
        public decimal Price { get; set; } 
        public int Stock { get; set; }
        public string Genre { get; set; } = null!;
     
        public List<Category> Categories { get; set; } = new();
        

    }
}
