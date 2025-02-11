using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Model.DTO.CategoryDtos
{
    public class CategoryCreateDto
    {
        [MaxLength(100)]
        [Required (ErrorMessage="Kategori ismi boş geçilemez!")]
        public string Name { get; set; } = null!;
        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
