namespace BookStoreApp.Model.DTO.OrderDtos
{
    public class OrderItemDto
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
