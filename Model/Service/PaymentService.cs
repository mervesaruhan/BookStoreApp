using AutoMapper;
using BookStoreApp.Model.DTO;
using BookStoreApp.Model.DTO.PaymentDtos;
using BookStoreApp.Model.Interface;
using BookStoreApp.Model.Entities;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BookStoreApp.Model.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public PaymentService (IPaymentRepository paymentRepository, IMapper mapper, IOrderRepository orderRepository,IOrderService orderService)
        {
            _paymentRepository=paymentRepository;
            _mapper=mapper;
            _orderRepository=orderRepository;
            _orderService=orderService;
        }



        public async Task<ResponseDto<PaymentDto>> AddPaymentAsync(int orderId, PaymentCreateDto paymentCreateDto)
        {
            await using var transaction = await _paymentRepository.BeginTransactionAsync();

            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(orderId);
                if (order == null || !order.OrderItems.Any()) return ResponseDto<PaymentDto>.Fail("Sipariş mevcut değil ya da siparişe bağlı ürün bulunamadı.");

                var payment = _mapper.Map<Payment>(paymentCreateDto);
                payment.Amount = order.TotalPrice;
                payment.PaymentStatus = PaymentStatus.Pending;
                payment.OrderId = orderId;
                payment.UserId = order.UserId;

                var createPayment =await _paymentRepository.AddPaymentAsync(payment);
                createPayment.PaymentStatus = PaymentStatus.Completed;
                await _paymentRepository.UpdatePaymentAsync(createPayment);

                await _orderService.UpdateOrderStatusAfterPaymentAsync(orderId, PaymentStatus.Completed);

                await transaction.CommitAsync();


                var result = _mapper.Map<PaymentDto>(createPayment);
                return ResponseDto<PaymentDto>.Succes(result);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ResponseDto<PaymentDto>.Fail(ex.Message);
            }

        }



        public async Task<ResponseDto<PaymentDto>> GetPaymentByIdAsync(int id)
        {
            try
            {
                var payment = await _paymentRepository.GetPaymentByIdAsync(id);
                if (payment == null) return ResponseDto<PaymentDto>.Fail("Ödeme bulunamadı");

                var result = _mapper.Map<PaymentDto>(payment);
                return ResponseDto<PaymentDto>.Succes(result);
            }
            catch (KeyNotFoundException)
            {
                return ResponseDto<PaymentDto>.Fail("Ödeme bulunamadı.");
            }
            catch (Exception ex)
            {
                return ResponseDto<PaymentDto>.Fail(ex.Message);
            }

        }



        public async Task<ResponseDto<List<PaymentDto>>> GetPaymentsByUserIdAsync(int userId)
        {
            try
            {
                var payments = await _paymentRepository.GetPaymentsByUserIdAsync(userId);
                if (payments.Count == 0)
                    return ResponseDto<List<PaymentDto>>.Fail("Kullanıcıya ait ödeme bulunamadı.");

                var result = _mapper.Map<List<PaymentDto>>(payments);
                return ResponseDto<List<PaymentDto>>.Succes(result);
            }
            catch (Exception ex)
            {
                return ResponseDto<List<PaymentDto>>.Fail(ex.Message);
            }

        }




        public async Task<ResponseDto<PaymentDto>> GetPaymentByOrderIdAsync(int orderId)
        {
            try
            {
                var payment = await _paymentRepository.GetPaymentByOrderIdAsync(orderId);
                if (payment == null) return ResponseDto<PaymentDto>.Fail($"Verilen ID '{orderId}' ile eşleşen bir ödeme bulunamadı.");

                var result = _mapper.Map<PaymentDto>(payment);
                return ResponseDto<PaymentDto>.Succes(result);
            }
            catch (Exception ex)
            {
                return ResponseDto<PaymentDto>.Fail("Beklenmeyen bir hata oluştu: " + ex.Message);
            }
        }




        public async Task<ResponseDto<List<PaymentDto>>> GetAllPaymentsAsync()
        {
            try
            {
                var payments = await _paymentRepository.GetAllPaymentsAsync();
                if (payments.Count == 0 || !payments.Any()) return ResponseDto<List<PaymentDto>>.Fail("Hiç ödeme bulunamadı.");

                var result = _mapper.Map<List<PaymentDto>>(payments);
                return ResponseDto<List<PaymentDto>>.Succes(result);
            }
            catch (Exception ex)
            {
                return ResponseDto<List<PaymentDto>>.Fail("Beklenmeyen hata oluştu" + ex.Message);
            }
        }




    }
}
