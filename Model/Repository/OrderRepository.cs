using BookStoreApp.Model.Entities;
using BookStoreApp.Model.Interface;

namespace BookStoreApp.Model.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public OrderRepository() { }

        private readonly List<Order> _orders = new();


        public async Task<Order> AddOrderAsync(Order order)
        {
            order.Id = _orders.Count + 1;
            order.OrderDate = DateTime.UtcNow;
            _orders.Add(order);

            return await Task.FromResult(order);
        }


        public async Task< List<Order>> GetAllOrdersAsync()
        {
            return await Task.FromResult( _orders);
        }



        public async Task<Order> UpdateOrderAsync(Order updatedOrder)
        {
            var existingOrder =  _orders.FirstOrDefault(o => o.Id == updatedOrder.Id);
            if (existingOrder == null)
            {
                throw new KeyNotFoundException("Girilen ID'de sipariş bulunamadı.");
            }

            // Güncellemeler
            existingOrder.Status = updatedOrder.Status;
            existingOrder.TotalPrice = updatedOrder.TotalPrice;
            existingOrder.Items = updatedOrder.Items;
            existingOrder.OrderDate = updatedOrder.OrderDate;

            return await Task.FromResult(existingOrder);
        }




        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            var orders = _orders.FirstOrDefault(o => o.Id == id);
            if (orders == null) throw new KeyNotFoundException("Girilen id'de sipariş bulunmamaktadır");
            return await Task.FromResult(orders);
        }



        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = _orders.Where(o => o.UserId == userId).ToList();
            if (!orders.Any()) throw new KeyNotFoundException("Bu id'deki kullanıcının siparişi yoktur.");

            return await Task.FromResult(orders);

        }



        public async Task<Order?> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order == null) 
            {
                throw new KeyNotFoundException($"Girilen Id'de ({orderId}) sipariş bulunamadı.");
            }

            order.Status = status;

            return await Task.FromResult(order);
        }





        public async Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            var orders = _orders.Where(o => o.Status == status).ToList();
            return await Task.FromResult(orders);
        } 
    }
    
    
}
