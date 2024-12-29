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



        public Order UpdateOrder(Order updatedOrder)
        {
            var existingOrder = _orders.FirstOrDefault(o => o.Id == updatedOrder.Id);
            if (existingOrder == null)
            {
                throw new Exception("Girilen ID'de sipariş bulunamadı.");
            }

            // Güncellemeler
            existingOrder.Status = updatedOrder.Status;
            existingOrder.TotalPrice = updatedOrder.TotalPrice;
            existingOrder.Items = updatedOrder.Items;
            existingOrder.OrderDate = updatedOrder.OrderDate;

            return existingOrder;
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





        public List<Order> GetOrdersByStatus(OrderStatus status)
        {
            return _orders.Where(o => o.Status == status).ToList();
        } 
    }
    
    
}
