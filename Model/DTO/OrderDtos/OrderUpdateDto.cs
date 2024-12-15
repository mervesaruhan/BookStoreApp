using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.DTO.OrderDtos
{
    public class OrderUpdateDto
    {
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
    }
}
