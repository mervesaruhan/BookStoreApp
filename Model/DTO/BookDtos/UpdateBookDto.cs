using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Model.DTO.BookDtos
{
    public class UpdateBookDto
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Author { get; set; } = null!;
        public string? ISBN { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage ="Fiyat sıfırdan büyük olmalıdır")]
        public decimal Price { get; set; }
        public string Genre { get; set; } = null!;
        public List<string> CategoryNames { get; set; } = new List<string>();

    }
}
