using BookStoreApp.Model.DTO.OrderDtos;
using BookStoreApp.Model.DTO;
using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Interface
{
    public interface IOrderService
    {
        ResponseDto<OrderDto> AddOrder(OrderCreateDto orderCreateDto);
        ResponseDto<OrderDto> GetOrderById(int id);
        ResponseDto<List<OrderDto>> GetOrdersByUserId(int userId);
        ResponseDto<bool> UpdateOrderStatus(int id, OrderStatus status);
        ResponseDto<List<OrderDto>> GetAllOrders();

    }
}
