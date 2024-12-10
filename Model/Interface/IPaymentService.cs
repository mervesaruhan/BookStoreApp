using BookStoreApp.Model.DTO.PaymentDtos;
using BookStoreApp.Model.DTO;

namespace BookStoreApp.Model.Interface
{
    public interface IPaymentService
    {
        ResponseDto<PaymentDto> AddPayment(PaymentCreateDto paymentDto);
        ResponseDto<PaymentDto> GetPaymentById(int id);
        ResponseDto<List<PaymentDto>> GetPaymentsByUserId(int userId);
        ResponseDto<List<PaymentDto>> GetAllPayments();
    }
}
