namespace BookStoreApp.Model.DTO.OrderDtos
{
    public class AddItemToOrderDto
    {
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }



}
