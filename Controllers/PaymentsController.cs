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

        public PaymentsController (IPaymentService paymentService)
        {
            _paymentService=paymentService;
        }

        [HttpPost]
        public IActionResult AddPayment(PaymentCreateDto paymentCreateDto)
        {
            if (!ModelState.IsValid) return BadRequest("Geçersiz veri gönderildi");

            var response = _paymentService.AddPayment(paymentCreateDto);
            if (response.Data == null) return NotFound(response);
            return CreatedAtAction(nameof(GetPaymentById), new { id = response.Data.Id }, response);

        }


        [HttpGet("{id}")]
        public IActionResult GetPaymentById(int id)
        {
            var response = _paymentService.GetPaymentById(id);
            if (response.Data == null) return NotFound(response);

            return Ok(response);

        }



        [HttpGet("user/{id}")]
        public IActionResult GetPaymentsByUserId(int userId)
        {
            var response = _paymentService.GetPaymentsByUserId(userId);
            if (response.Data == null || !response.Data.Any()) return NotFound(response);

            return Ok(response);
        }


        [HttpGet]
        public IActionResult GetAllPayments()
        {
            var response = _paymentService.GetAllPayments();
            if (response.Data == null || !response.Data.Any()) return NotFound(response);

            return Ok(response);
        }
    }
}
