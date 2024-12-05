using BookStoreApp.Model.DTO;
using BookStoreApp.Model.DTO.UserDtos;
using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Interface
{
    public interface IUserService
    {
        ResponseDto<UserDto> RegisterUser(UserRegisterDto userDto);
        ResponseDto<UserDto> AuthenticateUser(UserLoginDto userLoginDto);
        ResponseDto<UserDto> GetUserById(int id);
        ResponseDto<List<UserDto>> GetAllUsers();
        ResponseDto<UserDto> UpdateUser(int id, UserUpdateDto updatedUserDto);
        ResponseDto<bool> DeleteUser(int id);

    }
}
