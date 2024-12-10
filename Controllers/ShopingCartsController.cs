using BookStoreApp.Model.DTO.ShoppingCartDtos;
using BookStoreApp.Model.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopingCartsController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShopingCartsController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService=shoppingCartService;
        }

        [HttpPost]
        public IActionResult AddCart(ShoppingCartCreateDto createCart)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = _shoppingCartService.AddShoppingCart(createCart);
            if (response.Data == null) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("{userId}")]
        public IActionResult GetCart(int userId)
        {
            var response = _shoppingCartService.GetCartByUserId(userId);
            if (response.Data == null) { return BadRequest(response); }
            return Ok(response);
        }


        [HttpDelete("{userId}/clear")]
        public IActionResult ClearCart(int userId)
        {
            var response = _shoppingCartService.ClearCart(userId);
            if (!response.Data) return BadRequest(Response);
            return Ok(response);
        }
    }
}
