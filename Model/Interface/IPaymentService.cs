using BookStoreApp.Model.DTO.PaymentDtos;
using BookStoreApp.Model.DTO;
using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Interface
{
    public interface IPaymentService
    {
        Task<ResponseDto<PaymentDto>> AddPaymentAsync(int orderId, PaymentCreateDto paymentDto);
        Task<ResponseDto<PaymentDto>> GetPaymentByIdAsync(int id);
        Task<ResponseDto<List<PaymentDto>>> GetPaymentsByUserIdAsync(int userId);
        Task<ResponseDto<PaymentDto>> GetPaymentByOrderIdAsync(int orderId);
        Task<ResponseDto<List<PaymentDto>>> GetAllPaymentsAsync();
    }
}
