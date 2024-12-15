using AutoMapper;
using BookStoreApp.Model.DTO.OrderDtos;
using BookStoreApp.Model.Interface;
using BookStoreApp.Model.Entities;
using BookStoreApp.Model.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using BookStoreApp.Model.DTO.PaymentDtos;

namespace BookStoreApp.Model.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public OrderService (IOrderRepository orderRepository,IBookRepository bookRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }


        public ResponseDto<OrderDto> AddOrder(int userId, OrderCreateDto orderCreateDto)
        {
            try
            {
                foreach (var item in orderCreateDto.Items)
                {
                    var book = _bookRepository.GetBookById(item.BookId);
                    if (book == null || book.Stock < item.Quantity) return ResponseDto<OrderDto>.Fail($"Stok yetersiz: {item.BookId}");
                }

                foreach (var item in orderCreateDto.Items)
                {
                    var book = _bookRepository.GetBookById(item.BookId);
                    book.Stock -= item.Quantity;
                    _bookRepository.UpdateBook(book);
                }


                var order = _mapper.Map<Order>(orderCreateDto);

                order.UserId = userId;

                order.TotalPrice = orderCreateDto.Items.Sum(i =>
                {
                    var book = _bookRepository.GetBookById(i.BookId);
                    return book.Price * i.Quantity;
                });

                var createdOrder = _orderRepository.AddOrder(order);

                var result = _mapper.Map<OrderDto>(createdOrder);

                return ResponseDto<OrderDto>.Succes(result);
            }
            catch (Exception ex)
            {
                return ResponseDto<OrderDto>.Fail(ex.Message);
            }

        }


        public ResponseDto<OrderDto> GetOrderById(int id)
        {
            try
            {
                var order = _orderRepository.GetOrderById(id);
                if (order == null) return ResponseDto<OrderDto>.Fail("Girilen ID'ye ait sipariş bulunamadı");

                var result = _mapper.Map<OrderDto>(order);
                return ResponseDto<OrderDto>.Succes(result);
            }
            catch (Exception ex)
            {
                return ResponseDto<OrderDto>.Fail($"{ex.Message}");
            }

        }

        public ResponseDto<List<OrderDto>> GetOrdersByUserId (int userId)
        {
            try
            {
                var orders = _orderRepository.GetOrdersByUserId(userId);
                if (orders == null || !orders.Any()) return ResponseDto<List<OrderDto>>.Fail("Girilen Id'ye ait sipariş bulunamadı.");

                var result = _mapper.Map<List<OrderDto>>(orders);
                return ResponseDto<List<OrderDto>>.Succes(result);
            }
            catch (Exception ex)
            {
                return ResponseDto<List<OrderDto>>.Fail(ex.Message);
            }
        }

        public ResponseDto<bool> UpdateOrderStatus(int id, OrderStatus status)
        {
            try
            {
                var update = _orderRepository.UpdateOrderStatus(id, status);
                if (!update) return ResponseDto<bool>.Fail("Girilen Id'de sipariş bulunmamaktadır.Durum güncellenmedi.");

                return ResponseDto<bool>.Succes(true);
            }
            catch (Exception ex)
            {
                return ResponseDto<bool>.Fail(ex.Message);
            }

        }

        public ResponseDto<List<OrderDto>> GetAllOrders()
        {
            try
            {
                var orders = _orderRepository.GetAllOrders();
                if (orders == null || !orders.Any()) return ResponseDto<List<OrderDto>>.Fail("Sipariş bulunamadı.");

                var result = _mapper.Map<List<OrderDto>>(orders);
                return ResponseDto<List<OrderDto>>.Succes(result);
            }
            catch(Exception ex)
            {
                return ResponseDto<List<OrderDto>>.Fail(ex.Message);
            }
        }

        public ResponseDto<OrderDto> UpdateOrder(OrderUpdateDto orderUpdateDto)
        {
            try
            {
                var order = _mapper.Map<Order>(orderUpdateDto);

                var updatedOrder = _orderRepository.UpdateOrder(order);

                var result = _mapper.Map<OrderDto>(updatedOrder);
                return ResponseDto<OrderDto>.Succes(result);
            }
            catch (Exception ex)
            {
                return ResponseDto<OrderDto>.Fail(ex.Message);
            }
        }


        public ResponseDto<OrderDto> UpdatePaymentStatusAfterPayment(int orderId, PaymentStatus paymentStatus)
        {
            var order = _orderRepository.GetOrderById(orderId);
            if (order == null)
            {
                return ResponseDto<OrderDto>.Fail("Sipariş bulunamdı.");
            }

            if (paymentStatus == PaymentStatus.Completed)
            {
                order.Status = OrderStatus.Shipped;
            }
            else
            {
                order.Status = OrderStatus.Pending;
            }

            var updateOrder = _orderRepository.UpdateOrder(order);
            var result = _mapper.Map<OrderDto>(updateOrder);

            return ResponseDto<OrderDto>.Succes(result);

        }

        public ResponseDto<bool> UpdateStockAfterOrder(int bookId, int quantity)
        {
            var book = _bookRepository.GetBookById(bookId);
            if (book == null)
                return ResponseDto<bool>.Fail("Kitap bulunamadı");

            if (book.Stock < quantity)
                return ResponseDto<bool>.Fail("Stok yetersiz");

            book.Stock -= quantity;
            _bookRepository.UpdateBook(book);

            return ResponseDto<bool>.Succes(true);
        }





    }
}
