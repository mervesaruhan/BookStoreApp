using BookStoreApp.Model.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Model.DTO.UserDtos
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "İsim - Soyisim alanı boş geçilemez!")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Şifre alanı boş geçilemez!")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Şifre en az 4 en fazla 10 karakterden oluşturulmalıdır!")]
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;

        [RoleValidation]
        public UserRole Role { get; set; }

    }
}
