using BookStoreApp.Model.Entities;
using BookStoreApp.Model.Interface;
using BookStoreApp.Model.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Model.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;
        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        


        public async Task<Payment> AddPaymentAsync(Payment payment)
        {
            
            payment.PaymentDate = DateTime.UtcNow;
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            return payment;
        }



        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            var payment = _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == id);

            if (payment == null)  throw new KeyNotFoundException($"'{id}' ID'sine ait ödeme bulunamadı");
            return payment;
        }



        public async Task<List<Payment>> GetPaymentsByUserIdAsync(int userId)
        {
            var payments = await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .AsNoTracking()
                .Where(p => p.UserId == userId)
                .ToListAsync();
            return payments;
        }



        public async Task<Payment?> GetPaymentByOrderIdAsync(int orderId)
        {
            var payment = await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.OrderId == orderId);
                
            return payment;
        }

        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            var paymentsList = await _context.Payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .AsNoTracking()
                .ToListAsync();

            return paymentsList;
        }





        public async Task UpdatePaymentAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }






        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }



    }
}
