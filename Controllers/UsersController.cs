using BookStoreApp.Model.DTO.UserDtos;
using BookStoreApp.Model.Entities;
using BookStoreApp.Model.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }





        [HttpPost ("register")]
        public async Task<IActionResult> RegisterUserAsync(UserRegisterDto userDto)
        {
            if (!ModelState.IsValid) { return BadRequest("Geçersiz veri gönderildi"); }

            var response = await _userService.RegisterUserAsync(userDto);

            if (response.Data != null)
            {
                //return CreatedAtAction(nameof(GetUserByIdAsync), new { id = response.Data.Id }, response);
                return Ok(response);
            }

            return BadRequest(response.Errors);

        }





        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync(UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid) return BadRequest("Kullanıcı adı veya şifre yanlış.");

            var response =await  _userService.AuthenticateUserAsync(userLoginDto);
            if (response.Data == null)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var response =await _userService.GetAllUsersAsync();
            if (response.Data == null || !response.Data.Any()) return NotFound(response);


            return Ok(response);

        }






        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            var response = await _userService.GetUserByIdAsync(id);
            if (response?.Data == null) return NotFound(response);
            return Ok(response);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, UserUpdateDto updatedUserDto)
        {
   
            var response = await _userService.UpdateUserAsync(id,  updatedUserDto);
            if (response.Data == null) return NotFound(response);

            return Ok(response);

        }




        [HttpDelete ("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var response =await _userService.DeleteUserAsync(id);
            if (!response.Data) return NotFound(response);

            return NoContent();
        }


        [HttpGet("roles")]
        public IActionResult GetUserRoles()
        {
            var roles = Enum.GetValues(typeof(UserRole))
                            .Cast<UserRole>()
                            .Select(r => new { Key = (int)r, Value = r.ToString() })
                            .ToList();
            return Ok(roles);
        }



    }
}
