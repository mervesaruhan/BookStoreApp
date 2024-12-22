using AutoMapper;
using BookStoreApp.Model.DTO;
using BookStoreApp.Model.DTO.PaymentDtos;
using BookStoreApp.Model.Interface;
using BookStoreApp.Model.Entities;

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



        public ResponseDto<PaymentDto> AddPayment(int orderId, PaymentCreateDto paymentCreateDto)
        {
            try
            {
                var order = _orderRepository.GetOrderById(orderId);
                if (order == null || !order.Items.Any()) return ResponseDto<PaymentDto>.Fail("Sipariş bulunamadı veya siparişe ürün eklenmemiş");

                var payment = _mapper.Map<Payment>(paymentCreateDto);
                payment.Amount = order.TotalPrice;
                payment.PaymentStatus = PaymentStatus.Completed;
                payment.OrderId = orderId;
                payment.UserId = order.UserId;

                var createPayment = _paymentRepository.AddPayment(payment);
                _orderService.UpdateOrderStatusAfterPayment(orderId, PaymentStatus.Completed);
                

                var result = _mapper.Map<PaymentDto>(createPayment);
                return ResponseDto<PaymentDto>.Succes(result);
            }
            catch (Exception ex)
            {
                return ResponseDto<PaymentDto>.Fail(ex.Message);
            }

        }



        public ResponseDto<PaymentDto> GetPaymentById(int id)
        {
            var payment = _paymentRepository.GetPaymentById(id);
            if (payment == null) return ResponseDto<PaymentDto>.Fail("Ödeme bulunamadı");

            var result = _mapper.Map<PaymentDto>(payment);
            return ResponseDto<PaymentDto>.Succes(result);

        }



        public ResponseDto<List<PaymentDto>> GetPaymentsByUserId(int userId)
        {
            var payments = _paymentRepository.GetPaymentsByUserId(userId);
            if (payments == null || !payments.Any()) return ResponseDto<List<PaymentDto>>.Fail("Kulanıcıya ait ödeme kulunamadı");
            
            var result = _mapper.Map<List<PaymentDto>>(payments);
            return ResponseDto<List<PaymentDto>>.Succes(result);
        }




        public ResponseDto<PaymentDto> GetPaymentByOrderId(int orderId)
        {
            var payment = _paymentRepository.GetPaymentByOrderId(orderId);
            if (payment == null) return ResponseDto<PaymentDto>.Fail("Hiç ödeme bulunamadı.");

            var result = _mapper.Map<PaymentDto>(payment);
            return ResponseDto<PaymentDto>.Succes(result);
        }




        public ResponseDto<List<PaymentDto>> GetAllPayments()
        {
            var payments = _paymentRepository.GetAllPayments();
            if (payments == null || !payments.Any()) return ResponseDto<List<PaymentDto>>.Fail("Hiç ödeme bulunamadı.");
            
            var result = _mapper.Map<List<PaymentDto>>(payments);
            return ResponseDto<List<PaymentDto>>.Succes(result);
        }




    }
}
