using BookStoreApp.Model.DTO.PaymentDtos;
using BookStoreApp.Model.DTO;
using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Interface
{
    public interface IPaymentService
    {
        ResponseDto<PaymentDto> AddPayment(int cartId, PaymentCreateDto paymentDto);
        ResponseDto<PaymentDto> GetPaymentById(int id);
        ResponseDto<List<PaymentDto>> GetPaymentsByUserId(int userId);
        ResponseDto<List<PaymentDto>> GetAllPayments();
        void UpdateOrderStatusBasedOnPayment(Order order, Payment payment);
    }
}
