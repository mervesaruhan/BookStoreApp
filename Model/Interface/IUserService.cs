using BookStoreApp.Model.DTO;
using BookStoreApp.Model.DTO.UserDtos;
using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Interface
{
    public interface IUserService
    {
        Task<ResponseDto<UserDto>> RegisterUserAsync(UserRegisterDto userDto);
        Task<ResponseDto<UserDto>> AuthenticateUserAsync(UserLoginDto userLoginDto);
        Task<ResponseDto<UserDto>>? GetUserByIdAsync(int id);
        Task<ResponseDto<List<UserDto>>> GetAllUsersAsync();
        Task<ResponseDto<UserDto>> UpdateUserAsync(int id, UserUpdateDto updatedUserDto);
        Task<ResponseDto<bool>> DeleteUserAsync(int id);

    }
}
