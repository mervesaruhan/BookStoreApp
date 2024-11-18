using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Model.DTO
{
    public class UserLoginDto
    {
        [Required(ErrorMessage ="Giriş yapmak için mailinizi giriniz.")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage ="Şifre alanı boş bırakılamaz!")]
        public string Password { get; set; } = null!;
    }
}
