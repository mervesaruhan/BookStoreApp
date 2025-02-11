using BookStoreApp.Model.DTO.PaymentDtos;
using BookStoreApp.Model.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<IActionResult> AddPaymentAsync([FromQuery]int orderId, [FromBody]PaymentCreateDto paymentCreateDto)
        {
            if (!ModelState.IsValid) return BadRequest(new { Message = "Geçersiz veri gönderildi", Errors = ModelState.Values
                .SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });

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


        [HttpGet("{id:int}")] // yanlıs deger girilmesini engellemek için
        public async Task<IActionResult> GetPaymentByIdAsync(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Geçersiz ödeme ID" });

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
