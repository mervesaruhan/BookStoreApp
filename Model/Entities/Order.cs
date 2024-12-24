
namespace BookStoreApp.Model.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }


        public void AddItem(OrderItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.BookId == item.BookId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }


        public void RemoveItem(int bookId)
        {
            var item = Items.FirstOrDefault(i => i.BookId == bookId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }
    }

    public enum OrderStatus
    {
        Pending,
        Shipped,
        Completed,
        Cancelled
    }

   

}
