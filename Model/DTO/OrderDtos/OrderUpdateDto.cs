using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.DTO.OrderDtos
{
    public class OrderUpdateDto
    {
        public int Id { get; set; }
        public int Stock { get; set; }
        public OrderStatus Status { get; set; }
    }
}
