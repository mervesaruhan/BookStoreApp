using System.ComponentModel.DataAnnotations;
namespace BookStoreApp.Model.DTO.OrderDtos

{
    public class AddItemToOrderDto
    {
        [Required(ErrorMessage = "Kitap ID'si gereklidir.")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Kitap ID gereklidir.")]
        public int BookId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Miktar en az 1 olmalıdır.")]
        public int Quantity { get; set; }
    }



}
