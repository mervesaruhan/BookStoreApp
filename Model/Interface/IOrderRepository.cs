using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Interface
{
    public interface IOrderRepository
    {
        Order AddOrder(Order order);
        List<Order> GetAllOrders();
        Order? GetOrderById(int id);
        List<Order> GetOrdersByUserId(int userId);
        bool UpdateOrderStatus(int orderId, OrderStatus status);
        List<Order> GetOrdersByStatus(OrderStatus status);
    }
}
