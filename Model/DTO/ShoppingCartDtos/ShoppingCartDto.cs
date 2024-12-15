using BookStoreApp.Model.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Model.DTO.ShoppingCartDtos
{
    public class ShoppingCartDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<ShoppingCartItemsDto>? Items { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public ShoppingCartStatus Status { get; set; } = ShoppingCartStatus.Pending;
    }
}
