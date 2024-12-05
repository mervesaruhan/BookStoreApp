using AutoMapper;
using BookStoreApp.Model.DTO;
using BookStoreApp.Model.DTO.UserDtos;
using BookStoreApp.Model.Entities;
using BookStoreApp.Model.Interface;

namespace BookStoreApp.Model.Service
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;

        }



        public ResponseDto<UserDto> RegisterUser(UserRegisterDto userDto)
        {
            #region Manuel Mapping
            //var user = new User
            //{
            //    FullName = userDto.FullName,
            //    Email = userDto.Email,
            //    Role = userDto.Role
            //};

            //var passwordhashsalt = createpasswordhash(userdto.password);
            //user.passwordhash = passwordhashsalt.hash;
            //user.passwordsalt = passwordhashsalt.salt;

            //var createduser =_userRepository.Add(user);

            //return new UserDto
            //{
            //    Id = createduser.Id,
            //    FullName = createduser.FullName,
            //    Email = createduser.Email,
            //    Role = createduser.Role
            //}; 
            #endregion
            //UserRegisterDto -> user dönüşümü
            var user = _mapper.Map<User>(userDto);


            //şifre hashleme
            var passwordHashSalt = CreatePasswordHash(userDto.Password);
            user.PasswordHash = passwordHashSalt.Hash;
            user.PasswordSalt = passwordHashSalt.Salt;


            //kullanıcı kaydet ve user->userdto dönüşümü
            var createdUser = _userRepository.Add(user);
            var userDtoResult = _mapper.Map<UserDto>(createdUser);

            return ResponseDto<UserDto>.Succes(userDtoResult);

        }



        public ResponseDto<UserDto>  AuthenticateUser(UserLoginDto userLoginDto)
        {
            var user =_userRepository.GetByEmail(userLoginDto.Email);
            if (user == null || !VerifyPassword(userLoginDto.Password, user.PasswordHash, user.PasswordSalt)){
                return ResponseDto<UserDto>.Fail("Kullanıcı adı veya şifre yanlış!");
            }

            #region Manuel mapping
            //var loginDto = new UserDto
            //{
            //    Id =user.Id,
            //    FullName=user.FullName,
            //    Email = user.Email,
            //    Role = user.Role
            //}; 
            #endregion
            var loginDto = _mapper.Map<UserDto?>(user);

            return ResponseDto<UserDto>.Succes(loginDto!);
        }


        public ResponseDto<UserDto>? GetUserById(int id)
        {
            var user =_userRepository.GetById(id);
            if (user == null)
            {
                return ResponseDto<UserDto>.Fail("Kullanıcı bulunamadı! ");
            }

            #region Manuel mapping
            //return new UserDto
            //{
            //    Id = user.Id,
            //    FullName = user.FullName,
            //    Email = user.Email,
            //    Role = user.Role
            //}; 
            #endregion
            var result = _mapper.Map<UserDto>(user);
            return ResponseDto<UserDto>.Succes(result);
        }



        public ResponseDto<List<UserDto>> GetAllUsers()
        {

            var users = _userRepository.GetAll();
            if (users == null) return ResponseDto<List<UserDto>>.Fail("Hiçbir kullanıcı bulunamadı.");

            #region manuel mapping
            //return users.Select(user => new UserDto
            //{
            //    Id=user.Id,
            //    FullName = user.FullName,
            //    Email = user.Email,
            //    Role = user.Role

            //}).ToList(); 
            #endregion
            var resultList = _mapper.Map<List<UserDto>>(users);

            return ResponseDto<List<UserDto>>.Succes(resultList);  
        }


        public ResponseDto<UserDto> UpdateUser(int id, UserUpdateDto updatedUserDto)
        {
            var user = _userRepository.GetById(id);
            if (user == null)  return ResponseDto<UserDto>.Fail("Kullanıcı bulunamadı."); 

            user.Email = updatedUserDto.Email;
            user.FullName = updatedUserDto.FullName;


            var updatedUser = _userRepository.Update(user);

            #region manuel mapping
            //return new UserDto
            //{
            //    Id = updatedUser.Id ,
            //    FullName = updatedUser.FullName,
            //    Email = updatedUser.Email,
            //    Role = updatedUser.Role

            //}; 
            #endregion
            var resultDto = _mapper.Map<UserDto>(updatedUser);

            return ResponseDto<UserDto>.Succes(resultDto);

        }





        public ResponseDto<bool> DeleteUser(int id)
        {
            var result =  _userRepository.Delete(id);

            if (!result) return ResponseDto<bool>.Fail("Kullanıcı bulunamadı veya silinemedi");

            return ResponseDto<bool>.Succes(true);
        }


        #region Hash
        private (byte[] Hash, byte[] Salt) CreatePasswordHash(string password)
        {
            return (new byte[0], new byte[0]);
        }
        private bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            return true;
        }

     
        #endregion







    }

}
