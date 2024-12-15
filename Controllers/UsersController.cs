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
        public IActionResult RegisterUser(UserRegisterDto userDto)
        {
            if (!ModelState.IsValid) { return BadRequest("Geçersiz veri gönderildi"); }

            var response = _userService.RegisterUser(userDto);

            if (response.Data != null)
            {
                return CreatedAtAction(nameof(GetUserById), new { id = response.Data.Id }, response);
            }

            return BadRequest(response.Errors);

        }





        [HttpPost("login")]
        public IActionResult LoginUser(UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid) return BadRequest("Kullanıcı adı veya şifre yanlış.");

            var response = _userService.AuthenticateUser(userLoginDto);
            if (response.Data == null)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }


        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var response = _userService.GetAllUsers();
            if (response.Data == null || !response.Data.Any()) return NotFound(response);

            #region AlternativeWay
            //var userDtos = users.Select(user => new UserDto
            //{
            //    Id = user.Id,
            //    FullName = user.FullName,
            //    Email = user.Email,
            //    Role = user.Role
            //}).ToList(); 
            #endregion

            #region Response Former
            //var userDtos = new List<UserDto>();
            //foreach (var user in users)
            //{
            //    var userDto = new UserDto
            //    {
            //        Id =user.Id,
            //        FullName = user.FullName,
            //        Email = user.Email,
            //        Role = user.Role
            //    };
            //    userDtos.Add(userDto);
            //} 
            #endregion

            return Ok(response);

        }






        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var response = _userService.GetUserById(id);
            if (response?.Data == null) return NotFound(response);
            return Ok(response);
        }



        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UserUpdateDto updatedUserDto)
        {
   
            var response = _userService.UpdateUser(id,  updatedUserDto);
            if (response.Data == null) return NotFound(response);

            return Ok(response);

        }




        [HttpDelete ("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var response = _userService.DeleteUser(id);
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
