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
            if (!ModelState.IsValid) return BadRequest("Geçersiz veri gönderildi.");

            var response = _orderService.AddOrder(orderCreateDto);
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
            if (response.Data == null || !response.Data.Any()) return NotFound(Response);
            return Ok(response);

        }

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var response = _orderService.GetAllOrders();
            if (response == null || !response.Data.Any()) return NotFound(Response);
            return Ok(response);
        }

        [HttpPut("{id}/status")]
        public IActionResult UpdateOrderStatus(int id, [FromBody]OrderStatus status)
        {
            var response = _orderService.UpdateOrderStatus(id, status);
            if (!response.Data) return NotFound(response);
            return Ok(response);
        }

    }
}
