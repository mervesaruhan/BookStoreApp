using BookStoreApp.Model.Entities;
using Microsoft.EntityFrameworkCore.Storage;

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
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<Order?> UpdaterOrderStatusAfterPayment(int orderId, PaymentStatus paymentStatus);
        Task<Order?> AddItemToOrderAsync(int orderId, int bookId, int quantity);
        Task<Order?> RemoveItemFromOrderAsync(int orderId, int bookId);
        Task<Order?> UpdateOrderItemAsync(int orderId, int bookId, int newQuantity);
    }
}
