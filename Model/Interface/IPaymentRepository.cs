using BookStoreApp.Model.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookStoreApp.Model.Interface
{
    public interface IPaymentRepository
    {
        Task<Payment> AddPaymentAsync(Payment payment);
        Task<Payment?> GetPaymentByIdAsync(int id);
        Task<List<Payment>> GetPaymentsByUserIdAsync(int userId);
        Task<Payment?> GetPaymentByOrderIdAsync(int orderId);
        Task<List<Payment>> GetAllPaymentsAsync();
        Task UpdatePaymentAsync(Payment payment);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
