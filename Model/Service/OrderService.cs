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


        public async Task<ResponseDto<OrderDto>> AddOrderAsync(int userId, OrderCreateDto orderCreateDto)
        {
            try
            {
                // 1. Stok Kontrolü
                foreach (var item in orderCreateDto.Items)
                {
                    var book =await _bookRepository.GetBookByIdAsync(item.BookId);
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
                    var book = await _bookRepository.GetBookByIdAsync(item.BookId);
                    book!.Stock -= item.Quantity;
                    await _bookRepository.UpdateBookAsync(book.Id,book);

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

                var createdOrder =await  _orderRepository.AddOrderAsync(order);
                var result = _mapper.Map<OrderDto>(createdOrder);

                return ResponseDto<OrderDto>.Succes(result);
            }
            catch (Exception ex)
            {
                return ResponseDto<OrderDto>.Fail(ex.Message);
            }

        }



        public async Task<ResponseDto<OrderDto>> GetOrderByIdAsync(int id)
        {
            try
            {
                var order =await _orderRepository.GetOrderByIdAsync(id);
                if (order == null) return ResponseDto<OrderDto>.Fail("Girilen ID'ye ait sipariş bulunamadı");

                var result = _mapper.Map<OrderDto>(order);
                return ResponseDto<OrderDto>.Succes(result);
            }
            catch (Exception ex)
            {
                return ResponseDto<OrderDto>.Fail($"{ex.Message}");
            }

        }




        public async Task<ResponseDto<List<OrderDto>>> GetOrdersByUserIdAsync(int userId)
        {
            try
            {
                var orders =await _orderRepository.GetOrdersByUserIdAsync(userId);
                if (orders == null || !orders.Any()) return ResponseDto<List<OrderDto>>.Fail("Girilen Id'ye ait sipariş bulunamadı.");

                var result = _mapper.Map<List<OrderDto>>(orders);
                return ResponseDto<List<OrderDto>>.Succes(result);
            }
            catch (Exception ex)
            {
                return ResponseDto<List<OrderDto>>.Fail(ex.Message);
            }
        }




        public async Task<ResponseDto<List<OrderDto>>> GetOrdersByStatusAsync(OrderStatus status)
        {
           var orders = await _orderRepository.GetOrdersByStatusAsync(status);
            if (orders == null || !orders.Any()) return ResponseDto<List<OrderDto>>.Fail("Girilen durumda siprariş bulunamadı.");

            var result = _mapper.Map<List<OrderDto>>(orders);
            return ResponseDto<List<OrderDto>>.Succes(result);
        }




        public async Task<ResponseDto<OrderDto>> UpdateOrderStatusAsync(int id, OrderStatus newStatus)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(id);
                if (order == null) return ResponseDto<OrderDto>.Fail("Girilen ID'de sipariş bulunamadı");

                if (newStatus == OrderStatus.Cancelled && order.Status != OrderStatus.Cancelled)
                {
                    foreach (var item in order.Items)
                    {
                        var book =await _bookRepository.GetBookByIdAsync(item.Id);
                        if (book != null)
                        {
                            book.Stock += item.Quantity;
                            await _bookRepository.UpdateBookAsync(book.Id,book);
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
                        var book =await _bookRepository.GetBookByIdAsync(id);
                        if (book != null && book.Stock >= item.Quantity)
                        {
                            book.Stock -= item.Quantity;
                            await _bookRepository.UpdateBookAsync(book.Id,book);
                        }
                        else
                        {
                            return ResponseDto<OrderDto>.Fail("Girilen ID'de kitap bulunamadı veya " + $"Stok yetersiz: {item.BookId}" + " stok işlemleri başarısız!");

                        }
                    }
                }
                order.Status = newStatus;
                await _orderRepository.UpdateOrderAsync(order);

                var result = _mapper.Map<OrderDto>(order);
                return ResponseDto<OrderDto>.Succes(result);
            }
            catch (Exception ex) 
            {
                return  ResponseDto<OrderDto>.Fail(ex.Message); 
            }

        }




        public async Task<ResponseDto<List<OrderDto>>> GetAllOrdersAsync()
        {
            try
            {
                var orders = await _orderRepository.GetAllOrdersAsync();
                if (orders == null || !orders.Any()) return ResponseDto<List<OrderDto>>.Fail("Sipariş bulunamadı.");

                var result = _mapper.Map<List<OrderDto>>(orders);
                return ResponseDto<List<OrderDto>>.Succes(result);
            }
            catch(Exception ex)
            {
                return ResponseDto<List<OrderDto>>.Fail(ex.Message);
            }
        }






        public async Task<ResponseDto<OrderDto>> UpdateOrderStatusAfterPaymentAsync(int orderId, PaymentStatus paymentStatus)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(orderId);
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

                var updateOrder = await _orderRepository.UpdateOrderAsync(order);
                var result = _mapper.Map<OrderDto>(updateOrder);

                return ResponseDto<OrderDto>.Succes(result);
            }
            catch(Exception ex)
            {
                return ResponseDto<OrderDto>.Fail(ex.Message);
            }

        }


        //##############################################################



        public async Task<ResponseDto<OrderDto>> AddItemToOrderAsync(AddItemToOrderDto dto)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(dto.OrderId);
                if (order == null)
                    return ResponseDto<OrderDto>.Fail("Sipariş bulunamadı.");

                var book =await _bookRepository.GetBookByIdAsync(dto.BookId);
                if (book == null || book.Stock < dto.Quantity)
                    return ResponseDto<OrderDto>.Fail("Stok yetersiz veya kitap bulunamadı.");

                book.Stock -= dto.Quantity;
                await _bookRepository.UpdateBookAsync(book.Id,book);

                order.AddItem(new OrderItem
                {
                    BookId = dto.BookId,
                    Quantity = dto.Quantity,
                    UnitPrice = book.Price
                });

                await _orderRepository.UpdateOrderAsync(order);

                var result = _mapper.Map<OrderDto>(order);
                return ResponseDto<OrderDto>.Succes(result);
            }
            catch (Exception ex)
            {
                return ResponseDto<OrderDto>.Fail(ex.Message);
            }
        }




        public async Task<ResponseDto<bool>> UpdateOrderItemAsync(int orderId, int bookId, int quantity)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(orderId);

                if (order == null)
                    return  ResponseDto<bool>.Fail("Sipariş bulunamadı");

                if (order.Status != OrderStatus.Pending)
                    return ResponseDto<bool>.Fail("Yalnızca Pending aşamasındak siparişler güncelleme yapabilir");

                var orderItem = order.Items.FirstOrDefault(item => item.BookId == bookId);
                if (orderItem == null)
                    return ResponseDto<bool>.Fail("OrderItem bulunamadı");

                if (quantity == 0)
                {
                    // Remove the item
                    order.Items.Remove(orderItem);

                    // Update book stock
                    var book =await  _bookRepository.GetBookByIdAsync(bookId);
                    if (book != null)
                        book.Stock += orderItem.Quantity;
                }
                else
                {
                    // Update quantity
                    var difference = orderItem.Quantity - quantity;
                    orderItem.Quantity = quantity;

                    // Update book stock
                    var book = await _bookRepository.GetBookByIdAsync(bookId);
                    if (book != null)
                        book.Stock += difference;
                }

                await _orderRepository.UpdateOrderAsync(order);
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
