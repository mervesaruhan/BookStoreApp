using BookStoreApp.Model.DTO;
using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Interface
{
    public interface IUserService
    {
        UserDto RegisterUser(UserRegisterDto userDto);
        (UserDto? loginDto, string Message) AuthenticateUser(UserLoginDto userLoginDto);
        UserDto GetUserById(int id);
        List<UserDto> GetAllUsers();
        UserDto UpdateUser(int id, UserUpdateDto updatedUserDto);
        bool DeleteUser(int id);

    //    User RegisterUser(UserRegisterDto userDto, string password);
    //    (User? user, string Message) AuthenticateUser(string email, string password);
    //    User GetUserById(int id);
    //    List<User> GetAllUsers();
    //    User UpdateUser(int id, User updatedUser);
    //    bool DeleteUser(int id);

    }
}
