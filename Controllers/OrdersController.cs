using BookStoreApp.Model.DTO.OrderDtos;
using BookStoreApp.Model.Entities;
using BookStoreApp.Model.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController (IOrderService orderService)
        {
            _orderService = orderService;
        }



        [HttpPost]
        public IActionResult AddOrder(OrderCreateDto orderCreateDto)
        {
            if (!ModelState.IsValid) return BadRequest("Sipariş oluşturulurken geçersiz veri gönderildi.");

            var response = _orderService.AddOrder(orderCreateDto.UserId,orderCreateDto);
            if (response.Data == null) return BadRequest(response);

            return CreatedAtAction(nameof(GetOrderById), new { id = response.Data.Id }, response);
        }



        [HttpGet ("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var response = _orderService.GetOrderById(id);
            if (response.Data == null) return NotFound(response);
            return Ok(response);
        }



        [HttpGet("user/{userId}")]
        public IActionResult GetOrdersByUserId (int userId)
        {
            var response =_orderService.GetOrdersByUserId(userId);
            if (response.Data == null || !response.Data.Any()) return NotFound(response);
            return Ok(response);

        }



        [HttpGet("status/{status}")]
        public IActionResult GetOrdersByStatus(OrderStatus status)
        {
            if (!Enum.IsDefined(typeof(OrderStatus), status)) return BadRequest("Geçersiz sipariş durumu");
            var response = _orderService.GetOrdersByStatus(status);
            if (response.Data == null || !response.Data.Any()) return NotFound(response);
            return Ok(response);

        }




        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var response = _orderService.GetAllOrders();
            if (response == null || !response.Data.Any()) return NotFound(response);
            return Ok(response);
        }



        [HttpPut("{id}/status")]
        public IActionResult UpdateOrderStatus(int id, [FromBody]OrderStatus status)
        {
            if (!Enum.IsDefined(typeof(OrderStatus), status)) return BadRequest("Geçersiz sipariş durumu");

            var response = _orderService.UpdateOrderStatus(id, status);
            if (response.Data== null) return NotFound(response);
            return Ok(response);
        }




        [HttpPut("{orderId}/payment-status")]
        public IActionResult UpdateOrderStatusAfterPayment(int orderId, [FromBody] PaymentStatus paymentStatus)
        {
            if (!Enum.IsDefined(typeof(PaymentStatus), paymentStatus)) return BadRequest("Geçersiz sipariş durumu");

            var response = _orderService.UpdateOrderStatusAfterPayment(orderId, paymentStatus);
            if (response.Data == null) return NotFound(response);
            return Ok(response);
        }



        [HttpPost("{orderId}/items")]
        public IActionResult AddItemToOrder(int orderId, [FromBody] AddItemToOrderDto dto)
        {
            dto.OrderId = orderId;
            var response = _orderService.AddItemToOrder(dto);
            if (response.Data == null)
                return BadRequest(response.Errors);
            return Ok(response);
        }




        [HttpDelete("{orderId}/items/{bookId}")]
        public IActionResult RemoveItemFromOrder(int orderId, int bookId)
        {
            var response = _orderService.RemoveItemFromOrder(new RemoveItemFromOrderDto
            {
                OrderId = orderId,
                BookId = bookId
            });
            if (response.Data == null)
                return BadRequest(response.Errors);
            return Ok(response);
        }


    }


}
