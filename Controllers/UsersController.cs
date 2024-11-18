using BookStoreApp.Model.DTO;
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
            #region Comment
            //var user = new User
            //{
            //    FullName = userDto.FullName,
            //    Email = userDto.Email,
            //    Role = userDto.Role
            //}; 
            #endregion
            var createdUser = _userService.RegisterUser(userDto);


            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);

        }





        [HttpPost("login")]
        public IActionResult LoginUser(UserLoginDto userLoginDto)
        {
            var (user, message) = _userService.AuthenticateUser(userLoginDto);
            if (user == null)
            {
                return Unauthorized("Kullanıcı adı veya şifre yanlış");
            }

            return Ok("Giriş başarılı!");
        }


        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();

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

            return Ok(users);

        }






        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }



        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UserUpdateDto updatedUserDto)
        {
   
            var result = _userService.UpdateUser(id,  updatedUserDto);
            if (result == null) return NotFound("Kullanıcı bulunamadı.");

            return Ok(result);

        }




        [HttpDelete ("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var result = _userService.DeleteUser(id);
            if (!result) return NotFound("Kullanıcı bulunamadı.");

            return NoContent();
        }




    }
}
