using BookStoreApp.Model.Entities;
using BookStoreApp.Model.Interface;
using BookStoreApp.Model.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookStoreApp.Model.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        public OrderRepository (AppDbContext context)
        {
            _context = context;
        }

        

        public async Task<Order> AddOrderAsync(Order order)
        {
            order.OrderDate = DateTime.UtcNow;
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return order;
        }


        public async Task< List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .Include(o => o.Payment)
                .ToListAsync();

        }



        public async Task<Order> UpdateOrderAsync(Order updatedOrder)
        {
            var existingOrder = await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == updatedOrder.Id);


            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Girilen kitap ID'sinde '{updatedOrder.Id}' sipariş bulunamadı.");
            }

            // Güncellemeler
            existingOrder.Status = updatedOrder.Status;
            existingOrder.TotalPrice = updatedOrder.TotalPrice;
            existingOrder.OrderItems = updatedOrder.OrderItems;
            existingOrder.OrderDate = updatedOrder.OrderDate;

            return existingOrder ;
        }




        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems) // Siparişin içindeki ürünleri getir.
                .ThenInclude(oi => oi.Book) // Sipariş kalemlerinin kitap bilgilerini getir.
                .Include(o => o.Payment) // Siparişe bağlı ödeme bilgilerini getir.
                .AsNoTracking() // Performans için EF Core takip etmesin.
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                throw new KeyNotFoundException($"ID {id} ile sipariş bulunamadı.");

            return order;
        }




        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .Include(o => o.Payment)
                .AsNoTracking()
                .ToListAsync();

            if (!orders.Any()) throw new KeyNotFoundException($"'{userId}' : ID'deki kullanıcının siparişi yoktur.");

            return orders;

        }




        public async Task<Order?> UpdaterOrderStatusAfterPayment(int orderId, PaymentStatus paymentStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Girilen Id'de ({orderId}) sipariş bulunamadı.");
            }

            if (paymentStatus == PaymentStatus.Completed)
            {
                order.Status = OrderStatus.Shipped;
            }
            else
            {
                order.Status = OrderStatus.Pending;
                throw new Exception("Sipariş durumu ödeme tamamlanmadığı için güncellenemedi.");
            }

            await _context.SaveChangesAsync();

            return order;
        }






        public async Task<Order?> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) 
            {
                throw new KeyNotFoundException($"Girilen Id'de ({orderId}) sipariş bulunamadı.");
            }

            order.Status = status;
            await _context.SaveChangesAsync();

            return order;
        }







        public async Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            var orders = await _context.Orders
                .Where(o => o.Status == status)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .Include(o => o.Payment)
                .AsNoTracking()
                .ToListAsync();

            return orders; ;
        }







        public async Task<Order?> AddItemToOrderAsync(int orderId, int bookId, int quantity)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new KeyNotFoundException($"Sipariş bulunamadı. ID: {orderId}");

            var book = await _context.Books.FindAsync(bookId);
            if (book == null || book.Stock < quantity)
                throw new InvalidOperationException($"Kitap bulunamadı veya stok yetersiz! Kitap ID: {bookId}");

            var orderItem = new OrderItem
            {
                BookId = book.Id,
                Quantity = quantity,
                UnitPrice = book.Price
            };

            order.AddItem(orderItem); // ✅ **Entity içindeki `AddItem()` metodunu kullandık**

            book.Stock -= quantity; // **Stok düşürüyoruz**

            //order.TotalPrice = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice); // **Sipariş toplamını güncelle**

            await _context.SaveChangesAsync(); // **Veritabanına kaydet**
            return order;
        }







        public async Task<Order?> RemoveItemFromOrderAsync(int orderId, int bookId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new KeyNotFoundException($"Sipariş bulunamadı. ID: {orderId}");

            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
                throw new InvalidOperationException($"Kitap bulunamadı! Kitap ID: {bookId}");

            var orderItem = order.OrderItems.FirstOrDefault(i => i.BookId == bookId);
            if (orderItem == null)
                throw new InvalidOperationException($"Bu siparişte Kitap ID {bookId} bulunmuyor!");

            order.RemoveItem(bookId); // ✅ **Entity içindeki `RemoveItem()` metodunu kullandık**

            book.Stock += orderItem.Quantity; // **Kitap stoklarını geri ekliyoruz**

            //order.TotalPrice = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice); // **Sipariş toplamını güncelle**

            await _context.SaveChangesAsync(); // **Veritabanına kaydet**
            return order;
        }






        public async Task<Order?> UpdateOrderItemAsync(int orderId, int bookId, int newQuantity)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new KeyNotFoundException($"Sipariş bulunamadı. ID: {orderId}");

            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
                throw new InvalidOperationException($"Kitap bulunamadı! Kitap ID: {bookId}");

            var orderItem = order.OrderItems.FirstOrDefault(i => i.BookId == bookId);
            if (orderItem == null)
                throw new InvalidOperationException($"Bu siparişte Kitap ID {bookId} bulunmuyor!");

            // ✅ **Eski miktarı alalım ki stok farkını hesaplayabilelim**
            int previousQuantity = orderItem.Quantity;

            if (newQuantity == 0)
            {
                // ✅ **Miktar 0 ise ürünü tamamen çıkar**
                order.RemoveItem(bookId);
                book.Stock += previousQuantity; // **Stok geri ekleniyor**
            }
            else
            {
                // ✅ **Yeni miktarı güncelle**
                if (book.Stock + previousQuantity < newQuantity)
                {
                    throw new InvalidOperationException($"Stok yetersiz! Kitap ID: {bookId}");
                }

                book.Stock += previousQuantity - newQuantity; // **Stok değişimini uygula**
                orderItem.Quantity = newQuantity;
            }

            // ✅ **Sipariş toplam fiyatını güncelle**
            //order.TotalPrice = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);

            await _context.SaveChangesAsync();
            return order;
        }










        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }





       

    }
    
    
}
