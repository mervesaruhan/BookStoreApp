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



        public async Task<ResponseDto<UserDto>> RegisterUserAsync(UserRegisterDto userDto)
        {
            // Enum doğrulama: Role değeri geçerli mi?
            if (!Enum.TryParse<UserRole>(userDto.Role.ToString(), out var parsedRole) || !Enum.IsDefined(typeof(UserRole), parsedRole))
            {
                
                return ResponseDto<UserDto>.Fail("Geçersiz kullanıcı rolü! Sadece Admin (0) veya Customer (1) değerlerini kabul eder.");
            }

            //UserRegisterDto -> user dönüşümü
            var user = _mapper.Map<User>(userDto);
            user.Role = parsedRole; // Doğrulanmış enum değeri atanıyor


            //şifre hashleme
            var passwordHashSalt = CreatePasswordHash(userDto.Password);
            user.PasswordHash = passwordHashSalt.Hash;
            user.PasswordSalt = passwordHashSalt.Salt;


            //kullanıcı kaydet ve user->userdto dönüşümü
            var createdUser =await  _userRepository.AddAsync(user);
            var userDtoResult = _mapper.Map<UserDto>(createdUser);


            return ResponseDto<UserDto>.Succes(userDtoResult);

        }




        public async Task<ResponseDto<UserDto>> AuthenticateUserAsync(UserLoginDto userLoginDto)
        {
            var user = await _userRepository.GetByEmailAsync(userLoginDto.Email);
            if (user == null || !VerifyPassword(userLoginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return ResponseDto<UserDto>.Fail("Kullanıcı adı veya şifre yanlış!");
            }

            var loginDto = _mapper.Map<UserDto>(user);
            return ResponseDto<UserDto>.Succes(loginDto);
        }



        public async Task<ResponseDto<UserDto>>? GetUserByIdAsync(int id)
        {
            var user =await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return ResponseDto<UserDto>.Fail("Kullanıcı bulunamadı! ");
            }

            var result = _mapper.Map<UserDto>(user);
            return ResponseDto<UserDto>.Succes(result);
        }



        public async Task<ResponseDto<List<UserDto>>> GetAllUsersAsync()
        {

            var users = await _userRepository.GetAllAsync();
            if (users == null || !users.Any()) return ResponseDto<List<UserDto>>.Fail("Hiçbir kullanıcı bulunamadı.");

            var resultList = _mapper.Map<List<UserDto>>(users);

            return ResponseDto<List<UserDto>>.Succes(resultList);  
        }


        public async Task<ResponseDto<UserDto>> UpdateUserAsync(int id, UserUpdateDto updatedUserDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null) return ResponseDto<UserDto>.Fail("Kullanıcı bulunamadı.");

                user.Email = updatedUserDto.Email;
                user.FullName = updatedUserDto.FullName;


                var updatedUser = _userRepository.UpdateAsync(user);

                var resultDto = _mapper.Map<UserDto>(updatedUser);

                return ResponseDto<UserDto>.Succes(resultDto);
            }

            catch (Exception ex)
            {
                return ResponseDto<UserDto>.Fail(ex.Message);
            }

        }





        public async Task<ResponseDto<bool>> DeleteUserAsync(int id)
        {
            var result = await  _userRepository.DeleteAsync(id);

            if (!result) return ResponseDto<bool>.Fail("Kullanıcı bulunamadı veya silinemedi");

            return ResponseDto<bool>.Succes(true);
        }





        #region Hash
        private (byte[] Hash, byte[] Salt) CreatePasswordHash(string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var passwordSalt = hmac.Key;
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return (passwordHash, passwordSalt);
            }
        }



        private bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }


        #endregion







    }

}
