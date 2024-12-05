using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.DTO.BookDtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public decimal Price { get; set; }
        public string Genre { get; set; } = null!;
        public List<Category> Categories { get; set; } = new();
    }
}
