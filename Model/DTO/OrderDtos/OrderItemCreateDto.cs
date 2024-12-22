using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Model.DTO.OrderDtos
{
    public class OrderItemCreateDto
    {
        [Required(ErrorMessage = "Kitap ID'si gereklidir.")]
        public int BookId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Miktar en az 1 olmalıdır.")]
        public int Quantity { get; set; }
       
    }
}
