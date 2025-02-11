using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.DTO.OrderDtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }

 
}
