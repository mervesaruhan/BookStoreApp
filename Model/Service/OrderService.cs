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
                // 1. Stok Kontrolü
                foreach (var item in orderCreateDto.Items)
                {
                    var book = _bookRepository.GetBookById(item.BookId);
                    if (book == null)
                        return ResponseDto<OrderDto>.Fail($"Girilen ID'de kitap bulunamadı: {item.BookId}");
                    if (book.Stock < item.Quantity)
                        return ResponseDto<OrderDto>.Fail($"Stok yetersiz: {book.Title}");
                }

                // 2. Stok Güncellemesi ve OrderItem Bilgilerinin Hazırlanması
                var orderItems = new List<OrderItem>();
                decimal totalOrderPrice = 0;

                foreach (var item in orderCreateDto.Items)
                {
                    var book = _bookRepository.GetBookById(item.BookId);
                    book!.Stock -= item.Quantity;
                    _bookRepository.UpdateBook(book.Id,book);

                    var orderItem = new OrderItem
                    {
                        BookId = book.Id,
                        Quantity = item.Quantity,
                        UnitPrice = book.Price, // Kitap fiyatını otomatik ayarla
                    };

                    totalOrderPrice += orderItem.TotalPrice; // Toplam sipariş fiyatını güncelle
                    orderItems.Add(orderItem);
                }

                // 3. Sipariş Oluşturma
                var order = new Order
                {
                    UserId = userId,
                    Items = orderItems,
                    TotalPrice = totalOrderPrice,
                    Status = OrderStatus.Pending
                };

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



        public ResponseDto<List<OrderDto>> GetOrdersByStatus(OrderStatus status)
        {
           var orders = _orderRepository.GetOrdersByStatus(status);
            if (orders == null || !orders.Any()) return ResponseDto<List<OrderDto>>.Fail("Girilen durumda siprariş bulunamadı.");

            var result = _mapper.Map<List<OrderDto>>(orders);
            return ResponseDto<List<OrderDto>>.Succes(result);
        }




        public ResponseDto<OrderDto> UpdateOrderStatus(int id, OrderStatus newStatus)
        {
            try
            {
                var order = _orderRepository.GetOrderById(id);
                if (order == null) return ResponseDto<OrderDto>.Fail("Girilen ID'de sipariş bulunamadı");

                if (newStatus == OrderStatus.Cancelled && order.Status != OrderStatus.Cancelled)
                {
                    foreach (var item in order.Items)
                    {
                        var book = _bookRepository.GetBookById(item.Id);
                        if (book != null)
                        {
                            book.Stock += item.Quantity;
                            _bookRepository.UpdateBook(book.Id,book);
                        }
                        else
                        {
                            return ResponseDto<OrderDto>.Fail("Girilen ID'de kitap bulunamadı, stok işlemleri başarısız!");
                        }
                    }
                }
                else if (newStatus != OrderStatus.Cancelled && order.Status == OrderStatus.Cancelled)
                {
                    foreach (var item in order.Items)
                    {
                        var book = _bookRepository.GetBookById(id);
                        if (book != null && book.Stock >= item.Quantity)
                        {
                            book.Stock -= item.Quantity;
                            _bookRepository.UpdateBook(book.Id,book);
                        }
                        else
                        {
                            return ResponseDto<OrderDto>.Fail("Girilen ID'de kitap bulunamadı veya " + $"Stok yetersiz: {item.BookId}" + " stok işlemleri başarısız!");

                        }
                    }
                }
                order.Status = newStatus;
                _orderRepository.UpdateOrder(order);

                var result = _mapper.Map<OrderDto>(order);
                return ResponseDto<OrderDto>.Succes(result);
            }
            catch (Exception ex) 
            {
                return  ResponseDto<OrderDto>.Fail(ex.Message); 
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






        public ResponseDto<OrderDto> UpdateOrderStatusAfterPayment(int orderId, PaymentStatus paymentStatus)
        {
            try
            {
                var order = _orderRepository.GetOrderById(orderId);
                if (order == null)
                {
                    return ResponseDto<OrderDto>.Fail("Sipariş bulunamadı.");
                }

                if (paymentStatus == PaymentStatus.Completed)
                {
                    order.Status = OrderStatus.Shipped;

                }
                else
                {
                    order.Status = OrderStatus.Pending;
                    return ResponseDto<OrderDto>.Fail("Sipariş durumu ödeme tamamlanmadıgı için güncellenemedi.");
                }

                var updateOrder = _orderRepository.UpdateOrder(order);
                var result = _mapper.Map<OrderDto>(updateOrder);

                return ResponseDto<OrderDto>.Succes(result);
            }
            catch(Exception ex)
            {
                return ResponseDto<OrderDto>.Fail(ex.Message);
            }

        }


        //##############################################################

        public ResponseDto<OrderDto> AddItemToOrder(AddItemToOrderDto dto)
        {
            try
            {
                var order = _orderRepository.GetOrderById(dto.OrderId);
                if (order == null)
                    return ResponseDto<OrderDto>.Fail("Sipariş bulunamadı.");

                var book = _bookRepository.GetBookById(dto.BookId);
                if (book == null || book.Stock < dto.Quantity)
                    return ResponseDto<OrderDto>.Fail("Stok yetersiz veya kitap bulunamadı.");

                book.Stock -= dto.Quantity;
                _bookRepository.UpdateBook(book.Id,book);

                order.AddItem(new OrderItem
                {
                    BookId = dto.BookId,
                    Quantity = dto.Quantity,
                    UnitPrice = book.Price
                });

                _orderRepository.UpdateOrder(order);

                var result = _mapper.Map<OrderDto>(order);
                return ResponseDto<OrderDto>.Succes(result);
            }
            catch (Exception ex)
            {
                return ResponseDto<OrderDto>.Fail(ex.Message);
            }
        }




        public ResponseDto<bool> UpdateOrderItem(int orderId, int bookId, int quantity)
        {
            try
            {
                var order = _orderRepository.GetOrderById(orderId);

                if (order == null)
                    return  ResponseDto<bool>.Fail("Sipari bulunamadı");

                if (order.Status != OrderStatus.Pending)
                    return ResponseDto<bool>.Fail("SYalnızca Pending aşamasındak siparişler güncelleme yapabilir");

                var orderItem = order.Items.FirstOrDefault(item => item.BookId == bookId);
                if (orderItem == null)
                    return ResponseDto<bool>.Fail("OrderItem bulunamadı");

                if (quantity == 0)
                {
                    // Remove the item
                    order.Items.Remove(orderItem);

                    // Update book stock
                    var book = _bookRepository.GetBookById(bookId);
                    if (book != null)
                        book.Stock += orderItem.Quantity;
                }
                else
                {
                    // Update quantity
                    var difference = orderItem.Quantity - quantity;
                    orderItem.Quantity = quantity;

                    // Update book stock
                    var book = _bookRepository.GetBookById(bookId);
                    if (book != null)
                        book.Stock += difference;
                }

                _orderRepository.UpdateOrder(order);
                return  ResponseDto<bool>.Succes(true);
            }
            catch (Exception ex)
            {
                return  ResponseDto<bool>.Fail(ex.Message);
            }
        }



        #region Stock alternative way
        //public ResponseDto<bool> UpdateStockAfterOrder(int bookId, int quantity)
        //{
        //    var book = _bookRepository.GetBookById(bookId);
        //    if (book == null)
        //        return ResponseDto<bool>.Fail("Kitap bulunamadı");

        //    if (book.Stock < quantity)
        //        return ResponseDto<bool>.Fail("Stok yetersiz");

        //    book.Stock -= quantity;
        //    _bookRepository.UpdateBook(book);

        //    return ResponseDto<bool>.Succes(true);
        //} 
        #endregion





    }
}
