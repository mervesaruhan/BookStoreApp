using BookStoreApp.Model.DTO.PaymentDtos;
using BookStoreApp.Model.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService=paymentService;
        }



        [HttpPost]
        public async Task<IActionResult> AddPaymentAsync(int orderId, PaymentCreateDto paymentCreateDto)
        {
            if (!ModelState.IsValid) return BadRequest("Geçersiz veri gönderildi");

            var response = await _paymentService.AddPaymentAsync(orderId, paymentCreateDto);
            if (response.Data == null)
            {
                return NotFound(new
                {
                    Message = "Siparişe ait ödeme bulunamadı.",
                    Errors = response.Errors
                });
            }

            //return CreatedAtAction(nameof(GetPaymentByIdAsync), new { id = response.Data.Id }, response);
            return Ok(response);

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentByIdAsync(int id)
        {
            var response = await  _paymentService.GetPaymentByIdAsync(id);
            if (response.Data == null) return NotFound(response);

            return Ok(response);

        }



        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPaymentsByUserIdAsync(int userId)
        {
            var response = await _paymentService.GetPaymentsByUserIdAsync(userId);
            if (response.Data == null || !response.Data.Any()) return NotFound(response);

            return Ok(response);
        }



        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetPaymentByOrderIdAsync( int orderId)
        {
            var response = await _paymentService.GetPaymentByOrderIdAsync((int)orderId);
            if (response.Data == null ) return NotFound(response);
            return Ok(response);
        }



        [HttpGet]
        public async Task<IActionResult> GetAllPaymentsAsync()
        {
            var response = await _paymentService.GetAllPaymentsAsync();
            if (response.Data == null || !response.Data.Any()) return NotFound(response);

            return Ok(response);
        }
    }
}
