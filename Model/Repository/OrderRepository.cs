using BookStoreApp.Model.Entities;
using BookStoreApp.Model.Interface;

namespace BookStoreApp.Model.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public OrderRepository() { }

        private readonly List<Order> _orders = new();


        public Order AddOrder(Order order)
        { 
            order.Id = _orders.Count + 1;
            order.OrderDate = DateTime.UtcNow;
            _orders.Add(order);
            return order;
        }

        public List<Order> GetAllOrders()
        {
            return _orders;
        }



        public Order? GetOrderById(int id)
        {
            var orders = _orders.FirstOrDefault(o => o.Id == id);
            if (orders == null) throw new Exception("Girilen id'de sipariş bulunmamaktadır");
            return orders;
        }



        public List<Order> GetOrdersByUserId(int userId)
        {
            var orders = _orders.Where(o => o.UserId == userId);
            if (orders == null || !orders.Any()) throw new Exception("Bu id'deki kullanıcının siparişi yoktur.");
            return orders.ToList();

        }



        public Order? UpdateOrderStatus(int orderId, OrderStatus status)
        {
            var order = GetOrderById(orderId);
            if (order == null) return null;

            order.Status = status;

            return order;
        }



        public Order UpdateOrder (Order order)
        {
            var existingOrder = GetOrderById(order.Id);
            if (existingOrder == null)  throw new Exception($"{nameof(Order)} : siparişi yoktur");

            existingOrder.Status = order.Status;
            existingOrder.TotalPrice = order.TotalPrice;

            return existingOrder;
        }




        public List<Order> GetOrdersByStatus(OrderStatus status)
        {
            return _orders.Where(o => o.Status == status).ToList();
        } 
    }
    
    
}
