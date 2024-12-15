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
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public PaymentService (IPaymentRepository paymentRepository, IMapper mapper, IShoppingCartRepository shoppingCartRepository,IOrderRepository orderRepository)
        {
            _paymentRepository=paymentRepository;
            _mapper=mapper;
            _shoppingCartRepository=shoppingCartRepository;
            _orderRepository=orderRepository;
        }

        public ResponseDto<PaymentDto> AddPayment(int cartId, PaymentCreateDto paymentCreateDto)
        {
            try
            {
                var cart = _shoppingCartRepository.GetCartById(cartId);
                if (cart == null || cart.Items.Any()) return ResponseDto<PaymentDto>.Fail("Sepet bulunamadı");

                var payment = _mapper.Map<Payment>(paymentCreateDto);
                payment.Amount = cart.TotalPrice;
                payment.PaymentStatus = PaymentStatus.Completed;
                payment.PaymentDate = DateTime.UtcNow;

                var createPayment = _paymentRepository.AddPayment(payment);

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
            if (payments == null || !payments.Any()) return ResponseDto<List<PaymentDto>>.Fail("Kulanıcıya iat ödeme kulunamadı");
            
            var result = _mapper.Map<List<PaymentDto>>(payments);
            return ResponseDto<List<PaymentDto>>.Succes(result);
        }

        public ResponseDto<List<PaymentDto>> GetAllPayments()
        {
            var payments = _paymentRepository.GetAllPayments();
            if (payments == null || !payments.Any()) return ResponseDto<List<PaymentDto>>.Fail("Hiç ödeme bulunamadı.");
            
            var result = _mapper.Map<List<PaymentDto>>(payments);
            return ResponseDto<List<PaymentDto>>.Succes(result);
        }

        public void UpdateOrderStatusBasedOnPayment(Order order, Payment payment)
        {
            if (payment.PaymentStatus == PaymentStatus.Completed)
            {
                order.Status = OrderStatus.Shipped;
            }
            else
            {
                order.Status = OrderStatus.Pending;
            }
            _orderRepository.UpdateOrder(order.Id,order);
        }


    }
}
