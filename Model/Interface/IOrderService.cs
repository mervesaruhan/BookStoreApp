using BookStoreApp.Model.DTO.OrderDtos;
using BookStoreApp.Model.DTO;
using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Interface
{
    public interface IOrderService
    {
        ResponseDto<OrderDto> AddOrder(int userId, OrderCreateDto orderCreateDto);
        ResponseDto<OrderDto> GetOrderById(int id);
        ResponseDto<List<OrderDto>> GetOrdersByUserId(int userId);
        ResponseDto<List<OrderDto>> GetOrdersByStatus(OrderStatus status);
        ResponseDto<OrderDto> UpdateOrderStatus(int id, OrderStatus status);
        ResponseDto<List<OrderDto>> GetAllOrders();
        ResponseDto<OrderDto> UpdateOrderStatusAfterPayment(int orderId, PaymentStatus paymentStatus);
        ResponseDto<OrderDto> UpdateOrder(OrderUpdateDto updateDto);

    }
}
