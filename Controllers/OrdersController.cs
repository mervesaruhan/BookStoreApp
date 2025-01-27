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
        public async Task<IActionResult> AddOrderAsync(OrderCreateDto orderCreateDto)
        {
            if (!ModelState.IsValid) return BadRequest("Sipariş oluşturulurken geçersiz veri gönderildi.");

            var response =await _orderService.AddOrderAsync(orderCreateDto.UserId,orderCreateDto);
            if (response.Data == null) return BadRequest(response);

            //return CreatedAtAction(nameof(GetOrderByIdAsync), new { id = response.Data.Id }, response);
            return Ok(response);
        }



        [HttpGet ("{id}")]
        public async Task<IActionResult> GetOrderByIdAsync(int id)
        {
            var response =await _orderService.GetOrderByIdAsync(id);
            if (response.Data == null) return NotFound(response);
            return Ok(response);
        }



        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserIdAsync(int userId)
        {
            var response = await _orderService.GetOrdersByUserIdAsync(userId);
            if (response.Data == null || !response.Data.Any()) return NotFound(response);
            return Ok(response);

        }



        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetOrdersByStatusAsync(OrderStatus status)
        {
            if (!Enum.IsDefined(typeof(OrderStatus), status)) return BadRequest("Geçersiz sipariş durumu");
            var response = await _orderService.GetOrdersByStatusAsync(status);
            if (response.Data == null || !response.Data.Any()) return NotFound(response);
            return Ok(response);

        }




        [HttpGet]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var response = await _orderService.GetAllOrdersAsync();
            if (response == null || !response.Data.Any()) return NotFound(response);
            return Ok(response);
        }



        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatusAsync(int id, [FromBody]OrderStatus status)
        {
            if (!Enum.IsDefined(typeof(OrderStatus), status)) return BadRequest("Geçersiz sipariş durumu");

            var response = await _orderService.UpdateOrderStatusAsync(id, status);
            if (response.Data== null) return NotFound(response);
            return Ok(response);
        }




        [HttpPut("{orderId}/payment-status")]
        public async Task<IActionResult> UpdateOrderStatusAfterPaymentAsync(int orderId, [FromBody] PaymentStatus paymentStatus)
        {
            if (!Enum.IsDefined(typeof(PaymentStatus), paymentStatus)) return BadRequest("Geçersiz sipariş durumu");

            var response = await _orderService.UpdateOrderStatusAfterPaymentAsync(orderId, paymentStatus);
            if (response.Data == null) return NotFound(response);
            return Ok(response);
        }



        [HttpPost("{orderId}/items")]
        public async Task<IActionResult> AddItemToOrderAsync(int orderId, [FromBody] AddItemToOrderDto dto)
        {
            dto.OrderId = orderId;
            var response =await _orderService.AddItemToOrderAsync(dto);
            if (response.Data == null)
                return BadRequest(response.Errors);
            return Ok(response);
        }




        [HttpPut("{orderId}/items/{bookId}")]
        public async Task<IActionResult> UpdateOrderItemAsync(int orderId, int bookId, [FromBody] int quantity)
        {
            if (quantity < 0)
            {
                return BadRequest("Girilen miktar 0 ya da  pozitif bir sayı olalıdır.");
            }

            // Servisi çağır
            var response = await _orderService.UpdateOrderItemAsync(orderId, bookId, quantity);

            // Hataları kontrol et
            if (response.Errors != null && response.Errors.Any())
            {
                return BadRequest(response.Errors);
            }

            // Başarılı yanıt
            return Ok(new
            {
                Message = "Order item ubaşarılı şekilde güncellendi.",
                Success = response.Data
            });
        }


    }


}
