using AutoMapper;
using BookStoreApp.Model.DTO.OrderDtos;
using BookStoreApp.Model.Interface;
using BookStoreApp.Model.Entities;
using BookStoreApp.Model.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookStoreApp.Model.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService (IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }


        public ResponseDto<OrderDto> AddOrder(OrderCreateDto orderCreateDto)
        {
            try
            {
                var order = _mapper.Map<Order>(orderCreateDto);

                order.TotalPrice = order.Items.Sum(i => i.TotalPrice);

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


    }
}
