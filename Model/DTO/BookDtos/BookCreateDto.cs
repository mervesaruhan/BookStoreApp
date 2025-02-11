using BookStoreApp.Model.DTO.CategoryDtos;
using BookStoreApp.Model.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Model.DTO.BookDtos
{
    public class BookCreateDto
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Author { get; set; } = null!;
        [Required]
        public string Publisher { get; set; } = null!;
        [Required]
        public string ISBN { get; set; } = null!;

        [Range(0.01,double.MaxValue,ErrorMessage ="Fiyat sıfırdan büyük olmalıdır")]
        public decimal Price { get; set; }

        [Required]
        public string Genre { get; set; } = null!;
        public List<int> CategoryIds { get; set; } = new ();
        public int Stock {  get; set; }
    }
}
