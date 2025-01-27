using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Interface
{
    public interface IOrderRepository
    {
        Task<Order> AddOrderAsync(Order order);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order?> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status);
        Task<Order> UpdateOrderAsync(Order updatedOrder);
    }
}
