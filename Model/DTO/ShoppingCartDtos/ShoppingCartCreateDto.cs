using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Model.DTO.ShoppingCartDtos
{
    public class ShoppingCartCreateDto
    {
        [Required(ErrorMessage = "Kullanıcı ID gereklidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kullanıcı ID'si girin.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Sepet öğeleri gereklidir.")]
        [MinLength(1, ErrorMessage = "Sepet en az bir ürün içermelidir.")]
        public List<ShoppingCartItemCreateDto> Items { get; set; } = new();
    }

    public class ShoppingCartItemCreateDto
    {
        [Required(ErrorMessage = "Kitap ID'si gereklidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Kitap ID'si geçerli bir değer olmalıdır.")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Adet bilgisi gereklidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Adet en az 1 olmalıdır.")]
        public int Quantity { get; set; }
    }
}
