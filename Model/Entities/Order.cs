
namespace BookStoreApp.Model.Entities
{
    public class Order
    {
        public int Id { get; set; }

        //bire - çok ilişki : bir kullanıcı birden çok siparişe sahip olabilir
        public int UserId { get; set; }
        public User User { get; set; } // navigation property

        //bire - çok ilişki: bir sipariş birden çok sipariş kalemine sahip olabilir
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
       
        
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;


        //bire - bir ilişki : bir siparişin bir ödemesi olabilir
        public Payment? Payment { get; set; } // navigation property






        public void AddItem(OrderItem item)
        {
            var existingItem = OrderItems.FirstOrDefault(i => i.BookId == item.BookId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                OrderItems.Add(item);
            }
        }


        public void RemoveItem(int bookId)
        {
            var item = OrderItems.FirstOrDefault(i => i.BookId == bookId);
            if (item != null)
            {
                OrderItems.Remove(item);
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
