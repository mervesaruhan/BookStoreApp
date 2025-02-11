using AutoMapper;
using BookStoreApp.Model.DTO.OrderDtos;
using BookStoreApp.Model.Interface;
using BookStoreApp.Model.Entities;
using BookStoreApp.Model.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using BookStoreApp.Model.DTO.PaymentDtos;
using Microsoft.EntityFrameworkCore;

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
                //1.tüm kitapları çek
                var bookIds = orderCreateDto.Items.Select(x => x.BookId).ToList();
                var books = await _bookRepository.GetBooksByIdsAsync(bookIds);

                //eksik kitap var mı kontrolü
                var missingBookIds = bookIds.Except(books.Select(x => x.Id)).ToList();
                if (missingBookIds.Any())
                    return ResponseDto<OrderDto>.Fail($"Girilen ID'lerde kitap bulunamadı: {string.Join(",", missingBookIds)}");

                //stok kontrolü
                foreach (var item in orderCreateDto.Items)
                {
                    var book = books.FirstOrDefault(x => x.Id == item.BookId);
                    if (book!.Stock < item.Quantity)
                        return ResponseDto<OrderDto>.Fail($"Stok yetersiz: {book.Title}");
                }

                // sipariş kalemleri ve stok güncellemesi
                var orderItems = new List<OrderItem>();
                decimal totalOrderPrice = 0;
                foreach(var item in orderCreateDto.Items)
                {
                    var book = books.First(x => x.Id == item.BookId);
                    book.Stock -= item.Quantity;
                    

                    var orderItem = new OrderItem
                    {
                        BookId = book.Id,
                        Quantity = item.Quantity,
                        UnitPrice = book.Price,
                        TotalPrice = book.Price * item.Quantity
                    };

                    totalOrderPrice += orderItem.TotalPrice;
                    orderItems.Add(orderItem);
                }

                //transaction başlat ve sipariş oluştur
                using (var transaction = await _orderRepository.BeginTransactionAsync())
                {
                    try
                    {
                        var order = new Order
                        {
                            UserId = userId,
                            OrderItems = orderItems,
                            TotalPrice = totalOrderPrice,
                            Status = OrderStatus.Pending
                        };

                        var createdOrder = await _orderRepository.AddOrderAsync(order);
                        await _bookRepository.UpdateBooksAsync(books);

                        await transaction.CommitAsync(); //*Her şey başarılıysa işlemi onayla*

                        var result = _mapper.Map<OrderDto>(createdOrder);
                        return ResponseDto<OrderDto>.Succes(result);
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync(); //**Hata olursa her şeyi geri al**
                        throw;
                    }
                }
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
                using (var transaction = await _orderRepository.BeginTransactionAsync())
                {
                    var order = await _orderRepository.GetOrderByIdAsync(id);
                    if (order == null) return ResponseDto<OrderDto>.Fail("Girilen ID'de sipariş bulunamadı");

                    if (newStatus == OrderStatus.Cancelled && order.Status != OrderStatus.Cancelled)
                    {
                        foreach (var item in order.OrderItems) //sipariş iptal eidlirse stokları geri yükleme
                        {
                            var book = await _bookRepository.GetBookByIdAsync(item.BookId);
                            if (book != null)
                            {
                                book.Stock += item.Quantity;
                                await _bookRepository.UpdateBookAsync(book.Id, book);
                            }
                            else
                            {
                                return ResponseDto<OrderDto>.Fail("Girilen ID'de kitap bulunamadı, stok işlemleri başarısız!");
                            }
                        }
                    }
                    else if (newStatus != OrderStatus.Cancelled && order.Status == OrderStatus.Cancelled)
                    {
                        foreach (var item in order.OrderItems) //iptal edilen sipariş tekrar aktif hale getirilirse stoktan düşme
                        {
                            var book = await _bookRepository.GetBookByIdAsync(item.BookId);
                            if (book != null && book.Stock >= item.Quantity)
                            {
                                book.Stock -= item.Quantity;
                                await _bookRepository.UpdateBookAsync(book.Id, book);
                            }
                            else
                            {
                                return ResponseDto<OrderDto>.Fail("Girilen ID'de kitap bulunamadı veya "
                                    + $"Stok yetersiz: {item.BookId}" + " stok işlemleri başarısız!");

                            }
                        }
                    }
                    order.Status = newStatus;
                    await _orderRepository.UpdateOrderAsync(order);

                    await transaction.CommitAsync(); //t.a. onaylama işlemi
                    var result = _mapper.Map<OrderDto>(order);
                    return ResponseDto<OrderDto>.Succes(result);
                }

            }
            catch (Exception ex)
            {
                return ResponseDto<OrderDto>.Fail($"Sipariş güncellenirken hata oluştu: {ex.Message}");
            }
        }



        //******

        public async Task<ResponseDto<List<OrderDto>>> GetAllOrdersAsync()
        {
            try
            {
                var orders = await _orderRepository.GetAllOrdersAsync();
                if (orders == null || !orders.Any()) return ResponseDto<List<OrderDto>>.Fail("Sipariş bulunamadı.");

                var result = _mapper.Map<List<OrderDto>>(orders);
                return ResponseDto<List<OrderDto>>.Succes(result);
            }
            catch (Exception ex)
            {
                return ResponseDto<List<OrderDto>>.Fail(ex.Message);
            }
        }



        


        public async Task<ResponseDto<OrderDto>> UpdateOrderStatusAfterPaymentAsync(int orderId, PaymentStatus paymentStatus)
        {
            try
            {
                using (var transaction = await _orderRepository.BeginTransactionAsync())
                {
                    var order = await _orderRepository.UpdaterOrderStatusAfterPayment(orderId, paymentStatus);
                   

                    await transaction.CommitAsync();
                    var result = _mapper.Map<OrderDto>(order);
                    return ResponseDto<OrderDto>.Succes(result);
                }
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseDto<OrderDto>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseDto<OrderDto>.Fail($"Ödeme sonrası sipariş güncellenirken hata oluştu: {ex.Message}");
            }

        }




        //##############################################################



        public async Task<ResponseDto<OrderDto>> AddItemToOrderAsync(AddItemToOrderDto dto)
        {
            try
            {
                var order = await _orderRepository.AddItemToOrderAsync(dto.OrderId, dto.BookId, dto.Quantity);
                var result = _mapper.Map<OrderDto>(order);
                return ResponseDto<OrderDto>.Succes(result);
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseDto<OrderDto>.Fail(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return ResponseDto<OrderDto>.Fail($"Siparişe ürün eklenirken hata oluştu: {ex.Message}");
            }
        }








        public async Task<ResponseDto<OrderDto>> UpdateOrderItemAsync(int orderId, int bookId, int quantity)
        {
            try
            {
                using (var transaction = await _orderRepository.BeginTransactionAsync()) // ✅ Transaction Başlat
                {
                    var order = await _orderRepository.UpdateOrderItemAsync(orderId, bookId, quantity);
                    var result = _mapper.Map<OrderDto>(order);

                    await transaction.CommitAsync(); // ✅ Transaction'ı Onayla
                    return ResponseDto<OrderDto>.Succes(result);
                }
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseDto<OrderDto>.Fail(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return ResponseDto<OrderDto>.Fail($"Sipariş öğesi güncellenirken hata oluştu: {ex.Message}");
            }
        }









    }
}
