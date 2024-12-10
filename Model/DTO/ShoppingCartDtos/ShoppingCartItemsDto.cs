using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Model.DTO.ShoppingCartDtos
{
    public class ShoppingCartItemsDto
    {
        public int BookId { get; set; }

        public string BookTitle { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
