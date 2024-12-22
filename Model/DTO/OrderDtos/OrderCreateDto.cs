using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Model.DTO.OrderDtos
{
    public class OrderCreateDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Kullanıcı ID'si 1 veya daha büyük olmalıdır.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Sipariş ürünleri boş olamaz.")]
        [MinLength(1, ErrorMessage = "En az bir ürün eklemelisiniz.")]
        public List<OrderItemCreateDto> Items { get; set; } = new();

    }
}
