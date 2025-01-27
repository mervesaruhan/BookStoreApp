using BookStoreApp.Model.DTO.OrderDtos;
using BookStoreApp.Model.DTO;
using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Interface
{
    public interface IOrderService
    {
        Task<ResponseDto<OrderDto>> AddOrderAsync(int userId, OrderCreateDto orderCreateDto);
        Task<ResponseDto<OrderDto>> GetOrderByIdAsync(int id);
        Task<ResponseDto<List<OrderDto>>> GetOrdersByUserIdAsync(int userId);
        Task<ResponseDto<List<OrderDto>>> GetOrdersByStatusAsync(OrderStatus status);
        Task<ResponseDto<OrderDto>> UpdateOrderStatusAsync(int id, OrderStatus newStatus);
        Task<ResponseDto<List<OrderDto>>> GetAllOrdersAsync();
        Task<ResponseDto<OrderDto>> UpdateOrderStatusAfterPaymentAsync(int orderId, PaymentStatus paymentStatus);
        Task<ResponseDto<OrderDto>> AddItemToOrderAsync(AddItemToOrderDto dto);
        Task<ResponseDto<bool>> UpdateOrderItemAsync(int orderId, int bookId, int quantity);

    }
}
