using BookStoreApp.Model.Entities;
using BookStoreApp.Model.Interface;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Model.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        public PaymentRepository() { }

        private static readonly List<Payment> _payments = new();


        public async Task<Payment> AddPaymentAsync(Payment payment)
        {
            payment.Id = _payments.Count + 1;
            payment.PaymentDate = DateTime.UtcNow;
            _payments.Add(payment);

            return await Task.FromResult(payment);
        }


        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            var payment = _payments.FirstOrDefault(p => p.Id == id);
            if (payment == null)  throw new KeyNotFoundException("Ödeme bulunamadı");
            return await Task.FromResult(payment);
        }



        public async Task<List<Payment>> GetPaymentsByUserIdAsync(int userId)
        {
            var payments = _payments.Where(p => p.UserId == userId).ToList();
            return await Task.FromResult(payments);
        }



        public async Task<Payment?> GetPaymentByOrderIdAsync(int orderId)
        {
            var payments = _payments.SingleOrDefault(p => p.OrderId == orderId);
            return await Task.FromResult(payments);
        }

        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            var paymentsList = _payments;
            return await Task.FromResult(paymentsList);
        }


    }
}
