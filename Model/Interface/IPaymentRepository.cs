using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Interface
{
    public interface IPaymentRepository
    {
        Payment AddPayment(Payment payment);
        Payment? GetPaymentById(int id);
        List<Payment> GetPaymentsByUserId(int userId);
        List<Payment> GetPaymentsByOrderId(int orderId);
        List<Payment> GetAllPayments();
    }
}
