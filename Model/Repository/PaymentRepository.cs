using BookStoreApp.Model.Entities;
using BookStoreApp.Model.Interface;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Model.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        public PaymentRepository() { }

        private static readonly List<Payment> _payments = new();

        public Payment AddPayment(Payment payment)
        {
            payment.Id = _payments.Count + 1;
            payment.PaymentDate = DateTime.UtcNow;
            _payments.Add(payment);
            return payment;
        }


        public Payment? GetPaymentById(int id)
        {
            return _payments.FirstOrDefault(p => p.Id == id);
        }



        public List<Payment> GetPaymentsByUserId(int userId)
        {
            return _payments.Where(p =>p.UserId == userId).ToList();
        }



        public Payment? GetPaymentByOrderId(int orderId)
        {
            return _payments.SingleOrDefault(p => p.OrderId == orderId);
        }

        public List<Payment> GetAllPayments()
        {
            return _payments;
        }


    }
}
